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

        public DateTimeOffset PurchaseDate { get; set; }
        public double PurchasePrice { get; set; } = 0;

        public string? Repairs { get; set; }
        public double RepairCost { get; set; } = 0;

        public DateTimeOffset? LotDate { get; set; }
        public DateTimeOffset? SaleDate { get; set; }
    }
}
