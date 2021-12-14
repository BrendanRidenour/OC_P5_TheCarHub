namespace TheCarHub.Models.Main
{
    public class InventoryViewModel
    {
        public IReadOnlyList<ICar> Cars { get; }

        public InventoryViewModel(IReadOnlyList<ICar> inventory, ISystemClock clock)
        {
            this.Cars = (inventory ?? new List<ICar>())
                .OrderByDescending(c => c.SaleDate == null)
                .ThenBy(c => c.LotDate == null || c.LotDate > clock.UtcNow)
                .ToList()
                .AsReadOnly();
        }
    }
}