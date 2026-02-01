using LibraryForTheLibrary.Extension.Repository.Interfaces;
using LibraryForTheLibrary.Extension.Repository.Models;
using System.Runtime.Serialization;

namespace LibraryForTheLibrary.Extension.Repository.Implementation;

internal sealed class XmlBookRepository : IBookRepository
{
    private readonly string _filePath;

    public XmlBookRepository(string filePath)
    {
        _filePath = filePath;
    }

    public ValueTask<IReadOnlyCollection<BookDto>> Load()
    {
        if (!File.Exists(_filePath))
        {
            return ValueTask.FromResult<IReadOnlyCollection<BookDto>>([]);
        }

        try
        {
            var serializer = new DataContractSerializer(typeof(BookCollectionDto));

            using var stream = File.OpenRead(_filePath);

            var library = (BookCollectionDto)serializer.ReadObject(stream)!;

            return ValueTask.FromResult<IReadOnlyCollection<BookDto>>(library.Books);
        }
        catch (Exception ex)
        {
            throw new IOException($"Failed to load books from {_filePath}", ex);
        }
    }

    public ValueTask Save(BookCollectionDto books)
    {
        try
        {
            var serializer = new DataContractSerializer(typeof(BookCollectionDto));

            using var stream = File.Create(_filePath);

            serializer.WriteObject(stream, books);

            return ValueTask.CompletedTask;
        }
        catch (Exception ex)
        {
            throw new IOException($"Failed to save books to {_filePath}", ex);
        }
    }
}