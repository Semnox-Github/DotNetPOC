/********************************************************************************************
 * Project Name - PartnerRevenueShare DTO
 * Description  - Data object of Partner Revenue Share
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************
 *2.70        13-May-2019   Jagan Mohana Rao        Created 
 *2.90       21-May-2020   Mushahid Faizan     Modified : 3 tier changes for Rest API. 
 *********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.User
{
    public class PartnerRevenueShareDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by PARTNER_ID field
            /// </summary>
            PARTNER_ID,
            /// <summary>
            /// Search by PARTNER_ID field
            /// </summary>
            PARTNER_ID_LIST,
            /// <summary>
            /// Search by MACHINE_GROUP_ID field
            /// </summary>
            MACHINE_GROUP_ID,
            /// <summary>
            /// Search by POS_TYPE_ID field
            /// </summary>
            POS_TYPE_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            ISACTIVE
        }
        private int partnerRevenueShareId;
        private int machineGroupId;
        private int partnerId;
        private double revenueSharePercentage;
        private double minimumGuarantee;
        private int posTypeId;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int agentGroupId;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private bool isActive;


        private string month;
        private string totalAmount;
        private string finalAmount;
        private string partner;
        private string machineGroup;
        private string agentGroupName;
        private string shareAmount;


        public PartnerRevenueShareDTO(string month, string partner, string machineGroup, string agentGroupName, string totalAmount, double revenueSharePercentage, string shareAmount, double minimumGuarantee, string finalAmount)
        {
            log.LogMethodEntry();
            this.month = month;
            this.machineGroup = machineGroup;
            this.partner = partner;
            this.revenueSharePercentage = revenueSharePercentage;
            this.minimumGuarantee = minimumGuarantee;
            this.agentGroupName = agentGroupName;
            this.totalAmount = totalAmount;
            this.shareAmount = shareAmount;
            this.finalAmount = finalAmount;
            log.LogMethodExit();
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PartnerRevenueShareDTO()
        {
            log.LogMethodEntry();
            this.partnerRevenueShareId = -1;
            this.machineGroupId = -1;
            this.partnerId = -1;
            this.posTypeId = -1;
            log.LogMethodExit();
        }
        public PartnerRevenueShareDTO(string month, int partnerId, int machineGroupId, int agentGroupId, double revenueSharePercentage, double minimumGuarantee)
        {
            log.LogMethodEntry();
            this.month = month;
            this.machineGroupId = machineGroupId;
            this.partnerId = partnerId;
            this.revenueSharePercentage = revenueSharePercentage;
            this.minimumGuarantee = minimumGuarantee;
            this.agentGroupId = agentGroupId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public PartnerRevenueShareDTO(int partnerRevenueShareId, int partnerId, int machineGroupId, double revenueSharePercentage, double minimumGuarantee, int posTypeId, string guid, int siteId,
            bool synchStatus, int agentGroupId, int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, bool isActive)
        {
            log.LogMethodEntry(partnerRevenueShareId, partnerId, machineGroupId, revenueSharePercentage, minimumGuarantee, posTypeId, siteId, guid, synchStatus, agentGroupId, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, isActive);
            this.partnerRevenueShareId = partnerRevenueShareId;
            this.machineGroupId = machineGroupId;
            this.partnerId = partnerId;
            this.revenueSharePercentage = revenueSharePercentage;
            this.minimumGuarantee = minimumGuarantee;
            this.posTypeId = posTypeId;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.agentGroupId = agentGroupId;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        //  public string RevenueSharePercentage { get { return revenueSharePercentage; } set { revenueSharePercentage = value; } }
        public string Month { get { return month; } set { month = value; } }
        public string Partner { get { return partner; } set { partner = value; } }
        public string MachineGroup { get { return machineGroup; } set { machineGroup = value; } }
        public string AgentGroupName { get { return agentGroupName; } set { agentGroupName = value; } }
        public string ShareAmount { get { return shareAmount; } set { shareAmount = value; } }
        public string TotalAmount { get { return totalAmount; } set { totalAmount = value; } }
        public string FinalAmount { get { return finalAmount; } set { finalAmount = value; } }
        // public string MinimumGuarantee { get { return minimumGuarantee; } set { minimumGuarantee = value; } }

        /// <summary>
        /// Get/Set method of the PartnerRevenueShareId field
        /// </summary>
        [DisplayName("PartnerRevenueShareId")]
        public int PartnerRevenueShareId { get { return partnerRevenueShareId; } set { partnerRevenueShareId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PartnerId field
        /// </summary>
        [DisplayName("PartnerId")]
        public int PartnerId { get { return partnerId; } set { partnerId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MachineGroupId field
        /// </summary>
        [DisplayName("Group Id")]
        public int MachineGroupId { get { return machineGroupId; } set { machineGroupId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RevenueSharePercentage field
        /// </summary>
        [DisplayName("Revenue Share %")]
        public double RevenueSharePercentage { get { return revenueSharePercentage; } set { revenueSharePercentage = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MinimumGuarantee field
        /// </summary>
        [DisplayName("Min Guarantee(Monthly)")]
        public double MinimumGuarantee { get { return minimumGuarantee; } set { minimumGuarantee = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PosTypeId field
        /// </summary>
        [DisplayName("POS Counter")]
        [Browsable(false)]
        public int POSTypeId { get { return posTypeId; } set { posTypeId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Siteid field
        /// </summary>
        [DisplayName("Site id")]
        [Browsable(false)]
        public int Siteid { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        /// <summary>
        /// Get/Set method of the AgentGroupId field
        /// </summary>
        [DisplayName("Agent Group")]
        public int AgentGroupId { get { return agentGroupId; } set { agentGroupId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastupdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }
        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Is Active")]
        [Browsable(false)]
        public bool IsActive { get { return isActive; } set { isActive = value; } }
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
                    return notifyingObjectIsChanged || partnerRevenueShareId < 0;
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