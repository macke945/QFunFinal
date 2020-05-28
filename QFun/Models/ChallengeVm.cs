using QFun.Data.DbTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QFun.Models
{
    public class ChallengeVm
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public ICollection<Challenge> Challenges { get; set; } = new List<Challenge>();
    }
}
