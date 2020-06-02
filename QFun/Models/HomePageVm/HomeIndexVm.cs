using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QFun.Models.HomePageVm
{
    public class HomeIndexVm
    {
        public IList<HomeIndexData> Contributions { get; set; } = new List<HomeIndexData>();
    }
}
