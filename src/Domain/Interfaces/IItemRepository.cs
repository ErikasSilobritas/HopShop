using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IItemRepository
    {
        public Task <IEnumerable<Item>> GetAllItems();
        public Task<Item?> GetItemById(int id);
        public Task <Item?> GetItemByName(string itemName);
        public Task<Item> EditItem(Item item);
        public Task AddItem(Item item);
        public Task DeleteItem(int id);
    }
}


