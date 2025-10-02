namespace Library_Management_Sys.Models.Enums
{
   public enum BookStatus
   {
      inStock=0,
      outOfStock
   }

    public enum BorrowTransStatus
    {
        Borrowed=0,
        Returned,
        Overdue
    }


    public enum  Actions
    {
        Create=0,
        Update,
        Delete,
        insert,
        Borrow,
        Return

    }

    [Flags]
    public enum Permissions
    {
        None=0,
        Books_Create=1<<0,
        Books_Update=1<<1,
        Books_Delete=1<<2,
        Books_View = 1<<3,
        Members_Create = 1 << 4,
        Members_Update = 1 << 5,
        Members_Delete = 1 << 6,
        Members_View = 1 << 7,
        BorrowTransactions_Create = 1 << 8,
        BorrowTransactions_Update = 1 << 9,
        BorrowTransactions_Delete = 1 << 10,
        BorrowTransactions_View = 1 << 11,
        Authors_Create = 1 << 12,
        Authors_Update = 1 << 13,
        Authors_Delete = 1 << 14,
        Authors_View = 1 << 15,
        Categories_Create = 1 << 16,
        Categories_Update = 1 << 17,
        Categories_Delete = 1 << 18,
        Categories_View = 1 << 19,
        Publishers_Create = 1 << 20,
        Publishers_Update = 1 << 21,
        Publishers_Delete = 1 << 22,
        Publishers_View = 1 << 23,
    }

   


}
