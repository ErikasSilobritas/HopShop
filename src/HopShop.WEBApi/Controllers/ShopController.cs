using Application.Services;
using Domain.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HopShop.WEBApi.Controllers
{
    [ApiController]
    [Route("shop")]
    public class ShopController : ControllerBase
    {
        private readonly ShopService _shopService;
        public ShopController(ShopService shopService)
        {
            _shopService = shopService;
        }

        /// <summary>
        /// Get all shops
        /// </summary>
        [HttpGet]
        public async Task<ActionResult>GetAllShops()
        {
            return Ok(await _shopService.GetAllShops());
        }

        /// <summary>
        /// Get a shop by id
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetShopById(int id)
        {
            return Ok(await _shopService.GetShopById(id));
        }

        /// <summary>
        /// Edit a shop
        /// </summary>
        [HttpPut]
        public async Task<ActionResult> EditShop([FromBody] EditShop editShop)
        {
            var result = await _shopService.EditShop(editShop);
            return Ok();
        }


        /// <summary>
        /// Add a shop
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> AddShop([FromBody] CreateShop createShop)
        {
            await _shopService.AddShop(createShop);
            return Created();
        }


        /// <summary>
        /// Delete a shop by id
        /// </summary>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteShop(int id)
        {
            await _shopService.DeleteShop(id);
            return Ok();
        }
    }
}
