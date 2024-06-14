/********************************************************************************************
 * Project Name - Customer Feed Back Questions DTO
 * Description  - Data object of Customer FeedBack Survey Questions
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
using System.ComponentModel;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// This is the Customer Feed back Survey Details data object class. This acts as data holder for the Cust Feedback Questions business object
    /// </summary>
    public class CustomerFeedbackSurveyDetailsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByCustomerFeedbackSurveyDetailsParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByCustomerFeedbackSurveyDetailsParameters
        {
            /// <summary>
            /// Search by CUST_FB_QUESTION_ID field
            /// </summary>
            CUST_FB_SURVEY_DETAIL_ID,
            /// <summary>
            /// Search by CUST_FB_SURVEY_ID field
            /// </summary>
            CUST_FB_SURVEY_ID,
            /// <summary>
            /// Search by CUST_FB_QUESTION_ID field
            /// </summary>
            CUST_FB_QUESTION_ID,
            /// <summary>
            /// Search by CUST_FB_QUESTION_ID field
            /// </summary>
            NEXT_QUESTION_ID,
            /// <summary>
            /// Search by CRITERIA_ID field
            /// </summary>
            CRITERIA_ID,
            /// <summary>
            /// Search by CRITERIA_VALUE field
            /// </summary>
            CRITERIA_VALUE,
            /// <summary>
            /// Search by CUST_FB_RESPONSE_ID field
            /// </summary>
            CUST_FB_RESPONSE_ID,
            /// <summary>
            /// Search by EXPECTED_CUST_FB_RESPONSE_VALUE_ID field
            /// </summary>
            EXPECTED_CUST_FB_RESPONSE_VALUE_ID,
            /// <summary>
            /// Search by IS_RESPONSE_MANDATORY field
            /// </summary>
            IS_RESPONSE_MANDATORY,
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
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by CUST_FB_SURVEY_ID_LIST field
            /// </summary>
            CUST_FB_SURVEY_ID_LIST,
        }
        private int custFbSurveyDetailId;
        private int custFbSurveyId;
        private int custFbQuestionId;
        private int nextQuestionId;
        private int criteriaId;
        private string criteriaValue;
        private int expectedCustFbResponseValueId;
        private string expectedCustFbResponseValue;
        private int custFbResponseId;
        private bool isResponseMandatory;
        private bool isRecur;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private CustomerFeedbackResponseValuesDTO customerFeedbackResponseValuesDTO;
        private CustomerFeedbackQuestionsDTO surveyQuestion;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomerFeedbackSurveyDetailsDTO()
        {
            log.LogMethodEntry();
            custFbSurveyDetailId = -1;
            custFbSurveyId = -1;
            custFbQuestionId = -1;
            nextQuestionId = -1;
            criteriaId = -1;
            custFbResponseId = -1;
            expectedCustFbResponseValueId = -1;
            isResponseMandatory = true;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public CustomerFeedbackSurveyDetailsDTO(int custFbSurveyDetailId, int custFbSurveyId, int custFbQuestionId, int nextQuestionId, int criteriaId,
                                               string criteriaValue, int expectedCustFbResponseValueId, int custFbResponseId, bool isResponseMandatory,
                                               bool isRecur, bool isActive)
            : this()
        {
            log.LogMethodEntry(custFbSurveyDetailId, custFbSurveyId, custFbQuestionId, nextQuestionId, criteriaId,
                               criteriaValue, expectedCustFbResponseValueId, custFbResponseId, isResponseMandatory, isRecur, isActive);
            this.custFbSurveyDetailId = custFbSurveyDetailId;
            this.custFbSurveyId = custFbSurveyId;
            this.custFbQuestionId = custFbQuestionId;
            this.nextQuestionId = nextQuestionId;
            this.criteriaId = criteriaId;
            this.criteriaValue = criteriaValue;
            this.expectedCustFbResponseValueId = expectedCustFbResponseValueId;
            this.custFbResponseId = custFbResponseId;
            this.isResponseMandatory = isResponseMandatory;
            this.isRecur = isRecur;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CustomerFeedbackSurveyDetailsDTO(int custFbSurveyDetailId, int custFbSurveyId, int custFbQuestionId, int nextQuestionId, int criteriaId,
                                               string criteriaValue, int expectedCustFbResponseValueId, int custFbResponseId, bool isResponseMandatory, bool isRecur, bool isActive,
                                               string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate,
                                               int siteId, string guid, bool synchStatus, int masterEntityId)
            : this(custFbSurveyDetailId, custFbSurveyId, custFbQuestionId, nextQuestionId, criteriaId,
                               criteriaValue, expectedCustFbResponseValueId, custFbResponseId, isResponseMandatory, isRecur, isActive)
        {
            log.LogMethodEntry(custFbSurveyDetailId, custFbSurveyId, custFbQuestionId, nextQuestionId, criteriaId,
                               criteriaValue, expectedCustFbResponseValueId, custFbResponseId, isResponseMandatory, isRecur, isActive,
                               createdBy, creationDate, lastUpdatedBy, lastUpdatedDate,
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
        /// Get/Set method of the SurveyQuestions field
        /// </summary>
        public CustomerFeedbackQuestionsDTO SurveyQuestion { get { return surveyQuestion; } set { surveyQuestion = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the customerFeedbackResponseValuesDTO field
        /// </summary>
        public CustomerFeedbackResponseValuesDTO CustomerFeedbackResponseValuesDTO { get { return customerFeedbackResponseValuesDTO; } set { customerFeedbackResponseValuesDTO = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CustFbSurveyDetailId field
        /// </summary>
        [DisplayName("Survey Detail Id")]
        [ReadOnly(true)]
        public int CustFbSurveyDetailId { get { return custFbSurveyDetailId; } set { custFbSurveyDetailId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CustFbSurveyId field
        /// </summary>
        [DisplayName("Survey")]
        public int CustFbSurveyId { get { return custFbSurveyId; } set { custFbSurveyId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CustFbQuestionId field
        /// </summary>
        [DisplayName("Question")]
        public int CustFbQuestionId { get { return custFbQuestionId; } set { custFbQuestionId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the NextQuestionId field
        /// </summary>
        [DisplayName("Next Question")]
        public int NextQuestionId { get { return nextQuestionId; } set { nextQuestionId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CriteriaId field
        /// </summary>
        [DisplayName("Criteria")]
        public int CriteriaId { get { return criteriaId; } set { criteriaId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CriteriaValue field
        /// </summary>
        [DisplayName("Criteria Value")]
        public string CriteriaValue { get { return criteriaValue; } set { criteriaValue = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CustFbResponseId field
        /// </summary>
        [DisplayName("Response")]
        public int CustFbResponseId { get { return custFbResponseId; } set { custFbResponseId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ExpectedCustFbResponseValueId field
        /// </summary>
        [DisplayName("Expected Response Value Id")]
        public int ExpectedCustFbResponseValueId { get { return expectedCustFbResponseValueId; } set { expectedCustFbResponseValueId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ExpectedCustFbResponseValueId field
        /// </summary>
        [DisplayName("Expected Response Value")]
        //[ReadOnly(true)]
        public string ExpectedCustFbResponseValue { get { return expectedCustFbResponseValue; } set { expectedCustFbResponseValue = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsResponseMandatory field
        /// </summary>
        [DisplayName("IsResponseMandatory?")]
        public bool IsResponseMandatory { get { return isResponseMandatory; } set { isResponseMandatory = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsRecur field
        /// </summary>
        [DisplayName("IsRecur?")]
        public bool IsRecur { get { return isRecur; } set { isRecur = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive?")]
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
                    return notifyingObjectIsChanged || custFbSurveyDetailId < 0;
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
