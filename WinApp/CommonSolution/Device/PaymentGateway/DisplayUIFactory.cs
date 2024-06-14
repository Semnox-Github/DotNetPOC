/********************************************************************************************
 * Project Name - Device
 * Description  - Class DisplayUIFactory
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019    Girish kundar            Modified : Added Logger Methods.
 *2.150.1       22-Feb-2023    Guru S A                 Kiosk Cart Enhancements
 ********************************************************************************************/

using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// Display UI factory
    /// </summary>
    public class DisplayUIFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// This function will create and instance of the ui based on the mode
        /// </summary>
        /// <param name="IsUnattended">if true then kiosk ui will be populated else pos status ui will be populated</param>
        /// <param name="text">This should be passed in case of POS mode. Kiosk mode its not required</param>
        /// <returns>returns IDisplayStatusUI type object</returns>
        public static IDisplayStatusUI GetStatusUI(ExecutionContext executionContext,  bool IsUnattended, string amountChargeText, string text="")
        {
            log.LogMethodEntry(IsUnattended , amountChargeText , text);
            IDisplayStatusUI displayStatusUI;
            if(IsUnattended)
            {
                displayStatusUI = new frmKioskStatusUI(executionContext, text, amountChargeText);                
            }
            else
            {
                displayStatusUI = new frmPOSPaymentStatusUI(executionContext, text, amountChargeText);
            }
            log.LogMethodExit(displayStatusUI);
            return displayStatusUI;
        }
    }
}
