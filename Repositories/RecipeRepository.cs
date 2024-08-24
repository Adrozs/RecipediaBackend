using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Recipedia.Data;
using Recipedia.Exceptions;
using Recipedia.Models;
using Recipedia.ResultObjects;
using System.Security.Claims;

namespace Recipedia.Repositories
{
    public interface IRecipeRepository
    {
        public Task<Recipe> GetRecipeAsync(int recipeId, ClaimsPrincipal userClaims);
        public Task<List<Recipe>> GetAllRecipesAsync(ClaimsPrincipal userClaims);
        public Task<OperationResult> CreateOrUpdateRecipeAsync(Recipe recipe, ClaimsPrincipal userClaims); 
        public Task<OperationResult> DeleteRecipeAsync(int recipeId, ClaimsPrincipal userClaims);
    }

    public class RecipeRepository : IRecipeRepository
    {
        private readonly ApplicationContext _context;

        public RecipeRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Recipe> GetRecipeAsync(int recipeId, ClaimsPrincipal userClaims)
        {
            User user = await GetUserAndRecipesAsync(userClaims);

            var recipe = user.Recipes.SingleOrDefault(r => r.RecipeId ==  recipeId);

            return recipe;
        }

        public async Task<List<Recipe>> GetAllRecipesAsync(ClaimsPrincipal userClaims)
        {
            User user = await GetUserAndRecipesAsync(userClaims);

            var recipes = user.Recipes.ToList();

            if (recipes.IsNullOrEmpty())
                return null;

            return recipes;
        }

        public async Task<OperationResult> CreateOrUpdateRecipeAsync(Recipe recipe, ClaimsPrincipal userClaims)
        {
            User user = await GetUserAndRecipesAsync(userClaims); 

            // Check if incoming recipe already is saved to user
            var existingRecipe = user.Recipes.Single(r => r.RecipeId == recipe.RecipeId);
            if (existingRecipe != null)
            {
                // Update existing recipe with the incoming data
                existingRecipe.Name = recipe.Name;
                existingRecipe.IsFavorite = recipe.IsFavorite;
                existingRecipe.ImageUrl = recipe.ImageUrl;
                existingRecipe.Rating = recipe.Rating;
                existingRecipe.Allergens = recipe.Allergens;
                existingRecipe.Time = recipe.Time;
                existingRecipe.Ingredients = recipe.Ingredients;
                existingRecipe.Instructions = recipe.Instructions;
                existingRecipe.Nutrition = recipe.Nutrition;

                await _context.SaveChangesAsync();
                return OperationResult.Successful("Successfully updated recipe.");

            }
            // If recipe doesn't exist create it
            else if (existingRecipe == null)
            {
                // Add new recipe to user
                user.Recipes.Add(recipe);

                await _context.SaveChangesAsync();
                return OperationResult.Successful("Successfully created recipe.");

            }

            // This will never run ?!
            return OperationResult.Failed("Failed to update or create recipe");
        }


        public async Task<OperationResult> DeleteRecipeAsync(int recipeId, ClaimsPrincipal userClaims)
        {
            User user = await GetUserAndRecipesAsync(userClaims);

            var recipe = user.Recipes.SingleOrDefault(r => r.RecipeId == recipeId);

            if (recipe == null)
                return OperationResult.Failed("No matching recipe found.");

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return OperationResult.Successful("Successfully deleted recipe.");
        }  


        // Help methods
        private async Task<User> GetUserAndRecipesAsync(ClaimsPrincipal userClaims)
        {
            // Get the email from the JWT claims
            string? email = userClaims.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                throw new UserNotFoundException("No email found in token claims.");

            // Get user along with its saved job ads
            User? user = await _context.Users
                .Include(u => u.Recipes)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                throw new UserNotFoundException("No matching user found with the provided email.");

            return user;
        }
    }
}
