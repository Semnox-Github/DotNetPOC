/********************************************************************************************
 * Project Name - Device
 * Description  - Data structure of PaymentModeChannelsContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.140.0     03-Sep-2021   Fiona               Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class PaymentModeChannelsContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int paymentModeChannelId;
        private int paymentModeId;
        private int lookupValueId;
        private string guid;

        /// <summary>
        /// DefaultConstructor
        /// </summary>
        public PaymentModeChannelsContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Contructor
        /// </summary>
        /// <param name="paymentModeChannelId"></param>
        /// <param name="paymentModeId"></param>
        /// <param name="lookupValueId"></param>
        /// <param name="guid"></param>
        public PaymentModeChannelsContainerDTO(int paymentModeChannelId, int paymentModeId, int lookupValueId, string guid)
        {
            log.LogMethodEntry(paymentModeChannelId, paymentModeId, lookupValueId, guid);
            this.paymentModeChannelId = paymentModeChannelId;
            this.paymentModeId = paymentModeId;
            this.lookupValueId = lookupValueId;
            this.guid = guid;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the PaymentModeChannelId field
        /// </summary>
        public int PaymentModeChannelId
        {
            get
            {
                return paymentModeChannelId;
            }
            set
            {
                paymentModeChannelId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PaymentModeId field
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
        /// Get/Set method of the LookupValueId field
        /// </summary>
        
        public int LookupValueId
        {
            get
            {
                return lookupValueId;
            }
            set
            {
                lookupValueId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                guid = value;
            }
        }
    }
}
