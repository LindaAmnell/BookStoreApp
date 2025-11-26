using System;
using System.Collections.Generic;

namespace BookStoreApp.Models;

public partial class Publisher
{
    public int PublisherId { get; set; }

    public string? Name { get; set; }

    public string? Country { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
