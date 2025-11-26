using System;
using System.Collections.Generic;

namespace BookStoreApp.Models;

public partial class Store
{
    public int StoreId { get; set; }

    public string? StoreName { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }
}
