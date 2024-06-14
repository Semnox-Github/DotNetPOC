/********************************************************************************************
 * Project Name - Device.Printer
 * Description  - Class for  of MagiCard      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods.
 ********************************************************************************************/
using SDKShim;
using System;

namespace Semnox.Parafait.Device.Printer
{
    public class MagiCard
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IntPtr _hDC = IntPtr.Zero;
        IntPtr hSession = IntPtr.Zero;
        public MagiCard(IntPtr HDC)
        {
            log.LogMethodEntry();
            _hDC = HDC;
            log.LogMethodExit();
        }

        public void OpenSession()
        {
            log.LogMethodEntry();
            SDK.Return SDKReturn = SDK.ID_OpenSession(_hDC, out hSession, SDK.Config.Normal);
            log.Info("SDKReturn: " + SDKReturn);
            log.Debug(SDKReturn);
            if (SDKReturn != SDK.Return.Success)
            {
                HandleError("OpenSession", SDKReturn);
            }
            else
            {
                //Call the SDK to get the printer model 
                SDK.PrinterType printerTypeValue = SDK.ID_PrinterType(hSession);
                log.Info("printerTypeValue: " + printerTypeValue);
                log.Debug(printerTypeValue);
                if (printerTypeValue != SDK.PrinterType.Enduro && printerTypeValue != SDK.PrinterType.RioPro360)
                {
                    SDK.ID_CloseSession(hSession);
                    log.Error("Exception Occurred at OpenSession()  ");
                    throw new ApplicationException("Invalid Printer: Use with DTC Only");
                }
            }
            log.LogMethodExit();
        }

        public void CloseSession()
        {
            log.LogMethodEntry();
            SDK.Return SDKReturn = SDK.ID_CloseSession(hSession);
            if (SDKReturn != SDK.Return.Success)
            {
                HandleError("CloseSession", SDKReturn);
            }
            log.LogMethodExit();
        }

        public void FeedCard()
        {
            log.LogMethodEntry();
            SDK.FeedMode mode = SDK.FeedMode.Contactless;

            SDK.Return SDKReturn = SDK.ID_FeedCard(hSession, mode, 0);
            if (SDKReturn != SDK.Return.Success)
            {
                HandleError("FeedCard", SDKReturn);
            }
            else
            {
                WaitForPrinterToFinish();
            }
            log.LogMethodExit();
        }

        public IntPtr InitializeCanvas(bool isBackside = false)
        {
            log.LogMethodEntry(isBackside);
            SDK.Return SDKReturn;

            //Determine if the printer is online and ready before proceeding
            if (!PrinterIsReady())
            {
                log.Error("Exception Occurred : Printer is not available  ");
                throw new ApplicationException("InitializeCanvas: Printer is not available");
            }

            IntPtr CanvasHDC;

            SDKReturn = SDK.ID_CanvasInit(hSession, out CanvasHDC, isBackside ? SDK.Canvas.Back : SDK.Canvas.Front);
            if (SDKReturn != SDK.Return.Success)
            {
                HandleError("CanvasInitFront", SDKReturn);
            }
            log.LogMethodExit(CanvasHDC);
            return CanvasHDC;
        }

        public void PrintCard()
        {
            log.LogMethodEntry();
            SDK.Return SDKReturn = SDK.ID_PrintCard(hSession);
            if (SDKReturn != SDK.Return.Success)
            {
                HandleError("PrintCard", SDKReturn);
            }

            WaitForPrinterToFinish();
            log.LogMethodExit();
        }

        public void EjectCard()
        {
            log.LogMethodEntry();
            SDK.Return SDKReturn = SDK.ID_EjectCard(hSession);
            if (SDKReturn != SDK.Return.Success)
            {
                HandleError("EjectCard", SDKReturn);
            }

            WaitForPrinterToFinish();
            log.LogMethodExit();
        }

        private void WaitForPrinterToFinish()
        {
            log.LogMethodEntry();
            SDK.Return SDKReturn;
            int repeatCount = 4;

            do
            {
                //Repeat the wait until response is not timeout
                SDKReturn = SDK.ID_WaitForPrinter(hSession);
                if (SDKReturn == SDK.Return.Timeout)
                {
                    if (--repeatCount > 0)
                        continue;
                    else
                        HandleError("WaitForPrinterToFinish", SDKReturn);
                }
                else if (SDKReturn == SDK.Return.Success)
                {
                    break;
                }
                else
                {
                    HandleError("WaitForPrinterToFinish", SDKReturn);
                }
            } while (true);
            log.LogMethodExit();
        }

        private bool PrinterIsReady()
        {
            log.LogMethodEntry();
            bool returnValue = (SDK.ID_PrinterStatus(hSession) == SDK.PrinterStatus.Ready);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        private void HandleError(string action, SDK.Return result)
        {
            log.LogMethodEntry(action, result);
            string error;
            switch (result)
            {
                case SDK.Return.Timeout: error = "ID_TIMEOUT"; break;
                case SDK.Return.Error: error = "ID_ERROR"; break;
                case SDK.Return.PrinterError: error = "ID_PRINTER_ERROR"; break;
                case SDK.Return.DriverNotCompliant: error = "ID_DRIVER_NOTCOMPLIANT"; break;
                case SDK.Return.OpenPrinterError: error = "ID_OPENPRINTER_ERROR"; break;
                case SDK.Return.RemoteCommError: error = "ID_REMOTECOMM_ERROR"; break;
                case SDK.Return.LocalCommError: error = "ID_LOCALCOMM_ERROR"; break;
                case SDK.Return.SpoolerNotEmpty: error = "ID_SPOOLER_NOT_EMPTY"; break;
                case SDK.Return.RemoteCommInUse: error = "ID_REMOTECOMM_IN_USE"; break;
                case SDK.Return.LocalCommInUse: error = "ID_LOCALCOMM_IN_USE"; break;
                case SDK.Return.ParamError: error = "ID_PARAM_ERROR"; break;
                case SDK.Return.InvalidSession: error = "ID_INVALID_SESSION"; break;
                default: error = "Unknown API Error - " + result; break;
            }
            log.LogMethodExit();
            log.Error("Error Occurred :" + error);
            throw new ApplicationException(action + ": " + error);

        }
    }
}
