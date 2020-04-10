using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace KMA.CSharp2020.Lab05.Models
{
    class SingleProcess
    {
        #region Fields
        private static ulong _totalPhysicalMemory = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;
        private readonly string _name;
        private readonly int _id;
        private double _cpu;
        private TimeSpan _oldCpu;
        private double _CPU;
        private float _ram;
        private DateTime _lastTime;
        private readonly string _userName;
        private readonly string _path;
        private readonly DateTime _startTime;
        private readonly Process _process;
        private PerformanceCounter _cpuPerformanceCounter;
        private bool _updated;
        private readonly bool _access;

        #endregion

        #region Properties
        public string Name { get { return _name; } }
        public int Id { get { return _id; } }
        public bool Active { get { return _process.Responding; } }
        public double CPU { get { return _cpu; } }
        public float RAMPercentage { get { return _ram * 1024 / _totalPhysicalMemory * 100; } }
        public float RAM { get { return _ram; } }
        public int Threads { get { return _process.Threads.Count; } }
        public string StartTime { get { return _startTime != DateTime.MinValue ? _startTime.ToString() : "[Access denied]"; } }
        public string UserName { get { return _userName; } }
        public string Path { get { return _path; } }
        internal bool Updated { get { return _updated; } set { _updated = value; } }
        internal bool Accessible { get { return _access; } }
        internal Process Process { get { return _process; } }
        internal ProcessThreadCollection ThreadList { get { return Process.Threads; } }
        internal ProcessModuleCollection ModuleList { get { return Accessible ? Process.Modules : null; } }

        #endregion

        public SingleProcess(Process process, bool access)
        {
            try
            {
                _process = process;
                _access = access;
                if (Accessible)
                {
                    _lastTime = DateTime.UtcNow;
                    _oldCpu = process.TotalProcessorTime;
                }
                else
                {
                    _cpuPerformanceCounter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName);
                    _cpuPerformanceCounter.NextValue();
                }
                _ram = Process.PrivateMemorySize64 / 1024;
                _name = process.ProcessName;
                _id = process.Id;
                //_ramPercentage
                if (!access)
                {
                    _userName = "[Unable to get user]";
                    _startTime = DateTime.MinValue;
                    _path = "[Access denied]";
                }
                else
                {
                    _userName = GetProcessUser(process);
                    _startTime = process.StartTime;
                    _path = process.MainModule.FileName;
                }
                _updated = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
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
                return "[Unable to get user]";
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
            try
            {
                if (Accessible)
                {
                    TimeSpan newCpu = Process.TotalProcessorTime;
                    DateTime newTime = DateTime.UtcNow;
                    _cpu = (float)(newCpu - _oldCpu).TotalMilliseconds / ((newTime - _lastTime).TotalMilliseconds * Environment.ProcessorCount) * 100;
                    _oldCpu = newCpu;
                    _lastTime = newTime;
                }
                else
                    _cpu = _cpuPerformanceCounter.NextValue() / Environment.ProcessorCount;
                Process.Refresh();
                _ram = Process.PrivateMemorySize64 / 1024;
                Updated = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}