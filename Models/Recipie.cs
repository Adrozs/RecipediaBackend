using Azure.Core.Serialization;
using System.Text.Json.Serialization;

namespace Recipedia.Models
{
    public class Recipe
    {
        [JsonPropertyName("recipe_id")]
        public int RecipeId { get; set; }

        [JsonPropertyName("is_Favorite")]
        public bool IsFavorite { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("rating")]
        public int Rating{ get; set; }

        [JsonPropertyName("allergens")]
        public Allergens Allergens { get; set; }

        [JsonPropertyName("time")]
        public string Time { get; set; }

        [JsonPropertyName("ingredients")]
        public List<Ingredients> Ingredients { get; set; }

        [JsonPropertyName("instructions")]
        public List<string> Instructions { get; set; }

        [JsonPropertyName("nutrition")]
        public Nutrition Nutrition { get; set; }

        public virtual ICollection<Collection> Collections { get; set; }
    }

    public class Allergens
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

    public class Ingredients
    {
        public int Amount { get; set; }
        public string Unit { get; set; }
        public string Name { get; set; }
    }

    public class Nutrition
    {
        public int Energy { get; set;}
        public int Fat { get; set;}
        public int Salt { get; set;}
        public int Carbs { get; set;}
        public int Protein { get; set;}
    }
}
