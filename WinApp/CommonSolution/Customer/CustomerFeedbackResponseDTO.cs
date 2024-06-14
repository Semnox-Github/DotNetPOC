/********************************************************************************************
 * Project Name - Customer Feed Back Response DTO
 * Description  - Data object of Customer FeedBack Survey Response
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        02-Dec-2016   Raghuveera          Created 
 *2.70.2        19-Jul-2019    Girish Kundar       Modified : Added Constructor with required Parameter
 *                                                         and MasterEntityId field.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// This is the Customer Feed Back Survey response data object class. This acts as data holder for the Customer Feed Back Survey Response business object
    /// </summary>
    public class CustomerFeedbackResponseDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByCustomerFeedbackResponseParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByCustomerFeedbackResponseParameters
        {
            /// <summary>
            /// Search by CUST_FB_RESPONSE_ID field
            /// </summary>
            CUST_FB_RESPONSE_ID,
            /// <summary>
            /// Search by CUST_FB_RESPONSE_ID field
            /// </summary>
            CUST_FB_RESPONSE_ID_LIST,
            /// <summary>
            /// Search by RESPONSE_NAME field
            /// </summary>
            RESPONSE_NAME,
            /// <summary>
            /// Search by RESPONSE_TYPE_ID field
            /// </summary>
            RESPONSE_TYPE_ID,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Master entity id field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int custFbResponseId;
        private string responseName;
        private int responseTypeId;
        private String responseType;// added because in android it will be difficult to get the responseType from lookup
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private string guid;
        private int masterEntityId;
        private bool synchStatus;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        List<CustomerFeedbackResponseValuesDTO> customerFeedbackResponseValuesDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomerFeedbackResponseDTO()
        {
            log.LogMethodEntry();
            custFbResponseId = -1;
            responseTypeId = -1;
            responseType = string.Empty;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public CustomerFeedbackResponseDTO(int custFbResponseId, string responseName, int responseTypeId, bool isActive )
            : this()
        {
            log.LogMethodEntry( custFbResponseId,  responseName,  responseTypeId, isActive);
            this.custFbResponseId = custFbResponseId;
            this.responseName = responseName;
            this.responseTypeId = responseTypeId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CustomerFeedbackResponseDTO(int custFbResponseId, string responseName, int responseTypeId, bool isActive,
                                        string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate,
                                        int siteId, string guid, bool synchStatus , int masterEntityId)
            :this(custFbResponseId, responseName, responseTypeId, isActive)
        {
            log.LogMethodEntry( custFbResponseId,  responseName,  responseTypeId,  isActive,
                                         createdBy,  creationDate,  lastUpdatedBy,  lastUpdatedDate,
                                         siteId,  guid,  synchStatus,  masterEntityId);
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
        /// Get/Set method of the ResponseValues
        /// </summary>
        [DisplayName("Response Id")]
        [ReadOnly(true)]
        public List<CustomerFeedbackResponseValuesDTO> CustomerFeedbackResponseValuesDTOList { get { return customerFeedbackResponseValuesDTOList; } set { customerFeedbackResponseValuesDTOList = value;  } }

        /// <summary>
        /// Get/Set method of the CustFbResponseId field
        /// </summary>
        [DisplayName("Response Id")]
        [ReadOnly(true)]
        public int CustFbResponseId { get { return custFbResponseId; } set { custFbResponseId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ResponseName field
        /// </summary>
        [DisplayName("Name")]
        public string ResponseName { get { return responseName; } set { responseName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ResponseTypeId field
        /// </summary>
        [DisplayName("Type Id")]
        public int ResponseTypeId { get { return responseTypeId; } set { responseTypeId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ResponseType
        /// </summary>
        public string ResponseType { get { return responseType; } set { responseType = value; this.IsChanged = true; } }

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
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (customerFeedbackResponseValuesDTOList != null &&
                    customerFeedbackResponseValuesDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                
                return false;
            }
        }

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
                    return notifyingObjectIsChanged || custFbResponseId < 0;
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
