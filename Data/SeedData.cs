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

                // 部署データ
                var sales = new Department { CompanyId = company.Id, DepartmentName = "営業部" };
                var dev = new Department { CompanyId = company.Id, DepartmentName = "開発部" };
                var admin = new Department { CompanyId = company.Id, DepartmentName = "総務部" };
                var planning = new Department { CompanyId = company.Id, DepartmentName = "企画部" };
                var quality = new Department { CompanyId = company.Id, DepartmentName = "品質管理部" };

                context.Departments.AddRange(sales, dev, admin, planning, quality);
                context.SaveChanges();

                // 課データ
                var sales1 = new Division { CompanyId = company.Id, DepartmentId = sales.Id, DivisionName = "営業1課" };
                var sales2 = new Division { CompanyId = company.Id, DepartmentId = sales.Id, DivisionName = "営業2課" };
                var overseas = new Division { CompanyId = company.Id, DepartmentId = sales.Id, DivisionName = "海外営業課" };
                var system = new Division { CompanyId = company.Id, DepartmentId = dev.Id, DivisionName = "システム開発課" };
                var web = new Division { CompanyId = company.Id, DepartmentId = dev.Id, DivisionName = "Web開発課" };
                var mobile = new Division { CompanyId = company.Id, DepartmentId = dev.Id, DivisionName = "モバイル開発課" };
                var hr = new Division { CompanyId = company.Id, DepartmentId = admin.Id, DivisionName = "人事課" };
                var accounting = new Division { CompanyId = company.Id, DepartmentId = admin.Id, DivisionName = "経理課" };
                var product = new Division { CompanyId = company.Id, DepartmentId = planning.Id, DivisionName = "商品企画課" };
                var qa = new Division { CompanyId = company.Id, DepartmentId = quality.Id, DivisionName = "品質保証課" };

                context.Divisions.AddRange(sales1, sales2, overseas, system, web, mobile, hr, accounting, product, qa);
                context.SaveChanges();

                // 資格データ
                var basicIT = new License { LicenseName = "基本情報技術者" };
                var advancedIT = new License { LicenseName = "応用情報技術者" };
                var boki2 = new License { LicenseName = "簿記2級" };
                var boki1 = new License { LicenseName = "簿記1級" };
                var toeic800 = new License { LicenseName = "TOEIC800点以上" };
                var toeic900 = new License { LicenseName = "TOEIC900点以上" };
                var license = new License { LicenseName = "普通自動車免許" };
                var pmp = new License { LicenseName = "PMP(プロジェクトマネジメント)" };

                context.Licenses.AddRange(basicIT, advancedIT, boki2, boki1, toeic800, toeic900, license, pmp);
                context.SaveChanges();

                // 社員データ（1人）
                var testUser = new Employee { EmployeeName = "テストユーザー", PasswordHash = "test123" };
                context.Employees.Add(testUser);
                context.SaveChanges();

                // 関連付け
                testUser.Departments.Add(sales);
                testUser.Divisions.Add(sales1);
                testUser.Licenses.Add(basicIT);
                testUser.Licenses.Add(license);

                context.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}