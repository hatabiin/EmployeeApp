using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployeeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
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
	public async Task<ActionResult> Login(string employeeId, string password)
	{
		var is_id = string.IsNullOrEmpty(employeeId);
		var is_pass = string.IsNullOrEmpty(password);

		if (is_id || is_pass)
		{
			ViewBag.Error = "IDまたはパスワードがちがいます";
			return View();
		}
		else
		{
			var employee = _context
			.Employees.FirstOrDefault(e => e.Id.ToString() == employeeId);

			var CurrentCompanyId = employee.CompanyId;

			if (employee.Id == 1)
			{
				HttpContext.Session.SetInt32("CurrentCompanyId", 1);
			}
			else
			{
	    		HttpContext.Session.SetInt32("CurrentCompanyId", CurrentCompanyId);
			}

			if (employee == null)
			{
				ViewBag.Error = "IDまたはパスワードがちがいます";
				return View();
			}

			if (!BCrypt.Net.BCrypt.Verify(password, employee.PasswordHash) || employee.PasswordHash == null)
			{
				ViewBag.Error = "IDまたはパスワードがちがいます";
				return View();
			}

			// 認証情報を作成
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, employee.EmployeeName),
				new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString())
			};

			var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
			var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

			// 認証状態を記録
			await HttpContext.SignInAsync("Cookies", claimsPrincipal);

			return RedirectToAction("Index", "Home");
		}
	}

	[HttpPost]
	public async Task<ActionResult> Logout()
	{
		await HttpContext.SignOutAsync("Cookies");
		return RedirectToAction("Login", "Account");
	}
}