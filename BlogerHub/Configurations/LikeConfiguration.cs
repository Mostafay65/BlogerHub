using BlogerHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogerHub.Configurations;

public class LikeConfiguration : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder.HasKey(l => l.Id);
        builder.HasOne(l => l.Author)
            .WithMany(u => u.Likes)
            .HasForeignKey(l=>l.AuthorId);
        
        builder.HasOne(l => l.Blog)
            .WithMany(b => b.Likes)
            .HasForeignKey(l=>l.BlogId);
    }
}