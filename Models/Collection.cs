using System.Text.Json.Serialization;

namespace Recipedia.Models
{
    public class Collection
    {
        [JsonPropertyName("collection_id")]
        public int CollectionId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("total_recipes")]
        public int TotalRecipes { get; set; }

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }

    }
}
