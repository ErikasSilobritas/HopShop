using Domain.DTOs.Requests;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services
{
    public class ShopService
    {
        private readonly IShopRepository _shopRepository;
        public ShopService(IShopRepository shopRepository)
        {
            _shopRepository = shopRepository;
        }
        public async Task<List<GetShop>> GetAllShops()
        {
            var allShops = await _shopRepository.GetAllShops();

            return allShops.Select(i => new GetShop
            {
                Id = i.Id,
                Name = i.Name,
                Address = i.Address
            }).ToList();
        }

        public async Task<GetShop> GetShopById(int id)
        {
            var shop = await _shopRepository.GetShopById(id);

            if (shop is null)
            {
                throw new ShopNotFoundException();
            }

            var shopRequest = new GetShop
            {
                Id = shop.Id,
                Name = shop.Name,
                Address = shop.Address
            };

            return shopRequest;
        }

        public async Task<EditShop> EditShop(EditShop editShop)
        {
            if (await _shopRepository.GetShopById(editShop.Id) is null)
            {
                throw new ShopNotFoundException();
            }

            var shop = new Shop
            {
                Id = editShop.Id,
                Name = editShop.Name,
                Address = editShop.Address
            };

            var returnedShop = await _shopRepository.EditShop(shop);

            var editedShop = new EditShop
            {
                Id = returnedShop.Id,
                Name = returnedShop.Name,
                Address = returnedShop.Address
            };

            return editedShop;
        }

        public async Task AddShop(CreateShop createShop)
        {
            if (await _shopRepository.GetShopByName(createShop.Name) != null)
            {
                throw new ShopAlreadyExistsException();
            }
            Shop shop = new Shop
            {
                Name = createShop.Name,
                Address = createShop.Address
            };

            await _shopRepository.AddShop(shop);
        }

        public async Task DeleteShop(int id)
        {
            if (await _shopRepository.GetShopById(id) is null)
            {
                throw new ShopNotFoundException();
            }

            await _shopRepository.DeleteShop(id);
        }
    }
}
