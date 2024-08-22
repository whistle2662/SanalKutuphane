using WebApplication2.Models;
namespace WebApplication2.Models
{

    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public string Content { get; set; }
        public string? ApplicationUserId { get; set; } 
        public ApplicationUser ApplicationUser { get; set; } 
        public Author Author { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
    }


}