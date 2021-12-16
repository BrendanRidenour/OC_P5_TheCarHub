namespace TheCarHub.Models.Admin
{
    public class ManageCarsViewModel
    {
        public IReadOnlyList<ICar> Cars { get; }

        public ManageCarsViewModel(IReadOnlyList<ICar> inventory)
        {
            this.Cars = (inventory ?? new List<ICar>())
                .OrderByDescending(car => car.PurchaseDate)
                .ThenByDescending(car => car.LotDate)
                .ToList()
                .AsReadOnly();
        }
    }
}