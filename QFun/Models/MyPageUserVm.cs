using QFun.Data.DbTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QFun.Models
{
    public class MyPageUserVm
    {
        public IList<Contribution> Contributions { get; set; }
    }
}
