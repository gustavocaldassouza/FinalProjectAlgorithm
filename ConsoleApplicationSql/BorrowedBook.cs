using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApplicationSql
{
    public class BorrowedBook
    {
        public Book? book;
        public User? user;
        public int quantity;
        public DateTime dueDate;

        public BorrowedBook(Book book, User user, int quantity)
        {
            this.book = book;
            this.user = user;
            this.quantity = quantity;
            Random random = new Random();
            int days = random.Next(-10, 10);
            this.dueDate = DateTime.Now.AddDays(days);
        }

        public BorrowedBook()
        {

        }
    }
}