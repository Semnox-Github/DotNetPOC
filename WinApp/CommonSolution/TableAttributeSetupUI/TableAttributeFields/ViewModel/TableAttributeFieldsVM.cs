/********************************************************************************************
 * Project Name - Transaction UI
 * Description  - DeliveryOrder VM
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     09-Sep-2021    Fiona                  Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.TableAttributeSetup;
using Semnox.Parafait.TableAttributeDetailsUtils;
using Semnox.Parafait.ViewContainer;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Globalization;

namespace Semnox.Parafait.TableAttributeSetupUI
{
    public class TableAttributeFieldsVM : PopupWindowViewModel
    {
        #region Members     
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<TableAttributeDetailsDTO> tableAttributeDetailsDTOList;
        private ObservableCollection<DataEntryElement> dataEntryElements;
        private string moduleName;
        private string headerSection;
        private ICommand cancelCommand;
        private ICommand loadedCommand;      
        private TableAttributeFieldsView tableAttributeFieldsView;
        private ICommand okCommand;
        private GenericDataEntryVM genericDataEntryVM;
        private ICommand navigationClickCommand;
        private string errorMessage;
        private ButtonClickType buttonClick;
        private bool isReadonly;
        private bool isOkButtonEnable;
        private string dateFormat;
        private string dateTimeFormat;
        private Dictionary<TableAttributeDetailsDTO, List<KeyValuePair<string, string>>> dictionarySqlData;

        public enum ButtonClickType
        {
            Ok,
            Cancel
        }
        #endregion
        #region Properties
        public string HeaderSection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(headerSection);
                return headerSection;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref headerSection, value);
                log.LogMethodExit();
            }
        }
        public bool IsOkButtonEnable
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isOkButtonEnable);
                return isOkButtonEnable;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref isOkButtonEnable, value);
                log.LogMethodExit();
            }
        }
        public GenericDataEntryVM GenericDataEntryVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(genericDataEntryVM);
                return genericDataEntryVM;
            }
            set
            {
                log.LogMethodEntry();
                genericDataEntryVM = value;
                log.LogMethodExit();
            }
        }

        public ObservableCollection<DataEntryElement> DataEntryElements
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry();
                return dataEntryElements;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref dataEntryElements, value);
                log.LogMethodEntry();
            }
        }
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
                log.LogMethodExit();
            }
        }
        public ICommand CancelCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cancelCommand);
                return cancelCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref cancelCommand, value);
                log.LogMethodExit();
            }
        }
        public ICommand NavigationClickCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(navigationClickCommand);
                return navigationClickCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref navigationClickCommand, value);
                log.LogMethodExit();
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
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref loadedCommand, value);
                log.LogMethodExit();
            }
        }
        public ICommand OkCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(okCommand);
                return okCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref okCommand, value);
                log.LogMethodExit();
            }
        }
        public ButtonClickType ButtonClick
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return buttonClick;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref buttonClick, value);
                log.LogMethodExit();
            }
        }
        public List<TableAttributeDetailsDTO> TableAttributeDetailsDTOList
        {
            get
            {
                return tableAttributeDetailsDTOList;
            }
        }

        #endregion
        #region Methods
        private List<TableAttributeDetailsDTO> GetTableAttributeDetailsDTOList()
        {
            List<TableAttributeDetailsDTO> tableAttributeDetailsDTOs = new List<TableAttributeDetailsDTO>();
            TableAttributeDetailsDTO tableAttributeDetailsDTO = new TableAttributeDetailsDTO("Customer", "Customer", Guid.NewGuid().ToString(), "Remarks", "Code", EnabledAttributesDTO.IsMandatoryOrOptional.Mandatory, TableAttributeSetupDTO.DataTypeEnum.TEXT, -1, string.Empty, string.Empty, string.Empty, null, "A08Z-931-468A", "A08Z-931-468A");
            tableAttributeDetailsDTO.DataValidationRuleList = new List<string>();
            tableAttributeDetailsDTO.DataValidationRuleList.Add(@"^[a-zA-Z0-9]\d{2}[a-zA-Z0-9](-\d{3}){2}[A-Za-z0-9]$");
            tableAttributeDetailsDTOs.Add(tableAttributeDetailsDTO);
            tableAttributeDetailsDTOs.Add(new TableAttributeDetailsDTO("LastName", "Customer", Guid.NewGuid().ToString(), "Remarks", "Remarks", EnabledAttributesDTO.IsMandatoryOrOptional.Optional, TableAttributeSetupDTO.DataTypeEnum.TEXT, -1, string.Empty, string.Empty, string.Empty, null, null,null));
            tableAttributeDetailsDTOs.Add(new TableAttributeDetailsDTO("Date", "Date", Guid.NewGuid().ToString(), "Date", "From Date", EnabledAttributesDTO.IsMandatoryOrOptional.Mandatory, TableAttributeSetupDTO.DataTypeEnum.DATETIME, -1, string.Empty, string.Empty, string.Empty, null, "21-09-2021", "21-09-2021"));
            tableAttributeDetailsDTOs.Add(new TableAttributeDetailsDTO("Date", "Date", Guid.NewGuid().ToString(), "Date", "To Date", EnabledAttributesDTO.IsMandatoryOrOptional.Optional, TableAttributeSetupDTO.DataTypeEnum.DATETIME, -1, string.Empty, string.Empty, string.Empty, null, null, null));
            tableAttributeDetailsDTOs.Add(new TableAttributeDetailsDTO("LookupValue", "LookupValue", Guid.NewGuid().ToString(), "PAYMENT_GATEWAY", "Payment GateWay", EnabledAttributesDTO.IsMandatoryOrOptional.Mandatory, TableAttributeSetupDTO.DataTypeEnum.NONE, 455, string.Empty, string.Empty, string.Empty, null, "Moneris Payment Gateway.", "Moneris Payment Gateway."));
            tableAttributeDetailsDTOs.Add(new TableAttributeDetailsDTO("LookupValue", "LookupValue", Guid.NewGuid().ToString(), "PAYMENT_GATEWAY", "Payment GateWay", EnabledAttributesDTO.IsMandatoryOrOptional.Optional, TableAttributeSetupDTO.DataTypeEnum.NONE, 455, string.Empty, string.Empty, string.Empty, null, null, null));
            tableAttributeDetailsDTOs.Add(new TableAttributeDetailsDTO("AddressType", "AddressType", Guid.NewGuid().ToString(), "AddressType", "Address Type", EnabledAttributesDTO.IsMandatoryOrOptional.Mandatory, TableAttributeSetupDTO.DataTypeEnum.NONE, -1, "AddressType", "Name", "Id", null, "HOME","HOME"));
            tableAttributeDetailsDTOs.Add(new TableAttributeDetailsDTO("AddressType", "AddressType", Guid.NewGuid().ToString(), "AddressType", "Address Type", EnabledAttributesDTO.IsMandatoryOrOptional.Optional, TableAttributeSetupDTO.DataTypeEnum.NONE, -1, "AddressType", "Name", "Id", null, null, ""));
            return tableAttributeDetailsDTOs;
        }
        private void OnCancelClick(object parameter)
        {
            log.LogMethodEntry(parameter);
            try
            {
                ButtonClick = ButtonClickType.Cancel;
                if (tableAttributeFieldsView != null)
                {
                    tableAttributeFieldsView.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again.") + " " + ex.Message, MessageType.Error);
            }
           
            log.LogMethodExit();
        }
        private void OnLoad(object parameter)
        {
            log.LogMethodEntry(parameter);
            if(parameter!=null)
            {
                this.tableAttributeFieldsView = parameter as TableAttributeFieldsView;
            }
            log.LogMethodExit();
        }
       
        private void OnOKClicked(object obj)
        {
            log.LogMethodEntry(obj);
            try
            {
                foreach (DataEntryElement dataEntryElement in dataEntryElements)
                {
                    if (dataEntryElement.IsMandatory && string.IsNullOrEmpty(dataEntryElement.Text) && dataEntryElement.SelectedItem == null)
                    {
                        log.Error("Enter Value for " + dataEntryElement.Heading);
                        SetFooterContent("Enter Value for " + dataEntryElement.Heading, MessageType.Error);
                        return;
                    }
                    if(dataEntryElement.Type == DataEntryType.DatePicker && !string.IsNullOrEmpty(dataEntryElement.Text))
                    {
                        DateTime dt;
                        if (!DateTime.TryParseExact(dataEntryElement.Text, dateTimeFormat,
                                System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                        {
                            string message = MessageViewContainerList.GetMessage(ExecutionContext, "Enter Valid Date Format");
                            log.Error(message);
                            SetFooterContent(message, MessageType.Error);
                            return;
                        }
                            
                    }
                }
                for (int i = 0; i < tableAttributeDetailsDTOList.Count; i++)
                {
                    TableAttributeDetailsDTO tableAttributeDetailsDTO = tableAttributeDetailsDTOList[i];
                    string attributeValue = string.Empty;
                    if (DataEntryElements[i].Type == DataEntryType.ComboBox)
                    {
                        if (tableAttributeDetailsDTO.LookupId > -1 && DataEntryElements[i].SelectedItem != null)
                        {
                            LookupValuesContainerDTO lookupValuesContainerDTO = DataEntryElements[i].SelectedItem as LookupValuesContainerDTO;
                            if (lookupValuesContainerDTO != null)
                            {
                                attributeValue = lookupValuesContainerDTO.Description;
                            }
                        }
                        else if (!string.IsNullOrEmpty(tableAttributeDetailsDTO.SqlSource) && DataEntryElements[i].SelectedItem != null)
                        {
                            List<KeyValuePair<string, string>> sqldata = dictionarySqlData[tableAttributeDetailsDTO];
                            if (sqldata != null)
                            {
                                attributeValue = sqldata.FirstOrDefault(x => x.Value.ToLower() == DataEntryElements[i].SelectedItem.ToString().ToLower()).Key;
                            }
                        }
                    }
                    else
                    {
                        attributeValue = DataEntryElements[i].Text.ToString();
                    }

                    if (tableAttributeDetailsDTO.DataValidationRuleList != null && tableAttributeDetailsDTO.DataValidationRuleList.Any())
                    {

                        for (int j = 0; j < tableAttributeDetailsDTO.DataValidationRuleList.Count; j++)
                        {
                            Regex rgx = new Regex(tableAttributeDetailsDTO.DataValidationRuleList[j]);
                           
                            if (rgx.IsMatch(attributeValue) == false)
                            {

                                string errorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 4095, tableAttributeDetailsDTO.AttributeDisplayName);
                                log.Error(errorMessage);
                                SetFooterContent(errorMessage, MessageType.Error);
                                return;
                                //&1 field value failed to clear data validation rules
                            }
                        }
                    }
                    tableAttributeDetailsDTOList[i].AttributeValue = attributeValue;
                }
                ButtonClick = ButtonClickType.Ok;
                if (tableAttributeFieldsView != null)
                {
                    tableAttributeFieldsView.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again.") + " " + ex.Message, MessageType.Error);
            }
            
            log.LogMethodExit();
        }
        private DataEntryType GetDataEntryType(TableAttributeDetailsDTO tableAttributeDetailsDTO)
        {
            log.LogMethodEntry(tableAttributeDetailsDTO);
            DataEntryType dataEntryType = DataEntryType.TextBox;
            if (tableAttributeDetailsDTO.LookupId==-1 && 
                string.IsNullOrEmpty(tableAttributeDetailsDTO.SqlSource) && 
                tableAttributeDetailsDTO.DataType==TableAttributeSetupDTO.DataTypeEnum.DATETIME)
            {
                dataEntryType = DataEntryType.DatePicker;
            }
            else if(tableAttributeDetailsDTO.LookupId > -1 || !string.IsNullOrEmpty(tableAttributeDetailsDTO.SqlSource))
            {
                dataEntryType = DataEntryType.ComboBox;
            }
            log.LogMethodExit(dataEntryType);
            return dataEntryType;
        }
        private List<KeyValuePair<string, string>> GetSQLDataList(TableAttributeDetailsDTO tableAttributeDetailsDTO)
        {
            log.LogMethodEntry(tableAttributeDetailsDTO);
            ITableAttributeDetailsUseCases tableAttributeDetailsUseCases = TableAttributeDetailsUseCaseFactory.GetTableAttributeDetailsUseCases(ExecutionContext);
            List<KeyValuePair<string, string>> result = null;
            try
            {
                using (NoSynchronizationContextScope.Enter())
                {
                   Task<List<KeyValuePair<string, string>>> task = tableAttributeDetailsUseCases.GetSQLDataList(tableAttributeDetailsDTO.SqlSource, tableAttributeDetailsDTO.SqlDisplayMember, tableAttributeDetailsDTO.SqlValueMember);
                   task.Wait();
                   result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again.") + " " + ex.Message, MessageType.Error);
            }
            log.LogMethodExit(result);
            return result;
        }
        private void InitialzeDataEntryElements()
        {
            log.LogMethodEntry();
            List<LookupsContainerDTO> lookupsContainerDTOList = LookupsViewContainerList.GetLookupsContainerDTOList(ExecutionContext);
            DataEntryElements = new ObservableCollection<DataEntryElement>();
            dictionarySqlData = new Dictionary<TableAttributeDetailsDTO, List<KeyValuePair<string, string>>>();
            for (int i = 0; i < tableAttributeDetailsDTOList.Count; i++)
            {
                TableAttributeDetailsDTO tableAttributeDetailsDTO = tableAttributeDetailsDTOList[i];
                DataEntryElement dataEntryElement = new DataEntryElement
                {
                    Type = GetDataEntryType(tableAttributeDetailsDTO),
                    Heading = tableAttributeDetailsDTO.AttributeDisplayName,
                    IsReadOnly = isReadonly,
                };

                if(dataEntryElement.Type==DataEntryType.ComboBox)
                {
                    dataEntryElement.IsEnabled = !isReadonly;
                }
                
                if (tableAttributeDetailsDTO.MandatoryOrOptional == EnabledAttributesDTO.IsMandatoryOrOptional.Mandatory)
                {
                    dataEntryElement.IsMandatory = true;
                }
                else
                {
                    dataEntryElement.IsMandatory = false;
                }

                if(tableAttributeDetailsDTO.DataType==TableAttributeSetupDTO.DataTypeEnum.NUMBER)
                {
                    dataEntryElement.ValidationType = ValidationType.DecimalOnly;
                }
                else if(tableAttributeDetailsDTO.DataType == TableAttributeSetupDTO.DataTypeEnum.TEXT)
                {
                    dataEntryElement.ValidationType = ValidationType.Alphanumeric;
                }

                if (tableAttributeDetailsDTO.LookupId > -1)
                {
                    if (lookupsContainerDTOList != null && lookupsContainerDTOList.Any())
                    {
                        List<LookupValuesContainerDTO> lookupValueContainerDTOList = lookupsContainerDTOList.Where(x => x.LookupId == tableAttributeDetailsDTO.LookupId).FirstOrDefault().LookupValuesContainerDTOList;
                        if(lookupValueContainerDTOList==null)
                        {
                            return;
                        }
                        dataEntryElement.Options = new ObservableCollection<object>(lookupValueContainerDTOList);
                        dataEntryElement.DisplayMemberPath = "LookupValue";
                        if(!string.IsNullOrEmpty(tableAttributeDetailsDTO.AttributeValue))
                        {
                            LookupValuesContainerDTO selectedValue = lookupValueContainerDTOList.FirstOrDefault(x => x.Description.ToLower() == tableAttributeDetailsDTO.AttributeValue.ToLower());
                            if(selectedValue!=null)
                            {
                                dataEntryElement.SelectedItem = selectedValue;
                            }
                        }
                        else if (!string.IsNullOrEmpty(tableAttributeDetailsDTO.DefaultAttributeValue))
                        {
                            LookupValuesContainerDTO selectedValue = lookupValueContainerDTOList.FirstOrDefault(x => x.Description.ToLower() == tableAttributeDetailsDTO.DefaultAttributeValue.ToLower());
                            if (selectedValue != null)
                            {
                                dataEntryElement.SelectedItem = selectedValue;
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(tableAttributeDetailsDTO.SqlSource))
                {
                    List<KeyValuePair<string, string>> sqlData = GetSQLDataList(tableAttributeDetailsDTO);
                    dictionarySqlData.Add(tableAttributeDetailsDTO, sqlData);
                    List<string> sqlValues = sqlData.Select(x => x.Value).ToList();
                    dataEntryElement.Options = new ObservableCollection<object>(sqlValues);
                    if (!string.IsNullOrEmpty(tableAttributeDetailsDTO.AttributeValue))
                    {
                        string selectedValue = sqlData.FirstOrDefault(x => x.Key.ToLower() == tableAttributeDetailsDTO.AttributeValue.ToLower()).Value;
                        if(!string.IsNullOrEmpty(selectedValue))
                        {
                            dataEntryElement.SelectedItem = selectedValue;
                        }
                    }
                    else if(!string.IsNullOrEmpty(tableAttributeDetailsDTO.DefaultAttributeValue))
                    {
                        string selectedValue = sqlData.FirstOrDefault(x => x.Key.ToLower() == tableAttributeDetailsDTO.DefaultAttributeValue.ToLower()).Value;
                        if (!string.IsNullOrEmpty(selectedValue))
                        {
                            dataEntryElement.SelectedItem = selectedValue;
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(tableAttributeDetailsDTO.AttributeValue))
                    {
                        if(dataEntryElement.Type == DataEntryType.DatePicker)
                        {
                            if (!string.IsNullOrEmpty(tableAttributeDetailsDTO.AttributeValue))
                            {
                                DateTime dateTime = DateTime.Parse(tableAttributeDetailsDTO.AttributeValue);
                                dataEntryElement.Text = dateTime.ToString(dateTimeFormat);
                            }
                        }
                        else
                        {
                            dataEntryElement.Text = tableAttributeDetailsDTO.AttributeValue;
                        }
                        
                    }
                    else if(!string.IsNullOrEmpty(tableAttributeDetailsDTO.DefaultAttributeValue))
                    {
                        if (dataEntryElement.Type == DataEntryType.DatePicker)
                        {
                            DateTime dateTime = DateTime.MinValue;
                            if (DateTime.TryParse(tableAttributeDetailsDTO.DefaultAttributeValue, out dateTime))
                            {
                                dateTime = DateTime.Parse(tableAttributeDetailsDTO.DefaultAttributeValue);
                                dataEntryElement.Text = dateTime.ToString(dateTimeFormat);
                            }
                        }
                        else
                        {
                            dataEntryElement.Text = tableAttributeDetailsDTO.DefaultAttributeValue;
                        }
                    }
                }
               
                DataEntryElements.Add(dataEntryElement);
            }
            log.LogMethodExit();
        }


        private void OnNavigationClick(object parameter)
        {
            log.LogMethodEntry(parameter);
            ButtonClick = ButtonClickType.Cancel;
            if (tableAttributeFieldsView != null)
            {
                tableAttributeFieldsView.Close();
            }
            log.LogMethodExit();
        }

        #endregion
        #region Constructor
        public TableAttributeFieldsVM(ExecutionContext executionContext, List<TableAttributeDetailsDTO> tableAttributeDetailsDTOList, string headerSectionMessage, bool isReadonly=false) : 
            base(executionContext,null)
        {
            log.LogMethodEntry(executionContext, tableAttributeDetailsDTOList);
            this.ExecutionContext = executionContext;
            this.tableAttributeDetailsDTOList = tableAttributeDetailsDTOList;/*GetTableAttributeDetailsDTOList();*/

            HeaderSection = headerSectionMessage;
            ButtonClick = ButtonClickType.Cancel;
            this.isReadonly = isReadonly;
            LoadedCommand = new DelegateCommand(OnLoad);
            CancelCommand = new DelegateCommand(OnCancelClick);
            OkCommand = new DelegateCommand(OnOKClicked);
            dateTimeFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "DATETIME_FORMAT");
            dateFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "DATE_FORMAT");
            NavigationClickCommand = new DelegateCommand(OnNavigationClick);
            InitialzeDataEntryElements();
            IsOkButtonEnable = (isReadonly == false);
            FooterVM = new FooterVM(executionContext)
            {
                MessageType = MessageType.None,
                Message = string.Empty,
                HideSideBarVisibility = Visibility.Collapsed
            };
            log.LogMethodExit();
        }
      
        #endregion

    }
}
