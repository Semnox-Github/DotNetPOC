using System.Reflection;
using System.Windows.Input;
using System.Windows.Controls;

namespace Semnox.Parafait.CommonUI
{
    public enum PaginationActionType
    {
        None = 0,
        First = 1,
        Prev = 2,
        Next = 3,
        Last = 4,
        Apply = 5,
    }
    public class PaginationVM : ViewModelBase
    {
        #region Members
        private bool applyEnabled;
        private bool firstEnabled;
        private bool lastEnabled;
        private bool prevtEnabled;
        private bool nextEnabled;

        private int pageNumber = -1;
        private int pageCount = -1;
        private int pageSize = -1;
        private int minPageSize = 1;
        private int maxPageSize;
        private int maxPageSizeLength;
        private int maxPageNumberLength;

        private PaginationActionType clickedActionType;

        private PaginationUserControl paginationUserControl;

        private ICommand actionsCommand;
        private ICommand loadedCommand;

        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public bool ApplyEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(applyEnabled);
                return applyEnabled;
            }
            private set
            {
                log.LogMethodEntry(applyEnabled, value);
                SetProperty(ref applyEnabled, value);
                log.LogMethodExit(applyEnabled);
            }
        }
        public bool FirstEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(firstEnabled);
                return firstEnabled;
            }
            private set
            {
                log.LogMethodEntry(firstEnabled, value);
                SetProperty(ref firstEnabled, value);
                log.LogMethodExit(firstEnabled);
            }
        }
        public bool LastEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(lastEnabled);
                return lastEnabled;
            }
            private set
            {
                log.LogMethodEntry(lastEnabled, value);
                SetProperty(ref lastEnabled, value);
                log.LogMethodExit(lastEnabled);
            }
        }
        public bool NextEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(nextEnabled);
                return nextEnabled;
            }
            private set
            {
                log.LogMethodEntry(nextEnabled, value);
                SetProperty(ref nextEnabled, value);
                log.LogMethodExit(nextEnabled);
            }
        }
        public bool PrevEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(prevtEnabled);
                return prevtEnabled;
            }
            private set
            {
                log.LogMethodEntry(prevtEnabled, value);
                SetProperty(ref prevtEnabled, value);
                log.LogMethodExit(prevtEnabled);
            }
        }
        public int PageNumber
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(pageNumber);
                return pageNumber;
            }
            set
            {
                log.LogMethodEntry(pageNumber,value);
                SetProperty(ref pageNumber, value);
                log.LogMethodExit(pageNumber);
            }
        }
        public int PageSize
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(pageSize);
                return pageSize;
            }
            set
            {
                log.LogMethodEntry(pageSize, value);
                SetProperty(ref pageSize, value);
                log.LogMethodExit(pageSize);
            }
        }
        public int MaxPageSizeLength
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(maxPageSizeLength);
                return maxPageSizeLength;
            }
            private set
            {
                log.LogMethodEntry(maxPageSizeLength, value);
                SetProperty(ref maxPageSizeLength, value);
                log.LogMethodExit(maxPageSizeLength);
            }
        }
        public int MaxPageNumberLength
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(maxPageNumberLength);
                return maxPageNumberLength;
            }
            private set
            {
                log.LogMethodEntry(maxPageNumberLength, value);
                SetProperty(ref maxPageNumberLength, value);
                log.LogMethodExit(maxPageNumberLength);
            }
        }
        public int MaxPageSize
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(maxPageSize);
                return maxPageSize;
            }
            set
            {
                log.LogMethodEntry(maxPageSize, value);
                SetProperty(ref maxPageSize, value);
                log.LogMethodExit(maxPageSize);
            }
        }
        public int PageCount
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(pageCount);
                return pageCount;
            }
            set
            {
                log.LogMethodEntry(pageCount, value);
                SetProperty(ref pageCount, value);
                log.LogMethodExit(pageCount);
            }
        }
        public PaginationActionType ClickedActionType
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(clickedActionType);
                return clickedActionType;
            }
        }
        public ICommand ActionsCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(actionsCommand);
                return actionsCommand;
            }
        }
        public ICommand LoadedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(loadedCommand);
                return loadedCommand;
            }
        }
        #endregion

        #region Constructor
        public PaginationVM()
        {
            log.LogMethodEntry();
            SetDefaultValues(0,0,0,100);
            log.LogMethodExit();
        }
        public PaginationVM(int pageNumber, int pageCount, int pageSize, int maxPageSize)
        {
            log.LogMethodEntry(pageNumber, pageCount, pageSize, maxPageSize);
            SetDefaultValues(pageNumber, pageCount, pageSize, maxPageSize);
            log.LogMethodExit();
        }
        #endregion

        #region Methods
        private void SetDefaultValues(int pageNumber, int pageCount, int pageSize, int maxPageSize)
        {
            log.LogMethodEntry(pageNumber, pageCount, pageSize);
            InitializeCommands();
            this.PageCount = pageCount;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.MaxPageSize = maxPageSize;
            log.LogMethodExit();
        }
        private void InitializeCommands()
        {
            log.LogMethodEntry();
            PropertyChanged += OnPropertyChanged;
            actionsCommand = new DelegateCommand(OnActionsClicked);
            loadedCommand = new DelegateCommand(OnLoaded);
            log.LogMethodExit();
        }
        private void SetEnableState()
        {
            log.LogMethodEntry();
            FirstEnabled = pageNumber > 1 ? true : false;
            PrevEnabled = pageNumber > 1 ? true : false;
            LastEnabled = pageNumber < pageCount ? true : false;
            NextEnabled = pageNumber < pageCount ? true : false;
            ApplyEnabled = pageCount <= 0 ? false : true;
            MaxPageNumberLength = pageCount.ToString().Length;
            if (pageNumber <=0 && MaxPageNumberLength < 2)
            {
                MaxPageNumberLength = 2;
            }
            MaxPageSizeLength = maxPageSize.ToString().Length;            
            log.LogMethodExit();
        }
        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if(!string.IsNullOrEmpty(e.PropertyName))
            {   
                switch(e.PropertyName.ToLower())
                {
                    case "pagecount":
                        {
                            SetEnableState();
                        }
                        break;
                    case "pagenumber":
                        {
                            CheckMaxMinPageNumber();
                            SetEnableState();
                        }
                        break;
                    case "pagesize":
                    case "maxpagesize":
                        {
                            CheckMaxOrPageSize();
                        }
                        break;
                }
            }
            log.LogMethodExit();
        }
        private void CheckMaxOrPageSize(bool fromApply = false)
        {
            log.LogMethodEntry();
            if (pageSize > maxPageSize)
            {
                PageSize = maxPageSize;
            }
            if(fromApply && pageSize < minPageSize)
            {
                pageSize = minPageSize;
            }
            log.LogMethodExit();
        }
        private void CheckMaxMinPageNumber()
        {
            log.LogMethodEntry();
            if (pageNumber > pageCount)
            {
                PageNumber = pageCount;
            }
            if (pageNumber < 0)
            {
                PageNumber = 0;
            }
            log.LogMethodExit();
        }
        private void OnActionsClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if(parameter != null)
            {
                Button button = parameter as Button;
                if(button != null && !string.IsNullOrEmpty(button.Name) && pageCount > 0)
                {
                    string buttonName = button.Name.ToLower();
                    int currentPageNumber = pageNumber;
                    switch(buttonName)
                    {
                        case "btnfirst":
                            {
                                currentPageNumber = 1;
                                clickedActionType = PaginationActionType.First;
                            }
                            break;
                        case "btnprev":
                            {
                                int prevPageNumber = currentPageNumber - 1;
                                if (prevPageNumber > 0)
                                {
                                    currentPageNumber = prevPageNumber;
                                }
                                clickedActionType = PaginationActionType.Prev;
                            }
                            break;
                        case "btnnext":
                            {
                                int nextPageNumber = currentPageNumber + 1;
                                if (nextPageNumber <= pageCount)
                                {
                                    currentPageNumber = nextPageNumber;
                                }
                                clickedActionType = PaginationActionType.Next;
                            }
                            break;
                        case "btnlast":
                            {
                                currentPageNumber = pageCount;
                                clickedActionType = PaginationActionType.Last;
                            }
                            break;
                        case "btnapply":
                            {   
                                clickedActionType = PaginationActionType.Apply;
                            }
                            break;
                    }
                    if(clickedActionType != PaginationActionType.None)
                    {
                        PageNumber = currentPageNumber;
                        CheckMaxMinPageNumber();
                        CheckMaxOrPageSize(true);
                        if (paginationUserControl != null)
                        {
                            paginationUserControl.RaiseSelectionChangedEvent();
                        }
                        clickedActionType = PaginationActionType.None;
                    }
                }
            }
            log.LogMethodExit();
        }
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if(parameter != null)
            {
                paginationUserControl = parameter as PaginationUserControl;
                if(pageSize <= 0)
                {
                    PageSize = maxPageSize;
                }
            }
            log.LogMethodExit();
        }
        #endregion
    }
}
