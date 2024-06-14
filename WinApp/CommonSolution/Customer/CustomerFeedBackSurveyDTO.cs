/********************************************************************************************
 * Project Name - Customer Feed Back Survey DTO
 * Description  - Data object of Customer FeedBack Survey
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        02-Dec-2016   Raghuveera          Created 
 *2.70.2      19-Jul-2019   Girish Kundar       Modified : Added Constructor with required Parameter 
 *                                                          and MasterEntityId field.  
 *2.70.3      23-Feb-2020   Girish Kundar       Modified : Added List<CustomerFeedbackSurveyPOSMappingDTO> as child list
 *                                                   
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// This is the Customer Feed Back Survey data object class. This acts as data holder for the Customer Feed Back Survey business object
    /// </summary>
    public class CustomerFeedbackSurveyDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByCustomerFeedBackSurveyParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByCustomerFeedBackSurveyParameters
        {
            /// <summary>
            /// Search by CUST FB SURVEY ID field
            /// </summary>
            CUST_FB_SURVEY_ID,
            /// <summary>
            /// Search by QUESTION NO field
            /// </summary>
            SURVEY_NAME,
            /// <summary>
            /// Search by QUESTION field
            /// </summary>
            FROM_DATE,
            /// <summary>
            /// Search by CUST FB RESPONSE ID field
            /// </summary>
            TO_DATE,
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
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by CUST_FB_SURVEY_ID field
            /// </summary>
            CUST_FB_SURVEY_ID_LIST
        }

        private int custFbSurveyId;
        private string surveyName;
        private DateTime fromDate;
        private DateTime toDate;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool isResponseMandatory;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private List<CustomerFeedbackSurveyDetailsDTO>  customerFeedbackSurveyDetailsDTOList;
        private List<CustomerFeedbackSurveyPOSMappingDTO> customerFeedbackSurveyPOSMappingDTOList;
        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomerFeedbackSurveyDTO()
        {
            log.LogMethodEntry();
            custFbSurveyId = -1;
            isActive = true;
            isResponseMandatory = false;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public CustomerFeedbackSurveyDTO(int custFbSurveyId, string surveyName, DateTime fromDate,
                                         DateTime toDate, bool isResponseMandatory, bool isActive)
            : this()
        {
            log.LogMethodEntry(custFbSurveyId, surveyName, fromDate, toDate, isResponseMandatory, isActive);
            this.custFbSurveyId = custFbSurveyId;
            this.surveyName = surveyName;
            this.fromDate = fromDate;
            this.toDate = toDate;
            this.isResponseMandatory = isResponseMandatory;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CustomerFeedbackSurveyDTO(int custFbSurveyId, string surveyName, DateTime fromDate, 
                                         DateTime toDate, bool isResponseMandatory, bool isActive,string createdBy, DateTime creationDate,
                                         string lastUpdatedBy, DateTime lastUpdatedDate, int siteId, string guid,
                                         bool synchStatus , int masterEntityId)
            :this(custFbSurveyId, surveyName, fromDate, toDate, isResponseMandatory, isActive)
        {
            log.LogMethodEntry( custFbSurveyId,  surveyName,  fromDate,  toDate, isResponseMandatory, isActive,
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


        ///// <summary>
        ///// Get/Set method of the SurveyQuestions field
        ///// </summary>
        public List<CustomerFeedbackSurveyDetailsDTO> SurveyDetails { get { return customerFeedbackSurveyDetailsDTOList; } set { customerFeedbackSurveyDetailsDTOList = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CustFbSurveyId field
        /// </summary>
        [DisplayName("Customer Fb Survey Id")]
        [ReadOnly(true)]
        public int CustFbSurveyId { get { return custFbSurveyId; } set { custFbSurveyId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SurveyName field
        /// </summary>
        [DisplayName("Survey Name")]
        public string SurveyName { get { return surveyName; } set { surveyName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FromDate field
        /// </summary>
        [DisplayName("From Date")]
        public DateTime FromDate { get { return fromDate; } set { fromDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CustFbResponseId field
        /// </summary>
        [DisplayName("ToDate")]
        public DateTime ToDate { get { return toDate; } set { toDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsResponseMandatory field
        /// </summary>
        public bool IsResponseMandatory { get { return isResponseMandatory; } set { isResponseMandatory = value; this.IsChanged = true; } }

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
        /// Get/Set method of the customerFeedbackSurveyPOSMappingDTO List field
        /// </summary>
        public List<CustomerFeedbackSurveyPOSMappingDTO> CustomerFeedbackSurveyPOSMappingDTOList
        {
            get { return customerFeedbackSurveyPOSMappingDTOList; }
            set { customerFeedbackSurveyPOSMappingDTOList = value;  }
        }

        /// <summary>
        /// Returns whether the CustomerFeedbackSurveyDTO changed or any of its CustomerFeedbackSurveyDetails  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                //if ( customerFeedbackSurveyDetailsDTOList != null &&
                //     customerFeedbackSurveyDetailsDTOList.Any(x => x.IsChanged))
                //{
                //    return true;
                //}
                if (customerFeedbackSurveyPOSMappingDTOList != null &&
                    customerFeedbackSurveyPOSMappingDTOList.Any(x => x.IsChanged))
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
                    return notifyingObjectIsChanged || custFbSurveyId < 0;
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
