namespace HopShop.WEBApi.DTOs.Requests
{
    public class CreateItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
