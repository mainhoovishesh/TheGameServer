using MultiplayerBackend.CacheServices;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiplayerBackend.ClientCommunication.Services
{
    public class WebSocketCommunicationService
    {
        private readonly DataServices _dataServices;

        public WebSocketCommunicationService(DataServices dataServices)
        {
            _dataServices = dataServices;
        }

        public static async Task SendMessageToAClient(WebSocket client, string message)
        {
            try
            {
                var buffer = Encoding.UTF8.GetBytes(message);
                await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }
        }

        public async Task SendBroadcastMessage(string message)
        {
            foreach (var clientSocket in _dataServices.GetConnections())
            {
                try
                {
                    var buffer = Encoding.UTF8.GetBytes(message);
                    await clientSocket.Key.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(ex));
                }
            }
        }
    }
}
