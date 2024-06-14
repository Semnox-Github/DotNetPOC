/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - CustomerStatic.cs 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.80        3-Sep-2019       Deeksha            Added logger methods.
********************************************************************************************/
using Semnox.Parafait.Customer;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;

namespace Parafait_Kiosk
{
    public static class CustomerStatic
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static CustomerDTO ShowCustomerScreen(string CardNumber = "")
        {
            log.LogMethodEntry(CardNumber);
            if (KioskStatic.ShowRegistrationTAndC)
            {
                bool show = true;
                if (!string.IsNullOrEmpty(CardNumber))
                {
                    Card card = new Card(CardNumber, "", KioskStatic.Utilities);
                    if (card.customer_id > -1)
                        show = false; // customer already registered, dont show age gate and T&C
                }

                if (show)
                {
                    if (KioskStatic.Utilities.getParafaitDefaults("SHOW_REGISTRATION_AGE_GATE").Equals("Y"))
                    {
                        frmAgeGate fa = new frmAgeGate(CardNumber);
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

                    if (new frmRegisterTnC().ShowDialog() != System.Windows.Forms.DialogResult.Yes)
                    {
                        log.LogMethodExit();
                        return null;
                    }
                    else
                    {
                        Customer fcustomer = new Customer(CardNumber);
                        fcustomer.ShowDialog();
                        log.LogMethodExit(fcustomer.customerDTO);
                        return fcustomer.customerDTO;
                    }
                }
            }

            Customer customer = new Customer(CardNumber);
            customer.ShowDialog();
            log.LogMethodExit(customer.customerDTO);
            return customer.customerDTO;
        }
    }
}
