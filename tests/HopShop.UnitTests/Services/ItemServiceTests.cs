using Application.Services;
using AutoFixture.Xunit2;
using Domain.DTOs.Requests;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace HopShop.UnitTests.Services
{
    public class ItemServiceTests
    {
        private readonly Mock<IItemRepository> _itemRepositoryMock;
        private readonly ItemService _itemService;

        public ItemServiceTests()
        {
            _itemRepositoryMock = new Mock<IItemRepository>();
            _itemService = new ItemService(_itemRepositoryMock.Object);
        }

        [Theory]
        [AutoData]
        public async Task GetItemById_GivenValidId_ReturnsDTO(int id)
        {
            //ARRANGE
            _itemRepositoryMock.Setup(i => i.GetItemById(id)).ReturnsAsync(new Item
            {
                Id = id
            });

            //ACT
            var result = await _itemService.GetItemById(id);

            //ASSERT
            result.Id.Should().Be(id);
        }

        [Theory]
        [AutoData]

        public async Task GetItemById_GivenInvalidId_ThrowsItemNotFoundException(int id)
        {
            //ARRANGE
            _itemRepositoryMock.Setup(i => i.GetItemById(id)).Returns(Task.FromResult<Item>(null));

            //ACT
            var result = async () => await _itemService.GetItemById(id);

            //ASSERT
            await result.Should().ThrowAsync<ItemNotFoundException>();
        }

        [Theory]
        [AutoData]
        public async Task GetAllItems_WithItemsPresentInDatabase_ReturnsListOfDTO(List<Item> items)
        {
            //ARRANGE 
            _itemRepositoryMock.Setup(i => i.GetAllItems()).ReturnsAsync(items);

            //ACT
            var result = await _itemService.GetAllItems();

            //ASSERT
            result.Should().NotBeNull();
            result.Should().HaveCount(items.Count);
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

        [Theory]
        [AutoData]
        public async Task EditItem_ItemExists_ReturnsDTO(EditItem editItem)
        {
            //ARRANGE
            _itemRepositoryMock.Setup(i => i.GetItemById(editItem.Id)).ReturnsAsync(new Item { Id = editItem.Id, Name = "name", Price = 1m, Quantity = 1 });
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

        [Theory]
        [AutoData]
        public async Task EditItem_ItemDoesNotExist_ThrowsItemNotFoundException(EditItem editItem)
        {
            //ARRANGE
            _itemRepositoryMock.Setup(i => i.GetItemById(editItem.Id)).ReturnsAsync((Item)null);

            //ACT
            var result = async () => await _itemService.EditItem(editItem);

            //ASSERT
            await result.Should().ThrowAsync<ItemNotFoundException>();
        }


        [Theory]
        [AutoData]
        public async Task AddItem_ItemDoesNotExist_CallsRepositoryAddItem(CreateItem createItem)
        {
            //ARRANGE
            _itemRepositoryMock.Setup(i => i.GetItemByName(createItem.Name)).ReturnsAsync((Item)null);

            //ACT 
            await _itemService.AddItem(createItem);

            //ASSERT
            _itemRepositoryMock.Verify(i => i.AddItem(It.Is<Item>(
                item =>
                item.Name == createItem.Name &&
                item.Price == createItem.Price &&
                item.Quantity == createItem.Quantity)), Times.Once);
        }

        [Theory]
        [AutoData]
        public async Task AddItem_ItemExists_ThrowsItemAlreadyExistsException(CreateItem createItem)
        {
            //ARRANGE
            _itemRepositoryMock.Setup(i => i.GetItemByName(createItem.Name)).ReturnsAsync(new Item { Name = createItem.Name });

            //ACT
            var result = async () => await _itemService.AddItem(createItem);

            //ASSERT
            await result.Should().ThrowAsync<ItemAlreadyExistsException>();
        }

        [Theory]
        [AutoData]
        public async Task DeleteItem_ItemExists_CallsRepositoryDeleteItem(int id)
        {

            //ARRANGE
            _itemRepositoryMock.Setup(i => i.GetItemById(id)).ReturnsAsync(new Item { Id = id, Name = "name", Price = 1m, Quantity = 1 });

            //ACT
            await _itemService.DeleteItem(id);

            //ASSERT
            _itemRepositoryMock.Verify(i => i.DeleteItem(id), Times.Once);
        }

        [Theory]
        [AutoData]
        public async Task DeleteItem_ItemDoesNotExist_ThrowsItemNotFoundException(int id)
        {
            //ARANGE
            _itemRepositoryMock.Setup(i => i.GetItemById(id)).ReturnsAsync((Item)null);

            //ACT
            var result = async () => await _itemService.DeleteItem(id);

            //ASSERT
            await result.Should().ThrowAsync<ItemNotFoundException>();
        }
    }
}