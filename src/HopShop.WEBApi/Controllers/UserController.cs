using Application.Services;
using Domain.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HopShop.WEBApi.Controllers
{
    [ApiController()]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            return Ok(await _userService.GetUsers());
        }

        /// <summary>
        /// Get a user by id
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetUserById(int id)
        {
            return Ok(await _userService.GetUserById(id));
        }

        /// <summary>
        /// Add a user
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody]CreateUser user)
        {
            return Ok(await _userService.CreateUser(user));
        }

        /// <summary>
        /// Buy some items
        /// </summary>
        [HttpPut]
        public async Task<ActionResult> Buy([FromBody] BuyItem item)
        {
            return Ok(await _userService.Buy(item));
        }
    }
}
