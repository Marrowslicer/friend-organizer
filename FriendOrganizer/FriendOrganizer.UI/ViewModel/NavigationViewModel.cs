using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Event;

using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private IFriendLookupDataService m_friendLookupDataService;
        private IEventAggregator m_eventAggregator;

        public NavigationViewModel(
            IFriendLookupDataService friendLookupDataService,
            IEventAggregator eventAggregator)
        {
            m_friendLookupDataService = friendLookupDataService;
            m_eventAggregator = eventAggregator;
            Friends = new ObservableCollection<NavigationItemViewModel>();

            // Subscribe event (message)
            m_eventAggregator.GetEvent<AfterSaveFriendEvent>().Subscribe(AfterFriendSaved);
        }

        private void AfterFriendSaved(AfterSaveFriendEventArgs friend)
        {
            var lookupItem = Friends.Single(f => f.Id == friend.Id);
            lookupItem.DisplayMember = friend.DisplayMember;
        }

        public ObservableCollection<NavigationItemViewModel> Friends { get; }

        public async Task LoadAsync()
        {
            var lookup = await m_friendLookupDataService.GetFriendLookupAsync();
            Friends.Clear();

            foreach (var item in lookup)
            {
                Friends.Add(new NavigationItemViewModel(item.Id, item.DisplayMember,
                    m_eventAggregator));
            }
        }
    }
}
