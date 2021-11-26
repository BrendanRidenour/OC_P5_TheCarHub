namespace TheCarHub
{
    public interface IAdminService : IDealershipService
    {
        Task<Result> Login(string username, string password);
        Task Logout();
        Task<ICar?> GetCar(Guid id);
        Task CreateCar(ICar car);
        Task UpdateCar(ICar car);
        Task DeleteCar(Guid id);
    }
}