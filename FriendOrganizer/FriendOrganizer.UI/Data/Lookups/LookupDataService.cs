using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Lookups
{
    public class LookupDataService : IFriendLookupDataService, IProgrammingLanguageLookupDataService
    {
        private Func<FriendOrganizerDbContext> m_contextCreator;

        public LookupDataService(Func<FriendOrganizerDbContext> contextCreator)
        {
            m_contextCreator = contextCreator;
        }

        public async Task<IEnumerable<LookupItem>> GetFriendLookupAsync()
        {
            using (var context = m_contextCreator())
            {
                return await context.Friends.AsNoTracking()
                    .Select(f =>
                    new LookupItem
                    {
                        Id = f.Id,
                        DisplayMember = f.FirstName + " " + f.LastName
                    })
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetProgrammingLanguageLookupAsync()
        {
            using (var context = m_contextCreator())
            {
                return await context.ProgrammingLanguages.AsNoTracking()
                    .Select(pl =>
                    new LookupItem
                    {
                        Id = pl.Id,
                        DisplayMember = pl.Name
                    })
                    .ToListAsync();
            }
        }
    }
}
