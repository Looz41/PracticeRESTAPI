using System.ComponentModel.DataAnnotations;

public class MessageDeleteModel
{
    [Required]
    public int ChatId { get; set; }
    [Required]
    public int MessageId { get; set; }
}