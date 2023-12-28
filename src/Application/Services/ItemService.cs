using Domain.DTOs.Requests;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services
{
    public class ItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IShopRepository _shopRepository;

        public ItemService(IItemRepository itemRepository, IShopRepository shopRepository)
        {
            _itemRepository = itemRepository;
            _shopRepository = shopRepository;
        }

        public async Task<List<GetItem>> GetAllItems()
        {

            var allItems = await _itemRepository.GetAllItems();

            return allItems.Select(i => new GetItem
            {
                Id = i.Id,
                Name = i.Name,
                Price = i.Price,
                Quantity = i.Quantity
            }).ToList();
        }

        public async Task<GetItem> GetItemById(int id)
        {
            var item = await _itemRepository.GetItemById(id);
            
            if (item is null)
            {
                throw new ItemNotFoundException();
            }

            var itemRequest = new GetItem
            { 
                Id = item.Id, 
                Name = item.Name, 
                Price= item.Price, 
                Quantity= item.Quantity 
            };

            return itemRequest;
        }

        public async Task <EditItem> EditItem(EditItem editItem)
        {
            if (await _itemRepository.GetItemById(editItem.Id) is null)
            {
                throw new ItemNotFoundException();
            }

            var item = new Item
            {
                Id = editItem.Id,
                Name = editItem.Name,
                Price = editItem.Price,
                Quantity = editItem.Quantity
            };

            var returnedItem = await _itemRepository.EditItem(item);    
                
            var editedItem = new EditItem 
            {
                Id = returnedItem.Id,
                Name = returnedItem.Name,
                Price = returnedItem.Price,
                Quantity = returnedItem.Quantity
            };

            return editedItem;
        }

        public async Task AssignItemToShop(AssignItemToShop assign)
        {
            if (await _itemRepository.GetItemById(assign.Id) is null)
            {
                throw new ItemNotFoundException();
            }
            else if (await _shopRepository.GetShopById(assign.ShopId) is null)
            {
                throw new ShopNotFoundException();
            }

        }

        public async Task AddItem(CreateItem createItem) 
        {
            if (await _itemRepository.GetItemByName(createItem.Name) != null)
            {
                throw new ItemAlreadyExistsException();
            }
            Item item = new Item
            {
                Name = createItem.Name,
                Price = createItem.Price,
                Quantity = createItem.Quantity
            };

            await _itemRepository.AddItem(item);
        }

        public async Task DeleteItem(int id)
        {
            if (await _itemRepository.GetItemById(id) is null)
            {
                throw new ItemNotFoundException();
            }

            await _itemRepository.DeleteItem(id);
        }
    }
}
