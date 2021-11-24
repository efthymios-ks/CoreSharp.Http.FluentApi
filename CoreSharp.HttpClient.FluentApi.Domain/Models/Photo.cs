namespace CoreSharp.HttpClient.FluentApi.Domain.Models
{
    public class Photo
    {
        //Properties 
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}
