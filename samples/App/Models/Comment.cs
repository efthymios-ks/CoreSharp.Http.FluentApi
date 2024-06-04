namespace App.Models;

public record Comment(int Id, string? PostId, string? Name, string? Email, string? Body);
