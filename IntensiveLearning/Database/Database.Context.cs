﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IntensiveLearning.Database
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TaalimEntities : DbContext
    {
        public TaalimEntities()
            : base("name=TaalimEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Bnd> Bnds { get; set; }
        public virtual DbSet<Center> Centers { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Concern> Concerns { get; set; }
        public virtual DbSet<DailyActivity> DailyActivities { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeType> EmployeeTypes { get; set; }
        public virtual DbSet<Examination> Examinations { get; set; }
        public virtual DbSet<ExamType> ExamTypes { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }
        public virtual DbSet<NonUserAddRequest> NonUserAddRequests { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Period> Periods { get; set; }
        public virtual DbSet<Presence> Presences { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Regiment> Regiments { get; set; }
        public virtual DbSet<Stage> Stages { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Study_subject> Study_subject { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Proove> Prooves { get; set; }
    }
}
