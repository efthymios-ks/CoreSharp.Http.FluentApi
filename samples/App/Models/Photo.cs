namespace App.Models;

public record Photo(int Id, int AlbumId, string? Title, string? Url, string? ThumbnailUrl);
