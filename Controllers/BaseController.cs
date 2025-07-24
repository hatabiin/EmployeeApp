using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EmployeeApp.Data;
using EmployeeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Controllers;

public class BaseController : Controller
{
	protected readonly ApplicationDbContext _context;

	public BaseController(ApplicationDbContext context)
	{
		_context = context;
	}

	protected int GetCurrentUserId()
	{
		var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		return int.TryParse(userIdClaim, out int userId) ? userId : 0;
	}

	protected bool IsMasterUser()
	{
		return GetCurrentUserId() == 1;
	}

	protected async Task<int> GetCurrentCompanyIdAsync()
	{
		if (IsMasterUser())
		{
			return HttpContext.Session.GetInt32("CurrentCompanyId") ?? 1;
		}
		else
		{
			var userId = GetCurrentUserId();
			var employee = await _context.Employees.FindAsync(userId);
			return employee.CompanyId;
		}
	}
}