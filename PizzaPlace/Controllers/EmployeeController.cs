using Core.Employee_Models;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PizzaPlace.Dtos;
using PizzaPlace.Helpers;

namespace PizzaPlace.Controllers
{
    public class EmployeeController : BaseApiController
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHubContext<MyHub> _hubContext;

        public EmployeeController(IEmployeeRepository employeeRepository, IHubContext<MyHub> hubContext)
        {
            _employeeRepository = employeeRepository;
            _hubContext = hubContext;
        }

        [HttpPost("login")]
        public async Task<ActionResult<Employee>> Login([FromBody] EmployeeLoginDto employee)
        {
            var result = await _employeeRepository.Login(employee.Email, employee.Password);

            if (result == null)
            {
                return BadRequest(result);
            }

            var token = TokenGenerator.GenerateJwtToken(result);

            return Ok(new { result, token });
        }

        [HttpGet("getcheforders")]
        [Authorize(Policy = "Employee")]
        public async Task<ActionResult<List<AllGuestOrdersDto>>> GetOrders()
        {
            var result = await _employeeRepository.GetOrders();

            var data = OrderExtractor.Extract(result);

            if (result == null)
            {
                return BadRequest(data);
            }

            return Ok(data);
        }

        [HttpPost("updatestatus")]
        [Authorize(Policy = "Employee")]
        public async Task<ActionResult> UpdateStatus([FromBody]UpdateStatusDto data)
        {
            var result = await _employeeRepository.UpdateStatus(data.Status, data.Id);

            if (!result)
            {
                return BadRequest();
            }

            if(data.Status == "2")
            {
                await _hubContext.Clients.All.SendAsync("ReceiveUpdate", "Refresh waiter");
            }

            return Ok();
        }

        [HttpGet("getmanagerdata")]
        //[Authorize(Policy = "Employee")]
        public async Task<ActionResult<ManagerDataDto>> GetManagerData()
        {
            var result = await _employeeRepository.GetManagerData();

            if (result == null)
            {
                return BadRequest();
            }

            decimal totalEarned = PaymentController.GetDailyRevenue(DateTime.Now);

            ManagerDataDto data = new ManagerDataDto();
            data.DailyRevenue = Decimal.ToInt32(totalEarned);
            data.NumberOfOrders = result;

            return Ok(data);
        }
    }
}
