namespace TheCarHub
{
    public class DealershipService : IDealershipService
    {
        protected readonly ICarRepository CarRepository;

        public DealershipService(ICarRepository carRepository)
        {
            this.CarRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
        }

        public virtual Task<IReadOnlyList<ICar>> GetInventory() =>
            this.CarRepository.Retrieve();
    }
}