using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApplicationSql
{
    public class BorrowService
    {
        UserService userService;
        BookService bookService;
        BorrowedBook[] borrowedBooks = new BorrowedBook[2];
        public BorrowService(UserService userService, BookService bookService)
        {
            this.userService = userService;
            this.bookService = bookService;
        }
        void AddBorrowedBook(Book book, User user, int quantity)
        {
            BorrowedBook borrowedBook = new BorrowedBook(book, user, quantity);
            for (int i = 0; i < borrowedBooks.Length; i++)
            {
                if (borrowedBooks[i] == null)
                {
                    borrowedBooks[i] = borrowedBook;
                    return;
                }
            }

            // initialize a new array with new length and transfer the old one to the new one.
            BorrowedBook[] newArrBooks = new BorrowedBook[borrowedBooks.Length + 5];
            for (int i = 0; i < borrowedBooks.Length; i++)
            {
                newArrBooks[i] = borrowedBooks[i];
            }
            newArrBooks[borrowedBooks.Length] = borrowedBook;
            borrowedBooks = newArrBooks;
        }
        public void BorrowBook(string ISBN, int membershipId, int quantity = 1)
        {
            User user = userService.SearchUser(membershipId);
            if (user.MembershipId == membershipId)
            {
                Book book = bookService.SearchBook(ISBN);
                if (book.Quantity > 0)
                {
                    book.Quantity--;
                    Console.WriteLine("Book borrowed!");
                    AddBorrowedBook(book, user, quantity);
                    return;
                }
                else
                {
                    Console.WriteLine("Book is not available!");
                    return;
                }
            }
            Console.WriteLine("User not found!");
        }
        public void ReturnBook(string ISBN, int membershipId)
        {
            User user = userService.SearchUser(membershipId);
            if (user.MembershipId == membershipId)
            {
                Book book = bookService.SearchBook(ISBN);
                // if Book exists
                if (book.ISBN == ISBN && book.Quantity > 0)
                {
                    book.Quantity++;
                    Console.WriteLine("Book returned!");
                    Console.WriteLine();
                    for (int i = 0; i < borrowedBooks.Length; i++)
                    {
                        if (borrowedBooks[i] == null) break;
                        if (borrowedBooks[i].book?.ISBN == ISBN && borrowedBooks[i].user?.MembershipId == membershipId)
                        {
                            if (borrowedBooks[i].dueDate < DateTime.Now)
                            {
                                Console.WriteLine("Book is overdue!");
                                Console.WriteLine();
                            }
                            borrowedBooks[i] = new BorrowedBook();
                            return;
                        }
                    }
                    return;
                }
                Console.WriteLine("Book not found!");
                return;
            }
            Console.WriteLine("User not found!");
        }
        public void ListBorrowedBooks()
        {
            bool found = false;
            for (int i = 0; i < borrowedBooks.Length; i++)
            {
                if (borrowedBooks[i] == null) break;
                if (borrowedBooks[i].book == null) continue;
                found = true;
                Console.WriteLine("Title: " + borrowedBooks[i].book?.Title);
                Console.WriteLine("Author: " + borrowedBooks[i].book?.Author);
                Console.WriteLine("ISBN: " + borrowedBooks[i].book?.ISBN);
                Console.WriteLine("Quantity: " + borrowedBooks[i].book?.Quantity);
                Console.WriteLine();
                Console.WriteLine("User: " + borrowedBooks[i].user?.Name);
                Console.WriteLine("Membership ID: " + borrowedBooks[i].user?.MembershipId);
                Console.WriteLine("Due Date: " + borrowedBooks[i].dueDate);
                Console.WriteLine();
                if (borrowedBooks[i].dueDate < DateTime.Now)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Book is overdue!");
                    Console.WriteLine();
                    Console.ResetColor();
                }
            }
            if (!found)
            {
                Console.WriteLine("No borrowed books found!");
                Console.WriteLine();
            }
        }
    }
}