namespace App.Models;

public record User(int Id, string? Name, string? Username, string? Email, Address? Address, string? Phone, string? Website, Company? Company);
