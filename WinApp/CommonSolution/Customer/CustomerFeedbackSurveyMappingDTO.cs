/********************************************************************************************
 * Project Name - Customer Feedback Survey Mapping DTO
 * Description  - Data object of Customer FeedBack Survey Mapping Data Set Questions
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        02-Dec-2016   Raghuveera          Created 
 *2.70.2      19-Jul-2019    Girish Kundar       Modified : Added Constructor with required Parameter
 *                                                         and MasterEntityId field.
 *2.70.3       21-02-2020     Girish Kundar     Modified : 3 tier Changes for REST API                                                       
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// This is the Customer Feedback Survey Mapping Data Set Details data object class. This acts as data holder for the Cust Feedback Survey Mapping Set business object
    /// </summary>
    public class CustomerFeedbackSurveyMappingDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByCustomerFeedbackSurveyMappingParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByCustomerFeedbackSurveyMappingParameters
        {
            /// <summary>
            /// Search by CUST FB SURVEY MAP ID field
            /// </summary>
            CUST_FB_SURVEY_MAP_ID,
            /// <summary>
            /// Search by OBJECT NAME field
            /// </summary>
            OBJECT_NAME,
            /// <summary>
            /// Search by OBJECT ID field
            /// </summary>
            OBJECT_ID,
            /// <summary>
            /// Search by CUST FB SURVEY DATA SET ID LIST field
            /// </summary>
            CUST_FB_SURVEY_DATA_SET_ID_LIST,
            /// <summary>
            /// Search by CUST FB SURVEY DATA SET ID field
            /// </summary>
            CUST_FB_SURVEY_DATA_SET_ID,
            /// <summary>
            /// Search by LAST VISIT DATE field
            /// </summary>
            LAST_VISIT_DATE,
            /// <summary>
            /// Search by VISIT COUNT field
            /// </summary>
            VISIT_COUNT,
            /// <summary>
            /// Search by LAST CUST FB SURVEY DETAIL ID field
            /// </summary>
            LAST_CUST_FB_SURVEY_DETAIL_ID,
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

        private int custFbSurveyMapId;
        private string objectName;
        private int objectId;
        private int custFbSurveyDataSetId;
        private DateTime lastVisitDate;
        private int visitCount;
        private int lastCustFbSurveyDetailId;
        private bool isActive;
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

        private String dbDateFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomerFeedbackSurveyMappingDTO()
        {
            log.LogMethodEntry();
            custFbSurveyMapId = -1;
            objectId = -1;
            custFbSurveyDataSetId = -1;
            visitCount = 0;
            lastCustFbSurveyDetailId = -1;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public CustomerFeedbackSurveyMappingDTO(int custFbSurveyMapId, string objectName, int objectId, int custFbSurveyDataSetId,
                                                DateTime lastVisitDate, int visitCount, int lastCustFbSurveyDetailId, bool isActive)
            : this()
        {
            log.LogMethodEntry(custFbSurveyMapId, objectName, objectId, custFbSurveyDataSetId,
                               lastVisitDate, visitCount, lastCustFbSurveyDetailId, isActive);
            this.custFbSurveyMapId = custFbSurveyMapId;
            this.objectName = objectName;
            this.objectId = objectId;
            this.custFbSurveyDataSetId = custFbSurveyDataSetId;
            this.lastVisitDate = lastVisitDate;
            this.visitCount = visitCount;
            this.lastCustFbSurveyDetailId = lastCustFbSurveyDetailId;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CustomerFeedbackSurveyMappingDTO(int custFbSurveyMapId, string objectName, int objectId, int custFbSurveyDataSetId,
                                                DateTime lastVisitDate, int visitCount, int lastCustFbSurveyDetailId, bool isActive,
                                                string createdBy, DateTime creationDate, string lastUpdatedBy,
                                                DateTime lastUpdatedDate, int siteId, string guid, bool synchStatus, int masterEntityId)
            :this(custFbSurveyMapId, objectName, objectId, custFbSurveyDataSetId,
                               lastVisitDate, visitCount, lastCustFbSurveyDetailId, isActive)
        {
            log.LogMethodEntry(custFbSurveyMapId, objectName, objectId, custFbSurveyDataSetId,
                                                 lastVisitDate, visitCount, lastCustFbSurveyDetailId, isActive,
                                                 createdBy, creationDate, lastUpdatedBy,
                                                 lastUpdatedDate, siteId, guid, synchStatus, masterEntityId);
           
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
        /// Get/Set method of the CustFbSurveyDetailId field
        /// </summary>
        [DisplayName("Cust Fb Survey DataSet Id")]
        [ReadOnly(true)]
        public int CustFbSurveyMapId { get { return custFbSurveyMapId; } set { custFbSurveyMapId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ObjectName field
        /// </summary>
        [DisplayName("Object Name")]
        public string ObjectName { get { return objectName; } set { objectName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ObjectId field
        /// </summary>
        [DisplayName("ObjectId")]
        public int ObjectId { get { return objectId; } set { objectId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CustFbSurveyDataSetId field
        /// </summary>
        [DisplayName("CustFbSurveyDataSetId")]
        public int CustFbSurveyDataSetId { get { return custFbSurveyDataSetId; } set { custFbSurveyDataSetId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastVisitDate field
        /// </summary>
        [DisplayName("Last Visit Date")]
        public DateTime LastVisitDate { get { return lastVisitDate; } set { lastVisitDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the VisitCount field
        /// </summary>
        [DisplayName("Visit Count")]
        public int VisitCount { get { return visitCount; } set { visitCount = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastCustFbSurveyDetailId field
        /// </summary>
        [DisplayName("Last Cust Fb Survey Detail")]
        public int LastCustFbSurveyDetailId { get { return lastCustFbSurveyDetailId; } set { lastCustFbSurveyDetailId = value; this.IsChanged = true; } }

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


        /// <summary>
        /// Get/Set method of the LastVisitDateString field
        /// </summary>
        public string LastVisitDateString { get { return this.LastVisitDate.ToString(dbDateFormat); } set { this.LastVisitDate = DateTime.ParseExact(value, dbDateFormat, null); this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedDateString field
        /// </summary>
        public string LastUpdatedDateString { get { return this.LastUpdatedDate.ToString(dbDateFormat); } set { this.LastUpdatedDate = DateTime.ParseExact(value, dbDateFormat, null); this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreationDateString field
        /// </summary>
        public string CreationDateString { get { return this.CreationDate.ToString(dbDateFormat); } set { this.CreationDate = DateTime.ParseExact(value, dbDateFormat, null); this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || custFbSurveyMapId < 0;
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
