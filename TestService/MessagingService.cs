using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TestSignalRService.Hubs;
using TestSignalRService.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRService
{
	public class MessagingService : IHostedService, IDisposable
	{
		private readonly ILogger logger;
		private Timer timer;
		private readonly IBus bus;
		private readonly IHubContext<NotificationHub> hubContext;

		public MessagingService(ILogger<MessagingService> logger, IBus bus, IHubContext<NotificationHub> hubContext)
		{
			this.logger = logger;
			this.bus = bus;
			this.hubContext = hubContext;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			logger.LogInformation("Notification Service is starting.");
			timer = new Timer(AddNotificationMessage, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
			return Task.CompletedTask;
		}

		private void AddNotificationMessage(object state)
		{
			bus.Publish<NotificationSendRequestMessage>(new NotificationSendRequestMessage { UserId = "System", Message = $"Notification message - {DateTime.Now}" });
			var message = $"{DateTime.Now.ToLongTimeString()} - Message added to the queue from messaging service.  Context - {hubContext.GetHashCode()}.";
			logger.LogInformation(message);
			hubContext.Clients.All.SendAsync("Notify", message);
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			logger.LogInformation("Notification Service is stopping.");
			timer?.Change(Timeout.Infinite, 0);
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			timer?.Dispose();
		}
	}
}
