namespace TheCarHub
{
    public interface ICarRepository
    {
        Task<IReadOnlyList<ICar>> Retrieve();
        Task<ICar?> Retrieve(Guid id);
        Task Create(ICar car);
        Task Update(ICar car);
        //Task AddPicture(Guid carId, IFormFile picture);
        //Task DeletePicture(Guid carId, string pictureUri);
        Task Delete(Guid id);
    }
}