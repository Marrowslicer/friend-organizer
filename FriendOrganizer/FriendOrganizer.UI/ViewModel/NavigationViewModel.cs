using System;
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
            m_eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            m_eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        public ObservableCollection<NavigationItemViewModel> Friends { get; }

        public async Task LoadAsync()
        {
            var lookup = await m_friendLookupDataService.GetFriendLookupAsync();
            Friends.Clear();

            foreach (var item in lookup)
            {
                Friends.Add(new NavigationItemViewModel(item.Id, item.DisplayMember,
                    nameof(FriendDetailViewModel), m_eventAggregator));
            }
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs e)
        {
            switch (e.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    var lookupItem = Friends.SingleOrDefault(f => f.Id == e.Id);

                    if (lookupItem == null)
                    {
                        Friends.Add(new NavigationItemViewModel(e.Id, e.DisplayMember,
                            nameof(FriendDetailViewModel), m_eventAggregator));
                    }
                    else
                    {
                        lookupItem.DisplayMember = e.DisplayMember;
                    }

                    break;
            }
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs e)
        {
            switch (e.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    var friend = Friends.SingleOrDefault(f => f.Id == e.Id);

                    if (friend != null)
                    {
                        Friends.Remove(friend);
                    }

                    break;
            }
        }
    }
}
