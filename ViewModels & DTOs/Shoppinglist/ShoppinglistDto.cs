namespace Recipedia.ViewModels___DTOs
{
    public class ShoppinglistDto
    {
        public int ShoppinglistId { get; set; }
        public string Title { get; set; }
        public List<ItemsDto> Items { get; set; }
    }

    public class ItemsDto
    {
        public int Amount { get; set; }
        public string Unit { get; set; }
        public string Name { get; set; }
    }
}
