using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using TestSignalRService.Hubs;
using TestSignalRService.Messages;
using System;
using System.Threading.Tasks;

namespace TestSignalRService.Consumers
{
	public class MessageConsumer : IConsumer<NotificationSendRequestMessage>
	{
		private readonly ILogger<MessageConsumer> logger;
		private readonly IHubContext<NotificationHub> hubContext;

		public MessageConsumer(ILogger<MessageConsumer> logger, IHubContext<NotificationHub> hubContext)
		{
			this.logger = logger;
			this.hubContext = hubContext;
		}

		public Task Consume(ConsumeContext<NotificationSendRequestMessage> context)
		{
			var message = $"{DateTime.Now.ToLongTimeString()} - Message consumed from the queue.  Context - {hubContext.GetHashCode()}.";
			logger.LogInformation(message);
			hubContext.Clients.All.SendAsync("Notify", message);
			
			return context.ConsumeCompleted;
		}
	}
}
