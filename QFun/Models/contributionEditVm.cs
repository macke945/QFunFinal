using Microsoft.AspNetCore.Http;
using QFun.Data.DbTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QFun.Models
{
    public class contributionEditVm
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int ChallengeId { get; set; }
        public string UserName { get; set; }
        public bool ShowEditCheatError { get; internal set; }
    }
}
