using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntensiveLearning.Models
{
    public class ChangePassword
    {

        [Required]
        [DisplayName("كلمة المرور الجديدة")]

        public string Newpassword { get; set; }
        [Required]
        [DisplayName("كلمة المرور الحالية")]

        public string password { get; set; }
        [Required]
        [DisplayName("اعادة كلمة المرور الحالية")]
        public string confPassword { get; set; }
    }
}