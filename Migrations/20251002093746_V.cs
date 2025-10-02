using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Library_Management_Sys.Migrations
{
    /// <inheritdoc />
    public partial class V : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.AuthorId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemberId);
                });

            migrationBuilder.CreateTable(
                name: "Publishers",
                columns: table => new
                {
                    PublisherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publishers", x => x.PublisherId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserActivityLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserActivityLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Edition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoverImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    PublisherId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                    table.ForeignKey(
                        name: "FK_Books_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Books_Publishers_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Publishers",
                        principalColumn: "PublisherId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookAuthors",
                columns: table => new
                {
                    AuthorsAuthorId = table.Column<int>(type: "int", nullable: false),
                    BooksBookId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAuthors", x => new { x.AuthorsAuthorId, x.BooksBookId });
                    table.ForeignKey(
                        name: "FK_BookAuthors_Authors_AuthorsAuthorId",
                        column: x => x.AuthorsAuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookAuthors_Books_BooksBookId",
                        column: x => x.BooksBookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BorrowTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    BorrowDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    BorrowTransactionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorrowTransactions_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BorrowTransactions_BorrowTransactions_BorrowTransactionId",
                        column: x => x.BorrowTransactionId,
                        principalTable: "BorrowTransactions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BorrowTransactions_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("1a111111-1111-1111-1111-111111111111"), "1b111111-1111-1111-1111-111111111111", "Admin", "ADMIN" },
                    { new Guid("2a222222-2222-2222-2222-222222222222"), "2b222222-2222-2222-2222-222222222222", "Librarian", "LIBRARIAN" },
                    { new Guid("3a333333-3333-3333-3333-333333333333"), "3b333333-3333-3333-3333-333333333333", "Staff", "STAFF" }
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "AuthorId", "Bio", "Name" },
                values: new object[,]
                {
                    { 1, "British author best known for the Harry Potter fantasy series.", "J.K. Rowling" },
                    { 2, "American author of horror, supernatural fiction, suspense, crime, science-fiction, and fantasy novels.", "Stephen King" },
                    { 3, "English writer known for her detective novels, particularly those featuring Hercule Poirot and Miss Marple.", "Agatha Christie" },
                    { 4, "English novelist and essayist, journalist and critic, whose work is characterised by lucid prose.", "George Orwell" },
                    { 5, "English novelist known primarily for her six major novels which interpret the British landed gentry.", "Jane Austen" },
                    { 6, "American writer, humorist, entrepreneur, publisher, and lecturer.", "Mark Twain" },
                    { 7, "English writer and social critic who created some of the world's best-known fictional characters.", "Charles Dickens" },
                    { 8, "American novelist, short-story writer, and journalist.", "Ernest Hemingway" },
                    { 9, "American novelist and short story writer, widely regarded as one of the greatest American writers of the 20th century.", "F. Scott Fitzgerald" },
                    { 10, "American novelist widely known for To Kill a Mockingbird.", "Harper Lee" },
                    { 11, "English author, poet, philologist, and university professor, best known for The Hobbit and The Lord of the Rings.", "J.R.R. Tolkien" },
                    { 12, "American author best known for his thriller novels, including the Robert Langdon novels.", "Dan Brown" },
                    { 13, "Brazilian lyricist and novelist, best known for his novel The Alchemist.", "Paulo Coelho" },
                    { 14, "Colombian novelist, short-story writer, screenwriter, and journalist.", "Gabriel García Márquez" },
                    { 15, "American novelist, essayist, book editor, and college professor.", "Toni Morrison" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Name", "ParentCategoryId" },
                values: new object[,]
                {
                    { 1, "Fiction", null },
                    { 2, "Non-Fiction", null },
                    { 3, "Science & Technology", null },
                    { 4, "Arts & Literature", null },
                    { 5, "History & Biography", null }
                });

            migrationBuilder.InsertData(
                table: "Publishers",
                columns: new[] { "PublisherId", "Name" },
                values: new object[,]
                {
                    { 1, "Penguin Random House" },
                    { 2, "HarperCollins Publishers" },
                    { 3, "Simon & Schuster" },
                    { 4, "Macmillan Publishers" },
                    { 5, "Hachette Book Group" },
                    { 6, "Scholastic Corporation" },
                    { 7, "Wiley" },
                    { 8, "Pearson Education" },
                    { 9, "Oxford University Press" },
                    { 10, "Cambridge University Press" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "AuthorId", "CategoryId", "CoverImage", "Edition", "ISBN", "Language", "PublisherId", "Status", "Summary", "Title", "Year" },
                values: new object[,]
                {
                    { 6, 6, 1, "tomsawyer.jpg", "Dover Thrift Editions", "9780486400778", "English", 4, 0, "The adventures of a young boy growing up along the Mississippi River.", "The Adventures of Tom Sawyer", 1876 },
                    { 7, 7, 1, "taleoftwocities.jpg", "Dover Thrift Editions", "9780486406510", "English", 4, 0, "A historical novel set in London and Paris before and during the French Revolution.", "A Tale of Two Cities", 1859 },
                    { 8, 8, 1, "oldmanandthesea.jpg", "Scribner", "9780684801223", "English", 3, 0, "An aging Cuban fisherman struggles with a giant marlin far out in the Gulf Stream.", "The Old Man and the Sea", 1952 },
                    { 9, 9, 1, "greatgatsby.jpg", "Scribner", "9780743273565", "English", 3, 0, "A classic novel about the Jazz Age in the United States, narrated by Nick Carraway.", "The Great Gatsby", 1925 },
                    { 10, 10, 1, "tokillamockingbird.jpg", "Harper Perennial", "9780060935467", "English", 2, 0, "A gripping tale of racial injustice and childhood innocence in the American South.", "To Kill a Mockingbird", 1960 },
                    { 13, 13, 1, "thealchemist.jpg", "HarperOne", "9780061122415", "English", 2, 0, "A young Andalusian shepherd travels from Spain to Egypt in search of a treasure buried near the Pyramids.", "The Alchemist", 1988 },
                    { 14, 14, 1, "onehundredyears.jpg", "Harper Perennial", "9780060883287", "English", 2, 0, "The multi-generational story of the Buendía family, whose patriarch founded the town of Macondo.", "One Hundred Years of Solitude", 1967 },
                    { 15, 15, 1, "beloved.jpg", "Vintage Books", "9781400033416", "English", 1, 0, "A novel about the supernatural effects of slavery and trauma on Sethe and her family.", "Beloved", 1987 }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Name", "ParentCategoryId" },
                values: new object[,]
                {
                    { 6, "Fantasy", 1 },
                    { 7, "Mystery & Thriller", 1 },
                    { 8, "Romance", 1 },
                    { 9, "Science Fiction", 1 },
                    { 10, "Horror", 1 },
                    { 11, "Self-Help", 2 },
                    { 12, "Business", 2 },
                    { 13, "Health & Fitness", 2 },
                    { 14, "Travel", 2 },
                    { 15, "Religion & Spirituality", 2 }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "AuthorId", "CategoryId", "CoverImage", "Edition", "ISBN", "Language", "PublisherId", "Status", "Summary", "Title", "Year" },
                values: new object[,]
                {
                    { 1, 1, 6, "harrypotter1.jpg", "1st Edition", "9780747532699", "English", 1, 0, "Harry Potter, an eleven-year-old orphan, discovers that he is a wizard and is invited to study at Hogwarts.", "Harry Potter and the Philosopher's Stone", 1997 },
                    { 2, 2, 10, "theshining.jpg", "1st Edition", "9780385121675", "English", 2, 0, "A family heads to an isolated hotel for the winter where a sinister presence influences the father into violence.", "The Shining", 1977 },
                    { 3, 3, 7, "orientexpress.jpg", "Reprint Edition", "9780062693662", "English", 2, 0, "Detective Hercule Poirot investigates a murder aboard the famous European train.", "Murder on the Orient Express", 1934 },
                    { 4, 4, 9, "1984.jpg", "Signet Classic", "9780451524935", "English", 3, 0, "In a totalitarian future society, Winston Smith works for the Ministry of Truth and falls in love with Julia.", "1984", 1949 },
                    { 5, 5, 8, "prideandprejudice.jpg", "Penguin Classics", "9780141439518", "English", 1, 0, "The romantic clash between the opinionated Elizabeth Bennet and her proud beau, Mr. Darcy.", "Pride and Prejudice", 1813 },
                    { 11, 11, 6, "thehobbit.jpg", "Houghton Mifflin Harcourt", "9780547928227", "English", 5, 0, "Bilbo Baggins enjoys a comfortable, unambitious life until the wizard Gandalf chooses him to take part in an adventure.", "The Hobbit", 1937 },
                    { 12, 12, 7, "davincicode.jpg", "Anchor Books", "9780307474278", "English", 1, 0, "A mystery thriller that follows symbologist Robert Langdon as he investigates a murder in the Louvre Museum.", "The Da Vinci Code", 2003 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthors_BooksBookId",
                table: "BookAuthors",
                column: "BooksBookId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_PublisherId",
                table: "Books",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowTransactions_BookId",
                table: "BorrowTransactions",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowTransactions_BorrowTransactionId",
                table: "BorrowTransactions",
                column: "BorrowTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowTransactions_MemberId",
                table: "BorrowTransactions",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentCategoryId",
                table: "Categories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivityLogs_UserId",
                table: "UserActivityLogs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BookAuthors");

            migrationBuilder.DropTable(
                name: "BorrowTransactions");

            migrationBuilder.DropTable(
                name: "UserActivityLogs");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Publishers");
        }
    }
}
