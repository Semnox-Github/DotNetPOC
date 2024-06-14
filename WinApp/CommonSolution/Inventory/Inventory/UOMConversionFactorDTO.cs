/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data object of uomConversionFactor
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0       23-Jul-2020   Deeksha             Created for Recipe Management enhancement.
 *********************************************************************************************/
using System;

namespace Semnox.Parafait.Inventory
{
    public class UOMConversionFactorDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  uom Conversion Factor Id field
            /// </summary>
            UOM_CONVERSION_FACTOR_ID,
           
            /// <summary>
            /// Search by  uom Id field
            /// </summary>
            UOM_ID,
           
            /// <summary>
            /// Search by  Base uom Id field
            /// </summary>
            BASE_UOM_ID,
            
            /// </summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,
           
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
           
            /// <summary>
            /// Search by Master entity id field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int uomConversionFactorId;
        private int uomId;
        private int baseuomId;
        private double conversionFactor;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public UOMConversionFactorDTO()
        {
            log.LogMethodEntry();
            uomConversionFactorId = -1;
            uomId = -1;
            baseuomId = -1;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields.
        /// </summary>
        public UOMConversionFactorDTO(int uomConversionFactorId, int uomId, int baseuomId, double conversionFactor,
                                      bool isActive)
            : this()
        {
            log.LogMethodEntry(uomConversionFactorId, uomId, baseuomId, conversionFactor, isActive);
            this.uomConversionFactorId = uomConversionFactorId;
            this.uomId = uomId;
            this.baseuomId = baseuomId;
            this.conversionFactor = conversionFactor;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields.
        /// </summary>
        public UOMConversionFactorDTO(int uomConversionFactorId, int uomId, int baseuomId, double conversionFactor,
                                      bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                      DateTime lastUpdateDate, string guid, int siteId, bool synchStatus, int masterEntityId)
            : this(uomConversionFactorId, uomId, baseuomId, conversionFactor, isActive)
        {
            log.LogMethodEntry(uomConversionFactorId, uomId, baseuomId, conversionFactor, isActive, createdBy, creationDate,
                                lastUpdatedBy, lastUpdateDate, guid, siteId, synchStatus, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the uomConversionFactorId field
        /// </summary>
        public int UOMConversionFactorId { get { return uomConversionFactorId; } set { this.IsChanged = true; uomConversionFactorId = value; } }

        /// <summary>
        /// Get/Set method of the uomId field
        /// </summary>
        public int UOMId { get { return uomId; } set { this.IsChanged = true; uomId = value; } }

        /// <summary>
        /// Get/Set method of the BaseuomId field
        /// </summary>
        public int BaseUOMId { get { return baseuomId; } set { this.IsChanged = true; baseuomId = value; } }

        /// <summary>
        /// Get/Set method of the ConversionFactor field
        /// </summary>
        public double ConversionFactor { get { return conversionFactor; } set { this.IsChanged = true; conversionFactor = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { this.IsChanged = true; guid = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || uomConversionFactorId < 0;
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
