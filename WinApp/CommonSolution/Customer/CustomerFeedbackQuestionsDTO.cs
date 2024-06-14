/********************************************************************************************
 * Project Name - Customer Feed Back Questions DTO
 * Description  - Data object of Customer FeedBack Survey Questions
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        02-Dec-2016   Raghuveera          Created 
 *2.70.2      19-Jul-2019   Girish Kundar       Modified : Added Constructor with required Parameter
 *                                                       and masterEntityField.
 *2.70.3      19-Jul-2019    Girish Kundar      Modified : Added Child Lists for API changes                                                      
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// This is the Customer Feedback Questions data object class. This acts as data holder for the Cust Feedback Questions business object
    /// </summary>
    public class CustomerFeedbackQuestionsDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByCustomerFeedbackQuestionsParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByCustomerFeedbackQuestionsParameters
        {
            /// <summary>
            /// Search by CUST_FB_QUESTION_ID field
            /// </summary>
            CUST_FB_QUESTION_ID,
            /// <summary>
            /// Search by CUST_FB_QUESTION_ID field
            /// </summary>
            CUST_FB_QUESTION_ID_LIST,
            /// <summary>
            /// Search by QUESTION_NO field
            /// </summary>
            QUESTION_NO,
            /// <summary>
            /// Search by QUESTION field
            /// </summary>
            QUESTION,
            /// <summary>
            /// Search by CUST_FB_RESPONSE_ID field
            /// </summary>
            CUST_FB_RESPONSE_ID,
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

        private int custFbQuestionId;
        private string questionNo;
        private string question;
        private int custFbResponseId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private bool notifyingObjectIsChanged;
        private int masterEntityId;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private CustomerFeedbackResponseDTO customerFeedbackResponseDTO;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomerFeedbackQuestionsDTO()
        {
            log.LogMethodEntry();
            custFbResponseId = -1;
            custFbQuestionId = -1;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public CustomerFeedbackQuestionsDTO(int custFbQuestionId, string questionNo, string question,
                                            int custFbResponseId, bool isActive )
            :this()
        {
            log.LogMethodEntry(custFbQuestionId,  questionNo,  question, custFbResponseId,  isActive);
            this.custFbQuestionId = custFbQuestionId;
            this.questionNo = questionNo;
            this.question = question;
            this.custFbResponseId = custFbResponseId;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CustomerFeedbackQuestionsDTO(int custFbQuestionId, string questionNo, string question, int custFbResponseId, bool isActive,
                                        string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate,
                                        int siteId, string guid, bool synchStatus , int masterEntityId)
            :this(custFbQuestionId, questionNo, question, custFbResponseId, isActive)
        {
            log.LogMethodEntry(custFbQuestionId, questionNo, question, custFbResponseId, isActive,
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
        /// Get/Set method of the QuestionResponse
        /// </summary>
        public CustomerFeedbackResponseDTO QuestionResponse { get { return customerFeedbackResponseDTO; } set { customerFeedbackResponseDTO = value; } }

        /// <summary>
        /// Get/Set method of the CustFbQuestionId field
        /// </summary>
        [DisplayName("Question Id")]
        [ReadOnly(true)]
        public int CustFbQuestionId { get { return custFbQuestionId; } set { custFbQuestionId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the QuestionNo field
        /// </summary>
        [DisplayName("Question No")]
        public string QuestionNo { get { return questionNo; } set { questionNo = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Question field
        /// </summary>
        [DisplayName("Question")]
        public string Question { get { return question; } set { question = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CustFbResponseId field
        /// </summary>
        [DisplayName("Response")]
        public int CustFbResponseId { get { return custFbResponseId; } set { custFbResponseId = value; this.IsChanged = true; } }

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
        public int SiteId { get { return siteId; } set { siteId = value;  } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId     {  get {  return masterEntityId; } set {this.IsChanged = true;masterEntityId = value;  }  }
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
                    return notifyingObjectIsChanged || custFbQuestionId < 0;
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
