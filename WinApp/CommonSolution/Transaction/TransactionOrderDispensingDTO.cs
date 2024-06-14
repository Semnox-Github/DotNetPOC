/********************************************************************************************
 * Project Name - Transaction
 * Description  - TransctionOrderDispensing DTO  
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.140.0      02-Jun-2021    Fiona         Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// TransctionOrderDispensingDTO
    /// </summary>
    public class TransactionOrderDispensingDTO
    {
        

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        private int transctionOrderDispensingId;
        private int transactionId;
        private int orderDispensingTypeId;
        private int deliveryChannelId;
        private DateTime? scheduledDispensingTime;
        private ReConformationStatus reconfirmationOrder;
        private ReConformationStatus reConfirmPreparation;
        private int deliveryAddressId;
        private int deliveryContactId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string externalSystemReference;
        private string deliveryChannelCustomerReferenceNo;
        private List<TransactionDeliveryDetailsDTO> transactionDeliveryDetailsDTOList;

       


        /// <summary>
        /// TransctionOrderDispensingDTO
        /// </summary>
        public TransactionOrderDispensingDTO()
        {
            log.LogMethodEntry();
            this.transctionOrderDispensingId = -1;
            this.transactionId = -1;
            this.orderDispensingTypeId = -1;
            this.deliveryChannelId = -1;
            this.deliveryAddressId = -1;
            this.deliveryContactId = -1;
            this.isActive = true;
            this.siteId = -1;
            this.masterEntityId = -1;
            this.externalSystemReference = string.Empty;
            this.deliveryChannelCustomerReferenceNo = string.Empty;
            log.LogMethodExit();
        }
        /// <summary>
        /// Required fields Contructor
        /// </summary> 
        public TransactionOrderDispensingDTO(int transctionOrderDispensingId, int transactionId, int orderDispensingTypeId, int deliveryChannelId, DateTime? scheduledDispensingTime, ReConformationStatus reconfirmationOrder, ReConformationStatus reConfirmPreparation,int deliveryAddressId,int deliveryContactId, bool isActive, string externalSystemReference,
            string deliveryChannelCustomerReferenceNo)
        {
            log.LogMethodEntry(transctionOrderDispensingId, transactionId, orderDispensingTypeId, deliveryChannelId,  scheduledDispensingTime, reconfirmationOrder, reConfirmPreparation, deliveryAddressId, deliveryContactId, 
             isActive, externalSystemReference, deliveryChannelCustomerReferenceNo);
            this.transctionOrderDispensingId = transctionOrderDispensingId;
            this.transactionId = transactionId;
            this.orderDispensingTypeId = orderDispensingTypeId;
            this.deliveryChannelId = deliveryChannelId;
            this.scheduledDispensingTime = scheduledDispensingTime;
            this.reconfirmationOrder = reconfirmationOrder;
            this.reConfirmPreparation = reConfirmPreparation;
            this.deliveryAddressId = deliveryAddressId;
            this.deliveryContactId = deliveryContactId;
            this.isActive = isActive;
            this.externalSystemReference = externalSystemReference;
            this.deliveryChannelCustomerReferenceNo = deliveryChannelCustomerReferenceNo;
            log.LogMethodExit();
        }
        /// <summary>
        /// TransactionOrderDispensingDTO
        /// </summary> 
        public TransactionOrderDispensingDTO(int transctionOrderDispensingId, int transactionId, int orderDispensingTypeId, int deliveryChannelId, DateTime? scheduledDispensingTime, ReConformationStatus reconfirmationOrder, ReConformationStatus reConfirmPreparation, int deliverAddressId,int deliveryContactId, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, string guid, int siteId, bool synchStatus, int masterEntityId, string externalSystemReference, string deliveryChannelCustomerReferenceNo) : 
            this(transctionOrderDispensingId, transactionId, orderDispensingTypeId, deliveryChannelId, scheduledDispensingTime, reconfirmationOrder, reConfirmPreparation, deliverAddressId, deliveryContactId, isActive, externalSystemReference, deliveryChannelCustomerReferenceNo)
        {
            log.LogMethodEntry(transctionOrderDispensingId, transactionId, orderDispensingTypeId, deliveryChannelId, scheduledDispensingTime, reconfirmationOrder, reConfirmPreparation, deliverAddressId, deliveryContactId, isActive, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, guid, siteId, synchStatus, masterEntityId, externalSystemReference, deliveryChannelCustomerReferenceNo);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }
        /// <summary>
        /// TransctionOrderDispensingDTO
        /// </summary>
        /// <param name="transctionOrderDispensingDTO"></param>
        public TransactionOrderDispensingDTO(TransactionOrderDispensingDTO transctionOrderDispensingDTO)
            :this(transctionOrderDispensingDTO.TransactionOrderDispensingId, 
                transctionOrderDispensingDTO.TransactionId, 
                transctionOrderDispensingDTO.OrderDispensingTypeId,
                transctionOrderDispensingDTO.DeliveryChannelId, 
                transctionOrderDispensingDTO.ScheduledDispensingTime, 
                transctionOrderDispensingDTO.ReconfirmationOrder,
                transctionOrderDispensingDTO.ReConfirmPreparation, 
                transctionOrderDispensingDTO.DeliveryAddressId, 
                transctionOrderDispensingDTO.DeliveryContactId, 
                transctionOrderDispensingDTO.isActive,
                transctionOrderDispensingDTO.CreatedBy, 
                transctionOrderDispensingDTO.CreationDate, 
                transctionOrderDispensingDTO.lastUpdatedBy, 
                transctionOrderDispensingDTO.LastUpdateDate,
                transctionOrderDispensingDTO.Guid, 
                transctionOrderDispensingDTO.siteId, 
                transctionOrderDispensingDTO.SynchStatus, 
                transctionOrderDispensingDTO.MasterEntityId,
                transctionOrderDispensingDTO.externalSystemReference,
                transctionOrderDispensingDTO.deliveryChannelCustomerReferenceNo)
        {
            log.LogMethodEntry(transctionOrderDispensingDTO);            
            if(transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList != null)
            {
                transactionDeliveryDetailsDTOList = new List<TransactionDeliveryDetailsDTO>();
                foreach(TransactionDeliveryDetailsDTO transactionDeliveryDetailsDTO in transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList)
                {
                    transactionDeliveryDetailsDTOList.Add(new TransactionDeliveryDetailsDTO(transactionDeliveryDetailsDTO));
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// 
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search By transctionOrderDispensingId
            /// </summary>
            TRANSACTION_ORDER_DISPENSING_ID,
            /// <summary>
            /// Search By transactionId
            /// </summary>
            TRANSACTION_ID,

            /// <summary>
            /// Search By ACTIVE FLAG
            /// </summary>
            IS_ACTIVE,

            /// <summary>
            /// Search By SITE ID
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search By MASTER ENTITY ID
            /// </summary>
            MASTER_ENTITY_ID
        }
        /// <summary>
        /// 
        /// </summary>
        public enum ReConformationStatus
        {
            /// <summary>
            /// YES
            /// </summary>
            YES,
            /// <summary>
            /// NO
            /// </summary>
            NO,
            /// <summary>
            /// CONFIRMED
            /// </summary>
            CONFIRMED
        }

        /// <summary>
        /// ReConformationStatusToString
        /// </summary>
        /// <param name="deliverySTatus"></param>
        /// <returns></returns>
        public static string ReConformationStatusToString(ReConformationStatus deliverySTatus)
        {
            log.LogMethodEntry(deliverySTatus);
            string returnValue = "No";
            switch(deliverySTatus)
            {
                case ReConformationStatus.YES:
                    returnValue= "YES";
                    break;
                case ReConformationStatus.CONFIRMED:
                    returnValue= "CONFIRMED";
                    break;
                case ReConformationStatus.NO:
                    returnValue= "NO";
                    break;
                default:
                    log.LogMethodExit();
                    returnValue= "NO";
                    break;
            }
            return returnValue;
        }
        /// <summary>
        /// ReConformationStatusFromString
        /// </summary>
        /// <param name="deliveryStatus"></param>
        /// <returns></returns>
        public static ReConformationStatus ReConformationStatusFromString(string deliveryStatus)
        {
            log.LogMethodEntry(deliveryStatus);
            ReConformationStatus returnValue = ReConformationStatus.NO;
            switch (deliveryStatus)
            {
                case "YES":
                    returnValue = ReConformationStatus.YES;
                    break;
                case "CONFIRMED":
                    returnValue = ReConformationStatus.CONFIRMED;
                    break;
                case "NO":
                    returnValue = ReConformationStatus.NO;
                    break;
                default:
                    returnValue = ReConformationStatus.NO;
                    break;
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        ///  Get/Set method of the TransctionOrderDispensingId field
        /// </summary>
        public int TransactionOrderDispensingId
        {
            get
            {
                return transctionOrderDispensingId;
            }
            set
            {
                this.IsChanged = true;
                transctionOrderDispensingId = value;
            }
        }
        /// <summary>
        ///  Get/Set method of the TransactionId field
        /// </summary>
        public int TransactionId
        {
            get
            {
                return transactionId;
            }
            set
            {
                this.IsChanged = true;
                transactionId = value;
            }
        }
        /// <summary>
        ///  Get/Set method of the OrderDispensingTypeId field
        /// </summary>
        public int OrderDispensingTypeId
        {
            get
            {
                return orderDispensingTypeId;
            }
            set
            {
                this.IsChanged = true;
                orderDispensingTypeId = value;
            }
        }
        /// <summary>
        ///  Get/Set method of the DeliveryChannelId field
        /// </summary>
        public int DeliveryChannelId
        {
            get
            {
                return deliveryChannelId;
            }
            set
            {
                this.IsChanged = true;
                deliveryChannelId = value;
            }
        }
        /// <summary>
        ///  Get/Set method of the ScheduledDispensingTime field
        /// </summary>
        public DateTime? ScheduledDispensingTime
        {
            get
            {
                return scheduledDispensingTime;
            }
            set
            {
                this.IsChanged = true;
                scheduledDispensingTime = value;
            }
        }
        /// <summary>
        ///  Get/Set method of the ReconfirmationOrder field
        /// </summary>
        public ReConformationStatus ReconfirmationOrder
        {
            get
            {
                return reconfirmationOrder;
            }
            set
            {
                this.IsChanged = true;
                reconfirmationOrder = value;
            }
        }
        /// <summary>
        /// Get/Set method of the ReConfirmPreparation field
        /// </summary>
        public ReConformationStatus ReConfirmPreparation
        {
            get
            {
                return reConfirmPreparation;
            }
            set
            {
                this.IsChanged = true;
                reConfirmPreparation = value;
            }
        }
        /// <summary>
        /// DeliverAddressId
        /// </summary>
        public int DeliveryAddressId
        {
            get
            {
                return deliveryAddressId;
            }
            set
            {
                this.IsChanged = true;
                deliveryAddressId = value;
            }
        }
        /// <summary>
        /// DeliveryContactId
        /// </summary>
        public int DeliveryContactId
        {
            get
            {
                return deliveryContactId;
            }
            set
            {
                this.IsChanged = true;
                deliveryContactId = value;
            }
        }
        /// <summary>
        /// ExternalSystemReference
        /// </summary>
        public string ExternalSystemReference
        {
            get
            {
                return externalSystemReference;
            }
            set
            {
                this.IsChanged = true;
                externalSystemReference = value;
            }
        }
        /// <summary>
        /// deliveryChannelCustomerReferenceNo
        /// </summary>
        public string DeliveryChannelCustomerReferenceNo
        {
            get
            {
                return deliveryChannelCustomerReferenceNo;
            }
            set
            {
                this.IsChanged = true;
                deliveryChannelCustomerReferenceNo = value;
            }
        }
        
        /// <summary>
        /// Get/Set method of the TransactionDeliveryDetailsDTOList field
        /// </summary>
        public List<TransactionDeliveryDetailsDTO> TransactionDeliveryDetailsDTOList
        {
            get
            {
                return transactionDeliveryDetailsDTOList;
            }
            set
            {
                transactionDeliveryDetailsDTOList = value;
            }
        }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                this.IsChanged = true;
                isActive = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                createdBy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                creationDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {
                lastUpdatedBy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }
            set
            {
                lastUpdateDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get
            {
                return siteId;
            }
            set
            {
                siteId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }
            set
            {
                this.IsChanged = true;
                masterEntityId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
            set
            {
                synchStatus = value;
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
                this.IsChanged = true;
                guid = value;
            }
        }


        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || transctionOrderDispensingId < 0;
                }
            }

            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }

        /// <summary>
        /// Returns whether the TransctionOrderDispensingDTO changed or any of its Child  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (transactionDeliveryDetailsDTOList != null &&
                   transactionDeliveryDetailsDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }



    }
}
