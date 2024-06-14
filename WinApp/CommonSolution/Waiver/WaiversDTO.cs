/********************************************************************************************
 * Project Name - Waiver
 * Description  - Data object of the WaiverSetDetail
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.70        01-Jul -2019   Girish Kundar    Modified : Added Parametrized Constructor with required fields.
 *2.70.2      03-Oct-2019      Girish Kundar    Waiver phase 2 changes
 ********************************************************************************************* */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Waiver
{
    /// <summary>
    ///  This is the user data object class. This acts as data holder for the user business object
    /// </summary>   
    public class WaiversDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByWaiverParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 

        public enum SearchByWaivers
        {
            /// <summary>
            /// Search by WAIVERSETDETAIL ID field
            /// </summary>
            WAIVERSETDETAIL_ID,
            /// <summary>
            /// Search by WAIVERSETDETAIL_ID_LIST field
            /// </summary>
            WAIVERSETDETAIL_ID_LIST,
            /// <summary>
            /// Search by WAIVERSET ID field
            /// </summary>
            WAIVERSET_ID ,
            /// <summary>
            /// Search by NAME field
            /// </summary>
            NAME ,
            /// <summary>
            /// Search by WAIVER FILENAME field
            /// </summary>
            WAIVER_FILENAME,
            /// <summary>
            /// Search by VALID FOR DAYS field
            /// </summary>
            VALID_FOR_DAYS,
            /// <summary>
            /// Search by EFFECTIVE DATE field
            /// </summary>
            EFFECTIVE_DATE ,

            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            IS_ACTIVE ,

            /// <summary>
            /// Search by LAST UPDATED DATE field
            /// </summary>
            LAST_UPDATED_DATE ,
            /// <summary>
            /// Search by LAST UPDATED BY field
            /// </summary>
            LAST_UPDATED_BY,
            /// <summary>
            /// Search by GUID field
            /// </summary>
            GUID ,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID ,
            /// <summary>
            /// Search by SYNCH STATUS field
            /// </summary>
            SYNCH_STATUS ,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID ,
            /// <summary>
            /// Search by WAIVERSET_ID_LIST field
            /// </summary>
            WAIVERSET_ID_LIST,
            /// <summary>
            /// Search by SIGNING_OPTION_IS_SET field
            /// </summary>
            SIGNING_OPTION_IS_SET 
        }

      private int waiverSetDetailId;
      private int waiverSetId;
      private string name;
      private string waiverFileName;
      private int? validForDays;
      private DateTime? effectiveDate;
      private bool isActive;
      private DateTime creationDate;
      private string createdBy;
      private DateTime lastUpdatedDate;
      private string lastUpdatedBy;
      private string guid;
      private int site_id;
      private bool synchStatus;
      private int masterEntityId;
        private string waiverFileContentInBase64Format;

        /// <summary>
        /// Default constructor
        /// </summary>
        public WaiversDTO()
        {
            log.LogMethodEntry();
            waiverSetId = -1;
            masterEntityId = -1;
            waiverSetDetailId = -1;
            validForDays = -1;
            isActive = true;
            site_id = -1;
            log.LogMethodExit();

        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public WaiversDTO(int waiverSetDetailId, int waiverSetId, string name, string waiverFileName, int? validForDays,
                                  bool isActive, DateTime? effectiveDate)

            :this()
        {
            log.LogMethodEntry( waiverSetDetailId,  waiverSetId, name, waiverFileName, validForDays, isActive, effectiveDate);
            this.waiverSetDetailId = waiverSetDetailId;
            this.waiverSetId = waiverSetId;
            this.name = name;
            this.waiverFileName = waiverFileName;
            this.validForDays = validForDays;
            this.effectiveDate = effectiveDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public WaiversDTO(int waiverSetDetailId,int waiverSetId, string name, string waiverFileName, int? validForDays, bool isActive, DateTime? effectiveDate,
                                     DateTime creationDate, string createdBy, DateTime lastUpdatedDate, string lastUpdatedBy,
                                     string guid, int site_id, bool synchStatus, int masterEntityId)

            :this(waiverSetDetailId, waiverSetId, name, waiverFileName, validForDays, isActive, effectiveDate)
        {
            log.LogMethodEntry(waiverSetDetailId, waiverSetId, name, waiverFileName, validForDays, isActive, effectiveDate,
                               creationDate, createdBy, lastUpdatedDate, lastUpdatedBy,guid, site_id, synchStatus, masterEntityId);
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.site_id = site_id;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the WaiverSetDetailId field
        /// </summary>
        [DisplayName("WaiverSetDetail Id")]
        public int WaiverSetDetailId { get { return waiverSetDetailId; } set { waiverSetDetailId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the WaiverSetId field
        /// </summary>
        [DisplayName("WaiverSet Id")]
        public int WaiverSetId { get { return waiverSetId; } set { waiverSetId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Waiver File Name field
        /// </summary>
        [DisplayName("Waiver File Name")]
        public string WaiverFileName { get { return waiverFileName; } set { waiverFileName = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Valid For Days field
        /// </summary>
        [DisplayName("Valid For Days")]
        public int? ValidForDays { get { return validForDays; } set { validForDays = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the EffectiveDate field
        /// </summary>
        [DisplayName("Effective Date")]
        public DateTime? EffectiveDate { get { return effectiveDate; } set { effectiveDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Active?")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Updated Date")]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the GUID field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Site_id field
        /// </summary>s
        [DisplayName("Site Id")]
        public int Site_id { get { return site_id; } set { site_id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }


        [NotMapped]
        /// <summary>
        /// Get/Set method of the ObjectTranslationsDTOList field
        /// </summary>
        [DisplayName("ObjectTranslationsDTOList")]
        public List<ObjectTranslationsDTO> ObjectTranslationsDTOList { get; set; }

        /// <summary>
        /// Get/Set method of the waiverFileContentInBase64Format field
        /// </summary>
        [DisplayName("Waiver file Content")]
        [Browsable(false)]
        public string WaiverFileContentInBase64Format { get { return waiverFileContentInBase64Format; } set { waiverFileContentInBase64Format = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || waiverSetDetailId < 0;
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
