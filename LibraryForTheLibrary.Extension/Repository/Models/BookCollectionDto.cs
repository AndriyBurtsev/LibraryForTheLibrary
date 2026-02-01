using System.Runtime.Serialization;

namespace LibraryForTheLibrary.Extension.Repository.Models;

[DataContract(Name = "library")]
internal sealed class BookCollectionDto
{
    [DataMember(Name = "book")]
    public required BookDto[] Books { get; set; }
}