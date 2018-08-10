using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.EntityFrameworkCore;
using POC.Core;
using POC.Core.Data;

namespace POC.Data
{
    public class PocDbContext : DbContext, IDbContext
    {
        
        public PocDbContext(DbContextOptions<PocDbContext> options)
    : base(options)
        { }

        public DbSet<Student> Student { get; set; }

        //public virtual DbSet<Student> Students { get; set; }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }

        //public DbSet<Course> Courses { get; set; }
        //public DbSet<Enrollment> Enrollments { get; set; }
        //public DbSet<Student> Students { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Horse>().HasOne(p => p.Color).WithMany(b => b.Horses).HasForeignKey(p => p.ColorId).IsRequired();
            //modelBuilder.Entity<Horse>().HasOne(p => p.Color).WithMany(b => b.Horses).HasForeignKey(p => p.ColorId).IsRequired();
        }







    }
}
