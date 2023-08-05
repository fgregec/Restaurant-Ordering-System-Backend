using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DevelopmentRepository : IDevelopmentRepository
    {
        private readonly ApplicationContext _context;

        public DevelopmentRepository(ApplicationContext context)
        {
            _context = context;
        }
        public void LoadSeedData()
        {
            Guest guest = new Guest();
            guest.FirstName = "Fran";
            guest.LastName = "Gregec";
            guest.PhoneNumber = "0924385623";
            guest.Email = "fgreg@gmail.com";
            guest.Id = Guid.NewGuid();

            Ingredient ingredient = new Ingredient();
            ingredient.Id = 1;
            ingredient.Name = "Onion";

            Ingredient ingredient1 = new Ingredient();
            ingredient1.Id = 2;
            ingredient1.Name = "Mushrooms";
            
            Ingredient ingredient2 = new Ingredient();
            ingredient2.Id = 3;
            ingredient2.Name = "Olives";

            Ingredient ingredient3 = new Ingredient();
            ingredient3.Id = 4;
            ingredient3.Name = "Parsley";

            MenuItem menuItem = new MenuItem();
            menuItem.Id = 1;
            menuItem.Name = "Pizza Margharita";
            menuItem.Ingredients.Add(ingredient);
            menuItem.Ingredients.Add(ingredient1);
            menuItem.Icon = "asd";
            menuItem.Description = "Tasty pizza";

            MenuItem menuItem1 = new MenuItem();
            menuItem1.Id = 2;
            menuItem1.Name = "Vitos pasta";
            menuItem1.Ingredients.Add(ingredient2);
            menuItem1.Ingredients.Add(ingredient3);
            menuItem1.Icon = "des";
            menuItem1.Description = "Best pasta";

            OrderItem orderItem = new OrderItem();
            orderItem.Id = 1;
            orderItem.Quantity = 1;
            orderItem.MenuItem = menuItem;

            OrderItem orderItem1 = new OrderItem();
            orderItem1.Id = 2;
            orderItem1.Quantity = 2;
            orderItem1.MenuItem = menuItem1;

            GuestOrder guestOrder = new GuestOrder();
            guestOrder.Id = 1;
            guestOrder.UserId = guest.Id;
            guestOrder.Order.Add(orderItem);
            guestOrder.Order.Add(orderItem1);

            _context.Guests.Add(guest);
            _context.Ingredients.Add(ingredient);
            _context.Ingredients.Add(ingredient1);
            _context.Ingredients.Add(ingredient2);
            _context.Ingredients.Add(ingredient3);
            _context.MenuItem.Add(menuItem);
            _context.MenuItem.Add(menuItem1);
            _context.GuestOrder.Add(guestOrder);

            _context.SaveChangesAsync();
        }
    }
}
