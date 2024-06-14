/********************************************************************************************
 * Project Name - Kiosk
 * Description  - DTO of kiosk setup
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.60        18-Mar-2016   Jagan Mohana       Created 
 *            23-Apr-2019   Mushahid Faizan    Added ISACTIVE SearchByParameter,LogMethodEntry & Exit & removed enumeration numbering.
                                               Added ModelCode Get/Set Property.
 *2.70.2        25-Jul-2019   Dakshakh raj       Modified : Added Parameterized costrustor
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Core.GenericUtilities
{
    public class KioskSetupDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,
            
            /// <summary>
            /// Search by NOTE_COIN_FLAG field
            /// </summary>
            NOTE_COIN_FLAG,
            
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            
            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            ISACTIVE,
            
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int id;
        private string noteCoinFlag;
        private int denominationId;
        private string name;
        private byte[] image;
        private double kioskValue;
        private string acceptorHexCode;
        private bool active;
        private string modelCode;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;

        /// <summary>
        /// Default constructor
        /// Added siteId=-1 by Mushahid Faizan on 23-Apr-2019
        /// </summary>
        public KioskSetupDTO()
        {
            log.LogMethodEntry();
            id = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public KioskSetupDTO(int id, string noteCoinFlag, int denominationId, string name, byte[] image, double kioskValue, string acceptorHexCode, bool active, string modelCode)
            :this()
        {
            log.LogMethodEntry(id, noteCoinFlag, denominationId, name, image, kioskValue, acceptorHexCode, active, modelCode);
            this.id = id;
            this.noteCoinFlag = noteCoinFlag;
            this.denominationId = denominationId;
            this.name = name;
            this.image = image;
            this.kioskValue = kioskValue;
            this.acceptorHexCode = acceptorHexCode;
            this.active = active;
            this.modelCode = modelCode;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public KioskSetupDTO(int id, string noteCoinFlag, int denominationId, string name, byte[] image, double kioskValue, string acceptorHexCode, bool active, string modelCode,
                             int siteId, string guid, bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate)
            :this(id, noteCoinFlag, denominationId, name, image, kioskValue, acceptorHexCode, active, modelCode)
        {
            log.LogMethodEntry(siteId, guid, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }
        
        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        [ReadOnly(true)]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the NoteCoinFlag field
        /// </summary>
        [DisplayName("NoteCoinFlag")]
        public string NoteCoinFlag { get { return noteCoinFlag; } set { noteCoinFlag = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the DenominationId field
        /// </summary>
        [DisplayName("DenominationId")]
        public int DenominationId { get { return denominationId; } set { denominationId = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Get/Set method of the Image field
        /// </summary>
        [DisplayName("Image")]
        public byte[] Image { get { return image; } set { image = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Get/Set method of the Value field
        /// </summary>
        [DisplayName("Value")]
        public double Value { get { return kioskValue; } set { kioskValue = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Get/Set method of the AcceptorHexCode field
        /// </summary>
        [DisplayName("AcceptorHexCode")]
        public string AcceptorHexCode { get { return acceptorHexCode; } set { acceptorHexCode = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Active")]
        public bool Active { get { return active; } set { active = value; this.IsChanged = true; } }
       
        /// <summary>
        ///  Get/Set method of the ModelCode field
        /// </summary>
        [DisplayName("ModelCode")]
        public string ModelCode { get { return modelCode; } set { modelCode = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Get/Set method of the GUID field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        public int SiteId { get { return siteId; } set { siteId = value;  } }
        
        /// <summary>
        /// Get/Set method of the SyncStatus field
        /// </summary>
        [DisplayName("SyncStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }
       
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value;} }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value;  } }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

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