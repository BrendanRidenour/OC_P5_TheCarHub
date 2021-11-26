namespace TheCarHub
{
    public class InMemoryCarRepository : ICarRepository
    {
        private static readonly List<ICar> _cars = new List<ICar>()
        {
            new CarPoco()
            {
                Vin = "vin-111",
                Year = 1991,
                Make = "Mazda",
                Model = "Miata",
                Trim = "LE",
                PurchaseDate = BuildDate(2019, 1, 7),
                PurchasePrice = 1800,
                Repairs = "Full restoration",
                RepairCost = 7600,
                LotDate = BuildDate(2019, 4, 7),
                SaleDate = BuildDate(2019, 4, 8),
            },
            new CarPoco()
            {
                Vin = "vin-222",
                Year = 2007,
                Make = "Jeep",
                Model = "Liberty",
                Trim = "Sport",
                PurchaseDate = BuildDate(2019, 4, 4),
                PurchasePrice = 4500,
                Repairs = "Front wheel bearings",
                RepairCost = 350,
                LotDate = BuildDate(2019, 4, 7),
                SaleDate = null,
            },
            new CarPoco()
            {
                Vin = null,
                Year = 2017,
                Make = "Ford",
                Model = "Explorer",
                Trim = "XLT",
                PurchaseDate = BuildDate(2019, 4, 5),
                PurchasePrice = 24350,
                Repairs = "Tires, brakes",
                RepairCost = 1100,
                LotDate = BuildDate(2019, 4, 9),
                SaleDate = null,
            },
            new CarPoco()
            {
                Vin = null,
                Year = 2008,
                Make = "Honda",
                Model = "Civic",
                Trim = "LX",
                PurchaseDate = BuildDate(2019, 4, 6),
                PurchasePrice = 4000,
                Repairs = "AC, brakes",
                RepairCost = 475,
                LotDate = null,
                SaleDate = null,
            },
        };

        private static DateTimeOffset BuildDate(int year, int month, int day) =>
            new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero);

        public Task<IReadOnlyList<ICar>> Retrieve() =>
            Task.FromResult<IReadOnlyList<ICar>>(_cars);

        public Task<ICar?> Retrieve(Guid id) =>
            Task.FromResult<ICar?>(_cars.SingleOrDefault(c => c.Id == id));

        public Task Create(ICar car)
        {
            _cars.Add(car);

            return Task.CompletedTask;
        }

        public Task Update(ICar car)
        {
            var index = _cars.FindIndex(c => c.Id == car.Id);

            if (index == -1)
                throw new InvalidOperationException("Update failed because a car by that id does not exist.");

            _cars[index] = car;

            return Task.CompletedTask;
        }

        public Task Delete(Guid id)
        {
            var car = _cars.SingleOrDefault(car => car.Id == id);

            if (car != null)
                _cars.Remove(car);

            return Task.CompletedTask;
        }
    }
}