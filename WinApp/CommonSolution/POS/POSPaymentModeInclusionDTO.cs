/********************************************************************************************
 * Project Name - POS
 * Description  - Data object of POSPaymentModeInclusion
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.90.0        13-Jun-2020   Girish kundar       Created
 *********************************************************************************************/
using Semnox.Parafait.Device.PaymentGateway;
using System;

namespace Semnox.Parafait.POS
{
    public class POSPaymentModeInclusionDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ID field
            /// </summary>
            POS_PAYMENTMODE_INCLUSION_ID,
            /// <summary>
            /// Search by  pos machine Id field
            /// </summary>
            POS_MACHINE_ID,
            /// <summary>
            /// Search by  pos machine Id field
            /// </summary>
            POS_MACHINE_ID_LIST,
            /// <summary>
            /// Search by  payment mode id field
            /// </summary>
            PAYMENT_MODE_ID,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Master entity id field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int posPaymentModeInclusionId;
        private int posMachineId;
        private int paymentModeId;
        private string friendlyName;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        PaymentModeDTO paymentModeDTO;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public POSPaymentModeInclusionDTO()
        {
            log.LogMethodEntry();
            posPaymentModeInclusionId = -1;
            posMachineId = -1;
            paymentModeId = -1;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            friendlyName = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public POSPaymentModeInclusionDTO(int posPaymentModeInclusionId, int posMachineId, int paymentModeId, string friendlyName ,bool isActive)
            : this()
        {
            log.LogMethodEntry(posPaymentModeInclusionId,  posMachineId,  paymentModeId,friendlyName ,isActive);
            this.posPaymentModeInclusionId = posPaymentModeInclusionId;
            this.posMachineId = posMachineId;
            this.paymentModeId = paymentModeId;
            this.friendlyName = friendlyName;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public POSPaymentModeInclusionDTO(int posPaymentModeInclusionId, int posMachineId, int paymentModeId, string friendlyName ,bool isActive
                                          ,string guid,int siteId, int masterEntityId, bool synchStatus ,
                                          string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate)
            : this(posPaymentModeInclusionId, posMachineId, paymentModeId, friendlyName ,isActive)
        {
            log.LogMethodEntry(posPaymentModeInclusionId,  posMachineId,  paymentModeId, friendlyName ,isActive, guid,siteId,masterEntityId,synchStatus,
                               createdBy,  creationDate,  lastUpdatedBy,  lastUpdateDate);

            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int POSPaymentModeInclusionId
        {
            get  {  return posPaymentModeInclusionId; }
            set  {  this.IsChanged = true; posPaymentModeInclusionId = value; }
        }

        public int POSMachineId
        {
            get { return posMachineId; }
            set { this.IsChanged = true; posMachineId = value; }
        }

        public int PaymentModeId
        {
            get { return paymentModeId; }
            set { this.IsChanged = true; paymentModeId = value; }
        }

        /// <summary>
        /// Get/Set method of the FriendlyName field
        /// </summary>
        public string FriendlyName
        {
            get { return friendlyName; }
            set { this.IsChanged = true; friendlyName = value; }
        }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive
        {
            get  {   return isActive;     }
            set  {  this.IsChanged = true; isActive = value;  }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get    {  return createdBy;   }
            set    { createdBy = value;   }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get    { return creationDate;   }
            set    { creationDate = value;  }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get  {   return lastUpdatedBy; }
            set  {  lastUpdatedBy = value; }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set {lastUpdateDate = value; }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get  {  return siteId;  }
            set  {  siteId = value; }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get  {  return masterEntityId;  }
            set {  this.IsChanged = true; masterEntityId = value;  }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get  { return synchStatus; }
            set  { synchStatus = value; }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get  {   return guid;   }
            set  {  this.IsChanged = true; guid = value; }
        }

        /// <summary>
        /// Get/Set method of the PaymentGateway field
        /// </summary>
        public PaymentModeDTO PaymentModeDTO
        {
            get { return paymentModeDTO; }
            set { paymentModeDTO = value; }
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
                    return notifyingObjectIsChanged || posPaymentModeInclusionId < 0;
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
