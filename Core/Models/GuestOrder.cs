using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public enum OrderStatus
    {
        BOOKED, CANCELED, SERVED
    }
    public class GuestOrder
    {
        [Key]
        public Guid Id { get; set; }
        public Guid GuestId { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime PlacedFor { get; set; }
        public int NumberOfPeople { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public List<OrderMenuItem> OrderMenuItems { get; set; }
        public List<RemovedIngredient> RemovedIngredients { get; set; }
    }
}
