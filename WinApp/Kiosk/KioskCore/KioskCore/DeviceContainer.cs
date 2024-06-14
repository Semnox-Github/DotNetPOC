/********************************************************************************************
 * Project Name - KioskCore - DeviceContainer 
 * Description  - DeviceContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.4.0       05-Sep-2018      Guru S A           Created 
 *2.110       21-Dec-2020      Jinto Thomas       Modified: As part of WristBand printer changes
 *2.120       17-Apr-2021      Guru S A           Wristband printing flow enhancements 
 *2.80.7      21-Feb-2022      Guru S A           ACR reader performance fix 
 *2.140.2     24-Feb-2022      Girish Kundar      Modified : BOCA RFID printer changes 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Device;
using Semnox.Parafait.POS;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Site;
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Parafait.Device.Printer.WristBandPrinters;

namespace Semnox.Parafait.KioskCore
{
    /// <summary>
    /// Common container class for device registration methods
    /// </summary>
    public class DeviceContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Method to check the printerStatus
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="printer">Printer object</param>
        public static void PrinterStatus(ExecutionContext executionContext, PrinterDTO printer = null)
        {
            log.LogMethodEntry(executionContext, printer);
            string message = "";
            //This method will checks and retuns the printer status and status text will be placed in ref variable.
            try
            {
                PrinterSettings settings = new PrinterSettings();
                PrintServer printServer;
                if (printer != null)
                {
                    settings.PrinterName = (String.IsNullOrEmpty(printer.PrinterLocation) ? printer.PrinterName : printer.PrinterLocation);
                    //printServer = new PrintServer(printer.PrinterLocation);
                }
                printServer = new PrintServer();

                PrintQueueCollection printQueues = null;

                printQueues = printServer.GetPrintQueues(new[] { EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections });
                foreach (PrintQueue printQueue in printQueues)
                {
                    if (settings.PrinterName == printQueue.FullName)
                    {
                        log.Debug("printQueue.FullName : " + printQueue.FullName);
                        //Checking for all the possible status.
                        if (printQueue.IsOutOfPaper ||
                            printQueue.IsOffline ||
                            printQueue.IsNotAvailable || printQueue.IsPaperJammed ||
                            printQueue.IsOutOfMemory || printQueue.IsInError)
                        {
                            message = printQueue.QueueStatus.ToString();
                            printServer.Dispose();
                            printQueues.Dispose();
                            printQueue.Dispose();
                            log.Error(message);
                            throw new Exception(message);
                            //return false;
                        }
                        else if (printQueue.HasPaperProblem ||  !printQueue.HasToner || printQueue.IsBusy
                            || printQueue.IsDoorOpened || printQueue.IsInitializing || printQueue.IsTonerLow)
                        {
                            message = printQueue.QueueStatus.ToString();
                            printServer.Dispose();
                            printQueues.Dispose();
                            printQueue.Dispose();
                            log.Error(message);
                            break; //Just log error dont throw exception 
                        }
                        else
                        {
                            log.Debug("No issues with Printer");
                            printServer.Dispose();
                            printQueues.Dispose();
                            printQueue.Dispose();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 1603));
                //"Printer error.Please contact our staff"
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method is used to Register barcode scanner
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="ownerForm">The form in which scanner has to be registered </param>
        /// <param name="barCodeScanCompleteEventHandle">barcode scan event</param>
        /// <returns>Device class object</returns>
        public static DeviceClass RegisterBarcodeScanner(ExecutionContext executionContext, Form ownerForm, EventHandler barCodeScanCompleteEventHandle)
        {
            log.LogMethodEntry(executionContext, ownerForm.Name, barCodeScanCompleteEventHandle);
            string USBReaderVID = "";
            string USBReaderPID = "";
            string USBReaderOptionalString = "";

            DeviceClass barcodeScannerDevice;
            USBReaderVID = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "USB_BARCODE_READER_VID");
            USBReaderPID = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "USB_BARCODE_READER_PID");
            USBReaderOptionalString = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "USB_BARCODE_READER_OPT_STRING");
            log.LogVariableState("USBReaderVID :", USBReaderVID);
            log.LogVariableState("USBReaderPID :", USBReaderPID);
            log.LogVariableState("USBReaderOptionalString :", USBReaderOptionalString);
            USBDevice barcodeListener;
            if (IntPtr.Size == 4) //32 bit
            {
                barcodeListener = new KeyboardWedge32();
            }
            else
            {
                barcodeListener = new KeyboardWedge64();
            }

            foreach (string optString in USBReaderOptionalString.Split('|'))
            {
                if (string.IsNullOrEmpty(optString.Trim()))
                {
                    continue;
                }

                bool flag = barcodeListener.InitializeUSBReader(ownerForm, USBReaderVID, USBReaderPID, optString.Trim());
                if (barcodeListener.isOpen)
                {
                    barcodeListener.Register(barCodeScanCompleteEventHandle);
                    barcodeScannerDevice = barcodeListener;
                    log.LogMethodExit(barcodeScannerDevice);
                    return barcodeScannerDevice;
                }
            }

            string errorMessage = MessageContainerList.GetMessage(executionContext, 1634, USBReaderVID, USBReaderPID, USBReaderOptionalString, GetPosMachineName(executionContext));
            log.Error(errorMessage);
            throw new Exception(errorMessage);
        }

        /// <summary>
        /// This method is to register card reader.
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="ownerForm">The form in which scanner has to be registered</param>
        /// <param name="cardScanCompleteEventHandle">Card scan event</param>
        /// <returns>Device class object</returns>
        public static DeviceClass RegisterCardReader(ExecutionContext executionContext, Form ownerForm, EventHandler cardScanCompleteEventHandle)
        {
            log.LogMethodEntry(executionContext, ownerForm.Name, cardScanCompleteEventHandle);
            int deviceAddress = 0;
            bool response = false;

            DeviceClass mifareReader = null;
            response = true;

            try
            {
                mifareReader = new ACR1222L(deviceAddress);
                mifareReader.ClearDisplay();
                SiteList siteList = new SiteList(executionContext);
                List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchParam = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                searchParam.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<SiteDTO> siteDTOList = siteList.GetAllSites(searchParam);
                string siteName = "";
                if (siteDTOList != null && siteDTOList.Count > 0)
                {
                    siteName = siteDTOList[0].SiteName;
                }
                mifareReader.DisplayMessage(siteName, MessageContainerList.GetMessage(executionContext, 257));
                log.Debug("ACR1222L is the card reader");
            }
            catch
            {
                try
                {
                    string serialNumber = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CARD_READER_SERIAL_NUMBER").Trim();
                    if (string.IsNullOrEmpty(serialNumber))
                    {
                        mifareReader = new ACR1252U(deviceAddress);
                        log.Debug("ACR1252U is the card reader");
                    }
                    else
                    {
                        mifareReader = new ACR1252U(serialNumber.Split('|')[0]);
                        log.Debug("ACR1252U is the card reader with serial string " + serialNumber);
                    }
                }
                catch
                {
                    try
                    {
                        mifareReader = new ACR122U(deviceAddress);
                        log.Debug("ACR122U is the card reader");
                    }
                    catch
                    {
                        try
                        {
                            mifareReader = new MIBlack(deviceAddress);
                            log.Debug("MIBlack is the card reader");
                        }
                        catch
                        {
                            response = false;
                        }
                    }
                }
            }
            log.Debug("Response value is " + response.ToString());
            if (response)
            {
                mifareReader.Register(new EventHandler(cardScanCompleteEventHandle));
                log.Debug("Device registered ");
            }
            else
            {
                mifareReader = RegisterUSBCardReader(executionContext, ownerForm, cardScanCompleteEventHandle);
            }

            //Common.utils.getMifareCustomerKey();
            log.LogMethodExit(mifareReader);
            return mifareReader;
        }

        /// <summary>
        /// This method is to register USB card reader
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="ownerForm"></param>
        /// <param name="cardScanCompleteEventHandle"></param>
        /// <returns>Device class object</returns>
        public static DeviceClass RegisterUSBCardReader(ExecutionContext executionContext, Form ownerForm, EventHandler cardScanCompleteEventHandle)
        {
            log.LogMethodEntry(executionContext, ownerForm.Name, ownerForm);
            string USBReaderVID = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "USB_READER_VID");
            string USBReaderPID = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "USB_READER_PID");
            string USBReaderOptionalString = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "USB_READER_OPT_STRING");
            log.LogVariableState("USBReaderVID :", USBReaderVID);
            log.LogVariableState("USBReaderPID :", USBReaderPID);
            log.LogVariableState("USBReaderOptionalString :", USBReaderOptionalString);
            DeviceClass cardReaderDevice;

            USBDevice cardReaderListener;
            if (IntPtr.Size == 4) //32 bit
            {
                cardReaderListener = new KeyboardWedge32();
            }
            else
            {
                cardReaderListener = new KeyboardWedge64();
            }

            foreach (string optString in USBReaderOptionalString.Split('|'))
            {
                if (string.IsNullOrEmpty(optString.Trim()))
                    continue;

                bool flag = cardReaderListener.InitializeUSBReader(ownerForm, USBReaderVID, USBReaderPID, optString.Trim());
                if (cardReaderListener.isOpen)
                {
                    cardReaderListener.Register(cardScanCompleteEventHandle);
                    cardReaderDevice = cardReaderListener;
                    log.LogMethodExit(cardReaderDevice);
                    return cardReaderDevice;
                }
            }

            string mes = MessageContainerList.GetMessage(executionContext, 1621, USBReaderVID, USBReaderPID, USBReaderOptionalString, GetPosMachineName(executionContext));
            log.Error(mes);
            throw new Exception(mes);
        }
        /// <summary>
        /// This method is gets the PosName details for the machine linked with execution context 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param> 
        /// <returns>pos machine name</returns>
        public static string GetPosMachineName(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            POSMachineList posMachinesList = new POSMachineList(executionContext);
            int posMachineId = executionContext.GetMachineId();
            string posMachineName = "";
            if (posMachineId > -1)
            {
                posMachineName = (posMachinesList.GetPOSMachine(posMachineId) != null ? posMachinesList.GetPOSMachine(posMachineId).POSName : "");
            }
            log.LogMethodExit(posMachineName);
            return posMachineName;
        }

        /// <summary>
        /// Delegate to pass the message to UI
        /// </summary>
        /// <param name="message">string</param> 
        public delegate void ReturnMessageToUI(string message);
        public static ReturnMessageToUI returnMessageToUI;

        /// <summary>
        /// This method is gets the PosName details for the machine linked with execution context 
        /// </summary> 
        /// <returns></returns>
        public static void InitializeSerialPorts()
        {
            log.LogMethodEntry();
            //KioskStatic.spCoinAcceptor = new System.IO.Ports.SerialPort();

            System.Threading.ManualResetEvent mre = new System.Threading.ManualResetEvent(false);
            frmLogMessage logMessage = new frmLogMessage(mre)
            {
                IgnoreAndContinue = false
            };

            if (KioskStatic.config.dispport > 0)
            {
                System.IO.Ports.SerialPort spCardDispenser = new System.IO.Ports.SerialPort
                {
                    PortName = "COM" + KioskStatic.config.dispport.ToString(),
                    BaudRate = 9600
                };

                //if (KioskStatic.CardDispenserModel.Equals(CardDispenser.CardDispenser.Models.CVD))
                //    spCardDispenser.Parity = System.IO.Ports.Parity.Even;
                //else
                spCardDispenser.Parity = System.IO.Ports.Parity.None;
                spCardDispenser.StopBits = System.IO.Ports.StopBits.One;
                spCardDispenser.DataBits = 8;
                while (!logMessage.IgnoreAndContinue)
                {
                    try
                    {
                        if (!spCardDispenser.IsOpen)
                        {
                            spCardDispenser.Open();
                            KioskStatic.logToFile("Card dispenser port " + KioskStatic.config.dispport.ToString() + " opened");
                            log.LogVariableState("Card dispenser port " + KioskStatic.config.dispport.ToString() + " opened", KioskStatic.config.dispport);
                        }
                        if (spCardDispenser.IsOpen)
                        {
                            spCardDispenser.Close();
                            KioskStatic.logToFile("Card dispenser port " + KioskStatic.config.dispport.ToString() + " Closed");
                            log.LogVariableState("Card dispenser port " + KioskStatic.config.dispport.ToString() + " Closed", KioskStatic.config.dispport);
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        returnMessageToUI(ex.Message);
                        logMessage.AppendLog("Card Dispenser: " + ex.Message);
                        if (!logMessage.Visible)
                            logMessage.Show();
                        Application.DoEvents();
                        mre.WaitOne(2000);
                    }
                }
                logMessage.Hide();
                logMessage.IgnoreAndContinue = false;
                logMessage.ClearLog();
                mre.Reset();
            }


            if (KioskStatic.config.prport == -1)
            {
                KioskStatic.isUSBPrinter = true;
                KioskStatic.logToFile("USB Printer. Port set to -1 in setup");
                log.LogVariableState("USB Printer. Port set to -1 in setup", KioskStatic.isUSBPrinter);
            }
            else
            {
                KioskStatic.receipt = false;
                KioskStatic.logToFile("No Printer. Port set to 0 in setup");
                log.LogVariableState("No Printer. Port set to 0 in setup", KioskStatic.receipt);
            }

            if (KioskStatic.BillAcceptorModel.Equals(BillAcceptor.BillAcceptor.Models.NV9USB))
            {
                KioskStatic.logToFile("Bill Acceptor is NV9USB");
                log.LogVariableState("Bill Acceptor is NV9USB", KioskStatic.BillAcceptorModel);
            }
            else if (KioskStatic.config.baport > 0)
            {
                System.IO.Ports.SerialPort spBillAcceptor = new System.IO.Ports.SerialPort();
                spBillAcceptor.PortName = "COM" + KioskStatic.config.baport.ToString();
                spBillAcceptor.BaudRate = 9600;
                spBillAcceptor.DataBits = 8;
                spBillAcceptor.StopBits = System.IO.Ports.StopBits.One;
                spBillAcceptor.Parity = System.IO.Ports.Parity.Even;
                spBillAcceptor.RtsEnable = true;
                spBillAcceptor.WriteTimeout = spBillAcceptor.ReadTimeout = 1000;
                while (!logMessage.IgnoreAndContinue)
                {
                    try
                    {
                        try
                        {
                            if (!spBillAcceptor.IsOpen)
                            {
                                spBillAcceptor.Open();
                                KioskStatic.logToFile("Bill acceptor port " + KioskStatic.config.baport.ToString() + " opened");
                                log.LogVariableState("Bill acceptor port " + KioskStatic.config.baport.ToString() + " opened", KioskStatic.config.baport);
                            }
                            if (spBillAcceptor.IsOpen)
                            {
                                spBillAcceptor.Close();
                            }
                            break;
                        }
                        catch (Exception ex)
                        {
                            KioskStatic.logToFile("Error Opening Bill acceptor port " + KioskStatic.config.baport.ToString());
                            log.Error("Error Opening Bill acceptor port ", ex);
                            throw new Exception(ex.Message.ToString());
                        }
                        finally
                        {
                            if (spBillAcceptor.IsOpen)
                            {
                                spBillAcceptor.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        logMessage.AppendLog("Bill Acceptor: " + ex.Message);
                        if (!logMessage.Visible)
                            logMessage.Show();
                        Application.DoEvents();
                        mre.WaitOne(2000);
                    }
                }

                logMessage.Hide();
                logMessage.IgnoreAndContinue = false;
                logMessage.ClearLog();
                mre.Reset();
            }

            if (KioskStatic.CoinAcceptorModel.Equals(CoinAcceptor.CoinAcceptor.Models.MICROCOIN_SP))
            {
                KioskStatic.logToFile("Coin Acceptor is MICROCOIN_SP");
                log.LogVariableState("Coin Acceptor is MICROCOIN_SP", KioskStatic.CoinAcceptorModel);
                CoinAcceptor.CoinAcceptor coinAcceptorMSP = null;
                try
                {
                    while (!logMessage.IgnoreAndContinue)
                    {
                        try
                        {
                            coinAcceptorMSP = KioskStatic.getCoinAcceptor(KioskStatic.config.coinAcceptorport);
                            break;
                        }
                        catch (Exception ex)
                        {
                            KioskStatic.logToFile("Error while Initializing Coin Acceptor: " + ex.Message);
                            log.Error(ex);
                            logMessage.AppendLog("Coin Acceptor: " + ex.Message);
                            if (!logMessage.Visible)
                                logMessage.Show();
                            Application.DoEvents();
                            mre.WaitOne(2000);
                        }
                    }
                    logMessage.Hide();
                    logMessage.IgnoreAndContinue = false;
                    logMessage.ClearLog();
                    mre.Reset();
                }
                finally
                {
                    if (coinAcceptorMSP != null)
                    {
                        coinAcceptorMSP.disableCoinAcceptor();
                        coinAcceptorMSP = null;
                    }
                }
                logMessage.Close();
                logMessage.Dispose();
            }
            else
            {

                if (KioskStatic.config.coinAcceptorport > 0)
                {
                    System.IO.Ports.SerialPort spCoinAcceptor = new System.IO.Ports.SerialPort();
                    try
                    {
                        spCoinAcceptor.PortName = "COM" + KioskStatic.config.coinAcceptorport.ToString();

                        spCoinAcceptor.BaudRate = 9600;
                        spCoinAcceptor.DataBits = 8;
                        spCoinAcceptor.Parity = System.IO.Ports.Parity.None;
                        spCoinAcceptor.StopBits = System.IO.Ports.StopBits.One;

                        //KioskStatic.spCoinAcceptor.ReceivedBytesThreshold = 5;
                        spCoinAcceptor.WriteTimeout = spCoinAcceptor.ReadTimeout = 1000;

                        while (!logMessage.IgnoreAndContinue)
                        {
                            try
                            {
                                if (!spCoinAcceptor.IsOpen)
                                {
                                    spCoinAcceptor.Open();
                                    KioskStatic.logToFile("Coin acceptor port " + KioskStatic.config.coinAcceptorport.ToString() + " opened");
                                    log.LogVariableState("Coin acceptor port " + KioskStatic.config.coinAcceptorport.ToString() + " opened", KioskStatic.config.coinAcceptorport);
                                    spCoinAcceptor.Close();
                                    try
                                    {
                                        CoinAcceptor.CoinAcceptor coinAcceptor = KioskStatic.getCoinAcceptor(KioskStatic.config.coinAcceptorport);
                                        try
                                        {
                                            KioskStatic.logToFile("Create Coin acceptor object and disable to coin acceptor device");
                                            log.LogVariableState("Create Coin acceptor object and disable to coin acceptor device", spCoinAcceptor);
                                            //coinAcceptor.spCoinAcceptor = KioskStatic.spCoinAcceptor;
                                            if (coinAcceptor != null)
                                                coinAcceptor.disableCoinAcceptor();
                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error("Error Creating/disabling Coin Acceptor", ex);
                                            KioskStatic.logToFile("Error Creating/disabling Coin Acceptor" + ex.Message);
                                        }
                                        finally
                                        {
                                            if (coinAcceptor != null)
                                            {
                                                coinAcceptor = null;
                                                KioskStatic.logToFile("Cleanup Coin acceptor object");
                                                log.LogVariableState("Cleanup Coin acceptor object", coinAcceptor);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        KioskStatic.logToFile("Error while Initializing Coin Acceptor: " + ex.Message);
                                        throw;
                                    }
                                }
                                break;
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                logMessage.AppendLog("Coin Acceptor: " + ex.Message);
                                if (!logMessage.Visible)
                                    logMessage.Show();
                                Application.DoEvents();
                                mre.WaitOne(2000);
                            }
                        }

                        logMessage.Hide();
                        logMessage.IgnoreAndContinue = false;
                        logMessage.ClearLog();
                        mre.Reset();
                    }
                    finally
                    {
                        spCoinAcceptor.Dispose();
                        spCoinAcceptor = null;
                    }
                }

                logMessage.Close();
                logMessage.Dispose();
                //if (KioskStatic.config.coinAcceptorport > 0)
                //{
                //    KioskStatic.logToFile("Registering spCoinAcceptor_DataReceived with coin acceptor serial port");
                //    KioskStatic.spCoinAcceptor.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(spCoinAcceptor_DataReceived);
                //}
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method is used to initiate Dispenser Card readerr
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="ownerForm">The form in which scanner has to be registered </param> 
        /// <returns></returns>
        public static DeviceClass InitiateDispenserCardReader(ExecutionContext executionContext, Form ownerForm, CardDispenser.CardDispenser.Models dispenserModel)
        {
            log.LogMethodEntry(executionContext, (ownerForm != null ? ownerForm.Name : null) , dispenserModel);
            int deviceAddress = 0;
            bool response = true;
            DeviceClass readerDevice = null;
            if (dispenserModel.Equals(CardDispenser.CardDispenser.Models.SCT0M0))
            {
                log.LogMethodExit(readerDevice, "SCT0M0 has reader within device");
                return readerDevice;
            }

            string deviceName = "USB Device";
            try
            {
                readerDevice = new ACR122U(deviceAddress);
                deviceName = "ACRU122U";
                log.LogVariableState("USB Device selected", deviceName);
            }
            catch
            {
                try
                {
                    string serialNumber = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DISPENSER_READER_SERIAL_NUMBER").Trim();
                    log.LogVariableState("DISPENSER_READER_SERIAL_NUMBER", serialNumber);
                    if (string.IsNullOrEmpty(serialNumber))
                    {
                        readerDevice = new ACR1252U(deviceAddress);
                        deviceName = "ACRU1252U";
                        log.LogVariableState("USB Device selected", deviceName);
                    }
                    else
                    {
                        readerDevice = new ACR1252U(serialNumber.Split('|')[0]);
                        deviceName = "ACRU1252U-" + serialNumber.Split('|')[0];
                        log.LogVariableState("USB Device selected", deviceName);
                    }
                }
                catch
                {
                    try
                    {
                        readerDevice = new MIBlack(deviceAddress);
                        deviceName = "MIBlack";
                        log.LogVariableState("USB Device selected", deviceName);
                    }
                    catch
                    {
                        response = false;
                        log.LogVariableState("No USB Device is selected", response);
                    }
                }
            }


            if (readerDevice != null)
            {
                KioskStatic.logToFile(deviceName + " registered for dispenser reader");
            }
            else
            {
                KioskStatic.logToFile("Unable to register dispenser reader");
                throw new Exception(MessageContainerList.GetMessage(executionContext, 1652));
            }
            log.LogMethodExit(readerDevice);
            return readerDevice;
        }
        /// <summary>
        /// This method is to register top up card reader.
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="ownerForm">The form in which scanner has to be registered</param>
        /// <param name="cardScanCompleteEventHandle">Card scan event</param>
        /// <returns>Device class object</returns>
        public static DeviceClass RegisterTopupCardReader(ExecutionContext executionContext, Form ownerForm, EventHandler topUpCardScanCompleteEventHandle)
        {
            log.LogMethodEntry();
            string deviceName = "USB Device";
            bool response = false;
            EventHandler CardScanCompleteEvent = new EventHandler(topUpCardScanCompleteEventHandle);
            DeviceClass readerDevice = null;
            int deviceAddress = 0;
            bool readerForRechargeOnly = true;
            if (KioskStatic.config.dispport <= 0)
            {
                response = true;
                try
                {
                    readerDevice = new ACR122U(deviceAddress, readerForRechargeOnly);
                    deviceName = "ACRU122U";
                    log.LogVariableState("USB Device selected is", deviceName);
                }
                catch
                {
                    try
                    {
                        string serialNumber = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CARD_READER_SERIAL_NUMBER").Trim();
                        log.LogVariableState("CARD_READER_SERIAL_NUMBER", serialNumber);
                        if (string.IsNullOrEmpty(serialNumber))
                        {
                            readerDevice = new ACR1252U(deviceAddress, readerForRechargeOnly);
                            deviceName = "ACRU1252U";
                            log.LogVariableState("USB Device selected is", deviceName);
                        }
                        else
                        {
                            readerDevice = new ACR1252U(serialNumber.Split('|')[0], readerForRechargeOnly);
                            deviceName = "ACRU1252U-" + serialNumber.Split('|')[0];
                            log.LogVariableState("USB Device selected is", deviceName);
                        }
                    }
                    catch
                    {
                        try
                        {
                            readerDevice = new MIBlack(deviceAddress);
                            deviceName = "MIBlack";
                            log.LogVariableState("USB Device selected is", deviceName);
                        }
                        catch
                        {
                            response = false;
                        }
                    }
                }

                if (response)
                {
                    log.LogVariableState("Register USB Device", readerDevice);
                    readerDevice.Register(CardScanCompleteEvent);
                    log.LogMethodExit(readerDevice);
                    return (readerDevice);
                }
            }

            if (!response)
            {
                response = true;
                try
                {
                    string serialNumber = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CARD_READER_SERIAL_NUMBER").Trim();
                    log.LogVariableState("CARD_READER_SERIAL_NUMBER", serialNumber);
                    if (string.IsNullOrEmpty(serialNumber))
                    {
                        deviceAddress++;
                        readerDevice = new ACR1252U(deviceAddress, readerForRechargeOnly);
                        deviceName = "ACRU1252U";
                        log.LogVariableState("USB Device selected is", deviceName);
                    }
                    else
                    {
                        readerDevice = new ACR1252U(serialNumber.Split('|')[0], readerForRechargeOnly);
                        deviceName = "ACRU1252U-" + serialNumber.Split('|')[0];
                        log.LogVariableState("USB Device selected is", deviceName);
                    }
                    readerDevice.Register(CardScanCompleteEvent);
                    log.LogMethodExit(readerDevice);
                    return readerDevice;
                }
                catch
                {
                    response = false;
                    readerDevice = RegisterUSBCardReader(executionContext, ownerForm, topUpCardScanCompleteEventHandle);
                    log.LogMethodExit(readerDevice);
                    return readerDevice;
                }
            }

            if (readerDevice != null)
            {
                KioskStatic.logToFile(deviceName + " registered for Topup reader");
                log.Info(deviceName + " registered for Topup reader");
            }
            else
            {
                KioskStatic.logToFile("Unable to register Topup reader");
                throw new Exception(MessageContainerList.GetMessage(executionContext, 1653));
            }

            log.LogMethodExit(readerDevice);
            return readerDevice;
        }

        public static void RestartRFIDPrinter(ExecutionContext executionContext, int posMachineId)
        {
            log.LogMethodEntry(executionContext, posMachineId);
            List<POSPrinterDTO> posPrinterDTOList = null;
            if (KioskStatic.POSMachineDTO != null && KioskStatic.POSMachineDTO.PosPrinterDtoList != null && KioskStatic.POSMachineDTO.PosPrinterDtoList.Any())
            {
                posPrinterDTOList = new List<POSPrinterDTO>(KioskStatic.POSMachineDTO.PosPrinterDtoList);
            }
            else
            {
                POSMachines posMachine = new POSMachines(executionContext, posMachineId);
                posPrinterDTOList = posMachine.PopulatePrinterDetails();
            }

            if (posPrinterDTOList != null && posPrinterDTOList.Any())
            {
                List<POSPrinterDTO> rfidPrinterDTOList = posPrinterDTOList.Where(ppp => ppp.PrinterDTO != null && ppp.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.RFIDWBPrinter).ToList();
                if (rfidPrinterDTOList != null && rfidPrinterDTOList.Any())
                {
                    for (int i = 0; i < rfidPrinterDTOList.Count; i++)
                    {
                        POSPrinterDTO posPrinterDTO = rfidPrinterDTOList[i];
                        try
                        {
                            string writsbandModel = string.Empty;
                            string cardNumber = string.Empty;
                            WristBandPrinter wristBandPrinter = null;
                            LookupsContainerDTO lookupsContainerDTO = LookupsContainerList.GetLookupsContainerDTO(executionContext.GetSiteId(), "RFID_WRISTBAND_MODELS");
                            if (lookupsContainerDTO != null &&
                                 lookupsContainerDTO.LookupValuesContainerDTOList != null &&
                                 lookupsContainerDTO.LookupValuesContainerDTOList.Any()
                                 && posPrinterDTO.PrinterDTO.WBPrinterModel > -1)
                            {
                                writsbandModel = lookupsContainerDTO.LookupValuesContainerDTOList.Where(x => x.LookupValueId == posPrinterDTO.PrinterDTO.WBPrinterModel).FirstOrDefault().LookupValue;
                            }
                            wristBandPrinter = WristbandPrinterFactory.GetInstance(executionContext).GetWristBandPrinter(writsbandModel);
                            if (string.IsNullOrWhiteSpace(writsbandModel))
                                writsbandModel = "STIMA";
                            log.LogVariableState("writsbandModel", writsbandModel);
                            switch (writsbandModel)
                            {
                                case "STIMA":
                                    {
                                        wristBandPrinter.SetIPAddress(posPrinterDTO.PrinterDTO.IpAddress);
                                        wristBandPrinter.RestartPrinter();
                                    }
                                    break;
                                case "BOCA":
                                    {
                                        wristBandPrinter.SetPrinterName(posPrinterDTO.PrinterDTO.PrinterName);
                                        wristBandPrinter.RestartPrinter();
                                    }
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            KioskStatic.logToFile("Error While restarting the RFID Printer: " + ex.Message);
                            log.Error("Error While restarting the RFID Printer", ex);
                            throw new Exception(MessageContainerList.GetMessage(executionContext, 2997));
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
