using System.ComponentModel.DataAnnotations;

namespace LibraryForTheLibrary.Extension.Core.Models;

public sealed class Book
{
    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public ushort Pages { get; set; }
}