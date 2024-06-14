/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - AlohaCheck.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 ********************************************************************************************/
using System;
using Semnox.Parafait.ThirdParty;
using Semnox.Parafait.Transaction;

namespace Semnox.Parafait.KioskCore.ExternalPOSInterface
{
    public class AlohaCheck
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ParafaitAlohaIntegrator parafaitAlohaIntegrator;
        bool _isCheckOpen = false;
        public AlohaCheck()
        {
            log.LogMethodEntry();
            clsAlohaStatic.Init();
            parafaitAlohaIntegrator = new ParafaitAlohaIntegrator(clsAlohaStatic.AlohaTerminalId);
            try
            {
                parafaitAlohaIntegrator.PerformLogout();
            }
            catch(Exception ex)
            {
                log.Error("Error occurred while executing PerformLogout()" + ex.Message);
            }

            try
            {
                parafaitAlohaIntegrator.PerformLogin(clsAlohaStatic.AlohaEmpNumber, clsAlohaStatic.AlohaEmpPassword);
            }
            catch(ParafaitAlohaException ex)
            {
                int exceptionCode = System.Runtime.InteropServices.Marshal.GetHRForException(ex);
                exceptionCode = 0xfff & exceptionCode;
                if (exceptionCode != ParafaitAlohaException.ErrCOM_SomeoneAlreadyLoggedIn)
                {
                    throw ex;
                }
            }

            System.Threading.Thread.Sleep(300);
            try
            {
                parafaitAlohaIntegrator.PerformClockIn(clsAlohaStatic.AlohaJobCode);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing PerformClockIn()" + ex.Message);
            }
            KioskStatic.logToFile("Aloha Logged in: " + clsAlohaStatic.AlohaEmpNumber.ToString());
            log.LogMethodExit();
        }

        public bool checkAlohaValid()
        {
            log.LogMethodEntry();
            try
            {
                parafaitAlohaIntegrator.PerformLogin(clsAlohaStatic.AlohaEmpNumber, clsAlohaStatic.AlohaEmpPassword);
                log.LogMethodExit(true);
                return true;
            }
            catch (ParafaitAlohaException ex)
            {
                log.Error(ex.Message);
                int exceptionCode = System.Runtime.InteropServices.Marshal.GetHRForException(ex);
                exceptionCode = 0xfff & exceptionCode;
                if (exceptionCode == ParafaitAlohaException.ErrCOM_SomeoneAlreadyLoggedIn)
                {
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    log.LogMethodExit(false);
                    return false;
                }
            }
        }

        public ParafaitAlohaIntegrator getIntegrator()
        {
            log.LogMethodEntry();
            log.LogMethodExit(parafaitAlohaIntegrator);
            return parafaitAlohaIntegrator;
        }

        public bool isCheckOpen()
        {
            log.LogMethodEntry();
            log.LogMethodExit(_isCheckOpen);
            return _isCheckOpen;
        } 

        public bool OpenCheck(ref string message)
        {
            log.LogMethodEntry(message);
            try
            {
                parafaitAlohaIntegrator.PerformClockIn(clsAlohaStatic.AlohaJobCode);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing PerformClockIn()" + ex.Message);
            }

            System.Threading.Thread.Sleep(300);

            try
            {
                _isCheckOpen = false;
                parafaitAlohaIntegrator.OpenTransaction();

                _isCheckOpen = true;
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                log.Error(ex.Message);
                log.LogMethodExit(false);
                return false;
            }
        }

        
        void orderItem(int item, double price, int quantity)
        {
            log.LogMethodEntry(item, price, quantity);
            bool success = false;
            for (int j = 0; j < 20; j++)
            {
                try
                {
                    parafaitAlohaIntegrator.AddItem(item, price, quantity);
                    success = true;
                    break;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    int finalExcepCode = System.Runtime.InteropServices.Marshal.GetHRForException(ex) & 0xFFF;
                    if (finalExcepCode != ParafaitAlohaException.ErrCOM_InvalidOrderNumber)
                        throw ex;
                    else
                        System.Threading.Thread.Sleep(500);
                }
            }

            if (!success)
                throw new ApplicationException("Aloha Add Item Exception: Unable to get Order number after 20 attempts");
            log.LogMethodExit();
        }

        public bool ApplyCashPayment(double Amount, ref string message)
        {
            log.LogMethodEntry(Amount, message);
            try
            {
                parafaitAlohaIntegrator.ApplyCashPayment(Amount);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                message = ex.Message;
                log.LogMethodExit(false);
                return false;
            }
        }

        public bool ApplyProcessedCreditCardPayment(staticDataExchange.PaymentModeDetail paymentModeDetail, ref string message)
        {
            log.LogMethodEntry(paymentModeDetail, message);
            try
            {
                string CCName = paymentModeDetail.CreditCardName.ToUpper().Replace("_DEBIT", "");
                switch (CCName)
                {
                    case "VISA":
                    case "MASTERCARD":
                    case "DISCOVERER":
                    case "AMEX": break;
                    case "MASTER CARD": CCName = "MASTERCARD"; break;
                    case "AMERICANEXPRESS":
                    case "AMERICAN EXPRESS": CCName = "AMEX"; break;
                    default: CCName = "VISA"; break;
                }

                if (paymentModeDetail.CreditCardName.Contains("_DEBIT"))
                    CCName += "_DEBIT";

                int paymentId = parafaitAlohaIntegrator.ApplyProcessedCreditCardPayment(paymentModeDetail.Amount, CCName, paymentModeDetail.CreditCardNumber, paymentModeDetail.CreditCardExpiry, paymentModeDetail.CreditCardAuthorization);

                long status = ParafaitAlohaIntegrator.PAYMENT_WAITING;
                int count = 30;
                while (status == ParafaitAlohaIntegrator.PAYMENT_WAITING && count-- > 0)
                {
                    System.Threading.Thread.Sleep(300);
                    status = parafaitAlohaIntegrator.GetPaymentStatus(paymentId);
                }

                if (status == ParafaitAlohaIntegrator.PAYMENT_SUCCESS)
                {
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    log.LogMethodExit(false);
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                message = ex.Message;
                log.LogMethodExit(false);
                return false;
            }
        }
                    
        public void CancelCheck()
        {
            log.LogMethodEntry();
            try
            {
                parafaitAlohaIntegrator.ReverseTransaction();
                System.Threading.Thread.Sleep(1000);
                parafaitAlohaIntegrator.CloseTransaction();
            }
            catch(Exception ex)
            {
                log.Error("Error occurred while executing CancelCheck()" + ex.Message);
            }
            _isCheckOpen = false;
            log.LogMethodExit();
        }

        public bool CloseCheck(ref string message)
        {
            log.LogMethodEntry(message);
            try
            {
                try
                {
                    parafaitAlohaIntegrator.PlaceOrder();
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while executing CloseCheck()" + ex.Message);
                }
                parafaitAlohaIntegrator.CloseTransaction();
                
                _isCheckOpen = false;
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                message = ex.Message;
                log.LogMethodExit(false);
                return false;
            }
        }

        public void Dispose()
        {
            log.LogMethodEntry();
            try
            {
                parafaitAlohaIntegrator.PerformClockOut();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing PerformClockOut()" + ex.Message);
            }
            try
            {
                parafaitAlohaIntegrator.PerformLogout();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing PerformLogout()" + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
