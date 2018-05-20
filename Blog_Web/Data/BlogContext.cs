using Blog_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog_Web.Data
{
    public class BlogContext:DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tally> Tallys { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>().ToTable("Comment");
            modelBuilder.Entity<Tally>().ToTable("Tally");
            modelBuilder.Entity<Blog>().ToTable("Blog");
            modelBuilder.Entity<Administrator>().ToTable("Administrator");
            modelBuilder.Entity<Contact>().ToTable("Contact");
        }
    }
}
