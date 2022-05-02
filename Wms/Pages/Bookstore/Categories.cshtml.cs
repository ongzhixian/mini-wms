using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Wms.Pages.Bookstore
{
    public class CategoriesModel : PageModel
    {
        [BindProperty]
        [Display(Name = "Category name")]
        public string CategoryName { get; set; } = string.Empty;


        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (!string.IsNullOrWhiteSpace(CategoryName))
            {

            }

            CategoryName = string.Empty;
        }
    }
}
