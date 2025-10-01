using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Library_Management_Sys.Models
{
    public class LibraryDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }
        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<BorrowTransaction> BorrowTransactions { get; set; }
        public DbSet<UserActivityLog> UserActivityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasMany(u => u.ActivityLogs)
                .WithOne(ul => ul.user)
                .HasForeignKey(ul => ul.UserId);

            builder.Entity<Book>().HasMany(b => b.Authors).WithMany(a => a.Books).UsingEntity(j => j.ToTable("BookAuthors"));
            builder.Entity<Book>().HasOne(b => b.Category).WithMany(c => c.Books).HasForeignKey(b => b.CategoryId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Book>().HasOne(b => b.Publisher).WithMany(p => p.Books).HasForeignKey(b => b.PublisherId).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<BorrowTransaction>().HasOne(bt => bt.book)
                .WithMany(b => b.BorrowTransactions)
                .HasForeignKey(bt => bt.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<BorrowTransaction>().HasOne(bt => bt.member).WithMany(m => m.BorrowTransactions).HasForeignKey(bt => bt.MemberId).OnDelete(DeleteBehavior.Restrict); ;

            builder.Entity<Category>().HasOne(c => c.ParentCategory).WithMany(c => c.SubCategories).HasForeignKey(c => c.ParentCategoryId).OnDelete(DeleteBehavior.Restrict); ;

            builder.Entity<UserActivityLog>().ToTable("UserActivityLogs");

            // Seed roles using IdentityRole<Guid> with static ConcurrencyStamp values
            builder.Entity<IdentityRole<Guid>>().HasData(
                new IdentityRole<Guid>
                {
                    Id = Guid.Parse("1a111111-1111-1111-1111-111111111111"),
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "1b111111-1111-1111-1111-111111111111"
                },
                new IdentityRole<Guid>
                {
                    Id = Guid.Parse("2a222222-2222-2222-2222-222222222222"),
                    Name = "Librarian",
                    NormalizedName = "LIBRARIAN",
                    ConcurrencyStamp = "2b222222-2222-2222-2222-222222222222"
                },
                new IdentityRole<Guid>
                {
                    Id = Guid.Parse("3a333333-3333-3333-3333-333333333333"),
                    Name = "Staff",
                    NormalizedName = "STAFF",
                    ConcurrencyStamp = "3b333333-3333-3333-3333-333333333333"
                }
            );
        }


    }
}
