using Microsoft.EntityFrameworkCore;

public class MessagesService : IMessagesService
{
    private readonly DataBase _context;

    public MessagesService(DataBase context)
    {
        _context = context;
    }

    public async Task<bool> Send(MessageSendModel body, string Token)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Token == Token);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        var chat = await _context.Chats.FirstOrDefaultAsync(u => u.Id == body.ChatId);
        if (chat == null)
        {
            throw new ChatNotFoundException();
        }

        var isUserInChat = await _context.ChatUsers.AnyAsync(cu => cu.UserId == user.Id && cu.ChatId == chat.Id);
        if (!isUserInChat)
        {
            throw new UserNotAccessException();
        }

        var message = new Message
        {
            Text = body.Message,
            ChatId = chat.Id,
            SenderId = user.Id,
            Timestamp = DateTime.UtcNow,
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return true;
    }

    public class MessageInfo
    {
        public int messageId {  get; set; }
        public int senderId { get; set; }
        public string text { get; set; }
        public DateTime time { get; set; }
    }

    public async Task<List<MessageInfo>> GetMessages(int chatId, string token)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Token == token);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        var chat = await _context.Chats.FirstOrDefaultAsync(c => c.Id == chatId);
        if (chat == null)
        {
            throw new ChatNotFoundException();
        }

        var isUserInChat = await _context.ChatUsers.AnyAsync(cu => cu.UserId == user.Id && cu.ChatId == chat.Id);
        if (!isUserInChat)
        {
            throw new UserNotAccessException();
        }

        var messages = await _context.Messages
            .Where(m => m.ChatId == chat.Id)
            .OrderBy(m => m.Timestamp)
            .ToListAsync();

        var messageInfos = messages.Select(m => new MessageInfo
        {
            messageId = m.Id,
            senderId = m.SenderId,
            text = m.Text,
            time = m.Timestamp
        }).ToList();

        return messageInfos;
    }

    public async Task<bool> DeleteMessage(MessageDeleteModel body,string Token)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Token == Token);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        var chat = await _context.Chats.FirstOrDefaultAsync(c => c.Id == body.ChatId);
        if (chat == null)
        {
            throw new ChatNotFoundException();
        }

        var isUserInChat = await _context.ChatUsers.AnyAsync(cu => cu.UserId == user.Id && cu.ChatId == chat.Id);
        if (!isUserInChat)
        {
            throw new UserNotAccessException();
        }

        var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == body.MessageId);
        if (message == null)
        {
            throw new MessageNotFoundException();
        }

        if (message.SenderId != user.Id)
        {
            throw new UserNotAccessException();
        }

        _context.Messages.Remove(message);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAllUserMessages(MessageDeleteAllModel body, string Token)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Token == Token);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        var chat = await _context.Chats.FirstOrDefaultAsync(c => c.Id == body.ChatId);
        if (chat == null)
        {
            throw new ChatNotFoundException();
        }

        var isUserInChat = await _context.ChatUsers.AnyAsync(cu => cu.UserId == user.Id && cu.ChatId == chat.Id);
        if (!isUserInChat)
        {
            throw new UserNotAccessException();
        }

        var userMessages = await _context.Messages
            .Where(m => m.ChatId == chat.Id && m.SenderId == user.Id)
            .ToListAsync();

        if (userMessages.Any())
        {
            _context.Messages.RemoveRange(userMessages);
            await _context.SaveChangesAsync();
        }

        return true;
    }
}
