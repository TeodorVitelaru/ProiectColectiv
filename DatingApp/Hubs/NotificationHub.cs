using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace DatingApp.Hubs
{
    /// <summary>
    /// SignalR Hub for real-time notifications.
    /// </summary>
    [Authorize]
    public class NotificationHub : Hub
    {
        private static readonly Dictionary<long, string> _userConnections = new();

        /// <summary>
        /// Called when a client connects to the hub.
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            var userId = GetUserId();
            if (userId > 0)
            {
                _userConnections[userId] = Context.ConnectionId;
                await base.OnConnectedAsync();
            }
        }

        /// <summary>
        /// Called when a client disconnects from the hub.
        /// </summary>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = GetUserId();
            if (userId > 0)
            {
                _userConnections.Remove(userId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Gets the current user's ID from the JWT token.
        /// </summary>
        private long GetUserId()
        {
            var userIdClaim = Context.User?.FindFirst("sub")?.Value
                ?? Context.User?.FindFirst("userId")?.Value;

            if (long.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }
            return 0;
        }

        /// <summary>
        /// Gets the connection ID for a specific user.
        /// </summary>
        public static string? GetConnectionId(long userId)
        {
            return _userConnections.TryGetValue(userId, out var connectionId) ? connectionId : null;
        }
    }
}

