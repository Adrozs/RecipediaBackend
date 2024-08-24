using Microsoft.AspNetCore.Mvc;
using Recipedia.Repositories;
using Recipedia.Services;
using Recipedia.ViewModels___DTOs.Collection;
using Recipedia.ViewModels___DTOs.Recipe;
using System.Security.Claims;

namespace Recipedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private readonly ICollectionRepository _collectionRepository;
        private readonly DtoMapperService _dtoMapperService;

        public CollectionController(ICollectionRepository collectionRepository, DtoMapperService dtoMapperService)
        {
            _collectionRepository = collectionRepository;
            _dtoMapperService = dtoMapperService;
        }

        [HttpGet("collection/{collectionId}")]
        public async Task<IActionResult> GetCollectionAsync(int collectionId)
        {
            ClaimsPrincipal userClaims = User;

            var collection = await _collectionRepository.GetCollectionAsync(collectionId, userClaims);
            
            if (collection == null) 
                return NotFound("No matching collection found.");
        
            return Ok(collection);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCollectionAsync()
        {
            ClaimsPrincipal userClaims = User;

            var collections = await _collectionRepository.GetAllCollectionAsync(userClaims);

            if (collections == null)
                return NotFound("No collections were found.");

            return Ok(collections);
        }

        [HttpPost("create-or-update")]
        public async Task<IActionResult> CreateOrUpdateCollectionAsync([FromBody] CollectionDto collectionDto)
        {
            ClaimsPrincipal userClaims = User;

            var collection = _dtoMapperService.MapDtoToCollection(collectionDto);

            var result = await _collectionRepository.CreateOrUpdateCollectionAsync(collection, userClaims);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpDelete("delete/{collectionid}")]
        public async Task<IActionResult> DeleteCollectionAsync(int collectionId)
        {
            ClaimsPrincipal userClaims = User;

            var result = await _collectionRepository.DeteteCollectionAsync(collectionId, userClaims);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok("Successfully deleted collection.");
        }

    }
}
