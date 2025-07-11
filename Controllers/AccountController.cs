using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployeeApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeApp.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
	[HttpGet]
	public ActionResult Login()
	{
		return View();
	}
	[HttpPost]
	public ActionResult Login(string employee_id, string password_hash)
	{
		if (string.IsNullOrEmpty(employee_id) || string.IsNullOrEmpty(password_hash))
		{
			return View();
		}
		return View();
	}
}