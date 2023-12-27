using Core.Interfaces;
using Core.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Decorators
{
    public sealed class CachingGuestOrderRepository : IGuestOrderRepository
    {
        private readonly IGuestOrderRepository _guestOrderRepository;
        private readonly IMemoryCache _memoryCache;

        public CachingGuestOrderRepository(IGuestOrderRepository guestOrderRepository, IMemoryCache memoryCache)
        {
            _guestOrderRepository = guestOrderRepository;
            _memoryCache = memoryCache;
        }

        public Task<bool> CancelOrder(Guid orderId, Guid guestId)
        {
            return _guestOrderRepository.CancelOrder(orderId, guestId);
        }

        public Task<List<GuestOrder>> GetAllOrders(Guid guestID)
        {
            return _memoryCache.GetOrCreateAsync($"get-all-orders-{guestID}",
                cacheEntry =>
                {
                    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                    return _guestOrderRepository.GetAllOrders(guestID);
                });
        }

        public Task<ICollection<MenuItem>> GetMenuItem(Guid menuItemId)
        {
            return _memoryCache.GetOrCreateAsync($"menu-item-{menuItemId}",
                cacheEntry =>
                {
                    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                    return _guestOrderRepository.GetMenuItem(menuItemId);
                });
        }

        public Task<ICollection<Ingredient>> GetMenuItemIngredients(Guid menuItemId)
        {
            return _memoryCache.GetOrCreateAsync($"menu-item-ingredients-{menuItemId}",
                cacheEntry =>
                {
                    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                    return _guestOrderRepository.GetMenuItemIngredients(menuItemId);
                });
        }

        public Task<ICollection<MenuItem>> GetMenuItems()
        {
            return _memoryCache.GetOrCreateAsync($"menu-items",
                cacheEntry =>
                {
                    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromDays(1));
                    return _guestOrderRepository.GetMenuItems();
                });
        }

        public Task<bool> PlaceOrder(GuestOrder guestOrder)
        {
            return _guestOrderRepository.PlaceOrder(guestOrder);
        }
    }
}
