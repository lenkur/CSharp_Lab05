using KMA.CSharp2020.Lab05.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace KMA.CSharp2020.Lab05.Tools.DataStorage
{
    class ProcessDataStorage : IDataStorage
    {
        private List<SingleProcess> _processList;
        private List<int> _accessDeniedProcessList;
        public List<SingleProcess> ProcessList { get { return _processList; } }
        public List<int> AccessDeniedProcessList { get { return _accessDeniedProcessList; } }

        internal ProcessDataStorage()
        {
            _processList = new List<SingleProcess>();
            _accessDeniedProcessList = new List<int>();
            foreach (Process process in Process.GetProcesses())
            {
                ProcessList.Add(new SingleProcess(process, AbleToAccess(process)));
            }
        }

        public int ProcessExists(int id)
        {
            int i = 0;
            foreach (SingleProcess singleProcess in ProcessList)
            {
                if (singleProcess.Id == id) return i;
                i++;
            }
            return -1; ;
        }

        public void UpdateProcess(SingleProcess process)
        {
            process.Update();
        }

        public void UpdateList()
        {
            Process[] newSession = Process.GetProcesses();
            foreach (Process process in newSession)
            {
                int i = ProcessExists(process.Id);
                if (i != -1)
                    UpdateProcess(ProcessList[i]);
                else
                    ProcessList.Add(new SingleProcess(process, AbleToAccess(process)));
            }
            ProcessList.RemoveAll(i => i.Updated == false);
            ProcessList.ForEach(i => i.Updated = false);
        }

        private bool AbleToAccess(Process process)
        {
            try
            {
                DateTime startTime = process.StartTime;
                ProcessModule processModule = process.MainModule;
                return true;
            }
            catch (Win32Exception ex)
            {
                Console.WriteLine($"{ex.Message} Process name:{process.ProcessName}");
                AccessDeniedProcessList.Add(process.Id);
                return false;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"{ex.Message} Process name:{process.ProcessName}");
                return false;
            }
        }

        public void KillProcess(SingleProcess process)
        {
            try
            {
                process.Process.Kill();
                ProcessList.Remove(process);
            }
            catch (Win32Exception e) { Console.WriteLine(e.Message); }
        }
    }
}
