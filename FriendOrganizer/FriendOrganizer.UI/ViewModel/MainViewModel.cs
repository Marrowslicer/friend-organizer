using System.Collections.ObjectModel;
using System.Threading.Tasks;

using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IFriendDataService m_friendDataService;
        private Friend m_selectedFriend;

        public MainViewModel(IFriendDataService friendDataService)
        {
            m_friendDataService = friendDataService;
            Friends = new ObservableCollection<Friend>();
        }

        public ObservableCollection<Friend> Friends { get; set; }

        public Friend SelectedFriend
        {
            get { return m_selectedFriend; }
            set
            {
                m_selectedFriend = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadAsync()
        {
            var friends = await m_friendDataService.GetAllAsync();
            Friends.Clear();

            foreach (var friend in friends)
            {
                Friends.Add(friend);
            }
        }
    }
}
