using Message.Handlers.Configuration;
using Messenger.Application.Configuration;
using Messenger.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.Configure<NotificationsConfiguration>(
        builder.Configuration.GetSection(NotificationsConfiguration.SectionName));

    builder.Services
        .RegisterApplicationServices()
        .RegisterInfrastructureServices()
        .AddEventBusHandlers();
    
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.MapControllers();

    app.Run();
}
