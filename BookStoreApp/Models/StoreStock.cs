using System;
using System.Collections.Generic;

namespace BookStoreApp.Models;

public partial class StoreStock
{
    public int StoreId { get; set; }

    public string Isbn { get; set; } = null!;

    public int? Quantity { get; set; }
}
