using LibraryForTheLibrary.Extension.Repository.Implementation;
using LibraryForTheLibrary.Extension.Services.Implementation;
using LibraryForTheLibrary.Extension.Services.Interfaces;

namespace LibraryForTheLibrary.Extension.Core.Factories;

public static class BookLibraryFactory
{
    /// <summary>
    /// Creates a new instance of an IBookService that manages book data stored in an XML file.
    /// </summary>
    /// <remarks>Ensure that the XML file is well-formed and accessible to prevent runtime errors when
    /// accessing or modifying book data.</remarks>
    /// <param name="xmlFilePath">The path to the XML file that contains the book data. The file must exist and be accessible.</param>
    /// <returns>An IBookService instance that provides methods for managing books using the specified XML file as storage.</returns>
    public static IBookService CreateBookService(string xmlFilePath)
    {
        var repository = new XmlBookRepository(xmlFilePath);

        var bookService = new BookService(repository);

        return bookService;
    }
}