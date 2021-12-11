namespace TheCarHub
{
    public interface ICarRepository
    {
        Task<IReadOnlyList<ICar>> Retrieve();
        Task<ICar?> Retrieve(Guid id);
        Task Create(ICar car, IFormFile? picture);
        Task Update(ICar car, IFormFile? picture);
        Task Delete(Guid id);
    }
}