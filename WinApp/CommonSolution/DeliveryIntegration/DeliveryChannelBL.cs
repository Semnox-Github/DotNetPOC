/********************************************************************************************
 * Project Name - DeliveryIntegration                                                                       
 * Description  - DeliveryIntegration BL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar     Created : Urban Piper changes
  *2.140.0     01-Jun-2021   Fiona Lishal     Modified for Delivery Order enhancements for F&B
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.DeliveryIntegration
{
    /// <summary>
    /// 
    /// </summary>
    public class DeliveryChannelBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DeliveryChannelDTO deliveryChannelDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        private DeliveryChannelBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="parameterDeliveryChannelDTO">parameterDeliveryChannelDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public DeliveryChannelBL(ExecutionContext executionContext, DeliveryChannelDTO parameterDeliveryChannelDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parameterDeliveryChannelDTO, sqlTransaction);

            if (parameterDeliveryChannelDTO.DeliveryChannelId > -1)
            {
                LoadDeliveryChannelDTO(parameterDeliveryChannelDTO.DeliveryChannelId, sqlTransaction);//added sql
                ThrowIfDTOIsNull(parameterDeliveryChannelDTO.DeliveryChannelId);
                Update(parameterDeliveryChannelDTO);
            }
            else
            {
                Validate(parameterDeliveryChannelDTO, sqlTransaction);
                deliveryChannelDTO = new DeliveryChannelDTO(-1, parameterDeliveryChannelDTO.ChannelName, parameterDeliveryChannelDTO.ChannelAPIUrl, parameterDeliveryChannelDTO.ChannelAPIKey,
                                                                 parameterDeliveryChannelDTO.AutoAcceptOrders, parameterDeliveryChannelDTO.ManualRiderAssignmentAllowed, parameterDeliveryChannelDTO.DefaultRiderId, parameterDeliveryChannelDTO.ExternalSourceReference, parameterDeliveryChannelDTO.ReConfirmOrder, parameterDeliveryChannelDTO.ReConfirmPreparation, parameterDeliveryChannelDTO.IsActive,
                                                                 parameterDeliveryChannelDTO.DeliveryIntegrationId);
            }
            log.LogMethodExit();
        }
        private void ThrowIfDTOIsNull(int id)
        {
            log.LogMethodEntry();
            if (deliveryChannelDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "DeliveryChannels", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        private void LoadDeliveryChannelDTO(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, sqlTransaction);
            DeliveryChannelDataHandler deliveryChannelDataHandler = new DeliveryChannelDataHandler(sqlTransaction);
            deliveryChannelDTO = deliveryChannelDataHandler.GetDeliveryChannelDTO(id);
            ThrowIfDTOIsNull(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="parameterDeliveryChannelDTO"></param>
        /// <param name="sqlTransaction"></param>
        public void Update(DeliveryChannelDTO parameterDeliveryChannelDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterDeliveryChannelDTO);
            ChangeDeliveryChannelId(parameterDeliveryChannelDTO.DeliveryChannelId);
            ChangeChannelName(parameterDeliveryChannelDTO.ChannelName);
            ChangeChannelAPIUrl(parameterDeliveryChannelDTO.ChannelAPIUrl);
            ChangeChannelAPIKey(parameterDeliveryChannelDTO.ChannelAPIKey);
            ChangeAutoAcceptOrders(parameterDeliveryChannelDTO.AutoAcceptOrders);
            ChangeManualRiderAssignmentAllowed(parameterDeliveryChannelDTO.ManualRiderAssignmentAllowed);
            ChangeDefaultRiderId(parameterDeliveryChannelDTO.DefaultRiderId);
            ChangeExternalSourceReference(parameterDeliveryChannelDTO.ExternalSourceReference);
            ChangeReConfirmOrder(parameterDeliveryChannelDTO.ReConfirmOrder);
            ChangeReConfirmPreparation(parameterDeliveryChannelDTO.ReConfirmPreparation);
            ChangeIsActive(parameterDeliveryChannelDTO.IsActive);
            ChangeDeliveryIntegrationId(parameterDeliveryChannelDTO.DeliveryIntegrationId);
            log.LogMethodExit();
        }

        private void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (deliveryChannelDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to isActive");
                return;
            }
            deliveryChannelDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        private void ChangeReConfirmPreparation(bool reConfirmPreparation)
        {
            log.LogMethodEntry(reConfirmPreparation);
            if (deliveryChannelDTO.ReConfirmPreparation == reConfirmPreparation)
            {
                log.LogMethodExit(null, "No changes to reConfirmPreparation");
                return;
            }
            deliveryChannelDTO.ReConfirmPreparation = reConfirmPreparation;
            log.LogMethodExit();
        }

        private void ChangeReConfirmOrder(bool reConfirmOrder)
        {
            log.LogMethodEntry(reConfirmOrder);
            if (deliveryChannelDTO.ReConfirmOrder == reConfirmOrder)
            {
                log.LogMethodExit(null, "No changes to reConfirmOrder");
                return;
            }
            deliveryChannelDTO.ReConfirmOrder = reConfirmOrder;
            log.LogMethodExit();
        }

        private void ChangeExternalSourceReference(string externalSourceReference)
        {
            log.LogMethodEntry(externalSourceReference);
            if (deliveryChannelDTO.ExternalSourceReference == externalSourceReference)
            {
                log.LogMethodExit(null, "No changes to externalSourceReference");
                return;
            }
            deliveryChannelDTO.ExternalSourceReference = externalSourceReference;
            log.LogMethodExit();
        }

        private void ChangeDefaultRiderId(int defaultRiderId)
        {
            log.LogMethodEntry(defaultRiderId);
            if (deliveryChannelDTO.DefaultRiderId == defaultRiderId)
            {
                log.LogMethodExit(null, "No changes to defaultRiderId");
                return;
            }
            deliveryChannelDTO.DefaultRiderId = defaultRiderId;
            log.LogMethodExit();
        }

        private void ChangeManualRiderAssignmentAllowed(bool manualRiderAssignmentAllowed)
        {
            log.LogMethodEntry(manualRiderAssignmentAllowed);
            if (deliveryChannelDTO.ManualRiderAssignmentAllowed == manualRiderAssignmentAllowed)
            {
                log.LogMethodExit(null, "No changes to manualRiderAssignmentAllowed");
                return;
            }
            deliveryChannelDTO.ManualRiderAssignmentAllowed = manualRiderAssignmentAllowed;
            log.LogMethodExit();
        }

        private void ChangeAutoAcceptOrders(bool autoAcceptOrders)
        {
            log.LogMethodEntry(autoAcceptOrders);
            if (deliveryChannelDTO.AutoAcceptOrders == autoAcceptOrders)
            {
                log.LogMethodExit(null, "No changes to AutoAcceptOrders");
                return;
            }
            deliveryChannelDTO.AutoAcceptOrders = autoAcceptOrders;
            log.LogMethodExit();
        }

        private void ChangeChannelAPIKey(string channelAPIKey)
        {
            log.LogMethodEntry(channelAPIKey);
            if (deliveryChannelDTO.ChannelAPIKey == channelAPIKey)
            {
                log.LogMethodExit(null, "No changes to ChannelAPIKey");
                return;
            }
            deliveryChannelDTO.ChannelAPIKey = channelAPIKey;
            log.LogMethodExit();
        }

        private void ChangeChannelAPIUrl(string channelAPIUrl)
        {
            log.LogMethodEntry(channelAPIUrl);
            if (deliveryChannelDTO.ChannelAPIUrl == channelAPIUrl)
            {
                log.LogMethodExit(null, "No changes to channelAPIUrl");
                return;
            }
            deliveryChannelDTO.ChannelAPIUrl = channelAPIUrl;
            log.LogMethodExit();
        }

        private void ChangeChannelName(string channelName)
        {
            log.LogMethodEntry(channelName);
            if (deliveryChannelDTO.ChannelName == channelName)
            {
                log.LogMethodExit(null, "No changes to channelName");
                return;
            }
            deliveryChannelDTO.ChannelName = channelName;
            log.LogMethodExit();
        }

        private void ChangeDeliveryChannelId(int deliveryChannelId)
        {
            log.LogMethodEntry(deliveryChannelId);
            if (deliveryChannelDTO.DeliveryChannelId == deliveryChannelId)
            {
                log.LogMethodExit(null, "No changes to deliveryChannelId");
                return;
            }
            deliveryChannelDTO.DeliveryChannelId = deliveryChannelId;
            log.LogMethodExit();
        }
        private void ChangeDeliveryIntegrationId(int deliveryIntegrationId)
        {
            log.LogMethodEntry(deliveryIntegrationId);
            if (deliveryChannelDTO.DeliveryIntegrationId == deliveryIntegrationId)
            {
                log.LogMethodExit(null, "No changes to deliveryIntegrationId");
                return;
            }
            deliveryChannelDTO.DeliveryIntegrationId = deliveryIntegrationId;
            log.LogMethodExit();
        }
        private void Validate(DeliveryChannelDTO inputDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            // Validation code here 
            // return validation exceptions
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>
        public DeliveryChannelBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id);
            LoadDeliveryChannelDTO(id, sqlTransaction);
            log.LogMethodExit();
        }


        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            Validate(this.deliveryChannelDTO);
            DeliveryChannelDataHandler deliveryChannelDataHandler = new DeliveryChannelDataHandler(sqlTransaction);
            if (deliveryChannelDTO.DeliveryChannelId < 0)
            {
                deliveryChannelDTO = deliveryChannelDataHandler.Insert(deliveryChannelDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                if (!string.IsNullOrEmpty(deliveryChannelDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("DeliveryChannels", deliveryChannelDTO.Guid, sqlTransaction);
                }
                deliveryChannelDTO.AcceptChanges();
            }
            else
            {
                if (deliveryChannelDTO.IsChanged)
                {
                    deliveryChannelDTO = deliveryChannelDataHandler.Update(deliveryChannelDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    if (!string.IsNullOrEmpty(deliveryChannelDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("DeliveryChannels", deliveryChannelDTO.Guid, sqlTransaction);
                    }
                    deliveryChannelDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
         
        internal DeliveryChannelDTO DeliveryChannelDTO
        {
            get
            {
                DeliveryChannelDTO result = new DeliveryChannelDTO(deliveryChannelDTO);
                result.IsChanged = deliveryChannelDTO.IsChanged;
                return result;
            }
        } 

    }

    /// <summary>
    /// OrderDetailListBL list class for order details
    /// </summary>
    public class DeliveryChannelListBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext; 
        internal DeliveryChannelListBL()
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = null; 
            log.LogMethodExit();
        }
        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public DeliveryChannelListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext; 
            log.LogMethodExit();
        } 
        /// <summary>
        /// GetDeliveryChannels
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<DeliveryChannelDTO> GetDeliveryChannels(List<KeyValuePair<DeliveryChannelDTO.SearchByParameters, string>> searchParameters,
                                           SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            DeliveryChannelDataHandler deliveryChannelDataHandler = new DeliveryChannelDataHandler(sqlTransaction);
            List<DeliveryChannelDTO> deliveryChannelDTOList = deliveryChannelDataHandler.GetDeliveryChannels(searchParameters);
            log.LogMethodExit(deliveryChannelDTOList);
            return deliveryChannelDTOList;
        }
        /// <summary>
        /// Gets the DeliveryChannelDTO List for deliveryIntegrationIdList
        /// </summary>
        /// <param name="deliveryIntegrationIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of DeliveryChannelDTO</returns>
        public List<DeliveryChannelDTO> GetDeliveryChannelDTOList(List<int> deliveryIntegrationIdList,
                                                         bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(deliveryIntegrationIdList, activeRecords, sqlTransaction);
            DeliveryChannelDataHandler deliveryChannelDataHandler = new DeliveryChannelDataHandler(sqlTransaction);
            List<DeliveryChannelDTO> deliveryChannelDTOList = deliveryChannelDataHandler.GetDeliveryChannelDTOList(deliveryIntegrationIdList, activeRecords);
            log.LogMethodExit(deliveryChannelDTOList);
            return deliveryChannelDTOList;
        }
        /// <summary>
        /// This method should be used to Save and Update the DeliveryChannels.
        /// </summary>
        public List<DeliveryChannelDTO> Save(List<DeliveryChannelDTO> deliveryChannelDTOList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<DeliveryChannelDTO> savedDeliveryChannelDTOList = new List<DeliveryChannelDTO>();
            if (deliveryChannelDTOList == null || deliveryChannelDTOList.Any() == false)
            {
                log.LogMethodExit(savedDeliveryChannelDTOList);
                return savedDeliveryChannelDTOList;
            }
            foreach (DeliveryChannelDTO onlineOrderDeliveryIntegrationDTO in deliveryChannelDTOList)
            {
                DeliveryChannelBL deliveryChanneBL = new DeliveryChannelBL(executionContext, onlineOrderDeliveryIntegrationDTO, sqlTransaction);
                deliveryChanneBL.Save(sqlTransaction);
                savedDeliveryChannelDTOList.Add(deliveryChanneBL.DeliveryChannelDTO);
            }
            log.LogMethodExit(savedDeliveryChannelDTOList);
            return savedDeliveryChannelDTOList;
        } 
    }
}
