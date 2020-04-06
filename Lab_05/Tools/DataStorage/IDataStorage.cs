using KMA.CSharp2020.Lab05.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace KMA.CSharp2020.Lab05.Tools.DataStorage
{
    internal interface IDataStorage
    {
        int ProcessExists(int id);
        void KillProcess(SingleProcess selectedProcess);
        void OpenFolder(SingleProcess selectedProcess);
        void UpdateList();
        List<SingleProcess> ProcessList { get; }
        List<int> AccessDeniedProcessList { get; }
    }
}