using QFun.Data;
using QFun.Data.DbTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QFun.Services
{
    public class ChallengeServices
    {

        private readonly ApplicationDbContext context;

        public ChallengeServices(ApplicationDbContext context)
        {
            this.context = context;
        }


        public void AddChallange(Challenge challenge)
        {
            context.Add(challenge);
            context.SaveChanges();
        }

        public void RemoveChallengeById(int id)
        {
            var challenge = context.Challenge.Find(id);
            context.Remove(challenge);
            context.SaveChanges();
        }

    }
}
