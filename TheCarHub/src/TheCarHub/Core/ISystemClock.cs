namespace TheCarHub
{
    public interface ISystemClock
    {
        DateTimeOffset UtcNow { get; }
    }
}