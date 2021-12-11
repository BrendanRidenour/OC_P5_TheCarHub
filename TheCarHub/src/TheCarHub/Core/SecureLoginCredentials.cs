namespace TheCarHub
{
    public class SecureLoginCredentials
    {
        public string Username { get; }
        public string Password { get; }

        public SecureLoginCredentials(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
    }
}