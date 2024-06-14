/********************************************************************************************
 * Project Name - Customer Feed Back Response Values DTO
 * Description  - Data object of Customer FeedBack Survey Response value
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        02-Dec-2016   Raghuveera          Created
 *2.70.2       19-Jul-2019    Girish Kundar     Modified : Added Constructor with required Parameter
 *                                                       and masterEntityField.
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// This is the Customer Feed Back Survey response value data object class. This acts as data holder for the Customer Feed Back Survey Response value business object
    /// </summary>
    public class CustomerFeedbackResponseValuesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByCustomerFeedbackResponseValuesParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByCustomerFeedbackResponseValuesParameters
        {
            /// <summary>
            /// Search by CUST FB RESPONSE VALUE ID field
            /// </summary>
            CUST_FB_RESPONSE_VALUE_ID,
            /// <summary>
            /// Search by CUST FB RESPONSE ID field
            /// </summary>
            CUST_FB_RESPONSE_ID,
            /// <summary>
            /// Search by CUST FB RESPONSE ID field
            /// </summary>
            CUST_FB_RESPONSE_ID_LIST,
            /// <summary>
            /// Search by RESPONSE VALUE field
            /// </summary>
            RESPONSE_VALUE,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int custFbResponseValueId;
        private string responseValue;
        private int custFbResponseId;
        private byte[] image;
        private bool isActive;
        private decimal? score;
        private int sortOrder;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomerFeedbackResponseValuesDTO()
        {
            log.LogMethodEntry();
            custFbResponseId = -1;
            custFbResponseValueId = -1;
            isActive = true;
            image = null;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required  data fields
        /// </summary>
        public CustomerFeedbackResponseValuesDTO(int custFbResponseValueId, string responseValue, int custFbResponseId, byte[] image, decimal? score, int sortOrder, bool isActive)
            :this()
        {
            log.LogMethodEntry(custFbResponseValueId, responseValue, custFbResponseId, image, isActive);
            this.custFbResponseValueId = custFbResponseValueId;
            this.responseValue = responseValue;
            this.custFbResponseId = custFbResponseId;
            this.image = image;
            this.score = score;
            this.sortOrder = sortOrder;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CustomerFeedbackResponseValuesDTO(int custFbResponseValueId, string responseValue, int custFbResponseId, byte[] image, decimal? score, int sortOrder,
                                                 bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate,
                                                 int siteId, string guid, bool synchStatus, int masterEntityId)
            :this(custFbResponseValueId, responseValue, custFbResponseId, image, score, sortOrder, isActive)
        {
            log.LogMethodEntry(custFbResponseValueId, responseValue, custFbResponseId, image, isActive,
                               score, sortOrder, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate,
                                         siteId, guid, synchStatus, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CustFbResponseValueId field
        /// </summary>
        [DisplayName("Response Value Id")]
        [ReadOnly(true)]
        public int CustFbResponseValueId { get { return custFbResponseValueId; } set { custFbResponseValueId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ResponseValue field
        /// </summary>
        [DisplayName("Response Value")]
        public string ResponseValue { get { return responseValue; } set { responseValue = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CustFbResponseId field
        /// </summary>
        [DisplayName("Response Id")]
        public int CustFbResponseId { get { return custFbResponseId; } set { custFbResponseId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Score field
        /// </summary>
        public decimal? Score { get { return score; } set { score = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Order field
        /// </summary>
        public int SortOrder { get { return sortOrder; } set { sortOrder = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Image field
        /// </summary>
        [DisplayName("Image")]
        public byte[] Image { get { return image; } set { image = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }
       
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || custFbResponseValueId < 0;
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
