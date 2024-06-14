/********************************************************************************************
 * Project Name - Device
 * Description  - Aloha Payment Gateway Class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar            Modified : Added Logger Methods.
 ********************************************************************************************/


namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// Aloha Payment Gateway Class
    /// </summary>
    public class Aloha
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ParafaitAlohaIntegrator _alohaObject;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="alohaObject"></param>
        public Aloha(ParafaitAlohaIntegrator alohaObject)
        {
            log.LogMethodEntry(alohaObject);
            _alohaObject = alohaObject;
            log.LogMethodExit();
        }

        /// <summary>
        /// Makes Payment
        /// </summary>
        /// <param name="chargeAmount">chargeAmount</param>
        /// <returns> bool </returns>
        public bool MakePayment(double chargeAmount)
        {
            log.LogMethodEntry(chargeAmount);
            frmUI ui = new frmUI(_alohaObject, chargeAmount);
            System.Windows.Forms.DialogResult dr = ui.ShowDialog();
            ui.Dispose();
            if (dr == System.Windows.Forms.DialogResult.OK)
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
}
