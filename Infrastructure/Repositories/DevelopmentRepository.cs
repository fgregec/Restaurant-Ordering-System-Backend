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
        public async void LoadSeedData()
        {
            Guest guest = new Guest();
            guest.FirstName = "Fran";
            guest.LastName = "Gregec";
            guest.PhoneNumber = "0924385623";
            guest.Email = "fgreg@gmail.com";
            guest.Id = Guid.NewGuid();
            guest.Password = "123";

            Ingredient ingredient = new Ingredient();
            ingredient.Id = Guid.NewGuid();
            ingredient.Name = "Onion";

            Ingredient ingredient1 = new Ingredient();
            ingredient1.Id = Guid.NewGuid();
            ingredient1.Name = "Mushrooms";

            Ingredient ingredient2 = new Ingredient();
            ingredient2.Id = Guid.NewGuid();
            ingredient2.Name = "Olives";

            Ingredient ingredient3 = new Ingredient();
            ingredient3.Id = Guid.NewGuid();
            ingredient3.Name = "Parsley";

            MenuItem menuItem = new MenuItem();
            menuItem.Id = Guid.NewGuid();
            menuItem.Name = "Pizza";
            menuItem.Icon = "asd";
            menuItem.Description = "Tasty pizza";
            menuItem.Price = 5;

            MenuItem menuItem1 = new MenuItem();
            menuItem1.Id = Guid.NewGuid();
            menuItem1.Name = "Vitos pasta";
            menuItem1.Icon = "des";
            menuItem1.Description = "Best pasta";
            menuItem1.Price = 10;

            // PIZZA HAS ONION AND MUSHROOMS

            MenuItemIngredient menuItemIngredient = new MenuItemIngredient();
            menuItemIngredient.Id = Guid.NewGuid();
            menuItemIngredient.MenuItemId = menuItem.Id;
            menuItemIngredient.IngredientId = ingredient.Id;

            MenuItemIngredient menuItemIngredient1 = new MenuItemIngredient();
            menuItemIngredient1.Id = Guid.NewGuid();
            menuItemIngredient1.MenuItemId = menuItem.Id;
            menuItemIngredient1.IngredientId = ingredient1.Id;

            // VITOS PASTA HAS OLIVES AND PARSLEY

            MenuItemIngredient menuItemIngredient2 = new MenuItemIngredient();
            menuItemIngredient2.Id = Guid.NewGuid();
            menuItemIngredient2.MenuItemId = menuItem1.Id;
            menuItemIngredient2.IngredientId = ingredient2.Id;

            MenuItemIngredient menuItemIngredient3 = new MenuItemIngredient();
            menuItemIngredient3.Id = Guid.NewGuid();
            menuItemIngredient3.MenuItemId = menuItem1.Id;
            menuItemIngredient3.IngredientId = ingredient3.Id;

            GuestOrder guestOrder = new GuestOrder();
            guestOrder.Id = Guid.NewGuid();
            guestOrder.GuestId = guest.Id;
            guestOrder.PlacedAt = DateTime.Now;

            // ADDING FULLY FINISHED MENU ITEMS TO GUEST

            OrderMenuItem orderMenuItem = new OrderMenuItem();
            orderMenuItem.Id = Guid.NewGuid();
            orderMenuItem.GuestOrderId = guestOrder.Id;
            orderMenuItem.MenuItemId = menuItem.Id;
            orderMenuItem.Quantity = 1;

            OrderMenuItem orderMenuItem1 = new OrderMenuItem();
            orderMenuItem1.Id = Guid.NewGuid();
            orderMenuItem1.GuestOrderId = guestOrder.Id;
            orderMenuItem1.MenuItemId = menuItem1.Id;
            orderMenuItem1.Quantity = 2;

            //SHROOMS SHOULD BE REMOVED FROM VITOS PASTA

            RemovedIngredient removedIngredients = new RemovedIngredient();
            removedIngredients.Id = Guid.NewGuid();
            removedIngredients.OrderMenuItemId = orderMenuItem.Id;
            removedIngredients.IngredientId = ingredient.Id;

            _context.Guests.Add(guest);
            _context.Ingredients.AddRange(ingredient, ingredient1, ingredient2, ingredient3);
            _context.MenuItems.AddRange(menuItem, menuItem1);
            _context.MenuItemIngredients.AddRange(menuItemIngredient, menuItemIngredient1, menuItemIngredient2, menuItemIngredient3);
            _context.GuestOrders.Add(guestOrder);
            _context.OrderMenuItems.AddRange(orderMenuItem, orderMenuItem1);
            _context.RemovedIngredients.Add(removedIngredients);
            _context.SaveChanges();
        }
    }
}
