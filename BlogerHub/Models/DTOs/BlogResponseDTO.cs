namespace BlogerHub.Models.DTOs;

public class BlogResponseDTO
{
    public int  Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string? Media { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}