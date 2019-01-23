using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestSignalRService.Consumers;
using System;

namespace SignalRService
{
	public static class Extensions
	{
		public static void AddBusControl(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped(typeof(MessageConsumer));

			services.AddMassTransit(configure =>
			{
				var method = configure.GetType().GetMethod(nameof(IServiceCollectionConfigurator.AddConsumer));
				var genericMethod = method.MakeGenericMethod(typeof(MessageConsumer));
				genericMethod.Invoke(configure, null);
			});

			services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
			{
				var hostAddress = "rabbitmq://localhost:5672";
				var host = cfg.Host(new Uri(hostAddress), hostConfigurator =>
				{
					hostConfigurator.Username("guest");
					hostConfigurator.Password("guest");
				});
				
				cfg.ReceiveEndpoint(host, "NotificationTestQueue", e => e.LoadFrom(provider));
			}));

			services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
			services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
			services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
		}
	}
}
