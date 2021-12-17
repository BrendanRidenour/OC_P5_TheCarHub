using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Text.Json;

namespace TheCarHub
{
    public class AzureCarRepository : ICarRepository
    {
        private readonly string _connectionString;

        public AzureCarRepository(string connectionString)
        {
            this._connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<IReadOnlyList<ICar>> Retrieve()
        {
            var client = await this.CreateTableClient().ConfigureAwait(false);

            var query = client.QueryAsync<CarTableEntity>(
                ent => ent.PartitionKey == CarTableEntity.CreatePartitionKey());

            var cars = new List<ICar>();
            await foreach (var page in query.AsPages())
            {
                foreach (var entity in page.Values)
                {
                    if (entity is not null)
                        cars.Add(entity.ToCar());
                }
            }

            return cars;
        }

        public async Task<ICar?> Retrieve(Guid id)
        {
            var (partitionKey, rowKey) = CarTableEntity.CreateKeys(id);

            var client = await this.CreateTableClient().ConfigureAwait(false);

            var entity = await client.GetEntityAsync<CarTableEntity>(
                partitionKey, rowKey).ConfigureAwait(false);

            return entity?.Value.ToCar();
        }

        public async Task Create(ICar car)
        {
            if (car is null)
                throw new ArgumentNullException(nameof(car));

            var client = await this.CreateTableClient().ConfigureAwait(false);

            var entity = new CarTableEntity(car);

            await client.AddEntityAsync(entity).ConfigureAwait(false);
        }

        public async Task Update(ICar car)
        {
            if (car is null)
                throw new ArgumentNullException(nameof(car));

            var existingCar = await this.Retrieve(car.Id).ConfigureAwait(false);

            if (existingCar is null)
                throw new InvalidOperationException($"No car exists by this id: '{car.Id}'");

            var updatePoco = new CarPoco(car, existingCar.PictureUris);

            var tables = await this.CreateTableClient().ConfigureAwait(false);

            await tables.UpdateEntityAsync(new CarTableEntity(updatePoco),
                new ETag("*"), TableUpdateMode.Replace).ConfigureAwait(false);
        }

        public async Task AddPicture(Guid carId, IFormFile picture)
        {
            if (picture is null)
                throw new ArgumentNullException(nameof(picture));

            var car = await this.Retrieve(carId).ConfigureAwait(false);

            if (car is null)
                throw new InvalidOperationException($"No car exists by this id: '{carId}'");

            var blob = await this.CreateBlobClient(carId).ConfigureAwait(false);

            using var stream = picture.OpenReadStream();

            await blob.UploadAsync(
                stream,
                new BlobHttpHeaders()
                {
                    ContentType = picture.ContentType,
                }).ConfigureAwait(false);

            var pictureUris = new List<string>(car.PictureUris)
            {
                blob.Uri.ToString()
            };

            var updatePoco = new CarPoco(car, pictureUris);

            var tables = await this.CreateTableClient().ConfigureAwait(false);

            await tables.UpdateEntityAsync(new CarTableEntity(updatePoco),
                new ETag("*"), TableUpdateMode.Replace).ConfigureAwait(false);
        }

        public async Task DeletePicture(Guid carId, string pictureUri)
        {
            if (pictureUri is null)
                throw new ArgumentNullException(nameof(pictureUri));

            var car = await this.Retrieve(carId).ConfigureAwait(false);

            if (car is null)
                throw new InvalidOperationException($"No car exists by this id: '{carId}'");

            var container = await this.CreateBlobContainerClient(carId)
                .ConfigureAwait(false);

            var blobName = ParsePictureBlobName(pictureUri);

            await container.DeleteBlobIfExistsAsync(blobName)
                .ConfigureAwait(false);

            var pictureUris = car.PictureUris.ToList();

            pictureUris.RemoveAll(uri => uri.Equals(pictureUri, StringComparison.OrdinalIgnoreCase));

            var updatePoco = new CarPoco(car, pictureUris);

            var tables = await this.CreateTableClient().ConfigureAwait(false);

            await tables.UpdateEntityAsync(new CarTableEntity(updatePoco),
                new ETag("*"), TableUpdateMode.Replace).ConfigureAwait(false);
        }

        public async Task Delete(Guid id)
        {
            var (partitionKey, rowKey) = CarTableEntity.CreateKeys(id);

            var tables = await this.CreateTableClient().ConfigureAwait(false);

            await tables.DeleteEntityAsync(partitionKey, rowKey)
                .ConfigureAwait(false);

            var container = await this.CreateBlobContainerClient(id)
                .ConfigureAwait(false);

            await container.DeleteIfExistsAsync().ConfigureAwait(false);
        }

        private static bool tableCreated = false;
        private async Task<TableClient> CreateTableClient()
        {
            var client = new TableClient(this._connectionString, "thecarhub");

            if (!tableCreated)
            {
                await client.CreateIfNotExistsAsync().ConfigureAwait(false);

                tableCreated = true;
            }

            return client;
        }

        private async Task<BlobContainerClient> CreateBlobContainerClient(
            Guid carId)
        {
            var container = new BlobContainerClient(this._connectionString,
                $"thecarhub-car-{carId.ToString("N").ToLowerInvariant()}");

            await container.CreateIfNotExistsAsync().ConfigureAwait(false);
            await container.SetAccessPolicyAsync(PublicAccessType.Blob)
                .ConfigureAwait(false);

            return container;
        }

        private async Task<BlobClient> CreateBlobClient(Guid carId)
        {
            var container = await this.CreateBlobContainerClient(carId)
                .ConfigureAwait(false);

            return container.GetBlobClient($"car-pic-{Guid.NewGuid()}");
        }

        private static string ParsePictureBlobName(string pictureUri) =>
            pictureUri[pictureUri.IndexOf("car-pic-")..];

        private class CarTableEntity : CarPoco<string>, ITableEntity
        {
            public string PartitionKey { get; set; } = CreatePartitionKey();
            public string RowKey { get; set; } = string.Empty;
            public DateTimeOffset? Timestamp { get; set; }
            public ETag ETag { get; set; } = new ETag("*");

            public CarTableEntity() : base() { }

            public CarTableEntity(ICar car)
                : base(car, JsonSerializer.Serialize(car.PictureUris))
            {
                var (partitionKey, rowKey) = CreateKeys(car.Id);

                this.PartitionKey = partitionKey;
                this.RowKey = rowKey;
            }

            public ICar ToCar()
            {
                var list = !string.IsNullOrWhiteSpace(this.PictureUris)
                    ? JsonSerializer.Deserialize<List<string>>(this.PictureUris)!
                    : new List<string>();

                return new CarPoco(this, list);
            }

            public static string CreatePartitionKey() => "CarInventory";
            public static (string partitionKey, string rowKey) CreateKeys(Guid carId) =>
                (CreatePartitionKey(), carId.ToString().ToUpperInvariant());
        }
    }
}