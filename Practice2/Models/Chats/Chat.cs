public class Chat
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<ChatUser> ChatUsers { get; set; } = new List<ChatUser>();
    public List<User> Users => ChatUsers.Select(cu => cu.User).ToList();
    public List<Message> Messages { get; set; } = new List<Message>();
}
