namespace Domain.DTOs.Requests
{
    public class GetItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int? ShopId { get; set; }
    }
}
