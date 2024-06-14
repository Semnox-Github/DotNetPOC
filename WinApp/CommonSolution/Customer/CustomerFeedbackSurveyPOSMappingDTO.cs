/********************************************************************************************
 * Project Name - Cust Feedback Survey POS Mapping DTO
 * Description  - Data object of Cust Feedback Survey POS Mapping Data Set Questions
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
    /// This is the Customer Feedback Survey POS Mapping Data Set Details data object class. This acts as data holder for the Cust Feedback Survey POS Mapping Set business object
    /// </summary>
    public class CustomerFeedbackSurveyPOSMappingDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByCustomerFeedbackSurveyPOSMappingParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByCustomerFeedbackSurveyPOSMappingParameters
        {
            /// <summary>
            /// Search by CUST_FB_SURVEY_POS_MAPPING_ID field
            /// </summary>
            CUST_FB_SURVEY_POS_MAPPING_ID,
            /// <summary>
            /// Search by POS_MACHINE_ID field
            /// </summary>
            POS_MACHINE_ID,
            /// <summary>
            /// Search by CUST_FB_SURVEY_ID field
            /// </summary>
            CUST_FB_SURVEY_ID,
            /// <summary>
            /// Search by CUST_FB_SURVEY_ID field
            /// </summary>
            CUST_FB_SURVEY_ID_LIST,
            /// <summary>
            /// Search by IS_ACTIVE field
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
        private int custFbSurveyPOSMappingId;
        private int pOSMachineId;
        private int custFbSurveyId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private CustomerFeedbackSurveyDTO customerFeedbackSurveyDTO;
       // private  List<Semnox.Parafait.POS.POSMachineDTO> posMachineDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomerFeedbackSurveyPOSMappingDTO()
        {
            log.LogMethodEntry();
            custFbSurveyPOSMappingId = -1;
            pOSMachineId = -1;
            custFbSurveyId = -1;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            customerFeedbackSurveyDTO = new CustomerFeedbackSurveyDTO();
           // posMachineDTOList = new List<Semnox.Parafait.POS.POSMachineDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public CustomerFeedbackSurveyPOSMappingDTO(int custFbSurveyPOSMappingId, int pOSMachineId, int custFbSurveyId, bool isActive)
            : this()
        {
            log.LogMethodEntry(custFbSurveyPOSMappingId, pOSMachineId, custFbSurveyId, isActive);
            this.custFbSurveyPOSMappingId = custFbSurveyPOSMappingId;
            this.pOSMachineId = pOSMachineId;
            this.custFbSurveyId = custFbSurveyId;
            this.isActive = isActive;
            log.LogMethodExit();
        }



        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CustomerFeedbackSurveyPOSMappingDTO(int custFbSurveyPOSMappingId, int pOSMachineId, int custFbSurveyId, bool isActive,
                                            string createdBy, DateTime creationDate, string lastUpdatedBy,
                                            DateTime lastUpdatedDate, int siteId, string guid, bool synchStatus,int masterEntityId)
            :this(custFbSurveyPOSMappingId, pOSMachineId, custFbSurveyId, isActive)
        {
            log.LogMethodEntry(custFbSurveyPOSMappingId,  pOSMachineId,  custFbSurveyId,  isActive,
                               createdBy,  creationDate,  lastUpdatedBy,
                               lastUpdatedDate,  siteId,  guid,  synchStatus, masterEntityId);
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
        /// Get/Set method of the CustomerFeedbackSurveyDTO field
        /// </summary>
        public CustomerFeedbackSurveyDTO CustomerFeedbackSurveyDTO { get { return customerFeedbackSurveyDTO; } set { customerFeedbackSurveyDTO = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CustFbSurveyDetailId field
        /// </summary>
        [DisplayName("POS Mapping Id")]
        [ReadOnly(true)]
        public int CustFbSurveyPOSMappingId { get { return custFbSurveyPOSMappingId; } set { custFbSurveyPOSMappingId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the POSMachineId field
        /// </summary>
        [DisplayName("POS Machine")]
        public int POSMachineId { get { return pOSMachineId; } set { pOSMachineId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CustFbSurveyId field
        /// </summary>
        [DisplayName("Survey")]
        public int CustFbSurveyId { get { return custFbSurveyId; } set { custFbSurveyId = value; this.IsChanged = true; } }

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
        /// Get/Set method of the POSMachinesDTOList field
        /// </summary>
        //public List<Semnox.Parafait.POS.POSMachineDTO> POSMachineDTOList { get { return posMachineDTOList; } set { posMachineDTOList = value; this.IsChanged = true; } }
        /// <summary>
        /// Returns whether the CustomerFeedbackSurveyPOSMappingDTO changed or any of its PosMachinesDTOList  are changed
        /// </summary>
        //public bool IsChangedRecursive
        //{
        //    get
        //    {
        //        if (IsChanged)
        //        {
        //            return true;
        //        }
        //        if (posMachineDTOList != null &&
        //             posMachineDTOList.Any(x => x.IsChanged))
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //}
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
                    return notifyingObjectIsChanged || custFbSurveyPOSMappingId < 0;
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
