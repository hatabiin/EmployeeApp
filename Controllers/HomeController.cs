using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployeeApp.Models;
using Microsoft.AspNetCore.Authorization;
using EmployeeApp.Data;
using Microsoft.EntityFrameworkCore;


namespace EmployeeApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var employees = await _context.Employees
            .Include(e => e.Departments)
            .Include(e => e.Divisions)
            .Include(e => e.Licenses)
            .ToListAsync();
            return View(employees);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employee = await _context.Employees
            .Include(e => e.Departments)
            .Include(e => e.Divisions)
            .Include(e => e.Licenses)
            .FirstOrDefaultAsync(e => e.Id == id);

        ViewBag.AllDepartments = await _context.Departments.ToListAsync();
        ViewBag.AllDivisions = await _context.Divisions.ToListAsync();
        ViewBag.AllLicenses = await _context.Licenses.ToListAsync();
        return View(employee);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id,
                                          string employeeName, 
                                          int[] departmentIds,
                                          int[] divisionIds,
                                          int[] licenseIds)
    {
        var employee = await _context.Employees
            .Include(e => e.Departments)
            .Include(e => e.Divisions)
            .Include(e => e.Licenses)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null)
        {
            return NotFound();
        }

        employee.EmployeeName = employeeName;
        employee.Departments.Clear();
        var selectedDepartments = await _context.Departments
                                    .Where(d => departmentIds.Contains(d.Id))
                                    .ToListAsync();
         foreach (var dept in selectedDepartments)
        {
            employee.Departments.Add(dept);
        }

        employee.Divisions.Clear();
        var selectedDivisions = await _context.Divisions
                                    .Where(d => divisionIds.Contains(d.Id))
                                    .ToListAsync();
        foreach (var div in selectedDivisions)
        {
            employee.Divisions.Add(div);
        }

        employee.Licenses.Clear();
        var selectedLicenses = await _context.Licenses
                                    .Where(l => licenseIds.Contains(l.Id))
                                    .ToListAsync();
        foreach (var license in selectedLicenses)
        {
            employee.Licenses.Add(license);
        }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> Detail(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employee = await _context.Employees
            .Include(e => e.Departments)
            .Include(e => e.Divisions)
            .Include(e => e.Licenses)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null)
        {
            return NotFound();
        }

        return View(employee);
    }
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound();
        }

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
