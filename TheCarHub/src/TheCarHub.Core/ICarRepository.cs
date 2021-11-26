namespace TheCarHub
{
    public interface ICarRepository
    {
        Task<IReadOnlyList<ICar>> Retrieve();
        Task<ICar?> Retrieve(Guid id);
        Task Create(ICar car);
        Task Update(ICar car);
        Task Delete(Guid id);
    }
}