using Microsoft.AspNetCore.Http;
using QFun.Data.DbTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QFun.Models
{
    public class contributionUploadVm
    {
        public IFormFile Image { get; set; }
        public DateTime TimeOfUpload { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ChallengeId { get; set; }
        public Challenge Challenge { get; set; }
        public List<Vote> Votes { get; set; }
    }
}
