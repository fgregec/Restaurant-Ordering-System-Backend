using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GuestRepository : IGuestRepository
    {
        private readonly ApplicationContext _context;
        public GuestRepository(ApplicationContext context) 
        {
            _context = context;
        } 
        public Task<Guest> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Guest> Login(string email, string password)
        {
            var guestExists = await _context.Guests.FirstOrDefaultAsync(g => g.Email == email && g.Password == password);

            if (guestExists == null)
            {
                return null;
            }
            guestExists.Password = "null";
            return guestExists;
        }

        public async Task<Guest> Register(Guest guest)
        {
            var guestExists = await _context.Guests.FirstOrDefaultAsync(g => g.Email == guest.Email);
            if (guestExists != null)
            {
                return null;
            }
            await _context.Guests.AddAsync(guest);
            await _context.SaveChangesAsync();
            return guest;
        }
    }
}
