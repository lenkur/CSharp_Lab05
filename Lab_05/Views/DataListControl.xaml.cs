using KMA.CSharp2020.Lab05.ViewModel;
using System.Windows.Controls;

namespace KMA.CSharp2020.Lab05.Views
{
    /// <summary>
    /// Interaction logic for DataListControl.xaml
    /// </summary>
    public partial class DataListControl : UserControl
    {
        public DataListControl()
        {
            InitializeComponent();
            DataContext = new DataListViewModel();
        }
    }
}
