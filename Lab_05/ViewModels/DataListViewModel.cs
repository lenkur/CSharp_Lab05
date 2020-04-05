using KMA.CSharp2020.Lab05.Models;
using KMA.CSharp2020.Lab05.Tools.Managers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace KMA.CSharp2020.Lab05.ViewModel
{
    class DataListViewModel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<SingleProcess> _processList;
        private SingleProcess _selectedProcess;
        private string _textFilter;
        private ObservableCollection<string> _filterByList;
        private string _selectedFilter;

        private Thread _workingThread;
        private CancellationToken _token;
        private CancellationTokenSource _tokenSource;
        private BackgroundWorker _backgroundWorker;
        private Task _backgroundTask;

        #region Commands
        //private RelayCommand<object> _deletePersonCommand;
        #endregion
        #endregion

        #region Properties
        public ObservableCollection<SingleProcess> ProcessList
        {
            get { return _processList; }
            private set
            {
                _processList = value;
                OnPropertyChanged();
            }
        }
        public SingleProcess SelectedProcess
        {
            get { return _selectedProcess; }
            set
            {
                _selectedProcess = value;
                OnPropertyChanged();
            }
        }
        public string TextFilter
        {
            get { return _textFilter; }
            set
            {
                _textFilter = value;
                FilterProcesses();
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> FilterByList
        {
            get { return _filterByList; }
            set
            {
                _filterByList = value;
                OnPropertyChanged();
            }
        }
        public string SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                _selectedFilter = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public DataListViewModel()
        {
            _processList = new ObservableCollection<SingleProcess>(StationManager.DataStorage.ProcessList);
            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            StartWorkingThread();
            StationManager.StopThreads += StopWorkingThread;
        }
        private void FilterProcesses()
        {
            throw new NotImplementedException();
        }
        private void StartWorkingThread()
        {
            _workingThread = new Thread(WorkingThreadProcess);
            _workingThread.Start();
        }
        private void WorkingThreadProcess()
        {
            while (!_token.IsCancellationRequested)
            {
                int currentSelectedProcessId = -1;
                if (SelectedProcess != null)
                {
                    currentSelectedProcessId = SelectedProcess.Id;
                }
                StationManager.DataStorage.UpdateList();

                foreach (SingleProcess singleProcess in ProcessList)
                {
                    if (singleProcess.Id == currentSelectedProcessId)
                    {
                        SelectedProcess = singleProcess;
                        break;
                    }
                }
                Thread.Sleep(2000);
            }
        }
        internal void StopWorkingThread()
        {
            _tokenSource.Cancel();
            _workingThread.Join(2000);
            _workingThread.Abort();
            _workingThread = null;
        }
    }
}