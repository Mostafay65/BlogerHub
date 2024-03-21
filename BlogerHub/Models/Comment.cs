using System.ComponentModel.DataAnnotations;

namespace BlogerHub.Models;

public class Comment
{
    public int Id { get; set; }
    [MaxLength(500)]
    public string Content { get; set; }
    public string AuthorId { get; set; }
    public int BlogId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Blog Blog { get; set; }
    public ApplicationUser Author { get; set; }

}