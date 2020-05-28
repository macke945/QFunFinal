using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QFun.Data;
using QFun.Data.DbTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QFun.Services
{
    public class ContributionServices
    {
        private readonly ApplicationDbContext context;

        public ContributionServices(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IList<string> GetAllUsersId()
        {
            return context.Users
               .Select(u => u.Id)
               .ToList();
        }

        public string GetUserNameById(string id)
        {
            var user = context.Users
                .Find(id);

            return user.UserName;
                
        }
        

        public int GetUserVotes(string id)
        {
            //for debugging
            var random = new Random();
            int votes =random.Next(1, 99);

            //int votes = 0;

            var contributions = GetAllContributionsByUserId(id);

            foreach (var cont in contributions)
            {
                votes += cont.Votes.Count();
            }

            return votes;
        }




        public IList<Contribution> GetAllContributionsByUserId(string id)
        {
            return context.Contribution.Where(c => c.UserId == id)
                .Include(c => c.Challenge)
                .Include(c => c.Votes)
                .ThenInclude(v => v.User)
                .ToList();
        }

        public IList<Contribution> GetAllContributionsByChallengeId(int id)
        {
            return context.Contribution.Where(c => c.ChallengeId == id)
                .Include(c => c.User)
                .Include(c => c.Votes)
                .ThenInclude(v => v.User)
                .ToList();
        }

        public void RemoveContributionById(int id)
        {
            var contribution = context.Contribution.Find(id);
            context.Contribution.Remove(contribution);
            context.SaveChanges();
        }

        public void EditContribution(Contribution contribution)
        {
            context.Contribution.Update(contribution);
            context.SaveChanges();
        }


        //returns false if user already has contributed to challenge
        public bool UserAbleToContribute(Contribution contribution)
        {
            return !context.Contribution
                .Where(c => c.UserId == contribution.UserId && c.ChallengeId == contribution.ChallengeId)
                .Any();
        }

        //iamge path is userId + TimeOfUpload to make sure all images has unique name/path
        public string AddContribution(Contribution contribution)
        {
            contribution.TimeOfUpload = DateTime.UtcNow;
            contribution.Path = contribution.User.Id + contribution.TimeOfUpload.ToString();

            context.Add(contribution);
            context.SaveChanges();

            return contribution.Path;
        }
    }
}
