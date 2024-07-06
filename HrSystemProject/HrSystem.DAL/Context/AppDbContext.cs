using HrSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace HrSystem.DAL.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserLogin<string>>().HasKey(x => new { x.LoginProvider, x.ProviderKey });
            builder.Entity<IdentityUserRole<string>>().HasKey(x => new { x.UserId, x.RoleId });
            builder.Entity<IdentityUserToken<string>>().HasKey(x => new { x.UserId, x.LoginProvider, x.Name });
            builder.Entity<Attendance>()
            .HasOne(a => a.Employee)
            .WithMany(e => e.Attendances)
            .HasForeignKey(a => a.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<SalaryReport>()
            .HasOne(a => a.Employee)
            .WithMany(e => e.SalaryReports)
            .HasForeignKey(a => a.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Employee>()
                .Property(x => x.WorkHours)
                .HasComputedColumnSql("DATEDIFF(HOUR, CheckInTime, ISNULL(CheckOutTime, CURRENT_TIMESTAMP))");


            builder.Entity<GeneralSettingsNew>()
                .HasKey(x => new { x.EmployeeId, x.VacationId });

            builder.Entity<GeneralSettingsNew>()
                .HasOne(es => es.Employee)
                .WithMany(e => e.GeneralSettings)
                .HasForeignKey(es => es.EmployeeId);

            builder.Entity<GeneralSettingsNew>()
                .HasOne(es => es.Vacation)
                .WithMany(e => e.GeneralSettings)
                .HasForeignKey(es => es.VacationId);

            builder.Entity<IdentityUserRole<string>>()
           .HasOne<ApplicationUser>()
           .WithMany()
           .HasForeignKey(x => x.UserId)
           .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<IdentityUserRole<string>>()
                .HasOne<ApplicationRole>()
                .WithMany()
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

        }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<GeneralSetting> GeneralSettings { get; set; }
        public DbSet<SalaryReport> SalaryReports { get; set; }
        public DbSet<Vacation> Vacations { get; set; }
        public DbSet<GeneralSettingsNew> GeneralSettingsNew { get; set; }


    }
}
