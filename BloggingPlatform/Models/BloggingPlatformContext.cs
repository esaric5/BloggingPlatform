using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BloggingPlatform.Models
{
    public partial class BloggingPlatformContext : DbContext
    {
        public BloggingPlatformContext()
        {
        }

        public BloggingPlatformContext(DbContextOptions<BloggingPlatformContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<PostTag> PostTag { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.PostBody)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.PostCreated).HasColumnType("datetime");

                entity.Property(e => e.PostDescription)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.PostIdentifier).HasMaxLength(500);

                entity.Property(e => e.PostTitle)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.PostUpdated).HasColumnType("datetime");
            });

            modelBuilder.Entity<PostTag>(entity =>
            {
                entity.HasKey(e => new { e.PostId, e.TagId });

                entity.ToTable("Post_Tag");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostTag)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Post_Tag_Post");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.PostTag)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Post_Tag_Tag");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.TagName)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}
