using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployeeApp.Models;
using Microsoft.AspNetCore.Authorization;
using EmployeeApp.Data;

namespace EmployeeApp.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
	private readonly ApplicationDbContext _context;
	public AccountController(ApplicationDbContext context)
	{
		_context = context;
	}
	[HttpGet]
	public ActionResult Login()
	{
		return View();
	}
	[HttpPost]
	public ActionResult Login(string employeeId, string password)
	{
		Console.WriteLine($"ID: '{employeeId}', Pass:'{password}'");
		var is_id = string.IsNullOrEmpty(employeeId);
		var is_pass = string.IsNullOrEmpty(password);
		
		Console.WriteLine($"is_id: '{is_id}', is_pass:'{is_pass}'");

		if (is_id || is_pass)
		{
			ViewBag.Error = "IDまたはパスワードがちがいます";
			return View();
		}
		else
		{
			var employee = _context
			.Employees.FirstOrDefault(e => e.Id.ToString() == employeeId);
			if (employee == null)
			{
				ViewBag.Error = "IDまたはパスワードがちがいます";
				return View();
			}

			if (employee.PasswordHash != password || employee.PasswordHash == null)
			{
				ViewBag.Error = "IDまたはパスワードがちがいます";
				return View();
			}

			return RedirectToAction("Index", "Home");
		}
	}
}