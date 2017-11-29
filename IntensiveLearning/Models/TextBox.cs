using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveLearning.Models
{
    public class TextBox
    {
        public string SearchBoxData { get; set; }
        public DateTime SearchBoxDate { get; set; }
        public int? CitiesChange { get; set; }
        public int? CentersChange { get; set; }
        public int? RegimentsChange { get; set; }
        public int? PeriodsChange { get; set; }
        public int? StagesChange { get; set; }
        public int? EmployeeTypesChange { get; set; }
    }
}