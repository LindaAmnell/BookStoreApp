using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace BookStoreApp.Models;

public partial class BookStoreContext : DbContext
{
    public BookStoreContext()
    {
    }

    public BookStoreContext(DbContextOptions<BookStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Format> Formats { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<StoreStock> StoreStocks { get; set; }

    public virtual DbSet<StoreStockView> StoreStockViews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId).HasName("PK__Authors__70DAFC1485BE9F51");

            entity.Property(e => e.AuthorId).HasColumnName("AuthorID");
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Isbn13).HasName("PK__Books__3BF79E030DAEB112");

            entity.Property(e => e.Isbn13)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN13");
            entity.Property(e => e.AuthorId).HasColumnName("AuthorID");
            entity.Property(e => e.FormatId).HasColumnName("FormatID");
            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.LanguagesId).HasColumnName("LanguagesID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PublisherId).HasColumnName("PublisherID");
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK__Books__AuthorID__2F10007B");

            entity.HasOne(d => d.Format).WithMany(p => p.Books)
                .HasForeignKey(d => d.FormatId)
                .HasConstraintName("FK__Books__FormatID__30F848ED");

            entity.HasOne(d => d.Genre).WithMany(p => p.Books)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK__Books__GenreID__300424B4");

            entity.HasOne(d => d.Languages).WithMany(p => p.Books)
                .HasForeignKey(d => d.LanguagesId)
                .HasConstraintName("FK__Books__Languages__2E1BDC42");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Books)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("FK__Books__Publisher__31EC6D26");
        });

        modelBuilder.Entity<Format>(entity =>
        {
            entity.HasKey(e => e.FormatId).HasName("PK__Formats__5D3DCB791466C72F");

            entity.Property(e => e.FormatId).HasColumnName("FormatID");
            entity.Property(e => e.FormatType).HasMaxLength(100);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PK__Genres__0385055E3AB58BA3");

            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.GenreName).HasMaxLength(100);
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.LanguageId).HasName("PK__Language__B938558B5A8CE1FD");

            entity.Property(e => e.LanguageId).HasColumnName("LanguageID");
            entity.Property(e => e.LanguageName).HasMaxLength(100);
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.PublisherId).HasName("PK__Publishe__4C657E4BD4A4A88A");

            entity.Property(e => e.PublisherId).HasColumnName("PublisherID");
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreId).HasName("PK__Stores__3B82F0E196EE72C0");

            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.StoreName).HasMaxLength(100);
        });

        modelBuilder.Entity<StoreStock>(entity =>
        {
            entity.HasKey(e => new { e.StoreId, e.Isbn }).HasName("PK__StoreSto__9FC5238FDCF3AF00");

            entity.ToTable("StoreStock");

            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN");
        });

        modelBuilder.Entity<StoreStockView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("StoreStockView");

            entity.Property(e => e.BookTitle).HasMaxLength(100);
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN");
            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.StoreName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
