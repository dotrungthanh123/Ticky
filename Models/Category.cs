namespace Ticky.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public List<Event> Events { get; } = new List<Event>();  
        public string Name { get; set; }
    }
}
