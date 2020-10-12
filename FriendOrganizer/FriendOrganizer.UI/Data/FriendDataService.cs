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

        public async Task<List<Friend>> GetAllAsync()
        {
            using (var context = m_contextCreator())
            {
                return await context.Friends.AsNoTracking().ToListAsync();
            }
        }
    }
}
