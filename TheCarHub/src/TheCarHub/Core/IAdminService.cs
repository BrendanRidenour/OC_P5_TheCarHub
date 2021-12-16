namespace TheCarHub
{
    public interface IAdminService : IDealershipService
    {
        Task<Result> Login(string username, string password);
        Task Logout();
        Task<ICar?> GetCar(Guid id);
        Task CreateCar(ICar car);
        Task UpdateCar(ICar car);
        //Task UpdateCar_AddPicture(Guid carId, IFormFile picture);
        //Task UpdateCar_DeletePicture(Guid carId, string pictureUri);
        Task DeleteCar(Guid id);
    }
}