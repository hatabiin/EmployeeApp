using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployeeApp.Models;
using Microsoft.AspNetCore.Authorization;
using EmployeeApp.Data;
using Microsoft.EntityFrameworkCore;


namespace EmployeeApp.Controllers;

[Authorize]
public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context) : base(context)
    {
        _logger = logger;
    }

    public async Task<IActionResult> Index(int? companyId)
    {
        List<Employee>employees;

        if (IsMasterUser())
        {
            ViewBag.Companies = await _context.Companies.ToListAsync();
            if (companyId != null)
            {
                ViewBag.SelectedCompany = await _context.Companies
                    .Where(c => c.Id == companyId)
                    .FirstOrDefaultAsync();

                employees = await _context.Employees
                    .Where(e => e.CompanyId == companyId)
                    .Include(e => e.Departments)
                    .Include(e => e.Divisions)
                    .Include(e => e.Licenses)
                    .ToListAsync();
            }
            else
            {
                employees = await _context.Employees
                    .Include(e => e.Departments)
                    .Include(e => e.Divisions)
                    .Include(e => e.Licenses)
                    .ToListAsync();
            }
        }
        else
        {
            var userCompanyId = await GetCurrentCompanyIdAsync();
            employees = await _context.Employees
                .Where(e => e.CompanyId == userCompanyId)
                .Include(e => e.Departments)
                .Include(e => e.Divisions)
                .Include(e => e.Licenses)
                .ToListAsync();

            ViewBag.SelectedCompany = await _context.Companies
                .FindAsync(userCompanyId);
        }
        
        return View(employees);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.AllCompanies = await _context.Companies
            .Include(c => c.Departments)
            .Include(c => c.Divisions)
            .ToListAsync();
        ViewBag.AllLicenses = await _context.Licenses.ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(string employeeName,
                                            string newPassword,
                                            string confirmPassword,
                                            int companyId,
                                            int[] departmentIds,
                                            int[] divisionIds,
                                            int[] licenseIds)
    {
         
        if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
        {
            ViewBag.Error = "パスワードを入力してください";
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();
            ViewBag.AllDivisions = await _context.Divisions.ToListAsync();
            ViewBag.AllLicenses = await _context.Licenses.ToListAsync();
            return View();
        }

        if (newPassword != confirmPassword)
        {
            ViewBag.Error = "パスワードが異なります";
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();
            ViewBag.AllDivisions = await _context.Divisions.ToListAsync();
            ViewBag.AllLicenses = await _context.Licenses.ToListAsync();
            return View();
        }

        var employee = new Employee
        {
            EmployeeName = employeeName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword),
            CompanyId = companyId
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        var selectedDepartments = await _context.Departments
                                    .Where(d => departmentIds.Contains(d.Id))
                                    .ToListAsync();
        foreach (var dept in selectedDepartments)
        {
            employee.Departments.Add(dept);
        }

        var selectedDivisions = await _context.Divisions
                                    .Where(d => divisionIds.Contains(d.Id))
                                    .ToListAsync();
        foreach (var div in selectedDivisions)
        {
            employee.Divisions.Add(div);
        }

        var selectedLicenses = await _context.Licenses
                                    .Where(l => licenseIds.Contains(l.Id))
                                    .ToListAsync();
        foreach (var license in selectedLicenses)
        {
            employee.Licenses.Add(license);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Home");
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

        ViewBag.AllDepartments = await _context.Departments
            .Include(d => d.Divisions) 
            .ToListAsync();
        ViewBag.AllLicenses = await _context.Licenses.ToListAsync();
        
        return View(employee);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id,
                                          string employeeName,
                                          string newPassword,
                                          string confirmPassword,
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

        if (newPassword != confirmPassword)
        {
            ViewBag.Error = "パスワードが異なります";
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();
            ViewBag.AllDivisions = await _context.Divisions.ToListAsync();
            ViewBag.AllLicenses = await _context.Licenses.ToListAsync();
			return View(employee);
        }

        if (!string.IsNullOrEmpty(newPassword))
        {
            employee.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        };

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

        ViewBag.AllDepartments = await _context.Departments
            .Include(d => d.Divisions) 
            .ToListAsync();
        ViewBag.AllLicenses = await _context.Licenses.ToListAsync();

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
