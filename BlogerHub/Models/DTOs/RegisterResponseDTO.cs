namespace BlogerHub.Models.DTOs;

public class RegisterResponseDTO
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public UserDTO? User { get; set; }
}