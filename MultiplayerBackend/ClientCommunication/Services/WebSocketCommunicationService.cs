using MultiplayerBackend.ClientCommunication.Interfaces;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace MultiplayerBackend.ClientCommunication.Services
{
    public class WebSocketCommunicationService : ISocketCommunication<WebSocket>
    {
        public async Task SendMessageToAClient(WebSocket client, string message)
        {
            try
            {
                var buffer = Encoding.UTF8.GetBytes(message);
                await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Console.WriteLine(JsonConvert.SerializeObject(ex.ToString()));
            }
        }

        public async Task SendBroadcastMessage(IEnumerable<WebSocket> clients, string message)
        {
            foreach (var clientSocket in clients)
            {
                try
                {
                    var buffer = Encoding.UTF8.GetBytes(message);
                    await clientSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    // Log or handle the exception appropriately
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
