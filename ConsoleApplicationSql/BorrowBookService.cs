using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApplicationSql
{
    public class BorrowBookService
    {
        private BorrowBookRepository borrowBookRepository;
        private BookRepository bookRepository;
        private UserService userService;
        private BookService bookService;
        public BorrowBookService(UserService userService, BookService bookService)
        {
            this.userService = userService;
            this.bookService = bookService;
            borrowBookRepository = new BorrowBookRepository("myDatabase.sqlite");
            bookRepository = new BookRepository("myDatabase.sqlite");
        }
        public void BorrowBook(string ISBN, int membershipId, int quantity = 1)
        {
            User user = userService.SearchUser(membershipId);
            if (user.MembershipId == membershipId)
            {
                Book book = bookService.SearchBook(ISBN);
                if (book.Quantity > 0)
                {
                    bookRepository.RemoveQuantity(book, quantity);
                    borrowBookRepository.AddBorrowedBook(book, user, quantity);
                    Console.WriteLine("Book borrowed!");
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
                if (book.ISBN == ISBN && book.Quantity > 0)
                {
                    int quantity = borrowBookRepository.FetchQuantityBorrowed(book);
                    borrowBookRepository.ReturnBook(book, user);
                    bookRepository.AddQuantity(book, quantity);

                    Console.WriteLine("Book returned!");
                    Console.WriteLine();
                    BorrowBook[] borrowedBooks = borrowBookRepository.ListBorrowedBooks();
                    for (int i = 0; i < borrowedBooks.Length; i++)
                    {
                        if (borrowedBooks[i].book?.ISBN == ISBN && borrowedBooks[i].user?.MembershipId == membershipId)
                        {
                            if (borrowedBooks[i].dueDate < DateTime.Now)
                            {
                                Console.WriteLine("Book is overdue!");
                                Console.WriteLine();
                            }
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
            BorrowBook[] borrowedBooks = borrowBookRepository.ListBorrowedBooks();
            for (int i = 0; i < borrowedBooks.Length; i++)
            {
                Console.WriteLine("---------------------------------");
                Console.WriteLine("Title: " + borrowedBooks[i].book?.Title);
                Console.WriteLine("Author: " + borrowedBooks[i].book?.Author);
                Console.WriteLine("ISBN: " + borrowedBooks[i].book?.ISBN);
                Console.WriteLine("Quantity of copies: " + borrowedBooks[i].book?.Quantity);
                Console.WriteLine();
                Console.WriteLine("User: " + borrowedBooks[i].user?.Name);
                Console.WriteLine("Membership ID: " + borrowedBooks[i].user?.MembershipId);
                Console.WriteLine("Due Date: " + borrowedBooks[i].dueDate);
                Console.WriteLine("Quantity borrowed: " + borrowedBooks[i].quantity);
                Console.WriteLine();
                if (borrowedBooks[i].dueDate < DateTime.Now)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Book is overdue!");
                    Console.WriteLine();
                    Console.ResetColor();
                }
            }
            if (!borrowedBooks.Any())
            {
                Console.WriteLine("No borrowed books found!");
                Console.WriteLine();
            }
        }
    }
}