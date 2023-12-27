using HopShop.WEBApi.DTOs.Requests;
using HopShop.WEBApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HopShop.WEBApi.Controllers
{
    [ApiController]
    [Route("item")]
    public class ItemController : ControllerBase
    {
        private readonly ItemService _itemService;

        public ItemController(ItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task <ActionResult<GetItem>> GetItemById(int id)
        {
            var result = await _itemService.GetItemById(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task <ActionResult> GetAllItems()
        {
            var result = await _itemService.GetAllItems();
            return Ok(result);
        }

        [HttpPut]

        public async Task <ActionResult> EditItem([FromBody] EditItem editItem)
        {
            var result = await _itemService.EditItem(editItem);
            return Ok();
        }

        [HttpPost]
        public async Task <ActionResult> AddItem(string name, decimal price, int quantity)
        {
            await _itemService.AddItem(name, price, quantity);
            return Ok();
        }

        [HttpPost]
        [Route("from json")]
        public async Task <ActionResult> AddItem([FromBody]CreateItem createItem)
        {
            await _itemService.AddItem(createItem);
            return Created();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task <ActionResult> DeleteItem(int id)
        {
            await _itemService.DeleteItem(id);
            return Ok();
        }

       
    }
}
