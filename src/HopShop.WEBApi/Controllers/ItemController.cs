using Application.Services;
using Domain.DTOs.Requests;
using Domain.Entities;
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

        /// <summary>
        /// Get all items
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetAllItems()
        {
            var result = await _itemService.GetAllItems();
            return Ok(result);
        }

        /// <summary>
        /// Get an item by id
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task <ActionResult<GetItem>> GetItemById(int id)
        {
            var result = await _itemService.GetItemById(id);
            return Ok(result);
        }

        /// <summary>
        /// Edit an item
        /// </summary>
        [HttpPut("edit")]
        public async Task <ActionResult> EditItem([FromBody] EditItem editItem)
        {
            return Ok(await _itemService.EditItem(editItem));
        }

        /// <summary>
        /// Assign an item to a shop
        /// </summary>
        [HttpPut("assign")]
        public async Task<ActionResult<GetItem>> AssignItemToShop([FromBody] AssignItemToShop assign)
        {
            return Ok(await _itemService.AssignItemToShop(assign));
        }

        /// <summary>
        /// Add an item
        /// </summary>
        [HttpPost]
        public async Task <ActionResult> AddItem([FromBody]CreateItem createItem)
        {
            await _itemService.AddItem(createItem);
            return Created();
        }

        /// <summary>
        /// Delete an item by id
        /// </summary>
        [HttpDelete]
        [Route("{id}")]
        public async Task <ActionResult> DeleteItem(int id)
        {
            await _itemService.DeleteItem(id);
            return Ok();
        }
    }
}
