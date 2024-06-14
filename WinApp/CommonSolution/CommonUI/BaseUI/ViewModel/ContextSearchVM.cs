/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - view model for context search
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.CommonUI
{
    public class ContextSearchVM : ViewModelBase
    {
        #region Members
        private bool fromCheckPossibility;

        private string heading;
        private string searchText;
        private string resultCountText;
        private string textboxDefaultValue;
        
        private ICommand closeCommand;
        private ICommand selectedItemCommand;
        private DisplayParameters selectedParam;

        private ObservableCollection<int> searchIndexes;
        private ObservableCollection<DisplayParameters> searchParams;
        private ObservableCollection<DisplayParameters> backupSearchParams;
        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public ICommand CloseCommand
        {
            get
            {
                log.LogMethodEntry();
                return closeCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref closeCommand, value);
            }
        }

        public string Heading
        {
            get
            {
                log.LogMethodEntry();
                return heading;
            }
            set
            {
                log.LogMethodEntry(value);
                heading = value;
            }
        }

        public string TextBoxDefaultValue
        {
            get
            {
                log.LogMethodEntry();
                return textboxDefaultValue;
            }
            set
            {
                log.LogMethodEntry(value);
                textboxDefaultValue = value;
            }
        }

        public ObservableCollection<int> SearchIndexes
        {
            get
            {
                log.LogMethodEntry();
                return searchIndexes;
            }
            set
            {
                log.LogMethodEntry(value);
                searchIndexes = value;
            }
        }

        public DisplayParameters SelectedParam
        {
            get
            {
                log.LogMethodEntry();
                return selectedParam;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref selectedParam, value);
            }
        }

        public ObservableCollection<DisplayParameters> SearchParams
        {
            get
            {
                log.LogMethodEntry();
                return searchParams;
            }
            set
            {
                log.LogMethodEntry(value);
                if (!fromCheckPossibility)
                {
                    backupSearchParams = new ObservableCollection<DisplayParameters>(value);
                }
                SetProperty(ref searchParams, value);
            }
        }

        public ICommand SelectedItemCommand
        {
            get
            {
                log.LogMethodEntry();
                return selectedItemCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                selectedItemCommand = value;
            }
        }

        public string ResultCountText
        {
            get
            {
                log.LogMethodEntry();
                return resultCountText;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref resultCountText, value);
            }
        }

        public string SearchText
        {
            get
            {
                log.LogMethodEntry();
                return searchText;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref searchText, value);
                if (searchText != textboxDefaultValue)
                {
                    CheckPossibilty();
                }
            }

        }
        #endregion

        #region Methods
        private void OnCloseClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                ContextSearchView searchPopupWindow = parameter as ContextSearchView;

                if (searchPopupWindow != null)
                {
                    searchPopupWindow.SelectedId = -1;

                    searchPopupWindow.Close();
                }
            }
            log.LogMethodExit();
        }

        private void OnSelected(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                ContextSearchView searchPopupWindow = parameter as ContextSearchView;

                if (searchPopupWindow != null && this.SelectedParam != null)
                {
                    searchPopupWindow.SelectedId = this.SelectedParam.Id;

                    searchPopupWindow.Close();
                }
            }
            log.LogMethodExit();
        }

        private void CheckPossibilty()
        {
            log.LogMethodEntry();
            if (backupSearchParams != null && backupSearchParams.Count > 0)
            {
                List<DisplayParameters> displayParameters = new List<DisplayParameters>();

                List<DisplayParameters> tempList = null;

                if (searchIndexes != null && searchIndexes.Count > 0)
                {
                    foreach (int index in searchIndexes)
                    {
                        if (backupSearchParams.Count > 0 && backupSearchParams[0].ParameterNames != null &&
                            backupSearchParams[0].ParameterNames.Count > index)
                        {
                            if (searchText != null)
                                tempList = backupSearchParams.Where(g =>
                                g.ParameterNames != null
                                && g.ParameterNames[index] != null
                                && g.ParameterNames[index].ToLower().Contains(searchText.ToLower())).ToList();

                            if (tempList != null && tempList.Count > 0)
                            {
                                foreach (DisplayParameters searches in tempList)
                                {
                                    if (displayParameters != null && !displayParameters.Contains(searches))
                                    {
                                        displayParameters.Add(searches);
                                    }
                                }
                            }
                        }
                    }

                    fromCheckPossibility = true;

                    SearchParams = new ObservableCollection<DisplayParameters>(displayParameters);

                    fromCheckPossibility = false;

                    if (searchText != string.Empty && SearchParams != null)
                    {
                        ResultCountText = SearchParams.Count.ToString() + MessageViewContainerList.GetMessage(ExecutionContext, " results (Select to view details)");
                    }
                    else
                    {
                        ResultCountText = MessageViewContainerList.GetMessage(ExecutionContext, "Suggestions (Select to View Details)");
                    }
                }
            }
            log.LogMethodExit();
        }
        #endregion

        #region Constructors
        public ContextSearchVM(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ExecutionContext = executionContext;

            selectedItemCommand = new DelegateCommand(OnSelected);
            closeCommand = new DelegateCommand(OnCloseClicked);

            fromCheckPossibility = false;

            searchText = string.Empty;
            heading = MessageViewContainerList.GetMessage(ExecutionContext,"Search");
            textboxDefaultValue = MessageViewContainerList.GetMessage(ExecutionContext, "Search here...");
            resultCountText = MessageViewContainerList.GetMessage(ExecutionContext, "Suggestions (Select to View Details)");
            
            searchIndexes = new ObservableCollection<int>();
            searchParams = new ObservableCollection<DisplayParameters>();

            log.LogMethodExit();
        }
        #endregion
    }
}
