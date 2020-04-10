using System.Diagnostics;

namespace KMA.CSharp2020.Lab05.Models
{
    class SingleModule
    {
        #region Fields
        private readonly string _name;
        private readonly string _path;
        #endregion

        #region Properties
        public string Name { get { return _name; } }
        public string Path { get { return _path; } }
        #endregion

        public SingleModule(ProcessModule processModule)
        {
            _name = processModule.ModuleName;
            _path = processModule.FileName;
        }
    }
}
