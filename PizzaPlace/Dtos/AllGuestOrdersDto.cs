using Core.Models;

namespace PizzaPlace.Dtos
{
    public class AllGuestOrdersDto
    {
        public AllGuestOrdersDto()
        {
            MenuItems = new List<GuestOrderDto>();
        }
        public Guid OrderId { get; set; }
        public List<GuestOrderDto> MenuItems { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime PlacedFor { get; set; }
        public int NumberOfPeople { get; set; }
        public Guid GuestId { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
