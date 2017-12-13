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
            this.Centers = new HashSet<Center>();
            this.Centers1 = new HashSet<Center>();
            this.Concerns = new HashSet<Concern>();
            this.DailyActivities = new HashSet<DailyActivity>();
            this.Employees1 = new HashSet<Employee>();
            this.Employees11 = new HashSet<Employee>();
            this.Examinations = new HashSet<Examination>();
            this.Examinations1 = new HashSet<Examination>();
            this.Missions = new HashSet<Mission>();
            this.Missions1 = new HashSet<Mission>();
            this.Missions2 = new HashSet<Mission>();
            this.MissionPersonInCharges = new HashSet<MissionPersonInCharge>();
            this.MissionResponses = new HashSet<MissionResponse>();
            this.Orders = new HashSet<Order>();
            this.Presences = new HashSet<Presence>();
            this.Presences1 = new HashSet<Presence>();
            this.Prooves = new HashSet<Proove>();
            this.Students = new HashSet<Student>();
            this.Students1 = new HashSet<Student>();
            this.SubBnds = new HashSet<SubBnd>();
        }
        public int id { get; set; }
        [Required(ErrorMessage = "���� ����� �����")]
        [DisplayName("��� ������")]
        public string name { get; set; }
        [Required(ErrorMessage = "���� ����� ������")]
        [DisplayName("���� ������")]
        public string surname { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]

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
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]

        public Nullable<System.DateTime> SDate { get; set; }
        [DisplayName("����� ��������")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
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
        public Nullable<bool> Approval { get; set; }
        public Nullable<int> AddedBy { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        [DisplayName("����� �������")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]

        public Nullable<System.DateTime> AddingDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh\\:mm}")]
        [DisplayName("��� �������")]
        public Nullable<System.TimeSpan> AddingTime { get; set; }
        [DisplayName("��� ����� ������")]
        public Nullable<int> ExpYears { get; set; }
        [DisplayName("������� �������")]
        public string OldJob { get; set; }
        [DisplayName("���� �� ���� ������")]
        public string InsideOrOutside { get; set; }
        [DisplayName("��� ������")]
        public Nullable<long> telephoneNumber { get; set; }
        [DisplayName("�������")]
        public string Address { get; set; }
        [DisplayName("�������")]
        public string Email { get; set; }
        [DisplayName("����� ��� ����")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]

        public Nullable<System.DateTime> DateOfLastEntry { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh\\:mm}")]
        [DisplayName("��� ��� ����")]

        public Nullable<System.TimeSpan> TimeOfLastEntry { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Center> Centers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Center> Centers1 { get; set; }
        public virtual Center Center { get; set; }
        public virtual City City { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Concern> Concerns { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DailyActivity> DailyActivities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employee> Employees1 { get; set; }
        public virtual Employee Employee1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employee> Employees11 { get; set; }
        public virtual Employee Employee2 { get; set; }
        public virtual EmployeeType EmployeeType { get; set; }
        public virtual Period Period { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Examination> Examinations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Examination> Examinations1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mission> Missions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mission> Missions1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mission> Missions2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MissionPersonInCharge> MissionPersonInCharges { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MissionResponse> MissionResponses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Presence> Presences { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Presence> Presences1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Proove> Prooves { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Student> Students { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Student> Students1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubBnd> SubBnds { get; set; }
    }
}
