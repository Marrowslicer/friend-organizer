using System.Collections.ObjectModel;
using System.Threading.Tasks;

using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationViewModel : INavigationViewModel
    {
        private IFriendLookupDataService m_friendLookupDataService;

        public NavigationViewModel(IFriendLookupDataService friendLookupDataService)
        {
            m_friendLookupDataService = friendLookupDataService;
            Friends = new ObservableCollection<LookupItem>();
        }

        public ObservableCollection<LookupItem> Friends { get; }

        public async Task LoadAsync()
        {
            var lookup = await m_friendLookupDataService.GetFriendLookupAsync();
            Friends.Clear();

            foreach (var item in lookup)
            {
                Friends.Add(item);
            }
        }
    }
}
