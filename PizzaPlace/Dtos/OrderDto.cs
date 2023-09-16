using Core.Models;

namespace PizzaPlace.Dtos
{
    public class OrderDto
    {
        public List<GuestOrderDto> MenuItems { get; set; }
        public DateTime SelectedDate { get; set; }
        public int SelectedHours { get; set; }
        public int SelectedMinutes { get; set; }
        public int NumberOfPeople { get; set; }
    }
}
