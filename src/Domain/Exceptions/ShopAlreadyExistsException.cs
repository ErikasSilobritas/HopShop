namespace Domain.Exceptions
{
    public class ShopAlreadyExistsException : Exception
    {
        public ShopAlreadyExistsException() : base("An item by that name already exists")
        {
        }
    }
}
