namespace BlogerHub.Models.DTOs;

public class LoginResponseDTO
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public TokenDTO? Token { get; set; }
}