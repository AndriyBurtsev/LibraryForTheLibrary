using System.Runtime.Serialization;

namespace LibraryForTheLibrary.Extension.Repository.Models;

[DataContract(Name = "book")]
internal sealed class BookDto
{
    [DataMember(Name = "title", Order = 1)]
    public string Title { get; set; } = string.Empty;

    [DataMember(Name = "author", Order = 2)]
    public string Author { get; set; } = string.Empty;

    [DataMember(Name = "pages", Order = 3)]
    public ushort Pages { get; set; }
}