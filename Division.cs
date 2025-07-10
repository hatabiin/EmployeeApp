using System;
using System.Collections.Generic;

namespace EmployeeApp;

public partial class Division
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public int? DepartmentId { get; set; }

    public string DivisionName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Department? Department { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
