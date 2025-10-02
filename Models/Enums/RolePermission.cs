namespace Library_Management_Sys.Models.Enums
{
    public static class RolePermission
    {
        public const Permissions None = Permissions.None;
        //Admin can manage everything
        public const Permissions Admin = Permissions.Books_Create | Permissions.Books_Update | Permissions.Books_Delete | Permissions.Books_View |
         Permissions.Members_Create | Permissions.Members_Update | Permissions.Members_Delete | Permissions.Members_View |
         Permissions.BorrowTransactions_Create | Permissions.BorrowTransactions_Update | Permissions.BorrowTransactions_Delete | Permissions.BorrowTransactions_View |
         Permissions.Authors_Create | Permissions.Authors_Update | Permissions.Authors_Delete | Permissions.Authors_View |
         Permissions.Categories_Create | Permissions.Categories_Update | Permissions.Categories_Delete | Permissions.Categories_View |
         Permissions.Publishers_Create | Permissions.Publishers_Update | Permissions.Publishers_Delete | Permissions.Publishers_View;

         //Librarian can manage books and borrow transactions and manage members
        public const Permissions Librarian = Permissions.Books_Create | Permissions.Books_Update | Permissions.Books_Delete | Permissions.Books_View |
             Permissions.BorrowTransactions_Create | Permissions.BorrowTransactions_Update | Permissions.BorrowTransactions_Delete | Permissions.BorrowTransactions_View |
             Permissions.Members_View | Permissions.Members_Create | Permissions.Members_Update;

         //Member can only view books and their own borrow transactions
        public const Permissions Staff = Permissions.Members_Create | Permissions.Members_Update | Permissions.Members_View |
             Permissions.BorrowTransactions_Create | Permissions.BorrowTransactions_Update | Permissions.BorrowTransactions_View | Permissions.Books_View | Permissions.Authors_View | Permissions.Categories_View | Permissions.Publishers_View;

    }
}
