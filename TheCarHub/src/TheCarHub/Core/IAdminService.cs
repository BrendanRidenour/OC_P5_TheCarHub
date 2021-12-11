using Microsoft.AspNetCore.Http;

namespace TheCarHub
{
    public interface IAdminService : IDealershipService
    {
        Task<Result> Login(string username, string password);
        Task Logout();
        Task<ICar?> GetCar(Guid id);
        Task CreateCar(ICar car, IFormFile? picture);
        Task UpdateCar(ICar car, IFormFile? picture);
        Task DeleteCar(Guid id);
    }
}