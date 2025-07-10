using System;
using System.Collections.Generic;

namespace EmployeeApp.Models;

public partial class Department
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public string DapartmentName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Division> Divisions { get; set; } = new List<Division>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
