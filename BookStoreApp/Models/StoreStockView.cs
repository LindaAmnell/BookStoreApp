using System;
using System.Collections.Generic;

namespace BookStoreApp.Models;

public partial class StoreStockView
{
    public int StoreId { get; set; }

    public string? StoreName { get; set; }

    public string Isbn { get; set; } = null!;

    public string? BookTitle { get; set; }

    public int? Quantity { get; set; }
}
