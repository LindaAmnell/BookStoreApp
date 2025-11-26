using System;
using System.Collections.Generic;

namespace BookStoreApp.Models;

public partial class Format
{
    public int FormatId { get; set; }

    public string? FormatType { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
