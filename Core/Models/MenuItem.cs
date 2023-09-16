using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class MenuItem
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public int Price { get; set; }
    }
}
