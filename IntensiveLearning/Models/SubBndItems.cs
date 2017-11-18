using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveLearning.Models
{
    public class SubBndItems
    {
        public string Subject { get; set; }
        public double? PeacePrice { get; set; }
        public int? Quantity { get; set; }
        public string Center { get; set; }
        public int? Bndid { get; set; }
        public string Bnd { get; set; }
        public int? Centerid { get; set; }
        public string CenterDepended { get; set; }
        public DateTime? Date { get; set; }
        public int id { get; set; }
        public double? SumPrice { get; set; }
        public bool? PayymentApprove { get; set; }
        public bool? BuyingApprove { get; set; }
        public bool? ProofAcceptance { get; set; }
        public object proof { get; set; }


    }
}