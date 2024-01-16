namespace MultiplayerBackend.ClientCommunication.Interfaces
{
    public interface ISocketCommunication<TClient>
    {
        Task SendMessageToAClient(TClient client, string message);
        Task SendBroadcastMessage(IEnumerable<TClient> clients, string message);
    }
}
