namespace App.Models;

public sealed record Address(string? Street, string? Suite, string? City, string? Zipcode, Geo? Geo);
