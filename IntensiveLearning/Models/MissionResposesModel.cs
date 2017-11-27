using IntensiveLearning.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveLearning.Models
{
    public class MissionResposesModel
    {
        public Mission mission { get; set; }
        public List<MissionResponse> missionResponses { get; set; }
    }
}