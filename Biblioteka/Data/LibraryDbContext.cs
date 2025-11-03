using Biblioteka.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Biblioteka.Data
{
    public class LibraryDbContext : IdentityDbContext<ApplicationUser>
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BookTag> BookTags { get; set; }
        public DbSet<BookFile> BookFiles { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<QueueEntry> QueueEntries { get; set; }
        public DbSet<SearchHistory> SearchHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BookTag>()
                .HasKey(bt => new { bt.BookId, bt.TagId });

            builder.Entity<Rental>()
                .Property(r => r.FineAmount)
                .HasColumnType("decimal(18,2)");


            builder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Book>()
                .HasIndex(b => b.Title);
            builder.Entity<Book>()
                .HasIndex(b => b.Author);
            builder.Entity<Book>()
                .HasIndex(b => b.ISBN);

            builder.Entity<Rental>()
                .Property(r => r.State)
                .HasConversion<string>();
        }
    }
}
