using Microsoft.AspNetCore.Mvc;

namespace Practice2.Controllers
{
    [ApiController]

    public class ChatsController : Controller
    {
        private IChatsService _chatsService;

        public ChatsController(IChatsService chatsService)
        {
            _chatsService = chatsService;
        }

        [HttpPost("[controller]/[action]")]
        public async Task<IActionResult> Create(CreateChatModel model)
        {
            var result = await _chatsService.Create(model);

            if (result == null)
            {
                return Ok(new
                {
                    error = "Error",
                    result = false
                });
            }
            return Ok(new { result = result });
        }

        [HttpDelete("[controller]/[action]")]
        public async Task<IActionResult> Leave([FromBody] DeleteChatModel model, [FromHeader] string Token)
        {
            try
            {
                var result = await _chatsService.Leave(model, Token);
                return Ok(new { result = result });
            } catch (UserNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            } catch (ChatNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            } catch (UserNotAccessException ex)
            {
                return StatusCode(409, new { error = ex.Message });
            } catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("[controller]/[action]")]
        public async Task<IActionResult> Join([FromHeader] string Token, [FromBody] JoinChatModel model)
        {
            try
            {
                var result = await _chatsService.Join(Token, model.ChatId);
                return Ok(new { result = result });
            } catch (UserNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            } catch (ChatNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            } catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("[controller]/[action]")]
        public async Task<IActionResult> GetAll([FromHeader] string Token)
        {
            try
            {
                var result = await _chatsService.GetAll(Token);
                return Ok(new { chats = result });
            } catch (UserNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            } catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
