namespace MultiplayerBackend.DatabaseCommunication.Interfaces
{
    public interface IDBService
    {
        public void ConnectToDatabase(string connectionString);
        public string GetDatabaseValues(string query);
        public string SetDatabaseValues(string query);
    }
}
