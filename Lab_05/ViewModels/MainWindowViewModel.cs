namespace KMA.CSharp2020.Lab05
{
    class MainWindowViewModel : BaseViewModel
    {
        #region Fields
        //private Visibility _loaderVisibility = Visibility.Hidden;
        private bool _isControlEnabled = true;
        #endregion

        #region Properties
        //public Visibility LoaderVisibility
        //{
        //    get { return _loaderVisibility; }
        //    set
        //    {
        //        _loaderVisibility = value;
        //        OnPropertyChanged();
        //    }
        //}
        public bool IsControlEnabled
        {
            get { return _isControlEnabled; }
            set
            {
                _isControlEnabled = value;
                OnPropertyChanged();
            }
        }
        #endregion

        internal MainWindowViewModel()
        {
            //LoaderManager.Instance.Initialize(this);
        }
    }
}