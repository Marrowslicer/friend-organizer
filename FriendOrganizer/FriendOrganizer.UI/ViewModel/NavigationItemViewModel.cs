using System.Windows.Input;

using FriendOrganizer.UI.Event;

using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {
        private string m_displayMember;
        private string m_detailViewModelName;
        private IEventAggregator m_eventAggregator;

        public NavigationItemViewModel(
            int id,
            string displayMember,
            string detailViewModelName,
            IEventAggregator eventAggregator)
        {
            m_detailViewModelName = detailViewModelName;
            m_eventAggregator = eventAggregator;
            Id = id;
            DisplayMember = displayMember;
            OpenDetailViewCommand = new DelegateCommand(OnOpenDetailViewExecute);
        }

        public int Id { get; }

        public string DisplayMember
        {
            get => m_displayMember;
            set
            {
                m_displayMember = value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenDetailViewCommand { get; }

        private void OnOpenDetailViewExecute()
        {
            m_eventAggregator.GetEvent<OpenDetailViewEvent>().Publish(
                new OpenDetailViewEventArgs
                {
                    Id = Id,
                    ViewModelName = m_detailViewModelName
                });
        }
    }
}
