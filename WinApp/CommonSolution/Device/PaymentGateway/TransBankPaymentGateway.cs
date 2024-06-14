/********************************************************************************************
 * Project Name - TransBankPaymentGateway
 * Description  - TransBankPaymentGateway class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.0      25-Jun-2019      Raghuveera     Created
 *2.110.0     28-Jan-2021      Mathew         Added option to wait for device to response for each
 *                                            transaction
 *2.130.4     22-Feb-2022      Mathew Ninan   Modified DateTime to ServerDateTime 
 *2.150.1     22-Feb-2023      Guru S A       Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Transbank.Autoservicio;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class TransBankPaymentGateway : PaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Autoservicio autoservicio;
        Polling polling;
        IDisplayStatusUI statusDisplayUi;
        int comPort = -1;
        public TransBankPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
            : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            
            log.LogMethodExit();
        }
        public override void Initialize()
        {
            log.LogMethodEntry();
            try
            {
                statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, "", "TransBank Payment Gateway");
                statusDisplayUi.EnableCancelButton(false);
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                thr.Start();
                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Initialization process is in progress."));
                if(int.TryParse(utilities.getParafaitDefaults("CREDIT_CARD_TERMINAL_PORT_NO"), out comPort) == false ||
                    comPort <= 0)
                {
                    log.LogMethodExit(null, "Throwing Exception - TransBank CREDIT_CARD_TERMINAL_PORT_NO not set.");
                    throw new Exception("TransBank CREDIT_CARD_TERMINAL_PORT_NO not set.");
                }
                Configuration configuration = new Configuration();
                configuration.PortName = "COM" + comPort;
                configuration.PortBaudRate = 9600;
                configuration.PortParity = System.IO.Ports.Parity.None;
                configuration.PortDataBits = 8;
                configuration.PortStopBits = System.IO.Ports.StopBits.One;
                autoservicio = new Autoservicio(configuration);
                polling = autoservicio.PollingTransaction;
                log.LogVariableState("Object Autoservicio: ", autoservicio);
                if (autoservicio == null)
                {
                    log.Error("Device is not configured.");
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Device is not configured."));
                    throw (new Exception("Device is not configured."));
                }
                BatchClose();
            }
            finally
            {
                statusDisplayUi.CloseStatusWindow();
            }
            log.LogMethodExit();
        }
        public void BatchClose()
        {
            log.LogMethodEntry();
            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Closing all previous transactions."));
            CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
            TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO();
            transactionPaymentsDTO.TransactionId = Convert.ToInt32(ServerDateTime.Now.ToString("mmssfff"));
            CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, "CLOSE TRANSACTION");
            Cierre cierre;
            cierre = autoservicio.CierreTransaction;
            //do { log.Debug("Pooling...Espere..."); } while (!polling.Transaccion().Status);
            List<Task> tasks = new List<Task>();
            tasks.Add(Task.Factory.StartNew(() => WaitEvent()));
            Task.WaitAll(tasks.ToArray());
            CierreResponse cr = cierre.Transaccion(true);
            if (cr == null)
            {
                log.LogMethodExit(null, "Null response received.");
                throw new Exception("Response is not received.");
            }
            cCTransactionsPGWDTO.DSIXReturnCode = cr.CodigoRespuesta;
            cCTransactionsPGWDTO.TextResponse = GetResponseText(Convert.ToInt32(string.IsNullOrEmpty(cCTransactionsPGWDTO.DSIXReturnCode) ? "-1" : cCTransactionsPGWDTO.DSIXReturnCode));
            cCTransactionsPGWDTO.RecordNo = transactionPaymentsDTO.TransactionId.ToString();
            cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
            cCTransactionsPGWDTO.TranCode = TransactionType.BATCH_CLOSE.ToString();
            cCTransactionsPGWDTO.TransactionDatetime = ServerDateTime.Now;
            cCTransactionsPGWDTO.ProcessData = cr.CodigoComercio;
            int strLength = Math.Min((cr.TerminalID + "|" + cr.CampoImpresion).Length, 200);
            cCTransactionsPGWDTO.AcqRefData = (cr.TerminalID + "|" + cr.CampoImpresion).Substring(0, strLength);
            CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
            cCTransactionsPGWBL.Save();
            if (string.IsNullOrEmpty(cCTransactionsPGWDTO.DSIXReturnCode) || Convert.ToInt32(cCTransactionsPGWDTO.DSIXReturnCode) != 0)
            {
                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Closing the last transaction is failed." + cCTransactionsPGWDTO.TextResponse));
                log.Info("Closing the last transaction is failed." + cCTransactionsPGWDTO.TextResponse);                
            }
            else
            {
                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Closing the last transaction is successful." + cCTransactionsPGWDTO.TextResponse));
                log.Debug("Closing the last transaction is successful." + cCTransactionsPGWDTO.TextResponse);
            }

            log.LogMethodExit();
        }

        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            try
            {
                StandaloneRefundNotAllowed(transactionPaymentsDTO);
                statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "TransBank Payment Gateway");
                statusDisplayUi.EnableCancelButton(false);
                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                thr.Start();
                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                Venta venta;
                venta = autoservicio.VentaTransaction;
                //do { log.Debug("Pooling...Espere..."); } while (!polling.Transaccion().Status);
                List<Task> tasks = new List<Task>();
                tasks.Add(Task.Factory.StartNew(() => WaitEvent()));
                Task.WaitAll(tasks.ToArray());
                VentaResponse vr = venta.Transaccion((transactionPaymentsDTO.Amount).ToString(), ccRequestPGWDTO.RequestID.ToString(), true, false);
                if (vr == null)
                {
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("No Response."));
                    log.LogMethodExit(null, "Null response received.");
                    throw new Exception("Response is not received.");
                }
                cCTransactionsPGWDTO.RecordNo = string.IsNullOrEmpty(vr.TipoCuota) ? "-1" : vr.TipoCuota;
                cCTransactionsPGWDTO.Purchase = transactionPaymentsDTO.Amount.ToString();
                cCTransactionsPGWDTO.Authorize = vr.MontoCuota;
                cCTransactionsPGWDTO.TipAmount = vr.NumeroCuota + "+" + vr.GlosaTipoCuota;
                cCTransactionsPGWDTO.TokenID = vr.NumeroDeCuenta;
                cCTransactionsPGWDTO.RefNo = vr.NumeroOperacion;
                //cCTransactionsPGWDTO.TransactionDatetime = string.IsNullOrEmpty(vr.FechaTransaccion + vr.HoraTransaccion) ? DateTime.Now : Convert.ToDateTime(vr.FechaTransaccion + vr.HoraTransaccion);
                cCTransactionsPGWDTO.CardType = vr.TipoTarjeta;
                cCTransactionsPGWDTO.AcctNo = string.IsNullOrEmpty(vr.Ultimos4DigitosTarjeta) ? "" : vr.Ultimos4DigitosTarjeta.PadLeft(16, 'X');
                cCTransactionsPGWDTO.AuthCode = vr.CodigoAutorizacion;
                cCTransactionsPGWDTO.DSIXReturnCode = vr.CodigoRespuesta;
                cCTransactionsPGWDTO.TextResponse = GetResponseText(Convert.ToInt32((string.IsNullOrEmpty(cCTransactionsPGWDTO.DSIXReturnCode) ? "-1" : cCTransactionsPGWDTO.DSIXReturnCode)));
                cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                cCTransactionsPGWDTO.TranCode = TransactionType.SALE.ToString();
                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                cCTransactionsPGWDTO.ProcessData = vr.CodigoComercio;
                cCTransactionsPGWDTO.UserTraceData = vr.AbreviacionTarjeta;
                int strLength = Math.Min((vr.TerminalID + "|" + vr.CampoImpresion + "|" + vr.FechaContable + "|" + vr.Monto).Length, 200);
                cCTransactionsPGWDTO.AcqRefData = (vr.TerminalID + "|" + vr.CampoImpresion + "|" + vr.FechaContable + "|" + vr.Monto).Substring(0, strLength);
                CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                cCTransactionsPGWBL.Save();
                if (!string.IsNullOrEmpty(vr.CampoImpresion))
                {
                    string printText = FormatPrint(vr.CampoImpresion);
                    Print(printText);
                }
                if (string.IsNullOrEmpty(cCTransactionsPGWDTO.DSIXReturnCode) || Convert.ToInt32(cCTransactionsPGWDTO.DSIXReturnCode) != 0)
                {
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(cCTransactionsPGWDTO.TextResponse));
                    log.Info(cCTransactionsPGWDTO.TextResponse);
                    throw new Exception(cCTransactionsPGWDTO.TextResponse + "-" + cCTransactionsPGWDTO.DSIXReturnCode);
                }
                else
                {
                    transactionPaymentsDTO.CCResponseId = cCTransactionsPGWDTO.ResponseID;
                    transactionPaymentsDTO.Reference = cCTransactionsPGWDTO.RefNo;
                    transactionPaymentsDTO.CreditCardAuthorization = cCTransactionsPGWDTO.AuthCode;
                    transactionPaymentsDTO.CreditCardNumber = cCTransactionsPGWDTO.AcctNo;
                    transactionPaymentsDTO.Memo = vr.CampoImpresion;
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(cCTransactionsPGWDTO.TextResponse));
                    log.Debug(cCTransactionsPGWDTO.TextResponse);
                }

            }
            finally
            {
                statusDisplayUi.CloseStatusWindow();
            }
            
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }


        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            if(isUnattended == false)
            {
                log.LogMethodExit(null, "Refund is not allowed in POS.");
                throw new Exception("Refund is not allowed in POS.");
            }
            try
            {
                statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(4202, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "TransBank Payment Gateway");
                statusDisplayUi.EnableCancelButton(false);
                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Reverse Transaction") + utilities.MessageUtils.getMessage(1008));
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                thr.Start();
                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CCREQUEST_TRANSACTION_TYPE_REFUND);
                Anulacion anulacion;
                anulacion = autoservicio.AnulacionTransaction;
                //do { log.Debug("Pooling...Espere..."); } while (!polling.Transaccion().Status);
                List<Task> tasks = new List<Task>();
                tasks.Add(Task.Factory.StartNew(() => WaitEvent()));
                Task.WaitAll(tasks.ToArray());
                AnulacionResponse ar = anulacion.Transaccion();
                if (ar == null)
                {
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("No Response."));
                    log.LogMethodExit(null, "Null response received.");
                    throw new Exception("Response is not received.");
                }
                cCTransactionsPGWDTO.RecordNo = "-1";
                cCTransactionsPGWDTO.Purchase = transactionPaymentsDTO.Amount.ToString();
                cCTransactionsPGWDTO.RefNo = ar.NumeroOperacion;
                //cCTransactionsPGWDTO.TransactionDatetime = DateTime.Now;
                cCTransactionsPGWDTO.AuthCode = ar.CodigoAutorizacion;
                cCTransactionsPGWDTO.DSIXReturnCode = ar.CodigoRespuesta;
                cCTransactionsPGWDTO.TextResponse = GetResponseText(Convert.ToInt32(string.IsNullOrEmpty(cCTransactionsPGWDTO.DSIXReturnCode) ? "-1" : cCTransactionsPGWDTO.DSIXReturnCode));
                cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                cCTransactionsPGWDTO.TranCode = TransactionType.VOID.ToString();
                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                cCTransactionsPGWDTO.ProcessData = ar.CodigoComercio;
                cCTransactionsPGWDTO.AcqRefData = ar.TerminalID + "|" + "|" + "|";
                CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                cCTransactionsPGWBL.Save();
                if (string.IsNullOrEmpty(cCTransactionsPGWDTO.DSIXReturnCode) || Convert.ToInt32(cCTransactionsPGWDTO.DSIXReturnCode) != 0)
                {
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(cCTransactionsPGWDTO.TextResponse));
                    log.Info(cCTransactionsPGWDTO.TextResponse + "-" + cCTransactionsPGWDTO.DSIXReturnCode);
                    throw new Exception(cCTransactionsPGWDTO.TextResponse + "-" + cCTransactionsPGWDTO.DSIXReturnCode);
                }
                else
                {
                    transactionPaymentsDTO.CCResponseId = cCTransactionsPGWDTO.ResponseID;
                    transactionPaymentsDTO.Reference = cCTransactionsPGWDTO.RefNo;
                    transactionPaymentsDTO.CreditCardAuthorization = cCTransactionsPGWDTO.AuthCode;
                    transactionPaymentsDTO.CreditCardNumber = cCTransactionsPGWDTO.AcctNo;
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(cCTransactionsPGWDTO.TextResponse));
                    log.Debug(cCTransactionsPGWDTO.TextResponse);
                }
                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;
            }
            finally
            {
                statusDisplayUi.CloseStatusWindow();
            }
        }

        /// <summary>
        /// Format print receipt from Gateway and return formatted string
        /// </summary>
        /// <param name="paymentGatewayPrintText">Unformatted string from Gateway</param>
        /// <returns>Formatted string</returns>
        private string FormatPrint(string paymentGatewayReceipt)
        {
            log.LogMethodEntry(paymentGatewayReceipt);
            string formattedReceipt = string.Empty;
            if (paymentGatewayReceipt != null && paymentGatewayReceipt != "")
                formattedReceipt = Regex.Replace(paymentGatewayReceipt, "(.{" + 40 + "})", "$1" + Environment.NewLine);
            else
                formattedReceipt = paymentGatewayReceipt;
            log.LogMethodExit(formattedReceipt);
            return formattedReceipt;
        }
        private string GetResponseText(int response)
        {
            log.LogMethodEntry(response);
            string responseText;
            switch (response)
            {
                case 0:
                    responseText = utilities.MessageUtils.getMessage("Approved");
                    break;
                case 1:
                    responseText = utilities.MessageUtils.getMessage("Rejected");
                    break;
                case 2:
                    responseText = utilities.MessageUtils.getMessage("Authorizer does not respond");
                    break;
                case 3:
                    responseText = utilities.MessageUtils.getMessage("Fault connection");
                    break;
                case 4:
                    responseText = utilities.MessageUtils.getMessage("Transaction has already been canceled");
                    break;
                case 5:
                    responseText = utilities.MessageUtils.getMessage("There is no Transaction to Cancel");
                    break;
                case 6:
                    responseText = utilities.MessageUtils.getMessage("Card not supported");
                    break;
                case 7:
                    responseText = utilities.MessageUtils.getMessage("Transaction canceled");
                    break;
                case 8:
                    responseText = utilities.MessageUtils.getMessage("You can not Cancel Debit Transaction");
                    break;
                case 9:
                    responseText = utilities.MessageUtils.getMessage("Card Reading Error");
                    break;
                case 10:
                    responseText = utilities.MessageUtils.getMessage("Amount less than the minimum allowed");
                    break;
                case 11:
                    responseText = utilities.MessageUtils.getMessage("There is no sale");
                    break;
                case 12:
                    responseText = utilities.MessageUtils.getMessage("Unsupported Transaction");
                    break;
                case 13:
                    responseText = utilities.MessageUtils.getMessage("You must execute the closing");
                    break;
                case 14:
                    responseText = utilities.MessageUtils.getMessage("Error Encrypting PAN(BCYCLE)");
                    break;
                case 15:
                    responseText = utilities.MessageUtils.getMessage("Error Operating with Debit(BCYCLE)");
                    break;
                case 80:
                    responseText = utilities.MessageUtils.getMessage("Requesting Shape Amount");
                    break;
                case 81:
                    responseText = utilities.MessageUtils.getMessage("Requesting Key Entry");
                    break;
                case 82:
                    responseText = utilities.MessageUtils.getMessage("Sending transaction to the authorizer");
                    break;
                case 90:
                    responseText = utilities.MessageUtils.getMessage("Successful Initialization");
                    break;
                case 91:
                    responseText = utilities.MessageUtils.getMessage("Failed Initialization");
                    break;
                case 92:
                    responseText = utilities.MessageUtils.getMessage("Non - Connected Reader");
                    break;
                default:
                    responseText = utilities.MessageUtils.getMessage("Invalid Response code &1", response);
                    break;                    
            }
            log.LogMethodExit(responseText);
            return responseText;
        }

        private void Print(string printText)
        {
            log.LogMethodEntry(printText);
            try
            {
                using (System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument())
                {
                    pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize();//("Custom", 300, 700);
                    pd.PrintPage += (sender, e) =>
                    {
                        Font f = new Font("Courier New", 9);
                        e.Graphics.DrawString(printText, f, new SolidBrush(Color.Black), new RectangleF(0, 0, pd.DefaultPageSettings.PrintableArea.Width, pd.DefaultPageSettings.PrintableArea.Height));
                    };
                    pd.Print();
                }
            }
            catch (Exception ex)
            {
                utilities.EventLog.logEvent("Transbank PaymentGateway", 'I', "Receipt print failed.", printText, this.GetType().Name, 2, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                log.Error("Print receipt failed." + Environment.NewLine + printText, ex);
            }
            log.LogMethodExit(null);
        }
        private void WaitEvent()
        {
            log.LogMethodEntry();
            try
            {
                int loopCount = 0;
                while (!polling.Transaccion().Status && loopCount < 15)
                {
                    loopCount++;
                    Thread.Sleep(1000);
                    log.Debug("Loop count: " + loopCount);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(false);
            }
        }
    }
}
