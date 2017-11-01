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

    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            this.Concerns = new HashSet<Concern>();
            this.DailyActivities = new HashSet<DailyActivity>();
            this.Orders = new HashSet<Order>();
            this.Prooves = new HashSet<Proove>();
        }

        public int id { get; set; }
        [Required(ErrorMessage = "���� ����� �����")]
        [DisplayName("��� ������")]
        public string name { get; set; }
        [Required(ErrorMessage = "���� ����� ������")]
        [DisplayName("���� ������")]
        public string surname { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]

        [Required(ErrorMessage = "���� ����� ����� �������")]
        [DisplayName("����� �������")]
        public Nullable<System.DateTime> BDate { get; set; }
        [Required(ErrorMessage = "���� ����� �������")]
        [DisplayName("�������")]
        public string Certificate { get; set; }
        [Required(ErrorMessage = "���� ����� ��� �������")]
        [DisplayName("��� �������")]
        public string CType { get; set; }
        [DisplayName("������")]
        public string State { get; set; }
        public Nullable<int> Centerid { get; set; }
        public Nullable<int> Periodid { get; set; }
        [Required(ErrorMessage = "���� ����� ����� �����")]
        [DisplayName("����� �����")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]

        public Nullable<System.DateTime> SDate { get; set; }
        [DisplayName("����� ��������")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> EDate { get; set; }
        public Nullable<int> Job { get; set; }
        [Required(ErrorMessage = "���� ����� ��� ��������")]
        [DisplayName("��� ��������")]

        public string Username { get; set; }
        [DisplayName("���� ������")]

        public string Password { get; set; }
        public string Proof { get; set; }
        [Required(ErrorMessage = "���� ����� ������")]
        [DisplayName("������")]
        public Nullable<double> Salary { get; set; }
        public Nullable<int> CityID { get; set; }
        [Required(ErrorMessage = "���� ����� ��� ����")]
        [DisplayName("��� ����")]
        public string FathersName { get; set; }
        [Required(ErrorMessage = "���� ����� �����")]
        [DisplayName("�����")]
        public string Sex { get; set; }

        public virtual Center Center { get; set; }
        public virtual City City { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Concern> Concerns { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DailyActivity> DailyActivities { get; set; }
        public virtual EmployeeType EmployeeType { get; set; }
        public virtual Period Period { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Proove> Prooves { get; set; }
    }
}
