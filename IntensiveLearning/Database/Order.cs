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

    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.Prooves = new HashSet<Proove>();
            this.SubBnds = new HashSet<SubBnd>();
        }

        public int id { get; set; }
        public Nullable<int> Employeeid { get; set; }
        public Nullable<int> Bndid { get; set; }
        [DisplayName("������")]
        public string Subject { get; set; }
        [DisplayName("������")]
        public Nullable<int> Quantity { get; set; }
        [DisplayName("��� ������ ��������")]
        public Nullable<double> PeacePrice { get; set; }
        [DisplayName("������")]
        public string State { get; set; }
        [DisplayName("����� ��������")]
        public Nullable<bool> FirstLevelSign { get; set; }
        [DisplayName("����� �������")]
        public Nullable<bool> SecondLevelSign { get; set; }
        [DisplayName("����� ������ ��������")]
        public Nullable<bool> ThirdLevelSign { get; set; }
        [DisplayName("�������")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> Date { get; set; }
        [DisplayName("�����")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh\\:mm}")]
        public Nullable<System.TimeSpan> Time { get; set; }
        public Nullable<int> CenterID { get; set; }
        public Nullable<int> SubBndid { get; set; }
        [DisplayName("����� �����")]
        public Nullable<bool> PaymentApprove { get; set; }
        [DisplayName("����� ������")]
        public Nullable<bool> BuyingApprove { get; set; }
        [DisplayName("�������")]
        public Nullable<int> proof { get; set; }
        [DisplayName("����� �������")]
        public Nullable<bool> ProofAcceptance { get; set; }
        [DisplayName("����� ����� �����")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]

        public Nullable<System.DateTime> PaymentApprovalDate { get; set; }
        [DisplayName("����� ����� ������")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]

        public Nullable<System.DateTime> BuyingApprovalDate { get; set; }
        [DisplayName("����� ����� �������")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]

        public Nullable<System.DateTime> ProofAcceptanceDate { get; set; }
        public Nullable<int> Paymentid { get; set; }
        [DisplayName("����� �������")]

        public Nullable<short> Necessity { get; set; }
        [DisplayName("��� �����")]

        public string OrderType { get; set; }
        [DisplayName("��� �����")]

        public string CanclationReason { get; set; }
        [DisplayName("������� ����� ��������")]

        public Nullable<double> SumPrice { get; set; }
        [DisplayName("���� ������")]

        public Nullable<bool> QuantityChanged { get; set; }
        [DisplayName("����� ����� ��� �����")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]

        public Nullable<System.DateTime> PaymentOrderDate { get; set; }
        [DisplayName("��� ������ �������")]

        public Nullable<double> PeacePriceSyrian { get; set; }
        [DisplayName("��� ��� ������� �����")]

        public Nullable<double> CommissionPrice { get; set; }
        [DisplayName("������")]
        public string ItemUnit { get; set; }
    
        public virtual Bnd Bnd { get; set; }
        public virtual Center Center { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual SubBnd SubBnd { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Proove> Prooves { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubBnd> SubBnds { get; set; }
    }
}
