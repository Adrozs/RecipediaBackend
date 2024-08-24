using System.Text.Json.Serialization;

namespace Recipedia.Models
{
    public class Shoppinglist
    {
        [JsonPropertyName("shoppinglist_id")]
        public int ShoppinglistId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("items")]
        public List<Items> Items { get; set; }

    }

    public class Items
    {
        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("unit")]
        public string Unit { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

    }
}
