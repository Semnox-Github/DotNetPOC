/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - Kioks Helper class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.3.0       05-Jun-2018      Archana/Guru S A     Created 
 *2.4.0       03-Sep-2018      Guru S A             Modified for device container changes
 *2.8.0       21-Oct-2019      Girish Kundar        Modified for ticket station enhancement
 *2.130.7.0   26-Apr-2022       Guru S A             Enable remote debug option    
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Data;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Device;
using System.Printing;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.KioskCore;

namespace Redemption_Kiosk
{
    /// <summary>
    /// RedemptionKioskHelper class
    /// </summary>
    public class RedemptionKioskHelper
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static bool remoteDebugIsEnabled = false;
        private static bool initialLoadIsDone = false;       

        /// <summary>
        /// ReverseTopupCardNumber method
        /// </summary>
        /// <param name="cardNumber">cardNumber which need to be reversed</param>
        /// <returns>Reversed CardNumber</returns>
        public static string ReverseTopupCardNumber(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            if (Common.utils.getParafaitDefaults("REVERSE_KIOSK_TOPUP_CARD_NUMBER").Equals("N"))
            {
                log.LogMethodExit(cardNumber);
                return cardNumber;
            }
            else
            {
                try
                {
                    char[] arr = cardNumber.ToCharArray();

                    for (int i = 0; i < cardNumber.Length / 2; i += 2)
                    {
                        char x = arr[i];
                        char y = arr[i + 1];

                        arr[i] = arr[cardNumber.Length - i - 2];
                        arr[i + 1] = arr[cardNumber.Length - i - 1];

                        arr[cardNumber.Length - i - 2] = x;
                        arr[cardNumber.Length - i - 1] = y;
                    }
                    log.LogMethodExit(arr);
                    return new string(arr);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(cardNumber);
                    return cardNumber;
                }
            }
        }
        /// <summary>
        /// HandleCardRead method 
        /// </summary>
        /// <param name="inCardNumber"></param>
        /// <param name="readerDevice"></param>
        /// <returns></returns>
        internal static Card HandleCardRead(string inCardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(inCardNumber, readerDevice);
            Card card = null;
            if (readerDevice != null)
            {
                card = new Card(readerDevice, inCardNumber, Common.utils.ParafaitEnv.POSMachine, Common.utils);
            }
            else
            {
                card = new Card(inCardNumber, Common.utils.ParafaitEnv.LoginID, Common.utils);
            }

            //string message = "";
            Application.DoEvents();

            try
            {
                card = RefreshCardFromHQ(card);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null);
                return null;
            }

            if (card.CardStatus == "NEW")
            {
                Common.ShowMessage(Common.utils.MessageUtils.getMessage(459));
                log.LogMethodExit(null, Common.utils.MessageUtils.getMessage(459));
                return null;
            }
            else if (card.technician_card == 'Y') 
            {
                if (Application.OpenForms[Application.OpenForms.Count - 1].Name == "frmRedemptionKioskHomeScreen" || Application.OpenForms[Application.OpenForms.Count - 1].Name == "FrmRedemptionKioskAdmin")
                { 
                    throw new ValidationException(card.CardNumber,card.card_id,"Technician card has been taped");
                }
                else
                {
                    Common.ShowMessage(Common.utils.MessageUtils.getMessage(197, card.CardNumber));
                    log.LogMethodExit(null, Common.utils.MessageUtils.getMessage(197, card.CardNumber));
                    return null;
                }
            }
            log.LogMethodExit(card);
            return card;

        }
        internal static RemotingClient CardRoamingRemotingClient = null;

        /// <summary>
        /// Method to Refresh card from HQ
        /// </summary>
        /// <param name="CurrentCard"></param>
        /// <returns>Card object</returns>
        public static Card RefreshCardFromHQ(Card CurrentCard)
        {
            log.LogMethodEntry(CurrentCard);
            string message = "";
            try
            {
                if (CardRoamingRemotingClient == null)
                {
                    if (Common.utils.ParafaitEnv.ALLOW_ROAMING_CARDS == "Y" && Common.utils.ParafaitEnv.ENABLE_ON_DEMAND_ROAMING == "Y")
                    {
                        try
                        {
                            CardRoamingRemotingClient = new RemotingClient();
                            log.Info("Remoting client initialized");
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            throw new Exception(ex.Message);
                        }
                    }
                }
                CardUtils cardUtils = new CardUtils(Common.utils);
                if (cardUtils.getCardFromHQ(CardRoamingRemotingClient, ref CurrentCard, ref message))
                {
                    log.LogMethodExit(CurrentCard);
                    return CurrentCard;
                }
                else
                {
                    log.Error(message);
                    throw new Exception(message);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (ex.Message.ToLower().Contains("fault"))
                {
                    message = Common.utils.MessageUtils.getMessage(216);
                    try
                    {
                        CardRoamingRemotingClient = new RemotingClient();
                    }
                    catch (Exception exe)
                    {
                        log.Error(exe);
                        message = Common.utils.MessageUtils.getMessage(217);
                        throw new Exception(message);
                    }
                }
                else
                {
                    message = Common.utils.MessageUtils.getMessage(1604, ex.Message); //'On-Demand Roaming
                }
                log.LogMethodExit(CurrentCard, "Exception occured: " + message);
                throw new Exception(message);
                //return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="redemptionOrder"></param>
        /// <returns></returns>
        internal static TicketReceipt ProcessBarcode(string barCode, RedemptionBL redemptionOrder)
        {
            log.LogMethodEntry(barCode, redemptionOrder);
            if (!TicketReceipt.IsTicketFromCurrentSite(barCode))
            {
                Common.ShowMessage(Common.utils.MessageUtils.getMessage(1605));
                //"Ticket receipt is not from the current site"
                return null;
            }
            TicketStationFactory ticketStationFactory = new TicketStationFactory();
            TicketStationBL ticketStation = ticketStationFactory.GetTicketStationObject(barCode);
            TicketReceipt ticketReceipt = null;
            if (ticketStation != null)
            {
                if (ticketStation.ValidCheckBit(barCode))
                {
                    try
                    {
                        ticketReceipt = ticketStation.GetTicketStationRecepit(barCode);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        Common.ShowMessage(ex.Message);
                    }
                }
                else
                {
                    Common.ShowMessage(Common.utils.MessageUtils.getMessage(115));
                }
            }
            else
            {
                log.Error(Common.utils.MessageUtils.getMessage(2321));
                Common.ShowMessage(Common.utils.MessageUtils.getMessage(2321));
            }
            log.LogMethodExit(ticketReceipt);
            return ticketReceipt;
        }
        /// <summary>
        /// This method is to set the maximum value for vertical scroll bar.
        /// </summary>
        /// <param name="flp">FlowLayoutControl from the form</param>
        /// <param name="Number_Of_Rows">The No of rows the flowlayout control is displaying.</param>
        public static void InitializeFlowLayoutHorizontalScroll(FlowLayoutPanel flp, int Number_Of_Rows, int controlCount)
        {
            log.LogMethodEntry(flp, Number_Of_Rows, controlCount);
            if (flp.Controls.Count > 0)
            {
                flp.VerticalScroll.Maximum = (Math.Max(controlCount / 3, Number_Of_Rows) * (flp.Controls[0].Height + flp.Controls[0].Margin.Top + flp.Controls[0].Margin.Bottom) + 1);
                flp.VerticalScroll.SmallChange = flp.Controls[0].Height + flp.Controls[0].Margin.Top + flp.Controls[0].Margin.Bottom;
                flp.VerticalScroll.LargeChange = (flp.Controls[0].Height + flp.Controls[0].Margin.Top + flp.Controls[0].Margin.Bottom) * 3;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method can be used for scroll left event in flowlayout Panel
        /// </summary>
        /// <param name="flp">FlowLayoutControl from the form</param>
        /// <param name="index">Variable value which will decreament in each Call of this function and passed as reference</param>
        public static int FlowLayoutScrollLeft(FlowLayoutPanel flp, int index)
        {
            log.LogMethodEntry(flp, index);
            if (flp.Controls.Count > (flp.Height / (flp.Controls[0].Height + flp.Controls[0].Margin.Top + flp.Controls[0].Margin.Bottom)))
            {
                index -= flp.VerticalScroll.LargeChange;
                if (index <= (flp.VerticalScroll.SmallChange +1))
                {
                    index = 1;
                }

                flp.VerticalScroll.Value = index;
                flp.Refresh();
            }
            log.LogMethodExit(index);
            return index;
        }
        /// <summary>
        /// This method can be used for scroll right event in flowlayout Panel
        /// </summary>
        /// <param name="flp">FlowLayoutControl from the form</param>
        /// <param name="index">Variable value which will increament in each Call of this function and passed as reference</param>
       public static int FlowLayoutScrollRight(FlowLayoutPanel flp, int index)
        {
            log.LogMethodEntry(flp, index);
            int count = flp.Controls.Count;
            if (count > 0 & count < 4)
            {
                count = 4;
            }
            if (count != 0 && count > (flp.Height / (flp.Controls[0].Height + flp.Controls[0].Margin.Top + flp.Controls[0].Margin.Bottom)))
            {
                if (index <= 0)
                {
                    index = 1;
                }
                else
                {
                    index += flp.VerticalScroll.LargeChange;
                }
                if (index > (flp.VerticalScroll.Maximum - flp.VerticalScroll.LargeChange))
                {
                    index = flp.VerticalScroll.Maximum- flp.VerticalScroll.Minimum;
                }

                flp.VerticalScroll.Value = index;
                flp.Refresh();

            }
            log.LogMethodExit(index);
            return index;
        }
        ///// <summary>
        ///// This method is used to Register barcode scanner
        ///// </summary>
        ///// <param name="ownerForm">The form in which scanner has to be registered </param>
        ///// <param name="barCodeScanCompleteEventHandle">barcode scan event</param>
        ///// <returns>Device class object</returns>
        //public static DeviceClass RegisterBarcodeScanner(Form ownerForm, EventHandler barCodeScanCompleteEventHandle)
        //{
        //    log.LogMethodEntry(ownerForm.Name, barCodeScanCompleteEventHandle);
        //    try
        //    {
        //        DeviceClass barcodeScannerDevice;
        //        string USBReaderVID = Common.utils.getParafaitDefaults("USB_BARCODE_READER_VID");
        //        string USBReaderPID = Common.utils.getParafaitDefaults("USB_BARCODE_READER_PID");
        //        string USBReaderOptionalString = Common.utils.getParafaitDefaults("USB_BARCODE_READER_OPT_STRING");

        //        USBDevice barcodeListener;
        //        if (IntPtr.Size == 4) //32 bit
        //        {
        //            barcodeListener = new KeyboardWedge32();
        //        }
        //        else
        //        {
        //            barcodeListener = new KeyboardWedge64();
        //        }

        //        foreach (string optString in USBReaderOptionalString.Split('|'))
        //        {
        //            if (string.IsNullOrEmpty(optString.Trim()))
        //            {
        //                continue;
        //            }

        //            bool flag = barcodeListener.InitializeUSBReader(ownerForm, USBReaderVID, USBReaderPID, optString.Trim());
        //            if (barcodeListener.isOpen)
        //            {
        //                barcodeListener.Register(barCodeScanCompleteEventHandle);
        //                barcodeScannerDevice = barcodeListener;
        //                log.LogMethodExit(barcodeScannerDevice);
        //                return barcodeScannerDevice;
        //            }
        //        }

        //        string mes = Common.utils.MessageUtils.getMessage(1634, USBReaderVID, USBReaderPID, USBReaderOptionalString, Common.utils.ParafaitEnv.POSMachine); //"Unable to find USB Barcode reader scanner. VID: " + USBReaderVID + ", PID: " + USBReaderPID + ", OPT: " + USBReaderOptionalString + " POS: " + Common.utils.ParafaitEnv.POSMachine;
        //        Common.ShowMessage(mes + ". " + Common.utils.MessageUtils.getMessage(441));
        //        log.Debug(mes);
        //        log.LogMethodExit(null);
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// This method is to register card reader.
        ///// </summary>
        ///// <param name="ownerForm">The form in which scanner has to be registered</param>
        ///// <param name="cardScanCompleteEventHandle">Card scan event</param>
        ///// <returns>Device class object</returns>
        //public static DeviceClass RegisterCardReader(Form ownerForm, EventHandler cardScanCompleteEventHandle)
        //{
        //    int deviceAddress = 0;
        //    bool response = false;


        //    DeviceClass mifareReader = null;
        //    response = true;

        //    try
        //    {
        //        mifareReader = new ACR1222L(deviceAddress);
        //        mifareReader.ClearDisplay();
        //        mifareReader.DisplayMessage(Common.utils.ParafaitEnv.SiteName, Common.utils.MessageUtils.getMessage(257));
        //        log.Debug("ACR1222L is the card reader");
        //    }
        //    catch
        //    {
        //        try
        //        {
        //            string serialNumber = Common.utils.getParafaitDefaults("CARD_READER_SERIAL_NUMBER").Trim();
        //            if (string.IsNullOrEmpty(serialNumber))
        //            {
        //                mifareReader = new ACR1252U(deviceAddress);
        //                log.Debug("ACR1252U is the card reader");
        //            }
        //            else
        //            {
        //                mifareReader = new ACR1252U(serialNumber.Split('|')[0]);
        //                log.Debug("ACR1252U is the card reader with serial string " + serialNumber);
        //            }
        //        }
        //        catch
        //        {
        //            try
        //            {
        //                mifareReader = new ACR122U(deviceAddress);
        //                log.Debug("ACR122U is the card reader");
        //            }
        //            catch
        //            {
        //                try
        //                {
        //                    mifareReader = new MIBlack(deviceAddress);
        //                    log.Debug("MIBlack is the card reader");
        //                }
        //                catch
        //                {
        //                    response = false;
        //                }
        //            }
        //        }
        //    }
        //    log.Debug("Response value is " + response.ToString());
        //    if (response)
        //    {
        //        mifareReader.Register(new EventHandler(cardScanCompleteEventHandle));
        //        log.Debug("Device registered ");
        //    }
        //    else
        //    {
        //        mifareReader = RegisterUSBCardReader(ownerForm, cardScanCompleteEventHandle);
        //    }

        //    Common.utils.getMifareCustomerKey();
        //    log.LogMethodExit(mifareReader);
        //    return mifareReader;
        //}
        ///// <summary>
        ///// This method is to register USB card reader
        ///// </summary>
        ///// <param name="ownerForm"></param>
        ///// <param name="cardScanCompleteEventHandle"></param>
        ///// <returns>Device class object</returns>
        //public static DeviceClass RegisterUSBCardReader(Form ownerForm, EventHandler cardScanCompleteEventHandle)
        //{
        //    log.LogMethodEntry(ownerForm.Name, ownerForm);
        //    string USBReaderVID = Common.utils.getParafaitDefaults("USB_READER_VID");
        //    string USBReaderPID = Common.utils.getParafaitDefaults("USB_READER_PID");
        //    string USBReaderOptionalString = Common.utils.getParafaitDefaults("USB_READER_OPT_STRING");
        //    DeviceClass cardReaderDevice;

        //    USBDevice cardReaderListener;
        //    if (IntPtr.Size == 4) //32 bit
        //    {
        //        cardReaderListener = new KeyboardWedge32();
        //    }
        //    else
        //    {
        //        cardReaderListener = new KeyboardWedge64();
        //    }

        //    foreach (string optString in USBReaderOptionalString.Split('|'))
        //    {
        //        if (string.IsNullOrEmpty(optString.Trim()))
        //            continue;

        //        bool flag = cardReaderListener.InitializeUSBReader(ownerForm, USBReaderVID, USBReaderPID, optString.Trim());
        //        if (cardReaderListener.isOpen)
        //        {
        //            cardReaderListener.Register(cardScanCompleteEventHandle);
        //            cardReaderDevice = cardReaderListener;
        //            log.LogMethodExit(cardReaderDevice);
        //            return cardReaderDevice;
        //        }
        //    }
        //    string mes = Common.utils.MessageUtils.getMessage(1621, USBReaderVID, USBReaderPID, USBReaderOptionalString, Common.utils.ParafaitEnv.POSMachine); //"Unable to find USB card reader for Top-up. VID: " + USBReaderVID + ", PID: " + USBReaderPID + ", OPT: " + USBReaderOptionalString + " POS: " + Common.utils.ParafaitEnv.POSMachine;
        //    Common.ShowMessage(mes + ". " + Common.utils.MessageUtils.getMessage(441));

        //    log.LogMethodExit(null);
        //    return null;
        //}

        internal static DeviceClass RegisterCardReader(Form ownerForm, EventHandler cardScanCompleteEventHandle)
        {
            log.LogMethodEntry();
            DeviceClass cardReaderDevice = null;
            try
            {

                cardReaderDevice = DeviceContainer.RegisterCardReader(Common.utils.ExecutionContext, ownerForm, cardScanCompleteEventHandle);
                Common.utils.getMifareCustomerKey();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Common.ShowMessage(ex.Message + Common.utils.MessageUtils.getMessage(441));
            }
            log.LogMethodExit(cardReaderDevice);
            return cardReaderDevice;
        }

        internal static DeviceClass RegisterBarCodeScanner(Form ownerForm, EventHandler barCodeScanCompleteEventHandle)
        {
            log.LogMethodEntry();
            DeviceClass barcodeScannerDevice = null;
            try
            {
                barcodeScannerDevice = DeviceContainer.RegisterBarcodeScanner(Common.utils.ExecutionContext, ownerForm, barCodeScanCompleteEventHandle);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Common.ShowMessage(ex.Message + Common.utils.MessageUtils.getMessage(441));
            }
            log.LogMethodExit(barcodeScannerDevice);
            return barcodeScannerDevice;
        }

        public static bool RemoteDebugIsEnabled { get { return remoteDebugIsEnabled; } set { remoteDebugIsEnabled = value; } }

        public static void SetRemoteDebugIsEnabled(ExecutionContext machineUserContext)
        {
            log.LogMethodEntry(machineUserContext);
            log.LogVariableState("initialLoadIsDone", initialLoadIsDone);
            if (initialLoadIsDone == false)
            {
                remoteDebugIsEnabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(machineUserContext, "ENABLE_REMOTE_DEBUG", false);
                initialLoadIsDone = true;
            }
            log.LogVariableState("initialLoadIsDone", initialLoadIsDone);
            log.LogVariableState("remoteDebugIsEnabled", remoteDebugIsEnabled);
            log.LogMethodExit();
        }
    }
}
