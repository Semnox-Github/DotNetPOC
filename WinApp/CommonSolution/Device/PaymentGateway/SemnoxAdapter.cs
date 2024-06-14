using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IPADLib;
using System.Configuration;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;
//using Semnox.Mercury.PaymentGateway;
//using Semnox.Webservices.PaymentGateway;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{

    /// <summary>
    /// Semnox Adapter Class
    /// </summary>
    public class SemnoxMercuryAdapter
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private CardType Trantype;
        SimState sState;
        private IPAD pp = null;
        private ClsDynamagMagnesafe magnesafe = null;
        private DeviceMonitor devmon;
        private DeviceManager dv;
        private CardData cd;
        private Byte requestcardwaittime = Convert.ToByte(ConfigurationManager.AppSettings["RequestWaitCardTime"]);
        private Byte TMCardRequest = Convert.ToByte(ConfigurationManager.AppSettings["TMCardRequest"]);
        private Byte TMPINComplete = Convert.ToByte(ConfigurationManager.AppSettings["TMPINComplete"]);
        private Byte TMKeyComplete = Convert.ToByte(ConfigurationManager.AppSettings["TMKeyComplete"]);
        private Byte TMPINDisplay = Convert.ToByte(ConfigurationManager.AppSettings["TMPINDisplay"]);
        private Byte TMPINRequest = Convert.ToByte(ConfigurationManager.AppSettings["TMPINRequest"]);
        private string AccountSource = ConfigurationManager.AppSettings["AccountSource"];
        private string _amount = "";
        private string _invoiceNo = "";
        private string _refNo = "";
        private string _tranCode = "";
        private string _Tipamount = "0.00";//Modification on 09-Nov-2015:Tip Feature      
        private ErrorResponseEventArgs DefaultInitialErrorResAttr = null;

        private Semnox.Parafait.Device.PaymentGateway.AdapterUI.frmStatus _frm = null;
        /// <summary>
        /// Manual Reset Event
        /// </summary>
        public ManualResetEvent mre;
        private ClsResponseMessageAttributes objresponse = null;

        bool isMagnesafe = false;
        Utilities _Utilities;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="inUtilities"></param>
        public SemnoxMercuryAdapter(IntPtr handle, Utilities inUtilities)
        {
            log.LogMethodEntry(handle, inUtilities);

            _Utilities = inUtilities;

            try
            {
                // On load the webservice will be initiated. Actual WS method will be invoked in the Business layer.
                WebServiceInvoker.WebServiceInvoke(new Uri(ConfigurationManager.AppSettings["uri"]));
                objresponse = new ClsResponseMessageAttributes();
                WSCore._utilities = inUtilities;
            }
            catch(Exception e)
            {
                log.Error("Unable to initiate the webservice", e);

                ErrorResponseEventArgs ex = new ErrorResponseEventArgs(PGSEMessages.PGWSError.ToString(), null);

                log.LogMethodExit(null, "Throwing ErrorResponseEventArgs" + ex);
                throw ex;
            }

            // IPAD
            dv = new DeviceManager();
            string p = dv.FindIPAD();

            if (p != null)
            {
                DeviceConnect(p);
                devmon = new DeviceMonitor();
                devmon.RegisterForEvents(handle);
                devmon.DeviceAttachedEvent += new DeviceAttachedEventHandler(devmon_DeviceAttachedEvent);
                devmon.DeviceRemovedEvent += new DeviceRemovedEventHandler(devmon_DeviceRemovedEvent);
            }
            else
            {
                if (initDynamagReader() == false)
                {
                    log.LogMethodExit(null, "Throwing ErrorResponseEventArgs" + PGSEMessages.PGDeviceNotConnected.ToString());
                    throw new ErrorResponseEventArgs(PGSEMessages.PGDeviceNotConnected.ToString(), null);
                }
            }
            WSCore.OnWScoreEventReturn += this.OnWScoreErrorHandler;

            log.LogMethodExit(null);
        }

        /// <summary>
        /// Get/Set Property for RetrunResponseMessage
        /// </summary>
        public ClsResponseMessageAttributes RetrunResponseMessage
        {
            get
            {
                return objresponse;
            }
            set
            {
                objresponse = value;
            }
        }

        void _frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            bool Istransactionsucess = _frm.returnval;
            objresponse = _frm.lasttranResponseMsg;
            _frm.FormClosed -= _frm_FormClosed;
            _frm = null;
            if (objresponse == null)
            {
                objresponse = new ClsResponseMessageAttributes();
                objresponse.CmdStatus = "null";
            }
            RetrunResponseMessage = objresponse;

            if (mre != null)
                mre.Set();

            log.LogMethodExit(null);
        }

        private void CreateOrShow()
        {
            log.LogMethodEntry();

            if (_frm == null)
            {
                _frm = new Semnox.Parafait.Device.PaymentGateway.AdapterUI.frmStatus(DefaultInitialErrorResAttr);
                _frm.FormClosed += _frm_FormClosed;
                _frm.ShowDialog();
            }

            log.LogMethodExit(null);
        }

        private void showpopup(ErrorResponseEventArgs responseeventargs)
        {
            log.LogMethodEntry(responseeventargs);

            DefaultInitialErrorResAttr = responseeventargs;
            ThreadStart thr = delegate
            {
                CreateOrShow();
                if (_frm != null)
                    _frm.UpdateStatus(responseeventargs);
            };
            new Thread(thr).Start();

            log.LogMethodExit(null);
        }

        /// <summary>
        /// OnWScoreErrorHandler Event
        /// </summary>
        /// <param name="responseeventargs"></param>
        public void OnWScoreErrorHandler(ErrorResponseEventArgs responseeventargs)
        {
            log.LogMethodEntry(responseeventargs);
            showpopup(responseeventargs);
            log.LogMethodExit(null);
        }

        /// <summary>
        /// VoidTransaction Method
        /// </summary>
        /// <param name="ResponseId"></param>
        /// <param name="Amount"></param>
        public void VoidTransaction(object ResponseId, double Amount = -1)
        {
            log.LogMethodEntry(ResponseId, Amount);

            mre = new ManualResetEvent(false);
            RetrunResponseMessage = null;
            objresponse = new ClsResponseMessageAttributes();

            if (ResponseId != null && !ResponseId.Equals(DBNull.Value))
            {
                List<ClsRequestMessageAttributes> listVoidSaleReturn = WSCore.GetReturnAndVoidSale(ResponseId);
                if (listVoidSaleReturn.Count != 0)
                {
                    foreach (ClsRequestMessageAttributes objvoidsale in listVoidSaleReturn)
                    {
                        VoidTransaction(objvoidsale, Amount);
                    }
                }
                else
                {
                    objresponse.TextResponse = setCustomStatusMessage(enumCommandStatus.EmptyListVoidSale);
                    objresponse.CmdStatus = enumCmdResponse.Declined.ToString();
                    showpopup(new ErrorResponseEventArgs(setCustomStatusMessage(enumCommandStatus.EmptyListVoidSale), objresponse));
                    mre.Set();
                }
            }
            else
            {
                objresponse.CmdStatus = enumCmdResponse.Declined.ToString();
                objresponse.TextResponse = setCustomStatusMessage(enumCommandStatus.MissingFields);
                showpopup(new ErrorResponseEventArgs(setCustomStatusMessage(enumCommandStatus.MissingFields), objresponse));
                mre.Set();
            }

            log.LogMethodExit(null);
        }

        /// <summary>
        /// VoidTransaction Method
        /// </summary>
        /// <param name="objvoidsale"></param>
        /// <param name="Amount"></param>
        public void VoidTransaction(ClsRequestMessageAttributes objvoidsale, double Amount = -1)
        {
            log.LogMethodEntry(objvoidsale, Amount);

            bool fullReversal = false;
            if (Amount == Convert.ToDouble(objvoidsale.Purchase))
                fullReversal = true;
            AccountSource = ConfigurationManager.AppSettings["AccountSource"];//Modification On 2017-Jul-07 for showing entry mode
            // void the credit card sale if it is complete void
            if (objvoidsale.CardType != "DEBIT" && fullReversal)
            {
                ClsRequestMessageAttributes myrvoidsalemsgattributes = new ClsRequestMessageAttributes();
                myrvoidsalemsgattributes.TranCode = ConfigurationManager.AppSettings["TranCode"]; // myreqmsgattributes.TranCode;
                myrvoidsalemsgattributes.InvoiceNo = objvoidsale.InvoiceNo;
                myrvoidsalemsgattributes.TranType = (CardType)Enum.Parse(typeof(CardType), "Credit");//objvoidsale.TranType;
                myrvoidsalemsgattributes.RefNo = objvoidsale.RefNo;
                myrvoidsalemsgattributes.RecordNo = objvoidsale.RecordNo;
                myrvoidsalemsgattributes.Purchase = objvoidsale.Purchase;
                myrvoidsalemsgattributes.AuthCode = objvoidsale.AuthCode;
                myrvoidsalemsgattributes.AcqRefData = objvoidsale.AcqRefData;
                myrvoidsalemsgattributes.ProcessData = objvoidsale.ProcessData;
                RequestMessageCommonProperties(ref myrvoidsalemsgattributes);
                try
                {
                    string mystring = string.Format(PGSEMessages.MsgCustomCardSwipeVoidSale, objvoidsale.AcctNo);
                    showpopup(new ErrorResponseEventArgs(mystring, null));
                    objresponse = WSCore.VoidSaleInvoice(myrvoidsalemsgattributes, true);
                    if (objresponse.CmdStatus.Equals(enumCommandStatus.Approved.ToString()) == false)
                        objresponse = WSCore.VoidSaleInvoice(myrvoidsalemsgattributes, false);
                    showpopup(new ErrorResponseEventArgs(setCustomStatusMessage(WSCore.CmdStatus), objresponse));
                }
                catch (Exception ex)
                {
                    log.Error("Error occured during Void Transaction", ex);
                    objresponse.CmdStatus = enumCommandStatus.IPADCancel.ToString();
                    showpopup(new ErrorResponseEventArgs(ex.ToString(), objresponse));
                }
            }
            else
            {
                string mystring = string.Format(PGSEMessages.MsgCustomCardSwipeReturn, objvoidsale.AcctNo);
                showpopup(new ErrorResponseEventArgs(mystring, null));
                ReturnTransaction(objvoidsale.InvoiceNo, fullReversal? objvoidsale.Purchase : Amount.ToString("0.00"));
            }

            log.LogMethodExit(null);
        }

        void ReturnTransaction(string invoicenumber, string PurchaseAmount)
        {
            log.LogMethodEntry(invoicenumber, PurchaseAmount);

            _tranCode = "Return";
            _amount = PurchaseAmount;
            _refNo = invoicenumber;
            _invoiceNo = invoicenumber;
            if (validateAmount(_amount))
            {
                sState = SimState.SimComplete;
                pp.EndSession();
                pp.SendAmount(AmountType.Debit, _amount);

                try//Starts: Modification On 2017-Jul-07 for showing entry mode
                {
                    Semnox.Parafait.Device.PaymentGateway.AdapterUI.frmEntryMode entryMode = new Semnox.Parafait.Device.PaymentGateway.AdapterUI.frmEntryMode();
                    if (entryMode.ShowDialog() == DialogResult.OK)
                    {
                        AccountSource = entryMode.AccountSource;
                    }
                    else
                    {
                        objresponse.CmdStatus = enumCommandStatus.Error.ToString();
                        showpopup(new ErrorResponseEventArgs(PGSEMessages.PGCardCancelled, objresponse));
                        log.LogMethodExit(null);
                        return;
                    }
                }
                catch(Exception ex)
                {
                    log.Error("Error when entering the Entry Mode",ex);
                    objresponse.CmdStatus = enumCommandStatus.Error.ToString();
                    showpopup(new ErrorResponseEventArgs(PGSEMessages.PGError, objresponse));
                }//Ends: Modification On 2017-Jul-07 for showing entry mode
                if (AccountSource == "Keyed")
                    pp.RequestManualCardData(requestcardwaittime, Buzzer.SingleBeep);
                else
                    pp.RequestCard(requestcardwaittime, CardMsg.PleaseSwipeCard, Buzzer.SingleBeep);
            }
            else
            {
                Cancelled();
                showpopup(new ErrorResponseEventArgs(setCustomStatusMessage(enumCommandStatus.InValidPrice), objresponse));
            }

            log.LogMethodExit(null);
        }

        /// <summary>
        /// MakeDynamagPayment Method
        /// </summary>
        /// <param name="InvoiceNo"></param>
        /// <param name="Amount"></param>
        /// <param name="TipAmount"></param>
        /// <param name="ResponseId"></param>
        public void MakeDynamagPayment(string InvoiceNo, string Amount, string TipAmount, object ResponseId)//Begin Modification on 09-Nov-2015:Tip Feature
        {
            log.LogMethodEntry(InvoiceNo, Amount, TipAmount, ResponseId);

            _amount = Amount;
           _Tipamount = TipAmount;
           try
           {
               if (validateAmount(_amount) && validateAmount(_Tipamount))
               {
                   if (string.IsNullOrEmpty(ResponseId.ToString()))
                   {//End Modification on 09-Nov-2015:Tip Feature
                       if (initDynamagReader())
                       {
                           _tranCode = "Sale";
                           _invoiceNo = InvoiceNo;
                           _refNo = InvoiceNo;

                           objresponse.TextResponse = PGSEMessages.PGDeviceReady;
                           objresponse.CmdStatus = enumCommandStatus.None.ToString();
                           showpopup(new ErrorResponseEventArgs(PGSEMessages.PGDeviceReady, objresponse));
                           bool timeOut = !mre.WaitOne(175000);
                       }
                       else
                       {
                           objresponse.CmdStatus = enumCommandStatus.DeviceNotConnected.ToString();
                           showpopup(new ErrorResponseEventArgs(setCustomStatusMessage(enumCommandStatus.DeviceNotConnected), objresponse));
                       }
                   }//Begin Modification on 09-Nov-2015:Tip Feature               
                   else
                   {

                       _invoiceNo = InvoiceNo;
                       objresponse.CmdStatus = enumCommandStatus.None.ToString();
                       List<ClsRequestMessageAttributes> list = WSCore.GetReturnAndVoidSale(ResponseId);
                       ClsRequestMessageAttributes myreqmsgattributes1;
                       foreach (ClsRequestMessageAttributes myreqmsgattributes in list)
                       {
                           myreqmsgattributes1 = myreqmsgattributes;
                           RequestMessageCommonProperties(ref myreqmsgattributes1);
                           myreqmsgattributes1.TranCode = "AdjustByRecordNo";
                           myreqmsgattributes1.TipAmount = TipAmount;
                           objresponse = WSCore.PrepareforInvokingWS(Trantype, myreqmsgattributes1);
                       }

                   }//End Modification on 09-Nov-2015:Tip Feature     
               }
               else
               {
                   objresponse.CmdStatus = enumCommandStatus.SimDone.ToString();
                   showpopup(new ErrorResponseEventArgs(setCustomStatusMessage(enumCommandStatus.InValidPrice), objresponse));
               }

               mre.Set();

               if (WSCore.CardTrackDetails != null)
               {
                   WSCore.CardTrackDetails.Clear();
               }
           }
            finally
           {
               magnesafe.Close();
           }

            log.LogMethodExit(null);
        }

        /// <summary>
        /// Make Payment Method
        /// </summary>
        /// <param name="InvoiceNo"></param>
        /// <param name="Amount"></param>
        /// <param name="TipAmount"></param>
        /// <param name="ResponseId"></param>
        public void MakePayment(string InvoiceNo, string Amount, string TipAmount = "0.00", object ResponseId = null)//Modification on 09-Nov-2015:Tip Feature 
        {
            log.LogMethodEntry(InvoiceNo, Amount, TipAmount, ResponseId);

            RetrunResponseMessage = null;
            objresponse = new ClsResponseMessageAttributes();
            mre = new ManualResetEvent(false);
            AccountSource = ConfigurationManager.AppSettings["AccountSource"];//Modification On 2017-Jul-07 for showing entry mode
            if (pp == null)
            {
                if (isMagnesafe)
                    MakeDynamagPayment(InvoiceNo, Amount, TipAmount, ResponseId);//Modification on 09-Nov-2015:Tip Feature 
                else
                {
                    objresponse.CmdStatus = enumCommandStatus.DeviceNotConnected.ToString();
                    showpopup(new ErrorResponseEventArgs(setCustomStatusMessage(enumCommandStatus.DeviceNotConnected), objresponse));
                }
            }
            else
            {
                if (Convert.ToDouble(Amount) < 0)
                {
                    ReturnTransaction(InvoiceNo, (-1 * Convert.ToDouble(Amount)).ToString("0.00"));
                }
                else
                {
                    if (string.IsNullOrEmpty(ResponseId.ToString()))//Begin Modification on 12-Dec-2015:Tip Feature
                    {//Ends Modification on 12-Dec-2015:Tip Feature
                        _tranCode = "Sale";
                        _amount = Amount;
                        _invoiceNo = InvoiceNo;
                        _refNo = InvoiceNo;
                        _Tipamount = TipAmount;//Begins Modification on 12-Dec-2015:Tip Feature
                        if (validateAmount(_amount) && validateAmount(_Tipamount))//Ends Modification on 12-Dec-2015:Tip Feature
                        {
                            sState = SimState.SimComplete;
                            pp.EndSession();
                            pp.SendAmount(AmountType.Credit, (Convert.ToDouble(_amount) + Convert.ToDouble(_Tipamount)).ToString("0.00"));//Modification on 12-Dec-2015:Tip Feature
                            showpopup(new ErrorResponseEventArgs(PGSEMessages.PGDeviceReady, null));
                            
                            try//Starts: Modification On 2017-Jul-07 for showing entry mode
                            {
                                if (_Utilities.getParafaitDefaults("ALLOW_CUSTOMER_TO_DECIDE_ENTRY_MODE").Equals("Y"))
                                {
                                    Semnox.Parafait.Device.PaymentGateway.AdapterUI.frmEntryMode entryMode = new Semnox.Parafait.Device.PaymentGateway.AdapterUI.frmEntryMode();
                                    if (entryMode.ShowDialog() == DialogResult.OK)
                                    {
                                        AccountSource = entryMode.AccountSource;
                                    }
                                    else
                                    {
                                        objresponse.CmdStatus = enumCommandStatus.Error.ToString();
                                        showpopup(new ErrorResponseEventArgs(PGSEMessages.PGCardCancelled, objresponse));

                                        log.LogMethodExit(null);
                                        return;
                                    }
                                }
                                else
                                {
                                    AccountSource = "Swiped";
                                }
                            }
                            catch(Exception ex)
                            {
                                log.Error("Error occured while Sending Amount", ex);
                                objresponse.CmdStatus = enumCommandStatus.Error.ToString();
                                showpopup(new ErrorResponseEventArgs(PGSEMessages.PGError, objresponse));                                
                            }//Ends: Modification On 2017-Jul-07 for showing entry mode
                            if (AccountSource == "Keyed")
                                pp.RequestManualCardData(requestcardwaittime, Buzzer.SingleBeep);
                            else
                                pp.RequestCard(requestcardwaittime, CardMsg.PleaseSwipeCard, Buzzer.SingleBeep);
                        }
                        else
                        {
                            Cancelled();
                            showpopup(new ErrorResponseEventArgs(setCustomStatusMessage(enumCommandStatus.InValidPrice), objresponse));
                        }
                    }
                    else //Begin Modification on 12-Dec-2015:Tip Feature
                    {
                        _amount = Amount;
                        _Tipamount = TipAmount;
                        if (validateAmount(_amount) && validateAmount(_Tipamount))
                        {
                            _invoiceNo = InvoiceNo;
                            objresponse.CmdStatus = enumCommandStatus.None.ToString();
                            List<ClsRequestMessageAttributes> list = WSCore.GetReturnAndVoidSale(ResponseId);
                            ClsRequestMessageAttributes myreqmsgattributes1;
                            foreach (ClsRequestMessageAttributes myreqmsgattributes in list)
                            {
                                myreqmsgattributes1 = myreqmsgattributes;
                                RequestMessageCommonProperties(ref myreqmsgattributes1);
                                myreqmsgattributes1.TranCode = "AdjustByRecordNo";
                                myreqmsgattributes1.TipAmount = TipAmount;
                                objresponse = WSCore.PrepareforInvokingWS(Trantype, myreqmsgattributes1);
                            }
                        }

                    }//Ends Modification on 12-Dec-2015:Tip Feature
                }
            }

            if (WSCore.CardTrackDetails != null)
            {
                WSCore.CardTrackDetails.Clear();
            }

            log.LogMethodExit(null);
        }

        /// <summary>
        /// GetListfromMercury Method
        /// </summary>
        /// <param name="InvoiceNumber"></param>
        /// <returns></returns>
        public string GetListfromMercury(string InvoiceNumber)
        {
            log.LogMethodEntry(InvoiceNumber);

            string returnValueNew = WSCore.InvokeWS_Invoice(InvoiceNumber);
            log.LogMethodExit(returnValueNew);
            return returnValueNew;
        }

        private bool validateAmount(string amount)
        {
            log.LogMethodEntry(amount);

            bool flag = true;

            clsAmountInputArgs allamounts = new clsAmountInputArgs(amount);
            ClsValidations allvalidations = new ClsValidations();
            allamounts = allvalidations.validateAmonts(allamounts);
            if (allamounts.RetMessage != "")
            {
                flag = false;
            }

            log.LogMethodExit(flag);
            return flag;
        }

        void devmon_DeviceAttachedEvent(object sender, DeviceMonitorEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (pp == null)
            {
                DeviceConnect(e.device);
            }

            log.LogMethodExit(null);
        }

        void devmon_DeviceRemovedEvent(object sender, DeviceMonitorEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (pp != null)
            {
                if (pp.DevicePath == e.device)
                {
                    pp.Dispose();
                    pp = null;
                }
            }

            string p = dv.FindIPAD();
            if (p != null) DeviceConnect(p);

            log.LogMethodExit(null);
        }

        void pp_SignatureCaptureCompleteEvent(object sender, SignatureCaptureCompleteEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //MessageBox.Show("pp_SignatureCaptureCompleteEvent");
            log.LogMethodExit(null);
        }

        void pp_GetKeyCompleteEvent(object sender, GetKeyCompleteEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (e.OpStatus == IPADStatus.OK)
            {
                if (sState == SimState.SelectCreditDebit) //select credit/debit
                {
                    showpopup(new ErrorResponseEventArgs(PGSEMessages.PGCardTypeConfirm, null));
                    if (e.key == FunctionKey.Left)  // credit
                    {
                        Trantype = CardType.Credit;
                        sState = SimState.ConfirmAmount;
                        pp.GetResponse(TMKeyComplete, ResponseMsg.AmountOk, KeyMask.Right | KeyMask.Middle, 0); // do confirm amount
                    }
                    else
                    {
                        Trantype = CardType.Debit;
                        pp.RequestPIN(TMPINRequest, PinMsg.EnterPinAmt, 4, 8, Buzzer.SingleBeep, 0); // Options explained MagTek guide - p.8
                    }
                    showpopup(new ErrorResponseEventArgs(PGSEMessages.PGCardTypeSelected + " : " + Trantype, null));
                }
                else  //confirm amount
                {
                    showpopup(new ErrorResponseEventArgs(PGSEMessages.PGCardConfirmAmount, null));
                    if (e.key == FunctionKey.Middle)  // yes
                    {
                        showpopup(new ErrorResponseEventArgs(PGSEMessages.PGDBReqMsgInsert, null));
                        sState = SimState.DisplayThankYou;
                        pp.Display(TMPINDisplay, DisplayMsg.Processing);
                        ClsRequestMessageAttributes myreqmsgattributes = CreateRequestMessage();
                        if (myreqmsgattributes != null)
                        {
                            objresponse = WSCore.PrepareforInvokingWS(Trantype, myreqmsgattributes);
                            DisplayCommandtoIPAD();
                            showpopup(new ErrorResponseEventArgs(setCustomStatusMessage(WSCore.CmdStatus), objresponse));
                        }
                        else
                        {
                            objresponse.CmdStatus = enumCommandStatus.Error.ToString();
                            showpopup(new ErrorResponseEventArgs(PGSEMessages.PGDBReqMsgNotAttNotset, objresponse));
                        }
                    }
                    else
                    {
                        Cancelled(); // cancelled
                        showpopup(new ErrorResponseEventArgs(PGSEMessages.PGCardCancelled, objresponse));
                    }
                }
            }
            else if (e.OpStatus == IPADStatus.UserCancel)
            {
                Cancelled();
                showpopup(new ErrorResponseEventArgs(setCustomStatusMessage(enumCommandStatus.IPADCancel), objresponse));
            }
            else
            {
                SimDone();
                objresponse.CmdStatus = enumCommandStatus.SimDone.ToString();
                showpopup(new ErrorResponseEventArgs(setCustomStatusMessage(enumCommandStatus.SimDone), objresponse));
            }

            log.LogMethodExit(null);
        }

        void pp_PINRequestCompleteEvent(object sender, PINRequestCompleteEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (e.OpStatus == IPADStatus.OK)
            {
                if (WSCore.CardTrackDetails.ContainsKey("DervdKey"))
                {
                    WSCore.CardTrackDetails["DervdKey"] = WSCore.MakeHex(e.KSN, 0, e.KSN.Length);
                }
                else
                {
                    string mystr = WSCore.MakeHex(e.KSN, 0, e.KSN.Length);
                    mystr = mystr.Remove(0, 4); // Based on Kathy's suggestions, removed first 4 "F"s.
                    WSCore.CardTrackDetails.Add("DervdKey", mystr);
                }
                if (WSCore.CardTrackDetails.ContainsKey("PINBlock"))
                {
                    WSCore.CardTrackDetails["PINBlock"] = WSCore.MakeHex(e.EPB, 0, e.EPB.Length);
                }
                else
                {
                    WSCore.CardTrackDetails.Add("PINBlock", WSCore.MakeHex(e.EPB, 0, e.EPB.Length));
                }
                showpopup(new ErrorResponseEventArgs(PGSEMessages.PGDBReqMsgInsert, null));
                sState = SimState.DisplayTransResult;
                pp.Display(TMPINDisplay, DisplayMsg.Processing);
                ClsRequestMessageAttributes myreqmsgattributes = CreateRequestMessage();

                objresponse = WSCore.PrepareforInvokingWS(Trantype, myreqmsgattributes);
                DisplayCommandtoIPAD();
                showpopup(new ErrorResponseEventArgs(setCustomStatusMessage(WSCore.CmdStatus), objresponse));
            }
            else if (e.OpStatus == IPADStatus.UserCancel)
            {
                Cancelled();
                showpopup(new ErrorResponseEventArgs(setCustomStatusMessage(enumCommandStatus.IPADCancel), objresponse));
            }
            else if (e.OpStatus == IPADStatus.KBSecurity)
            {
                objresponse.CmdStatus = enumCommandStatus.IPADCancel.ToString();
                showpopup(new ErrorResponseEventArgs(setCustomStatusMessage(enumCommandStatus.KBSecurity), objresponse));
                sState = SimState.DisplayHandsOff;
                pp.Display(TMPINDisplay, DisplayMsg.HandsOff);
            }
            else
            {
                SimDone();
                objresponse.CmdStatus = enumCommandStatus.SimDone.ToString();
                showpopup(new ErrorResponseEventArgs(setCustomStatusMessage(enumCommandStatus.SimDone), objresponse));
            }

            log.LogMethodExit(null);
        }

        void pp_CardRequestCompleteEvent(object sender, CardRequestCompleteEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            try
            {
                if (e.OpStatus == IPADStatus.OK)
                {
                    if (e.CardStatus == CardStatus.OK)
                    {
                        showpopup(new ErrorResponseEventArgs(PGSEMessages.PGCardReading, null));
                        cd = e.card;
                        WSCore.DisplayCardData(e);
                        pp.SendAmount(AmountType.Debit, (Convert.ToDouble(_amount) + Convert.ToDouble(_Tipamount)).ToString("0.00"));//Modification on 12-Dec-2015:Tip Feature
                        sState = SimState.SelectCreditDebit;
                        pp.GetResponse(TMCardRequest, ResponseMsg.TransactionType, KeyMask.Right | KeyMask.Left, Buzzer.SingleBeep);  // Select credit/debit                       
                    }
                    else  //card error
                    {
                        showpopup(new ErrorResponseEventArgs(PGSEMessages.PGCardReadingError, null));
                        pp.RequestCard(TMCardRequest, CardMsg.PleaseSwipeAgain, Buzzer.DoubleBeep);
                    }
                }
                else if (e.OpStatus == IPADStatus.UserCancel)
                {
                    Cancelled();
                    showpopup(new ErrorResponseEventArgs(setCustomStatusMessage(enumCommandStatus.IPADCancel), objresponse));
                }
                else
                {
                    SimDone();
                    objresponse.CmdStatus = enumCommandStatus.SimDone.ToString();
                    showpopup(new ErrorResponseEventArgs(setCustomStatusMessage(enumCommandStatus.SimDone), objresponse));
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured during Card Request Complete Event", ex);
                
                log.LogMethodExit(null, "Throwing Exception");
                throw ex;
            }
        }

        private ClsRequestMessageAttributes RequestMessageCommonProperties(ref ClsRequestMessageAttributes myreqmsgattributes)
        {
            log.LogMethodEntry(myreqmsgattributes);
            myreqmsgattributes.MerchantID = ConfigurationManager.AppSettings["MerchantID"];
            myreqmsgattributes.Memo = ConfigurationManager.AppSettings["Memo"];
            myreqmsgattributes.Frequency = ConfigurationManager.AppSettings["Frequency"];
            myreqmsgattributes.PartialAuth = ConfigurationManager.AppSettings["PartialAuth"]; ;
            myreqmsgattributes.EncryptedFormat = ConfigurationManager.AppSettings["EncryptedFormat"]; ;
            myreqmsgattributes.AccountSource = AccountSource;//ConfigurationManager.AppSettings["AccountSource"]
            myreqmsgattributes.TerminalName = ConfigurationManager.AppSettings["TerminalName"]; //"Mercury";
            myreqmsgattributes.ShiftID = ConfigurationManager.AppSettings["ShiftID"];

            if (string.IsNullOrEmpty(_Utilities.ParafaitEnv.LoginID))
                myreqmsgattributes.OperatorID = ConfigurationManager.AppSettings["OperatorID"];
            else
                myreqmsgattributes.OperatorID = _Utilities.ParafaitEnv.LoginID;

            log.LogMethodExit(myreqmsgattributes);
            return myreqmsgattributes;
        }

        private ClsRequestMessageAttributes CreateRequestMessage()
        {
            log.LogMethodEntry();

            ClsRequestMessageAttributes myreqmsgattributes = new ClsRequestMessageAttributes();
            try
            {
                if (_tranCode == "Sale")
                {
                    myreqmsgattributes.TranType = Trantype;
                    myreqmsgattributes.RefNo = _refNo;
                    myreqmsgattributes.RecordNo = ConfigurationManager.AppSettings["RecordNumberRequested"];
                    myreqmsgattributes.Purchase = _amount;
                    myreqmsgattributes.TipAmount = _Tipamount;//Modification on 09-Nov-2015:TipFeature
                    WSCore.CmdStatus = enumCommandStatus.NewRequest; //"NewRequest";
                    // Initially for each request this will be the default value. Later depending on the approval
                    // statue message will be changed. There is a lookup table in the DB - tbl_Status.

                }
                else if (_tranCode == "Return")
                {
                    myreqmsgattributes.RecordNo = ConfigurationManager.AppSettings["RecordNumberRequested"];
                    myreqmsgattributes.TranType = Trantype;
                    myreqmsgattributes.RefNo = _refNo;
                    myreqmsgattributes.Purchase = _amount;
                    WSCore.CmdStatus = enumCommandStatus.Return; //"Return";
                    // Initially for each request this will be the default value. Later depending on the approval
                    // statue message will be changed. There is a lookup table in the DB - tbl_Status.

                }
                myreqmsgattributes.InvoiceNo = _invoiceNo;
                myreqmsgattributes.TranCode = _tranCode;
                myreqmsgattributes.EncryptedBlock = WSCore.CardTrackDetails["EncTrack2"];
                myreqmsgattributes.EncryptedKey = WSCore.CardTrackDetails["KSN"];
                RequestMessageCommonProperties(ref myreqmsgattributes);
                if (Trantype == CardType.Debit)
                {
                    myreqmsgattributes.PINBlock = WSCore.CardTrackDetails["PINBlock"];
                    myreqmsgattributes.DervdKey = WSCore.CardTrackDetails["DervdKey"];
                }
            }
            catch(Exception ex)
            {
                log.Error("Error occured while Creating Request Message", ex);
                WSCore.CmdStatus = enumCommandStatus.CardNotRead;

                log.LogMethodExit(null);
                return null;
            }

            log.LogMethodExit(myreqmsgattributes);
            return myreqmsgattributes;
        }

        void DeviceConnect(string path)
        {
            log.LogMethodEntry(path);

            if (path != null)
            {
                pp = new IPAD();
                pp.CardRequestCompleteEvent += new CardRequestCompleteEventHandler(pp_CardRequestCompleteEvent);
                pp.PINRequestCompleteEvent += new PINRequestCompleteEventHandler(pp_PINRequestCompleteEvent);
                pp.GetKeyCompleteEvent += new GetKeyCompleteEventHandler(pp_GetKeyCompleteEvent);

                pp.DisplayCompleteEvent += new DisplayCompleteEventHandler(pp_DisplayCompleteEvent);
                //pp.StateChangedEvent += new DeviceStateChangedEventHandler(pp_StateChangedEvent);
                //pp.SignatureCaptureCompleteEvent += new SignatureCaptureEventHandler(pp_SignatureCaptureCompleteEvent);
            }

            try
            {
                pp.Connect(path);
                pp.GetStatus();
            }
            catch (Exception ex)
            {
                log.Error("Error while cconnecting to the path or getting the status", ex);
                log.LogMethodExit(null, "Throwing Exception"+ex);
                throw ex;
            }
        }

        private void devicerestart()
        {
            log.LogMethodEntry();

            if (devmon != null)
            {
                devmon.Dispose();
                devmon = null;
            }

            if (pp != null)
            {
                pp.Dispose();
                pp = null;

            }

            string p = dv.FindIPAD();
            if (p != null)
            {
                DeviceConnect(p);
                devmon = new DeviceMonitor();
                //devmon.RegisterForEvents(this.Handle);
                devmon.DeviceAttachedEvent += new DeviceAttachedEventHandler(devmon_DeviceAttachedEvent);
                devmon.DeviceRemovedEvent += new DeviceRemovedEventHandler(devmon_DeviceRemovedEvent);
            }

            log.LogMethodExit(null);
        }

        private void Cancelled()
        {
            log.LogMethodEntry();

            try
            {
                objresponse.CmdStatus = enumCommandStatus.IPADCancel.ToString();
                sState = SimState.SimComplete;
                pp.Display(5, DisplayMsg.Cancelled);
            }
            catch (IPADException ex)
            {
                log.Error("Caught a IPADException - ", ex);

                log.LogMethodExit(null, "Throwing Exception"+ex);
                throw ex;
            }

            log.LogMethodExit(null);
        }

        private void SimDone()
        {
            log.LogMethodEntry();

            try
            {
                //objresponse.CmdStatus = enumCommandStatus.SimDone.ToString();
                pp.EndSession();
            }
            catch (IPADException ex)
            {
                log.Error("Caught a IPADException - Error when ending the session");

                log.LogMethodExit(null, "Throwing Exception");
                throw ex;
            }

            log.LogMethodExit(null);
        }

        void pp_StateChangedEvent(object sender, DeviceStateChangeEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if ((e.SessionSts & SessionState.PwrChg) != 0)
                SimDone();

            log.LogMethodExit(null);
        }

        void pp_DisplayCompleteEvent(object sender, DisplayCompleteEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            try
            {
                if (sState == SimState.DisplayThankYou)
                {
                    //DisplayCommandtoIPAD();

                }

                else if (sState == SimState.DisplayTransResult)
                {
                    //pp.Display(TMPINDisplay, DisplayMsg.ThankYou);
                }
                else SimDone();
            }
            catch (Exception ex)
            {
                log.Error("Error occured while displaying Message",ex);
                //throw ex;
            }
        }

        private void DisplayCommandtoIPAD()
        {
            log.LogMethodEntry();

            objresponse.CmdStatus = WSCore.CmdStatus.ToString();
            sState = SimState.SimComplete;
            pp.EndSession();

            switch (WSCore.CmdStatus)
            {
                case enumCommandStatus.Approved:
                    pp.Display(TMPINDisplay, DisplayMsg.Approved);
                    break;
                case enumCommandStatus.Declined:
                    pp.Display(TMPINDisplay, DisplayMsg.Declined);
                    break;
                case enumCommandStatus.Success:
                    pp.Display(TMPINDisplay, DisplayMsg.ThankYou);
                    break;
                case enumCommandStatus.Error:
                    pp.Display(TMPINDisplay, DisplayMsg.Cancelled);
                    break;
                case enumCommandStatus.WSError: // web service or internet not available
                    pp.Display(TMPINDisplay, DisplayMsg.Cancelled);
                    break;
                case enumCommandStatus.DBRequestError:
                    pp.Display(TMPINDisplay, DisplayMsg.Cancelled);
                    break;
                case enumCommandStatus.DBResponseError:
                    pp.Display(TMPINDisplay, DisplayMsg.Cancelled);
                    break;
                case enumCommandStatus.ErrorGettingInvoiceAmounts:
                    pp.Display(TMPINDisplay, DisplayMsg.Cancelled);
                    break;
                case enumCommandStatus.ErrorUpdateStatus:
                    pp.Display(TMPINDisplay, DisplayMsg.Cancelled);
                    break;
                case enumCommandStatus.CardNotRead:
                    pp.Display(TMPINDisplay, DisplayMsg.Cancelled);
                    break;
                default: // not invoked WS, returned CmdStatus null
                    pp.Display(TMPINDisplay, DisplayMsg.Cancelled);
                    break;
            }

            log.LogMethodExit(null);
        }

        private string setCustomStatusMessage(enumCommandStatus statusmessage)
        {
            log.LogMethodEntry(statusmessage);

            string customMessage = "";
            switch (statusmessage)
            {
                case enumCommandStatus.Approved:
                    customMessage = PGSEMessages.PGApproved;
                    break;
                case enumCommandStatus.Declined:
                    customMessage = PGSEMessages.PGDeclined;
                    break;
                case enumCommandStatus.Success:
                    customMessage = PGSEMessages.PGSuccess;
                    break;
                case enumCommandStatus.Error:
                    customMessage = PGSEMessages.PGError;
                    break;
                case enumCommandStatus.WSError:
                    customMessage = PGSEMessages.PGWSError;
                    break;
                case enumCommandStatus.DBRequestError:
                    customMessage = PGSEMessages.PGDBRequestError;
                    break;
                case enumCommandStatus.DBResponseError:
                    customMessage = PGSEMessages.PGDBResponseError;
                    break;
                case enumCommandStatus.DBDuplicateRecord:
                    customMessage = PGSEMessages.PGDBDuplicateInvoice;
                    break;
                case enumCommandStatus.CardNotRead:
                    customMessage = PGSEMessages.PGDBReqMsgNotAttNotset;
                    break;
                case enumCommandStatus.DeviceNotConnected:
                    customMessage = PGSEMessages.PGDeviceNotConnected;
                    break;
                case enumCommandStatus.InValidPrice:
                    customMessage = PGSEMessages.PGValPurAmtformat;
                    break;
                case enumCommandStatus.MissingFields:
                    customMessage = PGSEMessages.PGRequiredFieldsBlank;
                    break;
                case enumCommandStatus.EmptyListVoidSale:
                    customMessage = PGSEMessages.VoidSaleNoTransactionsEmptyList;
                    break;

                case enumCommandStatus.IPADCancel:
                    customMessage = enumCommandStatus.IPADCancel.ToString();
                    break;

                default:
                    customMessage = statusmessage.ToString();// .PGdefault;
                    break;
            }

            log.LogMethodExit(customMessage);
            return customMessage;
        }

        bool initDynamagReader()
        {
            log.LogMethodEntry();

            bool succ = false;
            ThreadStart thr = delegate
            {
                if (magnesafe == null)
                {
                    magnesafe = new ClsDynamagMagnesafe();
                }

                EventHandler CardDataEventHandler = new EventHandler(dynamagCardDataEventHandler);
                succ = magnesafe.Open(CardDataEventHandler);
            };

            Thread thread = new Thread(thr);
            thread.Start();
            thread.Join();

            if (succ)
            {
                isMagnesafe = true;

                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }
        
        void dynamagCardDataEventHandler(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            try
            {
                ClsDynamagMagnesafe.CardDataStateChangedEventArgs ev = e as ClsDynamagMagnesafe.CardDataStateChangedEventArgs;
                WSCore.CardTrackDetails.Clear();
                WSCore.CardTrackDetails.Add("EncTrack2", ev.CardData.m_szTrack2Data);
                WSCore.CardTrackDetails.Add("KSN", ev.CardData.m_szDUKPTKSN);

                ClsRequestMessageAttributes myreqmsgattributes = CreateRequestMessage();
                if (myreqmsgattributes != null)
                {
                    objresponse = WSCore.PrepareforInvokingWS(Trantype, myreqmsgattributes);
                    showpopup(new ErrorResponseEventArgs(setCustomStatusMessage(WSCore.CmdStatus), objresponse));
                    //Begin Modification on 09-Nov--2015 : Void sale if the transaction is partially approved. 
                    if (_Utilities.getParafaitDefaults("ALLOW_PARTIAL_APPROVAL").Equals("N") && Convert.ToDouble(objresponse.Authorize) != (Convert.ToDouble(myreqmsgattributes.Purchase) + Convert.ToDouble(myreqmsgattributes.TipAmount)))
                    {
                        List<ClsRequestMessageAttributes> listVoidSaleReturn = WSCore.GetReturnAndVoidSale(objresponse.ResponseId);
                        if (listVoidSaleReturn.Count != 0)
                        {
                            foreach (ClsRequestMessageAttributes objvoidsale in listVoidSaleReturn)
                            {
                                ClsRequestMessageAttributes myrvoidsalemsgattributes = new ClsRequestMessageAttributes();
                                myrvoidsalemsgattributes.TranCode = ConfigurationManager.AppSettings["TranCode"]; // myreqmsgattributes.TranCode;
                                myrvoidsalemsgattributes.InvoiceNo = objvoidsale.InvoiceNo;
                                myrvoidsalemsgattributes.TranType = (CardType)Enum.Parse(typeof(CardType), "Credit");//objvoidsale.TranType;
                                myrvoidsalemsgattributes.RefNo = objvoidsale.RefNo;
                                myrvoidsalemsgattributes.RecordNo = objvoidsale.RecordNo;
                                myrvoidsalemsgattributes.Purchase = objvoidsale.Purchase;
                                myrvoidsalemsgattributes.AuthCode = objvoidsale.AuthCode;
                                myrvoidsalemsgattributes.AcqRefData = objvoidsale.AcqRefData;
                                myrvoidsalemsgattributes.ProcessData = objvoidsale.ProcessData;
                                RequestMessageCommonProperties(ref myrvoidsalemsgattributes);
                                try
                                {
                                    string mystring = string.Format(PGSEMessages.MsgCustomCardSwipeVoidSale, objvoidsale.AcctNo);
                                    showpopup(new ErrorResponseEventArgs(mystring, null));
                                    objresponse = WSCore.VoidSaleInvoice(myrvoidsalemsgattributes, true);
                                    if (objresponse.CmdStatus.Equals(enumCommandStatus.Approved.ToString()) == false)
                                        objresponse = WSCore.VoidSaleInvoice(myrvoidsalemsgattributes, false);
                                    if (objresponse.CmdStatus.Equals(enumCommandStatus.Approved.ToString()))
                                    {
                                        objresponse.CmdStatus = enumCommandStatus.Declined.ToString();
                                        objresponse.TextResponse = PGSEMessages.PartialApprovalBlocked;
                                    }
                                    else
                                    {
                                        objresponse.CmdStatus = enumCommandStatus.Error.ToString();
                                        objresponse.TextResponse = PGSEMessages.PartiallyApproved;
                                    }
                                    showpopup(new ErrorResponseEventArgs(setCustomStatusMessage(enumCommandStatus.Declined), objresponse));
                                }
                                catch (Exception ex)
                                {
                                    log.Error("Error occured while processing", ex);
                                    objresponse.CmdStatus = enumCommandStatus.IPADCancel.ToString();
                                    showpopup(new ErrorResponseEventArgs(ex.ToString(), objresponse));
                                }
                            }
                        }
                    }//Ends Modification on 09-Nov-2015 
                }
                else
                {
                    objresponse.CmdStatus = enumCommandStatus.Error.ToString();
                    showpopup(new ErrorResponseEventArgs(PGSEMessages.PGDBReqMsgNotAttNotset, objresponse));
                }
            }
            catch (Exception Ex)
            {
                log.Error("Error occured while processing", Ex);
                objresponse.CmdStatus = enumCommandStatus.Error.ToString();
                showpopup(new ErrorResponseEventArgs(Ex.Message, objresponse));
            }

            log.LogMethodExit(null);
        }
    }
}
