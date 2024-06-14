/********************************************************************************************
 * Project Name - Invenotry UI
 * Description  - POReceiveRemarksVM
 * 
 **************
 **Version log
 **************
 *Version     Date          Modified By            Remarks          
 *********************************************************************************************
 *2.120       04-Mar-2021   Girish Kundar          Created - Is Radian change
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Languages;
using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;

namespace Semnox.Parafait.InventoryUI
{

    /// <summary>
    /// POReceiveRemarksVM
    /// </summary>
    public class POReceiveRemarksVM : BaseWindowViewModel
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DialogResult dialogResult;
        private string message;
        private string remarks;
        private string buttonClose;
        private string buttonOK;
        private POReceiveRemarkView poReceiveRemarksView;
        private DisplayTagsVM displayTagsVM;
        private CustomDataGridVM customDataGridVM;
        private ObservableCollection<ProductViewDTO> gridSource;
        private ICommand addCommand;
        private ICommand closeCommand;
        private ICommand navigationClickCommand;
        private string labelRemarks;
        private string moduleName;
        #endregion Members
        #region Properties
       

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
        /// LableRemarks
        /// </summary>
        public string LableRemarks
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(labelRemarks);
                return labelRemarks;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref labelRemarks, value);
            }
        }
        /// <summary>
        /// remarks
        /// </summary>
        public string Remarks
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(remarks);
                return remarks;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref remarks, value);
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
        /// ButtonClose
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
        public DialogResult FormDialogResult
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(dialogResult);
                return dialogResult;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref dialogResult, value);
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
        /// AddCommand
        /// </summary>
        public ICommand AddCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(addCommand);
                return addCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                addCommand = value;
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

        #endregion Properties
        #region Constructor
        /// <summary>
        /// ProductVM
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="SearchParameters"></param>
        /// <param name="remarks"></param>
        public POReceiveRemarksVM(ExecutionContext executionContext,string remarks)
        {
            log.LogMethodEntry(remarks);
            this.ExecutionContext = executionContext;
            LoadLables();
            SetDisplayTagsVM();
             using (NoSynchronizationContextScope.Enter())
            {
                addCommand = new DelegateCommand(AddButtonClick);
                closeCommand = new DelegateCommand(CloseButtonClick);
                navigationClickCommand = new DelegateCommand(NavigationClick);
            }
            log.LogMethodExit();
        }
        #endregion Constructor
        #region Methods
        private void SetDisplayTagsVM()
        {
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
                                               Text = MessageContainerList.GetMessage(ExecutionContext,  " REMARKS "),
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

        }

        private void LoadLables()
        {
            log.LogMethodEntry();
            buttonClose = MessageContainerList.GetMessage(ExecutionContext, "CLEAR");
            buttonOK = MessageContainerList.GetMessage(ExecutionContext, "OK");
            moduleName = MessageContainerList.GetMessage(ExecutionContext, "REMARKS");
            labelRemarks = MessageContainerList.GetMessage(ExecutionContext, "ENTER REMARK");
            log.LogMethodExit();
        }
        private void AddButtonClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            poReceiveRemarksView = param as POReceiveRemarkView;
            try
            {
                if (poReceiveRemarksView != null)
                {
                    SuccessMessage = Remarks;
                    poReceiveRemarksView.Close();
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
            FormDialogResult = DialogResult.OK;
            log.LogMethodExit();
        }


        private void NavigationClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            poReceiveRemarksView = param as POReceiveRemarkView;
            try
            {
                if (poReceiveRemarksView != null)
                {
                    FormDialogResult = DialogResult.OK;
                    poReceiveRemarksView.Close();
                }
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
                FormDialogResult = DialogResult.Cancel;
            }
            log.LogMethodExit();
        }
        private void CloseButtonClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            poReceiveRemarksView = param as POReceiveRemarkView;
            try
            {
                if (poReceiveRemarksView != null)
                {
                    FormDialogResult = DialogResult.OK;
                    Remarks = string.Empty;
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
                FormDialogResult = DialogResult.Cancel;
            };
            log.LogMethodExit();
        }

        private void ShowMessagePopup(string heading, string subHeading, string content)
        {
            log.LogMethodEntry(heading, subHeading, content);

            GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
            messagePopupView.Owner = poReceiveRemarksView;

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

        private void CloseAddWindow(string message)
        {
            SuccessMessage = message;
            if (poReceiveRemarksView != null)
            {
                poReceiveRemarksView.Close();
            }
        }
        #endregion Methods
    }
}
