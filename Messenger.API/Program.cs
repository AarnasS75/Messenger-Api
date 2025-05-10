using Messenger.Application.Configuration;
using Messenger.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.Configure<NotificationProvidersSettings>(
        builder.Configuration.GetSection(NotificationProvidersSettings.SectionName));

    builder.Services
        .RegisterApplicationServices()
        .RegisterInfrastructureServices();
    
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
