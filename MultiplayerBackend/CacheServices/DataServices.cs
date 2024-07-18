using System.Net.WebSockets;

namespace MultiplayerBackend.CacheServices
{
    public class DataServices
    {

        Dictionary<WebSocket, UserData> users = new Dictionary<WebSocket, UserData>();

        public DataServices()
        {

        }

        public void AddUserData(WebSocket webSocket, UserData userData)
        {
            users.Add(webSocket, userData);
        }

        public void UpdateUserData(WebSocket webSocket, UserData userData)
        {
            users[webSocket] = userData;
        }

        public void RemoveUserData(WebSocket webSocket)
        {
            users.Remove(webSocket);
        }

        public Dictionary<WebSocket, UserData> GetConnections()
        {
            return users;
        }
    }
}