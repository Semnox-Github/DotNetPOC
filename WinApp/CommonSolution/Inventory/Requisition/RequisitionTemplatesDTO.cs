/***********************************************************************************************
 * Project Name - Requisition Templates DTO
 * Description  - Data object of Requisition 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 ************************************************************************************************
 *2.70        16-Jul-2019   Dakshakh raj      Modified : Added Parameterized constructor,
 *                                                       Added MasterEntityId ,SynchStatus field
 *2.100.0     17-Sep-2020   Deeksha           Modified to handle Is changed property return true when requisitionId < 0
 *2.110.0     11-Dec-2020   Mushahid Faizan    Modified : Web Inventory Changes
 ************************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Semnox.Parafait.logging;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// RequisitionTemplatesDTO
    /// </summary>
    public class RequisitionTemplatesDTO
    {
       private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByRequisitionTypeParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByReqTemplatesTypeParameters
        {
            /// <summary>
            /// Search by TEMPLATE ID field
            /// </summary>
            TEMPLATE_ID,
            /// <summary>
            /// Search by TEMPLATE NAME field
            /// </summary>
            TEMPLATE_NAME,
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by REQUISITION TYPE field
            /// </summary>
            REQUISITION_TYPE,
            /// <summary>
            /// Search by STATUS field
            /// </summary>
            STATUS,
            /// <summary>
            /// Search by SITEID field
            /// </summary>
            SITEID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int templateId;
        private string templateName;
        private int requisitionType;
        private int requestingDept;
        private int fromDepartment;
        private int toDepartment;
        private DateTime requiredByDate;
        private double estimatedValue;
        private bool isActive;
        private string remarks;
        private string createdBy;
        private string status;
        private DateTime createdDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string Guid;
        private int site_id;
       // private string uom;
        private bool synchStatus;
        private int masterEntityId;
        private List<RequisitionTemplateLinesDTO> requisitionTemplateLinesListDTO;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public RequisitionTemplatesDTO()
        {
            log.LogMethodEntry();
            templateId = -1;
            requisitionType = -1;
            requestingDept = -1;
            fromDepartment = -1;
            toDepartment = -1;
            isActive = true;
            masterEntityId = -1;
            site_id = -1;
            requisitionTemplateLinesListDTO = new List<RequisitionTemplateLinesDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary> //double valuePassed,
        public RequisitionTemplatesDTO(int templateIdPassed, int typePassed, string namePassed, int reqDeptPassed, int fromDeptPassed,
                                       int toDeptPassed, bool IsactivePassed, string remarksPassed, string status, DateTime requiredByDatePassed)
            :this()
        {

            log.LogMethodEntry( templateIdPassed,  typePassed,  namePassed,  reqDeptPassed,  fromDeptPassed,
                                toDeptPassed,  IsactivePassed,  remarksPassed,  status,  requiredByDatePassed);
            this.templateId = templateIdPassed;
            this.templateName = namePassed;
            this.requisitionType = typePassed;
            this.requestingDept = reqDeptPassed;
            this.fromDepartment = fromDeptPassed;
            this.toDepartment = toDeptPassed;
            //this.estimatedValue = valuePassed;
            this.isActive = IsactivePassed;
            this.remarks = remarksPassed;
            this.status = status;
            this.requiredByDate = requiredByDatePassed;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary> //double valuePassed,
        public RequisitionTemplatesDTO(int templateIdPassed, int typePassed, string namePassed, int reqDeptPassed, int fromDeptPassed, int toDeptPassed,
                                       bool IsactivePassed, string remarksPassed, string status, DateTime requiredByDatePassed, string createdBy,
                                       DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, string Guid, int siteId, bool synchStatus, int masterEntityId)
            :this(templateIdPassed, typePassed, namePassed, reqDeptPassed, fromDeptPassed,toDeptPassed, IsactivePassed, remarksPassed, status, requiredByDatePassed)
        {
            log.LogMethodEntry(createdBy,  creationDate,  lastUpdatedBy,  lastUpdatedDate, Guid,  siteId,  synchStatus,  masterEntityId);
            this.createdBy = createdBy;
            this.createdDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.Guid = Guid;
            this.site_id = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Template Id fields
        /// </summary>
        public int TemplateId { get { return templateId; } set { templateId = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TemplateName fields
        /// </summary>
        public string TemplateName { get { return templateName; } set { templateName = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RequisitionType fields
        /// </summary>
        public int RequisitionType { get { return requisitionType; } set { requisitionType = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RequestingDept fields
        /// </summary>
        public int RequestingDept { get { return requestingDept; } set { requestingDept = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FromDepartment fields
        /// </summary>
        public int FromDepartment { get { return fromDepartment; } set { fromDepartment = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ToDepartment fields
        /// </summary>
        public int ToDepartment { get { return toDepartment; } set { toDepartment = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RequiredByDate fields
        /// </summary>
        public DateTime RequiredByDate { get { return requiredByDate; } set { requiredByDate = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the EstimatedValue fields
        /// </summary>
        public double EstimatedValue { get { return estimatedValue; } set { estimatedValue = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the isActive fields
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Remarks fields
        /// </summary>
        public string Remarks { get { return remarks; } set { remarks = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Status fields
        /// </summary>
        public string Status { get { return status; } set { status = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId fields
        /// </summary>
        public int SiteId { get { return site_id; } set { site_id = value; } }

        /// <summary>
        /// Get/Set method of the createdBy fields
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreatedDate fields
        /// </summary>
        public DateTime CreatedDate { get { return createdDate; } set { createdDate = value; } }
        /// <summary>
        /// Get/Set method of the lastUpdatedBy fields
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value;  } }

        /// <summary>
        /// Get/Set method of the lastUpdatedAt fields
        /// </summary>
        public DateTime lastUpdateDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }
        /// <summary>
        /// Get/Set method of the Guid fields
        /// </summary>
        public string GUID { get { return Guid; } set { Guid = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RequisitionTemplateLinesListDTO fields
        /// </summary>
        public List<RequisitionTemplateLinesDTO> RequisitionTemplateLinesListDTO { get { return requisitionTemplateLinesListDTO; } set { requisitionTemplateLinesListDTO = value; this.IsChanged = true; } }
        /// <summary>

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
                    return notifyingObjectIsChanged || templateId < 0;
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
        /// Returns whether the RequisitionTemplatesDTO changed or any of its RequisitionTemplatesDTO childeren are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (requisitionTemplateLinesListDTO != null &&

                   requisitionTemplateLinesListDTO.Any(x => x.IsChanged))
                {
                    return true;
                }

                return false;

            }
        }

        /// <summary>
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
