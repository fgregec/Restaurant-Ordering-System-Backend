using Core.Models;

namespace PizzaPlace.Dtos
{
    public class GuestOrderDto
    {
        public GuestOrderDto()
        {
            RemovedIngredients = new List<Ingredient>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public List<Ingredient> RemovedIngredients { get; set; }
        public int Quantity { get; set; }
        public MenuItem? MenuItem { get; set; }

    }
}
