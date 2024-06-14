/********************************************************************************************
 * Project Name - Customer App User Log                                                                     
 * Description  - DTO for Customer App configuration
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         08-May-2019   Nitin Pai            Created for Guest app
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.CustomerApp
{
    public class CustomerAppUserLogDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int id;
        private int customerId;
        private string controller;
        private string action;
        private string variableState;
        private string message;
        private string uuid;
        private string GUID;
        private bool isActive;
        private bool synchStatus;
        private int siteId;
        private int masterEntityId;
        private DateTime createdDate;
        private string createdBy;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        public CustomerAppUserLogDTO()
        {
            log.LogMethodEntry();
            id = -1;
            masterEntityId = -1;
            isActive = true;
            synchStatus = false;
            siteId = -1;
            log.LogMethodExit(null);
        }

        public CustomerAppUserLogDTO(int id, int customerId, string controller, string action, string variableState, string message, string uuid, string GUID, bool isActive, bool synchStatus, int siteId, int masterEntityId, DateTime createdDate, string createdBy, string lastUpdatedBy, DateTime lastUpdatedDate)
        {
            log.LogMethodEntry(id, customerId, controller, action, variableState, message, uuid, GUID, isActive, synchStatus, siteId, masterEntityId, createdDate, createdBy, lastUpdatedBy, lastUpdatedDate);
            this.id = id;
            this.customerId = customerId;
            this.controller = controller;
            this.action = action;
            this.variableState = variableState;
            this.message = message;
            this.uuid = uuid;
            this.GUID = GUID;
            this.isActive = isActive;
            this.synchStatus = synchStatus;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.createdDate = createdDate;
            this.createdBy = createdBy;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.uuid = uuid;
            log.LogMethodExit(null);
        }

        public int Id { get { return id; } set { id = value; } }
        public int CustomerId { get { return customerId; } set { customerId = value; IsChanged = true; } }
        public string Controller { get { return controller; } set { controller = value; IsChanged = true; } }
        public string Action { get { return action; } set { action = value; IsChanged = true; } }
        public string VariableState { get { return variableState; } set { variableState = value; IsChanged = true; } }
        public string Message { get { return message; } set { message = value; IsChanged = true; } }
        public string UUID { get { return uuid; } set { uuid = value; IsChanged = true; } }
        public string guid { get { return GUID; } set { GUID = value; IsChanged = true; } }
        public bool IsActive { get { return isActive; } set { isActive = value; IsChanged = true; } }
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; IsChanged = true; } }
        public int SiteId { get { return siteId; } set { siteId = value; IsChanged = true; } }
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; IsChanged = true; } }
        public DateTime CreatedDate { get { return createdDate; } set { createdDate = value; IsChanged = true; } }
        public string CreatedBy { get { return createdBy; } set { createdBy = value; IsChanged = true; } }
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; IsChanged = true; } }
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; IsChanged = true; } }

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
                    return notifyingObjectIsChanged;
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
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            IsChanged = false;
            log.LogMethodExit(null);
        }
    }
}
