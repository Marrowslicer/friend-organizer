using System;
using System.Threading.Tasks;
using System.Windows.Input;

using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;

using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IEventAggregator m_eventAggregator;
        private Func<IFriendDetailViewModel> m_friendDetailViewModelCreator;
        private IMessageDialogService m_messageDialogService;
        private IDetailViewModel m_detailViewModel;

        public MainViewModel(
            INavigationViewModel navigationViewModel,
            Func<IFriendDetailViewModel> friendDetailViewModelCreator,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            m_eventAggregator = eventAggregator;
            m_friendDetailViewModelCreator = friendDetailViewModelCreator;
            m_messageDialogService = messageDialogService;
            m_eventAggregator.GetEvent<OpenDetailViewEvent>()
                .Subscribe(OnOpenDetailView);
            m_eventAggregator.GetEvent<AfterDetailDeletedEvent>()
                .Subscribe(AfterDetailDeleted);

            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);
            NavigationViewModel = navigationViewModel;
        }

        public ICommand CreateNewDetailCommand { get; }

        public INavigationViewModel NavigationViewModel { get; }

        public IDetailViewModel DetailViewModel
        {
            get => m_detailViewModel;
            private set
            {
                m_detailViewModel = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        private async void OnOpenDetailView(OpenDetailViewEventArgs e)
        {
            if (DetailViewModel != null && DetailViewModel.HasChanges)
            {
                var result = m_messageDialogService.ShowOkCancelDialog(
                    "You've made changes. Navigate away?", "Question");

                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            switch (e.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    DetailViewModel = m_friendDetailViewModelCreator();

                    break;
            }
            
            await DetailViewModel.LoadAsync(e.Id);
        }

        private void OnCreateNewDetailExecute(Type viewModelType)
        {
            OnOpenDetailView(new OpenDetailViewEventArgs { ViewModelName = viewModelType.Name });
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs e)
        {
            DetailViewModel = null;
        }
    }
}
