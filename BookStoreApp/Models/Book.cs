using System;
using System.Collections.Generic;

namespace BookStoreApp.Models;

public partial class Book
{
    public string Isbn13 { get; set; } = null!;

    public string? Title { get; set; }

    public decimal? Price { get; set; }

    public int? LanguagesId { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public int? AuthorId { get; set; }

    public int? GenreId { get; set; }

    public int? FormatId { get; set; }

    public int? PublisherId { get; set; }

    public virtual Author? Author { get; set; }

    public virtual Format? Format { get; set; }

    public virtual Genre? Genre { get; set; }

    public virtual Language? Languages { get; set; }

    public virtual Publisher? Publisher { get; set; }
}
