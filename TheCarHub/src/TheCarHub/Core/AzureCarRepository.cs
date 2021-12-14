﻿using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

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
                        cars.Add(entity);
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

            return entity?.Value;
        }

        public async Task Create(ICar car, IFormFile? picture)
        {
            if (car is null)
            {
                throw new ArgumentNullException(nameof(car));
            }

            var client = await this.CreateTableClient().ConfigureAwait(false);

            var entity = new CarTableEntity(car);

            if (picture is not null)
            {
                await this.UploadPicture(entity, picture).ConfigureAwait(false);
            }

            await client.AddEntityAsync(entity).ConfigureAwait(false);
        }

        public async Task Update(ICar car, IFormFile? picture)
        {
            if (car is null)
            {
                throw new ArgumentNullException(nameof(car));
            }

            var (partitionKey, rowKey) = CarTableEntity.CreateKeys(car.Id);
            
            var tables = await this.CreateTableClient().ConfigureAwait(false);

            var existingEntity = await tables.GetEntityAsync<CarTableEntity>(
                partitionKey, rowKey).ConfigureAwait(false);

            var updateEntity = new CarTableEntity(car)
            {
                ETag = existingEntity.Value.ETag,
            };

            if (picture is null)
            {
                updateEntity.PictureUri = existingEntity.Value.PictureUri;
            }
            else
            {
                await this.UploadPicture(updateEntity, picture).ConfigureAwait(false);
            }

            await tables.UpdateEntityAsync(updateEntity, existingEntity.Value.ETag,
                TableUpdateMode.Replace).ConfigureAwait(false);
        }

        public async Task Delete(Guid id)
        {
            var (partitionKey, rowKey) = CarTableEntity.CreateKeys(id);

            var tables = await this.CreateTableClient().ConfigureAwait(false);

            await tables.DeleteEntityAsync(partitionKey, rowKey)
                .ConfigureAwait(false);

            var blobs = await this.CreateBlobClient(id).ConfigureAwait(false);

            await blobs.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots)
                .ConfigureAwait(false);
        }

        private static bool blobContainerCreated = false;
        private async Task<BlobClient> CreateBlobClient(Guid carId)
        {
            var container = new BlobContainerClient(this._connectionString,
                "thecarhub");

            if (!blobContainerCreated)
            {
                await container.CreateIfNotExistsAsync().ConfigureAwait(false);
                await container.SetAccessPolicyAsync(PublicAccessType.Blob)
                    .ConfigureAwait(false);

                blobContainerCreated = true;
            }

            return container.GetBlobClient(
                $"car-pic-{carId.ToString().ToUpperInvariant()}");
        }
        
        private async Task UploadPicture(CarTableEntity entity, IFormFile picture)
        {
            var blob = await this.CreateBlobClient(entity.Id).ConfigureAwait(false);

            using var stream = picture.OpenReadStream();

            await blob.UploadAsync(
                stream,
                new BlobHttpHeaders()
                {
                    ContentType = picture.ContentType,
                })
                .ConfigureAwait(false);

            entity.PictureUri = blob.Uri.ToString();
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

        private class CarTableEntity : CarPoco, ITableEntity
        {
            public string PartitionKey { get; set; } = CreatePartitionKey();
            public string RowKey { get; set; } = String.Empty;
            public DateTimeOffset? Timestamp { get; set; }
            public ETag ETag { get; set; } = new ETag("*");

            public CarTableEntity() : base() { }

            public CarTableEntity(ICar car)
                : base(car)
            {
                var (partitionKey, rowKey) = CreateKeys(car.Id);

                this.PartitionKey = partitionKey;
                this.RowKey = rowKey;
            }

            public static string CreatePartitionKey() => "CarInventory";
            public static (string partitionKey, string rowKey) CreateKeys(Guid carId) =>
                (CreatePartitionKey(), carId.ToString().ToUpperInvariant());
        }
    }
}