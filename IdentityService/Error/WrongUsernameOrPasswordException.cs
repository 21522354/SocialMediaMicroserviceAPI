
namespace IdentityService.Error
{
    public class WrongIdentitynameOrPasswordException : Exception
    {
        public WrongIdentitynameOrPasswordException() : base("Wrong Identityname or password")
        {
            
        }
        public WrongIdentitynameOrPasswordException(string message) : base(message) 
        {
            
        }
    }
}
