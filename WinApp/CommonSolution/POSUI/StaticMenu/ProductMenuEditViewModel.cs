/********************************************************************************************
 * Project Name - POS UI
 * Description  - Product Menu Edit View Model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.130.0     2-Jul-2021   Lakshminarayana          Created for product menu enhancement 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.ViewContainer;
using Semnox.Parafait.POS;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.POSUI.StaticMenu
{
    public class ProductMenuEditViewModel : ViewModelBase
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ICommand confirmClickCommand;
        private ICommand cancelClickCommand;
        private ICommand loadedCommand;
        private ProductMenuEditView productMenuEditView;
        private ProductMenuDTO productMenuDTO; 
        #endregion Members

        #region Constructor
        public ProductMenuEditViewModel(ExecutionContext executionContext, ProductMenuDTO productMenuDTO)
        {
            log.LogMethodEntry(executionContext, productMenuDTO);
            ExecutionContext = executionContext;
            this.productMenuDTO = productMenuDTO;
            InitalizeCommands();
            log.LogMethodExit();
        }
        #endregion Constructor

        #region Properties

        public ProductMenuDTO ProductMenuDTO
        {
            get
            {
                return productMenuDTO;
            }
            set
            {
                productMenuDTO = value;
                OnPropertyChanged("ProductMenuDTO");
                OnPropertyChanged("MenuId");
                OnPropertyChanged("Name");
                OnPropertyChanged("Description");
                OnPropertyChanged("Type");
                OnPropertyChanged("StartDate");
                OnPropertyChanged("EndDate");
                OnPropertyChanged("IsActive");
            }
        }

        public int MenuId 
        { 
            get 
            { 
                return productMenuDTO.MenuId; 
            } set 
                
            {
                productMenuDTO.MenuId = value; 
                OnPropertyChanged("MenuId");
            } 
        }
        public string Name
        {
            get
            {
                return productMenuDTO.Name;
            }
            set

            {
                productMenuDTO.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Description
        {
            get
            {
                return productMenuDTO.Description;
            }
            set

            {
                productMenuDTO.Description = value;
                OnPropertyChanged("Description");
            }
        }

        public string Type
        {
            get
            {
                return productMenuDTO.Type;
            }
            set

            {
                productMenuDTO.Type = value;
                OnPropertyChanged("Type");
            }
        }

        public string StartDate
        {
            get
            {
                return DateValueConverter.ToString(productMenuDTO.StartDate);
            }
            set

            {
                productMenuDTO.StartDate = DateValueConverter.FromString(value);
                OnPropertyChanged("StartDate");
            }
        }
        
        public string EndDate
        {
            get
            {
                return DateValueConverter.ToString(productMenuDTO.EndDate);
            }
            set

            {
                productMenuDTO.EndDate = DateValueConverter.FromString(value);
                OnPropertyChanged("EndDate");
            }
        }

        public bool IsActive
        {
            get
            {
                return productMenuDTO.IsActive;
            }
            set

            {
                productMenuDTO.IsActive = value;
                OnPropertyChanged("IsActive");
            }
        }

        public List<KeyValuePair<string, string>> ProductMenuTypeList
        {
            get
            {
                log.LogMethodEntry();
                List < KeyValuePair<string, string> > result = new List<KeyValuePair<string, string>>();
                result.Add(new KeyValuePair<string, string>("O", MessageViewContainerList.GetMessage(ExecutionContext, "Order Sale")));
                result.Add(new KeyValuePair<string, string>("B", MessageViewContainerList.GetMessage(ExecutionContext, "Reservation")));
                result.Add(new KeyValuePair<string, string>("R", MessageViewContainerList.GetMessage(ExecutionContext, "Redemption")));
                log.LogMethodExit(result);
                return result;
            }
        }

        #endregion Properties


        #region Methods
        private void InitalizeCommands()
        {
            log.LogMethodEntry();
            loadedCommand = new DelegateCommand(OnLoaded);
            confirmClickCommand = new DelegateCommand(OnConfirmClicked);
            cancelClickCommand = new DelegateCommand(OnCancelClicked);
            log.LogMethodExit();
        }
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                productMenuEditView = parameter as ProductMenuEditView;
            }
            log.LogMethodExit();
        }

        private async void OnConfirmClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            try
            {
                IProductMenuUseCases productMenuUseCases = POSUseCaseFactory.GetProductMenuUseCases(ExecutionContext);
                List<ProductMenuDTO> productMenuDTOList = await productMenuUseCases.SaveProductMenuDTOList(new List<ProductMenuDTO>(){ productMenuDTO});
                if(productMenuDTOList != null)
                {
                    ProductMenuDTO = productMenuDTOList[0];
                }

                ProductMenuEditView productMenuEditView = parameter as ProductMenuEditView;
                if (productMenuEditView != null)
                {
                    productMenuEditView.Close();
                }
            }
            catch (Exception ex)
            {
                GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
                messagePopupView.Owner = productMenuEditView;
                messagePopupView.DataContext = new GenericMessagePopupVM(ExecutionContext)
                {
                    Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Error"),
                    //SubHeading = MessageViewContainerList.GetMessage(ExecutionContext, "Required fields"),
                    Content = ex.Message,
                    CancelButtonText = MessageViewContainerList.GetMessage(ExecutionContext, "OK"),
                    TimerMilliSeconds = 5000,
                    PopupType = PopupType.Timer,
                };

                messagePopupView.ShowDialog();
            }
            
            log.LogMethodExit();
        }

        private void OnCancelClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ProductMenuEditView productMenuEditView = parameter as ProductMenuEditView;
            if (productMenuEditView != null)
            {
                productMenuEditView.Close();
            }
            log.LogMethodExit();
        }
        #endregion Methods

        #region Commands
        public ICommand ConfirmClickCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(confirmClickCommand);
                return confirmClickCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                confirmClickCommand = value;
            }
        }

        public ICommand CancelClickCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(cancelClickCommand);
                return cancelClickCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                cancelClickCommand = value;
            }
        }

        public ICommand LoadedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(loadedCommand);
                return loadedCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                loadedCommand = value;
            }
        }


        #endregion Commands
    }
}
