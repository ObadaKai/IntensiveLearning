using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using IntensiveLearning.Database;

namespace IntensiveLearning.Models
{
    public class CenterModel
    {
        public Center center { get; set; }
        [Required(ErrorMessage ="يرجى ادخال الشهر الاول")]
        [DisplayName("الشهر الأول")]
        public int firstMonth { get; set; }
        [Required(ErrorMessage = "يرجى ادخال الشهر الأخير")]
        [DisplayName("الشهر الأخير")]
        public int LastMonth { get; set; }
        public int Cityid { get; set; }

        public CenterModel() {
            center = new Center();
        }
    }
}