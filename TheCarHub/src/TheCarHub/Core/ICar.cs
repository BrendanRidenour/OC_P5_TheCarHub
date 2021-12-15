namespace TheCarHub
{
    public interface ICar
    {
        Guid Id { get; }

        string? Vin { get; }

        int Year { get; }
        string Make { get; }
        string Model { get; }
        string Trim { get; }

        DateTimeOffset PurchaseDate { get; }
        double PurchasePrice { get; }

        string? Repairs { get; }
        double RepairCost { get; }

        DateTimeOffset? LotDate { get; }
        DateTimeOffset? SaleDate { get; }

        double Profit { get; }

        double SellingPrice => this.PurchasePrice + this.RepairCost + this.Profit;

        string? PictureUri { get; }
    }
}