using HopShop.WEBApi.DTOs.Requests;
using HopShop.WEBApi.Entities;

namespace HopShop.WEBApi.Interfaces
{
    public interface IItemRepository
    {
        public Task <Item?> GetItemById(int id);

        public Task <IEnumerable<Item>> GetAllItems();

        public Task <Item?> GetItemByName(string itemName);

        public Task AddItem(string itemName, decimal itemPrice, int itemQuantity);

        public Task AddItem(Item item);

        public decimal? BuyItem(string itemName);

        public Task DeleteItem(int id);

        public Task<Item> EditItem(Item item);
    }
}


