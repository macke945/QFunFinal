using QFun.Data.DbTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QFun.Models.HomePageVm
{
    public class HomeIndexData
    {
       
        public string Path { get; set; }
        //public DateTime TimeOfUpload { get; set; }
        public string Description { get; set; }
        public ApplicationUser User { get; set; }
        public Challenge Challenge { get; set; }
        public int Votes { get; set; }
    }
}
