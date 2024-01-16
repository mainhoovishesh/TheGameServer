using System.Net.WebSockets;

namespace MultiplayerBackend.InternalCommunication.Interfaces
{
    public interface IMessageParserService
    {
        public Task ParseMessage(WebSocket webSocket, string message);

        public void PassMessageToService(IService service, string message);
    }
}
