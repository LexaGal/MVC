namespace Algorithm.Authentication
{
    public interface IAuthProvider
    {
        User Authenticate(string username, string password);
        User Register(string username, string password);
    }
}