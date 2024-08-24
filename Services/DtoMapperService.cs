using Recipedia.Migrations;
using Recipedia.Models;
using Recipedia.ViewModels___DTOs;
using Recipedia.ViewModels___DTOs.Collection;
using Recipedia.ViewModels___DTOs.Recipe;
using System.Linq;

namespace Recipedia.Services
{
    public class DtoMapperService
    {
        public Recipe MapDtoToRecipe(RecipeDto dto)
        {
            return new Recipe
            {
                RecipeId = dto.RecipeId,
                IsFavorite = dto.IsFavorite,
                Name = dto.Name,
                ImageUrl = dto.ImageUrl,
                Rating = dto.Rating,
                Allergens = new Allergens
                {
                    Beef = dto.Allergens.Beef,
                    Chicken = dto.Allergens.Chicken,
                    Pork = dto.Allergens.Pork,
                    Fish = dto.Allergens.Fish,
                    Shellfish = dto.Allergens.Shellfish,
                    Vegan = dto.Allergens.Vegan,
                    Milk = dto.Allergens.Milk,
                    Gluten = dto.Allergens.Gluten,
                    Nuts = dto.Allergens.Nuts,
                    Soy = dto.Allergens.Soy,
                    Eggs = dto.Allergens.Eggs,
                    Sesame = dto.Allergens.Sesame
                },
                Time = dto.Time,
                Ingredients = dto.Ingredients.Select(i => new Ingredients
                {
                    Amount = i.Amount,
                    Unit = i.Unit,
                    Name = i.Name
                }).ToList(),
                Instructions = dto.Instructions,
                Nutrition = new Nutrition
                {
                    Energy = dto.Nutrition.Energy,
                    Fat = dto.Nutrition.Fat,
                    Salt = dto.Nutrition.Salt,
                    Carbs = dto.Nutrition.Carbs,
                    Protein = dto.Nutrition.Protein
                }
            };
        }

        public Collection MapDtoToCollection(CollectionDto dto)
        {
            return new Collection
            {
                CollectionId = dto.CollectionId,
                Title = dto.Title,
                TotalRecipes = dto.TotalRecipes,
                ImageUrl = dto.ImageUrl
            };
        }

        public Shoppinglist MapDtoToShoppinglist(ShoppinglistDto dto)
        {
            return new Shoppinglist
            {
                ShoppinglistId = dto.ShoppinglistId,
                Items = dto.Items.Select(i => new Items
                {
                    Amount = i.Amount,
                    Unit = i.Unit,
                    Name = i.Name
                }).ToList(),
                Title = dto.Title
            };
        }
    }
}
