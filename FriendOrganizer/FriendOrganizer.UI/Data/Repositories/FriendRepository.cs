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
            return await m_context.Friends
                .Include(f => f.PhoneNumbers)
                .SingleAsync(f => f.Id == friendId);
        }

        public async Task SaveAsync()
        {
            await m_context.SaveChangesAsync();
        }

        public bool HasChanges()
        {
            return m_context.ChangeTracker.HasChanges();
        }

        public void Add(Friend friend)
        {
            m_context.Friends.Add(friend);
        }

        public void Remove(Friend friend)
        {
            m_context.Friends.Remove(friend);
        }

        public void RemovePhoneNumber(FriendPhoneNumber number)
        {
            m_context.FriendPhoneNumbers.Remove(number);
        }
    }
}
