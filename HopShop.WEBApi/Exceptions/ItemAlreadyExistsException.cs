namespace HopShop.WEBApi.Exceptions
{
    public class ItemAlreadyExistsException : Exception
    {
        public ItemAlreadyExistsException() : base("An item by that name already exists")
        { 
        }
    }
}
