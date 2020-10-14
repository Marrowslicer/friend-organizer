using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data
{
    public class FriendDataService : IFriendDataService
    {
        private Func<FriendOrganizerDbContext> m_contextCreator;

        public FriendDataService(Func<FriendOrganizerDbContext> contextCreator)
        {
            m_contextCreator = contextCreator;
        }

        public async Task<Friend> GetByIdAsync(int friendId)
        {
            using (var context = m_contextCreator())
            {
                return await context.Friends.AsNoTracking()
                    .SingleAsync(f => f.Id == friendId);
            }
        }

        public async Task SaveAsync(Friend friend)
        {
            using (var context = m_contextCreator())
            {
                context.Friends.Attach(friend);
                context.Entry(friend).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}
