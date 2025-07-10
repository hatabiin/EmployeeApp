using System;
using System.Collections.Generic;

namespace EmployeeApp;

public partial class License
{
    public int Id { get; set; }

    public string LicenseName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
