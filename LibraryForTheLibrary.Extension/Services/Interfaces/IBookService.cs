using LibraryForTheLibrary.Extension.Core.Models;

namespace LibraryForTheLibrary.Extension.Services.Interfaces;

/// <summary>
/// Provides an abstraction for managing a collection of books with asynchronous operations for retrieval, addition,
/// searching, saving, and sorting.
/// </summary>
/// <remarks>Implementations of this interface are expected to perform all operations asynchronously, making them
/// suitable for responsive applications and scenarios involving I/O-bound tasks. The interface ensures that consumers
/// can interact with book collections in a non-blocking manner, supporting efficient resource usage and scalability.
/// The specific behaviors, such as sorting criteria or data storage mechanisms, are determined by the implementing
/// class.</remarks>
public interface IBookService
{
    /// <summary>
    /// Asynchronously retrieves a read-only collection of books from the data source.
    /// </summary>
    /// <remarks>This method performs I/O operations and should be awaited to ensure the collection is
    /// retrieved before use.</remarks>
    /// <returns>A <see langword="ValueTask"/> that represents the asynchronous operation. The result contains a read-only
    /// collection of <see cref="Book"/> objects. The collection is empty if no books are available.</returns>
    ValueTask<IReadOnlyCollection<Book>> Get();


    /// <summary>
    /// Asynchronously adds a book to the collection.
    /// </summary>
    /// <remarks>This method performs the addition in a non-blocking manner, which is suitable for
    /// applications that require high responsiveness or handle I/O-bound operations.</remarks>
    /// <param name="book">The book to add to the collection. Cannot be null.</param>
    /// <returns>A ValueTask that represents the asynchronous add operation.</returns>
    ValueTask Add(Book book);


    /// <summary>
    /// Searches for books that match the specified search key.
    /// </summary>
    /// <remarks>This method performs a case-insensitive search and may return partial matches based on the
    /// search key.</remarks>
    /// <param name="searchKey">The search term used to filter the books. This parameter cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only collection of books that
    /// match the search criteria. The collection will be empty if no books are found.</returns>
    ValueTask<IReadOnlyCollection<Book>> Search(string searchKey);


    /// <summary>
    /// Asynchronously saves the specified collection of books to the underlying data store.
    /// </summary>
    /// <remarks>This method performs the save operation without blocking the calling thread. Ensure that the
    /// collection contains valid book instances before invoking this method.</remarks>
    /// <param name="books">A read-only collection of <see cref="Book"/> objects to be saved. This collection must not be null or empty.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous save operation.</returns>
    ValueTask Save(IReadOnlyCollection<Book> books);


    /// <summary>
    /// Retrieves a read-only collection of books sorted according to the implementation-defined criteria.
    /// </summary>
    /// <remarks>The sorting criteria are determined by the implementation of this method. This method is
    /// asynchronous and returns a <see cref="ValueTask{TResult}"/>, which allows for efficient resource usage when
    /// awaiting the result.</remarks>
    /// <returns>A read-only collection of <see cref="Book"/> objects, sorted as specified by the implementation.</returns>
    ValueTask<IReadOnlyCollection<Book>> GetSorted();
}