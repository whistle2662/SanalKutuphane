using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace WebApplication2.Models
{
    public class BookViewModel
    {
        public int Id { get; set; } 
        public string Title { get; set; } 
        public string Content { get; set; }  

        public int? SelectedAuthorId { get; set; } 
        public List<SelectListItem> Authors { get; set; }  
        public string NewAuthorName { get; set; } 

        public string ApplicationUserId { get; set; }
        public string AuthorName { get; set; }
        public string NewCategoryName { get; set; } 

    }
}
