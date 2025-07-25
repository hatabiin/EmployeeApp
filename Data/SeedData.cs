using EmployeeApp.Data;
using EmployeeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            try
            {
                context.Database.EnsureCreated();

                // 既存データを全削除
                var existingEmployees = context.Employees.ToList();
                context.Employees.RemoveRange(existingEmployees);
                
                var existingDivisions = context.Divisions.ToList();
                context.Divisions.RemoveRange(existingDivisions);
                
                var existingDepartments = context.Departments.ToList();
                context.Departments.RemoveRange(existingDepartments);
                
                var existingLicenses = context.Licenses.ToList();
                context.Licenses.RemoveRange(existingLicenses);
                
                var existingCompanies = context.Companies.ToList();
                context.Companies.RemoveRange(existingCompanies);
                
                context.SaveChanges();
                
                // AUTO_INCREMENT をリセット
                try
                {
                    context.Database.ExecuteSqlRaw("ALTER TABLE companies AUTO_INCREMENT = 1");
                    context.Database.ExecuteSqlRaw("ALTER TABLE departments AUTO_INCREMENT = 1");
                    context.Database.ExecuteSqlRaw("ALTER TABLE divisions AUTO_INCREMENT = 1");
                    context.Database.ExecuteSqlRaw("ALTER TABLE employees AUTO_INCREMENT = 1");
                    context.Database.ExecuteSqlRaw("ALTER TABLE licenses AUTO_INCREMENT = 1");
                }
                catch { }

                // 会社データ
                var company1 = new Company { CompanyName = "AAA株式会社" };
                var company2 = new Company { CompanyName = "BBB商事" };
                var company3 = new Company { CompanyName = "CCC工業" };

                context.Companies.AddRange(company1, company2, company3);
                context.SaveChanges();

                // 部署データ
                var sales1 = new Department { CompanyId = company1.Id, DepartmentName = "営業部" };
                var dev1 = new Department { CompanyId = company1.Id, DepartmentName = "開発部" };
                var admin1 = new Department { CompanyId = company1.Id, DepartmentName = "総務部" };

                var sales2 = new Department { CompanyId = company2.Id, DepartmentName = "営業部" };
                var dev2 = new Department { CompanyId = company2.Id, DepartmentName = "開発部" };
                var admin2 = new Department { CompanyId = company2.Id, DepartmentName = "総務部" };

                var sales3 = new Department { CompanyId = company3.Id, DepartmentName = "営業部" };
                var dev3 = new Department { CompanyId = company3.Id, DepartmentName = "開発部" };
                var admin3 = new Department { CompanyId = company3.Id, DepartmentName = "総務部" };

                context.Departments.AddRange(
                    sales1, dev1, admin1,
                    sales2, dev2, admin2,
                    sales3, dev3, admin3
                );
                context.SaveChanges();

                // 課データ
                var companies = new[] { company1, company2, company3 };
                var departmentsByCompany = new[]
                {
                    new[] { sales1, dev1, admin1 }, 
                    new[] { sales2, dev2, admin2 },   
                    new[] { sales3, dev3, admin3 }  
                };
                var divisionNames = new[] { "営業課", "企画課", "管理課" };
                var divisions = new List<Division>();

                for (int i = 0; i < companies.Length; i++)
                {
                    var company = companies[i];
                    var departments = departmentsByCompany[i];

                    foreach (var dept in departments)
                    {
                        foreach (var divName in divisionNames)
                        {
                            divisions.Add(new Division
                            {
                                CompanyId = company.Id,
                                DepartmentId = dept.Id,
                                DivisionName = divName
                            });
                        }
                    }
                }
                context.Divisions.AddRange(divisions);
                context.SaveChanges();

                // 資格データ
                var basicIT = new License { LicenseName = "基本情報技術者" };
                var boki2 = new License { LicenseName = "簿記2級" };
                var license = new License { LicenseName = "普通自動車免許" };

                context.Licenses.AddRange(basicIT, boki2, license);
                context.SaveChanges();

                // 社員データ
                var testUser = new Employee
                {
                    EmployeeName = "管理者ユーザー",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("test123"),
                    CompanyId = company1.Id
                };

                var bbbUser1 = new Employee 
                { 
                    EmployeeName = "BBB太郎", 
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("test123"),
                    CompanyId = company2.Id 
                };

                var bbbUser2 = new Employee 
                { 
                    EmployeeName = "BBB花子", 
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("test123"),
                    CompanyId = company2.Id 
                };

                var cccUser1 = new Employee 
                { 
                    EmployeeName = "CCC次郎", 
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("test123"),
                    CompanyId = company3.Id  
                };
                context.Employees.AddRange(testUser, bbbUser1, bbbUser2, cccUser1);
                context.SaveChanges();

                // 関連付け
                var salesDept = context.Departments.First(d => d.DepartmentName == "営業部");
                var salesDiv = context.Divisions.First(d => d.DepartmentId == salesDept.Id && d.DivisionName == "営業課");

                testUser.Departments.Add(salesDept);
                testUser.Divisions.Add(salesDiv);
                testUser.Licenses.Add(basicIT);

                context.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}