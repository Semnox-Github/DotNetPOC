/********************************************************************************************
 * Project Name - Sequences
 * Description  - Data object of DequenceDTO  
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.60        14-Mar-2016   Jagan Mohana        Created 
              13-May-2019   Mushahid Faizan     Added log MethodEntry/Exit in AcceptChanges() method.
 *2.90        11-May-2020   Girish Kundar       Modified : Changes as part of the REST API  
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// DTO class for sequence object
    /// </summary>
    public class SequencesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Search parameters
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by SEQUENCE_ID
            /// </summary>
            SEQUENCE_ID,
            /// <summary>
            /// Search by SEQUENCE_NAME
            /// </summary>
            SEQUENCE_NAME,
            /// <summary>
            /// Search by ORDER_TYPE_GROUP_ID
            /// </summary>
            ORDER_TYPE_GROUP_ID,

            /// <summary>
            /// Search by PREFIX
            /// </summary>
            PREFIX,
            /// <summary>
            /// Search by SUFFIX
            /// </summary>
            SUFFIX,
            /// <summary>
            /// Search by SITE_ID
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by ISACTIVE
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by POS MACHINE ID
            /// </summary>
            POS_MACHINE_ID,
            /// <summary>
            /// Search by POS MACHINE ID
            /// </summary>
            GUID
        }

      private int sequenceId;
      private string sequenceName;
      private int seed;
      private int incrementBy;
      private int currentVal;
      private bool synchStatus;
      private string guid;
      private int siteId;
      private string prefix;
      private string suffix;
      private int width;
      private string userColumnHeading;
      private int posMachineId;
      private int? maximumValue;
      private int masterEntityId;
      private int orderTypeGroupId;
      private string createdBy;
      private DateTime creationDate;
      private string lastUpdatedBy;
      private DateTime lastUpdateDate;
      private bool isActive;

        bool notifyingObjectIsChanged;
        readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>  
        public SequencesDTO()
        {
            log.LogMethodEntry();
            this.sequenceId = -1;
            this.siteId = -1;
            this.sequenceName = string.Empty;
            this.posMachineId = -1;
            this.orderTypeGroupId = -1;
            this.isActive = true;
            this.masterEntityId = -1;
            this.maximumValue = null;
            log.LogMethodExit();
        }

        /// <summary>
        ///  constructor with required Parameter
        /// </summary>
        public SequencesDTO(string sequenceName, int seed, int incrementBy, int currentVal, string prefix, string suffix, int width, string userColumnHeading, int sequenceId,
                               int posMachineId, int? maximumValue, int orderTypeGroupId, bool isActive)
            :this()
        {
            log.LogMethodEntry(sequenceName, seed, incrementBy, currentVal, prefix, suffix, width, userColumnHeading, sequenceId, posMachineId,
                               maximumValue, orderTypeGroupId,isActive);
            this.sequenceId = sequenceId;
            this.sequenceName = sequenceName;
            this.seed = seed;
            this.incrementBy = incrementBy;
            this.currentVal = currentVal;
            this.prefix = prefix;
            this.suffix = suffix;
            this.width = width;
            this.userColumnHeading = userColumnHeading;
            this.posMachineId = posMachineId;
            this.maximumValue = maximumValue;
            this.orderTypeGroupId = orderTypeGroupId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        ///  constructor with Parameter
        /// </summary>
        public SequencesDTO(string sequenceName, int seed, int incrementBy, int currentVal, string guid,
                               bool synchStatus, int siteId, string prefix, string suffix, int width, string userColumnHeading, int sequenceId,
                               int posMachineId, int? maximumValue, int masterEntityId, int orderTypeGroupId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, bool isActive)
            :this(sequenceName, seed, incrementBy, currentVal, prefix, suffix, width, userColumnHeading, sequenceId, posMachineId,
                               maximumValue, orderTypeGroupId, isActive)
        {
            log.LogMethodEntry(sequenceName, seed, incrementBy, currentVal, guid, synchStatus, siteId, prefix, suffix, width, userColumnHeading, sequenceId, posMachineId,
                               maximumValue, masterEntityId, orderTypeGroupId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, isActive);
            
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the SequenceId field
        /// </summary>
        [DisplayName("SequenceId")]
        public int SequenceId { get { return sequenceId; } set { sequenceId = value; } }

        /// <summary>
        /// Get/Set method of the SequenceName field
        /// </summary>
        [DisplayName("SequenceName")]
        public string SequenceName { get { return sequenceName; } set { sequenceName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Seed field
        /// </summary>
        [DisplayName("Seed")]
        public int Seed { get { return seed; } set { seed = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IncrementBy field
        /// </summary>
        [DisplayName("IncrementBy")]
        public int IncrementBy { get { return incrementBy; } set { incrementBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CurrentVal field
        /// </summary>
        [DisplayName("CurrentVal")]
        public int CurrentVal { get { return currentVal; } set { currentVal = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("Site_id")]
        public int Site_id { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the Prefix field
        /// </summary>
        [DisplayName("Prefix")]
        public string Prefix { get { return prefix; } set { prefix = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Suffix field
        /// </summary>
        [DisplayName("Suffix")]
        public string Suffix { get { return suffix; } set { suffix = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Width field
        /// </summary>
        [DisplayName("Width")]
        public int Width { get { return width; } set { width = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the User Column Heading Lookup field
        /// </summary>
        [DisplayName("User Column Heading")]
        public string UserColumnHeading { get { return userColumnHeading; } set { userColumnHeading = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the POSMachineId field
        /// </summary>
        [DisplayName("POSMachineId")]
        public int POSMachineId { get { return posMachineId; } set { posMachineId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MaximumValue field
        /// </summary>
        [DisplayName("Maximum Value")]
        public int? MaximumValue { get { return maximumValue; } set { maximumValue = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the OrderTypeGroupId field
        /// </summary>
        [DisplayName("Order Type Group")]
        public int OrderTypeGroupId { get { return orderTypeGroupId; } set { orderTypeGroupId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value;  } }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value;  } }
        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || sequenceId < 0;
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