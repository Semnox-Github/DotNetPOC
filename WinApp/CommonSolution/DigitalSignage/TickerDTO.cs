/********************************************************************************************
 * Project Name - Ticker DTO
 * Description  - Data object of Ticker
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        03-Mar-2017   Lakshminarayana          Created 
 *2.70.2        31-Jul-2019   Dakshakh raj             Modified : Added Parameterized costrustor
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.Text;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// This is the Ticker data object class. This acts as data holder for the Ticker business object
    /// </summary>
    public class TickerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  TickerID field
            /// </summary>
            TICKER_ID,
            
            /// <summary>
            /// Search by Name field
            /// </summary>
            NAME,
            
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

        private int tickerId;
        private string name;
        private string tickerText;
        private string textColor;
        private int scrollDirection;
        private string font;
        private string backColor;
        private int? tickerSpeed;
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
        public TickerDTO()
        {
            log.LogMethodEntry();
            tickerId = -1;
            masterEntityId = -1;
            isActive = true;
            scrollDirection = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public TickerDTO(int tickerId, string name, string tickerText, string textColor, int scrollDirection, string font,
                         string backColor, int? tickerSpeed, bool isActive)
        {
            log.LogMethodEntry(tickerId, name, tickerText, textColor, scrollDirection, font, backColor, tickerSpeed, isActive);
            this.tickerId = tickerId;
            this.name = name;
            this.tickerText = tickerText;
            this.textColor = textColor;
            this.scrollDirection = scrollDirection;
            this.font = font;
            this.backColor = backColor;
            this.tickerSpeed = tickerSpeed;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public TickerDTO(int tickerId, string name, string tickerText, string textColor, int scrollDirection, string font,
                         string backColor, int? tickerSpeed, bool isActive, string createdBy,DateTime creationDate, 
                         string lastUpdatedBy, DateTime lastUpdateDate, int siteId,int masterEntityId, bool synchStatus,
                         string guid)
            :this(tickerId, name, tickerText, textColor, scrollDirection, font, backColor, tickerSpeed, isActive)
        {
            log.LogMethodEntry(createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId, masterEntityId, synchStatus, guid);
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
        /// Get/Set method of the TickerId field
        /// </summary>
        [DisplayName("ID")]
        [ReadOnly(true)]
        public int TickerId
        {
            get
            {
                return tickerId;
            }

            set
            {
                this.IsChanged = true;
                tickerId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Ticker Name")]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                this.IsChanged = true;
                name = value;
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
        /// Get/Set method of the Ticker Text field
        /// </summary>
        [DisplayName("Ticker Text")]
        public string TickerText
        {
            get
            {
                return tickerText;
            }

            set
            {
                this.IsChanged = true;
                tickerText = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Text Color field
        /// </summary>
        [DisplayName("Text Color")]
        public string TextColor
        {
            get
            {
                return textColor;
            }

            set
            {
                this.IsChanged = true;
                textColor = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Scroll Direction field
        /// </summary>
        [DisplayName("Scroll Direction")]
        public int ScrollDirection
        {
            get
            {
                return scrollDirection;
            }

            set
            {
                this.IsChanged = true;
                scrollDirection = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Font field
        /// </summary>
        [DisplayName("Font")]
        public string Font
        {
            get
            {
                return font;
            }

            set
            {
                this.IsChanged = true;
                font = value;
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
                this.IsChanged = true;
                backColor = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TickerSpeed field
        /// </summary>
        [DisplayName("Ticker Speed (Number)")]
        public int? TickerSpeed
        {
            get
            {
                return tickerSpeed;
            }

            set
            {
                this.IsChanged = true;
                tickerSpeed = value;
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
                    return notifyingObjectIsChanged || tickerId < 0 ;
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
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Returns a string that represents the current TickerDTO.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            StringBuilder returnValue = new StringBuilder("\n-----------------------TickerDTO-----------------------------\n");
            returnValue.Append(" TickerID : " + TickerId);
            returnValue.Append(" Name : " + Name);
            returnValue.Append(" TickerText : " + TickerText);
            returnValue.Append(" IsActive : " + IsActive);
            returnValue.Append(" ScrollDirection : " + ScrollDirection);
            returnValue.Append(" TextColor : " + TextColor);
            returnValue.Append(" Font : " + Font);
            returnValue.Append(" BackColor : " + BackColor);
            returnValue.Append(" TickerSpeed : " + TickerSpeed);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.LogMethodExit(returnValue.ToString());
            return returnValue.ToString();
        }
    }
}
