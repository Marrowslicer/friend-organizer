using System;
using System.Threading.Tasks;
using System.Windows;

using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;

using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IEventAggregator m_eventAggregator;
        private Func<IFriendDetailViewModel> m_friendDetailViewModelCreator;
        private IMessageDialogService m_messageDialogService;
        private IFriendDetailViewModel m_friendDetailViewModel;

        public MainViewModel(
            INavigationViewModel navigationViewModel,
            Func<IFriendDetailViewModel> friendDetailViewModelCreator,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            m_eventAggregator = eventAggregator;
            m_friendDetailViewModelCreator = friendDetailViewModelCreator;
            m_messageDialogService = messageDialogService;
            m_eventAggregator.GetEvent<OpenFriendDetailViewEvent>()
                .Subscribe(OnOpenFriendDetailView);

            NavigationViewModel = navigationViewModel;
        }

        public INavigationViewModel NavigationViewModel { get; }

        public IFriendDetailViewModel FriendDetailViewModel
        {
            get => m_friendDetailViewModel;
            private set
            {
                m_friendDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        private async void OnOpenFriendDetailView(int friendId)
        {
            if (FriendDetailViewModel != null && FriendDetailViewModel.HasChanges)
            {
                var result = m_messageDialogService.ShowOkCancelDialog(
                    "You've made changes. Navigate away?", "Question");

                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            FriendDetailViewModel = m_friendDetailViewModelCreator();
            await FriendDetailViewModel.LoadAsync(friendId);
        }
    }
}
