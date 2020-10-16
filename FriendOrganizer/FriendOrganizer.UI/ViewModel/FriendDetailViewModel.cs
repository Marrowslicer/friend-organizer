using System.Threading.Tasks;
using System.Windows.Input;

using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.Wrapper;

using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private IFriendDataService m_friendDataService;
        private IEventAggregator m_eventAggregator;
        private FriendWrapper m_friend;

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

        public ICommand SaveCommand { get; }

        public FriendWrapper Friend
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
            var friend = await m_friendDataService.GetByIdAsync(friendId);

            Friend = new FriendWrapper(friend);
            Friend.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Friend.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };

            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private async void OnSaveExecute()
        {
            await m_friendDataService.SaveAsync(Friend.Model);

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
            //TODO: Check in addition if friend has changes
            return Friend != null && !Friend.HasErrors;
        }

        private async void OnOpenFriendDetailView(int friendId)
        {
            await LoadAsync(friendId);
        }
    }
}
