using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication2.Models;

namespace WebApplication2.Models
{
    public class LibraryViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<SelectListItem> Authors { get; set; } 
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public string Content { get; set; }
    }
}
