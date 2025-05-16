using Message.Handlers.Configuration;
using Messenger.API.Common.Errors;
using Messenger.API.Common.Mapping;
using Messenger.Application.Configuration;
using Messenger.Infrastructure.Configuration;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    var configsLocation = "Configuration/";
    builder.Configuration
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile($"{configsLocation}appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"{configsLocation}rabbitmq.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"{configsLocation}awsconfiguration.json", optional: false, reloadOnChange: true);
    
    builder.Services.Configure<NotificationsConfiguration>(
        builder.Configuration.GetSection(nameof(NotificationsConfiguration)));

    builder.Services
        .AddMapping()
        .RegisterApplicationServices()
        .RegisterInfrastructureServices(builder.Configuration)
        .AddRabbitMessageHandlers(builder.Configuration);
    
    builder.Services.AddControllers();
    builder.Services.AddSingleton<ProblemDetailsFactory, MessengerProblemDetailsFactory>();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.MapControllers();

    app.Run();
}
