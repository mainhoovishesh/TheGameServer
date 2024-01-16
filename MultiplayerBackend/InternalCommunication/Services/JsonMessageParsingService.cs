using MultiplayerBackend.InternalCommunication.Interfaces;
using System.Net.WebSockets;

namespace MultiplayerBackend.InternalCommunication.Services
{
    public class JsonMessageParsingService : IMessageParserService
    {
        public Task ParseMessage(WebSocket webSocket, string message)
        {
            throw new NotImplementedException();
        }

        public void PassMessageToService(IService service, string message)
        {
            throw new NotImplementedException();
        }
    }
}
