using DimWebRazor_Temp.Data;
using DimWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DimWebRazor_Temp.Pages.Categories
{
    public class CreateModel : PageModel
    {

        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Category Category { get; set; }
        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost(Category obj)
        {
            _db.Categories.Add(Category);
            _db.SaveChanges();
            TempData["success"] = "Category Created Successfully!";
            return RedirectToPage("Index");
        }
    }
}
