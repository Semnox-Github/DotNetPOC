/********************************************************************************************
 * Project Name - CustomerStatic
 * Description  - user interface for CustomerStatic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00                                        Created 
 *2.4.0       25-Nov-2018      Raghuveera     terms and condition is passing to the customer form
 *2.80        4-Sep-2019       Deeksha        Added logger methods.
 ********************************************************************************************/
using Semnox.Parafait.Customer;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
namespace Parafait_Kiosk
{
    public static class CustomerStatic
    {
        public static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static CustomerDTO ShowCustomerScreen(string CardNumber = "")
        {
            log.LogMethodEntry(CardNumber);
            Card card = null;
            if (KioskStatic.ShowRegistrationTAndC)
            {
                bool show = true;
                if (!string.IsNullOrEmpty(CardNumber))
                {
                    card = new Card(CardNumber, "", KioskStatic.Utilities);
                    if (card.customer_id > -1)
                        show = false; // customer already registered, dont show age gate and T&C
                    log.LogVariableState("Card", card);
                }

                if (show)
                {
                    if (KioskStatic.Utilities.getParafaitDefaults("SHOW_REGISTRATION_AGE_GATE").Equals("Y"))
                    {
                        using (frmAgeGate fa = new frmAgeGate(CardNumber))
                        {
                            if (fa.ShowDialog() != System.Windows.Forms.DialogResult.Yes)
                            {
                                log.LogMethodExit();
                                return null;
                            }
                            else
                            {
                                log.LogMethodExit(fa.customerDTO);
                                return fa.customerDTO;
                            }
                        }
                    }

                    if (new frmRegisterTnC().ShowDialog() != System.Windows.Forms.DialogResult.Yes)
                    {
                        log.LogMethodExit();
                        return null;
                    }
                    else
                    {
                        using (Customer fcustomer = new Customer(CardNumber, null, true))
                        {
                            fcustomer.ShowDialog();
                            log.LogMethodExit(fcustomer.customerDTO);
                            return fcustomer.customerDTO;
                        }
                    }
                }
            }
            using (Customer customer = new Customer(CardNumber))
            {
                customer.ShowDialog();
                log.LogMethodExit(customer.customerDTO);
                return customer.customerDTO;
            }
        }
    }
}
