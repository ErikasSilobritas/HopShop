namespace HopShop.WEBApi.Exceptions
{
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException() : base ("Item was not found") 
        {
        }

        public ItemNotFoundException(string message) : base (message)
        {
            
        }
    }
}
