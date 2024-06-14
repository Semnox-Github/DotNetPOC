/********************************************************************************************
 * Project Name - TableAttributeSetupUI  
 * Description  - TableAttributesUIHelper class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.140.0      13-Sep-2021      Fiona           Created 
 ********************************************************************************************/
//using ParafaitPOS;
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Languages;
using Semnox.Parafait.TableAttributeDetailsUtils;
using Semnox.Parafait.TableAttributeSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Interop;

namespace Semnox.Parafait.TableAttributeSetupUI
{
    /// <summary>
    /// TableAttributesUIHelper
    /// </summary>
    public static class TableAttributesUIHelper 
    {
        internal static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //public static object ParafaitPOS { get; private set; }

        public static TransactionPaymentsDTO GetEnabledAttributeDataForPaymentMode(ExecutionContext executionContext, TransactionPaymentsDTO transactionPaymentsDTO, bool canSkip = false, bool readOnly = false, object parentWindow = null)
        {
            log.LogMethodEntry(executionContext, transactionPaymentsDTO, canSkip, readOnly);
            bool inputReceived = false;
            int attemptCount = 0;

            List<TableAttributeDetailsDTO> tableAttributeDetailsDTOList = GetEnabledAttributes(executionContext, EnabledAttributesDTO.TableWithEnabledAttributes.PaymentMode,
                transactionPaymentsDTO.paymentModeDTO.Guid);
            if (tableAttributeDetailsDTOList != null && tableAttributeDetailsDTOList.Any())
            {
                string formTitle = MessageContainerList.GetMessage(executionContext, 4101, MessageContainerList.GetMessage(executionContext, "Payment"), transactionPaymentsDTO.paymentModeDTO.PaymentMode);
                //&1 attribute values for &2
                tableAttributeDetailsDTOList = SetAttributeValues(executionContext, transactionPaymentsDTO, tableAttributeDetailsDTOList);
                bool editMode = tableAttributeDetailsDTOList.Exists(tad => string.IsNullOrWhiteSpace(tad.AttributeValue) == false);

                while (inputReceived == false && attemptCount < 3)
                {
                    //App.machineUserContext = executionContext;
                    //App.EnsureApplicationResources();
                    TableAttributeFieldsView tableAttributeFieldsView = new TableAttributeFieldsView();
                    try
                    {
                        inputReceived = false;
                        TableAttributeFieldsVM tableAttributeFieldsVM = new TableAttributeFieldsVM(executionContext, tableAttributeDetailsDTOList, formTitle, readOnly);
                        tableAttributeFieldsView.DataContext = tableAttributeFieldsVM;
                        WindowInteropHelper helper = new WindowInteropHelper(tableAttributeFieldsView);
                        if (parentWindow != null)
                        {
                            if (parentWindow is IntPtr)
                            {
                                IntPtr owner = (IntPtr)parentWindow;
                                helper.Owner = owner;
                            }
                            else if (parentWindow is Window)
                            {
                                tableAttributeFieldsView.Owner = parentWindow as Window;
                            }
                        }
                        tableAttributeFieldsView.Closed += OnWindowClosed;
                        tableAttributeFieldsView.ShowDialog();
                        if (tableAttributeFieldsVM.ButtonClick == TableAttributeFieldsVM.ButtonClickType.Ok)
                        {
                            tableAttributeDetailsDTOList = tableAttributeFieldsVM.TableAttributeDetailsDTOList;
                            transactionPaymentsDTO = LoadObjectWithAttributeValues(executionContext, transactionPaymentsDTO, tableAttributeDetailsDTOList);
                            inputReceived = true;
                        }
                        else
                        {
                            if (editMode || canSkip) //allow to close in edit or skip mode
                            {
                                inputReceived = true;
                            }
                            else
                            {
                                attemptCount++;
                                GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
                                GenericMessagePopupVM messagePopupVM = new GenericMessagePopupVM(executionContext)
                                {
                                    Heading = MessageContainerList.GetMessage(executionContext, "Payment Mode Attributes"),
                                    Content = MessageContainerList.GetMessage(executionContext, 4100, EnabledAttributesDTO.TableWithEnabledAttributes.PaymentMode.ToString()),
                                    //'Please provide attribute values for the &1 record'
                                    //OkButtonText = MessageContainerList.GetMessage(executionContext, "Ok", null),
                                    CancelButtonText = MessageContainerList.GetMessage(executionContext, "Ok", null),
                                    MessageButtonsType = MessageButtonsType.OK
                                };
                                helper = new WindowInteropHelper(messagePopupView);
                                if (parentWindow != null)
                                {
                                    if (parentWindow is IntPtr)
                                    {
                                        IntPtr owner = (IntPtr)parentWindow;
                                        helper.Owner = owner;
                                    }
                                    else if (parentWindow is Window)
                                    {
                                        messagePopupView.Owner = parentWindow as Window;
                                    }
                                }
                                messagePopupView.DataContext = messagePopupVM;
                                messagePopupView.ShowDialog();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        //string msg = MessageContainerList.GetMessage(executionContext, 1824, ex.Message);
                        //POSUtils.ParafaitMessageBox(msg, MessageContainerList.GetMessage(executionContext, "Payment Mode Attributes"));
                        //MessageBox.Show(msg, MessageContainerList.GetMessage(executionContext, "Payment Mode Attributes"));

                        GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
                        GenericMessagePopupVM messagePopupVM = new GenericMessagePopupVM(executionContext)
                        {
                            Heading = MessageContainerList.GetMessage(executionContext, "Payment Mode Attributes"),
                            Content = MessageContainerList.GetMessage(executionContext, 1824, ex.Message),
                            //OkButtonText = MessageContainerList.GetMessage(executionContext, "Ok", null),
                            CancelButtonText = MessageContainerList.GetMessage(executionContext, "Ok", null),
                            MessageButtonsType = MessageButtonsType.OK
                        };
                        WindowInteropHelper helper = new WindowInteropHelper(messagePopupView);
                        if (parentWindow != null)
                        {
                            if (parentWindow is IntPtr)
                            {
                                IntPtr owner = (IntPtr)parentWindow;
                                helper.Owner = owner;
                            }
                            else if (parentWindow is Window)
                            {
                                messagePopupView.Owner = parentWindow as Window;
                            }
                        }

                        messagePopupView.DataContext = messagePopupVM;
                        messagePopupView.ShowDialog();
                        if (tableAttributeFieldsView != null)
                        {
                            tableAttributeFieldsView.Close();
                        }
                        inputReceived = false;
                        attemptCount++;
                    }
                }
                if (attemptCount >= 3 && inputReceived == false)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4100, EnabledAttributesDTO.TableWithEnabledAttributes.PaymentMode.ToString()));
                    //Please provide attribute values for the &1 record
                }
            }
            else
            {
                log.Info("No enabled attributes for Payment mode");
                inputReceived = true;
            }
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }
        public static List<TableAttributeDetailsDTO> GetEnabledAttributes(ExecutionContext executionContext, EnabledAttributesDTO.TableWithEnabledAttributes tableWithEnabledAttributes,
            string recordGuid)
        {
            log.LogMethodEntry(executionContext, recordGuid);
            AttributeEnabledTablesDTO.AttributeEnabledTableNames attributeEnabledTable = EnabledAttributesDTO.GetAttributeEnabledTable(tableWithEnabledAttributes);
            //EnabledAttributesDTO.TableWithEnabledAttributes tableWithEnabledAttributes = EnabledAttributesDTO.TableWithEnabledAttributes.PaymentMode;

            TableAttributeDetailsListBL tableAttributeDetailsListBL = new TableAttributeDetailsListBL(executionContext);
            List<TableAttributeDetailsDTO> tableAttributeDetailsDTOList = tableAttributeDetailsListBL.GetTableAttributeDetailsDTOList(tableWithEnabledAttributes, recordGuid);
            log.LogMethodExit(tableAttributeDetailsDTOList);
            return tableAttributeDetailsDTOList;
        }
        public static List<TableAttributeDetailsDTO> SetAttributeValues(ExecutionContext executionContext, TransactionPaymentsDTO trxPaymentDTO, List<TableAttributeDetailsDTO> tableAttributeDetailsDTOList)
        {
            log.LogMethodEntry(executionContext, trxPaymentDTO);
            if (tableAttributeDetailsDTOList != null && tableAttributeDetailsDTOList.Any())
            {
                for (int i = 0; i < tableAttributeDetailsDTOList.Count; i++)
                {
                    if (tableAttributeDetailsDTOList[i].EnabledAttributeName == "Attribute1")
                    {
                        tableAttributeDetailsDTOList[i].AttributeValue = trxPaymentDTO.Attribute1;
                    }
                    else if (tableAttributeDetailsDTOList[i].EnabledAttributeName == "Attribute2")
                    {
                        tableAttributeDetailsDTOList[i].AttributeValue = trxPaymentDTO.Attribute2;
                    }
                    else if (tableAttributeDetailsDTOList[i].EnabledAttributeName == "Attribute3")
                    {
                        tableAttributeDetailsDTOList[i].AttributeValue = trxPaymentDTO.Attribute3;
                    }
                    else if (tableAttributeDetailsDTOList[i].EnabledAttributeName == "Attribute4")
                    {
                        tableAttributeDetailsDTOList[i].AttributeValue = trxPaymentDTO.Attribute4;
                    }
                    else if (tableAttributeDetailsDTOList[i].EnabledAttributeName == "Attribute5")
                    {
                        tableAttributeDetailsDTOList[i].AttributeValue = trxPaymentDTO.Attribute5;
                    }
                }
            }
            log.LogMethodExit(tableAttributeDetailsDTOList);
            return tableAttributeDetailsDTOList;
        }

        public static TransactionPaymentsDTO LoadObjectWithAttributeValues(ExecutionContext executionContext, TransactionPaymentsDTO trxPaymentDTO, List<TableAttributeDetailsDTO> tableAttributeDetailsDTOList)
        {
            log.LogMethodEntry(executionContext, trxPaymentDTO);
            if (tableAttributeDetailsDTOList != null && tableAttributeDetailsDTOList.Any())
            {
                for (int i = 0; i < tableAttributeDetailsDTOList.Count; i++)
                {
                    if (tableAttributeDetailsDTOList[i].EnabledAttributeName == "Attribute1")
                    {
                        trxPaymentDTO.Attribute1 = tableAttributeDetailsDTOList[i].AttributeValue;
                    }
                    else if (tableAttributeDetailsDTOList[i].EnabledAttributeName == "Attribute2")
                    {
                        trxPaymentDTO.Attribute2 = tableAttributeDetailsDTOList[i].AttributeValue;
                    }
                    else if (tableAttributeDetailsDTOList[i].EnabledAttributeName == "Attribute3")
                    {
                        trxPaymentDTO.Attribute3 = tableAttributeDetailsDTOList[i].AttributeValue;
                    }
                    else if (tableAttributeDetailsDTOList[i].EnabledAttributeName == "Attribute4")
                    {
                        trxPaymentDTO.Attribute4 = tableAttributeDetailsDTOList[i].AttributeValue;
                    }
                    else if (tableAttributeDetailsDTOList[i].EnabledAttributeName == "Attribute5")
                    {
                        trxPaymentDTO.Attribute5 = tableAttributeDetailsDTOList[i].AttributeValue;
                    }
                }
            }
            log.LogMethodExit(trxPaymentDTO);
            return trxPaymentDTO;
        }

        private static void OnWindowClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            TableAttributeFieldsView tableAttributeFieldsView = sender as TableAttributeFieldsView;
            TableAttributeFieldsVM tableAttributeFieldsVM = tableAttributeFieldsView.DataContext as TableAttributeFieldsVM;
            List<TableAttributeDetailsDTO> result = null;
            if (tableAttributeFieldsVM.ButtonClick == TableAttributeFieldsVM.ButtonClickType.Ok)
            {
                result = tableAttributeFieldsVM.TableAttributeDetailsDTOList;
            }
            log.LogMethodExit();
        }
         

    }
}
