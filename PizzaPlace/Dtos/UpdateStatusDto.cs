using Core.Models;

namespace PizzaPlace.Dtos
{
    public class UpdateStatusDto
    {
        public string Status { get; set; }
        public Guid Id { get; set; }
    }
}
