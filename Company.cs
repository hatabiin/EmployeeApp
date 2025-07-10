using System;
using System.Collections.Generic;

namespace EmployeeApp;

public partial class Company
{
    public int Id { get; set; }

    public string CompanyName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<Division> Divisions { get; set; } = new List<Division>();
}
