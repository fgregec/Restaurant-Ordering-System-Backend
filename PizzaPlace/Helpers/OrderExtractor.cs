using Core.Models;
using PizzaPlace.Dtos;

namespace PizzaPlace.Helpers
{
    public class OrderExtractor
    {
        public static List<AllGuestOrdersDto> Extract(List<GuestOrder> result)
        {
            List<AllGuestOrdersDto> orders = new List<AllGuestOrdersDto>();

            for (int i = 0; i < result.Count; i++)
            {
                AllGuestOrdersDto allGuestOrdersDto = new AllGuestOrdersDto();
                allGuestOrdersDto.GuestId = result[i].GuestId;
                allGuestOrdersDto.PlacedAt = result[i].PlacedAt;
                allGuestOrdersDto.PlacedFor = result[i].PlacedFor;
                allGuestOrdersDto.OrderId = result[i].Id;
                allGuestOrdersDto.NumberOfPeople = result[i].NumberOfPeople;
                allGuestOrdersDto.OrderStatus = result[i].OrderStatus;

                for (int y = 0; y < result[i].OrderMenuItems.Count; y++)
                {
                    GuestOrderDto guestOrderDto = new GuestOrderDto();
                    guestOrderDto.Id = result[i].OrderMenuItems[y].MenuItemId;
                    guestOrderDto.Quantity = result[i].OrderMenuItems[y].Quantity;
                    guestOrderDto.Price = result[i].OrderMenuItems[y].MenuItem.Price;
                    guestOrderDto.MenuItem = result[i].OrderMenuItems[y].MenuItem;

                    for (int z = 0; z < result[i].RemovedIngredients.Count; z++)
                    {
                        if (result[i].RemovedIngredients[z].OrderMenuItemId == result[i].OrderMenuItems[y].Id)
                        {
                            guestOrderDto.RemovedIngredients.Add(result[i].RemovedIngredients[z].Ingredient);
                        }
                    }

                    allGuestOrdersDto.MenuItems.Add(guestOrderDto);
                }

                orders.Add(allGuestOrdersDto);
            }
            return orders;
        }
    }
}
