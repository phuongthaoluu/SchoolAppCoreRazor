
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolAppCoreRazor.Models;

namespace SchoolAppCoreRazor.Pages.Courses;

public class EditModel : PageModel
{
    private readonly SchoolContext _context;

    public EditModel(SchoolContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Course Course { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? courseid)
    {
        if (courseid is null)
        {
            return NotFound();
        }

        var course = await _context.Courses.FirstOrDefaultAsync(m => m.CourseID == courseid);

        if (course is null)
        {
            return NotFound();
        }

        Course = course;

        LoadDepartments();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("Course.Department");

        if (!ModelState.IsValid)
        {
            LoadDepartments();
            return Page();
        }

        _context.Attach(Course).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CourseExists(Course.CourseID))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private void LoadDepartments()
    {
        ViewData["DepartmentID"] = new SelectList(
            _context.Departments.Select(d => new
            {
                d.DepartmentID,
                DisplayText = d.DepartmentID + " - " + d.DepartmentName
            }),
            "DepartmentID",
            "DisplayText",
            Course.DepartmentID
        );
    }

    private bool CourseExists(int courseid)
    {
        return _context.Courses.Any(e => e.CourseID == courseid);
    }
}
