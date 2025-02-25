﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QFun.Data.DbTables
{
    public class Vote
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ContributionId { get; set; }
        public Contribution Contribution { get; set; }
    }
}
