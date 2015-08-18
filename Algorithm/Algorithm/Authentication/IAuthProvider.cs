namespace Algorithm.Authentication
{
    public interface IAuthProvider
    {
        Client Authenticate(string username, string password);
    }
}