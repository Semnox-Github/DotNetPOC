/********************************************************************************************
 * Project Name - Semnox.Parafait.DeliveryIntegration 
 * Description  - Container object of DeliveryChannelContainer  
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 2.150.0      11-Jul-2022   Guru S A       Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DeliveryIntegration
{
    /// <summary>
    /// DeliveryChannelContainerDTO
    /// </summary>
    public class DeliveryChannelContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int deliveryChannelId;
        private int deliveryIntegrationId;
        private string channelName;
        private string channelAPIUrl;
        private string channelAPIKey;
        private bool autoAcceptOrders;
        private bool manualRiderAssignmentAllowed;
        private bool reconfirmationOrder;
        private bool reConfirmPreparation;
        private int defaultRiderId;
        private string externalSourceReference;
        private bool isActive;
        private string guid;
        /// <summary>
        /// Default constructor
        /// </summary>
        public DeliveryChannelContainerDTO()
        {
            log.LogMethodEntry();
            deliveryChannelId = -1;
            deliveryIntegrationId = -1;
            defaultRiderId = 1; 
            log.LogMethodExit();
        }
        /// <summary>
        /// Paramter constructor
        /// </summary>
        public DeliveryChannelContainerDTO(int deliveryChannelId, int deliveryIntegrationId, string channelName, string channelAPIUrl, string channelAPIKey, 
            bool autoAcceptOrders, bool manualRiderAssignmentAllowed, bool reconfirmationOrder, bool reConfirmPreparation, int defaultRiderId, string externalSourceReference,
            bool isActive, string guid) : this()
        {
            log.LogMethodEntry(deliveryChannelId, deliveryIntegrationId, channelName, channelAPIUrl, channelAPIKey, autoAcceptOrders, manualRiderAssignmentAllowed,
                reconfirmationOrder, reConfirmPreparation, defaultRiderId, externalSourceReference, isActive, guid);
            this.deliveryChannelId = deliveryChannelId;
            this.deliveryIntegrationId = deliveryIntegrationId;
            this.channelName = channelName;
            this.channelAPIUrl = channelAPIUrl;
            this.channelAPIKey = channelAPIKey;
            this.autoAcceptOrders = autoAcceptOrders;
            this.manualRiderAssignmentAllowed = manualRiderAssignmentAllowed;
            this.reconfirmationOrder = reconfirmationOrder;
            this.reConfirmPreparation = reConfirmPreparation;
            this.defaultRiderId = defaultRiderId;
            this.externalSourceReference = externalSourceReference;
            this.isActive = isActive; 
            this.guid = guid;
            log.LogMethodExit();
        }
        /// <summary>
        /// Copy constructor
        /// </summary>
        public DeliveryChannelContainerDTO(DeliveryChannelContainerDTO deliveryChannelContainerDTO) 
            : this(deliveryChannelContainerDTO.DeliveryChannelId, deliveryChannelContainerDTO.DeliveryIntegrationId, deliveryChannelContainerDTO.ChannelName,
                  deliveryChannelContainerDTO.ChannelAPIUrl, deliveryChannelContainerDTO.ChannelAPIKey, deliveryChannelContainerDTO.AutoAcceptOrders,
                  deliveryChannelContainerDTO.ManualRiderAssignmentAllowed, deliveryChannelContainerDTO.ReconfirmationOrder, deliveryChannelContainerDTO.ReConfirmPreparation,
                  deliveryChannelContainerDTO.DefaultRiderId, deliveryChannelContainerDTO.ExternalSourceReference, deliveryChannelContainerDTO.IsActive,
                  deliveryChannelContainerDTO.Guid)
        {
            log.LogMethodEntry(deliveryChannelContainerDTO); 
            log.LogMethodExit();
        }
        /// <summary>
        /// Set and get for DeliveryChannelId
        /// </summary>
        public int DeliveryChannelId { get { return  deliveryChannelId;} set {  deliveryChannelId = value; } }
        /// <summary>
        /// Set and get for DeliveryIntegrationId
        /// </summary>
        public int DeliveryIntegrationId { get { return  deliveryIntegrationId;} set {  deliveryIntegrationId = value; } }
        /// <summary>
        /// Set and get for ChannelName
        /// </summary>
        public string ChannelName { get { return  channelName;} set {  channelName = value; } }
        /// <summary>
        /// Set and get for ChannelAPIUrl
        /// </summary>
        public string ChannelAPIUrl { get { return  channelAPIUrl;} set {  channelAPIUrl = value; } }
        /// <summary>
        /// Set and get for ChannelAPIKey
        /// </summary>
        public string ChannelAPIKey { get { return  channelAPIKey;} set {  channelAPIKey = value; } }
        /// <summary>
        /// Set and get for AutoAcceptOrders
        /// </summary>
        public bool AutoAcceptOrders { get { return  autoAcceptOrders;} set {  autoAcceptOrders = value; } }
        /// <summary>
        /// Set and get for ManualRiderAssignmentAllowed
        /// </summary>
        public bool ManualRiderAssignmentAllowed { get { return  manualRiderAssignmentAllowed;} set {  manualRiderAssignmentAllowed = value; } }
        /// <summary>
        /// Set and get for ReconfirmationOrder
        /// </summary> 
        public bool ReconfirmationOrder { get { return  reconfirmationOrder;} set {  reconfirmationOrder = value; } }
        /// <summary>
        /// Set and get for ReConfirmPreparation
        /// </summary>
        public bool ReConfirmPreparation { get { return  reConfirmPreparation;} set {  reConfirmPreparation = value; } }
        /// <summary>
        /// Set and get for DefaultRiderId
        /// </summary> 
        public int DefaultRiderId { get { return  defaultRiderId;} set {  defaultRiderId = value; } }
        /// <summary>
        /// Set and get for ExternalSourceReference
        /// </summary>
        public string ExternalSourceReference { get { return  externalSourceReference;} set {  externalSourceReference = value; } }
        /// <summary>
        /// Set and get for IsActive
        /// </summary>
        public bool IsActive { get { return  isActive;} set {  isActive = value; } }
        /// <summary>
        /// Set and get for Guid
        /// </summary>
        public string Guid { get { return  guid;} set {  guid = value; } }
    }
}
