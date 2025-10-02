using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Library_Management_Sys.Models.Enums;

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

            // Seed Publishers
            builder.Entity<Publisher>().HasData(
                new Publisher { PublisherId = 1, Name = "Penguin Random House" },
                new Publisher { PublisherId = 2, Name = "HarperCollins Publishers" },
                new Publisher { PublisherId = 3, Name = "Simon & Schuster" },
                new Publisher { PublisherId = 4, Name = "Macmillan Publishers" },
                new Publisher { PublisherId = 5, Name = "Hachette Book Group" },
                new Publisher { PublisherId = 6, Name = "Scholastic Corporation" },
                new Publisher { PublisherId = 7, Name = "Wiley" },
                new Publisher { PublisherId = 8, Name = "Pearson Education" },
                new Publisher { PublisherId = 9, Name = "Oxford University Press" },
                new Publisher { PublisherId = 10, Name = "Cambridge University Press" }
            );

            // Seed Authors
            builder.Entity<Author>().HasData(
                new Author { AuthorId = 1, Name = "J.K. Rowling", Bio = "British author best known for the Harry Potter fantasy series." },
                new Author { AuthorId = 2, Name = "Stephen King", Bio = "American author of horror, supernatural fiction, suspense, crime, science-fiction, and fantasy novels." },
                new Author { AuthorId = 3, Name = "Agatha Christie", Bio = "English writer known for her detective novels, particularly those featuring Hercule Poirot and Miss Marple." },
                new Author { AuthorId = 4, Name = "George Orwell", Bio = "English novelist and essayist, journalist and critic, whose work is characterised by lucid prose." },
                new Author { AuthorId = 5, Name = "Jane Austen", Bio = "English novelist known primarily for her six major novels which interpret the British landed gentry." },
                new Author { AuthorId = 6, Name = "Mark Twain", Bio = "American writer, humorist, entrepreneur, publisher, and lecturer." },
                new Author { AuthorId = 7, Name = "Charles Dickens", Bio = "English writer and social critic who created some of the world's best-known fictional characters." },
                new Author { AuthorId = 8, Name = "Ernest Hemingway", Bio = "American novelist, short-story writer, and journalist." },
                new Author { AuthorId = 9, Name = "F. Scott Fitzgerald", Bio = "American novelist and short story writer, widely regarded as one of the greatest American writers of the 20th century." },
                new Author { AuthorId = 10, Name = "Harper Lee", Bio = "American novelist widely known for To Kill a Mockingbird." },
                new Author { AuthorId = 11, Name = "J.R.R. Tolkien", Bio = "English author, poet, philologist, and university professor, best known for The Hobbit and The Lord of the Rings." },
                new Author { AuthorId = 12, Name = "Dan Brown", Bio = "American author best known for his thriller novels, including the Robert Langdon novels." },
                new Author { AuthorId = 13, Name = "Paulo Coelho", Bio = "Brazilian lyricist and novelist, best known for his novel The Alchemist." },
                new Author { AuthorId = 14, Name = "Gabriel García Márquez", Bio = "Colombian novelist, short-story writer, screenwriter, and journalist." },
                new Author { AuthorId = 15, Name = "Toni Morrison", Bio = "American novelist, essayist, book editor, and college professor." }
            );

            // Seed Categories
            builder.Entity<Category>().HasData(
                // Main categories
                new Category { CategoryId = 1, Name = "Fiction", ParentCategoryId = null },
                new Category { CategoryId = 2, Name = "Non-Fiction", ParentCategoryId = null },
                new Category { CategoryId = 3, Name = "Science & Technology", ParentCategoryId = null },
                new Category { CategoryId = 4, Name = "Arts & Literature", ParentCategoryId = null },
                new Category { CategoryId = 5, Name = "History & Biography", ParentCategoryId = null },
                
                // Fiction subcategories
                new Category { CategoryId = 6, Name = "Fantasy", ParentCategoryId = 1 },
                new Category { CategoryId = 7, Name = "Mystery & Thriller", ParentCategoryId = 1 },
                new Category { CategoryId = 8, Name = "Romance", ParentCategoryId = 1 },
                new Category { CategoryId = 9, Name = "Science Fiction", ParentCategoryId = 1 },
                new Category { CategoryId = 10, Name = "Horror", ParentCategoryId = 1 },
                
                // Non-Fiction subcategories
                new Category { CategoryId = 11, Name = "Self-Help", ParentCategoryId = 2 },
                new Category { CategoryId = 12, Name = "Business", ParentCategoryId = 2 },
                new Category { CategoryId = 13, Name = "Health & Fitness", ParentCategoryId = 2 },
                new Category { CategoryId = 14, Name = "Travel", ParentCategoryId = 2 },
                new Category { CategoryId = 15, Name = "Religion & Spirituality", ParentCategoryId = 2 }
            );

            // Seed Books
            builder.Entity<Book>().HasData(
                new Book 
                { 
                    BookId = 1, 
                    Title = "Harry Potter and the Philosopher's Stone", 
                    ISBN = "9780747532699", 
                    Edition = "1st Edition", 
                    Year = 1997, 
                    Summary = "Harry Potter, an eleven-year-old orphan, discovers that he is a wizard and is invited to study at Hogwarts.", 
                    CoverImage = "harrypotter1.jpg", 
                    Language = "English", 
                    Status = BookStatus.inStock, 
                    CategoryId = 6, 
                    PublisherId = 1, 
                    AuthorId = 1 
                },
                new Book 
                { 
                    BookId = 2, 
                    Title = "The Shining", 
                    ISBN = "9780385121675", 
                    Edition = "1st Edition", 
                    Year = 1977, 
                    Summary = "A family heads to an isolated hotel for the winter where a sinister presence influences the father into violence.", 
                    CoverImage = "theshining.jpg", 
                    Language = "English", 
                    Status = BookStatus.inStock, 
                    CategoryId = 10, 
                    PublisherId = 2, 
                    AuthorId = 2 
                },
                new Book 
                { 
                    BookId = 3, 
                    Title = "Murder on the Orient Express", 
                    ISBN = "9780062693662", 
                    Edition = "Reprint Edition", 
                    Year = 1934, 
                    Summary = "Detective Hercule Poirot investigates a murder aboard the famous European train.", 
                    CoverImage = "orientexpress.jpg", 
                    Language = "English", 
                    Status = BookStatus.inStock, 
                    CategoryId = 7, 
                    PublisherId = 2, 
                    AuthorId = 3 
                },
                new Book 
                { 
                    BookId = 4, 
                    Title = "1984", 
                    ISBN = "9780451524935", 
                    Edition = "Signet Classic", 
                    Year = 1949, 
                    Summary = "In a totalitarian future society, Winston Smith works for the Ministry of Truth and falls in love with Julia.", 
                    CoverImage = "1984.jpg", 
                    Language = "English", 
                    Status = BookStatus.inStock, 
                    CategoryId = 9, 
                    PublisherId = 3, 
                    AuthorId = 4 
                },
                new Book 
                { 
                    BookId = 5, 
                    Title = "Pride and Prejudice", 
                    ISBN = "9780141439518", 
                    Edition = "Penguin Classics", 
                    Year = 1813, 
                    Summary = "The romantic clash between the opinionated Elizabeth Bennet and her proud beau, Mr. Darcy.", 
                    CoverImage = "prideandprejudice.jpg", 
                    Language = "English", 
                    Status = BookStatus.inStock, 
                    CategoryId = 8, 
                    PublisherId = 1, 
                    AuthorId = 5 
                },
                new Book 
                { 
                    BookId = 6, 
                    Title = "The Adventures of Tom Sawyer", 
                    ISBN = "9780486400778", 
                    Edition = "Dover Thrift Editions", 
                    Year = 1876, 
                    Summary = "The adventures of a young boy growing up along the Mississippi River.", 
                    CoverImage = "tomsawyer.jpg", 
                    Language = "English", 
                    Status = BookStatus.inStock, 
                    CategoryId = 1, 
                    PublisherId = 4, 
                    AuthorId = 6 
                },
                new Book 
                { 
                    BookId = 7, 
                    Title = "A Tale of Two Cities", 
                    ISBN = "9780486406510", 
                    Edition = "Dover Thrift Editions", 
                    Year = 1859, 
                    Summary = "A historical novel set in London and Paris before and during the French Revolution.", 
                    CoverImage = "taleoftwocities.jpg", 
                    Language = "English", 
                    Status = BookStatus.inStock, 
                    CategoryId = 1, 
                    PublisherId = 4, 
                    AuthorId = 7 
                },
                new Book 
                { 
                    BookId = 8, 
                    Title = "The Old Man and the Sea", 
                    ISBN = "9780684801223", 
                    Edition = "Scribner", 
                    Year = 1952, 
                    Summary = "An aging Cuban fisherman struggles with a giant marlin far out in the Gulf Stream.", 
                    CoverImage = "oldmanandthesea.jpg", 
                    Language = "English", 
                    Status = BookStatus.inStock, 
                    CategoryId = 1, 
                    PublisherId = 3, 
                    AuthorId = 8 
                },
                new Book 
                { 
                    BookId = 9, 
                    Title = "The Great Gatsby", 
                    ISBN = "9780743273565", 
                    Edition = "Scribner", 
                    Year = 1925, 
                    Summary = "A classic novel about the Jazz Age in the United States, narrated by Nick Carraway.", 
                    CoverImage = "greatgatsby.jpg", 
                    Language = "English", 
                    Status = BookStatus.inStock, 
                    CategoryId = 1, 
                    PublisherId = 3, 
                    AuthorId = 9 
                },
                new Book 
                { 
                    BookId = 10, 
                    Title = "To Kill a Mockingbird", 
                    ISBN = "9780060935467", 
                    Edition = "Harper Perennial", 
                    Year = 1960, 
                    Summary = "A gripping tale of racial injustice and childhood innocence in the American South.", 
                    CoverImage = "tokillamockingbird.jpg", 
                    Language = "English", 
                    Status = BookStatus.inStock, 
                    CategoryId = 1, 
                    PublisherId = 2, 
                    AuthorId = 10 
                },
                new Book 
                { 
                    BookId = 11, 
                    Title = "The Hobbit", 
                    ISBN = "9780547928227", 
                    Edition = "Houghton Mifflin Harcourt", 
                    Year = 1937, 
                    Summary = "Bilbo Baggins enjoys a comfortable, unambitious life until the wizard Gandalf chooses him to take part in an adventure.", 
                    CoverImage = "thehobbit.jpg", 
                    Language = "English", 
                    Status = BookStatus.inStock, 
                    CategoryId = 6, 
                    PublisherId = 5, 
                    AuthorId = 11 
                },
                new Book 
                { 
                    BookId = 12, 
                    Title = "The Da Vinci Code", 
                    ISBN = "9780307474278", 
                    Edition = "Anchor Books", 
                    Year = 2003, 
                    Summary = "A mystery thriller that follows symbologist Robert Langdon as he investigates a murder in the Louvre Museum.", 
                    CoverImage = "davincicode.jpg", 
                    Language = "English", 
                    Status = BookStatus.inStock, 
                    CategoryId = 7, 
                    PublisherId = 1, 
                    AuthorId = 12 
                },
                new Book 
                { 
                    BookId = 13, 
                    Title = "The Alchemist", 
                    ISBN = "9780061122415", 
                    Edition = "HarperOne", 
                    Year = 1988, 
                    Summary = "A young Andalusian shepherd travels from Spain to Egypt in search of a treasure buried near the Pyramids.", 
                    CoverImage = "thealchemist.jpg", 
                    Language = "English", 
                    Status = BookStatus.inStock, 
                    CategoryId = 1, 
                    PublisherId = 2, 
                    AuthorId = 13 
                },
                new Book 
                { 
                    BookId = 14, 
                    Title = "One Hundred Years of Solitude", 
                    ISBN = "9780060883287", 
                    Edition = "Harper Perennial", 
                    Year = 1967, 
                    Summary = "The multi-generational story of the Buendía family, whose patriarch founded the town of Macondo.", 
                    CoverImage = "onehundredyears.jpg", 
                    Language = "English", 
                    Status = BookStatus.inStock, 
                    CategoryId = 1, 
                    PublisherId = 2, 
                    AuthorId = 14 
                },
                new Book 
                { 
                    BookId = 15, 
                    Title = "Beloved", 
                    ISBN = "9781400033416", 
                    Edition = "Vintage Books", 
                    Year = 1987, 
                    Summary = "A novel about the supernatural effects of slavery and trauma on Sethe and her family.", 
                    CoverImage = "beloved.jpg", 
                    Language = "English", 
                    Status = BookStatus.inStock, 
                    CategoryId = 1, 
                    PublisherId = 1, 
                    AuthorId = 15 
                }
            );
        }
    }
}
