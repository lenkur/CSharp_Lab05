using System;
using System.ComponentModel;
using System.Diagnostics;

namespace KMA.CSharp2020.Lab05.Models
{
    class SingleThread
    {
        #region Fields
        private readonly ProcessThread _processThread;
        private readonly bool _accesible;
        #endregion

        #region Properties
        public int Id { get { return _processThread.Id; } }
        public string StartTime { get { return _accesible ? _processThread.StartTime.ToString() : "[Access denied]"; } }
        public ThreadState State { get { return _processThread.ThreadState; } }
        #endregion

        public SingleThread(ProcessThread processThread, bool accessible)
        {
            _processThread = processThread;
            _accesible = accessible;
        }
    }
}
