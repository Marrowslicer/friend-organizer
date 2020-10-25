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
        private IMeetingLookupDataService m_meetingLookupDataService;
        private IEventAggregator m_eventAggregator;

        public NavigationViewModel(
            IFriendLookupDataService friendLookupDataService,
            IMeetingLookupDataService meetingLookupDataService,
            IEventAggregator eventAggregator)
        {
            m_friendLookupDataService = friendLookupDataService;
            m_meetingLookupDataService = meetingLookupDataService;
            m_eventAggregator = eventAggregator;

            m_eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            m_eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);

            Friends = new ObservableCollection<NavigationItemViewModel>();
            Meetings = new ObservableCollection<NavigationItemViewModel>();
        }

        public ObservableCollection<NavigationItemViewModel> Friends { get; }

        public ObservableCollection<NavigationItemViewModel> Meetings { get; }

        public async Task LoadAsync()
        {
            var lookup = await m_friendLookupDataService.GetFriendLookupAsync();
            Friends.Clear();

            foreach (var item in lookup)
            {
                Friends.Add(new NavigationItemViewModel(item.Id, item.DisplayMember,
                    nameof(FriendDetailViewModel), m_eventAggregator));
            }

            lookup = await m_meetingLookupDataService.GetMeetingLookupAsync();
            Meetings.Clear();

            foreach (var item in lookup)
            {
                Meetings.Add(new NavigationItemViewModel(item.Id, item.DisplayMember,
                    nameof(MeetingDetailViewModel), m_eventAggregator));
            }
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs e)
        {
            switch (e.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    AfterDetailSaved(Friends, e);

                    break;
                case nameof(MeetingDetailViewModel):
                    AfterDetailSaved(Meetings, e);

                    break;
            }
        }

        private void AfterDetailSaved(
            ObservableCollection<NavigationItemViewModel> items,
            AfterDetailSavedEventArgs e)
        {
            var lookupItem = items.SingleOrDefault(i => i.Id == e.Id);

            if (lookupItem == null)
            {
                items.Add(new NavigationItemViewModel(e.Id, e.DisplayMember,
                    e.ViewModelName, m_eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = e.DisplayMember;
            }
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs e)
        {
            switch (e.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    AfterDetailDeleted(Friends, e);

                    break;
                case nameof(MeetingDetailViewModel):
                    AfterDetailDeleted(Meetings, e);

                    break;
            }
        }

        private void AfterDetailDeleted(
            ObservableCollection<NavigationItemViewModel> items,
            AfterDetailDeletedEventArgs e)
        {
            var item = items.SingleOrDefault(i => i.Id == e.Id);

            if (item != null)
            {
                items.Remove(item);
            }
        }
    }
}
