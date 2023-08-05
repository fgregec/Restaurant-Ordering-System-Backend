using Core.Models;

namespace Core.Interfaces
{
    public interface IGuestRepository
    {
        Task<Guest> Register(Guest guest);
        Task<Guest> Login(string email, string password);
        Task<Guest> GetById(int id);
            
    }
}
