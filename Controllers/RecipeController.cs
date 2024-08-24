using Microsoft.AspNetCore.Mvc;
using Recipedia.Models;
using Recipedia.Repositories;
using Recipedia.Services;
using Recipedia.ViewModels___DTOs.Recipe;
using System.Security.Claims;

namespace Recipedia.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly DtoMapperService _dtoMapperService;

        public RecipeController(IRecipeRepository recipeRepository, DtoMapperService dtoMapperService)
        {
            _recipeRepository = recipeRepository;
            _dtoMapperService = dtoMapperService;
        }

        [HttpGet("id/{recipeId}")]
        public async Task<IActionResult> GetRecipeAsync(int recipeId)
        {
            ClaimsPrincipal userClaims = User;

            var recipe = await _recipeRepository.GetRecipeAsync(recipeId, userClaims);

            if (recipe == null)
                return NotFound($"No matching recipe was found.");

            // Return recipe json
            return Ok(recipe);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllRecipesAsync()
        {
            ClaimsPrincipal userClaims = User;

            var recipes = await _recipeRepository.GetAllRecipesAsync(userClaims);

            if (recipes == null) 
                return NotFound("No recipes found for user.");

            // Return recipe json
            return Ok(recipes);
        }

        [HttpPost("create-or-update")]
        public async Task<IActionResult> CreateOrUpdateRecipeAsync([FromBody] RecipeDto recipeDto)
        {
            ClaimsPrincipal userClaims = User;

            if (recipeDto == null)
                return BadRequest("Recipe is null.");

            // Map the incoming recipe to a recipe object
            var recipe = _dtoMapperService.MapDtoToRecipe(recipeDto);


            // Return a "recipe result? or just true or false??
            var result = await _recipeRepository.CreateOrUpdateRecipeAsync(recipe, userClaims);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok("Successfully created or updated recipe.");

        }

        [HttpDelete("id/{recipeId}")]
        public async Task<IActionResult> DeleteRecipeAsync(int recipeId)
        {
            ClaimsPrincipal userClaims = User;

            var result = await _recipeRepository.DeleteRecipeAsync(recipeId, userClaims);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok("Recipe deleted.");
        }

    }
}
