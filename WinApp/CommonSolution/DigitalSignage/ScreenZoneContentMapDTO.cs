/********************************************************************************************
 * Project Name - ScreenZoneContentMap DTO
 * Description  - Data object of ScreenZoneContentMap
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        06-Mar-2017   Lakshminarayana          Created 
 *2.70.2        31-Jul-2019   Dakshakh raj             Modified : Added Parameterized costrustor
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// This is the ScreenZoneContentMap data object class. This acts as data holder for the ScreenZoneContentMap business object
    /// </summary>
    public class ScreenZoneContentMapDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ScreenContentID field
            /// </summary>
            SCREEN_CONTENT_ID,
            
            /// <summary>
            /// Search by ZoneID field
            /// </summary>
            ZONE_ID,
            
            /// <summary>
            /// Search by ContentTypeID field
            /// </summary>
            CONTENT_TYPE_ID,
           
            /// <summary>
            /// Search by ContentID field
            /// </summary>
            CONTENT_ID,
            
            /// <summary>
            /// Search by ContentType field
            /// </summary>
            CONTENT_TYPE,
           
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,
            
            /// <summary>
            /// Search by site id field
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int screenContentId;
        private int zoneId;
        private int contentTypeId;
        private string contentType;
        private int contentId;
        private int backImage;
        private string backImageFileName;
        private string backColor;
        private string borderSize;
        private string borderColor;
        private int imgSize;
        private string imgAlignment;
        private int imgRefreshSecs;
        private string videoRepeat;
        private int lookupRefreshSecs;
        private string lookupHeaderDisplay;
        private int tickerScrollDirection;
        private int tickerSpeed;
        private int tickerRefreshSecs;
        private int displayOrder;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private DSLookupDTO dSLookupDTO;
        private TickerDTO tickerDTO;
        private MediaDTO mediaDTO;
        private SignagePatternDTO signagePatternDTO;
        private string contentGuid;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ScreenZoneContentMapDTO()
        {
            log.LogMethodEntry();
            screenContentId = -1;
            masterEntityId = -1;
            contentTypeId = -1;
            zoneId = -1;
            contentId = -1;
            backImage = -1;
            tickerScrollDirection = -1;
            imgSize = -1;
            isActive = true;
            videoRepeat = "N";
            lookupHeaderDisplay = "N";
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ScreenZoneContentMapDTO(int screenContentId, int zoneId, int contentTypeId, string contentType,
                                       int contentId, int backImage, string backColor, string borderSize,
                                       string borderColor, int imgSize, string imgAlignment, int imgRefreshSecs,
                                       string videoRepeat, int lookupRefreshSecs, string lookupHeaderDisplay,
                                       int tickerScrollDirection, int tickerSpeed, int tickerRefreshSecs,
                                       int displayOrder, bool isActive, string contentGuid)
            :this()
        {
            log.LogMethodEntry(screenContentId, zoneId, contentTypeId, contentType,
                                        contentId, backImage, backColor, borderSize,
                                        borderColor, imgSize, imgAlignment, imgRefreshSecs,
                                        videoRepeat, lookupRefreshSecs, lookupHeaderDisplay,
                                        tickerScrollDirection, tickerSpeed, tickerRefreshSecs,
                                        displayOrder, isActive, contentGuid);
            this.screenContentId = screenContentId;
            this.zoneId = zoneId;
            this.contentTypeId = contentTypeId;
            this.contentType = contentType;
            this.contentId = contentId;
            this.backImage = backImage;
            this.backColor = backColor;
            this.borderSize = borderSize;
            this.borderColor = borderColor;
            this.imgSize = imgSize;
            this.imgAlignment = imgAlignment;
            this.imgRefreshSecs = imgRefreshSecs;
            this.videoRepeat = videoRepeat;
            this.lookupRefreshSecs = lookupRefreshSecs;
            this.lookupHeaderDisplay = lookupHeaderDisplay;
            this.tickerScrollDirection = tickerScrollDirection;
            this.tickerSpeed = tickerSpeed;
            this.tickerRefreshSecs = tickerRefreshSecs;
            this.displayOrder = displayOrder;
            this.isActive = isActive;
            this.contentGuid = contentGuid;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ScreenZoneContentMapDTO(int screenContentId, int zoneId, int contentTypeId, string contentType, 
                                       int contentId, int backImage, string backColor, string borderSize, 
                                       string borderColor, int imgSize, string imgAlignment, int imgRefreshSecs, 
                                       string videoRepeat, int lookupRefreshSecs, string lookupHeaderDisplay, 
                                       int tickerScrollDirection, int tickerSpeed, int tickerRefreshSecs, 
                                       int displayOrder, bool isActive, string createdBy,DateTime creationDate, 
                                       string lastUpdatedBy, DateTime lastUpdateDate, int siteId,int masterEntityId, 
                                       bool synchStatus, string guid, string contentGuid)
            :this(screenContentId, zoneId, contentTypeId, contentType,
                                        contentId, backImage, backColor, borderSize,
                                        borderColor, imgSize, imgAlignment, imgRefreshSecs,
                                        videoRepeat, lookupRefreshSecs, lookupHeaderDisplay,
                                        tickerScrollDirection, tickerSpeed, tickerRefreshSecs,
                                        displayOrder, isActive, contentGuid)
        {
            log.LogMethodEntry(createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId, masterEntityId, synchStatus, guid, contentGuid);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.contentGuid = contentGuid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ScreenContentId field
        /// </summary>
        [ReadOnly(true)]
        public int ScreenContentId
        {
            get
            {
                return screenContentId;
            }

            set
            {
                IsChanged = true;
                screenContentId = value;
            }
        }
        
        /// <summary>
        /// Get/Set method of the ZoneId field
        /// </summary>
        [Browsable(false)]
        public int ZoneId
        {
            get
            {
                return zoneId;
            }

            set
            {
                IsChanged = true;
                zoneId = value;
            }
        }
        
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
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
        /// Get/Set method of the ContentTypeId field
        /// </summary>
        
        [DisplayName("Content Type")]
        public int ContentTypeId
        {
            get
            {
                return contentTypeId;
            }

            set
            {
                IsChanged = true;
                contentTypeId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ContentType field
        /// </summary>
        [Browsable(false)]
        public string ContentType
        {
            get
            {
                return contentType;
            }

            set
            {
                IsChanged = true;
                contentType = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ContentId field
        /// </summary>
        [Browsable(true)]
        [DisplayName("Content")]
        public int ContentId
        {
            get
            {
                return contentId;
            }
            set
            {
                IsChanged = true;
                contentId = value;
            }
        }
       
        /// <summary>
        /// Get/Set method of the DisplayOrder field
        /// </summary>
        [DisplayName("Display Order")]
        public int DisplayOrder
        {
            get
            {
                return displayOrder;
            }

            set
            {
                IsChanged = true;
                displayOrder = value;
            }
        }
        
        /// <summary>
        /// Get/Set method of the ContentType field
        /// </summary>
        [DisplayName("Background Image")]
        public int BackImage
        {
            get
            {
                return backImage;
            }

            set
            {
                IsChanged = true;
                backImage = value;
            }
        }

        /// <summary>
        /// Get/Set method of the BackColor field
        /// </summary>
        [DisplayName("Back Color")]
        public string BackColor
        {
            get
            {
                return backColor;
            }

            set
            {
                IsChanged = true;
                backColor = value;
            }
        }

        /// <summary>
        /// Get/Set method of the BorderSize field
        /// </summary>
        [DisplayName("Border Size")]
        public string BorderSize
        {
            get
            {
                return borderSize;
            }

            set
            {
                IsChanged = true;
                borderSize = value;
            }
        }

        /// <summary>
        /// Get/Set method of the BorderColor field
        /// </summary>
        [DisplayName("Border Color")]
        public string BorderColor
        {
            get
            {
                return borderColor;
            }

            set
            {
                IsChanged = true;
                borderColor = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ImgSize field
        /// </summary>
        [DisplayName("Image Size")]
        public int ImgSize
        {
            get
            {
                return imgSize;
            }

            set
            {
                IsChanged = true;
                imgSize = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ImgAlignment field
        /// </summary>
        [DisplayName("Image Alignment")]
        public string ImgAlignment
        {
            get
            {
                return imgAlignment;
            }

            set
            {
                IsChanged = true;
                imgAlignment = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ImgRefreshSecs field
        /// </summary>
        [DisplayName("Refresh(Secs)")]
        public int ImgRefreshSecs
        {
            get
            {
                return imgRefreshSecs;
            }

            set
            {
                IsChanged = true;
                imgRefreshSecs = value;
            }
        }

        /// <summary>
        /// Get/Set method of the VideoRepeat field
        /// </summary>
        [DisplayName("Video Repeat")]
        public string VideoRepeat
        {
            get
            {
                return videoRepeat;
            }

            set
            {
                IsChanged = true;
                videoRepeat = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LookupRefreshSecs field
        /// </summary>
        [DisplayName("Lookup Refresh (Secs)")]
        public int LookupRefreshSecs
        {
            get
            {
                return lookupRefreshSecs;
            }

            set
            {
                IsChanged = true;
                lookupRefreshSecs = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LookupHeaderDisplay field
        /// </summary>
        [DisplayName("Lookup Header Display")]
        public string LookupHeaderDisplay
        {
            get
            {
                return lookupHeaderDisplay;
            }

            set
            {
                IsChanged = true;
                lookupHeaderDisplay = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TickerScrollDirection field
        /// </summary>
        [DisplayName("Ticker Scroll Direction")]
        public int TickerScrollDirection
        {
            get
            {
                return tickerScrollDirection;
            }

            set
            {
                IsChanged = true;
                tickerScrollDirection = value;
            }
        }
        
        /// <summary>
        /// Get/Set method of the TickerSpeed field
        /// </summary>
        [DisplayName("Ticker Speed")]
        public int TickerSpeed
        {
            get
            {
                return tickerSpeed;
            }

            set
            {
                IsChanged = true;
                tickerSpeed = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TickerRefreshSecs field
        /// </summary>
        [DisplayName("Ticker Refresh (Secs)")]
        public int TickerRefreshSecs
        {
            get
            {
                return tickerRefreshSecs;
            }

            set
            {
                IsChanged = true;
                tickerRefreshSecs = value;
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

        [DisplayName("Content Guid")]
        public string ContentGuid
        {
            get
            {
                return contentGuid;
            }

            set
            {
                IsChanged = true;
                contentGuid = value;
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
                    return notifyingObjectIsChanged || screenContentId < 0;
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
        /// Get/Set method of the DSLookupDTO field
        /// </summary>
        [Browsable(false)]
        public DSLookupDTO DSLookupDTO
        {
            get
            {
                return dSLookupDTO;
            }

            set
            {
                dSLookupDTO = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TickerDTO field
        /// </summary>
        [Browsable(false)]
        public TickerDTO TickerDTO
        {
            get
            {
                return tickerDTO;
            }

            set
            {
                tickerDTO = value;
            }
        }

        /// <summary>
        /// Get/Set method of the SignagePatternDTO field
        /// </summary>
        [Browsable(false)]
        public SignagePatternDTO SignagePatternDTO
        {
            get
            {
                return signagePatternDTO;
            }

            set
            {
                signagePatternDTO = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MediaDTO field
        /// </summary>
        [Browsable(false)]
        public MediaDTO MediaDTO
        {
            get
            {
                return mediaDTO;
            }

            set
            {
                mediaDTO = value;
            }
        }

        /// <summary>
        /// Get/Set method of the BackImageFileName field
        /// </summary>
        [Browsable(false)]
        public string BackImageFileName
        {
            get
            {
                return backImageFileName;
            }

            set
            {
                backImageFileName = value;
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
