using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PizzaPlace.Dtos;

namespace PizzaPlace.Controllers
{
    public class GuestOrderController : BaseApiController
    {
        private readonly IGuestRepository _guestRepository;
        private readonly IGuestOrderRepository _guestOrderRepository;
        private readonly IMapper _mapper;

        public GuestOrderController(IGuestRepository guestRepository, IGuestOrderRepository guestOrderRepository, IMapper mapper)
        {
            _guestRepository = guestRepository;
            _guestOrderRepository = guestOrderRepository;
            _mapper = mapper;
        }

        [HttpGet("getmenuitems")]
        public async Task<ActionResult<ICollection<MenuItem>>> GetMenuItems()
        {
            return Ok(await _guestOrderRepository.GetMenuItems());
        }

        [HttpGet("getmenuitem")]
        public async Task<ActionResult<ICollection<MenuItem>>> GetMenuItem(Guid menuItemId)
        {
            return Ok(await _guestOrderRepository.GetMenuItem(menuItemId));
        }

        [HttpGet("getmenuingredients")]
        public async Task<ActionResult<ICollection<Ingredient>>> GetMenuItemIngredients(Guid menuItemId)
        {
            return Ok(await _guestOrderRepository.GetMenuItemIngredients(menuItemId));
        }

        [HttpPost("placeorder")]
        public async Task<ActionResult<string>> PlaceOrder([FromBody] OrderDto order)
        {

            string guestInfoJson = Request.Headers["Guest-Info"];
            Guest guest = guestInfoJson != null ? JsonConvert.DeserializeObject<Guest>(guestInfoJson) : null;

            if(guest == null)
            {
                return BadRequest();
            }

            GuestOrder guestOrder = new GuestOrder();
            guestOrder.Id = Guid.NewGuid();
            guestOrder.PlacedAt = DateTime.Now;
            guestOrder.GuestId = guest.Id;
            guestOrder.NumberOfPeople = order.NumberOfPeople;

            List<GuestOrderDto> menuItems = new List<GuestOrderDto>();
            menuItems = order.MenuItems;

            List<OrderMenuItem> orderMenuItems = new List<OrderMenuItem>();
            List<RemovedIngredient> removedIngredients = new List<RemovedIngredient>();

            for (int i = 0; i < menuItems.Count; i++)
            {
                orderMenuItems.Add(new OrderMenuItem
                {
                    Id = Guid.NewGuid(),
                    MenuItemId = menuItems[i].Id,
                    Quantity = menuItems[i].Quantity,
                    GuestOrderId = guestOrder.Id
                });

                for (int y = 0; y < menuItems[i].RemovedIngredients.Count; y++)
                {
                    removedIngredients.Add(new RemovedIngredient
                    {
                        Id = Guid.NewGuid(),
                        IngredientId = menuItems[i].RemovedIngredients[y].Id,
                        OrderMenuItemId = orderMenuItems[i].Id,
                    });
                }

            }

            guestOrder.OrderMenuItems = orderMenuItems;
            guestOrder.RemovedIngredients = removedIngredients;

            DateTime selectedDateAndTime = new DateTime(
                order.SelectedDate.Year,
                order.SelectedDate.Month,
                order.SelectedDate.Day,
                order.SelectedHours,
                order.SelectedMinutes,
                0
            );

            guestOrder.PlacedFor = selectedDateAndTime;
            guestOrder.OrderStatus = OrderStatus.BOOKED;

            if (await _guestOrderRepository.PlaceOrder(guestOrder))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("getguestorders")]
        public async Task<ActionResult<List<AllGuestOrdersDto>>> GetGuestOrders()
        {
            string guestInfoJson = Request.Headers["Guest-Info"];
            Guest guest = guestInfoJson != null ? JsonConvert.DeserializeObject<Guest>(guestInfoJson) : null;

            if (guest == null)
            {
                return BadRequest();
            }

            List<GuestOrder> result = await _guestOrderRepository.GetAllOrders(guest.Id);

            List<AllGuestOrdersDto> orders = new List<AllGuestOrdersDto>();

            for (int i = 0; i < result.Count; i++)
            {
                AllGuestOrdersDto allGuestOrdersDto = new AllGuestOrdersDto();
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

        [HttpPost("cancel")]
        public async Task<ActionResult<bool>> CancelOrder(Guid orderId)
        {
            string guestInfoJson = Request.Headers["Guest-Info"];
            Guest guest = guestInfoJson != null ? JsonConvert.DeserializeObject<Guest>(guestInfoJson) : null;

            if(guest == null)
            {
                return BadRequest(false);
            }

            if (await _guestOrderRepository.CancelOrder(orderId, guest.Id))
            {
                return Ok(true);
            }
            else
            {
                return BadRequest(false);
            }
        }

    }

}
