using Microsoft.AspNetCore.Mvc;

public interface IChatsService
{
    public Task<bool> Create(CreateChatModel body);
    public Task<bool> Leave(DeleteChatModel body, string Token);
    public Task<bool> Join([FromHeader] string Token, [FromBody] int ChatId);
    public Task<List<object>> GetAll(string token);
}