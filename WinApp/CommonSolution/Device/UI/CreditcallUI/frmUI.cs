using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
//using System.Drawing.Printing;
using Creditcall.ChipDna;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Globalization;

namespace Semnox.Parafait.Device.PaymentGateway.CreditcallUI
{
    public partial class frmUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// ClientHelper chipDnaClientHelper
        /// </summary>
        public ClientHelper chipDnaClientHelper;
        /// <summary>
        /// String variables amount, reference, batchReference,salesReference, languageCode, transactionType
        /// </summary>
        public string amount, reference, batchReference,salesReference, languageCode, transactionType;
        /// <summary>
        /// string status
        /// </summary>
        public static string status;
        /// <summary>
        /// bool retValTrans
        /// </summary>
        public bool retValTrans = false;
        private string signatureRequired = "False";
        string receipt = "";

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="_utilities"></param>
        /// <param name="amt"></param>
        /// <param name="refNo"></param>
        /// <param name="transType"></param>
        /// <param name="batchRef"></param>
        /// <param name="salesRef"></param>
        /// <param name="langCode"></param>
        public frmUI(Utilities _utilities,string amt, int refNo, string transType, string batchRef,string salesRef="", string langCode = "eng")
        {
            log.LogMethodEntry(_utilities, amt, refNo, transType, batchRef, salesRef, langCode);
            InitializeComponent();            
            chipDnaClientHelper = null;
            try
            {
                string terminalId = _utilities.getParafaitDefaults("CREDITCALL_TERMINAL_ID");
                string serverAddress = _utilities.getParafaitDefaults("CREDITCALL_SERVER_ADDRESS");
                string serverPort = _utilities.getParafaitDefaults("CREDITCALL_SERVER_PORT");
                if (terminalId != "" && serverAddress != "" && serverPort != "")
                {
                    chipDnaClientHelper = new ClientHelper(terminalId, serverAddress, Convert.ToInt32(serverPort));
                }
                else
                {
                    CreditcallGateway.returnedMessage = "Server parameters are not available.Creditcall initialization error !";
                    log.LogMethodExit(null, "Server parameters are not available.Creditcall initialization error !");
                    return;
                }            
            }
            catch(Exception ex)
            {
                log.Error("Error occured while retrieving configuration parameters", ex);
                CreditcallGateway.returnedMessage = "Error while retrieving server parameters";
                log.LogMethodExit(null);
                return;
            }            
            InitializeCreditcallEFTPOSEnv();
            amount = amt;
            reference = refNo.ToString();
            transactionType = transType;
            languageCode = langCode;
            batchReference = batchRef;
            salesReference = salesRef;
        }

        void InitializeCreditcallEFTPOSEnv()
        {
            log.LogMethodEntry();
            chipDnaClientHelper.TransactionUpdateEvent += ChipDnaClientLibOnTransactionUpdate;
            chipDnaClientHelper.TransactionFinishedEvent += ChipDnaClientLibOnTransactionFinished;
            chipDnaClientHelper.CardNotificationEvent += ChipDnaClientLibOnCardNotification;
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Makes Payment
        /// </summary>
        /// <returns></returns>
        public bool PerformSaleTransaction()
        {
            log.LogMethodEntry();
            clearValues();
            var parameters = new ParameterSet();
            if (!string.IsNullOrEmpty(amount))
            {
                parameters.Add(ParameterKeys.Amount, amount);
                log.LogVariableState("amount", amount);
            }

            if (!string.IsNullOrEmpty(reference))
            {
                parameters.Add(ParameterKeys.Reference, reference);
                log.LogVariableState("reference", reference);
            }

            parameters.Add(ParameterKeys.TransactionType, transactionType);
            log.LogVariableState("transactionType", transactionType);

            if (!String.IsNullOrEmpty(languageCode))
            {
                //parameters.Add(ParameterKeys.TerminalLanguage, languageCode);
                log.LogVariableState("languageCode", languageCode);
            }

            if (!string.IsNullOrEmpty(batchReference))
            {
                parameters.Add(ParameterKeys.BatchReference, batchReference);
                log.LogVariableState("batchReference", batchReference);
            }

            var response = chipDnaClientHelper.StartTransaction(parameters);
            string errors;
            if (response.GetValue(ParameterKeys.Errors, out errors))
            {
                CreditcallGateway.returnedMessage = errors.ToString();
                this.txtStatus.Text = errors.ToString();
                btnCancel.Enabled = true;
                retValTrans = false;
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            else
            {
                retValTrans = true;                
            }
            log.LogMethodExit(retValTrans);
            return retValTrans;
        }

        /// <summary>
        /// Reverts Payment
        /// </summary>
        /// <returns></returns>
        public bool PerformRefundTransaction()
        {
            log.LogMethodEntry();
            var parameters = new ParameterSet();
            if (!string.IsNullOrEmpty(amount))
            {
                parameters.Add(ParameterKeys.Amount, amount);
                log.LogVariableState("amount", amount);
            }

            if (!string.IsNullOrEmpty(reference))
            {
                parameters.Add(ParameterKeys.Reference, reference);
                log.LogVariableState("reference", reference);
            }

            parameters.Add(ParameterKeys.TransactionType, transactionType);
            log.LogVariableState("transactionType", transactionType);

            if (!String.IsNullOrEmpty(languageCode))
            {
                //parameters.Add(ParameterKeys.TerminalLanguage, languageCode);
                log.LogVariableState("languageCode", languageCode);
            }

            if (!string.IsNullOrEmpty(batchReference))
            {
                parameters.Add(ParameterKeys.BatchReference, batchReference);
                log.LogVariableState("batchReference", batchReference);
            }

            var response = chipDnaClientHelper.StartTransaction(parameters);
            string errors;
            if (response.GetValue(ParameterKeys.Errors, out errors))
            {
                CreditcallGateway.returnedMessage = errors.ToString();
                this.txtStatus.Text = errors.ToString();
                btnCancel.Enabled = true;
                retValTrans = false;
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            else
            {
                retValTrans = true;
            }
            log.LogMethodExit(retValTrans);
            return retValTrans;
        }

        /// <summary>
        /// Perform Linked Refund Transaction 
        /// </summary>
        /// <returns></returns>
        public bool PerformLinkedRefundTransaction()
        {
            log.LogMethodEntry();
            clearValues();
            if (reference == salesReference)
            {
                reference = "R_" + reference; //Prefix reference with R, since creditcall requires a new value
                //for reference. The sale reference and reference can't be same. 
            }            
            var parameters = new ParameterSet();
            if (!string.IsNullOrEmpty(amount))
            {
                parameters.Add(ParameterKeys.Amount, amount);
                log.LogVariableState("amount", amount);
            }

            if (!string.IsNullOrEmpty(reference))
            {
                parameters.Add(ParameterKeys.Reference, reference);
                log.LogVariableState("reference", reference);
            }

            if (!string.IsNullOrEmpty(salesReference))
            {
                parameters.Add(ParameterKeys.SaleReference, salesReference);
                log.LogVariableState("salesReference", salesReference);
            }
            var response = chipDnaClientHelper.LinkedRefundTransaction(parameters);
            string errors, result;
            if (response.GetValue(ParameterKeys.Errors, out errors))
            {
                CreditcallGateway.returnedMessage = errors.ToString();
                retValTrans = false;
                this.txtStatus.Text = errors.ToString();
                btnCancel.Enabled=true;
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }

            else
                if (response.GetValue(ParameterKeys.TransactionResult, out result))
                {
                    if (result != null && result.Equals("APPROVED", StringComparison.InvariantCultureIgnoreCase))
                    {
                        CreditcallGateway.trxStatus = result.ToUpper();
                        SetText(result.ToUpper());
                        retValTrans = true;
                    if (response.GetValue(ParameterKeys.ReceiptData, out receipt))
                    {
                        PrintReceiptData(ReceiptData.GetReceiptDataFromXml(receipt), "Customer Receipt");
                        CreditcallGateway.customerCopy = CreditcallGateway.receiptText;
                        if (CreditcallGateway.unAttendedCC == false)
                        {
                            PrintReceiptData(ReceiptData.GetReceiptDataFromXml(receipt), "Merchant Receipt");
                            CreditcallGateway.merchantCopy = CreditcallGateway.receiptText;
                        }
                    }
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                    else
                    {
                        retValTrans = false;
                        SetText(result.ToString());
                        CreditcallGateway.returnedMessage = "Transaction Declined";
                        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    }
                }
            log.LogMethodExit(retValTrans);
           return retValTrans;
        }

        private void clearValues()
        {
            log.LogMethodEntry();
            CreditcallGateway.trxStatus = CreditcallGateway.authCode = CreditcallGateway.returnedMessage = CreditcallGateway.receiptText = null;
            log.LogMethodExit(null);
        }

        private void PrintReceiptData(ReceiptData receiptData, string type)
        {
            log.LogMethodEntry(receiptData, type);
            if (receiptData == null)
            {
                log.LogMethodExit(null);
                return;
            }
            var receiptDataBuilder = new StringBuilder();
            receiptDataBuilder.Append("Receipt Data:");
            var receiptDataForPrinting = new StringBuilder();

            var entries = new List<ReceiptData.ReceiptEntry>(receiptData.ReceiptEntries);
            entries.Sort((entry1, entry2) =>
            {
                if (entry1.Priority > entry2.Priority)
                {
                    log.LogMethodExit(1);
                    return 1;
                }

                if (entry1.Priority < entry2.Priority)
                {
                    log.LogMethodExit(-1);
                    return -1;
                }
                log.LogMethodExit(0);
                return 0;
            });

            receiptDataForPrinting.AppendLine(type);
            receiptDataForPrinting.AppendLine("");

            foreach (var receiptEntry in entries)
            {
                receiptDataBuilder.AppendFormat(null,
                    "\t{0}=> {1}{2}\r\n",
                    string.Format("[{0} - {1}]", receiptEntry.ReceiptEntryId, receiptEntry.ReceiptItemType).PadRight(40, '-'),
                    string.IsNullOrEmpty(receiptEntry.Label) ? "" : String.Format("{0}: ", receiptEntry.Label),
                    receiptEntry.Value);

                receiptDataForPrinting.AppendFormat(null, "{0}{1}",
                    string.IsNullOrEmpty(receiptEntry.Label) ? "" : String.Format("{0}: ", receiptEntry.Label), receiptEntry.Value);
                receiptDataForPrinting.AppendLine("");//added on 5/5/2015 -to print the values one below the other.

                if (receiptEntry.ReceiptEntryId.Equals("SignatureLineLabel", StringComparison.InvariantCultureIgnoreCase))
                {
                    receiptDataForPrinting.AppendFormat(null, "{0}{0}{1}{0}", "\r\n", string.Empty.PadRight(30, '_'));
                }
            }

            CreditcallGateway.receiptText = receiptDataForPrinting.ToString();
            //PrintReceipt(CreditcallGateway.receiptText);
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Print receipt Class
        /// </summary>
        /// <param name="ReceiptText"></param>
        public void PrintReceipt(string ReceiptText)
        {
            log.LogMethodEntry(ReceiptText);
            if (string.IsNullOrEmpty(ReceiptText))
            {
                log.LogMethodExit(null);
                return;
            }
              

            try
            {
                System.Drawing.Printing.PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();
                
                printDocument.PrintPage += (sender, args) =>
                {
                    args.Graphics.DrawString(ReceiptText, new System.Drawing.Font("Arial", 9), System.Drawing.Brushes.Black, 0, 0);
                };
                printDocument.Print();
            }
            catch (Exception ex)
            {
                log.Error("Error occured while printing the document", ex);
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            log.LogMethodExit(null);
        }


        private void ChipDnaClientLibOnTransactionFinished(object sender, EventParameters transactionFinishedEventParameters)
        {
            log.LogMethodEntry(sender, transactionFinishedEventParameters);
            // Transaction Finished
            string result;           
            // Find result in parameter list
            transactionFinishedEventParameters.GetValue(ParameterKeys.TransactionResult, out result);

            // If Approved transaction should be either confirmed or voided
            if (result != null && result.Equals("APPROVED", StringComparison.InvariantCultureIgnoreCase))
            {
                CreditcallGateway.trxStatus = result;                
                var parameters = new ParameterSet();
                parameters.Add(ParameterKeys.Reference, reference);
                log.LogVariableState("reference", reference);
                transactionFinishedEventParameters.ToList().ForEach(param =>
                {
                    if (param.Key.Equals(ParameterKeys.SignatureVerificationRequired))
                    {
                        signatureRequired = param.Value;
                    }
                });

                if(CreditcallGateway.unAttendedCC==false)
                {
                    if (signatureRequired == "True")
                    {
                        DialogResult dr = MessageBox.Show("Signature verified?", "Signature", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            //if signature is verified...
                            //just proceed to confirm the transaction and print the receipts....
                            var response = chipDnaClientHelper.ConfirmTransaction(parameters);
                            string errors;
                            if (response.GetValue(ParameterKeys.Errors, out errors))
                            {
                                CreditcallGateway.returnedMessage = errors;
                                SetText("Error during confirmation..");
                                PrintReceipt(GetDeclinedReceipt(transactionFinishedEventParameters.ToString()));
                                if (CreditcallGateway.unAttendedCC == false)
                                {
                                    PrintReceipt(GetDeclinedReceipt(transactionFinishedEventParameters.ToString()).Replace("Customer Receipt", "Merchant Receipt"));
                                }
                                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                            }

                            else
                            {
                                CreditcallGateway.returnedMessage = "Transaction Confirmed";
                                if (response.GetValue(ParameterKeys.ReceiptData, out receipt))
                                {
                                    PrintReceiptData(ReceiptData.GetReceiptDataFromXml(receipt), "Customer Receipt");
                                    CreditcallGateway.customerCopy = CreditcallGateway.receiptText;
                                    if (CreditcallGateway.unAttendedCC == false)
                                    {
                                        PrintReceiptData(ReceiptData.GetReceiptDataFromXml(receipt), "Merchant Receipt");
                                        CreditcallGateway.merchantCopy = CreditcallGateway.receiptText;
                                    }
                                }

                                CreditcallGateway.referenceNo = reference;
                                transactionFinishedEventParameters.ToList().ForEach(param =>
                                {
                                    if (param.Key.Equals(ParameterKeys.AuthCode))
                                    {
                                        CreditcallGateway.authCode = param.Value;
                                    }
                                });

                                SetText(result.ToString());
                                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                            }

                        }
                        else
                        {
                            //if signature is not proper...
                            //void the transaction..
                            var response = chipDnaClientHelper.VoidTransaction(parameters);
                            string errors;
                            if (response.GetValue(ParameterKeys.Errors, out errors))
                            {
                                CreditcallGateway.returnedMessage = errors;
                                SetText("Error during voiding..");
                                PrintReceipt(GetDeclinedReceipt(transactionFinishedEventParameters.ToString()));
                                if (CreditcallGateway.unAttendedCC == false)
                                {
                                    PrintReceipt(GetDeclinedReceipt(transactionFinishedEventParameters.ToString()).Replace("Customer Receipt", "Merchant Receipt"));
                                }
                                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                            }
                            else
                            {
                                SetText("Transaction voided..");
                                CreditcallGateway.returnedMessage = "Transaction Voided";
                                PrintReceipt(GetDeclinedReceipt(transactionFinishedEventParameters.ToString()));
                                if (CreditcallGateway.unAttendedCC == false)
                                {
                                    PrintReceipt(GetDeclinedReceipt(transactionFinishedEventParameters.ToString()).Replace("Customer Receipt", "Merchant Receipt"));
                                }
                                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                            }
                        }

                    }
                    else
                    {
                        //if signature verification is not at all required...
                        //just proceed to confirm the transaction and print the receipts....
                        var response = chipDnaClientHelper.ConfirmTransaction(parameters);
                        string errors;
                        if (response.GetValue(ParameterKeys.Errors, out errors))
                        {
                            CreditcallGateway.returnedMessage = errors;
                            SetText("Error during confirmation..");
                            PrintReceipt(GetDeclinedReceipt(transactionFinishedEventParameters.ToString()));
                            if (CreditcallGateway.unAttendedCC == false)
                            {
                                PrintReceipt(GetDeclinedReceipt(transactionFinishedEventParameters.ToString()).Replace("Customer Receipt", "Merchant Receipt"));
                            }
                            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                        }

                        else
                        {
                            CreditcallGateway.returnedMessage = "Transaction Confirmed";
                            if (response.GetValue(ParameterKeys.ReceiptData, out receipt))
                            {
                                PrintReceiptData(ReceiptData.GetReceiptDataFromXml(receipt), "Customer Receipt");
                                CreditcallGateway.customerCopy = CreditcallGateway.receiptText;
                                if (CreditcallGateway.unAttendedCC == false)
                                {
                                    PrintReceiptData(ReceiptData.GetReceiptDataFromXml(receipt), "Merchant Receipt");
                                    CreditcallGateway.merchantCopy = CreditcallGateway.receiptText;
                                }
                            }

                            CreditcallGateway.referenceNo = reference;
                            transactionFinishedEventParameters.ToList().ForEach(param =>
                            {
                                if (param.Key.Equals(ParameterKeys.AuthCode))
                                {
                                    CreditcallGateway.authCode = param.Value;
                                }
                            });

                            SetText(result.ToString());
                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        }
                    } 
                }

                else
                {
                    //-un attended pos(kiosk)-no signature verification required and need not check for signature. Directly confirm transaction
                    var response = chipDnaClientHelper.ConfirmTransaction(parameters);
                    string errors;
                    if (response.GetValue(ParameterKeys.Errors, out errors))
                    {
                        CreditcallGateway.returnedMessage = errors;
                        SetText("Error during confirmation..");
                        PrintReceipt(GetDeclinedReceipt(transactionFinishedEventParameters.ToString()));
                        if (CreditcallGateway.unAttendedCC == false)
                        {
                            PrintReceipt(GetDeclinedReceipt(transactionFinishedEventParameters.ToString()).Replace("Customer Receipt", "Merchant Receipt"));
                        }
                        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    }

                    else
                    {
                        CreditcallGateway.returnedMessage = "Transaction Confirmed";
                        if (response.GetValue(ParameterKeys.ReceiptData, out receipt))
                        {
                            PrintReceiptData(ReceiptData.GetReceiptDataFromXml(receipt), "Customer Receipt");
                            CreditcallGateway.customerCopy = CreditcallGateway.receiptText;
                            if (CreditcallGateway.unAttendedCC==false)
                            {
                                PrintReceiptData(ReceiptData.GetReceiptDataFromXml(receipt), "Merchant Receipt");
                                CreditcallGateway.merchantCopy = CreditcallGateway.receiptText;
                            }                            
                        }

                        CreditcallGateway.referenceNo = reference;
                        transactionFinishedEventParameters.ToList().ForEach(param =>
                        {
                            if (param.Key.Equals(ParameterKeys.AuthCode))
                            {
                                CreditcallGateway.authCode = param.Value;
                            }
                        });

                        SetText(result.ToString());
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                }
                              
            }
            else
            {
                //--if status is not "Approved" (declined)
                CreditcallGateway.returnedMessage = "Transaction Declined";
                SetText(result.ToString() == null ? "Transaction Declined" : result.ToString());
                PrintReceipt(GetDeclinedReceipt(transactionFinishedEventParameters.ToString()));
                if (CreditcallGateway.unAttendedCC == false)
                {
                    PrintReceipt(GetDeclinedReceipt(transactionFinishedEventParameters.ToString()).Replace("Customer Receipt", "Merchant Receipt"));
                }
                //btnCancel.Enabled = true;
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            log.LogMethodExit(null);                   
        }
      private string GetDeclinedReceipt(string data)
        {
            string receiptText = "";
            log.LogMethodEntry();
            try
            {                
                if (!string.IsNullOrEmpty(data))
                {
                    string[] spliter = new string[] { "] " };
                    string[] dataarray = data.Substring(0, data.Length - 1).Replace("[", "").Split(spliter, StringSplitOptions.None);
                    if (dataarray != null && dataarray.Length > 0)
                    {
                        int index = -1;
                        //List<Dictionary<string, string>> dataDictionary = new List<Dictionary<string, string>>();
                        Dictionary<string, string> dataDictionary = new Dictionary<string, string>();
                        for (int i = 0; i < dataarray.Length; i++)
                        {
                            index = dataarray[i].IndexOf(" ");
                            //dataarray[i].ToCharArray()[index] = ',';
                            dataDictionary.Add(dataarray[i].Substring(0, index), dataarray[i].Substring(index + 1));
                        }
                        if (dataDictionary.Count > 1)
                        {
                            receiptText = "Customer Receipt" + Environment.NewLine;
                            receiptText += dataDictionary["MERCHANT_NAME"] + Environment.NewLine;
                            receiptText += dataDictionary["CARD_SCHEME"] + Environment.NewLine;
                            receiptText += "Card: " + (dataDictionary["PAN_MASKED"]).Substring(dataDictionary["PAN_MASKED"].Length - 4).PadLeft(dataDictionary["PAN_MASKED"].Length, '*') + Environment.NewLine;
                            receiptText += dataDictionary["TRANSACTION_SOURCE"] + Environment.NewLine;
                            receiptText += dataDictionary["TRANSACTION_TYPE"] + Environment.NewLine;
                            receiptText += "TOTAL:" + Convert.ToDouble(dataDictionary["TOTAL_AMOUNT"]) / 100.00 + Environment.NewLine;
                            receiptText += dataDictionary["CARDHOLDER_VERIFICATION"] + Environment.NewLine;
                            receiptText += dataDictionary["TRANSACTION_RESULT"] + Environment.NewLine;
                            receiptText += "Auth Code:" + Environment.NewLine;
                            receiptText += "Ref: " + dataDictionary["REFERENCE"] + Environment.NewLine;
                            receiptText += "MID: " + dataDictionary["MERCHANT_ID_MASKED"] + Environment.NewLine;
                            receiptText += "TID: " + dataDictionary["TERMINAL_ID_MASKED"] + Environment.NewLine;
                            receiptText += DateTime.ParseExact(dataDictionary["AUTH_DATE_TIME"], "yyyyMMddHHmmss", CultureInfo.InvariantCulture) + Environment.NewLine;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            log.LogMethodExit();
            return receiptText;
        }

        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            log.LogMethodEntry(text);
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            try
            {
                if (this.Visible == true)
                {
                    if (this.txtStatus.InvokeRequired)
                    {
                        SetTextCallback d = new SetTextCallback(SetText);
                        this.Invoke(d, new object[] { text });
                    }
                    else
                    {
                        this.txtStatus.Text = text;
                    }
                    btnCancel.Focus();
                    btnCancel.Select();
                }
            }
            catch(Exception ex)
            {
                log.Error("Error occured while invoking", ex);
            }
            log.LogMethodExit(null);            
        }

        /// <summary>
        /// ChipDna Client LibOn Transaction Update Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="transactionUpdateEventParameters"></param>
        public void ChipDnaClientLibOnTransactionUpdate(object sender, EventParameters transactionUpdateEventParameters)
        {
            log.LogMethodEntry(sender, transactionUpdateEventParameters);
            transactionUpdateEventParameters.ToList().ForEach(param =>
            {
                if (param.Key.Equals(ParameterKeys.Update))
                {
                    status = param.Value;
                    SetText(status);
                }               
            });
            log.LogMethodExit(null);
        }

        private void ChipDnaClientLibOnCardNotification(object sender, EventParameters cardNotificationEventArgs)
        {
            log.LogMethodEntry(sender, cardNotificationEventArgs);
            //-If values of card statuses have to be retrieved use the following values returned from the event
            //cardNotificationEventArgs.ToList().ForEach(param =>
            //{
            //    if (ParameterKeys.Notification.Equals(param.Key) && param.Value.Equals("Inserted"))
            //    {
            //        status = "Inserted";
            //    }
            //    else if (ParameterKeys.Notification.Equals(param.Key) && param.Value.Equals("Tapped"))
            //    {
            //        status = "Tapped";
            //    }
            //    else if (ParameterKeys.Notification.Equals(param.Key) && param.Value.Equals("Swiped"))
            //    {
            //        status = "Swiped";
            //    }
            //    else if (ParameterKeys.Notification.Equals(param.Key) && param.Value.Equals("Removed"))
            //    {
            //        status = "Removed";
            //    }
            //});
            log.LogMethodExit(null);
        }
     
        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
            log.LogMethodExit(null);
        }

        private void frmUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnCancel.Enabled = false;
            if (CreditcallGateway.unAttendedCC == true)
            {
                this.TopMost = true; //-for kiosk only make this top. For pos if signature verification is there,let that dialog box be on top
            }
            
            try
            {
                if (transactionType == "sale")
                {
                    PerformSaleTransaction();
                }
                else if(transactionType=="refund")
                {
                    PerformRefundTransaction();
                }
                else if(transactionType=="linkedRefund")
                {
                    PerformLinkedRefundTransaction();                    
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while processing - " + transactionType, ex);
                MessageBox.Show(ex.Message);
                btnCancel.Enabled = true;
                btnCancel.Visible = true;
            }
            log.LogMethodExit(null);
        }
    }
}
