/********************************************************************************************
 * Project Name - CashdrawerUI                                                                        
 * Description  -POSUserAssignmentVM
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.140.0     11-Aug-2021      Girish Kundar     Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.CashdrawersUI;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Printer.Cashdrawers;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CashdrawersUI.ViewModel
{
    public class POSUserAssignmentVM : BaseWindowViewModel
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ObservableCollection<UsersDTO> userDTOList;
        private Dictionary<string, CustomDataGridColumnElement> dataEntryElements;
        private UsersDTO selectedUserDTO;
        private CashdrawerDTO selectedCashDrawerDTO;

        private string message;
        private string buttonClose;
        private string buttonOK;
        private POSUserAssignmentView pOSUserAssignmentView;
        private DisplayTagsVM displayTagsVM;
        private CustomDataGridVM customDataGridVM;
        private ObservableCollection<UserContainerDTO> gridSource;
        private ICommand assignCommand;
        private ICommand closeCommand;
        private ICommand navigationClickCommand;
        private ICommand rowSectionCommand;
        private ICommand selectionChangedCmd;
        private string labelStatus;
        private string moduleName;
        #endregion Members
        #region Properties
        /// <summary>
        /// CustomDataGridVM
        /// </summary>
        public CustomDataGridVM CustomDataGridVM
        {
            get
            {
                return customDataGridVM;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref customDataGridVM, value);
                log.LogMethodExit(customDataGridVM);
            }

        }

        /// <summary>
        /// DisplayTagsVM
        /// </summary>
        public DisplayTagsVM DisplayTagsVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayTagsVM);
                return displayTagsVM;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref displayTagsVM, value);
                log.LogMethodExit(displayTagsVM);
            }
        }

        /// <summary>
        /// UserContainerDTOList
        /// </summary>
        public ObservableCollection<UserContainerDTO> UserContainerDTOList
        {
            get
            {
                return gridSource;
            }
            set
            {
                gridSource = value;
            }
        }

        /// <summary>
        /// DataEntryElements
        /// </summary>
        public Dictionary<string, CustomDataGridColumnElement> DataEntryElements
        {
            get
            {
                return dataEntryElements;
            }
        }
        /// <summary>
        /// labelStatus
        /// </summary>
        public string LabelStatus
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(labelStatus);
                return labelStatus;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref labelStatus, value);
            }
        }

        /// <summary>
        /// ModuleName
        /// </summary>
        public string ModuleName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(moduleName);
                return moduleName;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref moduleName, value);
                }
            }
        }
        /// <summary>
        /// ModuleName
        /// </summary>
        public string ButtonClose
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(buttonClose);
                return buttonClose;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref buttonClose, value);
                }
            }
        }
        public string ButtonOK
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(buttonOK);
                return buttonOK;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref buttonOK, value);
                }
            }
        }

        /// <summary>
        /// SelectedUserContainerDTO
        /// </summary>
        public UsersDTO SelectedUserDTO
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedUserDTO);
                return selectedUserDTO;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref selectedUserDTO, value);
                }
            }
        }
        
        /// <summary>
        /// SuccessMessage
        /// </summary>
        public string SuccessMessage
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(message);
                return message;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref message, value);
            }
        }

        #endregion Properties

        #region Commands
        public ICommand NavigationClickCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(navigationClickCommand);
                return navigationClickCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                navigationClickCommand = value;
            }
        }

        /// <summary>
        /// AssignCommand
        /// </summary>
        public ICommand AssignCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(assignCommand);
                return assignCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                assignCommand = value;
            }
        }

        /// <summary>
        /// SearchCommand   
        /// </summary>
        public ICommand RowSectionCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(rowSectionCommand);
                return rowSectionCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                rowSectionCommand = value;
            }
        }

        /// <summary>
        /// CloseCommand
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(closeCommand);
                return closeCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                closeCommand = value;
            }
        }

        /// <summary>
        /// navigationClickCommand
        /// </summary>
        public ICommand SelectionChangedCmd
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(selectionChangedCmd);
                return selectionChangedCmd;
            }
            set
            {
                log.LogMethodEntry(value);
                selectionChangedCmd = value;
            }
        }

       
        #endregion Properties
        #region Constructor
        /// <summary>
        /// POSUserAssignmentVM
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="SearchParameters"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public POSUserAssignmentVM(ExecutionContext executionContext, List<UsersDTO> usersDTOList, CashdrawerDTO cashDrawerDTO)
        {
            log.LogMethodEntry(userDTOList, cashDrawerDTO);
            this.ExecutionContext = executionContext;
            this.userDTOList = new ObservableCollection<UsersDTO>(usersDTOList);
            ObservableCollection<string> searchProperties = new ObservableCollection<string>();
            searchProperties.Add("LoginId");

            this.selectedCashDrawerDTO = cashDrawerDTO;
            LoadLables();
            SetDisplayTagsVM();
            dataEntryElements = new Dictionary<string, CustomDataGridColumnElement>();
            dataEntryElements.Add("LoginId", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, "Login Id"), Type = DataEntryType.TextBlock, IsReadOnly = true });
            using (NoSynchronizationContextScope.Enter())
            {
                assignCommand = new DelegateCommand(AssignButtonClick);
                closeCommand = new DelegateCommand(CloseButtonClick);
                navigationClickCommand = new DelegateCommand(NavigationClick);
                selectionChangedCmd = new DelegateCommand(SelectionChanged);
            };
            customDataGridVM = new CustomDataGridVM(ExecutionContext)
            {
                CollectionToBeRendered = new ObservableCollection<object>(userDTOList.OrderByDescending(x => x.UserId).ToList()),
                HeaderCollection = dataEntryElements,
                ShowSearchTextBox = true,
                SelectOption = SelectOption.ManualSelectionOnly,
               // IsComboAndSearchVisible = true,
                SearchProperties = searchProperties
            };
            FooterVM = new FooterVM(this.ExecutionContext)
            {
                Message = "",
                MessageType = MessageType.None,
                HideSideBarVisibility = Visibility.Collapsed
            };
            log.LogMethodExit();
        }
        #endregion Constructor
        #region Methods


        private void SelectionChanged(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            pOSUserAssignmentView = param as POSUserAssignmentView;
            try
            {
                if (pOSUserAssignmentView != null)
                {
                    pOSUserAssignmentView.UpdateLayout();
                    SelectedUserDTO = (UsersDTO)CustomDataGridVM.SelectedItem;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.SuccessMessage = ex.Message;
                SetFooterContent(SuccessMessage, string.IsNullOrEmpty(SuccessMessage) ? MessageType.None : MessageType.Error);
            }
            log.LogMethodExit();
        }
        internal void SetFooterContent(string message, MessageType messageType)
        {
            log.LogMethodEntry(message);
            if (FooterVM != null)
            {
                FooterVM.Message = message;
                FooterVM.MessageType = messageType;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// SetDisplayTagsVM
        /// </summary>
        private void SetDisplayTagsVM()
        {
            log.LogMethodEntry();
            try
            {
                if (DisplayTagsVM == null)
                {
                    DisplayTagsVM = new DisplayTagsVM();
                }
                DisplayTagsVM.DisplayTags = new ObservableCollection<ObservableCollection<DisplayTag>>()
                                    {
                                      new ObservableCollection<DisplayTag>()
                                      {
                                          new DisplayTag()
                                          {
                                               Text = MessageContainerList.GetMessage(ExecutionContext, selectedCashDrawerDTO.CashdrawerName),
                                               TextSize = TextSize.Medium,
                                               FontWeight = System.Windows.FontWeights.Bold
                                          }
                                      }
                                    };
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// LoadLables
        /// </summary>
        private void LoadLables()
        {
            log.LogMethodEntry();
            buttonClose = MessageContainerList.GetMessage(ExecutionContext, "CANCEL");
            buttonOK = MessageContainerList.GetMessage(ExecutionContext, "OK");
            moduleName = MessageContainerList.GetMessage(ExecutionContext, "ASSIGN");
            log.LogMethodExit();
        }

        /// <summary>
        /// AddButtonClick
        /// </summary>
        /// <param name="param"></param>
        private void AssignButtonClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            pOSUserAssignmentView = param as POSUserAssignmentView;
            try
            {
                selectedUserDTO = (UsersDTO)CustomDataGridVM.SelectedItem;
                if (selectedUserDTO == null)
                {
                    ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, 2460);
                    ShowMessagePopup(MessageContainerList.GetMessage(ExecutionContext, "Validation"), MessageContainerList.GetMessage(ExecutionContext, "Required"), ErrorMessage);
                    CustomDataGridVM.SelectedItems.Clear();
                    return;
                }
                if (pOSUserAssignmentView != null)
                {
                    pOSUserAssignmentView.Close();
                }
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                throw;
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// NavigationClick
        /// </summary>
        /// <param name="param"></param>
        private void NavigationClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            pOSUserAssignmentView = param as POSUserAssignmentView;
            try
            {
                if (pOSUserAssignmentView != null)
                {
                   // FormDialogResult = DialogResult.OK;
                    pOSUserAssignmentView.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
               // FormDialogResult = DialogResult.Cancel;
            }
        }

        /// <summary>
        /// CloseButtonClick
        /// </summary>
        /// <param name="param"></param>
        private void CloseButtonClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            pOSUserAssignmentView = param as POSUserAssignmentView;
            try
            {
                if (pOSUserAssignmentView != null)
                {
                   // FormDialogResult = DialogResult.OK;
                    pOSUserAssignmentView.Close();
                }
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                throw;

            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
                //FormDialogResult = DialogResult.Cancel;
            };
        }

        /// <summary>
        /// ShowMessagePopup
        /// </summary>
        /// <param name="heading"></param>
        /// <param name="subHeading"></param>
        /// <param name="content"></param>
        private void ShowMessagePopup(string heading, string subHeading, string content)
        {
            log.LogMethodEntry(heading, subHeading, content);
            GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
            messagePopupView.Owner = pOSUserAssignmentView;
            GenericMessagePopupVM messagePopupVM = new GenericMessagePopupVM(ExecutionContext)
            {
                Heading = heading,
                SubHeading = subHeading,
                Content = content,
                CancelButtonText = MessageContainerList.GetMessage(ExecutionContext, "OK"),
                TimerMilliSeconds = 5000,
                PopupType = PopupType.Timer,
            };
            messagePopupView.DataContext = messagePopupVM;
            messagePopupView.ShowDialog();
            log.LogMethodExit();
        }
        #endregion Methods
    }
}
