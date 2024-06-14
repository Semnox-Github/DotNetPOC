/********************************************************************************************
 * Project Name - Theme DTO
 * Description  - Data object of Event
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        01-Mar-2017   Lakshminarayana          Created 
 *2.70.2        30-Jul-2019   Dakshakh raj             Modified : Added Parameterized costrustor
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// This is the DSLookup data object class. This acts as data holder for the DSLookup business object
    /// </summary>
    public class DSLookupDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  DSLookupID field
            /// </summary>
            DSLOOKUP_ID,
            
            /// <summary>
            /// Search by DSLookupName field
            /// </summary>
            DSLOOKUP_NAME,
            
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,
            
            /// <summary>
            /// Search by site id field
            /// </summary>
            SITE_ID,
            
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int dSLookupID;
        private string dSLookupName;
        private string description;
        private int? offsetX;
        private int? offsetY;
        private int? hDR12Spacing;
        private int? hDR23Spacing;
        private int? hDR34Spacing;
        private int? hDR45Spacing;
        private int? textWidth;
        private int? textHeight;
        private string dynamicFlag;
        private string query;
        private int? refreshDataSecs;
        private int? moveDataSecs;
        private SortableBindingList<DSignageLookupValuesDTO> dSignageLookupValuesDTOList;

        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public DSLookupDTO()
        {
            log.LogMethodEntry();
            dSLookupID = -1;
            masterEntityId = -1;
            isActive = true;
            dynamicFlag = "N";
            siteId = -1;
            dSignageLookupValuesDTOList = new SortableBindingList<DSignageLookupValuesDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructorrequired data fields
        /// </summary>
        public DSLookupDTO(int dSLookupID, string dSLookupName, string description, int? offsetX, int? offsetY,
                            int? hDR12Spacing, int? hDR23Spacing, int? hDR34Spacing, int? hDR45Spacing, int? textWidth,
                            int? textHeight, string dynamicFlag, string query, int? refreshDataSecs, int? moveDataSecs,
                            bool isActive)
            :this()
        {
            log.LogMethodEntry(dSLookupID, dSLookupName, description, offsetX, offsetY,
                             hDR12Spacing, hDR23Spacing, hDR34Spacing, hDR45Spacing, textWidth,
                             textHeight, dynamicFlag, query, refreshDataSecs, moveDataSecs,
                             isActive);
            this.dSLookupID = dSLookupID;
            this.dSLookupName = dSLookupName;
            this.description = description;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.hDR12Spacing = hDR12Spacing;
            this.hDR23Spacing = hDR23Spacing;
            this.hDR34Spacing = hDR34Spacing;
            this.hDR45Spacing = hDR45Spacing;
            this.textWidth = textWidth;
            this.textHeight = textHeight;
            this.dynamicFlag = dynamicFlag;
            this.query = query;
            this.refreshDataSecs = refreshDataSecs;
            this.moveDataSecs = moveDataSecs;
            this.isActive = isActive;
            dSignageLookupValuesDTOList = new SortableBindingList<DSignageLookupValuesDTO>();
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DSLookupDTO(int dSLookupID, string dSLookupName, string description, int? offsetX, int? offsetY, 
                            int? hDR12Spacing, int? hDR23Spacing, int? hDR34Spacing, int? hDR45Spacing, int? textWidth, 
                            int? textHeight, string dynamicFlag, string query, int? refreshDataSecs, int? moveDataSecs,
                            bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, 
                            DateTime lastUpdateDate, int siteId, int masterEntityId, bool synchStatus, string guid)
            :this(dSLookupID, dSLookupName, description, offsetX, offsetY,
                             hDR12Spacing, hDR23Spacing, hDR34Spacing, hDR45Spacing, textWidth,
                             textHeight, dynamicFlag, query, refreshDataSecs, moveDataSecs,
                             isActive)
        {
            log.LogMethodEntry(createdBy, creationDate, lastUpdatedBy, lastUpdateDate,
                               siteId, masterEntityId, synchStatus, guid);
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
        /// Get/Set method of the DSLookupID field
        /// </summary>
        [DisplayName("SI#")]
        [ReadOnly(true)]
        public int DSLookupID
        {
            get
            {
                return dSLookupID;
            }

            set
            {
                this.IsChanged = true;
                dSLookupID = value;
            }
        }
        
        /// <summary>
        /// Get/Set method of the DSLookupName field
        /// </summary>
        [DisplayName("Name")]
        public string DSLookupName
        {
            get
            {
                return dSLookupName;
            }

            set
            {
                this.IsChanged = true;
                dSLookupName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the DynamicFlag field
        /// </summary>
        [DisplayName("Dynamic Flag")]
        public string DynamicFlag
        {
            get
            {
                return dynamicFlag;
            }

            set
            {
                this.IsChanged = true;
                dynamicFlag = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Query field
        /// </summary>
        [DisplayName("...")]
        public string Query
        {
            get
            {
                return query;
            }

            set
            {
                this.IsChanged = true;
                query = value;
            }
        }

        /// <summary>
        /// Get/Set method of the RefreshDataSecs field
        /// </summary>
        [DisplayName("Refresh Data (Secs)")]
        public int? RefreshDataSecs
        {
            get
            {
                return refreshDataSecs;
            }

            set
            {
                this.IsChanged = true;
                refreshDataSecs = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MoveDataSecs field
        /// </summary>
        [DisplayName("Move Data (Secs)")]
        public int? MoveDataSecs
        {
            get
            {
                return moveDataSecs;
            }

            set
            {
                this.IsChanged = true;
                moveDataSecs = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                this.IsChanged = true;
                description = value;
            }
        }

        /// <summary>
        /// Get/Set method of the OffsetX field
        /// </summary>
        [DisplayName("Offset- X Axis(in Pixels)")]
        public int? OffsetX
        {
            get
            {
                return offsetX;
            }

            set
            {
                this.IsChanged = true;
                offsetX = value;
            }
        }

        /// <summary>
        /// Get/Set method of the OffsetY field
        /// </summary>
        [DisplayName("Offset- Y Axis(in Pixels)")]
        public int? OffsetY
        {
            get
            {
                return offsetY;
            }

            set
            {
                this.IsChanged = true;
                offsetY = value;
            }
        }

        /// <summary>
        /// Get/Set method of the HDR12Spacing field
        /// </summary>
        [DisplayName("HDR 1 & HDR 2")]
        public int? HDR12Spacing
        {
            get
            {
                return hDR12Spacing;
            }

            set
            {
                this.IsChanged = true;
                hDR12Spacing = value;
            }
        }

        /// <summary>
        /// Get/Set method of the HDR23Spacing field
        /// </summary>
        [DisplayName("HDR 2 & HDR 3")]
        public int? HDR23Spacing
        {
            get
            {
                return hDR23Spacing;
            }

            set
            {
                this.IsChanged = true;
                hDR23Spacing = value;
            }
        }

        /// <summary>
        /// Get/Set method of the HDR34Spacing field
        /// </summary>
        [DisplayName("HDR 3 & HDR 4")]
        public int? HDR34Spacing
        {
            get
            {
                return hDR34Spacing;
            }

            set
            {
                this.IsChanged = true;
                hDR34Spacing = value;
            }
        }

        /// <summary>
        /// Get/Set method of the HDR45Spacing field
        /// </summary>
        [DisplayName("HDR 4 & HDR 5")]
        public int? HDR45Spacing
        {
            get
            {
                return hDR45Spacing;
            }

            set
            {
                this.IsChanged = true;
                hDR45Spacing = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TextHeight field
        /// </summary>
        [DisplayName("Text Height")]
        public int? TextHeight
        {
            get
            {
                return textHeight;
            }

            set
            {
                this.IsChanged = true;
                textHeight = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TextWidth field
        /// </summary>
        [DisplayName("Text Width")]
        public int? TextWidth
        {
            get
            {
                return textWidth;
            }

            set
            {
                this.IsChanged = true;
                textWidth = value;
            }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active Flag")]
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
            }
        }

        /// <summary>
        /// Get/Set method of the CreationDate field
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
                creationDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
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
                lastUpdatedBy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
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
                lastUpdateDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the SiteId field
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
                siteId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
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
                this.IsChanged = true;
                masterEntityId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the SynchStatus field
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
                synchStatus = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Guid field
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
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock(notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || dSLookupID < 0;
                }
            }

            set
            {
                lock(notifyingObjectIsChangedSyncRoot)
                {
                    if(!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }

        /// <summary>
        /// Get/Set method of the DSignageLookupValuesDTOList field
        /// </summary>
        [Browsable(false)]
        public SortableBindingList<DSignageLookupValuesDTO> DSignageLookupValuesDTOList
        {
            get
            {
                return dSignageLookupValuesDTOList;
            }

            set
            {
                dSignageLookupValuesDTOList = value;
            }
        }

        /// <summary>
        /// IsChangedRecursive
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (dSignageLookupValuesDTOList != null &&
                   dSignageLookupValuesDTOList.Any(x => x.IsChanged))
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
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns a string that represents the current DSLookupDTO
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            StringBuilder returnValue = new StringBuilder("\n-----------------------DSLookupDTO-----------------------------\n");
            returnValue.Append(" DSLookupID : " + DSLookupID);
            returnValue.Append(" DSLookupName : " + DSLookupName);
            returnValue.Append(" Description : " + Description);
            returnValue.Append(" IsActive : " + IsActive);
            returnValue.Append(" OffsetX : " + OffsetX);
            returnValue.Append(" OffsetY : " + OffsetY);
            returnValue.Append(" HDR12Spacing : " + HDR12Spacing);
            returnValue.Append(" HDR23Spacing : " + HDR23Spacing);
            returnValue.Append(" HDR34Spacing : " + HDR34Spacing);
            returnValue.Append(" HDR45Spacing : " + HDR45Spacing);
            returnValue.Append(" TextWidth : " + TextWidth);
            returnValue.Append(" TextHeight : " + TextHeight);
            returnValue.Append(" Dynamic_Flag : " + DynamicFlag);
            returnValue.Append(" Query : " + Query);
            returnValue.Append(" RefreshDataSecs : " + RefreshDataSecs);
            returnValue.Append(" MoveDataSecs : " + MoveDataSecs);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.LogMethodExit(returnValue.ToString());
            return returnValue.ToString();

        }
    }
}
