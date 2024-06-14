/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - BillAcceptor
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 *2.80.1      22-Feb-202       Deeksha            Issue Fix:Kiosk  cash payment issue
 *2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.IO.Ports;
using Semnox.Parafait.logging;

namespace Semnox.Parafait.KioskCore.BillAcceptor
{
    public class BillAcceptor
    {
        internal static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private double amountRemainingToBeCollected;
        public enum Models { ICTL77, GBAST1, NV9USB };
        public SerialPort spBillAcceptor;
        public int ReceivedNoteDenomination = 0;
        public bool criticalError = false;
        public bool overpayReject = false;
        //public KioskStatic.acceptance acceptance = new KioskStatic.acceptance();
        bool _working = false;
        internal bool stillProcessingBills = false;
        internal string portName;
        public double AmountRemainingToBeCollected { get { return amountRemainingToBeCollected; }  set { amountRemainingToBeCollected = value; } }
        public virtual bool ProcessReceivedData(byte[] billRec, ref string message)
        {
            log.LogMethodEntry(billRec, message);
            log.LogMethodExit(false);
            return false;
        }

        public virtual bool Working
        {
            get { return _working; }
            set { _working = value; }
        }

        public virtual void disableBillAcceptor()
        {
        }

        public virtual void requestStatus()
        {
        }

        public virtual void initialize()
        {
        }

        public bool OverPayAllowed(int denomination)
        {
            log.LogMethodEntry(denomination);
            log.Info("KioskStatic.AllowOverPay: " + KioskStatic.AllowOverPay.ToString());
            double amountReceived = (double)Math.Round(KioskStatic.config.Notes[denomination].Value, 2, MidpointRounding.AwayFromZero);

            if (KioskStatic.AllowOverPay == false)
            {
                //if (Math.Round(KioskStatic.config.Notes[denomination].Value + acceptance.totalValue, 2) > Math.Round(acceptance.productPrice, 2))
                // added MidpointRounding.AwayFromZero on 29/02/2016 by iqbal 
                //if (Math.Round(KioskStatic.config.Notes[denomination].Value + acceptance.totalValue, 2, MidpointRounding.AwayFromZero) > Math.Round(acceptance.productPrice, 2, MidpointRounding.AwayFromZero))
                //{
                //    // added logging on 29/02/2016 by iqbal
                //    KioskStatic.logToFile("Total value inserted: " + (Math.Round(KioskStatic.config.Notes[denomination].Value + acceptance.totalValue, 2)).ToString());
                //    KioskStatic.logToFile("Product price: " + Math.Round(acceptance.productPrice, 2).ToString());
                //    log.Info("Total value inserted: " + (Math.Round(KioskStatic.config.Notes[denomination].Value + acceptance.totalValue, 2)).ToString());
                //    log.Info("Product price: " + Math.Round(acceptance.productPrice, 2).ToString());
                //    // end add
                //    log.LogMethodExit(false);
                //    return false;
                //} 
                if (amountReceived > Math.Round(amountRemainingToBeCollected, 2, MidpointRounding.AwayFromZero)) 
                {

                    string msg = "Amount received: " + Math.Round(amountReceived, 2).ToString() + " is more than Amount Remaining To Be Collected: " + Math.Round(amountRemainingToBeCollected, 2).ToString();
                    KioskStatic.logToFile(msg);
                    log.Error(msg);
                    log.LogMethodExit(false);
                    return false; 
                }
            }
            //amountRemainingToBeCollected = amountRemainingToBeCollected - amountReceived;
            log.LogMethodExit(true);
            return true;
        }

        public bool StillProcessing()
        {
            log.LogMethodEntry();
            bool returnValue = (Working == true ? stillProcessingBills : Working);
            string msg1 = "Still Processing Check - ProcessingBills: " + stillProcessingBills.ToString() + " Working: "+ Working.ToString() + " returnValue: "+ returnValue.ToString();
            KioskStatic.logToFile(msg1);
            log.Info(msg1);  
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
