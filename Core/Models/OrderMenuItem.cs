using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class OrderMenuItem
    {
        [Key]
        public Guid Id { get; set; }
        public Guid MenuItemId { get; set; }
        public Guid GuestOrderId { get; set; }
        public int Quantity { get; set; }
        public MenuItem MenuItem { get; set; }
    }
}
