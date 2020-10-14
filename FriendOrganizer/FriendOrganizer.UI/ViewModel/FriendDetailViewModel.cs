using System;
using System.Threading.Tasks;
using System.Windows.Input;

using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;

using Prism.Commands;
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

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        private async void OnSaveExecute()
        {
            await m_friendDataService.SaveAsync(Friend);

            // Raise event (send message)
            m_eventAggregator.GetEvent<AfterSaveFriendEvent>().Publish(
                new AfterSaveFriendEventArgs
                {
                    Id = Friend.Id,
                    DisplayMember = $"{Friend.FirstName} {Friend.LastName}"
                });
        }

        private bool OnSaveCanExecute()
        {
            //TODO: Check if friend is valid
            return true;
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

        public ICommand SaveCommand { get; }

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
