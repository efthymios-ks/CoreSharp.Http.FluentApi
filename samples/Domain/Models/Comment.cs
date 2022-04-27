namespace Samples.Domain.Models
{
    public class Comment
    {
        //Properties 
        public int Id { get; set; }
        public string PostId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
    }
}
