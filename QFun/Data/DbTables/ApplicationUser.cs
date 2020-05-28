using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QFun.Data.DbTables
{
    public class ApplicationUser : IdentityUser
    {
        public string AboutMe { get; set; }
        public string ImagePath { get; set; }
        //public List<Contribution> Contributions { get; set; }
        //public int Votes { get; set; }
    }
}
