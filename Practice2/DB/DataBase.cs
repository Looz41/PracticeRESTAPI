using Microsoft.EntityFrameworkCore;

public class DataBase : DbContext, IDataBase
{
    public DbSet<User> Users { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ChatUser> ChatUsers { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DataBase(DbContextOptions<DataBase> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new ChatConfiguration());
        modelBuilder.ApplyConfiguration(new ChatUserConfiguration());
    }

    public Task SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }
}
