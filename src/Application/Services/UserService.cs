using Domain.DTOs.Requests;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services
{
    public class UserService
    {
        private readonly IJsonPlaceholderClient _client;
        private readonly IShopRepository _shopRepository;
        private readonly IItemRepository _itemRepository;

        public UserService(IJsonPlaceholderClient client, IShopRepository shopRepository, IItemRepository itemRepository)
        {
            _client = client;
            _shopRepository = shopRepository;
            _itemRepository = itemRepository;
        }

        public async Task<List<GetUser>> GetUsers()
        {
            
            return await _client.GetUsers();
        }

        public async Task<GetUser> GetUserById(int id)
        {
            var user = await _client.GetUserById(id);
            if (!user.IsSuccessful)
            {
                throw new UserNotFoundException();
            }    
         
            return user.Data;
        }

        public async Task<CreateUser> CreateUser (CreateUser user)
        {
            CreateUser createdUser = await _client.CreateUser(user);
            return createdUser; 
        }

        public async Task<BuyItem> Buy(BuyItem itemToBuy)
        {
            if (await _itemRepository.GetItemById(itemToBuy.ItemId) is null)
            {
                throw new ItemNotFoundException();
            }
            if (await _shopRepository.GetShopById(itemToBuy.ShopId) is null)
            {
                throw new ShopNotFoundException();
            }
            //PATIKRINTI AR TAS SHOPAS TURI BUTENT TA ITEM'a

            if (itemToBuy.Quantity < 0)
            {
                throw new IncorrectQuantityException();
            }

            int quantityToBuy = itemToBuy.Quantity;
            int? quantityLeftAfterPurchase = (await _itemRepository.GetItemById(itemToBuy.ItemId)).Quantity - quantityToBuy;

            if(quantityLeftAfterPurchase < 0)
            {
                throw new InsufficientItemQuantityException();
            }

            Item item = new Item
            {
                Id = itemToBuy.ItemId,
                Quantity = quantityLeftAfterPurchase,
                ShopId = itemToBuy.ShopId
            };

            await _itemRepository.EditItem(item);

            PurchaseHistory history = new PurchaseHistory
            {
                UserId = itemToBuy.UserId,
                ItemId= itemToBuy.ItemId,
                Quantity = itemToBuy.Quantity,
                ShopId = itemToBuy.ShopId
            };
            await _itemRepository.AppendPurchaseHistory(history);
            return itemToBuy;
        }
    }
}
