/********************************************************************************************
 * Project Name - Transaction UI
 * Description  - PaymentSettlement VM
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     17-Sep-2021    Fiona                  Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.POS;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Transaction.KDS;
using Semnox.Parafait.User;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.ViewContainer;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Authentication;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Customer;
using Semnox.Parafait.TableAttributeSetup;
using Semnox.Parafait.TableAttributeDetailsUtils;

namespace Semnox.Parafait.TransactionUI
{
    public class AdvancedFilterVM : BaseWindowViewModel
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string heading;
        private ObservableCollection<AdvancedFiltersSummaryDTO> advancedFiltersSummaryDTOCollection;
        private string attributeValue;
        private ObservableCollection<TableAttributeDetailsDTO> tableAttributeDetailsDTOCollection;
        private TableAttributeDetailsDTO selectedTableAttributeDetail;
        private string selectedCondition;
        private string recordGuid;
        private ICommand loadedCommand;
        private ICommand actionsCommand;
        private AdvancedFilterView advancedFilterView;
        private ButtonClickType buttonClick;
        private ObservableCollection<string> conditions;
        private EnabledAttributesDTO.TableWithEnabledAttributes tableWithEnabledAttributes;
        private ICommand removeCommand;
        private ICommand attributesSelectionChangedCommand;
        private List<LookupsContainerDTO> lookupsContainerDTOList;
        private ObservableCollection<string> attributeValueCBCollection;
        private List<string> operatorsForDateTime;
        private List<string> operatorsForText;
        private List<string> operatorsForNumber;
        private AttributeValueType selectedAttributeValueType;
        private Dictionary<TableAttributeDetailsDTO, List<KeyValuePair<string, string>>> dictionarySqlData;


        



        public enum AttributeValueType
        {
            Text = 0,
            Combo = 1,
            Date = 2,
            Number=3
        }
        
        public enum ButtonClickType
        {
            Ok,
            Cancel
        }
        
        #endregion
        #region Properties
        public string Heading
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return heading;
            }
            set
            {
                if (!object.Equals(heading, value))
                {
                    heading = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<AdvancedFiltersSummaryDTO> AdvancedFiltersSummaryDTOCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(advancedFiltersSummaryDTOCollection);
                return advancedFiltersSummaryDTOCollection;
            }
            set
            {
                if (!object.Equals(advancedFiltersSummaryDTOCollection, value))
                {
                    advancedFiltersSummaryDTOCollection = value;
                    OnPropertyChanged();
                }
            }
        }
        public TableAttributeDetailsDTO SelectedTableAttributeDetail
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedTableAttributeDetail);
                return selectedTableAttributeDetail;
            }
            set
            {
                if (!object.Equals(selectedTableAttributeDetail, value))
                {
                    selectedTableAttributeDetail = value;
                    OnPropertyChanged();
                }
            }
        }
        public string AttributeValue
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(attributeValue);
                return attributeValue;
            }
            set
            {
                if (!object.Equals(attributeValue, value))
                {
                    attributeValue = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<TableAttributeDetailsDTO> TableAttributeDetailsDTOCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return tableAttributeDetailsDTOCollection;
            }
            set
            {
                if (!object.Equals(tableAttributeDetailsDTOCollection, value))
                {
                    tableAttributeDetailsDTOCollection = value;
                    OnPropertyChanged();
                }
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
                if (!object.Equals(loadedCommand, value))
                {
                    loadedCommand = value;
                    OnPropertyChanged();
                }
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
            set
            {
                if (!object.Equals(actionsCommand, value))
                {
                    actionsCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        public string SelectedCondition
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return selectedCondition;
            }
            set
            {
                if (!object.Equals(selectedCondition, value))
                {
                    selectedCondition = value;
                    OnPropertyChanged();
                }
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
                if (!object.Equals(buttonClick, value))
                {
                    buttonClick = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public ObservableCollection<string> Conditions
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(conditions);
                return conditions;
            }
            set
            {
                if (!object.Equals(conditions, value))
                {
                    conditions = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand RemoveCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(removeCommand);
                return removeCommand;
            }
            set
            {
                if (!object.Equals(removeCommand, value))
                {
                    removeCommand = value;
                    OnPropertyChanged();
                }
            }
        }
       
        public ObservableCollection<string> AttributeValueCBCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(attributeValueCBCollection);
                return attributeValueCBCollection;
            }
            set
            {
                if (!object.Equals(attributeValueCBCollection, value))
                {
                    attributeValueCBCollection = value;
                    OnPropertyChanged();
                }
            }
        }
        public AttributeValueType SelectedAttributeValueType
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedAttributeValueType);
                return selectedAttributeValueType;
            }
            set
            {
                if (!object.Equals(selectedAttributeValueType, value))
                {
                    selectedAttributeValueType = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand AttributesSelectionChangedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(attributesSelectionChangedCommand);
                return attributesSelectionChangedCommand;
            }
            set
            {
                if (!object.Equals(attributesSelectionChangedCommand, value))
                {
                    attributesSelectionChangedCommand = value;
                    OnPropertyChanged();
                }
            }
        }


        #endregion
        #region Methods
        //private void GetTableAttributes()
        //{
        //    AdvancedFiltersSummaryDTOCollection.Add(new AdvancedFiltersSummaryDTO
        //    {
        //        AttributeDisplayName="Cash",
        //        Condition="Less than",
        //        AttributeValue="Infosys",
        //        AddLabelVisibility=Visibility.Hidden
        //    });
        //    AdvancedFiltersSummaryDTOCollection.Add(new AdvancedFiltersSummaryDTO
        //    {
        //        AttributeDisplayName = "Coporate",
        //        Condition = "Contains",
        //        AttributeValue = "Wipro",
        //        AddLabelVisibility = Visibility.Visible
        //    });
        //    AdvancedFiltersSummaryDTOCollection.Add(new AdvancedFiltersSummaryDTO
        //    {
        //        AttributeDisplayName = "Coporate",
        //        Condition = "Contains",
        //        AttributeValue = "Infosys",
        //        AddLabelVisibility = Visibility.Visible
        //    });
        //    AdvancedFiltersSummaryDTOCollection.Add(new AdvancedFiltersSummaryDTO
        //    {
        //        AttributeDisplayName = "Coporate",
        //        Condition = "Contains",
        //        AttributeValue = "Wipro",
        //        AddLabelVisibility = Visibility.Visible
        //    });
        //    AdvancedFiltersSummaryDTOCollection.Add(new AdvancedFiltersSummaryDTO
        //    {
        //        AttributeDisplayName = "Coporate",
        //        Condition = "Contains",
        //        AttributeValue = "Infosys",
        //        AddLabelVisibility = Visibility.Visible
        //    });
        //}
        private string GetMessage(string message = "", int messageNo = -1)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return messageNo > -1 ? MessageViewContainerList.GetMessage(ExecutionContext, messageNo) : MessageViewContainerList.GetMessage(ExecutionContext, message);
        }
        private IOperator GetOperator(Operator condition)
        {
            log.LogMethodEntry(condition);
            IOperator @operator = OperatorFactory.GetOperator(condition);
            log.LogMethodExit(@operator);
            return @operator;
        }
        private void FetchOperatorsForDateTime()
        {
            log.LogMethodEntry();
            operatorsForDateTime = new List<string>();
            operatorsForDateTime.Add(GetOperator(Operator.EQUAL_TO).DisplayName.Trim());
            operatorsForDateTime.Add(GetOperator(Operator.GREATER_THAN).DisplayName.Trim());
            operatorsForDateTime.Add(GetOperator(Operator.LESSER_THAN).DisplayName.Trim());
            operatorsForDateTime.Add(GetOperator(Operator.GREATER_THAN_OR_EQUAL_TO).DisplayName.Trim());
            operatorsForDateTime.Add(GetOperator(Operator.LESSER_THAN_OR_EQUAL_TO).DisplayName.Trim());
            log.LogMethodExit();
        }
        private void FetchOperatorsForText()
        {
            log.LogMethodEntry();
            operatorsForText = new List<string>();
            operatorsForText.Add(GetOperator(Operator.EQUAL_TO).DisplayName.Trim());
            operatorsForText.Add(GetOperator(Operator.NOT_EQUAL_TO).DisplayName.Trim());
            operatorsForText.Add(GetOperator(Operator.LIKE).DisplayName.Trim());
            operatorsForText.Add(GetOperator(Operator.NOT_LIKE).DisplayName.Trim());
            log.LogMethodExit();
        }
        private void FetchOperatorsForNumbers()
        {
            log.LogMethodEntry();
            operatorsForNumber = new List<string>();
            operatorsForNumber.Add(GetOperator(Operator.EQUAL_TO).DisplayName.Trim());
            operatorsForNumber.Add(GetOperator(Operator.GREATER_THAN).DisplayName.Trim());
            operatorsForNumber.Add(GetOperator(Operator.LESSER_THAN).DisplayName.Trim());
            operatorsForNumber.Add(GetOperator(Operator.GREATER_THAN_OR_EQUAL_TO).DisplayName.Trim());
            operatorsForNumber.Add(GetOperator(Operator.LESSER_THAN_OR_EQUAL_TO).DisplayName.Trim());
            log.LogMethodExit();

        }

        private void GetConditions()
        {
            log.LogMethodEntry();
            List<string> operators=new List<string>();
            foreach (Operator condition in Enum.GetValues(typeof(Operator)))
            {
                IOperator @operator = OperatorFactory.GetOperator(condition);
                string displayName = GetMessage(@operator.DisplayName);
                operators.Add(displayName);
            }
            log.LogMethodExit();
        }
        private void GetTableAttributeDetailsDTOCollection()
        {
            log.LogMethodEntry();
            ITableAttributeDetailsUseCases tableAttributeDetailsUseCases = TableAttributeDetailsUseCaseFactory.GetTableAttributeDetailsUseCases(ExecutionContext);
            List<TableAttributeDetailsDTO> tableAttributeDetailsDTOList = null;
            using (NoSynchronizationContextScope.Enter())
            {
                Task<List<TableAttributeDetailsDTO>> task  = tableAttributeDetailsUseCases.GetTableAttributeDetailsDTOList(tableWithEnabledAttributes, recordGuid);
                task.Wait();
                tableAttributeDetailsDTOList= task.Result;
            }
            TableAttributeDetailsDTOCollection = new ObservableCollection<TableAttributeDetailsDTO>(tableAttributeDetailsDTOList);
                
            log.LogMethodExit();
        }
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                advancedFilterView = parameter as AdvancedFilterView;
            }
            log.LogMethodExit();
        }
        private void InitializeCommands()
        {
            log.LogMethodEntry();
            
            LoadedCommand = new DelegateCommand(OnLoaded);
            ActionsCommand = new DelegateCommand(OnActionsClicked);
            PropertyChanged += OnPropertyChanged;
            RemoveCommand = new DelegateCommand(OnRemoveClicked);
            
            log.LogMethodExit();
        }

       

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (!string.IsNullOrEmpty(e.PropertyName))
            {
                switch(e.PropertyName)
                {
                    case "SelectedTableAttributeDetail":
                        {
                            OnAttributesSelectionChanged();
                        }
                        break;
                }
            }
            log.LogMethodExit();
        }

        private void OnAttributesSelectionChanged()
        {
            log.LogMethodEntry();
            AttributeValue = string.Empty;
            IsLoadingVisible = true;
            if (selectedTableAttributeDetail != null)
            {
                if(selectedTableAttributeDetail.DataType == TableAttributeSetupDTO.DataTypeEnum.DATETIME)
                {
                    SelectedAttributeValueType = AttributeValueType.Date;
                }
                else if(selectedTableAttributeDetail.LookupId !=-1 || !string.IsNullOrEmpty(selectedTableAttributeDetail.SqlSource))
                {
                    SelectedAttributeValueType = AttributeValueType.Combo;
                }
                else if(selectedTableAttributeDetail.DataType == TableAttributeSetupDTO.DataTypeEnum.TEXT)
                {
                    SelectedAttributeValueType = AttributeValueType.Text;
                }
                else
                {
                    SelectedAttributeValueType = AttributeValueType.Number;
                }

                if(selectedTableAttributeDetail.LookupId > -1)
                {
                    if (lookupsContainerDTOList != null && lookupsContainerDTOList.Any())
                    {
                        List<LookupValuesContainerDTO> lookupValueContainerDTOList = lookupsContainerDTOList.Where(x => x.LookupId == selectedTableAttributeDetail.LookupId).FirstOrDefault().LookupValuesContainerDTOList;
                        if (lookupValueContainerDTOList == null)
                        {
                            lookupValueContainerDTOList = new List<LookupValuesContainerDTO>();
                        }
                        List<string> loopupvalues = lookupValueContainerDTOList.Select(x => x.LookupValue).ToList();
                        AttributeValueCBCollection = new ObservableCollection<string>(loopupvalues);
                    }
                }
                else if(!string.IsNullOrEmpty(selectedTableAttributeDetail.SqlSource))
                {
                    List<KeyValuePair<string, string>> sqlData = GetSQLDataList(selectedTableAttributeDetail);
                    if(!dictionarySqlData.ContainsKey(selectedTableAttributeDetail))
                    {
                        dictionarySqlData.Add(selectedTableAttributeDetail, sqlData);
                    }
                    List<string> sqlValues = sqlData.Select(x => x.Value).ToList();
                    AttributeValueCBCollection = new ObservableCollection<string>(sqlValues);
                }

                if(selectedTableAttributeDetail.DataType == TableAttributeSetupDTO.DataTypeEnum.DATETIME)
                {
                    Conditions = new ObservableCollection<string>(operatorsForDateTime);
                }
                else if(selectedTableAttributeDetail.DataType == TableAttributeSetupDTO.DataTypeEnum.NUMBER)
                {
                    Conditions = new ObservableCollection<string>(operatorsForNumber);
                }
                else
                {
                    Conditions = new ObservableCollection<string>(operatorsForText);
                }
            }
            IsLoadingVisible = false;
            log.LogMethodExit();
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

        private void OnRemoveClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            AdvancedFiltersSummaryDTO advancedFiltersSummaryDTO = parameter as AdvancedFiltersSummaryDTO;
            AdvancedFiltersSummaryDTOCollection.Remove(advancedFiltersSummaryDTO);
            RefershAdvancedFiltersSummaryDTOCollection();
            log.LogMethodExit();
        }

        private void RefershAdvancedFiltersSummaryDTOCollection()
        {
            log.LogMethodEntry();
            if(AdvancedFiltersSummaryDTOCollection.Count!=0)
            {
                advancedFiltersSummaryDTOCollection[0].AddLabelVisibility = Visibility.Hidden;
                for (int i = 1; i < AdvancedFiltersSummaryDTOCollection.Count; i++)
                {
                    advancedFiltersSummaryDTOCollection[i].AddLabelVisibility = Visibility.Visible;
                }
            }
            AdvancedFiltersSummaryDTOCollection = advancedFiltersSummaryDTOCollection;
            log.LogMethodExit();
        }

        private void OnActionsClicked(object parameter)
        {

            if (parameter != null)
            {
                Button button = parameter as Button;
                if (button != null && !string.IsNullOrEmpty(button.Name))
                {
                    switch (button.Name)
                    {
                        case "btnAdd":
                            {
                                AddTableAttributes();
                            }
                            break;
                        case "btnOK":
                            {
                                OnOkClicked();
                            }
                            break;
                        case "btnCancel":
                            {
                                OnCancelledClicked();
                            }
                            break;
                        //case "btnRemove":
                        //    {
                        //        RemoveTableAttributes();
                        //    }
                        //    break;

                    }
                }
            }
        }
        private void OnOkClicked()
        {
            log.LogMethodEntry();
            buttonClick = ButtonClickType.Ok;
            if(advancedFilterView!=null)
            {
                advancedFilterView.Close();
            }
            log.LogMethodExit();
        }
        private void AddTableAttributes()
        {
            log.LogMethodEntry();
            IsLoadingVisible = true;
            
            if (selectedTableAttributeDetail==null || string.IsNullOrEmpty(selectedCondition) || string.IsNullOrEmpty(attributeValue))
            {
                IsLoadingVisible = false;
                return;
            }
            AdvancedFiltersSummaryDTO advancedFiltersSummaryDTO = new AdvancedFiltersSummaryDTO()
            {
                EnabledAttributeName = selectedTableAttributeDetail.EnabledAttributeName,
                AttributeDisplayName = selectedTableAttributeDetail.AttributeDisplayName,
                Condition = selectedCondition,
                AttributeValue = attributeValue
            };
            if (!string.IsNullOrEmpty(selectedTableAttributeDetail.SqlSource))
            {
                List<KeyValuePair<string, string>> sqldata = dictionarySqlData[selectedTableAttributeDetail];
                if (sqldata != null)
                {
                    advancedFiltersSummaryDTO.Value = sqldata.FirstOrDefault(x => x.Value.ToLower() == attributeValue.ToLower()).Key;
                }
            }
            else if(selectedTableAttributeDetail.LookupId!=-1)
            {
                if (lookupsContainerDTOList != null && lookupsContainerDTOList.Any())
                {
                    List<LookupValuesContainerDTO> lookupValueContainerDTOList = lookupsContainerDTOList.Where(x => x.LookupId == selectedTableAttributeDetail.LookupId).FirstOrDefault().LookupValuesContainerDTOList;
                    if (lookupValueContainerDTOList == null)
                    {
                        lookupValueContainerDTOList = new List<LookupValuesContainerDTO>();
                    }
                    advancedFiltersSummaryDTO.Value = lookupValueContainerDTOList.FirstOrDefault(x => x.LookupValue == attributeValue).Description;
                }
            }
            else
            {
                advancedFiltersSummaryDTO.Value = attributeValue;
            }
            if (AdvancedFiltersSummaryDTOCollection.Count == 0)
            {
                advancedFiltersSummaryDTO.AddLabelVisibility = Visibility.Hidden;
            }
            else
            {
                advancedFiltersSummaryDTO.AddLabelVisibility = Visibility.Visible;
            }
            
            AdvancedFiltersSummaryDTOCollection.Add(advancedFiltersSummaryDTO);
            
            IsLoadingVisible = false;
            log.LogMethodExit();
        }
        private void OnCancelledClicked()
        {
            log.LogMethodEntry();
            buttonClick = ButtonClickType.Cancel;
            if (advancedFilterView != null)
            {
                advancedFilterView.Close();
            }
            log.LogMethodExit();
        }
        private void SetInitialValues()
        {
            log.LogMethodEntry();
            AdvancedFiltersSummaryDTOCollection = new ObservableCollection<AdvancedFiltersSummaryDTO>();
            lookupsContainerDTOList = LookupsViewContainerList.GetLookupsContainerDTOList(ExecutionContext);
            Conditions = new ObservableCollection<string>();
            AttributeValueCBCollection = new ObservableCollection<string>();
            buttonClick = ButtonClickType.Cancel;
            SelectedAttributeValueType = AttributeValueType.Text;
            FetchOperatorsForText();
            FetchOperatorsForDateTime();
            FetchOperatorsForNumbers();
            log.LogMethodExit();
        }
        
        #endregion
        #region Constructor
        public AdvancedFilterVM(ExecutionContext executionContext, EnabledAttributesDTO.TableWithEnabledAttributes tableAttribute, string recordGuid=null)
        {
            log.LogMethodEntry(executionContext, tableAttribute, recordGuid);
            this.ExecutionContext = executionContext;
            SetInitialValues();
            tableWithEnabledAttributes = tableAttribute;
            this.recordGuid = recordGuid;
            dictionarySqlData = new Dictionary<TableAttributeDetailsDTO, List<KeyValuePair<string, string>>>();
            InitializeCommands();
            //GetTableAttributes();
            //GetConditions();
            GetTableAttributeDetailsDTOCollection();
            Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Advanced Filters for ") + EnabledAttributesDTO.TableWithEnabledAttributesToString(tableAttribute);
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
