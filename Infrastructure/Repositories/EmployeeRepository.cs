using Core.Employee_Models;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationContext _context;
        public EmployeeRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<Employee> GetById(Guid id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return null;
            }
            employee.Password = null;
            return employee;
        }

        public async Task<Employee> Login(string email, string password)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email && e.Password == password);

            if(employee == null)
            {
                return null;
            }
            employee.Password = "";
            return employee;
        }

        public async Task<Employee> Register(Employee employee)
        {
            var result = _context.Employees.FirstOrDefaultAsync(e => e.Email == employee.Email);

            if (result!=null)
            {
                return null;
            }

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<List<GuestOrder>> GetOrders()
        {
            DateTime dateNow = DateTime.Now;

            List<GuestOrder> guestOrder = _context.GuestOrders
                .Where(go => go.PlacedFor.Date == dateNow.Date)
                .Where(go => go.OrderStatus != OrderStatus.CANCELED)
                .Include(go => go.OrderMenuItems)
                .ThenInclude(omi => omi.MenuItem)
                .Include(go => go.RemovedIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .ToList();

            return guestOrder;
        }

        public async Task<bool> UpdateStatus(string status, Guid id)
        {
            if (int.TryParse(status, out int statusIntValue))
            {
                if (Enum.IsDefined(typeof(OrderStatus), statusIntValue))
                {
                    OrderStatus orderStatus = (OrderStatus)statusIntValue;

                    GuestOrder order = await _context.GuestOrders.FirstOrDefaultAsync(go => go.Id == id);
                    order.OrderStatus = orderStatus;
                    var result = await _context.SaveChangesAsync();

                    return result > 0;
                }
            }

            return false; 
        }

        public async Task<int> GetManagerData()
        {
            DateTime dateNow = DateTime.Now;

            return _context.GuestOrders
                .Where(go => go.PlacedFor.Date == dateNow.Date)
                .Where(go => go.OrderStatus != OrderStatus.CANCELED)
                .Include(go => go.OrderMenuItems)
                .ThenInclude(omi => omi.MenuItem)
                .Include(go => go.RemovedIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .ToList().Count;
        }
    }
}
