/********************************************************************************************
 * Project Name - Device
 * Description  - WristBandPrinters
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.130.5     01-Mar-2021   Girish Kundar           Created 
 *2.130.10    01-Sep-2022   Vignesh Bhat            Support for Reverse card number logic is missing in RFID printer card reads
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer.WristBandPrinters.Boca;

namespace Semnox.Parafait.Device.Printer.WristBandPrinters
{
    public class WristBandPrinter : MifareDevice, IDisposable
    {
        private static readonly Semnox.Parafait.logging.Logger log =
                                 new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected ExecutionContext executionContext;

        protected TagNumberParser tagNumberParser;

        public WristBandPrinter(ExecutionContext executionContext,  List<byte[]> defaultKey) : base(defaultKey)
        {
            log.LogMethodEntry(executionContext, defaultKey);
            this.executionContext = executionContext;
            this.tagNumberParser = new TagNumberParser(this.executionContext);
            log.LogMethodExit();
        }
        public WristBandPrinter(ExecutionContext executionContext) :base ()
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.tagNumberParser = new TagNumberParser(this.executionContext);
            log.LogMethodExit();
        }
        public virtual void Open()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        public virtual void SetIPAddress(string IPAddress)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        public virtual void SetIPPort(int IPPort)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        public virtual void SetPrinterName(string printerName)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        public virtual string ReadRFIDTag()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return string.Empty;
        }
        public virtual void SendData(string data)
        {
            log.LogMethodEntry(data);
            log.LogMethodExit();
        }
        //public virtual void Print(string printText)
        //{
        //    log.LogMethodEntry(printText);
        //    log.LogMethodExit();
        //}
        public virtual void Print(byte[] printText)
        {
            log.LogMethodEntry(printText);
            log.LogMethodExit();
        }
        public virtual void Close()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public virtual void RestartPrinter()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public virtual PrinterDataClass GetStatus()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return null;
        }

        public virtual PrinterDataClass GetFullStatus()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return null;
        }

        public virtual void SetAttributes()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public virtual void CanPrint(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            log.LogMethodExit(); 
        }
        /// <summary>
        /// GetCardNumberLength
        /// </summary>
        /// <returns></returns>
        internal int GetCardNumberLength()
        {
            log.LogMethodEntry();
            int cardNoLength = 8;
            try
            {
                cardNoLength = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "CARD_NUMBER_LENGTH", 8);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                cardNoLength = 8;
            }
            log.LogMethodExit(cardNoLength);
            return cardNoLength;
        }
    }
}
