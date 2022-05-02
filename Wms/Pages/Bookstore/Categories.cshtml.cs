using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Wms.Helpers;
using Wms.Models.Data.Bookstore;
using Wms.Services;

namespace Wms.Pages.Bookstore
{
    public class CategoriesModel : PageModel
    {
        
        [BindProperty(SupportsGet = true)]
        public Category Category { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<Category> CategoryList { get; set; }

        private CategoryService categoryService;

        public CategoriesModel(CategoryService categoryService)
        {
            this.categoryService = categoryService;
            this.Category = new Category();
        }

        public async Task OnGetAsync()
        {
            CategoryList = await categoryService.GetAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Category != null)
            {
                await categoryService.CreateAsync(Category);
                ModelState.Clear();
                Category = new Category();
            }

            return Page();
        }

    }
}
