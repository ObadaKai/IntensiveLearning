//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Center
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Center()
        {
            this.Employees = new HashSet<Employee>();
            this.Orders = new HashSet<Order>();
            this.Students = new HashSet<Student>();
            this.Prooves = new HashSet<Proove>();
        }
    
        public int id { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string Desc { get; set; }
        public string State { get; set; }
        public Nullable<int> HolesN { get; set; }
        public Nullable<int> Cityid { get; set; }
        public string FoundationType { get; set; }
        public Nullable<int> Periodid { get; set; }
        public string Notes { get; set; }
        public string Owner { get; set; }
        public Nullable<bool> Month10 { get; set; }
        public Nullable<bool> Month11 { get; set; }
        public Nullable<bool> Month12 { get; set; }
        public Nullable<bool> Month1 { get; set; }
        public Nullable<bool> Month2 { get; set; }
        public Nullable<bool> Month3 { get; set; }
        public Nullable<bool> Month4 { get; set; }
        public Nullable<bool> Month5 { get; set; }
        public Nullable<bool> Month6 { get; set; }
        public Nullable<bool> Month7 { get; set; }
        public Nullable<bool> Month8 { get; set; }
        public Nullable<bool> Month9 { get; set; }
        public Nullable<int> TargetedMen { get; set; }
        public Nullable<int> TargetedWomen { get; set; }
        public string CenterType { get; set; }
        public Nullable<int> MonthlyPayment { get; set; }
        public Nullable<int> QuarterPayment { get; set; }
        public string Proof { get; set; }
        public Nullable<int> ProjectID { get; set; }
    
        public virtual City City { get; set; }
        public virtual Period Period { get; set; }
        public virtual Project Project { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employee> Employees { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Student> Students { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Proove> Prooves { get; set; }
    }
}
