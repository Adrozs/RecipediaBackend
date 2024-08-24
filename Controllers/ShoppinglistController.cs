using Microsoft.AspNetCore.Mvc;
using Recipedia.Repositories;
using Recipedia.Services;
using Recipedia.ViewModels___DTOs;

namespace Recipedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppinglistController : ControllerBase
    {
        private readonly IShoppinglistRepository _shoppinglistRepository;
        private readonly DtoMapperService _dtoMapperService;

        public ShoppinglistController(IShoppinglistRepository shoppinglistRepository, DtoMapperService dtoMapperService)
        {
            _shoppinglistRepository = shoppinglistRepository;
            _dtoMapperService = dtoMapperService;
        }

        [HttpGet("shoppinglist/{shoppinglistId}")]
        public async Task<IActionResult> GetShoppinglistAsync(int shoppinglistId)
        {
            throw new NotImplementedException();

        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllShoppinglistAsync()
        {
            throw new NotImplementedException();

        }

        [HttpPost("create-or-update")]
        public async Task<IActionResult> CreateOrUpdateShoppinglistAsync([FromBody] ShoppinglistDto shoppinglistDto)
        {
            // Shoppinglist auto mapper here

            throw new NotImplementedException();

        }

        [HttpDelete("delete/{shoppinglistid}")]
        public async Task<IActionResult> DeleteShoppinglistAsync(int shoppinglistId)
        {
            throw new NotImplementedException();

        }
    }
}