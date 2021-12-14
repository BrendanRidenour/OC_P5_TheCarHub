namespace TheCarHub.Models.Admin
{
    public class InventoryViewModel
    {
        public IReadOnlyList<ICar> Cars { get; }

        public InventoryViewModel(IReadOnlyList<ICar> inventory)
        {
            this.Cars = (inventory ?? new List<ICar>())
                .OrderByDescending(car => car.PurchaseDate)
                .ThenByDescending(car => car.LotDate)
                .ToList()
                .AsReadOnly();
        }
    }
}