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
	public ActionResult Login(string employeeId, string password)
	{
		if (string.IsNullOrEmpty(employeeId) || string.IsNullOrEmpty(password))
		{
			return View();
		}
		return View();
	}
}