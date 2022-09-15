namespace Domain.Models;

public class Post
{
    // Properties 
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
}
