using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IGuestOrderRepository
    {
        Task<ICollection<MenuItem>> GetMenuItems();
        Task<ICollection<MenuItem>> GetMenuItem(Guid menuItemId);
        Task<ICollection<Ingredient>> GetMenuItemIngredients(Guid menuItemId);
        Task<bool> PlaceOrder(GuestOrder guestOrder);
        Task<List<GuestOrder>> GetAllOrders(Guid guestID);
        Task<bool> CancelOrder(Guid orderId, Guid guestId);
    }
}
