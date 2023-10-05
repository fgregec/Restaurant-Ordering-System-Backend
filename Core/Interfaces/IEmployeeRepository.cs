using Core.Employee_Models;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee> Register(Employee employee);
        Task<Employee> Login(string email, string password);
        Task<Employee> GetById(Guid id);
        Task<List<GuestOrder>> GetOrders();
        Task<bool> UpdateStatus(string status, Guid id);
        Task<int> GetManagerData();
    }
}
