using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using static MessagesService;

namespace Practice2.Controllers
{
    [ApiController]

    public class MessagesResponse
    {
        [JsonPropertyName("messages")]
        public List<MessageInfo> Messages { get; set; }
    }
    public class MessagesController : Controller
    {
        private IMessagesService _messagesService;

        public MessagesController(IMessagesService messagesService)
        {
            _messagesService = messagesService;
        }

        [HttpPost("[controller]/[action]")]
        public async Task<IActionResult> Send([FromBody] MessageSendModel model, [FromHeader] [Required] string Token)
        {
            try
            {

            var result = await _messagesService.Send(model, Token);
            return Ok(new { result = result });
            }

            catch (UserNotAccessException ex)
            {
                return StatusCode(403, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("[controller]/[action]")]
        public async Task<IActionResult> Get([FromHeader] int ChatId, [FromHeader] string Token)
        {
            try
            {
                var result = await _messagesService.GetMessages(ChatId, Token);

                var messagesResponse = new MessagesResponse { Messages = result };

                string json = JsonSerializer.Serialize(messagesResponse, new JsonSerializerOptions
                {
                    IgnoreNullValues = true,
                    WriteIndented = true
                });

                json = json.Replace("$id", "id").Replace("$values", "messages");

                return Content(json, "application/json");
            }
            catch (UserNotFoundException ex)
            {
                var errorJson = JsonSerializer.Serialize(new { error = ex.Message });
                return Content(errorJson, "application/json");
            }
            catch (ChatNotFoundException ex)
            {
                var errorJson = JsonSerializer.Serialize(new { error = ex.Message });
                return Content(errorJson, "application/json");
            }
            catch (UserNotAccessException ex)
            {
                var errorJson = JsonSerializer.Serialize(new { error = ex.Message });
                return Content(errorJson, "application/json");
            }
            catch (Exception ex)
            {
                var errorJson = JsonSerializer.Serialize(new { error = ex.Message });
                return Content(errorJson, "application/json");
            }
        }

        [HttpDelete("[controller]/[action]")]
        public async Task<IActionResult> Delete([FromBody] MessageDeleteModel model, [FromHeader] [Required] string Token)
        {
            try
            {
                var result = await _messagesService.DeleteMessage(model, Token);

                return Ok(new { result = result });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (ChatNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (UserNotAccessException ex)
            {
                return StatusCode(403, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete("[controller]/[action]")]
        public async Task<IActionResult> DeleteAll([FromBody] MessageDeleteAllModel model, [FromHeader][Required] string Token)
        {
            try
            {
                var result = await _messagesService.DeleteAllUserMessages(model, Token);

                return Ok(new { result = result });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (ChatNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (UserNotAccessException ex)
            {
                return StatusCode(403, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
