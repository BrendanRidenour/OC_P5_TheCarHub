﻿namespace TheCarHub.Models.Main
{
    public class InventoryViewModel
    {
        public IReadOnlyList<ICar> Cars { get; }

        public InventoryViewModel(IReadOnlyList<ICar> inventory)
        {
            this.Cars = (inventory ?? new List<ICar>())
                .Where(car => !car.SaleDate.HasValue)
                .OrderByDescending(car => car.LotDate)
                .ToList()
                .AsReadOnly();
        }
    }
}