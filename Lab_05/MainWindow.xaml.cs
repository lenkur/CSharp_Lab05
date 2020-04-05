using KMA.CSharp2020.Lab05.Tools.DataStorage;
using KMA.CSharp2020.Lab05.Tools.Managers;
using System.ComponentModel;
using System.Windows;

namespace KMA.CSharp2020.Lab05
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ProcessDataStorage _processDataStorage;

        public MainWindow()
        {
            _processDataStorage = new ProcessDataStorage();
            StationManager.Initialize(_processDataStorage);
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            StationManager.CloseApp();
        }
    }
}
