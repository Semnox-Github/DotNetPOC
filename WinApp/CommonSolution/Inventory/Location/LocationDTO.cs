/***********************************************************************************************
 * Project Name - Location DTO
 * Description  - Data object of Location 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 ************************************************************************************************
 *1.00        09-Aug-2016   Suneetha          Created
 *2.60.2      11-Jun-2019   Nagesh Badiger    modified isActive property (from string to bool type)
 *2.70        15-Jul-2019   Dakshakh raj      Modified : Added Parameterized constructor,
 *                                                       Added CreatedBy and CreationDate fields
 *2.70.2      02-Jan-2020   Girish Kundar     Modified : Added Location name exact search parameter   
 *2.100.0     17-Sep-2020   Deeksha           Modified Is changed property to handle unsaved records.
 ************************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the Location data object class. This acts as data holder for the Location business object
    /// </summary>
    public class LocationDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();


        /// <summary>
        /// SearchByProductParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByLocationParameters
        {
            /// <summary>
            /// Search by LOCATION ID field
            /// </summary>
            LOCATION_ID,
            /// <summary>
            /// Search by LOCATION TYPE ID field
            /// </summary>
            LOCATION_TYPE_ID,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Location Name field
            /// </summary>
            LOCATION_NAME,
            /// <summary>
            /// Search by Location Name exact field
            /// </summary>
            LOCATION_NAME_EXACT,
            /// <summary>
            /// Search by BARCODE NUMBER field
            /// </summary>
            BARCODE,
            /// <summary>
            /// Search by ISSTORE field
            /// </summary>
            ISSTORE,
            /// <summary>
            /// Search by MASSUPDATEALLOWED field
            /// </summary>
            MASSUPDATEALLOWED,
            /// <summary>
            /// Search by ISREMARKSMANDATORY field
            /// </summary>
            ISREMARKSMANDATORY,
            /// <summary>
            /// Search by CUSTOMDATASETID field
            /// </summary>
            CUSTOMDATASETID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            LOCATION_ID_LIST
        }

        private int locationId;
        private string name;
        private string lastUpdatedBy;

        private DateTime lastUpdatedDate;
        private bool isActive;
        private string isAvailableToSell;
        private string barcode;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private string isTurnInLocation;
        private string isStore;
        private string massUpdateAllowed;
        private string remarksMandatory;
        private int locationTypeId;
        //int machine_id;
        private int customDataSetId;
        private int masterEntityId;
        private string externalSystemReference;
        private string createdBy;
        private DateTime creationDate;
        /// <summary>
        /// Default constructor
        /// </summary>
        public LocationDTO()
        {
            log.LogMethodEntry();
            locationId = -1;
            siteId = -1;
            isTurnInLocation = "Y";
            isActive = true;
            isAvailableToSell = "N";
            isStore = "N";
            massUpdateAllowed = "Y";
            remarksMandatory = "Y";
            customDataSetId = -1;
            externalSystemReference = "";
            locationTypeId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public LocationDTO(int locationIdPassed, string namePassed, string isSellable, string barcode, string isTurnInLocationPassed, string isStorePassed, string massUpdatePassed,
                            string remarksMandatoryPassed, int locationTypeIdPassed, int customDataSetId, string externalSystemReference)
             : this()
        {
            log.LogMethodEntry(locationIdPassed, namePassed, isSellable, barcode, isTurnInLocationPassed, isStorePassed, massUpdatePassed, remarksMandatoryPassed, locationTypeIdPassed, customDataSetId, externalSystemReference);
            this.locationId = locationIdPassed;
            this.name = namePassed;
            this.isAvailableToSell = isSellable;
            this.barcode = barcode;
            this.isTurnInLocation = isTurnInLocationPassed;
            this.isStore = isStorePassed;
            this.massUpdateAllowed = massUpdatePassed;
            this.remarksMandatory = remarksMandatoryPassed;
            this.locationTypeId = locationTypeIdPassed;
            this.customDataSetId = customDataSetId;
            this.externalSystemReference = externalSystemReference;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public LocationDTO(int locationIdPassed, string namePassed, string lastModUserIdPassed, DateTime lastModDttmPassed,
                           bool isActivePassed, string isSellable, string barcode, int siteId, string guid, bool synchStatusPassed,
                           string isTurnInLocationPassed, string isStorePassed, string massUpdatePassed, string remarksMandatoryPassed,
                           int locationTypeIdPassed, int customDataSetId, string externalSystemReference, int masterEntityId, string createdBy, DateTime creationDate)
            : this(locationIdPassed, namePassed, isSellable, barcode, isTurnInLocationPassed, isStorePassed, massUpdatePassed, remarksMandatoryPassed, locationTypeIdPassed, customDataSetId, externalSystemReference)
        {
            log.LogMethodEntry(isActivePassed, lastModUserIdPassed, lastModDttmPassed, synchStatusPassed, siteId, guid, masterEntityId, createdBy, creationDate);
            this.isActive = isActivePassed;
            this.lastUpdatedBy = lastModUserIdPassed;
            this.lastUpdatedDate = lastModDttmPassed;
            this.synchStatus = synchStatusPassed;
            this.siteId = siteId;
            this.guid = guid;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the LocationId field
        /// </summary>
        [DisplayName("Location Id")]
        [ReadOnly(true)]
        public int LocationId { get { return locationId; } set { locationId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        [DisplayName("Remarks Mandatory")]
        public string RemarksMandatory { get { return remarksMandatory; } set { remarksMandatory = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the isAvailableToSell field
        /// </summary>
        [DisplayName("Available To Sell")]
        public string IsAvailableToSell { get { return isAvailableToSell; } set { isAvailableToSell = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsStore field
        /// </summary>
        [DisplayName("Is Store")]
        public string IsStore { get { return isStore; } set { isStore = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastModUserId field
        /// </summary>
        [DisplayName("Last Mod User Id")]
        [Browsable(false)]
        [ReadOnly(true)]
        public string LastModUserId { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get/Set method of the LastModDttm field
        /// </summary>
        [DisplayName("Last Modified Date")]
        [Browsable(false)]
        [ReadOnly(true)]
        public DateTime LastModDttm { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Is Active")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsTurnInLocation field
        /// </summary>
        [DisplayName("Turn In Location")]
        public string IsTurnInLocation { get { return isTurnInLocation; } set { isTurnInLocation = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [ReadOnly(true)]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [ReadOnly(true)]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [ReadOnly(true)]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MassUpdatedAllowed field
        /// </summary>
        [DisplayName("Allow Mass Update")]
        public string MassUpdatedAllowed { get { return massUpdateAllowed; } set { massUpdateAllowed = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LocationTypeId field
        /// </summary>
        [DisplayName("Location Type Id")]
        public int LocationTypeId { get { return locationTypeId; } set { locationTypeId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the barcode field
        /// </summary>
        [DisplayName("Barcode")]
        public string Barcode { get { return barcode; } set { barcode = value; this.IsChanged = true; } }
        ///// <summary>
        ///// Get/Set method of the barcode field
        ///// </summary>
        //[DisplayName("MachineId")]
        //[ReadOnly(true)]
        //public int MachineId { get { return machine_id; } set { machine_id = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CustomDataSetId field
        /// </summary>
        [DisplayName("CustomDataSetId")]
        [ReadOnly(true)]
        public int CustomDataSetId { get { return customDataSetId; } set { customDataSetId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ExternalSystemRefernce field
        /// </summary>
        [DisplayName("External System Refernce")]
        public string ExternalSystemReference { get { return externalSystemReference; } set { externalSystemReference = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

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
                    return notifyingObjectIsChanged || locationId < 0;
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
