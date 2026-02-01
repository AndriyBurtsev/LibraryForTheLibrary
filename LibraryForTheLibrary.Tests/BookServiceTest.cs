using LibraryForTheLibrary.Extension.Repository.Interfaces;
using LibraryForTheLibrary.Extension.Repository.Models;
using LibraryForTheLibrary.Extension.Services.Implementation;
using LibraryForTheLibrary.Extension.Core.Models;
using Moq;

namespace LibraryForTheLibrary.Tests;

public sealed class BookServiceTest
{
    private readonly Mock<IBookRepository> _mockRepository;
    private readonly BookService _bookService;

    public BookServiceTest()
    {
        _mockRepository = new Mock<IBookRepository>();
        _bookService = new BookService(_mockRepository.Object);
    }

    [Fact]
    public async Task Save_WithValidBooks_CallsRepositorySave()
    {
        // Arrange
        var books = new List<Book>
        {
            new() 
            { 
                Title = "Test Book", 
                Author = "Test Author", 
                Pages = 100,
            }
        };

        _mockRepository
            .Setup(r => r.Save(It.IsAny<BookCollectionDto>()))
            .Returns(ValueTask.CompletedTask);

        // Act
        await _bookService.Save(books);

        // Assert
        _mockRepository.Verify(r => r.Save(It.IsAny<BookCollectionDto>()), Times.Once);
    }

    [Fact]
    public async Task Save_WithMultipleBooks_MapsAllBooks()
    {
        // Arrange
        var books = new List<Book>
        {
            new() { Title = "Book 1", Author = "Author 1", Pages = 100 },
            new() { Title = "Book 2", Author = "Author 2", Pages = 200 },
            new() { Title = "Book 3", Author = "Author 3", Pages = 300 },
        };

        var length = books.Count;

        BookCollectionDto? capturedDto = null;

        _mockRepository
            .Setup(r => r.Save(It.IsAny<BookCollectionDto>()))
            .Callback<BookCollectionDto>(dto => capturedDto = dto)
            .Returns(ValueTask.CompletedTask);

        // Act
        await _bookService.Save(books);

        // Assert
        Assert.NotNull(capturedDto);
        Assert.Equal(length, capturedDto.Books.Length);
    }

    [Fact]
    public async Task Save_MapsBookPropertiesCorrectly()
    {
        // Arrange
        var book = new Book
        {
            Title = "Sample Title",
            Author = "Sample Author",
            Pages = 150,
        };

        var books = new List<Book> { book };

        BookCollectionDto? capturedDto = null;

        _mockRepository
            .Setup(r => r.Save(It.IsAny<BookCollectionDto>()))
            .Callback<BookCollectionDto>(dto => capturedDto = dto)
            .Returns(ValueTask.CompletedTask);

        // Act
        await _bookService.Save(books);

        // Assert
        Assert.NotNull(capturedDto);
        var savedBook = capturedDto.Books.First();
        Assert.Equal(book.Title, savedBook.Title);
        Assert.Equal(book.Author, savedBook.Author);
        Assert.Equal(book.Pages, savedBook.Pages);
    }

    [Fact]
    public async Task Save_WithEmptyCollection_SavesEmptyCollection()
    {
        // Arrange
        var books = new List<Book>();

        BookCollectionDto? capturedDto = null;

        _mockRepository
            .Setup(r => r.Save(It.IsAny<BookCollectionDto>()))
            .Callback<BookCollectionDto>(dto => capturedDto = dto)
            .Returns(ValueTask.CompletedTask);

        // Act
        await _bookService.Save(books);

        // Assert
        Assert.NotNull(capturedDto);
        Assert.Empty(capturedDto.Books);
    }

    [Fact]
    public async Task Get_ReturnsAllBooks()
    {
        // Arrange
        var bookCollection = new[]
        {
            new BookDto { Title = "Book 1", Author = "Author 1", Pages = 100 },
            new BookDto { Title = "Book 2", Author = "Author 2", Pages = 200 }
        };

        var collectionLength = bookCollection.Length;

        _mockRepository
            .Setup(r => r.Load())
            .ReturnsAsync(bookCollection);

        // Act
        var result = await _bookService.Get();

        // Assert
        Assert.Equal(collectionLength, result.Count);
    }

    [Fact]
    public async Task Add_AddsNewBookToExistingCollection()
    {
        // Arrange
        var existingBooks = new[]
        {
            new BookDto { Title = "Existing Book", Author = "Author", Pages = 100 }
        };

        _mockRepository
            .Setup(r => r.Load())
            .ReturnsAsync(existingBooks);

        _mockRepository
            .Setup(r => r.Save(It.IsAny<BookCollectionDto>()))
            .Returns(ValueTask.CompletedTask);

        var newBook = new Book { Title = "New Book", Author = "New Author", Pages = 150 };

        // Act
        await _bookService.Add(newBook);

        // Assert
        _mockRepository.Verify(r => r.Save(It.Is<BookCollectionDto>(
            dto => dto.Books.Length == 2)), Times.Once);
    }

    [Fact]
    public async Task Search_FindsBooksByTitle()
    {
        // Arrange
        var bookDtos = new[]
        {
            new BookDto { Title = "The Great Gatsby", Author = "Fitzgerald", Pages = 180 },
            new BookDto { Title = "Great Expectations", Author = "Dickens", Pages = 544 },
            new BookDto { Title = "1984", Author = "Orwell", Pages = 328 }
        };

        const string Title = "Great";

        _mockRepository.Setup(r => r.Load()).ReturnsAsync(bookDtos);

        // Act
        var result = await _bookService.Search(Title);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, b => Assert.Contains(Title, b.Title, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetSorted_SortsByAuthorThenTitle()
    {
        var book1 = new BookDto
        {
            Title = "The Ugly Duckling",
            Author = "Andersen",
            Pages = 50,
        };

        var book2 = new BookDto
        {
            Title = "The Little Mermaid",
            Author = "Andersen",
            Pages = 60,
        };

        var book3 = new BookDto
        {
            Title = "Misery",
            Author = "King",
            Pages = 320,
        };

        var book4 = new BookDto
        {
            Title = "Carrie",
            Author = "King",
            Pages = 199,
        };

        // Arrange
        var bookCollection = new[]
        {
            book1,
            book2,
            book3,
            book4
        };

        _mockRepository.Setup(r => r.Load()).ReturnsAsync(bookCollection);

        // Act
        var result = await _bookService.GetSorted();

        // Assert
        var resultArray = result.ToArray();
        Assert.Equal(book2.Author, resultArray[0].Author);
        Assert.Equal(book2.Title, resultArray[0].Title);
        Assert.Equal(book1.Author, resultArray[1].Author);
        Assert.Equal(book1.Title, resultArray[1].Title);
        Assert.Equal(book4.Author, resultArray[2].Author);
        Assert.Equal(book4.Title, resultArray[2].Title);
    }
}