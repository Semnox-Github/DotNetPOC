/********************************************************************************************
 * Project Name - Membership
 * Description  - DTO of MembershipReward
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       19-Jul-2019    Girish Kundar  Modified : Added Constructor with required Parameter
  ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Customer.Membership
{
    /// <summary>
    /// This is the MembershipRule data object class. This acts as data holder for the MembershipRule business object
    /// </summary>
    public class MembershipRuleDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>

        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  MembershipRuleId field
            /// </summary>
            MEMBERSHIP_RULE_ID,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int membershipRuleID;
        private string ruleName;
        private string description;
        private int qualifyingPoints;
        private int qualificationWindow;
        private string unitOfQualificationWindow;
        private int retentionPoints;
        private int retentionWindow;
        private string unitOfRetentionWindow;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public MembershipRuleDTO()
        {
            log.LogMethodEntry();
            membershipRuleID = -1;
            ruleName = "";
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();

        }


        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public MembershipRuleDTO(int membershipRuleID, string ruleName, string description, int qualifyingPoints, int qualificationWindow,
                                 string unitOfQualificationWindow, int retentionPoints, int retentionWindow, string unitOfRetentionWindow,
                                 bool isActive)
            : this()
        {
            log.LogMethodEntry(membershipRuleID, ruleName, description, qualifyingPoints, qualificationWindow,
                                  unitOfQualificationWindow, retentionPoints, retentionWindow, unitOfRetentionWindow,
                                  isActive);
            this.membershipRuleID = membershipRuleID;
            this.ruleName = ruleName;
            this.description = description;
            this.qualifyingPoints = qualifyingPoints;
            this.qualificationWindow = qualificationWindow;
            this.unitOfQualificationWindow = unitOfQualificationWindow;
            this.retentionPoints = retentionPoints;
            this.retentionWindow = retentionWindow;
            this.unitOfRetentionWindow = unitOfRetentionWindow;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public MembershipRuleDTO(int membershipRuleID, string ruleName, string description, int qualifyingPoints, int qualificationWindow,
                                 string unitOfQualificationWindow, int retentionPoints, int retentionWindow, string unitOfRetentionWindow,
                                 bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate,
                                 int siteId, int masterEntityId, bool synchStatus, string guid)
            :this(membershipRuleID, ruleName, description, qualifyingPoints, qualificationWindow,
                                  unitOfQualificationWindow, retentionPoints, retentionWindow, unitOfRetentionWindow,
                                  isActive)
        {
            log.LogMethodEntry(membershipRuleID, ruleName, description, qualifyingPoints, qualificationWindow,
                                  unitOfQualificationWindow, retentionPoints, retentionWindow, unitOfRetentionWindow,
                                  isActive, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate,
                                  siteId, masterEntityId, synchStatus, guid);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the MembershipRuleID field
        /// </summary>
        [DisplayName("MembershipRuleID")]
        [ReadOnly(true)]
        public int MembershipRuleID
        {
            get
            {
                return membershipRuleID;
            }

            set
            {
                this.IsChanged = true;
                membershipRuleID = value;
            }
        }

        /// <summary>
        /// Get/Set method of the RuleName field
        /// </summary>
        [DisplayName("Rule Name")]
        public string RuleName
        {
            get
            {
                return ruleName;
            }

            set
            {
                this.IsChanged = true;
                ruleName = value;
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
        /// Get/Set method of the QualifyingPoints field
        /// </summary>
        [DisplayName("Qualifying Points")]
        public int QualifyingPoints
        {
            get
            {
                return qualifyingPoints;
            }

            set
            {
                this.IsChanged = true;
                qualifyingPoints = value;
            }
        }

        /// <summary>
        /// Get/Set method of the QualificationWindow field
        /// </summary>
        [DisplayName("Qualification Window")]
        public int QualificationWindow
        {
            get
            {
                return qualificationWindow;
            }

            set
            {
                this.IsChanged = true;
                qualificationWindow = value;
            }
        }

        /// <summary>
        /// Get/Set method of the UnitOfQualificationWindow field
        /// </summary>
        [DisplayName("Unit Of Qualification Window")]
        public string UnitOfQualificationWindow
        {
            get
            {
                return unitOfQualificationWindow;
            }

            set
            {
                this.IsChanged = true;
                unitOfQualificationWindow = value;
            }
        }

        /// <summary>
        /// Get/Set method of the RetentionPoints field
        /// </summary>
        [DisplayName("Retention Points")]
        public int RetentionPoints
        {
            get
            {
                return retentionPoints;
            }

            set
            {
                this.IsChanged = true;
                retentionPoints = value;
            }
        }

        /// <summary>
        /// Get/Set method of the RetentionWindow field
        /// </summary>
        [DisplayName("Retention Window")]
        public int RetentionWindow
        {
            get
            {
                return retentionWindow;
            }

            set
            {
                this.IsChanged = true;
                retentionWindow = value;
            }
        }

        /// <summary>
        /// Get/Set method of the UnitOfRetentionWindow field
        /// </summary>
        [DisplayName("Unit Of Retention Window")]
        public string UnitOfRetentionWindow
        {
            get
            {
                return unitOfRetentionWindow;
            }

            set
            {
                this.IsChanged = true;
                unitOfRetentionWindow = value;
            }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Is Active")]
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
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
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
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
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
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated User")]
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
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Updated Date")]
        [Browsable(false)]
        public DateTime LastUpdatedDate
        {
            get
            {
                return lastUpdatedDate;
            }
            set
            {
                lastUpdatedDate = value;
            }
        }

        /// <summary>
        /// Get method of the SiteId field
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
        /// Get method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
        }

        /// <summary>
        /// Get method of the Guid field
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
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || membershipRuleID < 0;
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
