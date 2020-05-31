using QFun.Data.DbTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QFun.Models.ContributionsVms
{
    public class ContributionUserData
    {
        public string UserName { get; set; }
        public List<Vote> Votes { get; set; } = new List<Vote>();
    }
}
