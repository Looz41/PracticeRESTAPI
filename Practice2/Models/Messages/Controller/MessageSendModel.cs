using System.ComponentModel.DataAnnotations;

public class MessageSendModel
{
    [Required]
    public int ChatId { get; set; }
    [Required]
    public string Message { get; set; }
}