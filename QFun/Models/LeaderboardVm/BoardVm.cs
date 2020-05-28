using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace QFun.Models.LeaderboardVm
{
    public class BoardVm
    {
        public IList<LeaderboardUserData> UserVoteData { get; set; } = new List<LeaderboardUserData>();
    }
}
