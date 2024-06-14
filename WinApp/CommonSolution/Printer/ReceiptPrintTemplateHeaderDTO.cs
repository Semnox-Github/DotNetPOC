/********************************************************************************************
 * Project Name - Receipt Print Template Header DTO
 * Description  - DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        16-Sep-2018      Mathew Ninan   Created 
 *2.70.2        18-Jul-2019      Deeksha        Modifications as per 3 tier standard.
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Printer
{
    /// <summary>
    /// DTO class for ReceiptPrintTemplateHeader
    /// </summary>
    public class ReceiptPrintTemplateHeaderDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        private int templateId;
        private string templateName;
        private string fontName;
        private int? fontSize;
        private bool isActive;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private int site_id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private IList<ReceiptPrintTemplateDTO> lstReceiptPrintTemplateDTO;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public ReceiptPrintTemplateHeaderDTO()
        {
            log.LogMethodEntry();
            templateId = -1;
            site_id = -1;
            templateName = string.Empty;
            isActive = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields.
        /// </summary>  
        public ReceiptPrintTemplateHeaderDTO(int templateId, string templateName, string fontName, int? fontSize, bool isActive )
            :this()
        {    
            log.LogMethodEntry(templateId, templateName, fontName, fontSize, isActive);
            this.templateId = templateId;
            this.templateName = templateName;
            this.fontName = fontName;
            this.fontSize = fontSize;
            this.isActive = isActive;        
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields.
        /// </summary>  
        public ReceiptPrintTemplateHeaderDTO(int templateId, string templateName, string fontName, int? fontSize,
                                            bool isActive, DateTime creationDate, string createdBy, DateTime lastUpdateDate,
                                            string lastUpdatedBy, int site_id, string guid, bool synchStatus, int masterEntityId)
            :this(templateId, templateName, fontName, fontSize, isActive)
        {
            log.LogMethodEntry(templateId, templateName, fontName, fontSize, isActive,creationDate, createdBy, lastUpdateDate, 
                                lastUpdatedBy, site_id, guid, synchStatus, masterEntityId);
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.site_id = site_id;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        public ReceiptPrintTemplateHeaderDTO(ReceiptPrintTemplateHeaderDTO receiptPrintTemplateHeaderDTO)
            : this(receiptPrintTemplateHeaderDTO.templateId, receiptPrintTemplateHeaderDTO.templateName, receiptPrintTemplateHeaderDTO.fontName, receiptPrintTemplateHeaderDTO.fontSize,
                  receiptPrintTemplateHeaderDTO.isActive, receiptPrintTemplateHeaderDTO.creationDate, receiptPrintTemplateHeaderDTO.createdBy, receiptPrintTemplateHeaderDTO.lastUpdateDate,
                  receiptPrintTemplateHeaderDTO.lastUpdatedBy, receiptPrintTemplateHeaderDTO.site_id, receiptPrintTemplateHeaderDTO.guid, receiptPrintTemplateHeaderDTO.synchStatus,
                  receiptPrintTemplateHeaderDTO.masterEntityId)
        {
            log.LogMethodEntry(receiptPrintTemplateHeaderDTO);
            if (receiptPrintTemplateHeaderDTO.lstReceiptPrintTemplateDTO != null)
            {
                lstReceiptPrintTemplateDTO = new List<ReceiptPrintTemplateDTO>();
                foreach (var receiptPrintTemplateDTO in receiptPrintTemplateHeaderDTO.lstReceiptPrintTemplateDTO)
                {
                    lstReceiptPrintTemplateDTO.Add(new ReceiptPrintTemplateDTO(receiptPrintTemplateDTO));
                }
            }
            log.LogMethodExit();
        }

        // <summary>
        /// SearchBy UserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            //Search by Template Id
            TEMPLATE_ID,
            //Search by Is Active
            IS_ACTIVE,
            //Search by site id
            SITE_ID,
            //Search by Template Name
            TEMPLATE_NAME,
            //Search by MasterEnityId 
            MASTER_ENTITY_ID,
            //Search by GUID 
            GUID
        }

        [DisplayName("TemplateId")]
        [ReadOnly(true)]
        public int TemplateId
        {
            // Get/Set method of the TemplateId field
            get
            {
                return templateId;
            }

            set
            {
                this.IsChanged = true;
                templateId = value;
            }
        }

        [DisplayName("Template Name")]
        public string  TemplateName
        {
            //Get/Set method of the TemplateName field
            get
            {
                return templateName;
            }

            set
            {
                this.IsChanged = true;
                templateName = value;
            }
        }

        [DisplayName("Font Name")]
        public string FontName
        {
            //Get/Set method of the FontName field
            get
            {
                return fontName;
            }

            set
            {
                this.IsChanged = true;
                fontName = value;
            }
        }

        [DisplayName("Font Size")]
        public int? FontSize
        {
            //Get/Set method of the FontSize field
            get
            {
                return fontSize;
            }

            set
            {
                this.IsChanged = true;
                fontSize = value;
            }
        }

        [DisplayName("Active Flag")]
        public bool IsActive
        {
            //Get/Set method of the IsActive field
            get
            {
                return isActive;
            }

            set
            {
                this.IsChanged = true;
                isActive = value;
            }
        }
        [Browsable(false)]
        [DisplayName("LastUpdatedDate")]
        public DateTime LastUpdatedDate
        {
            //Get/Set method of the LastUpdatedDate field
            get
            {
                return lastUpdateDate;
            }
            set
            {
                lastUpdateDate = value;
            }
        }

        [Browsable(false)]
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy
        {
            // Get/Set method of the LastUpdatedBy field
            get
            {
                return lastUpdatedBy;
            }
            set
            {
                lastUpdatedBy = value;
            }
        }
        [Browsable(false)]
        public DateTime CreationDate
        {
            // Get/Set method of the CreationDate field
            get
            {
                return creationDate;
            }
            set
            {
                creationDate = value;
            }
        }
        [Browsable(false)]
        public string CreatedBy
        {
            // Get/Set method of the CreatedBy field
            get
            {
                return createdBy;
            }
            set
            {
                createdBy = value;
            }
        }
        [Browsable(false)]
        public int Site_Id
        {
            //Get/Set method of the site_id field
            get
            {
                return site_id;
            }
            set
            {
                site_id = value;
            }
        }
        [Browsable(false)]
        public string GUID
        {
            //Get/Set method of the guid field
            get
            {
                return guid;
            }
            set
            {
                this.IsChanged = true;
                guid = value;
            }
        }
        [Browsable(false)]
        public bool SynchStatus
        {
            //Get/Set method of the SynchStatus field
            get
            {
                return synchStatus;
            }
            set
            {
                synchStatus = value;
            }
        }

        [Browsable(false)]
        public int MasterEntityId
        {

            //Get/Set method of the MasterEntityId field
            get
            {
                return masterEntityId;
            }
            set
            {
                this.IsChanged = true;
                masterEntityId = value;
            }
        }

        /// <summary>
        /// Returns whether the ReceiptPrintTemplateHeaderDTO is changed or any of its Lists  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (lstReceiptPrintTemplateDTO != null &&
                   lstReceiptPrintTemplateDTO.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Get/Set methods for ReceiptPrintTemplateDTOs
        /// </summary>
        public IList<ReceiptPrintTemplateDTO> ReceiptPrintTemplateDTOList
        {
            get
            {
                return lstReceiptPrintTemplateDTO;
            }
            set
            {
                lstReceiptPrintTemplateDTO = value;
            }
        }

        [Browsable(false)]
        public bool IsChanged
        {
            // Get/Set method to track changes to the object
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

