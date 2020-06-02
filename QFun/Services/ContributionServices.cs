using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QFun.Data;
using QFun.Data.DbTables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QFun.Services
{
    public class ContributionServices
    {
        private readonly ApplicationDbContext context;



        public bool IsImage(IFormFile formFile)
        {
            if (!string.Equals(formFile.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(formFile.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(formFile.ContentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(formFile.ContentType, "image/gif", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(formFile.ContentType, "image/x-png", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(formFile.ContentType, "image/png", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }


            var postedFileExtension = Path.GetExtension(formFile.FileName);
            if (!string.Equals(postedFileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".png", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".gif", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
           
            return true;
        }
        public ContributionServices(ApplicationDbContext context)
        {
            this.context = context;
        }


        public IList<ApplicationUser> GetAllUsers()
        {
            return context.ApplicationUser
                .Include(a => a.Contributions)
                .ThenInclude(c => c.Votes).ToList();
        }

        public int CountUserVotes(ApplicationUser user)
        {
            //for debugging
            var random = new Random();
            int votes = random.Next(1, 99);

            //int votes = 0;

            foreach (var cont in user.Contributions)
                votes += cont.Votes.Count();

            return votes;
        }

        public Contribution GetContributionById(int id)
        {
            return context.Contribution.Find(id);
        }

        public ApplicationUser GetUserById(string id)
        {
            return context.Users.Find(id);
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

        public string GetUserIdById(string id)
        {
            var user = context.Users
                .Find(id);

            return user.Id;
        }


        public int GetUserVotes(string id)
        {
            //for debugging
            var random = new Random();
            int votes = random.Next(1, 99);

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
                //.ThenInclude(v => v.User)
                .ToList();
        }

        public IList<Contribution> GetAllContributionsByChallengeId(int id)
        {
            return context.Contribution.Where(c => c.ChallengeId == id)
                .Include(c => c.User)
                .Include(c => c.Votes)
                //.ThenInclude(v => v.User)
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
            //contribution.Path = contribution.User.Id + contribution.TimeOfUpload.ToString();

            context.Add(contribution);
            context.SaveChanges();

            return contribution.Path;
        }
    }
}
