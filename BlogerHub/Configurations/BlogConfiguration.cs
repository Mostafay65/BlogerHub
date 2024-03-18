using BlogerHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogerHub.Configurations;

public class BlogConfiguration :  IEntityTypeConfiguration<Blog>
{
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
        builder.ToTable("Blogs");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Title).HasMaxLength(50);
        builder.Property(b => b.Content).HasMaxLength(500);
        
        builder.HasOne(b => b.Author)
            .WithMany(u => u.Blogs)
            .HasForeignKey(b => b.AuthorId);
        
    }
}