public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public List<ChatUser> ChatUsers { get; set; } = new List<ChatUser>();
    public List<Chat> Chats => ChatUsers.Select(cu => cu.Chat).ToList();
}
