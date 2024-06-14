/********************************************************************************************
 * Project Name - Product
 * Description  - Product menu panel content data transfer object
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ********************************************************************************************* 
 *2.130.0        27-May-2021      Prajwal S       Created
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Product
{
    public class ProductMenuPanelContentDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        private int id;
        private int panelId;
        private string objectType;
        private string objectGuid;
        private string imageURL;
        private string backColor;
        private string textColor;
        private string font;
        private int columnIndex;
        private int rowIndex;
        private string buttonType;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool isActive;

        public ProductMenuPanelContentDTO()
        {
            log.LogMethodEntry();
            id = -1;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            objectType = string.Empty;
            panelId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public ProductMenuPanelContentDTO(int id, int panelId, string objectType, string objectGuid, string imageURL, string backColor, string textColor, string font, int columnIndex, int rowIndex, string buttonType, bool isActive)
            : this()
        {
            log.LogMethodEntry(id, panelId, objectType, objectGuid, imageURL, backColor, textColor, font, columnIndex, rowIndex, buttonType, isActive);
            this.id = id;
            this.panelId = panelId;
            this.objectType = objectType;
            this.objectGuid = objectGuid;
            this.imageURL = imageURL;
            this.backColor = backColor;
            this.textColor = textColor;
            this.columnIndex = columnIndex;
            this.rowIndex = rowIndex;
            this.buttonType = buttonType;
            this.font = font;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ProductMenuPanelContentDTO(int id, int panelId, string objectType, string objectGuid, string imageURL, string backColor, string textColor, string font, int columnIndex, int rowIndex, string buttonType, string guid, int siteId, bool synchStatus, int masterEntityId,
                             string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, bool isActive)
            : this(id, panelId, objectType, objectGuid, imageURL, backColor, textColor, font, columnIndex, rowIndex, buttonType, isActive)
        {
            log.LogMethodEntry(id, panelId, objectType, imageURL, backColor, textColor, font, columnIndex, rowIndex, buttonType, guid, siteId, synchStatus, masterEntityId, creationDate, lastUpdatedBy, lastUpdateDate, isActive);
            this.masterEntityId = masterEntityId;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Copy Constructor.
        /// </summary>
        public ProductMenuPanelContentDTO(ProductMenuPanelContentDTO productMenuPanelContentDTO)
            : this()
        {
            log.LogMethodEntry(productMenuPanelContentDTO); 
            id = productMenuPanelContentDTO.id;
            panelId = productMenuPanelContentDTO.panelId;
            objectType = productMenuPanelContentDTO.objectType;
            objectGuid = productMenuPanelContentDTO.objectGuid;
            imageURL = productMenuPanelContentDTO.imageURL;
            backColor = productMenuPanelContentDTO.backColor;
            textColor = productMenuPanelContentDTO.textColor;
            buttonType = productMenuPanelContentDTO.buttonType;
            font = productMenuPanelContentDTO.font;
            columnIndex = productMenuPanelContentDTO.columnIndex;
            rowIndex = productMenuPanelContentDTO.rowIndex;
            columnIndex = productMenuPanelContentDTO.columnIndex;
            isActive = productMenuPanelContentDTO.isActive;
            siteId = productMenuPanelContentDTO.siteId;
            synchStatus = productMenuPanelContentDTO.synchStatus;
            guid = productMenuPanelContentDTO.guid;
            lastUpdatedBy = productMenuPanelContentDTO.lastUpdatedBy;
            lastUpdatedDate = productMenuPanelContentDTO.lastUpdatedDate;
            createdBy = productMenuPanelContentDTO.createdBy;
            creationDate = productMenuPanelContentDTO.creationDate;
            masterEntityId = productMenuPanelContentDTO.masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Id { get { return id; } set { this.IsChanged = true; id = value; } }

        /// <summary>
        /// Get/Set method of the PanelId field
        /// </summary>
        public int PanelId { get { return panelId; } set { this.IsChanged = true; panelId = value; } }

        /// <summary>
        /// Get/Set method of the ObjectType field
        /// </summary>
        public string ObjectType { get { return objectType; } set { this.IsChanged = true; objectType = value; } }
        /// <summary>
        /// Get/Set method of the ObjectType field
        /// </summary>
        public string ObjectGuid { get { return objectGuid; } set { this.IsChanged = true; objectGuid = value; } }

        /// <summary>
        /// Get/Set method of the RowIndex field
        /// </summary>
        public int RowIndex { get { return rowIndex; } set { this.IsChanged = true; rowIndex = value; } }

        /// <summary>
        /// Get/Set method of the ColumnIndex field
        /// </summary>
        public int ColumnIndex { get { return columnIndex; } set { this.IsChanged = true; columnIndex = value; } }

        /// <summary>
        /// Get/Set method of the ImageURL field
        /// </summary>
        public string ImageURL { get { return imageURL; } set { this.IsChanged = true; imageURL = value; } }

        /// <summary>
        /// Get/Set method of the ButtonType field
        /// </summary>
        public string ButtonType { get { return buttonType; } set { this.IsChanged = true; buttonType = value; } }

        /// <summary>
        /// Get/Set method of the BackColor field
        /// </summary>
        public string BackColor { get { return backColor; } set { this.IsChanged = true; backColor = value; } }

        /// <summary>
        /// Get/Set method of the TextColor field
        /// </summary>
        public string TextColor { get { return textColor; } set { this.IsChanged = true; textColor = value; } }

        /// <summary>
        /// Get/Set method of the Font field
        /// </summary>
        public string Font { get { return font; } set { this.IsChanged = true; font = value; } }

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
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

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
                    return notifyingObjectIsChanged || id < 0;
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
