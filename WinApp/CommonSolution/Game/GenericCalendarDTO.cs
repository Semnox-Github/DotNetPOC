/********************************************************************************************
 * Project Name - Generic Calendar DTO                                                                         
 * Description  - DTO of the GenericCalendar class
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.40        06-Oct-2018    Jagan Mohana Rao      Created new clas to have properties of Generic Calendar.
 *2.50.0      12-dec-2018    Guru S A              Who column changes
 *2.70.2        26-Jul-2019    Deeksha               Created a new constructor with required fields.
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Game
{
    public class GenericCalendarDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        public enum SearchByGenericCalendarParameters
        {           
            /// <summary>
            /// SITE ID search field
            /// </summary>
            SITE_ID ,
            /// <summary>
            /// IS ACTIVE search field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// MASTER ENTITY ID search field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// CALENDAR_ID search field
            /// </summary>
            CALENDAR_ID
        }


        private int calendarId;
        private string calendarType;
        private int entityId;
        private string entityName;
        private int machineGroupId;
        private int day;
        private DateTime? date;
        private string fromTime;
        private string toTime;
        private string value1;
        private string value2;
        private string value3;
        private string value4;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private bool isActive;
        private string entity;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastupdateDate;
        private bool enabledOutOfService;
        private int themeId;

        /// <summary>
        /// Basic constructor for GenericCalender DTO
        /// </summary>
        public GenericCalendarDTO()
        {
            log.LogMethodEntry();
            calendarId = -1;
            entityId = -1;
            machineGroupId = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true; 
            log.LogMethodExit();

        }
    
        /// <summary>
        /// Constructor with required table fields.
        /// </summary>
        public GenericCalendarDTO(int calendarId, string calendarType, int entityId, string entityName, int machineGroupId, int day,
                                 DateTime? date, string fromTime, string toTime, string value1, string value2, string value3,
                                 string value4, bool isActive,  bool enabledOutOfService,int themeId, string entity = "")
            :this()
        {
            log.LogMethodEntry(calendarId, calendarType,  entityId,  entityName,  machineGroupId,  day,  date,  fromTime,
                                toTime,  value1,  value2, value3,  value4, isActive, enabledOutOfService, themeId, entity);
            this.calendarId = calendarId;
            this.calendarType = calendarType;
            this.entityId = entityId;
            this.entityName = entityName;
            this.machineGroupId = machineGroupId;
            this.day = day;
            this.date = date;
            this.fromTime = fromTime;
            this.toTime = toTime;
            this.value1 = value1;
            this.value2 = value2;
            this.value3 = value3;
            this.value4 = value4;
            this.isActive = isActive;
            this.entity = entity;           
            this.enabledOutOfService = enabledOutOfService;
            this.themeId = themeId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the table fields.
        /// </summary>
        public GenericCalendarDTO(int calendarId, string calendarType, int entityId, string entityName, int machineGroupId,
                                    int day, DateTime? date, string fromTime, string toTime, string value1, string value2,
                                    string value3, string value4, string guid, int siteId, bool synchStatus, int masterEntityId, 
                                    bool isActive, bool enabledOutOfService, int themeId, string createdBy, DateTime creationDate,
                                    string lastUpdatedBy, DateTime lastupdateDate, string entity = "")
            :this(calendarId, calendarType, entityId, entityName, machineGroupId, day, date, fromTime,
                                toTime, value1, value2, value3, value4, isActive, enabledOutOfService, themeId, entity)
        {
            log.LogMethodEntry(calendarId, calendarType, entityId, entityName, machineGroupId, day, date, fromTime,
                toTime, value1, value2, value3, value4, guid, siteId, synchStatus, masterEntityId, isActive, 
                enabledOutOfService, createdBy, creationDate, lastUpdatedBy, lastupdateDate, entity);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastupdateDate = lastupdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set for CalendarId 
        /// </summary>
        public int CalendarId { get { return calendarId; } set { calendarId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for  CalendarType
        /// </summary>
        public string CalendarType { get { return calendarType; } set { calendarType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for MachineId 
        /// </summary>
        public int EntityId { get { return entityId; } set { entityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for GameProfileId 
        /// </summary>
        public string EntityName { get { return entityName; } set { entityName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for MachineGroupId 
        /// </summary>
        public int MachineGroupId { get { return machineGroupId; } set { machineGroupId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for Day 
        /// </summary>
        public int Day { get { return day; } set { day = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for  Date
        /// </summary>
        public DateTime? Date { get { return date; } set { date = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for FromTime
        /// </summary>
        public string FromTime { get { return fromTime; } set { fromTime = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for ToTime
        /// </summary>
        public string ToTime { get { return toTime; } set { toTime = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for Value1
        /// </summary>
        public string Value1 { get { return value1; } set { value1 = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for Value2
        /// </summary>
        public string Value2 { get { return value2; } set { value2 = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for Value3
        /// </summary>
        public string Value3 { get { return value3; } set { value3 = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for Value4
        /// </summary>
        public string Value4 { get { return value4; } set { value4 = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for Guid
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for site_id 
        /// </summary>
        public int site_id { get { return siteId; } set { siteId = value;  } }

        /// <summary>
        /// Get/Set for SynchStatus 
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }

        /// <summary>
        /// Get/Set for MasterEntityId 
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the EnabledOutOfService field
        /// </summary>
        public bool EnabledOutOfService { get { return enabledOutOfService; } set { enabledOutOfService = value; } }

        /// <summary>
        /// Get/Set for ThemeId 
        /// </summary>
        public int ThemeId { get { return themeId; } set { themeId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for  Entity
        /// </summary>
        public string Entity { get { return entity; } set { entity = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for  createdBy
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for  creationDate
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        /// <summary>
        /// Get/Set for  lastUpdatedBy
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for  lastupdateDate
        /// </summary>
        public DateTime LastupdateDate { get { return lastupdateDate; } set { lastupdateDate = value; } }

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
                    return notifyingObjectIsChanged || calendarId < 0;
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
