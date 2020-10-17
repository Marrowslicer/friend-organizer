using System.Data.Entity;
using System.Threading.Tasks;

using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class FriendRepository : IFriendRepository
    {
        private FriendOrganizerDbContext m_context;

        public FriendRepository(FriendOrganizerDbContext context)
        {
            m_context = context;
        }

        public async Task<Friend> GetByIdAsync(int friendId)
        {
            return await m_context.Friends.SingleAsync(f => f.Id == friendId);
        }

        public async Task SaveAsync()
        {
            await m_context.SaveChangesAsync();
        }

        public bool HasChanges()
        {
            return m_context.ChangeTracker.HasChanges();
        }
    }
}
