/********************************************************************************************
 * Project Name - AttractionPlays DTO
 * Description  - Data object of ReservationCore
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By         Remarks          
 *********************************************************************************************
 *1.00        17-August-2017    Rakshith            Created 
 *2.70        26-Nov-2018       Guru S A            Moved to Product project
 *2.70        31-Jan-2019       Nagesh Badiger      Modified
              07-Jun-2019       Akshay Gulaganji    Moved from Booking to Product Project (Code merge from Development to WebManagementStudio branch)  
 * *******************************************************************************************
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// AttractionPlaysDTO   Class
    /// </summary>
    [Table("attractionPlays")]
    public class AttractionPlaysDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        ///// <summary>
        ///// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        ///// </summary>
        public enum SearchByAttractionPlaysParameters
        {
            /// <summary>
            /// Search by AttractionPlaysId
            /// </summary>
            ID,

            /// <summary>
            /// Search by site_id
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by PlayName
            /// </summary>
            PLAYNAME,

            /// <summary>
            /// Search by Guid
            /// </summary>
            GUID,

            /// <summary>
            /// Search by MasterEntityId
            /// </summary>
            MASTERENTITY_ID,

            /// <summary>
            /// Search by IS_ACTIVE
            /// </summary>
            IS_ACTIVE
        }

        int attractionPlayId;
        string playName;
        DateTime? expiryDate;
        double? price;
        string guid;
        int siteId;
        bool synchStatus;
        int masterEntityId;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdateDate;
        bool isActive;
        
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public AttractionPlaysDTO()
        {
            log.LogMethodEntry();
            this.attractionPlayId = -1;
            this.playName = "";
            this.expiryDate = DateTime.MinValue;
            this.siteId = -1;
            this.price = 0;
            this.masterEntityId = -1;
            this.isActive = true;            
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with some  data fields
        /// </summary>
        public AttractionPlaysDTO(  int AttractionPlayId, string PlayName) : this()
        {
            log.LogMethodEntry(AttractionPlayId, PlayName);
            this.AttractionPlayId = AttractionPlayId;
            this.PlayName = PlayName; 
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public AttractionPlaysDTO(int attractionPlayId, string playName, DateTime? expiryDate, double? price, string guid, int site_id, bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, bool isActive) : base()
        {
            log.LogMethodEntry(attractionPlayId, playName, expiryDate, price, guid, site_id, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.attractionPlayId = attractionPlayId;
            this.playName = playName;
            this.expiryDate = expiryDate;
            this.price = price;
            this.guid = guid;
            this.siteId = site_id;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdateDate = lastUpdateDate;
            this.isActive = isActive;
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Get/Set method of the attractionPlayId field
        /// </summary>
        [DisplayName("AttractionPlayId")]
        public int AttractionPlayId
        {
            get
            {
                return attractionPlayId;
            }

            set
            {
                this.IsChanged = true;
                attractionPlayId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the playName field
        /// </summary>
        [DisplayName("PlayName")]
        public string PlayName
        {
            get
            {
                return playName;
            }

            set
            {
                this.IsChanged = true;
                playName = value;
            }
        }


        /// <summary>
        /// Get/Set method of the expiryDate field
        /// </summary>
        [DisplayName("ExpiryDate")]
        public DateTime? ExpiryDate
        {
            get
            {
                return expiryDate;
            }

            set
            {
                this.IsChanged = true;
                expiryDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the price field
        /// </summary>
        [DisplayName("Price")]
        public double? Price
        {
            get
            {
                return price;
            }

            set
            {
                this.IsChanged = true;
                price = value;
            }
        }

        /// <summary>
        /// Get/Set method of the guid field
        /// </summary>
        [Browsable(false)]
        public string Guid
        {
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

        /// <summary>
        /// Get/Set method of the site_id field
        /// </summary>
        [Browsable(false)]
        public int SiteId
        {
            get
            {
                return siteId;
            }

            set
            {
                this.IsChanged = true;
                siteId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the synchStatus field
        /// </summary>
        [Browsable(false)]
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }

            set
            {
                this.IsChanged = true;
                synchStatus = value;
            }
        }

        /// <summary>
        /// Get/Set method of the masterEntityId field
        /// </summary>
        [Browsable(false)]
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }
            set
            {
                masterEntityId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>

        [Browsable(false)]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                createdBy = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the creationDate field
        /// </summary>

        [Browsable(false)]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                this.IsChanged = true;
                creationDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the lastUpdatedBy field
        /// </summary>

        [Browsable(false)]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {
                this.IsChanged = true;
                lastUpdatedBy = value;
            }
        }
        /// <summary>
        /// Get/Set method of the lastUpdateDate field
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }
            set
            {
                this.IsChanged = true;
                lastUpdateDate = value;
            }
        }
        /// <summary>
        /// Get/Set method of the isActive field
        /// </summary>
        [DisplayName("Active")]
        public bool IsActive
        {
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
                    return notifyingObjectIsChanged || attractionPlayId < 0;
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
