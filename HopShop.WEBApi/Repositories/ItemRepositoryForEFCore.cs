using HopShop.WEBApi.Contexts;
using HopShop.WEBApi.Entities;
using HopShop.WEBApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HopShop.WEBApi.Repositories
{
    public class ItemRepositoryForEFCore : IItemRepository
    {
        private readonly DataContext _dataContext;

        public ItemRepositoryForEFCore(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddItem(string itemName, decimal itemPrice, int itemQuantity)
        {
            _dataContext.items.Add(new Item
            {
                Name = itemName,
                Price = itemPrice,
                Quantity = itemQuantity
            });
            await _dataContext.SaveChangesAsync();
            
        }

        public async Task AddItem(Item item)
        {
            _dataContext.items.Add(item);
            await _dataContext.SaveChangesAsync();
        }

        public decimal? BuyItem(string itemName)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteItem(int id)
        {
            var existingItem = _dataContext.items.FirstOrDefault(t => t.Id == id && t.IsDeleted == false);
            existingItem.IsDeleted = true;
            await _dataContext.SaveChangesAsync();
        }

        public async Task<Item> EditItem(Item item)
        {
            var existingItem = _dataContext.items.FirstOrDefault(t => t.Id == item.Id && t.IsDeleted == false);

            existingItem.Name = item.Name;
            existingItem.Price = item.Price;
            existingItem.Quantity = item.Quantity;
            await _dataContext.SaveChangesAsync();

            return existingItem;
        }

        public async Task<IEnumerable<Item>> GetAllItems()
        {
            return await _dataContext.items.Where(i => !i.IsDeleted).ToListAsync();
        }

        public async Task<Item?> GetItemById(int id)
        {
            return await _dataContext.items.FirstOrDefaultAsync(i => i.Id == id && i.IsDeleted == false);
        }

        public async Task<Item?> GetItemByName(string itemName) 
        {
            return await _dataContext.items.FirstOrDefaultAsync(i => i.Name == itemName && i.IsDeleted == false);
        }
    }
}
