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
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.Prooves = new HashSet<Proove>();
            this.PaymentsRecords = new HashSet<PaymentsRecord>();
        }
    
        public int id { get; set; }
        public Nullable<int> Employeeid { get; set; }
        public Nullable<int> Bndid { get; set; }
        public string Subject { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> PeacePrice { get; set; }
        public string State { get; set; }
        public Nullable<bool> FirstLevelSign { get; set; }
        public Nullable<bool> SecondLevelSign { get; set; }
        public Nullable<bool> ThirdLevelSign { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.TimeSpan> Time { get; set; }
        public Nullable<int> CenterID { get; set; }
        public Nullable<int> SubBndid { get; set; }
    
        public virtual Bnd Bnd { get; set; }
        public virtual Center Center { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual SubBnd SubBnd { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Proove> Prooves { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaymentsRecord> PaymentsRecords { get; set; }
    }
}
