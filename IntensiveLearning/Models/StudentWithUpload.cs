using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IntensiveLearning.Database;

namespace IntensiveLearning.Models
{
    public class StudentWithUpload
    {
        public Student student { get; set; }
        public HttpRequestBase FormData { get; set; }
    }
}