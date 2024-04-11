using static MessagesService;

public interface IMessagesService
{
    public Task<bool> Send(MessageSendModel body, string Token);

    public Task<List<MessageInfo>> GetMessages(int ChatId, string Token);

    public Task<bool> DeleteMessage(MessageDeleteModel body, string Token);

    public Task<bool> DeleteAllUserMessages(MessageDeleteAllModel body, string Token);
}
