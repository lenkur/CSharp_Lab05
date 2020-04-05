using KMA.CSharp2020.Lab05.Tools.Managers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows;

namespace KMA.CSharp2020.Lab05.Models
{
    class SingleProcess
    {
        #region Fields
        private readonly string _name;
        private readonly int _id;
        //private readonly bool _active;
        private float _cpu;
        //private readonly string _ramPercentage;
        private long _ram;
        private readonly string _userName;
        private readonly string _path;
        private readonly DateTime _startTime;
        private Process _process;
        private PerformanceCounter _performanceCounter;
        private bool _updated;
        #endregion

        #region Properties
        public string Name { get { return _name; } }
        public int Id { get { return _id; } }
        public bool Active { get { return _process.Responding; } }
        public float CPU { get { return _cpu; } }
        //public string RAMPercentage { get { return _ramPercentage; } }
        public long RAM { get { return _ram / 1024; } }
        public int Threads { get { return _process.Threads.Count; } }
        public string StartTime { get { return _startTime != DateTime.MinValue ? _startTime.ToString() : "[Access denied]"; } }
        public string UserName { get { return _userName; } }
        public string Path { get { return _path; } }
        internal bool Updated { get { return _updated; } set { _updated = value; } }
        #endregion

        public SingleProcess(Process process)
        {
            _process = process;
            _performanceCounter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName);
            _performanceCounter.NextValue();
            _name = process.ProcessName;
            _id = process.Id;
            //_ramPercentage
            _userName = GetProcessUser(process);
            if (_userName == null)
            {
                _userName = "[Unable to get user]";
                _startTime = DateTime.MinValue;
                _path = "[Access denied]";
            }
            else
            {
                _startTime = process.StartTime;
                _path = process.MainModule.FileName;
            }
            _updated = true;
        }

        private static string GetProcessUser(Process process)
        {
            IntPtr processHandle = IntPtr.Zero;
            try
            {
                OpenProcessToken(process.Handle, 8, out processHandle);
                WindowsIdentity wi = new WindowsIdentity(processHandle);
                string user = wi.Name;
                return user.Contains(@"\") ? user.Substring(user.IndexOf(@"\") + 1) : user;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (processHandle != IntPtr.Zero)
                {
                    CloseHandle(processHandle);
                }
            }
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        internal void Update()
        {
            _cpu = _performanceCounter.NextValue() / Environment.ProcessorCount;
            _ram = _process.PrivateMemorySize64;
            Updated = true;
        }
    }
}