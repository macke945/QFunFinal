using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QFun.Data.DbTables
{
    public class Contribution
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public DateTime TimeOfUpload { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public int ChallengeId { get; set; }
        public Challenge Challenge { get; set; }
        public List<Vote> Votes { get; set; }
    }
}
