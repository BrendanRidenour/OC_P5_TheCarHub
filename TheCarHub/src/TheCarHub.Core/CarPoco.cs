namespace TheCarHub
{
    public class CarPoco : ICar
    {
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

        private static DateTimeOffset GetToday()
        {
            var today = DateTimeOffset.UtcNow;

            return new DateTimeOffset(year: today.Year, month: today.Month,
                day: today.Day, hour: 0, minute: 0, second: 0, offset: TimeSpan.Zero);
        }
    }
}