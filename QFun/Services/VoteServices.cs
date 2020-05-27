using QFun.Data;
using QFun.Data.DbTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QFun.Services
{
    public class VoteServices
    {
        private readonly ApplicationDbContext context;

        public VoteServices(ApplicationDbContext context)
        {
            this.context = context;
        }

        //returns false if user already has a vote on contribution
        public bool UserAbleToVote(Vote vote)
        {
            return !context.Vote
                .Where(v => v.UserId == vote.UserId && v.ContributionId == vote.ContributionId)
                .Any();
        }

        public void AddVote(Vote vote)
        {
            context.Add(vote);
            context.SaveChanges();
        }

        public void RemoveVote(Vote vote)
        {
            context.Remove(vote);
            context.SaveChanges();
        }
    }
}
