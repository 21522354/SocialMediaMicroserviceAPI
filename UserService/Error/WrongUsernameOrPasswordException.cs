
namespace UserService.Error
{
    public class WrongUsernameOrPasswordException : Exception
    {
        public WrongUsernameOrPasswordException() : base("Wrong username or password")
        {
            
        }
        public WrongUsernameOrPasswordException(string message) : base(message) 
        {
            
        }
    }
}
