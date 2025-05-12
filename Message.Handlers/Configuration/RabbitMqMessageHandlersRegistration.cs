using MassTransit;
using Message.Handlers.Contracts;
using Message.Handlers.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Message.Handlers.Configuration;

public static class RabbitMqMessageHandlersRegistration
{
    public static IServiceCollection AddRabbitMessageHandlers(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<UserAuthenticationEventHandler>();
            x.AddConsumer<ShipmentCompletedEventHandler>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/",h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                
                cfg.UseRawJsonSerializer();
                
                cfg.ReceiveEndpoint("Users.Exchange.UserAuthenticationEvent", e =>
                {
                    e.ConfigureConsumeTopology = false;
                    e.Bind("Users.Exchange", s =>
                    {
                        s.RoutingKey = "UserAuthenticationEvent";
                        s.ExchangeType = "topic";
                    });

                    e.ConfigureConsumer<UserAuthenticationEventHandler>(context);
                });
                
                cfg.ReceiveEndpoint("Orders.Exchange.ShipmentCompletedEvent", e =>
                {
                    e.ConfigureConsumeTopology = false;
                    e.Bind("Orders.Exchange", s =>
                    {
                        s.RoutingKey = "ShipmentCompletedEvent";
                        s.ExchangeType = "topic";
                    });

                    e.ConfigureConsumer<ShipmentCompletedEventHandler>(context);
                });
            });
        });

        return services;
    }
}