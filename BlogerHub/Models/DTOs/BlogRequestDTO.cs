namespace BlogerHub.Models.DTOs;

public class BlogRequestDTO
{
    public string Title { get; set; }
    public string Content { get; set; }
    public IFormFile? Media { get; set; }
}