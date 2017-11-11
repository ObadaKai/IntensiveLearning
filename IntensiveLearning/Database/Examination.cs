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
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Examination
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Examination()
        {
            this.Prooves = new HashSet<Proove>();
        }

        public int id { get; set; }
        public Nullable<int> ExamTypeid { get; set; }
        [Required(ErrorMessage = "���� ����� �������")]
        [DisplayName("�������")]
        public Nullable<double> Mark { get; set; }
        [DisplayName("�����")]
        public string Desc { get; set; }
        public Nullable<int> Subjectid { get; set; }
        public Nullable<int> Studentid { get; set; }
        public Nullable<int> Stageid { get; set; }
        [Required(ErrorMessage = "���� ����� �������")]
        [DisplayName("�������")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]

        public Nullable<System.DateTime> Date { get; set; }
        public string Proof { get; set; }
        public Nullable<bool> Approval { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<int> AddedBy { get; set; }
        public Nullable<System.DateTime> AddingDate { get; set; }
        public Nullable<System.TimeSpan> AddingTime { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual Employee Employee1 { get; set; }
        public virtual ExamType ExamType { get; set; }
        public virtual Stage Stage { get; set; }
        public virtual Student Student { get; set; }
        public virtual Study_subject Study_subject { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Proove> Prooves { get; set; }
    }
}
