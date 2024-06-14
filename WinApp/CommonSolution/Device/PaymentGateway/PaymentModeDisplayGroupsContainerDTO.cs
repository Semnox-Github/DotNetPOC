/********************************************************************************************
 * Project Name - PaymentModeDisplayGroupsContainerDTO
 * Description  - Data structure of PaymentModeDisplayGroupsContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.150.1     26-Jan-2023   Guru S A               Created
 ********************************************************************************************/

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class PaymentModeDisplayGroupsContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int paymentModeDisplayGroupId;
        private int paymentModeId;
        private int productDisplayGroupId;
        private string guid;
        /// <summary>
        /// Default Contructor
        /// </summary>
        public PaymentModeDisplayGroupsContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }/// <summary>
         /// Default Contructor
         /// </summary>
        public PaymentModeDisplayGroupsContainerDTO(int paymentModeDisplayGroupId, int paymentModeId, int productDisplayGroupId, string guid)
        {
            log.LogMethodEntry(paymentModeDisplayGroupId, paymentModeId, productDisplayGroupId, guid);
            this.paymentModeDisplayGroupId = paymentModeDisplayGroupId;
            this.paymentModeId = paymentModeId;
            this.productDisplayGroupId = productDisplayGroupId;
            this.guid = guid;
            log.LogMethodExit();
        }
        /// <summary>
        ///  Get/Set method of the paymentModeDisplayGroupId field
        /// </summary>
        public int PaymentModeDisplayGroupId { get { return paymentModeDisplayGroupId; } set { paymentModeDisplayGroupId = value; } }
        /// <summary>
        ///  Get/Set method of the PaymentModeId field
        /// </summary>
        public int PaymentModeId { get { return paymentModeId; } set { paymentModeId = value; } }
        /// <summary>
        ///  Get/Set method of the productDisplayGroupId field
        /// </summary>
        public int ProductDisplayGroupId { get { return productDisplayGroupId; } set { productDisplayGroupId = value; } }
    }
}
