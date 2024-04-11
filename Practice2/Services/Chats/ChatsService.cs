using Microsoft.EntityFrameworkCore;

public class ChatsService : IChatsService
{
    private readonly DataBase _context;
    private readonly ILogger<ChatsService> _logger;

    public ChatsService(DataBase context, ILogger<ChatsService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Create(CreateChatModel body)
    {
        var chat = new Chat
        {
            Name = string.IsNullOrEmpty(body.Name) ? "Default chat" : body.Name
        };

        await _context.Chats.AddAsync(chat);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Leave(DeleteChatModel body, string Token)
    {
        var chat = await _context.Chats.FindAsync(body.chatId);
        if (chat == null)
        {
            throw new ChatNotFoundException();
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Token == Token);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        var existingChatUser = await _context.ChatUsers.FirstOrDefaultAsync(cu => cu.UserId == user.Id && cu.ChatId == chat.Id);
        if (existingChatUser == null)
        {
            throw new UserNotAccessException();
        }

        _context.ChatUsers.Remove(existingChatUser);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Join(string token, int chatId)
    {
        var chat = await _context.Chats.FindAsync(chatId);
        if (chat == null)
        {
            throw new ChatNotFoundException();
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Token == token);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        var existingChatUser = await _context.ChatUsers.FirstOrDefaultAsync(cu => cu.UserId == user.Id && cu.ChatId == chat.Id);
        if (existingChatUser != null)
        {
            throw new UserNotFoundException("User already in chat");
        }

        using (var transaction = await _context.Database.BeginTransactionAsync())
        {

            _context.ChatUsers.Add(new ChatUser { UserId = user.Id, ChatId = chat.Id });
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return true;

        }
    }

    public async Task<List<object>> GetAll(string token)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Token == token);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        var userChats = await _context.ChatUsers
            .Where(cu => cu.UserId == user.Id)
            .Select(cu => new { id = cu.Chat.Id, name = cu.Chat.Name })
            .ToListAsync();

        return userChats.Cast<object>().ToList();
    }
}