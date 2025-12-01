# 📚 BookStore -- Entity Framework Database First Application

## 📖 About the Project

This project was developed as part of **Lab 2 -- Application with Entity
Framework (Database First)**.\
The goal was to build an application that interacts with a SQL database
using Entity Framework.

My project is a **BookStore management system** where the user can work
with books, authors, and store inventories.\
The database structure is created using the included SQL script
**BookStoreDB.sql**.

The application allows the user to:

-   📘 View all books and detailed information\
-   🏬 View store stock levels\
-   ➕ Add or remove books from stores\
-   ✏️ Edit existing books\
-   👤 Manage authors\
-   🌍 Manage genres, languages, formats, and publishers\
-   🆕 Add new books and authors (VG requirement)

The project fulfills both **G and VG requirements** according to the lab
specification.

------------------------------------------------------------------------

## 🛠️ Requirements Fulfilled

### ✅ Pass (G)

-   Fully working CRUD operations\
-   Multiple related tables (Books, Stores, Authors, etc.)\
-   Entity Framework **Database First** generated model\
-   User can read and update store stock and book data\
-   Project can be cloned and run smoothly using the SQL setup

### ⭐ Distinction (VG)

-   Add completely new books\
-   Add new authors\
-   Edit and delete books and authors\
-   Full metadata support (price, release date, genre, language, format,
    publisher)\
-   Extended functionality and clean structure

------------------------------------------------------------------------

## 🗄️ How to Set Up the Database

Before running the application, install the database using the provided
SQL script:

1.  Open **SQL Server Management Studio (SSMS)**\
2.  Go to **File → Open → File...**\
3.  Select **BookStoreDB.sql**\
4.  Click **Execute**\
5.  Refresh your **Databases** list to confirm *BookStoreDB* was created

You **must** run this SQL script before starting the application.
