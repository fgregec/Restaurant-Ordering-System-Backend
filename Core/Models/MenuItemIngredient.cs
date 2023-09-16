using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class MenuItemIngredient
    {
        [Key]
        public Guid Id { get; set; }
        public Guid MenuItemId { get; set; }
        public Guid IngredientId { get; set; }

    }
}
