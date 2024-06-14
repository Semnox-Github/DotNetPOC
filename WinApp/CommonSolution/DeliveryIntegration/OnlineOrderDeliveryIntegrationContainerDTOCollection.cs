/********************************************************************************************
 * Project Name - DeliveryIntegration 
 * Description  - Data object of OnlineOrderDeliveryIntegrationContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 2.150.0      13-Jul-2022   Guru S A                   Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DeliveryIntegration
{
    /// <summary>
    /// OnlineOrderDeliveryIntegrationContainerDTOCollection
    /// </summary>
    public class OnlineOrderDeliveryIntegrationContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<OnlineOrderDeliveryIntegrationContainerDTO> deliveryIntegrationContainerDTOList;
        private string hash;
        /// <summary>
        /// Default constructor
        /// </summary>
        public OnlineOrderDeliveryIntegrationContainerDTOCollection()
        {
            log.LogMethodEntry();
            this.deliveryIntegrationContainerDTOList = new List<OnlineOrderDeliveryIntegrationContainerDTO>();
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameter constructor
        /// </summary>
        /// <param name="deliveryIntegrationContainerDTOList"></param>
        public OnlineOrderDeliveryIntegrationContainerDTOCollection(List<OnlineOrderDeliveryIntegrationContainerDTO> deliveryIntegrationContainerDTOList)
        {
            log.LogMethodEntry();
            this.deliveryIntegrationContainerDTOList = deliveryIntegrationContainerDTOList;
            if (this.deliveryIntegrationContainerDTOList == null)
            {
                this.deliveryIntegrationContainerDTOList = new List<OnlineOrderDeliveryIntegrationContainerDTO>();
            }
            this.hash = new DtoListHash(GetDTOList(deliveryIntegrationContainerDTOList));
            log.LogMethodExit();
        }
        private IEnumerable<object> GetDTOList(List<OnlineOrderDeliveryIntegrationContainerDTO> deliveryChannelContainerDTOList)
        {
            log.LogMethodEntry(deliveryChannelContainerDTOList);
            foreach (OnlineOrderDeliveryIntegrationContainerDTO deliveryIntegrationContainerDTO in deliveryChannelContainerDTOList.OrderBy(x => x.DeliveryIntegrationId))
            {
                yield return deliveryIntegrationContainerDTO;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// OnlineOrderDeliveryIntegrationContainerDTOList
        /// </summary>
        public List<OnlineOrderDeliveryIntegrationContainerDTO> OnlineOrderDeliveryIntegrationContainerDTOList
        {
            get
            {
                return deliveryIntegrationContainerDTOList;
            }

            set
            {
                deliveryIntegrationContainerDTOList = value;
            }
        }
        /// <summary>
        /// Hash
        /// </summary>
        public string Hash
        {
            get
            {
                return hash;
            }
            set
            {
                hash = value;
            }
        }
    }
}
