using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class RemovedIngredient
    {
        [Key]
        public Guid Id { get; set; }
        public Guid OrderMenuItemId { get; set; }
        public Guid IngredientId { get; set; }

        public Ingredient Ingredient { get; set; }
    }
}
