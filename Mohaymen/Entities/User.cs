
using Mohaymen.Enums;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public StatusEnum Status { get; set; }
    public List<Message> SentMessages { get; set; } = [];
    public List<Message> ReceivedMessages { get; set; } = [];

}