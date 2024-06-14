//using Semnox.Parafait.PaymentGateway;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Semnox.Parafait.TransactionPayments;
using System.Threading.Tasks;
 using Semnox.Core.Utilities;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Semnox.Parafait.Device.PaymentGateway
{
    class QuestPaymentGateway : PaymentGateway
    {
        QuestEFTPOS quest;
        private bool purchaseOrRefund = false;
        private bool Status = false;
        private bool initialized = false;
        CCRequestPGWDTO cCRequestPGWDTO;

        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="isUnattended"></param>
        /// <param name="showMessageDelegate"></param>
        /// <param name="writeToLogDelegate"></param>
        public QuestPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate , WriteToLogDelegate writeToLogDelegate) : base(utilities, isUnattended, showMessageDelegate,writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            quest = new QuestEFTPOS(utilities);
            QuestEFTPOS.unattended = isUnattended;
            log.LogMethodExit(null);
        }

        public override void Initialize()
        {
            log.LogMethodEntry();
            if(initialized == false)
            {
                initialized = true;
                SetupPaymentQuestEFTPOS();
            }
            log.LogMethodExit(null);
        }

        private void SetupPaymentQuestEFTPOS()
        {
            log.LogMethodEntry();
            try
            {
                quest.StatusCheck();
                QuestWaitForEvent();
                if (!quest.errorMessage.Equals("OK"))
                {
                    showMessageDelegate("Quest Pinpad Error :" + quest.errorMessage + ".Quest Last Transactions cannot be performed.Please check Pinpad connection and restart the application.", "Quest", MessageBoxButtons.OK);
                    log.LogMethodExit(null);
                    return;
                }
                CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchTransactionParameter = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                searchTransactionParameter.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.MERCHANT_ID, utilities.ParafaitEnv.POSMachine));
                cCRequestPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchTransactionParameter);

                if (cCRequestPGWDTO != null && cCRequestPGWDTO.InvoiceNo != null)
                {
                    long status;
                    status = quest.LastTransactionStatus(cCRequestPGWDTO.InvoiceNo.ToString());
                    QuestWaitForEvent(true);//It should wait for 5 minutes if event is not fired.So true is passed.
                    LastQuestTrx_Status();
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to fetch the Last Quest Transaction", ex);
                throw new PaymentGatewayException(ex.ToString());
            }

            log.LogMethodExit(null);
        }

        private void LastQuestTrx_Status()
        {
            log.LogMethodEntry();

            string message = "";
            switch (quest.nResult)
            {
                case 0:
                    if (quest.nResult == 0)
                    {
                        TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL();
                        List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> searchTransactionParameter = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                        searchTransactionParameter.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, cCRequestPGWDTO.InvoiceNo));
                        List<TransactionPaymentsDTO> transactionPaymentsDTOList = transactionPaymentsListBL.GetTransactionPaymentsDTOList(searchTransactionParameter);

                        if (transactionPaymentsDTOList == null)
                        {
                            message = "Last Quest payment for Trx Id: " + cCRequestPGWDTO.InvoiceNo + " was Approved but not applied. This payment will be voided.";

                            bool returnValue;
                            int transactionAmount = quest.amount;
                            while (transactionAmount != 0)
                            {
                                while (transactionAmount > 0)
                                {
                                    returnValue = quest.PerformRefund(cCRequestPGWDTO.InvoiceNo.ToString(), utilities.ParafaitEnv.POSMachine, transactionAmount);
                                    if (returnValue)
                                    {
                                        QuestWaitForEvent();
                                        if (quest.nResult == 0)
                                        {
                                            quest.PrintReceipt(quest.printText);
                                            quest.PrintReceipt(quest.printText.Replace("CUSTOMER COPY", "MERCHANT COPY"));
                                            message = quest.errorMessage;
                                            transactionAmount -= quest.amount;
                                        }
                                    }
                                    else
                                    {
                                        message = "Error in refund... swipe your card again..";
                                    }
                                }
                                while (transactionAmount < 0)
                                {
                                    returnValue = quest.PerformSale(cCRequestPGWDTO.InvoiceNo.ToString(), utilities.ParafaitEnv.POSMachine, transactionAmount * -1);
                                    if (returnValue)
                                    {
                                        QuestWaitForEvent();
                                        if (quest.nResult == 0)
                                        {
                                            quest.PrintReceipt(quest.printText);
                                            quest.PrintReceipt(quest.printText.Replace("CUSTOMER COPY", "MERCHANT COPY"));
                                            message = quest.errorMessage;
                                            transactionAmount = (transactionAmount - quest.amount) * -1;
                                        }
                                    }
                                    else
                                    {
                                        message = "Error in refund... swipe your card again...";
                                    }
                                }
                            }
                        }
                        else
                        {
                            message = "Last Quest payment for Trx Id: " + cCRequestPGWDTO.InvoiceNo + " was Approved and applied.";
                        }
                    }
                    break;
                case 1:
                    message = "Last Transaction with transaction Id:" + cCRequestPGWDTO.InvoiceNo + "";
                    break;
                case 2:
                    message = "The EFT transaction  with transaction Id:" + cCRequestPGWDTO.InvoiceNo + "  has been declined by the host.";
                    break;
                case 3:
                    message = "The EFT transaction  with transaction Id:" + cCRequestPGWDTO.InvoiceNo + "  has failed.";
                    break;
                case 4:
                    message = "The EFT transaction  with transaction Id:" + cCRequestPGWDTO.InvoiceNo + " was successful – the transaction was approved offline from the host. Not implemented. ";
                    break;
                case 5:
                    message = "Unable to determine the status of the last EFTPOS transaction for transaction Id:" + cCRequestPGWDTO.InvoiceNo + "";
                    break;
                default:
                    message = "Error in tracking last Transaction status for transaction Id:" + cCRequestPGWDTO.InvoiceNo + ".";
                    break;
            }

            if (!string.IsNullOrWhiteSpace(message) && writeToLogDelegate != null)
            {
                writeToLogDelegate(Convert.ToInt32(cCRequestPGWDTO.InvoiceNo), "Last Transaction",Convert.ToInt32(cCRequestPGWDTO.InvoiceNo),(quest.amount / 100), message, utilities.ParafaitEnv.POSMachineId, utilities.ParafaitEnv.POSMachine);
            }
            else if (!string.IsNullOrWhiteSpace(message)  && showMessageDelegate != null)
            {
                    showMessageDelegate(message, "Quest", MessageBoxButtons.OK);
            }
            log.LogMethodExit(null);
        }

        public void QuestWaitForEvent(bool lastTran = false)//used in pos ccGatewayUtils //Quest:starts
        {
            log.LogMethodEntry(lastTran);

            //this method is used to wait for the quest method response..
            Form frm = new Form();
            int counter = 0;
            int cancelDisabler = 0;//used to disable the cancel button after enter key press.
            frm.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            frm.Load += (sen, arg) =>
            {
                Label lblMsg = new Label();
                Button btnCancel = new Button();
                lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblMsg.AutoSize = true;
                lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                frm.Controls.Add(lblMsg);
                if (purchaseOrRefund && QuestEFTPOS.unattended)
                {
                    frm.Text = "Quest Gateway";
                    frm.ClientSize = new System.Drawing.Size(508, 130);
                    frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
                    lblMsg.AutoSize = false;
                    lblMsg.Location = new System.Drawing.Point(-1, 34);
                    lblMsg.Size = new System.Drawing.Size(511, 29);
                    lblMsg.BackColor = System.Drawing.Color.White;
                    lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    btnCancel.Size = new System.Drawing.Size(160, 50);
                    btnCancel.Location = new System.Drawing.Point(184, 82);
                    btnCancel.BackColor = System.Drawing.Color.DimGray;
                    btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                    btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                    btnCancel.Text = "Cancel";
                    btnCancel.AutoSize = true;
                    btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left))));
                    btnCancel.Click += (send, argu) =>
                    {
                        quest.CancelTransaction();
                    };
                    frm.Controls.Add(btnCancel);
                }
                else
                {
                    frm.Size = new System.Drawing.Size(300, 60);
                }
                if (lastTran)
                {
                    lblMsg.Text = "Last trasaction status Check...";//is displaying 
                    lblMsg.Refresh();
                }
                else if (string.IsNullOrEmpty(quest.statusMessage) && QuestEFTPOS.unattended)
                {
                    lblMsg.Text = "Pinpad status Check...";//is displaying       
                    lblMsg.Refresh();
                }
                else
                {
                    lblMsg.Text = "Quest Processing...";//is displaying       
                    lblMsg.Refresh();
                }
                frm.ControlBox = false;
                System.Windows.Forms.Timer tmr = new System.Windows.Forms.Timer();
                tmr.Interval = 1000;
                tmr.Tick += (s, a) =>
                {
                    if (!string.IsNullOrEmpty(quest.statusMessage) && QuestEFTPOS.unattended)
                    {
                        lblMsg.Text = quest.statusMessage.Replace("\n", " ").Trim();
                        for (int i = 0; i < 5; i++)
                        {
                            lblMsg.Text = lblMsg.Text.Replace("  ", " ");
                        }
                        if (cancelDisabler == 1 && !lblMsg.Text.Contains("PIN"))
                        {
                            btnCancel.Visible = false;
                        }
                        if (lblMsg.Text.Contains("PIN") && cancelDisabler != 1)
                        {
                            cancelDisabler++;
                        }
                        lblMsg.Refresh();
                    }
                    if (string.IsNullOrEmpty(quest.errorMessage) == false)
                    {
                        lblMsg.Text = quest.errorMessage;
                        quest.statusMessage = string.Empty;
                        lblMsg.Refresh();
                        tmr.Stop();
                        (sen as Form).Close();
                    }
                    if (lastTran)// for last transaction it should wait for 5 minutes if response is not coming.
                    {
                        if (counter == 300)//300*1000= 5 minutes
                        {
                            quest.errorMessage = "Unknown";
                            tmr.Stop();
                            (sen as Form).Close();
                        }
                    }
                    else
                    {
                        if (counter == 90 && Status)//if pinpad is not connected.
                        {
                            quest.errorMessage = "Quest response timeout!...";
                            tmr.Stop();
                            (sen as Form).Close();
                        }
                    }
                    counter++;
                };
                tmr.Start();
            };

            frm.FormClosing += (sen, arg) =>
            {
                if (string.IsNullOrEmpty(quest.errorMessage))
                {
                    arg.Cancel = true;
                }
            };
            frm.ShowDialog();
            log.LogMethodExit(null);
        }

        public override bool IsPrinterRequired
        {
            get
            {
                return true;
            }
        }

        public override bool IsPartiallyApproved
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Makes payment.
        /// </summary>
        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            if (!initialized)
            {
                log.LogMethodExit(null, "Throwing PaymentGatewayException - Payment Gateway not initialized");
                throw new PaymentGatewayException("Payment Gateway not initialized");
            }
            else
            {
                CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CCREQUEST_TRANSACTION_TYPE_PURCHASE);
                bool returnValue = false;
                string Message = "";
                purchaseOrRefund = true;//This message is used to display the swipe your credit card message in side the wait window. used in QuestWaitForEvent()
                Status = quest.StatusCheck();//Before starting the Transaction checking the status of the pin pad.(Connected or not)
                QuestWaitForEvent();//waiting for the response
                if (quest.errorMessage.Equals("OK"))//If no error
                {
                    Status = false;//used in QuestWaitForEvent()
                    returnValue = quest.PerformSale(cCRequestPGWDTO.RequestID.ToString(), utilities.ParafaitEnv.POSMachine, int.Parse((transactionPaymentsDTO.Amount * 100).ToString()));// The amount which we are passing should be muliplied by 100. Means 1$=100 cents
                    QuestWaitForEvent();//waiting for response
                    if (returnValue)
                    {
                        if (quest.nResult == 0)//if approved
                        {
                            //If transaction is successfull for the requested amount.
                            transactionPaymentsDTO.CreditCardAuthorization = "Quest";
                            transactionPaymentsDTO.CreditCardNumber = "";
                            transactionPaymentsDTO.NameOnCreditCard = "";
                            transactionPaymentsDTO.CreditCardName = "";
                            if (utilities.getParafaitDefaults("PRINT_CUSTOMER_RECEIPT") == "Y")
                            {
                                quest.PrintReceipt(quest.printText);//printing customer copy for unattended POS as per certification reqt.
                            }
                            if (!QuestEFTPOS.unattended)//if not unattended
                            {
                                if (utilities.getParafaitDefaults("PRINT_MERCHANT_RECEIPT") == "Y")
                                {
                                    quest.PrintReceipt(quest.printText.Replace("CUSTOMER COPY", "MERCHANT COPY"));//printing merchant copy 
                                }
                                if (int.Parse((transactionPaymentsDTO.Amount * 100).ToString()) != quest.amount)//checking for difference amount after transaction.
                                {
                                    if (int.Parse((transactionPaymentsDTO.Amount * 100).ToString()) > quest.amount)
                                    {
                                        showMessageDelegate("The transaction amount is less than the amount required.Please pay the remaining amount.", "Quest");
                                    }
                                    else if (int.Parse((transactionPaymentsDTO.Amount * 100).ToString()) < quest.amount)
                                    {
                                        showMessageDelegate("The transaction amount is more than the amount required. Please make refund to reverse the last transaction.", "Quest");
                                    }
                                }
                            }
                            else
                            {
                                //unattended part.
                                if (int.Parse((transactionPaymentsDTO.Amount * 100).ToString()) != quest.amount)//checking for difference amount
                                {
                                    if (int.Parse((transactionPaymentsDTO.Amount * 100).ToString()) > quest.amount)
                                    {
                                        Message = "The transaction amount is less than the amount required.Please pay the remaining amount.";
                                    }
                                }
                            }
                            utilities.EventLog.logEvent(PaymentGateways.Quest.ToString(), 'I', "Quest", Message, CREDIT_CARD_PAYMENT, 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                            transactionPaymentsDTO.Amount = double.Parse(quest.amount.ToString()) / 100;
                            transactionPaymentsDTO.Reference = quest.tranReference;
                        }
                        else
                        {
                            log.Error("Unable to complete the Quest transction");
                            purchaseOrRefund = false;
                            Message = (string.IsNullOrEmpty(quest.errorMessage)) ? "Transaction Not Complete" : quest.errorMessage;
                            utilities.EventLog.logEvent(PaymentGateways.Quest.ToString(), 'D', Message, Message, CREDIT_CARD_PAYMENT, 3);
                            log.LogMethodExit(null, "Throwing PaymentGatewayException - " + Message);
                            throw new PaymentGatewayException(Message);
                        }
                    }
                    else
                    {
                        log.Error("Unable to complete the Quest transction");
                        purchaseOrRefund = false;
                        Message = (string.IsNullOrEmpty(quest.errorMessage))?"Transaction Not Complete": quest.errorMessage;
                        utilities.EventLog.logEvent(PaymentGateways.Quest.ToString(), 'D', Message, Message, CREDIT_CARD_PAYMENT, 3);
                        log.LogMethodExit(null, "Throwing PaymentGatewayException - " + Message);
                        throw new PaymentGatewayException(Message);
                    }
                }
                else
                {
                    log.Error("Unable to complete the Quest transction");
                    purchaseOrRefund = false;
                    Message = quest.errorMessage;
                    utilities.EventLog.logEvent(PaymentGateways.Quest.ToString(), 'D', Message, Message, CREDIT_CARD_PAYMENT, 3);
                    log.LogMethodExit(null, "Throwing PaymentGatewayException - " + Message);
                    throw new PaymentGatewayException(Message);
                }

                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;
            }
        }

        /// <summary>
        /// Reverts the payment.
        /// </summary>
        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            if (!initialized)
            {
                log.LogMethodExit(null, "Throwing PaymentGatewayException - Payment Gateway not initialized");
                throw new PaymentGatewayException("Payment Gateway not initialized");
            }
            else
            {
                //quest refund transaction starts
                CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CCREQUEST_TRANSACTION_TYPE_REFUND);
                bool returnValue = false;
                string Message = "";
                purchaseOrRefund = true;//used to display 'swipe your credit card message'. used in QuestWaitForEvent()//16-06-2015
                Status = quest.StatusCheck();//checks for the status of the pinpad whether it is connected or not.
                QuestWaitForEvent();//wait for the response
                if (quest.errorMessage.Equals("OK"))//if in proper condition
                {
                    Status = false;//16-06-2015:used in QuestWaitForEvent()
                    returnValue = quest.PerformRefund(transactionPaymentsDTO.TransactionId.ToString(), utilities.ParafaitEnv.POSMachine, int.Parse((transactionPaymentsDTO.Amount * 100).ToString()));//Amount should be integer and transaction reference should be string.
                    QuestWaitForEvent();//waiting for the response
                    if (returnValue)
                    {
                        if (quest.nResult == 0)//if approved
                        {
                            transactionPaymentsDTO.CreditCardAuthorization = "Quest";
                            transactionPaymentsDTO.CreditCardNumber = "";
                            transactionPaymentsDTO.NameOnCreditCard = "";
                            transactionPaymentsDTO.CreditCardName = "";
                            if (!QuestEFTPOS.unattended)//if not unattended pos
                            {
                                if (utilities.getParafaitDefaults("PRINT_CUSTOMER_RECEIPT") == "Y")
                                {
                                    quest.PrintReceipt(quest.printText);//printing customer reciept
                                }
                                if (utilities.getParafaitDefaults("PRINT_MERCHANT_RECEIPT") == "Y")
                                {
                                    quest.PrintReceipt(quest.printText.Replace("CUSTOMER COPY", "MERCHANT COPY"));//printing murchant reciept
                                }
                            }
                            utilities.EventLog.logEvent(PaymentGateways.Quest.ToString(), 'I', "Quest", quest.errorMessage, CREDIT_CARD_REFUND, 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);//entering log
                            if (int.Parse((transactionPaymentsDTO.Amount * 100).ToString()) != quest.amount)//checking difference in transaction amount. after transaction.
                            {
                                if (int.Parse((transactionPaymentsDTO.Amount * 100).ToString()) > quest.amount)
                                {
                                    showMessageDelegate("The amount refunded is less than the required.Please refund remaining amount.", "Quest");
                                }
                                else if (int.Parse((transactionPaymentsDTO.Amount * 100).ToString()) < quest.amount)
                                {
                                    showMessageDelegate("The amount refunded is more than the required. Please make payment to reverse the last transaction.", "Quest");
                                }
                            }
                            //here unattended will not come into picture because there is no refund in unattended.
                            transactionPaymentsDTO.Amount = double.Parse(quest.amount.ToString()) / 100;//setting transaction amount
                            transactionPaymentsDTO.Reference = quest.tranReference;//setting the reference
                        }
                        else
                        {
                            log.Error("Refund: Unable to complete the Quest transction");
                            purchaseOrRefund = false;
                            Message = (string.IsNullOrEmpty(quest.errorMessage)) ? "Transaction Not Complete" : quest.errorMessage;
                            utilities.EventLog.logEvent(PaymentGateways.Quest.ToString(), 'D', Message, Message, CREDIT_CARD_REFUND, 3);
                            log.LogMethodExit(null, "Refund Throwing PaymentGatewayException - " + Message);
                            throw new PaymentGatewayException(Message);
                        }
                    }
                    else
                    {
                        log.Error("Refund: Unable to complete the Quest transction");
                        purchaseOrRefund = false;
                        Message = (string.IsNullOrEmpty(quest.errorMessage)) ? "Transaction Not Complete" : quest.errorMessage;
                        utilities.EventLog.logEvent(PaymentGateways.Quest.ToString(), 'D', Message, Message, CREDIT_CARD_REFUND, 3);
                        log.LogMethodExit(null, "Throwing PaymentGatewayException - " + Message);
                        throw new PaymentGatewayException(Message);
                    }
                }
                else
                {
                    log.Error("Refund: Unable to complete the Quest transction");
                    purchaseOrRefund = false;
                    Message = quest.errorMessage;
                    utilities.EventLog.logEvent(PaymentGateways.Quest.ToString(), 'D', Message, Message, CREDIT_CARD_REFUND, 3);
                    log.LogMethodExit(null, "Throwing PaymentGatewayException - Payment Gateway not initialized");
                    throw new PaymentGatewayException(Message);
                }

                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;
            }
            
        }
    }
}
