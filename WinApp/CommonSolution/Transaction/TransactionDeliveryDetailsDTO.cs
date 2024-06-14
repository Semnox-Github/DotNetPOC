/********************************************************************************************
 * Project Name - Transaction
 * Description  - TrasactionDeliveryDetails DTO  
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.140.0      01-Jun-2021    Fiona         Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// TrasactionDeliveryDetailsDTO
    /// </summary>
    public class TransactionDeliveryDetailsDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int trasactionDeliveryDetailsId;
        private int transctionOrderDispensingId;
        private int riderId;
        private string externalRiderName;
        private string ridePhoneNumber;
        private int riderDeliveryStatus;
        private string remarks;
        private string externalSystemReference;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;

        /// <summary>
        /// 
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search By deliveryChannelId
            /// </summary>
            TRANSACTION_DELIVERY_DETAILS_ID,
            /// <summary>
            /// 
            /// </summary>
            TRANSACTION_ORDER_DISPENSING_ID,
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
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search By MASTER ENTITY ID
            /// </summary>
            EXTERNAL_SYSTEM_REFERENCE
        }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public TransactionDeliveryDetailsDTO()
        {
            log.LogMethodEntry();
            this.trasactionDeliveryDetailsId = -1;
            this.transctionOrderDispensingId = -1;
            this.riderId = 1;
            this.riderDeliveryStatus = -1;
            this.isActive = true;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parametized Constructor
        /// </summary>
        /// <param name="trasactionDeliveryDetailsId"></param>
        /// <param name="orderDispensingId"></param>
        /// <param name="riderId"></param>
        /// <param name="externalRiderName"></param>
        /// <param name="ridePhoneNumber"></param>
        /// <param name="riderDeliveryStatus"></param>
        /// <param name="remarks"></param>
        /// <param name="externalSystemReference"></param>
        /// <param name="isActive"></param>
        public TransactionDeliveryDetailsDTO(int trasactionDeliveryDetailsId, int orderDispensingId, int riderId, string externalRiderName, string ridePhoneNumber, int riderDeliveryStatus, string remarks,string externalSystemReference, bool isActive) : this()
        {
            log.LogMethodEntry(trasactionDeliveryDetailsId, orderDispensingId, riderId, externalRiderName, ridePhoneNumber, riderDeliveryStatus, remarks, externalSystemReference, isActive);
            this.trasactionDeliveryDetailsId = trasactionDeliveryDetailsId;
            this.transctionOrderDispensingId = orderDispensingId;
            this.riderId = riderId;
            this.externalRiderName = externalRiderName;
            this.ridePhoneNumber = ridePhoneNumber;
            this.riderDeliveryStatus = riderDeliveryStatus;
            this.remarks = remarks;
            this.externalSystemReference = externalSystemReference;
            this.isActive = isActive;
            log.LogMethodExit();
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="trasactionDeliveryDetailsId"></param>
       /// <param name="orderDispensingId"></param>
       /// <param name="riderId"></param>
       /// <param name="externalRiderName"></param>
       /// <param name="ridePhoneNumber"></param>
       /// <param name="riderDeliveryStatus"></param>
       /// <param name="remarks"></param>
       /// <param name="externalSystemReference"></param>
       /// <param name="isActive"></param>
       /// <param name="createdBy"></param>
       /// <param name="creationDate"></param>
       /// <param name="lastUpdatedBy"></param>
       /// <param name="lastUpdateDate"></param>
       /// <param name="guid"></param>
       /// <param name="siteId"></param>
       /// <param name="synchStatus"></param>
       /// <param name="masterEntityId"></param>
        public TransactionDeliveryDetailsDTO(int trasactionDeliveryDetailsId, int orderDispensingId, int riderId, string externalRiderName, string ridePhoneNumber, int riderDeliveryStatus, string remarks,string externalSystemReference, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, string guid, int siteId, bool synchStatus, int masterEntityId) 
            : this(trasactionDeliveryDetailsId, orderDispensingId, riderId, externalRiderName, ridePhoneNumber, riderDeliveryStatus, remarks, externalSystemReference, isActive)
        {
            log.LogMethodEntry(trasactionDeliveryDetailsId, orderDispensingId, riderId, externalRiderName, ridePhoneNumber, riderDeliveryStatus, remarks, externalSystemReference, isActive,  createdBy,  creationDate,  lastUpdatedBy,  lastUpdateDate,  guid,  siteId,  synchStatus,  masterEntityId);
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
        /// 
        /// </summary>
        /// <param name="transactionDeliveryDetailsDTO"></param>
        public TransactionDeliveryDetailsDTO(TransactionDeliveryDetailsDTO transactionDeliveryDetailsDTO)
            :this(transactionDeliveryDetailsDTO.TrasactionDeliveryDetailsId, 
                 transactionDeliveryDetailsDTO.TransctionOrderDispensingId,
                 transactionDeliveryDetailsDTO.RiderId,
                 transactionDeliveryDetailsDTO.ExternalRiderName,
                 transactionDeliveryDetailsDTO.RiderPhoneNumber,
                 transactionDeliveryDetailsDTO.RiderDeliveryStatus,
                 transactionDeliveryDetailsDTO.Remarks,
                 transactionDeliveryDetailsDTO.ExternalSystemReference,
                 transactionDeliveryDetailsDTO.IsActive,
                 transactionDeliveryDetailsDTO.CreatedBy,
                 transactionDeliveryDetailsDTO.creationDate,
                 transactionDeliveryDetailsDTO.LastUpdatedBy,
                 transactionDeliveryDetailsDTO.LastUpdateDate,
                 transactionDeliveryDetailsDTO.Guid,
                 transactionDeliveryDetailsDTO.SiteId,
                 transactionDeliveryDetailsDTO.SynchStatus,
                 transactionDeliveryDetailsDTO.MasterEntityId)
        {
            log.LogMethodEntry(transactionDeliveryDetailsDTO);
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of TrasactionDeliveryDetailsId
        /// </summary>
        public int TrasactionDeliveryDetailsId
        {
            get
            {
                return trasactionDeliveryDetailsId;
            }
            set
            {
                this.IsChanged = true;
                trasactionDeliveryDetailsId = value;
            }
        }
        /// <summary>
        /// Get/Set method of OrderDispensingId
        /// </summary>
        public int TransctionOrderDispensingId
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
        /// Get/Set method of OrderDispensingId
        /// </summary>
        public int RiderId
        {
            get
            {
                return riderId;
            }
            set
            {
                this.IsChanged = true;
                riderId = value;
            }
        }
        /// <summary>
        /// Get/Set method of RiderName
        /// </summary>
        public string ExternalRiderName
        {
            get
            {
                return externalRiderName;
            }
            set
            {
                this.IsChanged = true;
                externalRiderName = value;
            }
        }
        
        
        
        
        /// <summary>
        /// Get/Set method of RidePhoneNumber
        /// </summary>
        public string RiderPhoneNumber
        {
            get
            {
                return ridePhoneNumber;
            }
            set
            {
                this.IsChanged = true;
                ridePhoneNumber = value;
            }
        }
        /// <summary>
        /// Get/Set method of RiderDeliveryStatus
        /// </summary>
        public int RiderDeliveryStatus
        {
            get
            {
                return riderDeliveryStatus;
            }
            set
            {
                this.IsChanged = true;
                riderDeliveryStatus = value;
            }
        }
        /// <summary>
        ///  Get/Set method of Remarks
        /// </summary>
        public string Remarks
        {
            get
            {
                return remarks;
            }
            set
            {
                this.IsChanged = true;
                remarks = value;
            }
        }
        /// <summary>
        /// Get/Set method of the ExternalSystemReference field
        /// </summary>
        public string ExternalSystemReference
        {
            get
            {
                return externalSystemReference;
            }

            set
            {
                IsChanged = true;
                externalSystemReference = value;
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
                    return notifyingObjectIsChanged || trasactionDeliveryDetailsId < 0;
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
