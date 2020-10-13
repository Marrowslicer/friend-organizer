using System.Threading.Tasks;

using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private IFriendDataService m_friendDataService;

        public FriendDetailViewModel(IFriendDataService friendDataService)
        {
            m_friendDataService = friendDataService;
        }

        private Friend m_friend;
        public Friend Friend
        {
            get => m_friend;
            set
            {
                m_friend = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadAsync(int friendId)
        {
            Friend = await m_friendDataService.GetByIdAsync(friendId);
        }
    }
}
