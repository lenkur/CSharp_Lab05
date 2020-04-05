using KMA.CSharp2020.Lab05.Tools.DataStorage;
using System;

namespace KMA.CSharp2020.Lab05.Tools.Managers
{
    internal static class StationManager
    {
        private static IDataStorage _dataStorage;
        public static event Action StopThreads;

        internal static IDataStorage DataStorage { get { return _dataStorage; } }
        internal static void Initialize(IDataStorage dataStorage) { _dataStorage = dataStorage; }

        internal static void CloseApp()
        {
            StopThreads?.Invoke();
            Environment.Exit(1);
        }
    }
}