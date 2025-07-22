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
                var company = new Company { CompanyName = "テックソリューション株式会社" };
                context.Companies.Add(company);
                context.SaveChanges();

                // 部署データ（3つに減らす）
                var sales = new Department { CompanyId = company.Id, DepartmentName = "営業部" };
                var dev = new Department { CompanyId = company.Id, DepartmentName = "開発部" };
                var admin = new Department { CompanyId = company.Id, DepartmentName = "総務部" };

                context.Departments.AddRange(sales, dev, admin);
                context.SaveChanges();

                // 課データ - 各部署ごとに3種類の課を作成
                var divisions = new List<Division>();

                // 営業部の課（3種類）
                divisions.Add(new Division { CompanyId = company.Id, DepartmentId = sales.Id, DivisionName = "営業課" });
                divisions.Add(new Division { CompanyId = company.Id, DepartmentId = sales.Id, DivisionName = "企画課" });
                divisions.Add(new Division { CompanyId = company.Id, DepartmentId = sales.Id, DivisionName = "管理課" });

                // 開発部の課（3種類）
                divisions.Add(new Division { CompanyId = company.Id, DepartmentId = dev.Id, DivisionName = "営業課" });
                divisions.Add(new Division { CompanyId = company.Id, DepartmentId = dev.Id, DivisionName = "企画課" });
                divisions.Add(new Division { CompanyId = company.Id, DepartmentId = dev.Id, DivisionName = "管理課" });

                // 総務部の課（3種類）
                divisions.Add(new Division { CompanyId = company.Id, DepartmentId = admin.Id, DivisionName = "営業課" });
                divisions.Add(new Division { CompanyId = company.Id, DepartmentId = admin.Id, DivisionName = "企画課" });
                divisions.Add(new Division { CompanyId = company.Id, DepartmentId = admin.Id, DivisionName = "管理課" });

                context.Divisions.AddRange(divisions);
                context.SaveChanges();

                // 資格データ（3つに減らす）
                var basicIT = new License { LicenseName = "基本情報技術者" };
                var boki2 = new License { LicenseName = "簿記2級" };
                var license = new License { LicenseName = "普通自動車免許" };

                context.Licenses.AddRange(basicIT, boki2, license);
                context.SaveChanges();

                // 社員データ（1人）
                var testUser = new Employee 
                { 
                    EmployeeName = "テストユーザー", 
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("test123")
                };
                context.Employees.Add(testUser);
                context.SaveChanges();

                // 関連付け（営業部の営業課に所属させる例）
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