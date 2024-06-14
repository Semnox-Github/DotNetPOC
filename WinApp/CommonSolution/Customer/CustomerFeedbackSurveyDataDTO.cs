/********************************************************************************************
 * Project Name - Customer Feed Back Survey Data Set DTO
 * Description  - Data object of Customer FeedBack  Survey Data Set Questions
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        02-Dec-2016   Raghuveera     Created
 *2.70.2        19-Jul-2019   Girish Kundar  Modified : Added Constructor with required Parameter 
 *                                                    and MasterEntityId field.
 *2.70.3       21-02-2020     Girish Kundar     Modified : 3 tier Changes for REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// This is the Customer Feed back Survey Data Details data object class. This acts as data holder for the Cust Feedback Survey Data business object
    /// </summary>
    public class CustomerFeedbackSurveyDataDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByCustomerFeedbackSurveyDataParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByCustomerFeedbackSurveyDataParameters
        {
            /// <summary>
            /// Search by CUST FB SURVEY DATA ID field
            /// </summary>
            CUST_FB_SURVEY_DATA_ID,
            /// <summary>
            /// Search by CUST FB SURVEY DATASET ID LISt field
            /// </summary>
            CUST_FB_SURVEY_DATASET_ID_LIST,
            /// <summary>
            /// Search by CUST FB SURVEY DATASET ID field
            /// </summary>
            CUST_FB_SURVEY_DATASET_ID,
            /// <summary>
            /// Search by CUST FB SURVEY DETAIL ID field
            /// </summary>
            CUST_FB_SURVEY_DETAIL_ID,
            /// <summary>
            /// Search by CUST FB RESPONSE VALUE ID field
            /// </summary>
            CUST_FB_RESPONSE_VALUE_ID,
            /// <summary>
            /// Search by CUST FB RESPONSE TEXT field
            /// </summary>
            CUST_FB_RESPONSE_TEXT,
            /// <summary>
            /// Search by CUST FB RESPONSE DATE field
            /// </summary>
            CUST_FB_RESPONSE_DATE,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int custFbSurveyDataId;
        private int custFbSurveyDataSetId;
        private int custFbSurveyDetailId;
        private int custFbResponseValueId;
        private string custFbResponseText;
        private DateTime custFbResponseDate;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOList; 
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        private String dbDateFormat = "yyyy-MM-dd HH:mm:ss";

        //String custFbResponseDateString;
        //String creationDateString;
        //String lastUpdatedDateString;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomerFeedbackSurveyDataDTO()
        {
            log.LogMethodEntry();
            custFbSurveyDataSetId = -1;
            custFbSurveyDetailId = -1;
            custFbResponseValueId = -1;
            custFbSurveyDataId = -1;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            customerFeedbackSurveyDetailsDTOList = new List<CustomerFeedbackSurveyDetailsDTO>();
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public CustomerFeedbackSurveyDataDTO(int custFbSurveyDataId, int custFbSurveyDataSetId, int custFbSurveyDetailId, int custFbResponseValueId, string custFbResponseText,
                                            DateTime custFbResponseDate, bool isActive)
            : this()
        {
            log.LogMethodEntry(custFbSurveyDataId, custFbSurveyDataSetId, custFbSurveyDetailId, custFbResponseValueId, custFbResponseText,
                                             custFbResponseDate, isActive);
            this.custFbSurveyDataId = custFbSurveyDataId;
            this.custFbSurveyDataSetId = custFbSurveyDataSetId;
            this.custFbSurveyDetailId = custFbSurveyDetailId;
            this.custFbResponseValueId = custFbResponseValueId;
            this.custFbResponseText = custFbResponseText;
            this.custFbResponseDate = custFbResponseDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CustomerFeedbackSurveyDataDTO(int custFbSurveyDataId, int custFbSurveyDataSetId, int custFbSurveyDetailId, int custFbResponseValueId, string custFbResponseText,
                                            DateTime custFbResponseDate, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                            DateTime lastUpdatedDate, int siteId, string guid, bool synchStatus ,int masterEntityId)
            :this(custFbSurveyDataId, custFbSurveyDataSetId, custFbSurveyDetailId, custFbResponseValueId, custFbResponseText,
                                             custFbResponseDate, isActive)
        {
            log.LogMethodEntry( custFbSurveyDataId,  custFbSurveyDataSetId,  custFbSurveyDetailId,  custFbResponseValueId,  custFbResponseText,
                                             custFbResponseDate,  isActive,  createdBy,  creationDate,  lastUpdatedBy,
                                             lastUpdatedDate,  siteId,  guid,  synchStatus,  masterEntityId);
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
        /// Get/Set method of the CustFbSurveyDataId field
        /// </summary>
        [DisplayName("Customer Fb Survey Data Id")]
        [ReadOnly(true)]
        public int CustFbSurveyDataId { get { return custFbSurveyDataId; } set { custFbSurveyDataId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CustFbSurveyDataSetId field
        /// </summary>
        [DisplayName("Customer Fb Survey DataSet Id")]
        [ReadOnly(true)]
        public int CustFbSurveyDataSetId { get { return custFbSurveyDataSetId; } set { custFbSurveyDataSetId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CustFbSurveyDetailId field
        /// </summary>
        [DisplayName("Customer Fb Survey Detail")]
        public int CustFbSurveyDetailId { get { return custFbSurveyDetailId; } set { custFbSurveyDetailId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CustFbResponseValueId field
        /// </summary>
        [DisplayName("Customer Fb Response Value")]
        public int CustFbResponseValueId { get { return custFbResponseValueId; } set { custFbResponseValueId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CustFbResponseText field
        /// </summary>
        [DisplayName("Customer Fb Response Text")]
        public string CustFbResponseText { get { return custFbResponseText; } set { custFbResponseText = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CustFbResponseDate field
        /// </summary>
        [DisplayName("CustFbResponseDate")]
        public DateTime CustFbResponseDate { get { return custFbResponseDate; } set { custFbResponseDate = value; this.IsChanged = true; } }

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
        public int MasterEntityId {  get   {return masterEntityId;  }   set { this.IsChanged = true;masterEntityId = value;  }  }
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
                    return notifyingObjectIsChanged || custFbSurveyDataId < 0;
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
        /// Get/Set method of the CustFbResponseDateString field
        /// </summary>
        public string CustFbResponseDateString { get { return this.CustFbResponseDate.ToString(dbDateFormat); } set { this.CustFbResponseDate = DateTime.ParseExact(value, dbDateFormat, null); this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedDateString field
        /// </summary>
        public string LastUpdatedDateString { get { return this.LastUpdatedDate.ToString(dbDateFormat); } set { this.LastUpdatedDate = DateTime.ParseExact(value, dbDateFormat, null); this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreationDateString field
        /// </summary>
        public string CreationDateString { get { return this.CreationDate.ToString(dbDateFormat); } set { this.CreationDate = DateTime.ParseExact(value, dbDateFormat, null); this.IsChanged = true; } }
      
        /// <summary>
        /// Get/Set method of the CheckInDetailDTOList field
        /// </summary>
        public List<CustomerFeedbackSurveyDetailsDTO> CustomerFeedbackSurveyDetailsDTOList
        {
            get { return customerFeedbackSurveyDetailsDTOList; }
            set { customerFeedbackSurveyDetailsDTOList = value; }
        }

        /// <summary>
        /// Returns whether the CustomerFeedbackSurveyDataDTO changed or any of its CustomerFeedbackSurveyDetails  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if ( customerFeedbackSurveyDetailsDTOList != null &&
                    customerFeedbackSurveyDetailsDTOList.Any(x => x.IsChanged))
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
