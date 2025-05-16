using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Message.Handlers.Configuration;

public static class RabbitMqMessageHandlersRegistration
{
    public static IServiceCollection AddRabbitMessageHandlers(this IServiceCollection services, ConfigurationManager configuration)
    {
        var rabbitSection = configuration.GetSection(nameof(RabbitMQSettings));
        var rabbitSettings = new RabbitMQSettings();
        rabbitSection.Bind(rabbitSettings);
        
        services.AddMassTransit(x =>
        {
            foreach (var queue in rabbitSettings.Queues)
            {
                var consumerType = Type.GetType($"Message.Handlers.Handlers.{queue.Consumer}");
                if (consumerType != null)
                {
                    x.AddConsumer(consumerType);
                }
            }

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitSettings.Host, rabbitSettings.Port, "/", h =>
                {
                    h.Username(rabbitSettings.Username);
                    h.Password(rabbitSettings.Password);
                });


                cfg.UseRawJsonSerializer();

                foreach (var queue in rabbitSettings.Queues)
                {
                    var consumerType = Type.GetType($"Message.Handlers.Handlers.{queue.Consumer}");

                    if (consumerType != null)
                    {
                        cfg.ReceiveEndpoint(queue.QueueName, e =>
                        {
                            e.ConfigureConsumeTopology = false;
                            e.Bind(queue.Exchange, s =>
                            {
                                s.RoutingKey = queue.RoutingKey;
                                s.ExchangeType = queue.ExchangeType;
                            });

                            e.ConfigureConsumer(context, consumerType);
                        });
                    }
                }
            });
        });

        return services;
    }
}