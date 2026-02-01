using LibraryForTheLibrary.Extension.Repository.Models;

namespace LibraryForTheLibrary.Extension.Repository.Interfaces;

internal interface IBookRepository
{
    ValueTask<IReadOnlyCollection<BookDto>> Load();

    ValueTask Save(BookCollectionDto books);
}