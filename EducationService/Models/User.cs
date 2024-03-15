namespace EducationService.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string HashedPassword { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}