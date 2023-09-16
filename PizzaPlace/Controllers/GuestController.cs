using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Dtos;
using System.ComponentModel.DataAnnotations;

namespace PizzaPlace.Controllers
{
    public class GuestController : BaseApiController
    {
        private readonly IGuestRepository _guestRepository;
        private readonly IDevelopmentRepository _developmentRepository;
        private readonly IMapper _mapper;

        public GuestController(IGuestRepository guestRepository, IDevelopmentRepository developmentRepository, IMapper mapper)
        {
            _guestRepository = guestRepository;
            _developmentRepository = developmentRepository;
            _mapper = mapper;   
        }

        [HttpPost("register")]
        public async Task<ActionResult<Guest>> Register([FromBody] Guest guest)
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest("Please fill out all fields when registering!");
            }
            guest.Id = Guid.NewGuid();
            var registeredGuest = await _guestRepository.Register(guest);
            if (registeredGuest == null)
            {
                return BadRequest("User already exists!");
            }

            return Ok(registeredGuest);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Guest>> Login([FromBody] LoginGuestDto loginGuest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please provide email and password!");
            }

            Guest guest = await _guestRepository.Login(loginGuest.Email, loginGuest.Password);

            if (guest == null)
            {
                return Unauthorized("Login failed!");
            }

            LoginGuestInfoDto loginGuestInfo = new LoginGuestInfoDto();
                
            _mapper.Map(guest, loginGuestInfo);

            return Ok(loginGuestInfo);
        }

        [HttpPost("loadseeddata")]
        public async void LoadSeedData()
        {
           _developmentRepository.LoadSeedData();
        }

    }
}
