using System.Net.Mime;

namespace BlogerHub.Models;

public class Connection
{
    public string FollowerId { get; set; }
    public string FollowedId { get; set; }
    public ApplicationUser Follower { get; set; }
    public ApplicationUser Followed { get; set; }
}