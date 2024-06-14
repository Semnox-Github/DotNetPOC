/********************************************************************************************
 * Project Name - ShiftConfigurationsDTO
 * Description  - Data object of Shift Configurations
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.90.0      01-JUL-2020   Akshay Gulaganji   Created  
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// This is the ShiftConfigurations data object class. This acts as data holder for the ShiftConfigurations business object
    /// </summary>
    public class ShiftConfigurationsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by SHIFT CONFIGURATION ID field
            /// </summary>
            SHIFT_CONFIGURATION_ID,
            /// <summary>
            /// Search by SHIFT CONFIGURATION NAME field
            /// </summary>
            SHIFT_CONFIGURATION_NAME,
            /// <summary>
            /// Search by SHIFT TRACK ALLOWED field
            /// </summary>
            SHIFT_TRACK_ALLOWED,
            /// <summary>
            /// Search by OVERTIME ALLOWED field
            /// </summary>
            OVERTIME_ALLOWED,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int shiftConfigurationId;
        private string shiftConfigurationName;
        private int shiftMinutes;
        private int? weeklyShiftMinutes;
        private int graceMinutes;
        private bool shiftTrackAllowed;
        private bool overtimeAllowed;
        private int? maximumOvertimeMinutes;
        private int? maximumWeeklyOvertimeMinutes;
        private bool isActive;
        private string guid;
        private int masterEntityId;
        private int siteId;
        private bool synchStatus;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ShiftConfigurationsDTO()
        {
            log.LogMethodEntry();
            shiftConfigurationId = -1;
            weeklyShiftMinutes = null;
            shiftTrackAllowed = false;
            overtimeAllowed = false;
            maximumOvertimeMinutes = null;
            maximumWeeklyOvertimeMinutes = null;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with required fields
        /// </summary>
        /// <param name="shiftConfigurationId"></param>
        /// <param name="shiftConfigurationName"></param>
        /// <param name="shiftMinutes"></param>
        /// <param name="weeklyShiftMinutes"></param>
        /// <param name="graceMinutes"></param>
        /// <param name="shiftTrackAllowed"></param>
        /// <param name="overtimeAllowed"></param>
        /// <param name="maximumOvertimeMinutes"></param>
        /// <param name="maximumWeeklyOvertimeMinutes"></param>
        /// <param name="isActive"></param>
        public ShiftConfigurationsDTO(int shiftConfigurationId, string shiftConfigurationName, int shiftMinutes, int? weeklyShiftMinutes, int graceMinutes, bool shiftTrackAllowed, bool overtimeAllowed,
                                      int? maximumOvertimeMinutes, int? maximumWeeklyOvertimeMinutes, bool isActive)
            : this()
        {
            log.LogMethodEntry(shiftConfigurationId, shiftConfigurationName, shiftMinutes, weeklyShiftMinutes, graceMinutes, shiftTrackAllowed, overtimeAllowed, maximumOvertimeMinutes, maximumWeeklyOvertimeMinutes, isActive);
            this.shiftConfigurationId = shiftConfigurationId;
            this.shiftConfigurationName = shiftConfigurationName;
            this.shiftMinutes = shiftMinutes;
            this.weeklyShiftMinutes = weeklyShiftMinutes;
            this.graceMinutes = graceMinutes;
            this.shiftTrackAllowed = shiftTrackAllowed;
            this.overtimeAllowed = overtimeAllowed;
            this.maximumOvertimeMinutes = maximumOvertimeMinutes;
            this.maximumWeeklyOvertimeMinutes = maximumWeeklyOvertimeMinutes;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all fields
        /// </summary>
        /// <param name="shiftConfigurationId"></param>
        /// <param name="shiftConfigurationName"></param>
        /// <param name="shiftMinutes"></param>
        /// <param name="weeklyShiftMinutes"></param>
        /// <param name="graceMinutes"></param>
        /// <param name="shiftTrackAllowed"></param>
        /// <param name="overtimeAllowed"></param>
        /// <param name="maximumOvertimeMinutes"></param>
        /// <param name="maximumWeeklyOvertimeMinutes"></param>
        /// <param name="isActive"></param>
        /// <param name="guid"></param>
        /// <param name="siteId"></param>
        /// <param name="synchStatus"></param>
        /// <param name="masterEntityId"></param>
        /// <param name="lastUpdatedDate"></param>
        /// <param name="lastUpdatedBy"></param>
        /// <param name="createdBy"></param>
        /// <param name="creationDate"></param>
        public ShiftConfigurationsDTO(int shiftConfigurationId, string shiftConfigurationName, int shiftMinutes, int? weeklyShiftMinutes, int graceMinutes, bool shiftTrackAllowed, bool overtimeAllowed,
                                      int? maximumOvertimeMinutes, int? maximumWeeklyOvertimeMinutes, bool isActive, string guid, int siteId, bool synchStatus, int masterEntityId,
                                      DateTime lastUpdatedDate, string lastUpdatedBy, string createdBy, DateTime creationDate)
            : this(shiftConfigurationId, shiftConfigurationName, shiftMinutes, weeklyShiftMinutes, graceMinutes, shiftTrackAllowed, overtimeAllowed, maximumOvertimeMinutes, maximumWeeklyOvertimeMinutes, isActive)
        {
            log.LogMethodEntry(shiftConfigurationId, shiftConfigurationName, shiftMinutes, weeklyShiftMinutes, graceMinutes, shiftTrackAllowed, overtimeAllowed, maximumOvertimeMinutes, maximumWeeklyOvertimeMinutes,
                               isActive, guid, siteId, synchStatus, masterEntityId, lastUpdatedDate, lastUpdatedBy, createdBy, creationDate);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Shift Configuration Id field
        /// </summary>
        public int ShiftConfigurationId
        {
            get { return shiftConfigurationId; }
            set { shiftConfigurationId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Shift Configuration Name field
        /// </summary>
        public string ShiftConfigurationName
        {
            get { return shiftConfigurationName; }
            set { shiftConfigurationName = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Shift Minutes field
        /// </summary>
        public int ShiftMinutes
        {
            get { return shiftMinutes; }
            set { shiftMinutes = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Weekly Shift Minutes field
        /// </summary>
        public int? WeeklyShiftMinutes
        {
            get { return weeklyShiftMinutes; }
            set { weeklyShiftMinutes = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Grace Minutes field
        /// </summary>
        public int GraceMinutes
        {
            get { return graceMinutes; }
            set { graceMinutes = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Shift Track Allowed field
        /// </summary>
        public bool ShiftTrackAllowed
        {
            get { return shiftTrackAllowed; }
            set { shiftTrackAllowed = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Overtime Allowed field
        /// </summary>
        public bool OvertimeAllowed
        {
            get { return overtimeAllowed; }
            set { overtimeAllowed = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Maximum Overtime Minutes field
        /// </summary>
        public int? MaximumOvertimeMinutes
        {
            get { return maximumOvertimeMinutes; }
            set { maximumOvertimeMinutes = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Maximum Weekly Overtime Minutes field
        /// </summary>
        public int? MaximumWeeklyOvertimeMinutes
        {
            get { return maximumWeeklyOvertimeMinutes; }
            set { maximumWeeklyOvertimeMinutes = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Is Active field
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Master Entity Id field
        /// </summary>
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Site Id field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }
        /// <summary>
        /// Get/Set method of the Synch Status field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; }
        }
        /// <summary>
        /// Get/Set method of the Last Updated By field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }
        /// <summary>
        ///  Get/Set method of the Last Update Date field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
        }
        /// <summary>
        /// Get/Set method of the Created By field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        /// <summary>
        /// Get/Set method of the Creation Date field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
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
                    return notifyingObjectIsChanged || shiftConfigurationId < 0;
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
            IsChanged = false;
            log.LogMethodExit();
        }
    }
}
