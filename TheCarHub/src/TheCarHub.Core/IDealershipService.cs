namespace TheCarHub
{
    public interface IDealershipService
    {
        public Task<IReadOnlyList<ICar>> GetInventory();
    }
}