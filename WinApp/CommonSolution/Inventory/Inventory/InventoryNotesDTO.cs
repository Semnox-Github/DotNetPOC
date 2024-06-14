/********************************************************************************************
 * Project Name -Inventory Notes DTO
 * Description  -Data object of inventory Notes  
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        18-Aug-2016    Amaresh          Created 
 *2.70.2        14-Jul-2019    Deeksha          Modifications as per three tier standard
 ********************************************************************************************/

using System;
using Semnox.Parafait.logging;
using System.ComponentModel;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the inventory notes data object class. This acts as data holder for the inventory notes object
    /// </summary>
    public class InventoryNotesDTO
    {
        private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByInventoryParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByInventoryNotesParameters
        {
            /// <summary>
            /// Search by INVENTORY NOTE ID field
            /// </summary>
            INVENTORY_NOTE_ID,
            /// <summary>
            /// Search by PARAFAIT OBJECT NAME field
            /// </summary>
            PARAFAIT_OBJECT_NAME,
            /// <summary>
            /// Search by PARAFAIT OBJECT ID field
            /// </summary>
            PARAFAIT_OBJECT_ID ,
            /// <summary>
            /// Search by NOTE TYPE ID field
            /// </summary>
            NOTE_TYPE_ID ,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID ,
            /// <summary>
            /// Search by MATER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID ,
            ISACTIVE,
            /// <summary>
            /// Search by INVENTORY NOTE ID field
            /// </summary>
            INVENTORY_NOTE_ID_LIST,
            PARAFAIT_OBJECT_ID_LIST
        }

        private int inventoryNoteId;
        private int noteTypeId;
        private string parafaitObjectName;
        private int parafaitObjectId;
        private string notes;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;

         /// <summary>
        /// Default Contructor
        /// </summary>
        public InventoryNotesDTO()
        {
            log.LogMethodEntry();
            inventoryNoteId = -1;
            parafaitObjectId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public InventoryNotesDTO(int inventoryNoteId, int noteTypeId, string parafaitObjectName, int parafaitObjectId, string notes)
            : this()
        {
            log.LogMethodEntry(inventoryNoteId, noteTypeId, parafaitObjectName, parafaitObjectId, notes);
            this.inventoryNoteId = inventoryNoteId;
            this.noteTypeId = noteTypeId;
            this.parafaitObjectName = parafaitObjectName;
            this.parafaitObjectId = parafaitObjectId;
            this.notes = notes;
            log.LogMethodExit();
           
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public InventoryNotesDTO( int inventoryNoteId, int noteTypeId, string parafaitObjectName, int parafaitObjectId, string  notes, DateTime creationDate,	
                                    string lastUpdatedBy, DateTime lastUpdatedDate, string guid, int siteId, bool synchStatus, int masterEntityId,string createdBy)
            :this(inventoryNoteId, noteTypeId, parafaitObjectName, parafaitObjectId, notes)
        {
            log.LogMethodEntry(inventoryNoteId, noteTypeId, parafaitObjectName, parafaitObjectId, notes, creationDate,
                               lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, createdBy);
           
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.createdBy = createdBy;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the InventoryNoteId fields
        /// </summary>
        public int InventoryNoteId { get { return inventoryNoteId; } set { inventoryNoteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the NoteTypeId fields
        /// </summary>
        public int NoteTypeId { get { return noteTypeId; } set { noteTypeId = value; } }

        /// <summary>
        /// Get/Set method of the ParafaitObjectName fields
        /// </summary>
        public string ParafaitObjectName { get { return parafaitObjectName; } set { parafaitObjectName = value; } }

        /// <summary>
        /// Get/Set method of the ParafaitObjectId fields
        /// </summary>
        public int ParafaitObjectId { get { return parafaitObjectId; } set { parafaitObjectId = value; } }


        /// <summary>
        /// Get/Set method of the Notes fields
        /// </summary>
        public string Notes { get { return notes; } set { notes = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate fields
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy fields
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy fields
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the LastupdatedDate fields
        /// </summary>
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value;} }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;} }


        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
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
                    return notifyingObjectIsChanged || inventoryNoteId < 0;
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
