using IntensiveLearning.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveLearning.Models
{
    public class MissionsModel
    {
        public Mission mission { get; set; }
        public int ManagerId { get; set; }
        public int[] PeopleInCharge { get; set; }
    }
}