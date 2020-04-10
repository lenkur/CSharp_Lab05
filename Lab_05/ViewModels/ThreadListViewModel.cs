using KMA.CSharp2020.Lab05.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace KMA.CSharp2020.Lab05.ViewModels
{
    class ThreadListViewModel:BaseViewModel
    {
        #region Fields
        private ObservableCollection<SingleThread> _threadList;
        private string _processLabel;
        #endregion

        #region Properties
        public ObservableCollection<SingleThread> ThreadList
        {
            get { return _threadList; }
            private set
            {
                _threadList = value;
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

        public ThreadListViewModel(SingleProcess process)
        {
            ProcessLabel = $"Process: {process.Id} {process.Name}";
            _threadList = new ObservableCollection<SingleThread>();
            foreach (ProcessThread processThread in process.ThreadList)
            {
                ThreadList.Add(new SingleThread(processThread, process.Accessible));
            } 
        }
    }
}
