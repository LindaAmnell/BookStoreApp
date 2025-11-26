using System;
using System.Collections.Generic;

namespace BookStoreApp.Models;

public partial class Author
{
    public int AuthorId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? Birthdate { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
