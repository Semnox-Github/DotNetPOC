/********************************************************************************************
 * Project Name - Device
 * Description  - WristbandPrinterFactory
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.130.5     01-Mar-2021   Girish Kundar           Created 
 *2.130.10    01-Sep-2022   Vignesh Bhat            Support for Reverse card number logic is missing in RFID printer card reads
 ********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.Printer.WristBandPrinters.Boca;
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.Device.Printer.WristBandPrinters
{
    public enum WristBandPrinters
    {
        /// <summary>
        /// STIMA Wristband printer.
        /// </summary>
        STIMA,
        /// <summary>
        /// Wristband printer.
        /// </summary>
        BOCA

    }
    public class WristbandPrinterFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static WristbandPrinterFactory wristbandPrinterFactory;
        private ExecutionContext executionContext;
        protected Dictionary<WristBandPrinters, WristBandPrinter> wristbandPrinterDictionary = null;

        public WristbandPrinterFactory(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public static WristbandPrinterFactory GetInstance(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            if (wristbandPrinterFactory == null)
            {
                wristbandPrinterFactory = new WristbandPrinterFactory(executionContext);
            }
            log.LogMethodExit(wristbandPrinterFactory);
            return wristbandPrinterFactory;
        }

        public WristBandPrinter GetWristBandPrinter(string wristBandPrinter)
        {
            log.LogMethodEntry(wristBandPrinter);
            WristBandPrinters WristBandPrinterName;
            WristBandPrinter WristBandPrinter = null;

            if (wristbandPrinterDictionary == null)
            {
                wristbandPrinterDictionary = new Dictionary<WristBandPrinters, WristBandPrinter>();
            }
            if (Enum.TryParse<WristBandPrinters>(wristBandPrinter, out WristBandPrinterName))
            {
                if (wristbandPrinterDictionary.ContainsKey(WristBandPrinterName))
                {
                    WristBandPrinter = wristbandPrinterDictionary[WristBandPrinterName];
                }
                else
                {
                    WristBandPrinter = GetWristBandPrinterInstance(WristBandPrinterName);
                    if (WristBandPrinter == null)
                    {
                        log.LogMethodExit(null, "WristBand Printer Configuration Exception - Error occured while creating the WristBandPrinter. type: " + WristBandPrinterName.ToString());
                    }
                    else
                    {
                        wristbandPrinterDictionary.Add(WristBandPrinterName, WristBandPrinter);
                    }
                }
            }
            else
            {

                if (wristbandPrinterDictionary.ContainsKey(WristBandPrinterName) == false)
                {
                    WristBandPrinter = GetWristBandPrinterInstance(WristBandPrinters.STIMA);
                    wristbandPrinterDictionary.Add(WristBandPrinterName, WristBandPrinter);
                    WristBandPrinter = wristbandPrinterDictionary[WristBandPrinters.STIMA];
                }
            }
            log.LogMethodExit(WristBandPrinter);
            return WristBandPrinter;
        }

        /// <summary>
        /// Not yet fully implemented as Factory. Do not use this method
        /// </summary>
        /// <param name="wristbandPrinterName"></param>
        /// <returns></returns>
        private WristBandPrinter GetWristBandPrinterInstance(WristBandPrinters wristbandPrinterName)
        {
            log.LogMethodEntry(wristbandPrinterName);
            WristBandPrinter wristBandPrinter = null;

            switch (wristbandPrinterName)
            {
                case WristBandPrinters.STIMA:
                    {
                        wristBandPrinter = new StimaCLS(executionContext);  // This class implements multiple interfaces and a class
                        break;
                    }
                case WristBandPrinters.BOCA:
                    {
                        wristBandPrinter = new BOCAPrinter(executionContext);
                        break;
                    }
            }
            log.LogMethodExit(wristBandPrinter);
            return wristBandPrinter;
        }
    }
}
