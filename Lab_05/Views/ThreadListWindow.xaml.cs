using KMA.CSharp2020.Lab05.Tools.Managers;
using KMA.CSharp2020.Lab05.ViewModels;
using System.Windows;

namespace KMA.CSharp2020.Lab05.Views
{
    /// <summary>
    /// Interaction logic for ThreadListWindow.xaml
    /// </summary>
    public partial class ThreadListWindow : Window
    {
        public ThreadListWindow()
        {
            InitializeComponent();
            DataContext = new ThreadListViewModel(StationManager.DataStorage.SelectedProcess);
        }
    }
}
