using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public MenuItem MenuItem  { get; set; }
        public int Quantity { get; set; }
    }
}
