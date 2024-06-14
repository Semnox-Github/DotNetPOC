using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Site
{
    public class SiteDetailContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int siteDetailId;
        private int parentSiteId;   
        private int deliveryChannelId;
        private decimal? onlineChannelStartHour;
        private decimal? onlineChannelEndHour;
        private string orderDeliveryType;
        private List<string> zipCodes;
        private int siteId;

        /// <summary>
        /// default constructor
        /// </summary>
        public SiteDetailContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields.
        /// </summary>
        public SiteDetailContainerDTO(int siteDetailId, int parentSiteId, int deliveryChannelId , decimal? onlineChannelStartHour, decimal? onlineChannelEndHour,
                                               string orderDeliveryType, List<string> zipCodes, int siteId)
    : this()
        {
            log.LogMethodEntry(deliveryChannelId, onlineChannelStartHour, onlineChannelEndHour, orderDeliveryType, zipCodes);
            this.siteDetailId = siteDetailId;
            this.parentSiteId = parentSiteId;
            this.onlineChannelStartHour = onlineChannelStartHour;
            this.onlineChannelEndHour = onlineChannelEndHour;
            this.orderDeliveryType = orderDeliveryType;
            this.zipCodes = zipCodes;
            this.deliveryChannelId = deliveryChannelId;
            this.siteId = siteId;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the deliveryChannelId field
        /// </summary>
        public int DeliveryChannelId { get { return deliveryChannelId; } set { deliveryChannelId = value; } }

        /// <summary>
        /// Get/Set method of the SiteDetailId field
        /// </summary>
        public int SiteDetailId { get { return siteDetailId; } set { siteDetailId = value; } }

        /// <summary>
        /// Get/Set method of the ParentSiteId field
        /// </summary>
        public int ParentSiteId { get { return parentSiteId; } set { parentSiteId = value; } }
        /// <summary>
        /// Get/Set method of the onlineChannelStartHour field
        /// </summary>
        public decimal? OnlineChannelStartHour { get { return onlineChannelStartHour; } set { onlineChannelStartHour = value; } }
        /// <summary>
        /// Get/Set method of the onlineChannelStartHour field
        /// </summary>
        public decimal? OnlineChannelEndHour { get { return onlineChannelEndHour; } set { onlineChannelEndHour = value; } }
        /// <summary>
        /// Get/Set method of the orderDeliveryType field
        /// </summary>
        public string OrderDeliveryType { get { return orderDeliveryType; } set { orderDeliveryType = value; } }
        /// <summary>
        /// Get/Set method of the zipCodes field
        /// </summary>
        public List<string> ZipCodes { get { return zipCodes; } set { zipCodes = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

    }
}
