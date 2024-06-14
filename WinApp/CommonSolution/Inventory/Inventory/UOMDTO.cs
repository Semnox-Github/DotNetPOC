/********************************************************************************************
 * Project Name -UOM
 * Description  -Data object of UOM 
 * 
 **************
 *Version Log
 **************
 *Version     Date             Modified By       Remarks          
 *********************************************************************************************
 *2.60.3       17-Jun-2019     Akshay Gulaganji     Added Who Columns and modified Is active to isActive
 *2.70.2        20-Jul-2019      Deeksha              modifications as per 3 tier standards
 *2.100.0      26-Jul-2020     Deeksha              Modified for Recipe Management enhancement.
 *2.110.0      26-Oct-2020     Mushahid Faizan      Web Inventory changes for UI.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Semnox.Parafait.logging;

namespace Semnox.Parafait.Inventory
{
    public class UOMDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByUOMParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByUOMParameters
        {
            /// <summary>
            /// Search by UOMID field
            /// </summary>
            UOMID,
            /// <summary>
            /// Search by UOMID LIST field
            /// </summary>
            UOMID_LIST,
            /// <summary>
            /// Search by UOM field
            /// </summary>
            UOM,
            /// <summary>
            /// Search by SITEID field
            /// </summary>
            SITEID,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int uOMId;
        private string uOM;
        private string remarks;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private List<UOMConversionFactorDTO> uomConversionFactorDTOList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public UOMDTO()
        {
            log.LogMethodEntry();
            uOMId = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            uomConversionFactorDTOList = new List<UOMConversionFactorDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with required  data fields
        /// </summary>
        public UOMDTO(int uOMId, string uOM, string remarks)
            : this()
        {
            log.LogMethodEntry(uOMId, uOM, remarks);
            this.uOMId = uOMId;
            this.uOM = uOM;
            this.remarks = remarks;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with all the data fields
        /// </summary>
        public UOMDTO(int uOMId, string uOM, string remarks, int siteId, string guid, bool synchStatus, int masterEntityId,
                       bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate)
           : this(uOMId, uOM, remarks)
        {
            log.LogMethodEntry(uOMId, uOM, remarks, siteId, guid, synchStatus, masterEntityId,
                         isActive, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.isActive = isActive;
            this.lastUpdateDate = lastUpdateDate;
            this.createdBy = createdBy;
            this.lastUpdatedBy = lastUpdatedBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the UOMId field
        /// </summary>
        [DisplayName("UOM Id")]
        [ReadOnly(true)]
        public int UOMId { get { return uOMId; } set { uOMId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UOM field
        /// </summary>
        [DisplayName("UOM")]
        public string UOM { get { return uOM; } set { uOM = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        [DisplayName("Remarks")]
        public string Remarks { get { return remarks; } set { remarks = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the site_id field
        /// </summary>
        [DisplayName("site_id")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }
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
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the isactive field
        /// </summary>
        [DisplayName("Is Active")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary> 
        [DisplayName("CreatedBy")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }


        /// <summary>
        /// Get/Set method of the LastUpdateBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public String LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }

        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        public DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
        } 

        /// <summary>
        /// Get/Set method of the UOMConversionFactorDTO field
        /// </summary>
        public List<UOMConversionFactorDTO> UOMConversionFactorDTOList
        {
            get { return uomConversionFactorDTOList; }
            set { uomConversionFactorDTOList = value; }
        }

        /// <summary>
        /// Returns true or false whether the UOMDTO changed or any of its children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (uomConversionFactorDTOList != null &&
                   uomConversionFactorDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        ///  Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || uOMId < 0;
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
