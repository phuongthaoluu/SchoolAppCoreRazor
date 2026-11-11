using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolAppCoreRazor.Models;

namespace SchoolAppCoreRazor.Pages.Courses;

public class CreateModel : PageModel
{
    private readonly SchoolContext _context;

    public CreateModel(SchoolContext context)
    {
        _context = context;
    }
    public IActionResult OnGet()
    {
        ViewData["DepartmentID"] = new SelectList(
            _context.Departments.Select(d => new
            {
                d.DepartmentID,
                DisplayText = d.DepartmentID + " - " + d.DepartmentName
            }),
            "DepartmentID",
            "DisplayText"
        );

        return Page();
    }


    [BindProperty]
    public Course Course { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Courses.Add(Course);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
