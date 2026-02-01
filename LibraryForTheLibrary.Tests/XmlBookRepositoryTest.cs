using LibraryForTheLibrary.Extension.Repository.Implementation;
using LibraryForTheLibrary.Extension.Repository.Models;

namespace LibraryForTheLibrary.Tests;

public sealed class XmlBookRepositoryTest : IDisposable
{
    private readonly string _tempFilePath;

    public XmlBookRepositoryTest()
    {
        _tempFilePath = "testXmlBookRepositoryTest.xml";
    }

    public void Dispose()
    {
        if (File.Exists(_tempFilePath))
        {
            File.Delete(_tempFilePath);
        }
    }

    [Fact]
    public async Task Save_WithValidBooks_CreatesFile()
    {
        // Arrange
        var repository = new XmlBookRepository(_tempFilePath);

        var bookDto = new BookDto 
        { 
            Title = "Test Book", 
            Author = "Test Author", 
            Pages = 100,
        };

        var books = new BookCollectionDto
        {
            Books = [bookDto]
        };

        // Act
        await repository.Save(books);

        // Assert
        Assert.True(File.Exists(_tempFilePath));
        Assert.True(new FileInfo(_tempFilePath).Length > 0);
    }

    [Fact]
    public async Task Save_WithEmptyCollection_CreatesValidFile()
    {
        // Arrange
        var repository = new XmlBookRepository(_tempFilePath);

        var books = new BookCollectionDto { Books = [] };

        // Act
        await repository.Save(books);

        // Assert
        Assert.True(File.Exists(_tempFilePath));
    }

    [Fact]
    public async Task Load_AfterSave_ReturnsOriginalData()
    {
        // Arrange
        var repository = new XmlBookRepository(_tempFilePath);

        var bookDto1 = new BookDto 
        { 
            Title = "Book 1", 
            Author = "Author 1", 
            Pages = 100,
        };

        var bookDto2 = new BookDto
        {
            Title = "Book 2",
            Author = "Author 2",
            Pages = 200,
        };

        var originalBooks = new BookCollectionDto
        {
            Books =
            [
                bookDto1,
                bookDto2,
            ]
        };

        var collectionLength = originalBooks.Books.Length;

        await repository.Save(originalBooks);

        // Act
        var loadedBooks = await repository.Load();

        // Assert
        Assert.Equal(collectionLength, loadedBooks.Count);
        Assert.Contains(loadedBooks, b => b.Title == bookDto1.Title && b.Author == bookDto1.Author && b.Pages == bookDto1.Pages);
        Assert.Contains(loadedBooks, b => b.Title == bookDto2.Title && b.Author == bookDto2.Author && b.Pages == bookDto2.Pages);
    }

    [Fact]
    public async Task Save_WithSpecialCharacters_HandlesCorrectly()
    {
        // Arrange
        var repository = new XmlBookRepository(_tempFilePath);

        var bookDto = new BookDto
        {
            Title = "Book <With> \"Special\" & 'Chars'",
            Author = "Author",
            Pages = 50,
        };

        var books = new BookCollectionDto
        {
            Books = [bookDto]
        };

        // Act
        await repository.Save(books);
        var loadedBooks = await repository.Load();

        // Assert
        Assert.Single(loadedBooks);
        Assert.Equal(loadedBooks.First().Title, bookDto.Title);
    }
}