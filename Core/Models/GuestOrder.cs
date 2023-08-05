using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class GuestOrder
    {
        [Key]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public List<OrderItem> Order { get; set; }
    }
}
