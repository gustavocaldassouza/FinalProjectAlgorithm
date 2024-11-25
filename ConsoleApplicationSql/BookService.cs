using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApplicationSql
{
    public class BookService
    {
        private BookRepository bookRepository;
        public BookService()
        {
            bookRepository = new BookRepository("myDatabase.sqlite");
        }
        public void AddBook(Book book)
        {
            Book _book = bookRepository.SearchBook(book.ISBN!);
            if (_book.ISBN == book.ISBN)
            {
                Console.WriteLine("Book already exists!");
                return;
            }
            bookRepository.AddBook(book);
        }
        public Book SearchBook(string ISBN)
        {
            Book book = bookRepository.SearchBook(ISBN);
            if (book.ISBN == ISBN)
            {
                Console.WriteLine("Book found!");
                Console.WriteLine("Title: " + book.Title);
                Console.WriteLine("Author: " + book.Author);
                Console.WriteLine("ISBN: " + book.ISBN);
                Console.WriteLine("Quantity: " + book.Quantity);
                Console.WriteLine();
                return book;
            }

            Console.WriteLine();
            Console.WriteLine("No book found with this specific ISBN!");
            Console.WriteLine();
            return new Book();
        }
        public void ListBooks()
        {
            Book[] books = bookRepository.SearchBooks();
            for (int i = 0; i < books.Length; i++)
            {
                if (books[i] == null) break;
                Console.WriteLine("Title: " + books[i].Title);
                Console.WriteLine("Author: " + books[i].Author);
                Console.WriteLine("ISBN: " + books[i].ISBN);
                Console.WriteLine("Quantity: " + books[i].Quantity);
                Console.WriteLine();
            }
        }
    }
}