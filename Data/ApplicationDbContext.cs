using System;
using System.Collections.Generic;
using EmployeeApp.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace EmployeeApp.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Division> Divisions { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<License> Licenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder
				.UseCollation("utf8mb4_0900_ai_ci")
				.HasCharSet("utf8mb4");

		modelBuilder.Entity<Company>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PRIMARY");

			entity.ToTable("companies");

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.CompanyName)
							.HasMaxLength(200)
							.HasColumnName("company_name");
			entity.Property(e => e.CreatedAt)
							.HasDefaultValueSql("CURRENT_TIMESTAMP")
							.HasColumnType("timestamp")
							.HasColumnName("created_at");
			entity.Property(e => e.UpdatedAt)
							.ValueGeneratedOnAddOrUpdate()
							.HasDefaultValueSql("CURRENT_TIMESTAMP")
							.HasColumnType("timestamp")
							.HasColumnName("updated_at");
		});

		modelBuilder.Entity<Department>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PRIMARY");

			entity.ToTable("departments");

			entity.HasIndex(e => e.CompanyId, "company_id");

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.CompanyId).HasColumnName("company_id");
			entity.Property(e => e.CreatedAt)
							.HasDefaultValueSql("CURRENT_TIMESTAMP")
							.HasColumnType("timestamp")
							.HasColumnName("created_at");
			entity.Property(e => e.DepartmentName)
							.HasMaxLength(200)
							.HasColumnName("department_name");
			entity.Property(e => e.UpdatedAt)
							.ValueGeneratedOnAddOrUpdate()
							.HasDefaultValueSql("CURRENT_TIMESTAMP")
							.HasColumnType("timestamp")
							.HasColumnName("updated_at");

			entity.HasOne(d => d.Company).WithMany(p => p.Departments)
							.HasForeignKey(d => d.CompanyId)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("departments_ibfk_1");
		});

		modelBuilder.Entity<Division>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PRIMARY");

			entity.ToTable("divisions");

			entity.HasIndex(e => e.CompanyId, "company_id");

			entity.HasIndex(e => e.DepartmentId, "department_id");

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.CompanyId).HasColumnName("company_id");
			entity.Property(e => e.CreatedAt)
							.HasDefaultValueSql("CURRENT_TIMESTAMP")
							.HasColumnType("timestamp")
							.HasColumnName("created_at");
			entity.Property(e => e.DepartmentId).HasColumnName("department_id");
			entity.Property(e => e.DivisionName)
							.HasMaxLength(200)
							.HasColumnName("division_name");
			entity.Property(e => e.UpdateAt)
							.ValueGeneratedOnAddOrUpdate()
							.HasDefaultValueSql("CURRENT_TIMESTAMP")
							.HasColumnType("timestamp")
							.HasColumnName("update_at");

			entity.HasOne(d => d.Company).WithMany(p => p.Divisions)
							.HasForeignKey(d => d.CompanyId)
							.OnDelete(DeleteBehavior.ClientSetNull)
							.HasConstraintName("divisions_ibfk_1");

			entity.HasOne(d => d.Department).WithMany(p => p.Divisions)
							.HasForeignKey(d => d.DepartmentId)
							.HasConstraintName("divisions_ibfk_2");
		});

		modelBuilder.Entity<Employee>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PRIMARY");

			entity.ToTable("employees");

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.CreatedAt)
							.HasDefaultValueSql("CURRENT_TIMESTAMP")
							.HasColumnType("timestamp")
							.HasColumnName("created_at");
			entity.Property(e => e.EmployeeName)
							.HasMaxLength(200)
							.HasColumnName("employee_name");
			entity.Property(e => e.PasswordHash)
							.HasMaxLength(255)
							.HasColumnName("password_hash");
			entity.Property(e => e.UpdatedAt)
							.ValueGeneratedOnAddOrUpdate()
							.HasDefaultValueSql("CURRENT_TIMESTAMP")
							.HasColumnType("timestamp")
							.HasColumnName("updated_at");

			entity.HasMany(d => d.Departments).WithMany(p => p.Employees)
							.UsingEntity<Dictionary<string, object>>(
									"DepartmentAssignment",
									r => r.HasOne<Department>().WithMany()
											.HasForeignKey("DepartmentId")
											.HasConstraintName("department_assignments_ibfk_2"),
									l => l.HasOne<Employee>().WithMany()
											.HasForeignKey("EmployeeId")
											.HasConstraintName("department_assignments_ibfk_1"),
									j =>
									{
									j.HasKey("EmployeeId", "DepartmentId")
													.HasName("PRIMARY")
													.HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
									j.ToTable("department_assignments");
									j.HasIndex(new[] { "DepartmentId" }, "department_id");
									j.IndexerProperty<int>("EmployeeId").HasColumnName("employee_id");
									j.IndexerProperty<int>("DepartmentId").HasColumnName("department_id");
								});

			entity.HasMany(d => d.Divisions).WithMany(p => p.Employees)
							.UsingEntity<Dictionary<string, object>>(
									"DivisionAssignment",
									r => r.HasOne<Division>().WithMany()
											.HasForeignKey("DivisionId")
											.HasConstraintName("division_assignments_ibfk_2"),
									l => l.HasOne<Employee>().WithMany()
											.HasForeignKey("EmployeeId")
											.HasConstraintName("division_assignments_ibfk_1"),
									j =>
									{
									j.HasKey("EmployeeId", "DivisionId")
													.HasName("PRIMARY")
													.HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
									j.ToTable("division_assignments");
									j.HasIndex(new[] { "DivisionId" }, "division_id");
									j.IndexerProperty<int>("EmployeeId").HasColumnName("employee_id");
									j.IndexerProperty<int>("DivisionId").HasColumnName("division_id");
								});

			entity.HasMany(d => d.Licenses).WithMany(p => p.Employees)
							.UsingEntity<Dictionary<string, object>>(
									"Holding",
									r => r.HasOne<License>().WithMany()
											.HasForeignKey("LicenseId")
											.HasConstraintName("holdings_ibfk_2"),
									l => l.HasOne<Employee>().WithMany()
											.HasForeignKey("EmployeeId")
											.HasConstraintName("holdings_ibfk_1"),
									j =>
									{
									j.HasKey("EmployeeId", "LicenseId")
													.HasName("PRIMARY")
													.HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
									j.ToTable("holdings");
									j.HasIndex(new[] { "LicenseId" }, "license_id");
									j.IndexerProperty<int>("EmployeeId").HasColumnName("employee_id");
									j.IndexerProperty<int>("LicenseId").HasColumnName("license_id");
								});
		});

		modelBuilder.Entity<License>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PRIMARY");

			entity.ToTable("licenses");

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.CreatedAt)
							.HasDefaultValueSql("CURRENT_TIMESTAMP")
							.HasColumnType("timestamp")
							.HasColumnName("created_at");
			entity.Property(e => e.LicenseName)
							.HasMaxLength(200)
							.HasColumnName("license_name");
			entity.Property(e => e.UpdatedAt)
							.ValueGeneratedOnAddOrUpdate()
							.HasDefaultValueSql("CURRENT_TIMESTAMP")
							.HasColumnType("timestamp")
							.HasColumnName("updated_at");
		});

		OnModelCreatingPartial(modelBuilder);
	}

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
