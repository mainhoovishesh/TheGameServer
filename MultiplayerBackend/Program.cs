using MultiplayerBackend.CacheServices;
using MultiplayerBackend.ClientCommunication.Services;
using MultiplayerBackend.InternalCommunication.Interfaces;
using MultiplayerBackend.InternalCommunication.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Register services with the DI container
builder.Services.AddSingleton<MessageHandlerService>();
builder.Services.AddSingleton<DataServices>();
builder.Services.AddSingleton<WebSocketCommunicationService>();
builder.Services.AddSingleton<IMessageParserService, JsonMessageParsingService>();

// Add controllers
builder.Services.AddControllers();

var app = builder.Build();

// Use WebSocket middleware
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};
app.UseWebSockets(webSocketOptions);

// Use custom middleware for handling WebSocket requests
app.UseMiddleware<MessageHandlerService>();

app.UseRouting();

// Add endpoints for controllers
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

// Start a background thread for sending periodic messages
var serviceProvider = app.Services;
Thread thread = new Thread(async () =>
{
    using (var scope = serviceProvider.CreateScope())
    {
        var webSocketCommunicationService = scope.ServiceProvider.GetRequiredService<WebSocketCommunicationService>();

        while (true)
        {
            await Task.Delay(2000);
            Console.WriteLine("Sending Hello");
            await webSocketCommunicationService.SendBroadcastMessage("Hello");
        }
    }
});

thread.Start();
