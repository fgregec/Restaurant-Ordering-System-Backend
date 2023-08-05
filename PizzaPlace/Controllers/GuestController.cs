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

        public GuestController(IGuestRepository guestRepository, IDevelopmentRepository developmentRepository)
        {
            _guestRepository = guestRepository;
            _developmentRepository = developmentRepository;
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

            var guest = await _guestRepository.Login(loginGuest.Email, loginGuest.Password);

            if (guest == null)
            {
                return Unauthorized("Login failed!");
            }

            return Ok(guest);
        }

        [HttpPost("loadseeddata")]
        public async void LoadSeedData()
        {
           _developmentRepository.LoadSeedData();
        }

    }
}
