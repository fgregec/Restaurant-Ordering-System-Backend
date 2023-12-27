using AutoMapper;
using Azure;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GuestOrderRepository : IGuestOrderRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public GuestOrderRepository(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ICollection<Ingredient>> GetMenuItemIngredients(Guid menuItemId)
        {
            ICollection<MenuItemIngredient> menuItemIngredients = await _context.MenuItemIngredients
                .Where(m => m.MenuItemId == menuItemId)
                .ToListAsync();

            List<Guid> ingredientIds = menuItemIngredients.Select(mi => mi.IngredientId).ToList();

            List<Ingredient> ingredients = await _context.Ingredients
                .Where(i => ingredientIds.Contains(i.Id))
                .ToListAsync();

            return ingredients;
        }

        public async Task<ICollection<MenuItem>> GetMenuItems()
        {
            return await _context.MenuItems.ToListAsync();
        }

        public async Task<ICollection<MenuItem>> GetMenuItem(Guid menuItemId)
        {
            return await _context.MenuItems.Where(m => m.Id == menuItemId).ToListAsync();
        }

        public async Task<bool> PlaceOrder(GuestOrder guestOrder)
        {

            _context.GuestOrders.Add(guestOrder);

            int result = await _context.SaveChangesAsync();

            if(result > 0)
            {
                return true;
            } else
            {
                return false;
            }
            
        }
        public async Task<List<GuestOrder>> GetAllOrders(Guid guestID)
        {
            List<GuestOrder> guestOrder = await _context.GuestOrders
                .Where(go => go.GuestId == guestID)
                .Include(go => go.OrderMenuItems)
                .ThenInclude(omi => omi.MenuItem)
                .Include(go => go.RemovedIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .ToListAsync();

            return guestOrder;

        }

        public async Task<bool> CancelOrder(Guid orderId, Guid guestId)
        {
            var guestOrder = await _context.GuestOrders
                   .FirstAsync(o => o.Id == orderId && o.GuestId == guestId);

            if(guestOrder.PlacedFor < DateTime.UtcNow)
            {
                return false;
            }

            if (guestOrder != null)
            {
                guestOrder.OrderStatus = OrderStatus.CANCELED;
                await _context.SaveChangesAsync();
                return true; 
            }

            return false; 
        }
    }
}
