using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Employee_Models
{
    public enum EmployeeRole
    {
        CHEF, WAITER, MANAGER
    }
    public class Employee
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public EmployeeRole Role { get; set; }

    }
}
