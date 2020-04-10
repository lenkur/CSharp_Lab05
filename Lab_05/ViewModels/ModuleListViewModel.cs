using KMA.CSharp2020.Lab05.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace KMA.CSharp2020.Lab05.ViewModels
{
    class ModuleListViewModel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<SingleModule> _modulesList;
        private string _processLabel;
        #endregion

        #region Properties
        public ObservableCollection<SingleModule> ModuleList
        {
            get { return _modulesList; }
            private set
            {
                _modulesList = value;
                OnPropertyChanged();
            }
        }
        public string ProcessLabel
        {
            get { return _processLabel; }
            private set
            {
                _processLabel = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public ModuleListViewModel(SingleProcess process)
        {
            ProcessLabel = $"Process: {process.Id} {process.Name}";
            _modulesList = new ObservableCollection<SingleModule>();
            foreach (ProcessModule processModule in process.ModuleList)
            {
                ModuleList.Add(new SingleModule(processModule));
            }
        }
    }
}
