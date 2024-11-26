using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApplicationSql
{
    public class BorrowBook
    {
        public int Id;
        public Book? book;
        public User? user;
        public int quantity;
        public DateTime dueDate;

        public BorrowBook(Book book, User user, int quantity)
        {
            this.book = book;
            this.user = user;
            this.quantity = quantity;
            Random random = new Random();
            int days = random.Next(-10, 10);
            this.dueDate = DateTime.Now.AddDays(days);
        }

        public BorrowBook(int Id, Book book, User user, int quantity, DateTime dueDate)
        {
            this.Id = Id;
            this.book = book;
            this.user = user;
            this.quantity = quantity;
            this.dueDate = dueDate;
        }

        public BorrowBook()
        {

        }
    }
}