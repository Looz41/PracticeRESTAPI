using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.HasKey(c => c.Id);

        builder
            .HasMany(c => c.Users)
            .WithMany(u => u.Chats)
            .UsingEntity<ChatUser>(
                j => j
                    .HasOne(cu => cu.User)
                    .WithMany(u => u.ChatUsers)
                    .HasForeignKey(cu => cu.UserId),
                j => j
                    .HasOne(cu => cu.Chat)
                    .WithMany(c => c.ChatUsers)
                    .HasForeignKey(cu => cu.ChatId)
            );
    }
}
