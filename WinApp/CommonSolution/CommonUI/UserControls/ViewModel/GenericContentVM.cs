/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Generic Content Area View Model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 *2.110.0     25-Nov-2020   Raja Uthanda            Modified for multi screen
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.ViewContainer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Semnox.Parafait.CommonUI
{
    public class GenericContentVM : ViewModelBase
    {
        #region Members
        private MultiScreenItemBackground multiScreenItemBackground;
        private bool ismultiScreenRowTwo;
        private bool multiScreenMode;
        private bool isComboAndSearchVisible;
        private DisplayValues selectedDisplayItem;
        private ComboGroupVM comboGroupVM;
        private ContextSearchVM contextSearchVM;
        private ICommand comboSelectionChangedCommand;
        private ICommand selectedItemCommand;
        private ICommand scrollChangedCommand;
        private ICommand searchCommand;
        private ObservableCollection<DisplayValues> backupDisplayParameters;
        private ObservableCollection<string> headings;
        private ObservableCollection<string> searchIndexes;
        private ObservableCollection<DisplayValues> displayParams;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public MultiScreenItemBackground MultiScreenItemBackground
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(multiScreenItemBackground);
                return multiScreenItemBackground;
            }
            set
            {
                log.LogMethodEntry(multiScreenItemBackground, value);
                SetProperty(ref multiScreenItemBackground, value);
                log.LogMethodExit(multiScreenItemBackground);
            }
        }

        public bool IsMultiScreenRowTwo
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(ismultiScreenRowTwo);
                return ismultiScreenRowTwo;
            }
            set
            {
                log.LogMethodEntry(ismultiScreenRowTwo, value);
                SetProperty(ref ismultiScreenRowTwo, value);
                log.LogMethodExit(ismultiScreenRowTwo);
            }
        }

        public bool MultiScreenMode
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(multiScreenMode);
                return multiScreenMode;
            }
            set
            {
                log.LogMethodEntry(multiScreenMode, value);
                SetProperty(ref multiScreenMode, value);
                log.LogMethodExit(multiScreenMode);
            }
        }

        public bool IsComboAndSearchVisible
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isComboAndSearchVisible);
                return isComboAndSearchVisible;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref isComboAndSearchVisible, value);
                log.LogMethodExit(isComboAndSearchVisible);
            }
        }

        internal ObservableCollection<DisplayValues> BackupDisplayParameters
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedDisplayItem);
                return backupDisplayParameters;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref backupDisplayParameters, value);
                log.LogMethodExit(backupDisplayParameters);
            }
        }

        public DisplayValues SelectedDisplayItem
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedDisplayItem);
                return selectedDisplayItem;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref selectedDisplayItem, value);
            }
        }

        public ContextSearchVM ContextSearchVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(contextSearchVM);
                return contextSearchVM;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref contextSearchVM, value);
            }
        }

        public ObservableCollection<string> ComboSearchHeadings
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(searchIndexes);
                return searchIndexes;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref searchIndexes, value);
            }
        }

        public ICommand SearchCommnad
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(searchCommand);
                return searchCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                searchCommand = value;
            }
        }

        public ICommand ComboSelectionChangedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(comboSelectionChangedCommand);
                return comboSelectionChangedCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                comboSelectionChangedCommand = value;
            }
        }

        public ICommand ScrollChangedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(scrollChangedCommand);
                return scrollChangedCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                scrollChangedCommand = value;
            }
        }

        public ICommand SelectedItemCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedItemCommand);
                return selectedItemCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                selectedItemCommand = value;
            }
        }

        public ComboGroupVM ComboGroupVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(comboGroupVM);
                return comboGroupVM;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref comboGroupVM, value);
            }
        }

        public ObservableCollection<string> Headings
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(headings);
                return headings;
            }
            set
            {
                log.LogMethodEntry(headings, value);
                SetProperty(ref headings, value);
                log.LogMethodEntry(headings);
            }
        }

        public ObservableCollection<DisplayValues> DisplayParams
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayParams);
                return displayParams;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref displayParams, value);
                if (backupDisplayParameters.Count <= 0)
                {
                    backupDisplayParameters = new ObservableCollection<DisplayValues>(displayParams);
                }
            }
        }

        private void DisplayParams_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                backupDisplayParameters.Add(e.NewItems as DisplayValues);
            }
        }
        #endregion

        #region Methods
        private void OnSearchClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                GenericDataGridUserControl dataGridUserControl = parameter as GenericDataGridUserControl;

                if (dataGridUserControl != null)
                {
                    dataGridUserControl.RaiseSearchClickedEvent();
                }
            }
            log.LogMethodExit();
        }

        internal void OnComboSelectionChanged(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                GenericDataGridUserControl dataGridUserControl = parameter as GenericDataGridUserControl;

                if (dataGridUserControl != null && dataGridUserControl.GMComboBoxGroupUserControl != null)
                {
                    ComboGroupUserControl comboGroupUserControl = dataGridUserControl.GMComboBoxGroupUserControl;

                    ObservableCollection<DisplayValues> tempDisplayParameters = new ObservableCollection<DisplayValues>();

                    List<string> searchItems = new List<string>();

                    searchItems = ComboGroupVM.ComboList.Select(h => h.SelectedItem).ToList();

                    List<string> headerItems = ComboGroupVM.ComboList.Select(h => h.Header).ToList();

                    if (comboGroupUserControl != null && comboGroupUserControl.SelectedComboBoxField != null &&
                        ComboGroupVM != null && ComboGroupVM.ComboList != null && ComboGroupVM.ComboList.Count > 0
                            && ComboGroupVM.ComboList.Contains(comboGroupUserControl.SelectedComboBoxField)
                            && headerItems != null && headerItems.Count > 0)
                    {
                        int comboBoxIndex = ComboGroupVM.ComboList.IndexOf(comboGroupUserControl.SelectedComboBoxField);

                        foreach (DisplayValues displayParameters in backupDisplayParameters)
                        {
                            if (displayParameters != null && displayParameters.DisplayItems != null && displayParameters.DisplayItems.Count > 0)
                            {
                                bool isSatisfied = false;

                                for (int i = 0; i < headerItems.Count; i++)
                                {
                                    int headerIndex = this.Headings.IndexOf(headerItems[i]);

                                    if (headerIndex != -1 && searchItems.Count > i && searchItems[i] != null)
                                    {
                                        if (searchItems[i].ToLower() == MessageViewContainerList.GetMessage(executionContext, "All", null).ToLower())
                                        {
                                            continue;
                                        }
                                        else if (displayParameters.DisplayItems.Count > headerIndex &&
                                            displayParameters.DisplayItems[headerIndex].ToLower() == searchItems[i].ToLower())
                                        {
                                            isSatisfied = true;
                                        }
                                        else
                                        {
                                            isSatisfied = false;
                                            break;
                                        }
                                    }
                                }
                                if (isSatisfied && !tempDisplayParameters.Contains(displayParameters))
                                {
                                    tempDisplayParameters.Add(displayParameters);
                                }
                            }
                        }
                    }

                    int backupSelectedId = -1;

                    if (this.SelectedDisplayItem != null)
                    {
                        backupSelectedId = this.SelectedDisplayItem.Id;
                    }
                    DisplayParams = new ObservableCollection<DisplayValues>(tempDisplayParameters);

                    if (searchItems != null && searchItems.All(h => h != null && h.ToLower() == MessageViewContainerList.GetMessage(executionContext, "All", null).ToLower()))
                    {
                        DisplayParams = new ObservableCollection<DisplayValues>(backupDisplayParameters);
                    }

                    if (backupSelectedId != -1)
                    {
                        this.SelectedDisplayItem = DisplayParams.FirstOrDefault(x => x.Id == backupSelectedId);
                    }
                    if (this.SelectedDisplayItem == null)
                    {
                        if (DisplayParams.Count > 0)
                        {
                            this.SelectedDisplayItem = DisplayParams[0];
                        }
                        dataGridUserControl.RaiseIsSelectionChangedEvent();
                    }
                }
            }
            log.LogMethodExit();
        }

        private void OnScrollChanged(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                GenericDataGridUserControl dataGridUserControl = parameter as GenericDataGridUserControl;

                if (dataGridUserControl != null && dataGridUserControl.scvList != null && dataGridUserControl.scvParent != null)
                {
                    ScrollBar scrollBar = (ScrollBar)dataGridUserControl.scvList.Template.FindName("PART_VerticalScrollBar", dataGridUserControl.scvList);

                    dataGridUserControl.scvParent.ScrollToHorizontalOffset(dataGridUserControl.scvList.HorizontalOffset);
                }
            }
            log.LogMethodExit();
        }

        private void OnSelectedItemChanged(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                GenericDataGridUserControl dataGridUserControl = parameter as GenericDataGridUserControl;

                if (dataGridUserControl != null)
                {
                    dataGridUserControl.RaiseIsSelectionChangedEvent();

                    if (dataGridUserControl.lbContent != null && this.SelectedDisplayItem != null)
                    {
                        dataGridUserControl.lbContent.ScrollIntoView(this.SelectedDisplayItem);
                    }
                }
            }
            log.LogMethodExit();
        }
        #endregion

        #region Constructors
        public GenericContentVM(ExecutionContext executionContext)
        {
            log.LogMethodEntry();

            this.executionContext = executionContext;

            isComboAndSearchVisible = true;
            multiScreenMode = false;
            IsMultiScreenRowTwo = false;

            multiScreenItemBackground = MultiScreenItemBackground.Grey;

            searchCommand = new DelegateCommand(OnSearchClicked);
            comboSelectionChangedCommand = new DelegateCommand(OnComboSelectionChanged);
            scrollChangedCommand = new DelegateCommand(OnScrollChanged);
            selectedItemCommand = new DelegateCommand(OnSelectedItemChanged);

            headings = new ObservableCollection<string>();
            displayParams = new ObservableCollection<DisplayValues>();
            searchIndexes = new ObservableCollection<string>();
            backupDisplayParameters = new ObservableCollection<DisplayValues>();

            DisplayParams.CollectionChanged += DisplayParams_CollectionChanged;

            log.LogMethodExit();
        }
        #endregion
    }
}
