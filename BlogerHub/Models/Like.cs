namespace BlogerHub.Models;

public class Like
{
    public int Id { get; set; }
    public string AuthorId { get; set; }
    public int BlogId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Blog Blog { get; set; }
    public ApplicationUser Author { get; set; }
}