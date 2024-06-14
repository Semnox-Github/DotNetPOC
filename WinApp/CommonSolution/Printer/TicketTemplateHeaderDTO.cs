/********************************************************************************************
 * Project Name - Printer
 * Description  - Data object of TicketTemplateHeader
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        27-May-2019   Divya A                 Created 
 *2.130       27-Dec-2021   Girish kundar           Modified: Added NotchDistance,NotchWidth,PrintReverse fields as per Wristband issue Fix 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Printer
{
    /// <summary>
    /// This is the TicketTemplateHeader data object class. This acts as data holder for the TicketTemplateHeader business objects
    /// </summary>
    public class TicketTemplateHeaderDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByTicketTemplateHeaderParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByTicketTemplateHeaderParameters
        {
            /// <summary>
            /// Search by TicketTemplate Id field
            /// </summary>
            TICKET_TEMPLATE_ID,
            /// <summary>
            /// Search by Template Id field
            /// </summary>
            TEMPLATE_ID,
            /// <summary>
            /// Search by Site Id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Master Entity Id field
            /// </summary>
            MASTER_ENTITY_ID,
            IS_ACTIVE
        }

        private int ticketTemplateId;
        private int templateId;
        private decimal width;
        private decimal height;
        private int? leftMargin;
        private int? rightMargin;
        private int? topMargin;
        private int? bottomMargin;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private int? borderWidth;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private byte[] backgroundImage;
        private int? backsideTemplateId;
        private string createdBy;
        private DateTime creationDate;
        private bool? activeFlag;
        private decimal? notchDistance;
        private decimal? notchWidth;
        private bool printReverse;

        private List<TicketTemplateElementDTO> ticketTemplateElementDTOList;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor of TicketTemplateHeaderDTO
        /// </summary>
        public TicketTemplateHeaderDTO()
        {
            log.LogMethodEntry();
            ticketTemplateId = -1;
            templateId = -1;
            siteId = -1;
            masterEntityId = -1;
            notchDistance = null;
            notchWidth = null;
            printReverse = false;
            ticketTemplateElementDTOList = new List<TicketTemplateElementDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor of TicketTemplateHeaderDTO with required fields
        /// </summary>
        public TicketTemplateHeaderDTO(int ticketTemplateId, int templateId, decimal width, decimal height,
                        int? leftMargin, int? rightMargin, int? topMargin, int? bottomMargin, int? borderWidth,
                        byte[] backgroundImage, int? backsideTemplateId, bool? activeFlag, decimal? notchDistance,
                        decimal? notchWidth, bool printReverse) : this()
        {
            log.LogMethodEntry(ticketTemplateId, templateId, width, height, leftMargin, rightMargin, topMargin, bottomMargin, borderWidth,
                   backgroundImage, backsideTemplateId, activeFlag, notchDistance, notchWidth, printReverse);
            this.ticketTemplateId = ticketTemplateId;
            this.templateId = templateId;
            this.width = width;
            this.height = height;
            this.leftMargin = leftMargin;
            this.rightMargin = rightMargin;
            this.topMargin = topMargin;
            this.bottomMargin = bottomMargin;
            this.borderWidth = borderWidth;
            this.backgroundImage = backgroundImage;
            this.backsideTemplateId = backsideTemplateId;
            this.activeFlag = activeFlag;
            this.notchDistance = notchDistance;
            this.notchWidth = notchWidth;
            this.printReverse = printReverse;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor of TicketTemplateHeaderDTO with all fields
        /// </summary>
        public TicketTemplateHeaderDTO(int ticketTemplateId, int templateId, decimal width, decimal height,
                        int? leftMargin, int? rightMargin, int? topMargin, int? bottomMargin, DateTime lastUpdateDate,
                        string lastUpdatedBy, int? borderWidth, string guid, int siteId, bool synchStatus, int masterEntityId,
                        byte[] backgroundImage, int? backsideTemplateId, string createdBy, DateTime creationDate, bool? activeFlag,
                        decimal? notchDistance, decimal? notchWidth, bool printReverse)
            : this(ticketTemplateId, templateId, width, height, leftMargin, rightMargin, topMargin, bottomMargin, borderWidth,
                  backgroundImage, backsideTemplateId, activeFlag, notchDistance, notchWidth, printReverse)
        {
            log.LogMethodEntry(ticketTemplateId, templateId, width, height, leftMargin, rightMargin, topMargin, bottomMargin, borderWidth,
                  backgroundImage, backsideTemplateId, lastUpdateDate, lastUpdatedBy, guid, siteId, synchStatus, masterEntityId, createdBy, creationDate, activeFlag,
                  notchDistance, notchWidth, printReverse);
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the TicketTemplateId field
        /// </summary>
        public int TicketTemplateId { get { return ticketTemplateId; } set { ticketTemplateId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TemplateId field
        /// </summary>
        public int TemplateId { get { return templateId; } set { templateId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Width field
        /// </summary>
        public decimal Width { get { return width; } set { width = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Height field
        /// </summary>
        public decimal Height { get { return height; } set { height = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LeftMargin field
        /// </summary>
        public int? LeftMargin { get { return leftMargin; } set { leftMargin = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RightMargin field
        /// </summary>
        public int? RightMargin { get { return rightMargin; } set { rightMargin = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TopMargin field
        /// </summary>
        public int? TopMargin { get { return topMargin; } set { topMargin = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the BottomMargin field
        /// </summary>
        public int? BottomMargin { get { return bottomMargin; } set { bottomMargin = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get/Set method of the BorderWidth field
        /// </summary>
        public int? BorderWidth { get { return borderWidth; } set { borderWidth = value; IsChanged = true; } }
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
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the BackgroundImage field
        /// </summary>
        public byte[] BackgroundImage { get { return backgroundImage; } set { backgroundImage = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the BacksideTemplateId field
        /// </summary>
        public int? BacksideTemplateId { get { return backsideTemplateId; } set { backsideTemplateId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        public bool? ActiveFlag { get { return activeFlag; } set { activeFlag = value; IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the NotchDistance field
        /// </summary>
        public decimal? NotchDistance { get { return notchDistance; } set { notchDistance = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the NotchWidth field
        /// </summary>
        public decimal? NotchWidth { get { return notchWidth; } set { notchWidth = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PrintReverse field
        /// </summary>
        public bool PrintReverse { get { return printReverse; } set { printReverse = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TicketTemplateElementDTO List
        /// </summary>
        public List<TicketTemplateElementDTO> TicketTemplateElementDTOList { get { return ticketTemplateElementDTOList; } set { ticketTemplateElementDTOList = value; } }

        /// <summary>
        /// Returns whether the TicketTemplateHeaderDTO changed or any of children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (ticketTemplateElementDTOList != null &&
                    ticketTemplateElementDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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
                    return notifyingObjectIsChanged || ticketTemplateId < 0;
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
