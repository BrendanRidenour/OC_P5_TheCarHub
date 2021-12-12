namespace TheCarHub.Models.Admin
{
    public class InventoryViewModel
    {
        public IReadOnlyList<ICar> Cars { get; }

        public InventoryViewModel(IReadOnlyList<ICar> inventory)
        {
            this.Cars = (inventory ?? new List<ICar>())
                .OrderBy(car => car.PurchaseDate)
                .ThenBy(car => car.LotDate)
                .ToList()
                .AsReadOnly();
        }
    }
}