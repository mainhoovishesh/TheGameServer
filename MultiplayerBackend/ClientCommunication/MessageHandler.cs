
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;

namespace MultiplayerBackend.ClientCommunication
{
    public class MessageHandler : IMiddleware
    {

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    await HandleWebSocketRequest(context);
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
            catch (WebSocketException webSocketEx)
            {
                Console.WriteLine($"WebSocketException: {webSocketEx.Message}\nStackTrace: {webSocketEx.StackTrace}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }

        private async Task HandleWebSocketRequest(HttpContext context)
        {
            using (WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync())
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
        }

        private async Task ProcessWebSocketMessages(WebSocket webSocket)
        {
            while (webSocket.State == WebSocketState.Open)
            {
                byte[] buffer = new byte[1024 * 4];
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), default);

                string messageReceived = Encoding.UTF8.GetString(buffer, 0, result.Count);

                

                await messageParserService.MessageParser(messageReceived, webSocket);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await HandleWebSocketClosure(webSocket);
                }
            }

            await HandleWebSocketClosure(webSocket);
        }

        private async Task HandleWebSocketClosure(WebSocket webSocket, Exception ex = null)
        {
            SendClientLeftMessage(webSocket);

            if (ex != null)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }

            gameManager.clientSockets.TryRemove(webSocket, out _);
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Socket closed", CancellationToken.None);
        }

        private void SendClientLeftMessage(WebSocket webSocket)
        {
            if (gameManager.clientSockets.TryGetValue(webSocket, out var clientInfo))
            {
                var clientLeftMessage = new ClientLeftMessage { playerID = clientInfo.playerID };
                var serverMessage = new ServerMessage { type = StringConstants.PlayerLeft, data = clientLeftMessage };
                string message = JsonConvert.SerializeObject(serverMessage);
                SendDataToAllClients(message);
            }
        }

    }
}
