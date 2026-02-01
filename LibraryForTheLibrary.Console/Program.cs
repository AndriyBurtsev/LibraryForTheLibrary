using LibraryForTheLibrary.Extension.Core.Factories;
using LibraryForTheLibrary.Extension.Core.Models;
using System;
using System.Threading.Tasks;

namespace LibraryForTheLibrary.Console;

public class Program
{
    static async Task Main(string[] args)
    {
        var bookService = BookLibraryFactory.CreateBookService("books.xml");

        var collection = await bookService.Get();

        foreach (var book in collection)
        {
            System.Console.WriteLine($"{book.Title} by {book.Author} ({book.Pages})");
        }

        System.Console.WriteLine("Hello, World!");
    }
}