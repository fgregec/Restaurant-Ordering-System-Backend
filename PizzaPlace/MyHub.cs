namespace PizzaPlace
{
    using Microsoft.AspNetCore.SignalR;

    public class MyHub : Hub
    {
        public async Task SendUpdate(string message)
        {
            await Clients.All.SendAsync("ReceiveUpdate", message);
        }
        public async Task SendNewOrderMessage()
        {
            await Clients.All.SendAsync("ReceiveNewOrderMessage");
        }
    }

}
