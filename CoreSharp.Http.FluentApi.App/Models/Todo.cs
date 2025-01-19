namespace App.Models;

public record Todo(int Id, int UserId, string? Title, bool Completed);
