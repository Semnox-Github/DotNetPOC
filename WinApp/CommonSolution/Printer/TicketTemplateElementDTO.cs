/********************************************************************************************
 * Project Name - Printer
 * Description  - Data object of TicketTemplate
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        27-May-2019   Divya A                 Created 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Printer
{
    /// <summary>
    /// This is the TicketTemplateElement data object class. This acts as data holder for the TicketTemplateElement business objects
    /// </summary>
    public class TicketTemplateElementDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByTicketTemplateElementParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByTicketTemplateElementParameters
        {
            /// <summary>
            /// Search by TicketTemplateElement Id field
            /// </summary>
            TICKET_TEMPLATE_ELEMENT_ID,
            /// <summary>
            /// Search by TicketTemplate Id field
            /// </summary>
            TICKET_TEMPLATE_ID,
            /// <summary>
            /// Search by TicketTemplate Id  List field
            /// </summary>
            TICKET_TEMPLATE_ID_LIST,
            /// <summary>
            /// Search by Unique Id field
            /// </summary>
            UNIQUE_ID,
            /// <summary>
            /// Search by Type field
            /// </summary>
            TYPE,
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by Location field
            /// </summary>
            LOCATION,
            /// <summary>
            /// Search by Site Id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by master entity Id field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int ticketTemplateElementId;
        private int ticketTemplateId;
        private string uniqueId;
        private string name;
        private string value;
        private int type;
        private string font;
        private string location;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int width;
        private char alignment;
        private char rotate;
        private int masterEntityId;
        private int formatId;
        private string color;
        private int barCodeHeight;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private bool? activeFlag;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor for TicketTemplateElementDTO
        /// </summary>
        public TicketTemplateElementDTO()
        {
            log.LogMethodEntry();
            ticketTemplateElementId = -1;
            ticketTemplateId = -1;
            type = -1;
            barCodeHeight = -1;
            formatId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for TicketTemplateElementDTO with required fields
        /// </summary>
        public TicketTemplateElementDTO(int ticketTemplateElementId, int ticketTemplateId, string uniqueId,
                            string name, string value, int type, string font, string location, int width, char alignment, char rotate,
                            int formatId, string color, int barCodeHeight, bool? activeFlag) : this()
        {
            log.LogMethodEntry(ticketTemplateElementId, ticketTemplateId, uniqueId, name, value, type,
                font, location, guid, siteId, synchStatus, width, alignment, rotate, masterEntityId,
                formatId, color, barCodeHeight, activeFlag);
            this.ticketTemplateElementId = ticketTemplateElementId;
            this.ticketTemplateId = ticketTemplateId;
            this.uniqueId = uniqueId;
            this.name = name;
            this.value = value;
            this.type = type;
            this.font = font;
            this.location = location;
            this.width = width;
            this.alignment = alignment;
            this.rotate = rotate;
            this.formatId = formatId;
            this.color = color;
            this.barCodeHeight = barCodeHeight;
            this.activeFlag = activeFlag;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for TicketTemplateElementDTO
        /// </summary>
        public TicketTemplateElementDTO(int ticketTemplateElementId, int ticketTemplateId, string uniqueId,
                            string name, string value, int type, string font, string location, string guid,
                            int siteId, bool synchStatus, int width, char alignment, char rotate, int masterEntityId,
                            int formatId, string color, int barCodeHeight, string createdBy, DateTime creationDate,
                            string lastUpdatedBy, DateTime lastUpdateDate, bool? activeFlag)
            : this(ticketTemplateElementId, ticketTemplateId, uniqueId, name, value, type, font, location, width, alignment, rotate, formatId, color, barCodeHeight, activeFlag)
        {
            log.LogMethodEntry(ticketTemplateElementId, ticketTemplateId, uniqueId, name, value, type, font,
                location, guid, siteId, synchStatus, width, alignment, rotate, masterEntityId, formatId, color,
                barCodeHeight, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, activeFlag);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the TicketTemplateElementId field
        /// </summary>
        public int TicketTemplateElementId { get { return ticketTemplateElementId; } set { ticketTemplateElementId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TicketTemplateId field
        /// </summary>
        public int TicketTemplateId { get { return ticketTemplateId; } set { ticketTemplateId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UniqueId field
        /// </summary>
        public string UniqueId { get { return uniqueId; } set { uniqueId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        public string Name { get { return name; } set { name = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Value field
        /// </summary>
        public string Value { get { return value; } set { this.value = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Type field
        /// </summary>
        public int Type { get { return type; } set { type = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Font field
        /// </summary>
        public string Font { get { return font; } set { font = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Location field
        /// </summary>
        public string Location { get { return location; } set { location = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        /// <summary>
        /// Get/Set method of the Width field
        /// </summary>
        public int Width { get { return width; } set { width = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Alignment field
        /// </summary>
        public char Alignment { get { return alignment; } set { alignment = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Rotate field
        /// </summary>
        public char Rotate { get { return rotate; } set { rotate = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the FormatId field
        /// </summary>
        public int FormatId { get { return formatId; } set { formatId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Color field
        /// </summary>
        public string Color { get { return color; } set { color = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the BarCodeHeight field
        /// </summary>
        public int BarCodeHeight { get { return barCodeHeight; } set { barCodeHeight = value; IsChanged = true; } }
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
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }
        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        public bool? ActiveFlag { get { return activeFlag; } set { activeFlag = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || TicketTemplateElementId < 0;
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
