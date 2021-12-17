using TheCarHub.Core.Internal;

namespace TheCarHub
{
    public class CarPoco<TPictureUris> : ICarBase<TPictureUris>
    {
        public CarPoco() { }

        public CarPoco(ICarBase<TPictureUris> car)
            : this(car, car.PictureUris)
        { }

        public CarPoco(ICarBase car, TPictureUris pictureUris)
            : this(car)
        {
            this.PictureUris = pictureUris;
        }

        private CarPoco(ICarBase car)
        {
            if (car is null)
            {
                throw new ArgumentNullException(nameof(car));
            }

            this.Id = car.Id;
            this.Vin = car.Vin;
            this.Year = car.Year;
            this.Make = car.Make;
            this.Model = car.Model;
            this.Trim = car.Trim;
            this.PurchaseDate = car.PurchaseDate;
            this.PurchasePrice = car.PurchasePrice;
            this.Repairs = car.Repairs;
            this.RepairCost = car.RepairCost;
            this.LotDate = car.LotDate;
            this.SaleDate = car.SaleDate;
            this.Profit = car.Profit;
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        public string? Vin { get; set; }

        public int Year { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Trim { get; set; } = string.Empty;

        public DateTimeOffset PurchaseDate { get; set; } = GetToday();
        public double PurchasePrice { get; set; } = 0;

        public string? Repairs { get; set; }
        public double RepairCost { get; set; } = 0;

        public DateTimeOffset? LotDate { get; set; }
        public DateTimeOffset? SaleDate { get; set; }

        public double Profit { get; set; } = 500;

        public TPictureUris PictureUris { get; set; } = default!;

        public override string ToString() => $"{this.Year} {this.Make} {this.Model}";

        private static DateTimeOffset GetToday()
        {
            var today = DateTimeOffset.UtcNow;

            return new DateTimeOffset(year: today.Year, month: today.Month,
                day: today.Day, hour: 0, minute: 0, second: 0, offset: TimeSpan.Zero);
        }
    }
}