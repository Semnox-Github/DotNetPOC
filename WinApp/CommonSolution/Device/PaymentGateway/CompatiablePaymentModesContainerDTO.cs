/********************************************************************************************
 * Project Name - Device
 * Description  - Data structure of PaymentModesContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.140.0     17-Aug-2021   Fiona               Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class CompatiablePaymentModesContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int id;
        private int paymentModeId;
        private int compatiablePaymentModeId;
        private string guid;
        /// <summary>
        /// DefaultConstructor
        /// </summary>
        public CompatiablePaymentModesContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// CompatiablePaymentModesContainerDTO with paramters
        /// </summary>
        /// <param name="id"></param>
        /// <param name="paymentModeId"></param>
        /// <param name="compatiablePaymentModeId"></param>
        /// <param name="guid"></param>
        public CompatiablePaymentModesContainerDTO(int id, int paymentModeId, int compatiablePaymentModeId, string guid)
        {
            log.LogMethodEntry(id,  paymentModeId,  compatiablePaymentModeId,  guid);
            this.id = id;
            this.paymentModeId = paymentModeId;
            this.compatiablePaymentModeId = compatiablePaymentModeId;
            this.guid = guid;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of Id
        /// </summary>
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        /// <summary>
        /// Get/Set method of PaymentModeId
        /// </summary>
        public int PaymentModeId
        {
            get
            {
                return paymentModeId;
            }
            set
            {
                paymentModeId = value;
            }
        }
        /// <summary>
        /// Get/Set method of CompatiablePaymentModeId
        /// </summary>
        public int CompatiablePaymentModeId
        {
            get
            {
                return compatiablePaymentModeId;
            }
            set
            {
                compatiablePaymentModeId = value;
            }
        }
    }
}
