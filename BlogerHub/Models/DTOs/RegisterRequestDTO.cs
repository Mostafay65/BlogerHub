namespace BlogerHub.Models.DTOs;

public class RegisterRequestDTO
{
    public string UserName  { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}