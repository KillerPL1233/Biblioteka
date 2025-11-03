using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Biblioteka.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
        public ICollection<SearchHistory> SavedSearches { get; set; } = new List<SearchHistory>();
    }


    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public string TableOfContents { get; set; } // wyciąg ze spisu treści
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int Stock { get; set; }

        public ICollection<BookTag> BookTags { get; set; }
        public ICollection<BookFile> Files { get; set; }
        public ICollection<Rental> Rentals { get; set; }
        public ICollection<QueueEntry> QueueEntries { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public Category ParentCategory { get; set; }
        public ICollection<Category> Children { get; set; }
        public ICollection<Book> Books { get; set; }
    }

    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<BookTag> BookTags { get; set; }
    }

    public class BookTag
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }

    public class BookFile
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }

    public enum RentalState
    {
        InStock,
        WaitingPickup,
        Rented
    }

    public class Rental
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public DateTime RequestedAt { get; set; }
        public DateTime? PickupBy { get; set; }
        public DateTime? RentedAt { get; set; }
        public DateTime? DueAt { get; set; }
        public decimal? FineAmount { get; set; }
        public RentalState State { get; set; }
    }

    public class QueueEntry
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public DateTime EnqueuedAt { get; set; }
    }

    public class SearchHistory
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string Query { get; set; }
        public DateTime SavedAt { get; set; }
    }
}
