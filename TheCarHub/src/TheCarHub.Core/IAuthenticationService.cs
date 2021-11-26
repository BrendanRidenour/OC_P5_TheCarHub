namespace TheCarHub
{
    public interface IAuthenticationService
    {
        Task<Result> Login(string username, string password);
        Task Logout();
    }
}