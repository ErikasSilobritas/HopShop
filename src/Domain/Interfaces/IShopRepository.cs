using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IShopRepository
    {
        Task<IEnumerable<Shop>> GetAllShops();
        Task<Shop?> GetShopById(int? id);
        Task<Shop?> GetShopByName(string shopName);
        Task<Shop> EditShop(Shop shop);
        Task AddShop(Shop shop);
        Task DeleteShop(int id);



    }
}
