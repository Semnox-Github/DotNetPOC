/********************************************************************************************
 * Project Name - Customer Feedback Survey Data Set DTO
 * Description  - Data object of Customer Feedback Survey Data Set DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        13-Dec-2016   Raghuveera          Created 
 *2.70.2      19-Jul-2019     Girish Kundar       Modified : Added Constructor with required Parameter
 *                                                           and MasterEntityId field.
 *2.70.3      20-Feb-2020     Girish Kundar       Modified : Added List<CustomerFeedbackSurveyDataDTO>  and
 *                                                           List<CustomerFeedbackSurveyMappingDTO>
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// This is the Customer Feedback Survey Data Set data object class. This acts as data holder for the Customer Feedback Survey Data Set business object
    /// </summary>
    public class CustomerFeedbackSurveyDataSetDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchByCustomerFeedbackSurveyDataSetParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByCustomerFeedbackSurveyDataSetParameters
        {
            /// <summary>
            /// Search by CUST FB SURVEY DATA SET ID field
            /// </summary>
            CUST_FB_SURVEY_DATA_SET_ID,
            /// <summary>
            /// Search by CUST FB SURVEY DATA SET ID field
            /// </summary>
            CUST_FB_SURVEY_DATA_SET_ID_LIST,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int custFbSurveyDataSetId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private List<CustomerFeedbackSurveyDataDTO> customerFeedbackSurveyDataDTOList;
        private List<CustomerFeedbackSurveyMappingDTO> customerFeedbackSurveyMappingDTOList ;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomerFeedbackSurveyDataSetDTO()
        {
            log.LogMethodEntry();
            custFbSurveyDataSetId = -1;
            siteId = -1;
            masterEntityId = -1;
            customerFeedbackSurveyDataDTOList = new List<CustomerFeedbackSurveyDataDTO>();
            customerFeedbackSurveyMappingDTOList = new List<CustomerFeedbackSurveyMappingDTO>();
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with REquired data fields
        /// </summary>
        public CustomerFeedbackSurveyDataSetDTO(int custFbSurveyDataSetId)
            : this()
        {
            log.LogMethodEntry(custFbSurveyDataSetId);
            this.custFbSurveyDataSetId = custFbSurveyDataSetId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CustomerFeedbackSurveyDataSetDTO(int custFbSurveyDataSetId, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                        DateTime lastUpdatedDate, string guid, int siteId, bool synchStatus, int masterEntityId)
            : this(custFbSurveyDataSetId)
        {
            log.LogMethodEntry(custFbSurveyDataSetId, createdBy, creationDate, lastUpdatedBy,
                               lastUpdatedDate, guid, siteId, synchStatus, masterEntityId);
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
        /// Get method of the SegmentCategoryId field
        /// </summary>
        [DisplayName("Data Set Id")]
        [ReadOnly(true)]
        public int CustFbSurveyDataSetId { get { return custFbSurveyDataSetId; } set { custFbSurveyDataSetId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastupdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }
        /// <summary>
        /// Get/Set method of the Siteid field
        /// </summary>
        [DisplayName("Site id")]
        [Browsable(false)]
        public int Siteid { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }
        /// <summary>
        /// Get/Set method of the customerFeedbackSurveyDataDTOList field
        /// </summary>
        public List<CustomerFeedbackSurveyDataDTO> CustomerFeedbackSurveyDataDTOList
        {
            get { return customerFeedbackSurveyDataDTOList; }
            set { customerFeedbackSurveyDataDTOList = value; }
        }

        /// <summary>
        /// Get/Set method of the customerFeedbackSurveyMappingDTOList field
        /// </summary>
        public List<CustomerFeedbackSurveyMappingDTO> CustomerFeedbackSurveyMappingDTOList
        {
            get { return customerFeedbackSurveyMappingDTOList; }
            set { customerFeedbackSurveyMappingDTOList = value; }
        }

        /// <summary>
        /// Returns whether the MessagingTriggerDTO changed or any of its MessagingTriggerCriteria DTO  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (customerFeedbackSurveyMappingDTOList != null &&
                  customerFeedbackSurveyMappingDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (customerFeedbackSurveyDataDTOList != null &&
                  customerFeedbackSurveyDataDTOList.Any(x => x.IsChanged))
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
                    return notifyingObjectIsChanged || custFbSurveyDataSetId < 0;
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
