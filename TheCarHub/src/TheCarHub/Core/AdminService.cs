namespace TheCarHub
{
    public class AdminService : DealershipService, IAdminService
    {
        protected readonly IAuthenticationService AuthenticationService;

        public AdminService(ICarRepository carRepository,
            IAuthenticationService authenticationService)
            : base(carRepository)
        {
            this.AuthenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        public Task<Result> Login(string username, string password) =>
            this.AuthenticationService.Login(username, password);

        public Task Logout() =>
            this.AuthenticationService.Logout();

        public Task<ICar?> GetCar(Guid id) =>
            this.CarRepository.Retrieve(id);

        public Task CreateCar(ICar car) =>
            this.CarRepository.Create(car);

        public Task UpdateCar(ICar car) =>
            this.CarRepository.Update(car);

        public Task UpdateCar_AddPicture(Guid carId, IFormFile picture) =>
            this.CarRepository.AddPicture(carId, picture);

        public Task UpdateCar_DeletePicture(Guid carId, string pictureUri) =>
            this.CarRepository.DeletePicture(carId, pictureUri);

        public Task DeleteCar(Guid id) =>
            this.CarRepository.Delete(id);
    }
}