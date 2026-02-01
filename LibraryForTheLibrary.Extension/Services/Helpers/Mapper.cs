using LibraryForTheLibrary.Extension.Core.Models;
using LibraryForTheLibrary.Extension.Repository.Models;

namespace LibraryForTheLibrary.Extension.Services.Helpers;

internal static class Mapper
{
    public static Book Map(BookDto bookDto)
    {
        var book = new Book()
        {
            Title = bookDto.Title,
            Author = bookDto.Author,
            Pages = bookDto.Pages,
        };
            
        return book;
    }

    public static BookDto Map(Book book)
    {
        var bookDto = new BookDto()
        {
            Title = book.Title,
            Author = book.Author,
            Pages = book.Pages
        };

        return bookDto;
    }
}