/********************************************************************************************
 * Project Name - Device
 * Description  - StimaCLS
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 * 2.70.2      08-Aug-2019       Deeksha        Added Logger Methods.
 * 2.120       17-Apr-2021       Guru S A       Wristband printing flow enhancements
 *             27-Dec-2021       Iqbal          Direct printing support
 *2.130.10     01-Sep-2022       Vignesh Bhat   Support for Reverse card number logic is missing in RFID printer card reads
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Device.Printer.WristBandPrinters;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.Device
{
    public class StimaCLS : WristBandPrinter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private StimaCLSAPI rfid;

        //public bool IsOutOfTickets = false;
        //public bool IsTicketJam = false;
        //public bool IsPrinterReady = false;
        public bool IsTicketPrinted = false;
        public PrinterDataClass PrinterData;
        public StimaCLS(Core.Utilities.ExecutionContext executionContext) : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            rfid = new StimaCLSAPI();
            //PrinterData = GetStatus();
            log.LogMethodExit();
        }

        /// <summary>
        /// SetIPAddress
        /// </summary>
        /// <param name="IPAddress"></param>
        public override void SetIPAddress(string IPAddress)
        {
            log.LogMethodEntry("IPAddress");
            rfid.SetIPAddress(IPAddress);
            log.LogMethodExit();
        }

        /// <summary>
        /// SetIPPort
        /// </summary>
        /// <param name="IPPort"></param>
        public override void SetIPPort(int IPPort)
        {
            log.LogMethodEntry("IPPort");
            rfid.SetIPPort(IPPort);
            log.LogMethodExit();
        }
        public StimaCLS(Core.Utilities.ExecutionContext executionContext, List<byte[]> defaultKey) : base(executionContext, defaultKey)
        {
            log.LogMethodEntry(executionContext, "defaultKey");
            rfid = new StimaCLSAPI();
            log.LogMethodExit();
        }
        public override void Open()
        {
            log.LogMethodEntry();
            lock (LockObject)
            {
                log.LogMethodEntry();

                rfid.Open();
            }
            log.LogMethodExit();
        }

        public override void Close()
        {
            log.LogMethodEntry();
            lock (LockObject)
            {
                log.LogMethodEntry();

                if (rfid != null)
                    rfid.Close();
            }
            log.LogMethodExit();
        }

        public override void Print(byte[] printCommand)
        {
            log.LogMethodEntry();
            lock (LockObject)
            {
                log.LogMethodEntry();
                rfid.Print(printCommand);
                IsTicketPrinted = true;
            }
            log.LogMethodExit();
        }

        public override PrinterDataClass GetStatus()
        {
            log.LogMethodEntry();
            PrinterDataClass printerDataClass = new PrinterDataClass();
            lock (LockObject)
            {
                log.LogMethodEntry();
                byte[] status = rfid.GetStatus();
                switch (status[0])
                {
                    case 0x11: printerDataClass.IsPrinterReady = true; break;
                    case 0x10: printerDataClass.IsOutOfTickets = true; break;
                }
                log.LogMethodExit(printerDataClass);
                return printerDataClass;
            }
        }

        /// <summary>
        /// GetFullStatus
        /// </summary>
        /// <returns></returns>
        public override PrinterDataClass GetFullStatus()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return GetStatus();
        }

        public override string ReadRFIDTag()
        {
            log.LogMethodEntry();
            lock (LockObject)
            {
                log.LogMethodEntry();
                TagNumber tagNumber;
                string cardNumber = "";
                try
                {
                    cardNumber = rfid.Select();
                    if (tagNumberParser.TryParse(cardNumber, out tagNumber) == false)
                    {
                        string message = tagNumberParser.Validate(cardNumber);
                        log.LogMethodExit(null, "Invalid Tag Number. " + message);
                        throw new ValidationException("Invalid Tag Number. " + message);
                    }
                    cardNumber = tagNumber.Value;
                    log.LogMethodExit(cardNumber); 
                    return cardNumber;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred  while executing ReadRFIDTag()", ex);
                    throw;
                }
            }
        }

        public override string readCardNumber()
        {
            lock (LockObject)
            {
                log.LogMethodEntry();
                string cardNumber = "";
                try
                {
                    rfid.Open();
                    try
                    {
                        cardNumber = rfid.Select();
                        log.LogMethodExit(cardNumber);
                        return cardNumber;
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occurred  while executing readCardNumber()", ex);
                        throw;
                    }
                }
                finally
                {
                    if (rfid != null)
                        rfid.Close();
                }
            }
        }

        public override bool Validate()
        {
            log.LogMethodEntry();
            bool response = false;
            foreach (byte[] key in FinalAuthKeys)
            {
                try
                {
                    rfid.LoadKey(key);
                    rfid.Authenticate(PURSE_BLOCK);
                    response = true;
                }
                catch (Exception ex)
                {
                    rfid.Close();
                    rfid.Open();
                    rfid.Select();
                    response = false;
                    log.Error("Error occurred  while executing Validate()", ex);
                }

                if (response)
                {
                    if (bytesEqual(key, defaultAuthKey, 6) == false // if other than default key was allowed, and key is not customer authkey then change key to default key
                        && bytesEqual(key, customerAuthKey, 6) == false)
                    {
                        try
                        {
                            byte[] dataBuffer = rfid.ReadBlock(PURSE_BLOCK + 3, 1);

                            for (int i = 0; i < 6; i++)
                                dataBuffer[i] = defaultAuthKey[i];

                            rfid.WriteBlock(PURSE_BLOCK + 3, 1, dataBuffer);

                            response = true;
                        }
                        catch (Exception ex)
                        {
                            response = false;
                            log.Error("Error occurred  while executing WriteBlock()", ex);
                        }

                        if (!response)
                        {
                            log.LogMethodExit(false);
                            return false;
                        }
                        else
                            beep();
                        defaultKeyChanged = true;
                        isRoamingCard = false;
                        TagSiteId = SiteId;
                        byte[] siteIdBuffer = new byte[16];
                        siteIdBuffer[0] = (byte)(SiteId >> 0);
                        siteIdBuffer[1] = (byte)(SiteId >> 8);
                        siteIdBuffer[2] = (byte)(SiteId >> 16);
                        siteIdBuffer[3] = (byte)(SiteId >> 24);

                        try
                        {
                            rfid.Close();
                            rfid.Open();
                            rfid.Select();
                            rfid.LoadKey(defaultAuthKey);
                            rfid.Authenticate(6);
                            rfid.WriteBlock(6, 1, siteIdBuffer);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occurred  while executing Validate()", ex);
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                    else
                    {
                        defaultKeyChanged = false;
                        byte[] siteIdBuffer = rfid.ReadBlock(6, 1);

                        int tagSiteId = BitConverter.ToInt32(siteIdBuffer, 0);
                        TagSiteId = tagSiteId;
                        if (tagSiteId == SiteId)
                            isRoamingCard = false;
                        else
                            isRoamingCard = true;

                    }
                    log.LogMethodExit(true);
                    return true;
                }
            }
            log.LogMethodExit(false);
            return false;
        }

        public override bool read_data(int blockAddress, int numberOfBlocks, byte[] paramAuthKey, ref byte[] paramReceivedData, ref string message)
        {
            lock (LockObject)
            {
                log.LogMethodEntry(blockAddress, numberOfBlocks, "paramAuthKey", paramReceivedData, message);
                try
                {
                    rfid.Open();
                    rfid.Select();
                    rfid.LoadKey(paramAuthKey);
                    rfid.Authenticate(blockAddress);
                    paramReceivedData = rfid.ReadBlock(blockAddress, numberOfBlocks);
                    log.LogMethodExit(true);
                    return true;
                }
                finally
                {
                    if (rfid != null)
                        rfid.Close();
                }
            }
        }

        public override bool write_data(int blockAddress, int numberOfBlocks, byte[] authKey, byte[] writeData, ref string message)
        {
            lock (LockObject)
            {
                log.LogMethodEntry(blockAddress, numberOfBlocks, "authKey", writeData, message);
                try
                {
                    rfid.Open();
                    rfid.Select();
                    rfid.LoadKey(authKey);
                    rfid.Authenticate(blockAddress);
                    rfid.WriteBlock(blockAddress, numberOfBlocks, writeData);
                    log.LogMethodExit(true);
                    return true;
                }
                finally
                {
                    if (rfid != null)
                        rfid.Close();
                }
            }
        }

        public override bool change_authentication_key(int blockAddress, byte[] currentAuthKey, byte[] newAuthKey, ref string message)
        {
            log.LogMethodEntry(blockAddress, "currentAuthKey", "newAuthKey", message);
            lock (LockObject)
            {
                byte[] dataBuffer = new byte[0];
                if (!read_data(blockAddress, 1, currentAuthKey, ref dataBuffer, ref message))
                {
                    log.LogMethodExit(false);
                    return false;
                }

                for (int i = 0; i < 6; i++)
                    dataBuffer[i] = newAuthKey[i];

                write_data(blockAddress, 1, currentAuthKey, dataBuffer, ref message);
                log.LogMethodExit(true);
                return true;
            }
        }

        public override void Authenticate(byte blockNumber, byte[] key)
        {
            log.LogMethodEntry(blockNumber, "key");
            try
            {
                rfid.Open();
                rfid.Select();
                rfid.LoadKey(key);
                rfid.Authenticate(blockNumber);
            }
            finally
            {
                if (rfid != null)
                    rfid.Close();
            }
            log.LogMethodExit();
        }

        public override void RestartPrinter()
        {
            log.LogMethodEntry();
            try
            {
                rfid.RestartPrinter();
                OpenCloseRFIDMode();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }

        private void OpenCloseRFIDMode()
        {
            log.LogMethodEntry();
            try
            {
                rfid.Open();
            }
            catch { try { rfid.Open(); } catch { } }
            finally
            {
                if (rfid != null)
                {
                    try { rfid.Close(); } catch { }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// CanPrint - Throws exception if printer is not ready to print
        /// </summary>
        public override void CanPrint(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            PrinterDataClass printerDataClass = GetStatus();
            if (printerDataClass.IsOutOfTickets)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "RFID Printer is out of tickets. Please contact staff"));
            } 
            if (printerDataClass.IsPrinterReady == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "RFID Printer is not ready. Please contact staff"));
            }
            log.LogMethodExit();
        }
    }
}
