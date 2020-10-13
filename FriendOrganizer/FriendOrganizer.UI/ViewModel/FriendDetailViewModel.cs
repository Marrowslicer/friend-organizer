using System.Threading.Tasks;

using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;

using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private IFriendDataService m_friendDataService;
        private IEventAggregator m_eventAggregator;

        public FriendDetailViewModel(
            IFriendDataService friendDataService,
            IEventAggregator eventAggregator)
        {
            m_friendDataService = friendDataService;
            m_eventAggregator = eventAggregator;

            m_eventAggregator.GetEvent<OpenFriendDetailViewEvent>()
                .Subscribe(OnOpenFriendDetailView);
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

        private async void OnOpenFriendDetailView(int friendId)
        {
            await LoadAsync(friendId);
        }
    }
}
