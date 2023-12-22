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
        [Fact]
        public async Task GetItemById_GivenValidId_ReturnsDTO()
        {
            //ARRANGE
            int id = 1;
            var testRepository = new Mock<IItemRepository>();
            testRepository.Setup(i => i.GetItemById(id)).ReturnsAsync(new Item
            {
                Id = id
            });

            var itemService = new ItemService(testRepository.Object);

            //ACT
            var result = await itemService.GetItemById(id);

            //ASSERT
            result.Id.Should().Be(id);
        }

        [Fact]

        public async Task GetItemById_GivenInvalidId_ThrowsItemNotFoundException()
        {
            //ARRANGE
            int id = 1;
            var testRepository = new Mock<IItemRepository>();
            testRepository.Setup(i => i.GetItemById(id)).Returns(Task.FromResult<Item>(null));
            var itemService = new ItemService(testRepository.Object);

            //ACT AND ASSERT IN ONE
            //await Assert.ThrowsAsync<ItemNotFoundException>(async () => await itemService.GetItemById(id));

            //ACT
            Func<Task> result = async () => await itemService.GetItemById(id);

            //ASSERT
            await result.Should().ThrowAsync<ItemNotFoundException>();
        }

        [Fact]

        public async Task GetAllItems_WhitItemsPresentInDatabase_ReturnsListOfDTO()
        {
            //ARRANGE 

            var testRepository = new Mock<IItemRepository>();

            // cia pasiklaust del IEnumerable, nes mano repositorija ten grazina IEnumerable<Item> o as cia sakau kad list, su IEnum neveikia.
            testRepository.Setup(i => i.GetAllItems()).ReturnsAsync(new List<Item>
            {
                new Item { Id = 1, Name = "Apple"},
                new Item { Id = 2, Name = "Orange"}
            });

            var itemService = new ItemService(testRepository.Object);

            //ACT

            var result = await itemService.GetAllItems();

            //ASSERT

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAllItems_WithNoItemsInDatabase_ReturnsEmptyList()
        {
            //ARRANGE

            var testRepository = new Mock<IItemRepository>();
            testRepository.Setup(i => i.GetAllItems()).ReturnsAsync(new List<Item>());

            var itemService = new ItemService(testRepository.Object);

            //ACT
            var result = await itemService.GetAllItems();

            //ASSERT

            result.Should().BeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]

        public async Task EditItem_IfItemExists_ReturnsDTO()
        {
            //ARRANGE
            var editItem = new EditItem
            {
                Id = 1,
                Name = "Orange",
                Price = 1.99m,
                Quantity = 1
            };
            var testRepository = new Mock<IItemRepository>();

            //It.IsAny<Item> cia nera korektiska nes EditItem cia neva paima betkoki item, o jis turi buti egzistuojantis. Cia neiseina kitaip padaryt dabar,
            //nes dalis errorhandlinimo logikos yra repositorijos metode, kas ner teisinga. Tai dabar net jeigu paduosi duombazeje neegzistuojanti itema ir prasysi editint, editins. :(

            testRepository.Setup(i => i.EditItem(It.IsAny<Item>())).ReturnsAsync(new Item 
            { 
                Id = editItem.Id, 
                Name = editItem.Name,
                Price = editItem.Price,
                Quantity = editItem.Quantity
            });

            var itemService = new ItemService(testRepository.Object);

            //ACT

            var result = await itemService.EditItem(editItem);

            //ASSERT

            result.Should().NotBeNull();
            result.Id.Should().Be(editItem.Id);
            result.Name.Should().Be(editItem.Name);
            result.Price.Should().Be(editItem.Price);
            result.Quantity.Should().Be(editItem.Quantity);

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

            var testRepository = new Mock<IItemRepository>();
            testRepository.Setup(i => i.GetItemByName(createItem.Name)).ReturnsAsync((Item)null);

            var itemService = new ItemService(testRepository.Object);

            //ACT 

            await itemService.AddItem(createItem);

            //ASSERT (kadangi testuojamas metodas nk negrazina tai tikriname ar jis sekmingai pacall'ina repositorijos AddItem metoda lygiai viena karta (po itemo neegzistavimo validacijos)

            testRepository.Verify(i => i.AddItem(It.IsAny<Item>()), Times.Once);
        }


    }
}