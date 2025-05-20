using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Hospital.Services
{
    [Authorize]
    public class ChatHub : Hub
    {
        private static Dictionary<string, string> userConnections = new Dictionary<string, string>();

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public async Task RegisterUser(string userType) 
        {
            var userId = Context.UserIdentifier; 
            userConnections[userId] = Context.ConnectionId;
            await Groups.AddToGroupAsync(Context.ConnectionId, userType);
            await Clients.Caller.SendAsync("UserRegistered", $"You are registered as {userType}");
        }

        public async Task SendMessageToUser(string receiverId, string message)
        {
            var senderId = Context.UserIdentifier; 
            if (userConnections.TryGetValue(receiverId, out string connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceivePrivateMessage", senderId, message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.UserIdentifier;
            if (userId != null && userConnections.ContainsKey(userId))
            {
                userConnections.Remove(userId);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}