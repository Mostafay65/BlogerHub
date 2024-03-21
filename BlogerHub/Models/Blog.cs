using System.ComponentModel.DataAnnotations;

namespace BlogerHub.Models;

public class Blog
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Title { get; set; }
    [MaxLength(5000)]
    public string Content { get; set; }
    public string? Media { get; set; }
    public string AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ApplicationUser Author { get; set; }
    public List<Comment>? Comments { get; set; }
    public List<Like>? Likes { get; set; }

}