using MultiplayerBackend.InternalCommunication.Interfaces;
using System.Net.WebSockets;
using System.Text;
using MultiplayerBackend.CacheServices;
using Microsoft.AspNetCore.Http;

namespace MultiplayerBackend.ClientCommunication.Services
{
    public class MessageHandlerService : IMiddleware
    {
        private readonly IMessageParserService _messageParserService;
        private readonly DataServices _dataServices;

        public MessageHandlerService(IMessageParserService messageParserService, DataServices dataServices)
        {
            _messageParserService = messageParserService;
            _dataServices = dataServices;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                _dataServices.AddUserData(webSocket, new UserData()); // Add the connection to the users dictionary
                await HandleWebSocketRequest(context, webSocket);
            }
            else
            {
                context.Response.StatusCode = 400;
            }

            if (!context.Response.HasStarted)
            {
                await next(context);
            }
        }

        private async Task HandleWebSocketRequest(HttpContext context, WebSocket webSocket)
        {
            try
            {
                await ProcessWebSocketMessages(webSocket);
            }
            catch (Exception ex)
            {
                await HandleWebSocketClosure(webSocket, ex);
            }
        }

        private async Task ProcessWebSocketMessages(WebSocket webSocket)
        {
            while (webSocket.State == WebSocketState.Open)
            {
                byte[] buffer = new byte[1024 * 4];
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                string messageReceived = Encoding.UTF8.GetString(buffer, 0, result.Count);

                // await _messageParserService.ParseMessage(messageReceived, webSocket);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await HandleWebSocketClosure(webSocket, null);
                }
            }

            await HandleWebSocketClosure(webSocket, null);
        }

        private async Task HandleWebSocketClosure(WebSocket webSocket, Exception ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }

            _dataServices.RemoveUserData(webSocket); // Remove the connection from the users dictionary
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Socket closed", CancellationToken.None);
        }
    }
}
