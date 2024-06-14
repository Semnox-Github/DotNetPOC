/********************************************************************************************
 * Project Name - PaymentModeDTO Programs
 * Description  - Data object of PaymentModeParams      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        07-Feb-2017   Rakshith       Created 
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods.
 ********************************************************************************************/
using System.ComponentModel;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    ///  PaymentParams Class
    /// </summary>
    public class PaymentModeParams
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int paymentModeId;
        private string paymentChannel;
        private int siteId;
        private string paymentMode;

        /// <summary>
        /// Default constructor
        /// </summary> 
        public PaymentModeParams()
        {
            log.LogMethodEntry();
            this.paymentModeId = -1;
            this.paymentChannel = "";
            this.siteId = -1;
            this.paymentMode = "";
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the PaymentModeId field
        /// </summary>
        [DisplayName("PaymentModeId")]
        [DefaultValue(-1)]
        public int PaymentModeId { get { return paymentModeId; } set { paymentModeId = value; } }


        /// <summary>
        /// Get/Set method of the PaymentMode field
        /// </summary>
        [DisplayName("PaymentMode")]
        [DefaultValue("")]
        public string PaymentMode { get { return paymentMode; } set { paymentMode = value; } }


        /// <summary>
        /// Get/Set method of the PaymentChannnel field
        /// </summary>
        [DisplayName("SiteId")]
        [DefaultValue(-1)]
        public int SiteId { get { return siteId; } set { siteId = value; } }


        /// <summary>
        /// Get/Set method of the PaymentChannel field from LookUpValue
        /// </summary>
        [DisplayName("PaymentChannel")]
        [DefaultValue("")]
        public string PaymentChannel { get { return paymentChannel; } set { paymentChannel = value; } }

    }

}
