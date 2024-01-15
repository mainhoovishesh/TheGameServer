namespace MultiplayerBackend.InternalCommunication.Interfaces
{
    public interface IMessageParserService
    {
        public Task ParseMessage(string Message);

        public void PassMessageToService(string Message, IService service);
    }
}
