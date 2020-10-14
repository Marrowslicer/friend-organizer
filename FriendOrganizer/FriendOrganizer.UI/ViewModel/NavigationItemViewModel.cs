namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {
        public NavigationItemViewModel(int id, string displayMember)
        {
            Id = id;
            DisplayMember = displayMember;

        }

        public int Id { get; }

        private string m_displayMember;
        public string DisplayMember
        {
            get => m_displayMember;
            set
            {
                m_displayMember = value;
                OnPropertyChanged();
            }
        }
    }
}
