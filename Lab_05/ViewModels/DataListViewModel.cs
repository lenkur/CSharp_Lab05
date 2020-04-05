using KMA.CSharp2020.Lab05.Models;
using KMA.CSharp2020.Lab05.Tools.Managers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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
        private string _textFilter = "";
        private ObservableCollection<string> _filterByList;
        private string _selectedFilter;

        private Thread _workingThread;
        private CancellationToken _token;
        private CancellationTokenSource _tokenSource;

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
            _filterByList = new ObservableCollection<string>();
            FilterByList.Add("Name");
            FilterByList.Add("ID");
            FilterByList.Add("CPU");
            FilterByList.Add("RAM");
            FilterByList.Add("Threads");
            FilterByList.Add("Start Time");
            FilterByList.Add("User Name");
            FilterByList.Add("File Path");
            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            StartWorkingThread();
            StationManager.StopThreads += StopWorkingThread;
        }
        private void FilterProcesses()
        {
            switch (SelectedFilter)
            {
                case "Name":
                    ProcessList = new ObservableCollection<SingleProcess>(
                        from process in StationManager.DataStorage.ProcessList
                        where process.Name.Contains(TextFilter)
                        orderby process.Name
                        select process);
                    break;
                case "ID":
                    ProcessList = new ObservableCollection<SingleProcess>(
                        from process in StationManager.DataStorage.ProcessList
                        orderby process.Id
                        where process.Id.ToString().Contains(TextFilter)
                        select process);
                    break;
                case "CPU":
                    ProcessList = new ObservableCollection<SingleProcess>(
                        from process in StationManager.DataStorage.ProcessList
                        orderby process.CPU descending
                        where process.CPU.ToString().Contains(TextFilter)
                        select process);
                    break;
                case "RAM":
                    ProcessList = new ObservableCollection<SingleProcess>(
                        from process in StationManager.DataStorage.ProcessList
                        orderby process.RAM descending
                        where process.RAM.ToString().Contains(TextFilter)
                        select process);
                    break;
                case "Threads":
                    ProcessList = new ObservableCollection<SingleProcess>(
                        from process in StationManager.DataStorage.ProcessList
                        orderby process.Threads
                        where process.Threads.ToString().Contains(TextFilter)
                        select process);
                    break;
                case "Start Time":
                    ProcessList = new ObservableCollection<SingleProcess>(
                        from process in StationManager.DataStorage.ProcessList
                        orderby process.StartTime
                        where process.StartTime.Contains(TextFilter)
                        select process);
                    break;
                case "User Name":
                    ProcessList = new ObservableCollection<SingleProcess>(
                        from process in StationManager.DataStorage.ProcessList
                        orderby process.UserName
                        where process.UserName.Contains(TextFilter)
                        select process);
                    break;
                case "File Path":
                    ProcessList = new ObservableCollection<SingleProcess>(
                        from process in StationManager.DataStorage.ProcessList
                        orderby process.Path
                        where process.Path.Contains(TextFilter)
                        select process);
                    break;
                default: break;
            }
        }
        private void SortProcesses()
        {
            switch (SelectedFilter)
            {
                case "Name":
                    ProcessList = new ObservableCollection<SingleProcess>(
                        from process in StationManager.DataStorage.ProcessList
                        orderby process.Name
                        select process);
                    break;
                case "ID":
                    ProcessList = new ObservableCollection<SingleProcess>(
                        from process in StationManager.DataStorage.ProcessList
                        orderby process.Id
                        select process);
                    break;
                case "CPU":
                    ProcessList = new ObservableCollection<SingleProcess>(
                        from process in StationManager.DataStorage.ProcessList
                        orderby process.CPU
                        select process);
                    break;
                case "RAM":
                    ProcessList = new ObservableCollection<SingleProcess>(
                        from process in StationManager.DataStorage.ProcessList
                        orderby process.RAM
                        select process);
                    break;
                case "Threads":
                    ProcessList = new ObservableCollection<SingleProcess>(
                        from process in StationManager.DataStorage.ProcessList
                        orderby process.Threads
                        select process);
                    break;
                case "Start Time":
                    ProcessList = new ObservableCollection<SingleProcess>(
                        from process in StationManager.DataStorage.ProcessList
                        orderby process.StartTime
                        select process);
                    break;
                case "User Name":
                    ProcessList = new ObservableCollection<SingleProcess>(
                        from process in StationManager.DataStorage.ProcessList
                        orderby process.UserName
                        select process);
                    break;
                case "File Path":
                    ProcessList = new ObservableCollection<SingleProcess>(
                        from process in StationManager.DataStorage.ProcessList
                        orderby process.Path
                        select process);
                    break;
                default: break;
            }
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
                ProcessList = new ObservableCollection<SingleProcess>(StationManager.DataStorage.ProcessList);
                FilterProcesses();

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