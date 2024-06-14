    /********************************************************************************************
 * Project Name - Receipt Print Template DTO
 * Description  - DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *1.00        16-Sep-2018      Mathew Ninan     Created
 *2.60        07-May-2019      Mushahid Faizan  Modified IsActive Datatype from string to bool.
 *2.7.0       08-Jul-2019      Archana          Modified: Redemption Receipt changes to show ticket allocation details
 *2.70.2        18-Jul-2019      Deeksha          Modifications as per 3 tier standard.
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.Drawing;

namespace Semnox.Parafait.Printer
{
    /// <summary>
    /// DTO class for ReceiptPrintTemplate
    /// </summary>
    public class ReceiptPrintTemplateDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;
        private int id;
        private string section;
        private int sequence;
        private string col1Data;
        private string col1Alignment;
        private string col2Data;
        private string col2Alignment;
        private string col3Data;
        private string col3Alignment;
        private string col4Data;
        private string col4Alignment;
        private string col5Data;
        private string col5Alignment;
        private int templateId;
        private string fontName;
        private int? fontSize;
        private string metadata;
        private bool isActive;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private int site_id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private Font receiptFont;
        
        /// <summary>
        /// Selection enum
        /// </summary>
        public enum Selection
        {
            Section = 1,
            HEADER = 2,
            PRODUCT = 3,
            PRODUCTSUMMARY = 4,
            CARDINFO = 5,
            TRANSACTIONTOTAL = 6,
            DISCOUNTS = 7,
            DISCOUNTTOTAL = 8,
            TAXLINE = 9,
            TAXABLECHARGES = 10,
            TAXTOTAL = 11,
            NONTAXABLECHARGES = 12,
            GRANDTOTAL = 13,
            FOOTER = 14,
            ITEMSLIP = 15,
            REDEMPTION_SOURCE_HEADER = 16,
            REDEMPTION_SOURCE = 17,
            REDEMPTION_SOURCE_TOTAL = 18,
            REDEMPTION_BALANCE = 19,
            REDEEMED_GIFTS = 20
        }

        /// <summary>
        /// ALignment enum
        /// </summary>
        public enum Alignment
        {
            Hide = 1,
            Left = 2,
            Right = 3,
            Center = 4
        }

        
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public ReceiptPrintTemplateDTO()
        {           
            log.LogMethodEntry();
            id = -1;
            templateId = -1;
            isActive = true;
            site_id = -1;
            masterEntityId = -1;
            col1Alignment = col2Alignment = col3Alignment = Col4Alignment = col5Alignment = "H";
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with Required data fields.
        /// </summary>
        public ReceiptPrintTemplateDTO(int id, string section, int sequence, string col1Data, string col1Alignment, string col2Data,
                                       string col2Alignment, string col3Data, string col3Alignment, string col4Data, string col4Alignment,
                                       string col5Data, string col5Alignment, int templateId, string fontName, int? fontSize,
                                       string metadata, bool isActive, Font receiptFont)
            :this()
        {
            log.LogMethodEntry(id, section, sequence, col1Data, col1Alignment, col2Data, col2Alignment, col3Data, col3Alignment,
                                col4Data, col4Alignment, col5Data, col5Alignment, templateId, fontName,  fontSize, metadata, isActive, receiptFont);
            this.id = id;
            this.section = section;
            this.sequence = sequence;
            this.col1Data = col1Data;
            this.col1Alignment = col1Alignment;
            this.col2Data = col2Data;
            this.col2Alignment = col2Alignment;
            this.col3Data = col3Data;
            this.col3Alignment = col3Alignment;
            this.col4Data = col4Data;
            this.col4Alignment = col4Alignment;
            this.col5Data = col5Data;
            this.col5Alignment = col5Alignment;
            this.templateId = templateId;
            this.fontName = fontName;
            this.fontSize = fontSize;
            this.metadata = metadata;
            this.isActive = isActive;            
            this.receiptFont = receiptFont;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with all the data fields.
        /// </summary>
        public ReceiptPrintTemplateDTO(int id, string section, int sequence, string col1Data, string col1Alignment, string col2Data,
                                       string col2Alignment, string col3Data, string col3Alignment, string col4Data, string col4Alignment,
                                       string col5Data, string col5Alignment, int templateId, string fontName, int? fontSize,
                                       string metadata, bool isActive, DateTime creationDate, string createdBy, DateTime lastUpdateDate,
                                       string lastUpdatedBy, int site_id, string guid, bool synchStatus, int masterEntityId, Font receiptFont)
            :this(id, section, sequence, col1Data, col1Alignment, col2Data, col2Alignment, col3Data, col3Alignment,
                                col4Data, col4Alignment, col5Data, col5Alignment, templateId, fontName, fontSize, metadata, isActive, receiptFont)
        {
            log.LogMethodEntry(id, section, sequence, col1Data, col1Alignment, col2Data, col2Alignment, col3Data, col3Alignment,
                                col4Data, col4Alignment, col5Data, col5Alignment, templateId, fontName, fontSize, metadata, isActive,
                                creationDate, createdBy, lastUpdateDate, lastUpdatedBy, site_id, guid, synchStatus, masterEntityId, receiptFont);
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
        public ReceiptPrintTemplateDTO(ReceiptPrintTemplateDTO receiptPrintTemplateDTO)
            : this(receiptPrintTemplateDTO.id, receiptPrintTemplateDTO.section, receiptPrintTemplateDTO.sequence, receiptPrintTemplateDTO.col1Data, receiptPrintTemplateDTO.col1Alignment,
                 receiptPrintTemplateDTO.col2Data, receiptPrintTemplateDTO.col2Alignment, receiptPrintTemplateDTO.col3Data, receiptPrintTemplateDTO.col3Alignment, receiptPrintTemplateDTO.col4Data,
                 receiptPrintTemplateDTO.col4Alignment, receiptPrintTemplateDTO.col5Data, receiptPrintTemplateDTO.col5Alignment, receiptPrintTemplateDTO.templateId, receiptPrintTemplateDTO.fontName,
                 receiptPrintTemplateDTO.fontSize, receiptPrintTemplateDTO.metadata, receiptPrintTemplateDTO.isActive, receiptPrintTemplateDTO.creationDate, receiptPrintTemplateDTO.createdBy,
                 receiptPrintTemplateDTO.lastUpdateDate, receiptPrintTemplateDTO.lastUpdatedBy, receiptPrintTemplateDTO.site_id, receiptPrintTemplateDTO.guid, receiptPrintTemplateDTO.synchStatus,
                 receiptPrintTemplateDTO.masterEntityId, receiptPrintTemplateDTO.receiptFont)
        {
            log.LogMethodEntry(receiptPrintTemplateDTO);
            log.LogMethodExit();
        }

        /// <summary>
        /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            //SearchBy UserParameters enum controls the search fields, this can be expanded to include additional fields
            //Search by Id
            ID,
            //Search by TemplateId
            TEMPLATE_ID,
            //Search by Is_Active
            IS_ACTIVE,
            //Search by site_id
            SITE_ID,
            //Search by MasterEnityId 
            MASTER_ENTITY_ID
        }

        [DisplayName("ID")]
        [Browsable(false)]
        public int ID
        {
            // Get/Set method of the id field
            get
            {
                return id;
            }

            set
            {
                this.IsChanged = true;
                id = value;
            }
        }
      
        [DisplayName("Template Id")]
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
       
        [DisplayName("Font Name")]
        public string FontName
        {
            // Get/Set method of the FontName field
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
            // Get/Set method of the FontSize field
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
        
        [DisplayName("MetaData")]
        public string MetaData
        {
            // Get/Set method of the MetaData field
            get
            {
                return metadata;
            }

            set
            {
                this.IsChanged = true;
                metadata = value;
            }
        }
        
        [DisplayName("Section")]
        public string Section
        {
            // Get/Set method of the section field
            get
            {
                return section;
            }

            set
            {
                this.IsChanged = true;
                section = value;
            }
        }
        
        [DisplayName("Sequence")]
        public int Sequence
        {
            // Get/Set method of the sequence field
            get
            {
                return sequence;
            }

            set
            {
                this.IsChanged = true;
                sequence = value;
            }
        }

        [DisplayName("Col1Data")]
        public string Col1Data
        {
            // Get/Set method of the col1Data field
            get
            {
                return col1Data;
            }

            set
            {
                this.IsChanged = true;
                col1Data = value;
            }
        }

        [DisplayName("Col1Alignment")]
        public string Col1Alignment
        {
            // Get/Set method of the col1Alignment field
            get
            {
                return col1Alignment;
            }

            set
            {
                this.IsChanged = true;
                col1Alignment = value;
            }
        }

        [DisplayName("Col2Data")]
        public string Col2Data
        {
            // Get/Set method of the col2Data field
            get
            {
                return col2Data;
            }

            set
            {
                this.IsChanged = true;
                col2Data = value;
            }
        }

        [DisplayName("Col2Alignment")]
        public string Col2Alignment
        {
            // Get/Set method of the col2Alignment field
            get
            {
                return col2Alignment;
            }

            set
            {
                this.IsChanged = true;
                col2Alignment = value;
            }
        }

        [DisplayName("Col3Data")]
        public string Col3Data
        {
            // Get/Set method of the col3Data field
            get
            {
                return col3Data;
            }

            set
            {
                this.IsChanged = true;
                col3Data = value;
            }
        }

        [DisplayName("Col3Alignment")]
        public string Col3Alignment
        {
            // Get/Set method of the col3Alignment field
            get
            {
                return col3Alignment;
            }

            set
            {
                this.IsChanged = true;
                col3Alignment = value;
            }
        }

        [DisplayName("Col4Data")]
        public string Col4Data
        {
            // Get/Set method of the col4Data field
            get
            {
                return col4Data;
            }

            set
            {
                this.IsChanged = true;
                col4Data = value;
            }
        }

        [DisplayName("Col4Alignment")]
        public string Col4Alignment
        {
            // Get/Set method of the col4Alignment field
            get
            {
                return col4Alignment;
            }

            set
            {
                this.IsChanged = true;
                col4Alignment = value;
            }
        }

        [DisplayName("Col5Data")]
        public string Col5Data
        {
            // Get/Set method of the col5Data field
            get
            {
                return col5Data;
            }

            set
            {
                this.IsChanged = true;
                col5Data = value;
            }
        }

        [DisplayName("Col5Alignment")]
        public string Col5Alignment
        {
            // Get/Set method of the col5Alignment field
            get
            {
                return col5Alignment;
            }

            set
            {
                this.IsChanged = true;
                col5Alignment = value;
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

        [DisplayName("LastUpdatedDate")]
        [Browsable(false)]
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

        [DisplayName("Last Updated By")]
        [Browsable(false)]
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

        [Browsable(false)]
        public Font ReceiptFont
        {

            //Get/Set method of the MasterEntityId field
            get
            {
                return receiptFont;
            }
            set
            {
                receiptFont = value;
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
                    return notifyingObjectIsChanged || id < 0;
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
