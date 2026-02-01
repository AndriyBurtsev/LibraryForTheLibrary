using LibraryForTheLibrary.Extension.Core.Models;
using LibraryForTheLibrary.Extension.Repository.Interfaces;
using LibraryForTheLibrary.Extension.Repository.Models;
using LibraryForTheLibrary.Extension.Services.Helpers;
using LibraryForTheLibrary.Extension.Services.Interfaces;

namespace LibraryForTheLibrary.Extension.Services.Implementation;

public sealed class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    internal BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async ValueTask Add(Book book)
    {
        ArgumentNullException.ThrowIfNull(book);

        var existingBooks = await _bookRepository.Load().ConfigureAwait(false);

        var bookDto = Mapper.Map(book);

        var updatedBooks = existingBooks
            .Append(bookDto)
            .ToArray();

        var bookCollectionDto = new BookCollectionDto { Books = updatedBooks };

        await _bookRepository.Save(bookCollectionDto).ConfigureAwait(false);
    }

    public async ValueTask<IReadOnlyCollection<Book>> Get()
    {
        var booksFromRepository = await _bookRepository.Load().ConfigureAwait(false);

        var resultCollection = booksFromRepository
            .Select(Mapper.Map)
            .ToArray();

        return resultCollection;
    }

    public async ValueTask Save(IReadOnlyCollection<Book> books)
    {
        ArgumentNullException.ThrowIfNull(books);

        var bookForSave = books
            .Select(Mapper.Map)
            .ToArray();

        var bookCollectionDto = new BookCollectionDto { Books = bookForSave };

        await _bookRepository.Save(bookCollectionDto).ConfigureAwait(false);
    }

    public async ValueTask<IReadOnlyCollection<Book>> Search(string searchKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(searchKey);

        var books = await Get();

        var filteredBooks = books
            .Where(b => b.Title.Contains(searchKey, StringComparison.OrdinalIgnoreCase))
            .ToArray();

        return filteredBooks;
    }

    public async ValueTask<IReadOnlyCollection<Book>> GetSorted()
    {
        var books = await Get();

        var sortedCollection = books
            .OrderBy(b => b.Author, StringComparer.OrdinalIgnoreCase)
            .ThenBy(b => b.Title, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return sortedCollection;
    }
}