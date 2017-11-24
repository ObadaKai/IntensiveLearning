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

    public partial class Lesson
    {
        public int id { get; set; }
        [Required(ErrorMessage = "���� ����� �����")]
        [DisplayName("�����")]
        public string Day { get; set; }
        [DisplayName("������")]
        public string State { get; set; }
        [Required(ErrorMessage = "���� ����� ����� ")]
        [DisplayName("����� 1")]
        public string Lesson1 { get; set; }
        [Required(ErrorMessage = "���� ����� ����� ")]
        [DisplayName("����� 2")]
        public string Lesson2 { get; set; }
        [Required(ErrorMessage = "���� ����� ����� ")]
        [DisplayName("����� 3")]
        public string Lesson3 { get; set; }
        [Required(ErrorMessage = "���� ����� ����� ")]
        [DisplayName("����� 4")]
        public string Lesson4 { get; set; }
        [Required(ErrorMessage = "���� ����� ����� ")]
        [DisplayName("����� 5")]
        public string Lesson5 { get; set; }
        [DisplayName("����� 6")]
        public string Lesson6 { get; set; }
        [DisplayName("����� 7")]
        public string Lesson7 { get; set; }
        public Nullable<int> Regimentid { get; set; }
        public Nullable<int> Stageid { get; set; }
        public Nullable<int> Periodid { get; set; }
        public Nullable<int> Centerid { get; set; }
    
        public virtual Center Center { get; set; }
        public virtual Period Period { get; set; }
        public virtual Regiment Regiment { get; set; }
        public virtual Stage Stage { get; set; }
    }
}
