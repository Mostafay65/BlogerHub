using BlogerHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogerHub.Configurations;

public class UserConfiguration :IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("ApplicationUsers");

        builder.HasMany(u => u.Follwers)
            .WithMany(u => u.Follwing)
            .UsingEntity<Connection>(c => c
                    .HasOne(c => c.Follower)
                    .WithMany()
                    .HasForeignKey(c => c.FollowerId)
                    .OnDelete(DeleteBehavior.Cascade),
                c => c
                    .HasOne(c => c.Followed)
                    .WithMany()
                    .HasForeignKey(c => c.FollowedId)
                    .OnDelete(DeleteBehavior.Cascade),
                c => c.HasKey(c => new { c.FollowerId, c.FollowedId })
            );

    }
}