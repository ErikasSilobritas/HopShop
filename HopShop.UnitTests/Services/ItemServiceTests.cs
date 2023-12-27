using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using HopShop.WEBApi.DTOs.Requests;
using HopShop.WEBApi.Entities;
using HopShop.WEBApi.Exceptions;
using HopShop.WEBApi.Interfaces;
using HopShop.WEBApi.Services;
using Moq;

namespace HopShop.UnitTests.Services
{
    public class ItemServiceTests
    {
        private readonly Mock<IItemRepository> _itemRepositoryMock;
        private readonly ItemService _itemService;
        private readonly Fixture _fixture;

        public ItemServiceTests()
        {
            _itemRepositoryMock = new Mock<IItemRepository>();
            _itemService = new ItemService(_itemRepositoryMock.Object);
            _fixture = new Fixture();

        }

        [Fact]
        public async Task GetItemById_GivenValidId_ReturnsDTO()
        {
            //ARRANGE
            int id = 1;
            
            _itemRepositoryMock.Setup(i => i.GetItemById(id)).ReturnsAsync(new Item
            {
                Id = id
            });

            

            //ACT
            var result = await _itemService.GetItemById(id);

            //ASSERT
            result.Id.Should().Be(id);
        }

        [Fact]

        public async Task GetItemById_GivenInvalidId_ThrowsItemNotFoundException()
        {
            //ARRANGE
            int id = 1;
            
            _itemRepositoryMock.Setup(i => i.GetItemById(id)).Returns(Task.FromResult<Item>(null));
            

            //ACT AND ASSERT IN ONE
            //await Assert.ThrowsAsync<ItemNotFoundException>(async () => await _itemService.GetItemById(id)); // alternative (assert yra "teigiu, kad)

            //ACT
            Func<Task> result = async () => await _itemService.GetItemById(id);

            //ASSERT
            await result.Should().ThrowAsync<ItemNotFoundException>();
        }

        [Fact]

        public async Task GetAllItems_WithItemsPresentInDatabase_ReturnsListOfDTO()
        {
            //ARRANGE 

            

            // cia pasiklaust del IEnumerable, nes mano repositorija ten grazina IEnumerable<Item> o as cia sakau kad list, su IEnum neveikia.
            _itemRepositoryMock.Setup(i => i.GetAllItems()).ReturnsAsync(new List<Item>
            {
                new Item { Id = 1, Name = "Apple"},
                new Item { Id = 2, Name = "Orange"}
            });

           

            //ACT

            var result = await _itemService.GetAllItems();

            //ASSERT

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAllItems_WithNoItemsInDatabase_ReturnsEmptyList()
        {
            //ARRANGE

            
            _itemRepositoryMock.Setup(i => i.GetAllItems()).ReturnsAsync(new List<Item>());

            

            //ACT
            var result = await _itemService.GetAllItems();

            //ASSERT

            result.Should().BeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]

        public async Task EditItem_ItemExists_ReturnsDTO()
        {
            //ARRANGE
            var editItem = new EditItem
            {
                Id = 1,
                Name = "Orange",
                Price = 1.99m,
                Quantity = 1
            };

            _itemRepositoryMock.Setup(i => i.GetItemById(editItem.Id)).ReturnsAsync(new Item { Id = editItem.Id, Name = "name", Price = 1m, Quantity = 1});
            _itemRepositoryMock.Setup(i => i.EditItem(It.Is<Item>(item => item.Id == editItem.Id && item.Name == editItem.Name && item.Price == editItem.Price && item.Quantity == editItem.Quantity))).ReturnsAsync(new Item
            { 
                Id = editItem.Id, 
                Name = editItem.Name,
                Price = editItem.Price,
                Quantity = editItem.Quantity
            });

            //ACT

            var result = await _itemService.EditItem(editItem);

            //ASSERT

            result.Should().NotBeNull();
            result.Id.Should().Be(editItem.Id);
            result.Name.Should().Be(editItem.Name);
            result.Price.Should().Be(editItem.Price);
            result.Quantity.Should().Be(editItem.Quantity);

        }

        [Fact]
        public async Task EditItem_ItemDoesNotExist_ThrowsItemNotFoundException()
        {
            //ARRANGE
            var editItem = new EditItem
            {
                Id = 1,
                Name = "Orange",
                Price = 1.99m,
                Quantity = 1
            };
            
            _itemRepositoryMock.Setup(i => i.GetItemById(editItem.Id)).ReturnsAsync((Item)null);
            //_itemRepositoryMock.Setup(i => i.GetItemById(editItem.Id)).Returns(Task.FromResult<Item>(null)); Same, ar ner skirtumo kuri rasyt?

            //ACT

            Func<Task> result = async () => await _itemService.EditItem(editItem);

            //ASSERT
            await result.Should().ThrowAsync<ItemNotFoundException>();


        }


        [Fact]
        public async Task AddItem_ItemDoesNotExist_CallsRepositoryAddItem()
        {
            //ARRANGE
            var createItem = new CreateItem
            {
                Name = "Apple",
                Price = 1.49m,
                Quantity = 10
            };

            var item = new Item
            {
                Name = createItem.Name,
                Price = createItem.Price,
                Quantity = createItem.Quantity
            };

           
            _itemRepositoryMock.Setup(i => i.GetItemByName(createItem.Name)).ReturnsAsync((Item)null);

            //ACT 

            await _itemService.AddItem(createItem);

            //ASSERT (kadangi testuojamas metodas nk negrazina tai tikriname ar jis sekmingai pacall'ina repositorijos AddItem metoda
            //lygiai viena karta (po itemo neegzistavimo validacijos)

            //itemRepositoryMock.Verify(i => i.AddItem(It.IsAny<Item>()), Times.Once); ne visai korektiska.

            _itemRepositoryMock.Verify(i => i.AddItem(It.Is<Item>(
                item =>
                item.Name == createItem.Name &&
                item.Price == createItem.Price &&
                item.Quantity == createItem.Quantity)), Times.Once);

        }

        [Fact]
        
        public async Task AddItem_ItemExists_ThrowsItemAlreadyExistsException()
        {
            //ARRANGE
            //var createItem = new CreateItem
            //{
            //    Name = "Apple",
            //    Price = 1.49m,
            //    Quantity = 10
            //};

            CreateItem createItem = _fixture.Create<CreateItem>();

            _itemRepositoryMock.Setup(i => i.GetItemByName(createItem.Name)).ReturnsAsync(new Item { Name = createItem.Name });


            //ACT and ASSERT
            // await Assert.ThrowsAsync<ItemAlreadyExistsException>(async () => await _itemService.AddItem(createItem)); arba taip
            // await _itemService.Invoking(i => i.AddItem(createItem)).Should().ThrowAsync<ItemAlreadyExistsException>(); arba sitaip

            //ACT

            var result = async () => await _itemService.AddItem(createItem);

            //ASSERT

            result.Should().ThrowAsync<ItemAlreadyExistsException>();

        }

        [Theory]
        [InlineAutoData]
        [InlineAutoData]
        [InlineAutoData]
        public async Task DeleteItem_ItemExists_CallsRepositoryDeleteItem(int id)
        {

            //ARRANGE

            _itemRepositoryMock.Setup(i => i.GetItemById(id)).ReturnsAsync(new Item { Id = id, Name = "name", Price = 1m, Quantity = 1 });

            //ACT

            await _itemService.DeleteItem(id);

            //ASSERT

            _itemRepositoryMock.Verify(i => i.DeleteItem(id), Times.Once);
        }

        [Fact]
        public async Task DeleteItem_ItemDoesNotExist_ThrowsItemNotFoundException()
        {
            int id = _fixture.Create<int>();

            //ARANGE

            _itemRepositoryMock.Setup(i => i.GetItemById(id)).ReturnsAsync((Item)null);

            //ACT

            var result = async () => await _itemService.DeleteItem(id);

            //ASSERT

            result.Should().ThrowAsync<ItemNotFoundException>();
        }


    }
}