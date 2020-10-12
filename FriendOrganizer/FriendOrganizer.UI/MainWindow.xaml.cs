using System.Windows;

using FriendOrganizer.UI.ViewModel;

namespace FriendOrganizer.UI
{
    public partial class MainWindow : Window
    {
        private MainViewModel m_mainViewModel;

        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            m_mainViewModel = mainViewModel;
            DataContext = m_mainViewModel;
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await m_mainViewModel.LoadAsync();
        }
    }
}
