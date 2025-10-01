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
}
