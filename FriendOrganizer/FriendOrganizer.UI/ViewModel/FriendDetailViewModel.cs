using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.Wrapper;

using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private IFriendRepository m_friendRepository;
        private IEventAggregator m_eventAggregator;
        private IMessageDialogService m_messageDialogService;
        private IProgrammingLanguageLookupDataService m_programmingLanguageLookupDataService;

        private FriendWrapper m_friend;
        private bool m_hasChanges;

        public FriendDetailViewModel(
            IFriendRepository friendRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IProgrammingLanguageLookupDataService programmingLanguageLookupDataService)
        {
            m_friendRepository = friendRepository;
            m_eventAggregator = eventAggregator;
            m_messageDialogService = messageDialogService;
            m_programmingLanguageLookupDataService = programmingLanguageLookupDataService;

            ProgrammingLanguages = new ObservableCollection<LookupItem>();

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
        }

        public ObservableCollection<LookupItem> ProgrammingLanguages { get; }

        public ICommand SaveCommand { get; }

        public ICommand DeleteCommand { get; }

        public FriendWrapper Friend
        {
            get => m_friend;
            set
            {
                m_friend = value;
                OnPropertyChanged();
            }
        }

        public bool HasChanges
        {
            get { return m_hasChanges; }
            set
            {
                if (m_hasChanges != value)
                {
                    m_hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public async Task LoadAsync(int? friendId)
        {
            var friend = friendId.HasValue
                ? await m_friendRepository.GetByIdAsync(friendId.Value)
                : CreateNewFriend();
            
            InitializeFriend(friend);
            await LoadProgrammingLanguagesAsync();
        }

        private void InitializeFriend(Friend friend)
        {
            Friend = new FriendWrapper(friend);
            Friend.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = m_friendRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Friend.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };

            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (Friend.Id == 0)
            {
                // Trigger the validation
                Friend.FirstName = "";
            }
        }

        private async Task LoadProgrammingLanguagesAsync()
        {
            ProgrammingLanguages.Clear();
            ProgrammingLanguages.Add(new NullLookupItem { DisplayMember = "-" });
            var lookup = await m_programmingLanguageLookupDataService.GetProgrammingLanguageLookupAsync();

            foreach (var lookupItem in lookup)
            {
                ProgrammingLanguages.Add(lookupItem);
            }
        }

        private async void OnSaveExecute()
        {
            await m_friendRepository.SaveAsync();

            HasChanges = m_friendRepository.HasChanges();

            // Raise event (send message)
            m_eventAggregator.GetEvent<AfterFriendSavedEvent>().Publish(
                new AfterSaveFriendEventArgs
                {
                    Id = Friend.Id,
                    DisplayMember = $"{Friend.FirstName} {Friend.LastName}"
                });
        }

        private bool OnSaveCanExecute()
        {
            return Friend != null && !Friend.HasErrors && HasChanges;
        }

        private async void OnDeleteExecute()
        {
            var result = m_messageDialogService
                .ShowOkCancelDialog($"Do you really want to delete the friend {Friend.FirstName} {Friend.LastName}?",
                "Question");

            if (result == MessageDialogResult.OK)
            {
                m_friendRepository.Remove(Friend.Model);
                await m_friendRepository.SaveAsync();
                m_eventAggregator.GetEvent<AfterFriendDeletedEvent>().Publish(Friend.Id);
            }
        }

        private Friend CreateNewFriend()
        {
            var friend = new Friend();

            m_friendRepository.Add(friend);

            return friend;
        }
    }
}
