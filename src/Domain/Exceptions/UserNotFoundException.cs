namespace Domain.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("User by that id does not exist")
        {
        }
    }
}



