using Microsoft.AspNetCore.Identity;

namespace BlogerHub.Models;

public class ApplicationUser : IdentityUser
{
    public DateOnly CreatedAt { get; set; }
    
    public List<Blog> Blogs { get; set; }
    public List<Comment> Comments { get; set; }
    public List<Like> Likes { get; set; }
    public List<ApplicationUser> Follwers { get; set; }
    public List<ApplicationUser> Follwing { get; set; }
}