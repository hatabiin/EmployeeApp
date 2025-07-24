using System;
using System.Collections.Generic;

namespace EmployeeApp.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string EmployeeName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int CompanyId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<Division> Divisions { get; set; } = new List<Division>();

    public virtual ICollection<License> Licenses { get; set; } = new List<License>();

    public virtual Company Company { get; set; }
}
