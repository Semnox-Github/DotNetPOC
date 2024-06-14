/********************************************************************************************
 * Project Name - Device
 * Description  - CompatiablePaymentModesDTO 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.140.0    15-Aug-2021    Fiona         Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class CompatiablePaymentModesDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int id;
        private int paymentModeId;
        private int compatiablePaymentModeId;
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
        /// SearchByParameters
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search By id
            /// </summary>
            ID,

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
            /// Search By PAYMENT_MODE_ID
            /// </summary>
            PAYMENT_MODE_ID
        }
        /// <summary>
        /// CompatiablePaymentModesDTO
        /// </summary>
        public CompatiablePaymentModesDTO()
        {
            log.LogMethodEntry();
            this.id = -1;
            this.paymentModeId = -1;
            this.compatiablePaymentModeId = -1;
            this.isActive = true;
            this.siteId = -1;
            this.masterEntityId = -1;
            log.LogMethodExit();
        }
        /// <summary>
        /// CompatiablePaymentModesDTO with paramters
        /// </summary> 
        public CompatiablePaymentModesDTO(int id, int paymentModeId, int compatiablePaymentModeId, bool isActive):this()
        {
            log.LogMethodEntry(id, paymentModeId, compatiablePaymentModeId, isActive);
            this.id = id;
            this.paymentModeId = paymentModeId;
            this.compatiablePaymentModeId = compatiablePaymentModeId;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        /// <summary>
        /// CompatiablePaymentModesDTO with all parameters
        /// </summary> 
        public CompatiablePaymentModesDTO(int id, int paymentModeId, int compatiablePaymentModeId, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, string guid, int siteId, bool synchStatus, int masterEntityId) : 
            this(id, paymentModeId, compatiablePaymentModeId, isActive)
        {
            log.LogMethodEntry(id,  paymentModeId,  compatiablePaymentModeId,  isActive,  createdBy,  creationDate,  lastUpdatedBy,  lastUpdateDate,  guid,  siteId,  synchStatus,  masterEntityId);
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
        /// CompatiablePaymentModesDTO copy constructor
        /// </summary>
        /// <param name="compatiablePaymentModesDTO"></param>
        public CompatiablePaymentModesDTO(CompatiablePaymentModesDTO compatiablePaymentModesDTO)
            :this (compatiablePaymentModesDTO.id, compatiablePaymentModesDTO.paymentModeId, compatiablePaymentModesDTO.compatiablePaymentModeId, compatiablePaymentModesDTO.isActive,
                 compatiablePaymentModesDTO.createdBy, compatiablePaymentModesDTO.creationDate, compatiablePaymentModesDTO.lastUpdatedBy, compatiablePaymentModesDTO.lastUpdateDate
                 , compatiablePaymentModesDTO.guid, compatiablePaymentModesDTO.siteId, compatiablePaymentModesDTO.synchStatus, compatiablePaymentModesDTO.masterEntityId )
        {
            log.LogMethodEntry(compatiablePaymentModesDTO); 
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
                this.IsChanged = true;
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
                this.IsChanged = true;
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
                this.IsChanged = true;
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
                    return notifyingObjectIsChanged || id < 0;
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
