using Microsoft.AspNetCore.Identity;
using QFun.Data.DbTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QFun.Models.ContributionsVms
{
    public class ContributionsVm
    {
        public ICollection<Contribution> Contributions { get; set; } = new List<Contribution>();
        public int ContributionsId { get; set; }
        public int ChallengeId { get; set; }
        public string Path { get; set; }
        public DateTime TimeOfUpload { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public Challenge Challenge { get; set; }
        public List<Vote> Votes { get; set; } = new List<Vote>();
        public IList<ContributionUserData> ContributionUserData = new List<ContributionUserData>();
    }
}
