using CarPoolingAPI.Data;
using CarPoolingAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CarPoolingAPI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IConfiguration configuration;
        private readonly DataContext context;
        public ChatHub(DataContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public async Task SendMessageToDriver(int requestId, string message)
        {
            var tripRequest = context.TripRequests.FirstOrDefault(x => x.RequestId == requestId);
            await Clients.All.SendAsync("ReceiveMessageFromPassenger", tripRequest.Id, message);

            var chat = new Chat()
            {
                Message = message,
                DateTime = DateTime.Now,
                IsDriverMessage = false,
                TripRequestId = tripRequest.Id
            };

            context.Add(chat);
            await context.SaveChangesAsync();
        }

        public async Task SendMessageToPassenger(int tripRequestId, string message)
        {
            var tripRequest = context.TripRequests.FirstOrDefault(x => x.Id == tripRequestId);
            await Clients.All.SendAsync("ReceiveMessageFromDriver", tripRequest.RequestId, message);

            var chat = new Chat()
            {
                Message = message,
                DateTime = DateTime.Now,
                IsDriverMessage = true,
                TripRequestId = tripRequestId
            };

            context.Add(chat);
            await context.SaveChangesAsync();
        }

        public async Task OnConnect(int tripRequestId)
        {
            await Clients.All.SendAsync("OnConnect", tripRequestId);
        }

        public async Task OnDisconnect(int tripRequestId)
        {
            await Clients.All.SendAsync("OnDisconnect", tripRequestId);
        }
    }
}
