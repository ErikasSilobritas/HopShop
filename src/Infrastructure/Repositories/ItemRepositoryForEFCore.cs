using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ItemRepositoryForEFCore : IItemRepository
    {
        private readonly DataContext _dataContext;

        public ItemRepositoryForEFCore(DataContext dataContext)
        {
            _dataContext = dataContext;
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

        public async Task<Item> EditItem(Item item)
        {
            var existingItem = _dataContext.items.FirstOrDefault(t => t.Id == item.Id && t.IsDeleted == false);

            existingItem.Name = item.Name ?? existingItem.Name;
            existingItem.Price = item.Price ?? existingItem.Price;
            existingItem.Quantity = item.Quantity ?? existingItem.Quantity;
            existingItem.ShopId = item.ShopId ?? existingItem.ShopId;
            await _dataContext.SaveChangesAsync();
            return existingItem;
        }

        public async Task AddItem(Item item)
        {
            _dataContext.items.Add(item);
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteItem(int id)
        {
            var existingItem = _dataContext.items.FirstOrDefault(t => t.Id == id && t.IsDeleted == false);
            existingItem.IsDeleted = true;
            await _dataContext.SaveChangesAsync();
        }

        public async Task AppendPurchaseHistory(PurchaseHistory history)
        {
            _dataContext.purchases.Add(history);
            await _dataContext.SaveChangesAsync();
        }
    }
}
