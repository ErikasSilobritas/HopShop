namespace Domain.DTOs.Requests
{
    public class BuyItem
    {
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int ShopId { get; set; }
    }
}
