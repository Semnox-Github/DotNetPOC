/********************************************************************************************
 * Project Name - TagsUI
 * Description  - PerformManualEventVM 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By            Remarks          
 *********************************************************************************************
*2.130.4        04-Mar-2021   Girish Kundar          Created - Is Radian change
*2.130.4        13-Apr-2021      Girish Kundar          Modified: Issue Fixes
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Languages;
namespace Semnox.Parafait.PrintUI
{



    public class PrintOptionDTO
    {
        private string key;
        private string option;
        public string Key
        {
            get { return key; }
            set { key = value; }
        }
        public string Option
        {
            get { return option; }
            set { option = value; }
        }
        public PrintOptionDTO(string key, string value)
        {
            this.key = key;
            this.option = value;
        }
    }

    /// <summary>
    /// VCATPrintOptionVM
    /// </summary>
    public class VCATPrintOptionVM : BaseWindowViewModel
    {
        private enum VCATPrintOptions
        {
            Customer,
            Business,
            No
        }

        #region Members
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string printOption;
        private DisplayTagsVM displayTagsVM;
        private const string VOLUNTEER_ISSUANCE = "V";
        private VCATPrintOptionView vCATPrintOptionView;
        private ObservableCollection<PrintOptionDTO> printOptions;
        private ICommand closeCommand;
        private ICommand loadedCommand;
        private ICommand optionButtionClicked;

        // Lables 
        private string labelOptionContent;
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
                return displayTagsVM;
            }
            set
            {
                SetProperty(ref displayTagsVM, value);
            }
        }
        public ObservableCollection<PrintOptionDTO> PrintOptionButtons
        {
            get
            {
                return printOptions;
            }
            set
            {
                SetProperty(ref printOptions, value);
            }
        }
        /// <summary>
        /// ModuleName
        /// </summary>
        public string ModuleName
        {
            get
            {
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
        public string LabelOptionContent
        {
            get
            {
                return labelOptionContent;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref labelOptionContent, value);
                }
            }
        }

        /// <summary>
        /// CloseCommand
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                return closeCommand;
            }
            set
            {
                SetProperty(ref closeCommand, value);
            }
        }

        /// <summary>
        /// LoadedCommand
        /// </summary>
        public ICommand LoadedCommand
        {
            get
            {
                return loadedCommand;
            }
            set
            {
                SetProperty(ref loadedCommand, value);
            }
        }
        /// <summary>
        /// OptionButtonClicked
        /// </summary>
        public ICommand OptionButtonClicked
        {
            get
            {
                return optionButtionClicked;
            }
            set
            {
                SetProperty(ref optionButtionClicked, value);
            }
        }

        /// <summary>
        /// PrintOption
        /// </summary>
        public string PrintOption
        {
            get
            {
                return printOption;
            }
            set
            {
                SetProperty(ref printOption, value);
            }
        }
        /// <summary>
        /// tagCommands
        /// </summary>

        #endregion Properties

        #region Constructor
        /// <summary>
        /// NotificationTagsVM
        /// </summary>
        /// <param name="executionContext"></param>
        public VCATPrintOptionVM(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.ExecutionContext = executionContext;
            InitializeCommands();
            InitializeOptionButtons();
            SetDisplayTagsVM();
            log.LogMethodExit();
        }
        #endregion Constructor

        #region Methods

        private void SetDisplayTagsVM()
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
                                               Text = MessageContainerList.GetMessage(ExecutionContext,  4238),
                                               TextSize = TextSize.Medium,
                                               FontWeight = System.Windows.FontWeights.Bold
                                          }
                                      }
                                    };

        }


        private void ShowPopUp()
        {
            log.LogMethodEntry();
            PerformClose();
            log.LogMethodExit(printOption);
        }

        private void PerformClose()
        {
            if (vCATPrintOptionView != null)
            {
                vCATPrintOptionView.Close();
            }
        }

        private void InitializeCommands()
        {
            LoadedCommand = new DelegateCommand(OnLoaded);
            CloseCommand = new DelegateCommand(Close);
            OptionButtonClicked = new DelegateCommand(OnPrintOptionClick);
        }

        private void InitializeOptionButtons()
        {
            log.LogMethodEntry();
            List<PrintOptionDTO> vCATPrintOptions = new List<PrintOptionDTO>();
            vCATPrintOptions.Add(new PrintOptionDTO("C", (MessageContainerList.GetMessage(ExecutionContext, "Customer"))));
            vCATPrintOptions.Add(new PrintOptionDTO("B", (MessageContainerList.GetMessage(ExecutionContext, "Business Person"))));
            vCATPrintOptions.Add(new PrintOptionDTO("V", (MessageContainerList.GetMessage(ExecutionContext, "Volunteer Issuance"))));
            PrintOptionButtons = new ObservableCollection<PrintOptionDTO>(vCATPrintOptions);
            LabelOptionContent = MessageContainerList.GetMessage(ExecutionContext, 4238);
            log.LogMethodExit();
        }
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry();
            vCATPrintOptionView = parameter as VCATPrintOptionView;
            log.LogMethodExit();
        }
        private void OnPrintOptionClick(object parameter)
        {
            log.LogMethodEntry();
            if (parameter != null)
            {
                string option = parameter.ToString();
                switch (option)
                {
                    case "C":
                        {
                            PrintOption = "CUSTOMER";
                        }
                        break;
                    case "B":
                        {
                            PrintOption = "BUSINESS_PERSON";
                        }
                        break;
                    case "V":
                        {
                            PrintOption = "VOLUNTEER_ISSUANCE";
                        }
                        break;
                    default:
                        {
                            PrintOption = "CUSTOMER";
                        }
                        break;
                }
            }
            PerformClose();
            log.LogMethodExit();
        }
        private void Close(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            vCATPrintOptionView = param as VCATPrintOptionView;
            try
            {
                if (vCATPrintOptionView != null)
                {
                    PrintOption = string.Empty;
                    vCATPrintOptionView.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            };
        }
        private void NavigationClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            vCATPrintOptionView = param as VCATPrintOptionView;
            try
            {
                if (vCATPrintOptionView != null)
                {
                    vCATPrintOptionView.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            };
        }


        private void CloseAddWindow(string message)
        {
            if (vCATPrintOptionView != null)
            {
                vCATPrintOptionView.Close();
            }
        }

        #endregion Methods
    }
}
