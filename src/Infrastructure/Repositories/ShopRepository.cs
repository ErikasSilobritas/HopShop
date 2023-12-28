using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ShopRepository : IShopRepository
    {
        private readonly DataContext _dataContext;

        public ShopRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<Shop>> GetAllShops()
        {
            return await _dataContext.shops.Where(i => !i.IsDeleted).ToListAsync();
        }

        public async Task<Shop?>GetShopById(int? id)
        {
            return await _dataContext.shops.FirstOrDefaultAsync(i => i.Id == id && i.IsDeleted == false);
        }

        public async Task<Shop?> GetShopByName(string shopName)
        {
            return await _dataContext.shops.FirstOrDefaultAsync(i => i.Name == shopName && i.IsDeleted == false);
        }

        public async Task<Shop> EditShop(Shop shop)
        {
            var existingShop = _dataContext.shops.FirstOrDefault(t => t.Id == shop.Id && t.IsDeleted == false);

            existingShop.Name = shop.Name;
            existingShop.Address = shop.Address;
            await _dataContext.SaveChangesAsync();

            return existingShop;
        }

        public async Task AddShop(Shop shop)
        {
            _dataContext.shops.Add(shop);
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteShop(int id)
        {
            var existingShop = _dataContext.shops.FirstOrDefault(t => t.Id == id && t.IsDeleted == false);
            existingShop.IsDeleted = true;
            await _dataContext.SaveChangesAsync();
        }
    }
}
