using Microsoft.EntityFrameworkCore;

public interface IDataBase
{
    DbSet<User> Users { get; set; }
    DbSet<Chat> Chats { get; set; }
    DbSet<ChatUser> ChatUsers { get; set; }

    DbSet<Message> Messages { get; set; }
    Task SaveChangesAsync();
}