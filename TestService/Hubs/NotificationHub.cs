using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace TestSignalRService.Hubs
{
	public class NotificationHub : Hub
	{
		private readonly ILogger<NotificationHub> logger;

		public NotificationHub(ILogger<NotificationHub> logger)
		{
			this.logger = logger;
		}

		public override Task OnConnectedAsync()
		{
			var connectionId = Context.ConnectionId;

			logger.LogInformation($"Client {connectionId} Connected.");
			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception exception)
		{
			logger.LogInformation($"Client {Context.ConnectionId} Disconnected.");
			return base.OnDisconnectedAsync(exception);
		}
	}
}
