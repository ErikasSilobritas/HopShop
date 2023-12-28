namespace Domain.Exceptions
{
    public class ShopNotFoundException : Exception
    {
        public ShopNotFoundException() : base("Shop was not found")
        {        
        }
    }
}
