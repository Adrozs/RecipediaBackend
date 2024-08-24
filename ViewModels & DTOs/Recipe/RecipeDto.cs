using System.Text.Json.Serialization;

namespace Recipedia.ViewModels___DTOs.Recipe
{
    public class RecipeDto
    {
        
        public int RecipeId { get; set; }

        public bool IsFavorite { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public int Rating { get; set; }

        public AllergensDto Allergens { get; set; }

        public string Time { get; set; }

        public List<IngredientsDto> Ingredients { get; set; }

        public List<string> Instructions { get; set; }

        public NutritionDto Nutrition { get; set; }
    }


    public class AllergensDto
    {
        public bool Beef { get; set; }
        public bool Chicken { get; set; }
        public bool Pork { get; set; }
        public bool Fish { get; set; }
        public bool Shellfish { get; set; }
        public bool Vegan { get; set; }
        public bool Milk { get; set; }
        public bool Gluten { get; set; }
        public bool Nuts { get; set; }
        public bool Soy { get; set; }
        public bool Eggs { get; set; }
        public bool Sesame { get; set; }
    }

    public class IngredientsDto
    {
        public int Amount { get; set; }
        public string Unit { get; set; }
        public string Name { get; set; }
    }

    public class NutritionDto
    {
        public int Energy { get; set; }
        public int Fat { get; set; }
        public int Salt { get; set; }
        public int Carbs { get; set; }
        public int Protein { get; set; }
    }
}

