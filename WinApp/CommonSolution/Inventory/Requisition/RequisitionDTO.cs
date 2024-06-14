/***********************************************************************************************
 * Project Name - Requisition DTO
 * Description  - Data object of Requisition 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 ************************************************************************************************
 *2.70        15-Jul-2019   Dakshakh raj      Modified : Added Parameterized costrustor,
 *                                                       Added MasterEntityId field
 *2.100.0     17-Sep-2020   Deeksha           Modified to handle Is changed property return true when requisitionId < 0
 *2.110.0     11-Dec-2020   Mushahid Faizan   Modified : Inventory Redesign changes
 ************************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Semnox.Parafait.logging;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// RequisitionDTO  class
    /// </summary>
    public class RequisitionDTO
    {
        private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByRequisitionTypeParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByRequisitionParameters
        {
            /// <summary>
            /// Search by REQUISITION ID field
            /// </summary>
            REQUISITION_ID,
            /// <summary>
            /// Search by TEMPLATE ID field
            /// </summary>
            TEMPLATE_ID,
            /// <summary>
            /// Search by REQUISITION NUMBER field
            /// </summary>
            REQUISITION_NUMBER,
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
            /// Search by SITE ID  field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by EXPECTED RECEIPT DATE  field
            /// </summary>
            EXPECTED_RECEIPT_DATE,
            /// <summary>
            /// Search by GUID  field
            /// </summary>
            GUID ,
            /// <summary>
            /// Search by FROM SITE ID  field
            /// </summary>
            FROM_SITE_ID ,
            /// <summary>
            /// Search by TO SITE ID  field
            /// </summary>
            TO_SITE_ID,
            /// <summary>
            /// Search by ORIGINAL REFERENCE_GUID  field
            /// </summary>
            ORIGINAL_REFERENCE_GUID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by REQUISITION_ID field
            /// </summary>
            REQUISITION_ID_LIST,
            GUID_ID_LIST
        }

        private int requisitionId;
        private  string requisitionNo;
        private int templateId;
        private int requisitionType;
        private string documentTypeName;
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
        private DateTime lastUpdatedAt;
        private string Guid;
        private int site_id;
        private bool synchStatus;
        private int fromSiteId;
        private int toSiteId;
        private string originalReferenceGUID;
        private int masterEntityId;
        private List<RequisitionLinesDTO> requisitionLinesListDTO;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public RequisitionDTO()
        {
            log.LogMethodEntry();
            isActive = true;
            fromSiteId = -1;
            toSiteId = -1;
            masterEntityId = -1;
            requisitionId = -1;
            requisitionType = -1;
            requestingDept = -1;
            FromDepartment = -1;
            toDepartment = -1;
            templateId = -1;
            requisitionLinesListDTO = new List<RequisitionLinesDTO>(); 
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public RequisitionDTO(int requisitionIdPassed, string requisitionNumber, int templateIdPassed, int typePassed, int reqDeptPassed,
                              int fromDeptPassed, int toDeptPassed, double valuePassed, bool IsactivePassed, string remarksPassed, string status,
                              DateTime requiredByDatePassed, int site_id, string documentTypeNamePassed, int fromSiteId, int toSiteId, string originalReferenceGUID, List<RequisitionLinesDTO> requisitionLinesListDTO)
            :this()
        {
            log.LogMethodEntry( requisitionIdPassed,  requisitionNumber,  templateIdPassed,  typePassed,  reqDeptPassed,  fromDeptPassed, toDeptPassed,  valuePassed,
                                IsactivePassed, remarksPassed,  status,  requiredByDatePassed, site_id,  documentTypeNamePassed,  fromSiteId,  toSiteId, originalReferenceGUID, requisitionLinesListDTO);
            this.requisitionId = requisitionIdPassed;
            this.requisitionNo = requisitionNumber;
            this.templateId = templateIdPassed;
            this.requisitionType = typePassed;
            this.requestingDept = reqDeptPassed;
            this.fromDepartment = fromDeptPassed;
            this.toDepartment = toDeptPassed;
            this.estimatedValue = valuePassed;
            this.isActive = IsactivePassed;
            this.remarks = remarksPassed;
            this.status = status;
            this.requiredByDate = requiredByDatePassed;
            this.site_id = site_id;
            this.documentTypeName = documentTypeNamePassed;
            this.requisitionLinesListDTO = requisitionLinesListDTO;
            this.fromSiteId = fromSiteId;
            this.toSiteId = toSiteId;
            this.originalReferenceGUID = originalReferenceGUID;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public RequisitionDTO(int requisitionIdPassed, string requisitionNumber, int templateIdPassed, int typePassed, int reqDeptPassed,
                              int fromDeptPassed, int toDeptPassed, double valuePassed, bool IsactivePassed, string remarksPassed, string status,
                              DateTime requiredByDatePassed, int site_id, string documentTypeNamePassed, int fromSiteId, int toSiteId,  
                               string originalReferenceGUID, string createdBy, DateTime createdDate, string lastUpdatedBy, DateTime lastUpdatedAt,
                              bool synchStatus, string Guid, int masterEntityId, List<RequisitionLinesDTO> requisitionLinesListDTO)
            :this(requisitionIdPassed, requisitionNumber, templateIdPassed, typePassed, reqDeptPassed, fromDeptPassed, toDeptPassed, valuePassed, IsactivePassed,
                  remarksPassed, status, requiredByDatePassed, site_id, documentTypeNamePassed, fromSiteId, toSiteId,  originalReferenceGUID,requisitionLinesListDTO)
        {
            log.LogMethodEntry(createdBy, createdDate, lastUpdatedBy, lastUpdatedAt, synchStatus, Guid,masterEntityId);
            this.synchStatus = synchStatus;
            this.Guid = Guid;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.createdDate = createdDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedAt = lastUpdatedAt;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Requisition Id fields
        /// </summary>
        public int RequisitionId { get { return requisitionId; } set { requisitionId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Template Id fields
        /// </summary>
        public int TemplateId { get { return templateId; } set { templateId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Requisition Number fields
        /// </summary>
        public string RequisitionNo { get { return requisitionNo; } set { requisitionNo = value; IsChanged = true; } }
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
        public int SiteId { get { return site_id; } set { site_id = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DocumentTypeName fields
        /// </summary>
        public string DocumentTypeName { get { return documentTypeName; } set { documentTypeName = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the createdBy fields
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value;  } }

        /// <summary>
        /// Get/Set method of the CreatedDate fields
        /// </summary>
        public DateTime CreatedDate { get { return createdDate; } set { createdDate = value;  } }
        /// <summary>
        /// Get/Set method of the lastUpdatedBy fields
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value;  } }

        /// <summary>
        /// Get/Set method of the lastUpdatedAt fields
        /// </summary>
        public DateTime LastUpdatedAt { get { return lastUpdatedAt; } set { lastUpdatedAt = value;  } }
        /// <summary>
        /// Get/Set method of the Guid fields
        /// </summary>
        public string GUID { get { return Guid; } set { Guid = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the FromSiteId fields
        /// </summary>
        public int FromSiteId { get { return fromSiteId; } set { fromSiteId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ToSiteId fields
        /// </summary>
        public int ToSiteId { get { return toSiteId; } set { toSiteId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the OriginalReferenceGUID fields
        /// </summary>
        public string OriginalReferenceGUID { get { return originalReferenceGUID; } set { originalReferenceGUID = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RequisitionLinesListDTO fields
        /// </summary>
        public List<RequisitionLinesDTO> RequisitionLinesListDTO { get { return requisitionLinesListDTO; } set { requisitionLinesListDTO = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>

        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
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
                    return notifyingObjectIsChanged || requisitionId < 0;
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
        /// Returns whether the RequisitionDTO changed or any of its RequisitionDTO childeren are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (requisitionLinesListDTO != null &&

                   requisitionLinesListDTO.Any(x => x.IsChanged))
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
            {
                log.LogMethodEntry();
                this.IsChanged = false;
                log.LogMethodExit();
            }
        }
    }
}
