/********************************************************************************************
 * Project Name - Generic Utilities
 * Description  - Data object of AccountingCalendarMaster
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0       23-Jul-2020   Deeksha             Created for Recipe Management enhancement.
 *********************************************************************************************/
using System;

namespace Semnox.Core.GenericUtilities
{
    public class AccountingCalendarMasterDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by Accounting Calendar Master Id field
            /// </summary>
            ACCOUNTING_CALENDAR_MASTER_ID,

            /// <summary>
            /// Search by  accounting Calender Date field
            /// </summary>
            ACCOUNTING_CALENDAR_FROM_DATE,

            /// <summary>
            /// Search by  accounting Calender Date field
            /// </summary>
            ACCOUNTING_CALENDAR_TO_DATE,

            /// </summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,
            
            /// <summary>
            /// Search by site id field
            /// </summary>
            SITE_ID,
            
            /// <summary>
            /// Search by Master entity id field
            /// </summary>
            MASTER_ENTITY_ID
            
        }

        private int accountingCalendarMasterId;
        private DateTime accountingCalenderDate;
        private int? month;
        private int? day;
        private int? year;
        private int? quarter;
        private int? weekMonth;
        private int? dayYear;
        private int? weekYear;
        private int? dayWeek;
        private int? dayQtr;
        private int? monthQtr;
        private string accountingCalenderDay;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccountingCalendarMasterDTO()
        {
            log.LogMethodEntry();
            accountingCalendarMasterId = -1;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            quarter = null;
            weekMonth = null;
            dayYear = null;
            weekYear = null;
            month = -1;
            day = -1;
            year = -1;
            accountingCalenderDay = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public AccountingCalendarMasterDTO(int accountingCalendarMasterId, DateTime accountingCalenderDate, int? month,
                                           int? day, int? year, int? quarter, int? weekMonth, int? dayYear, int? weekYear,
                                           int? dayWeek , int? dayQtr, int? monthQtr,
                                           string accountingCalenderDay, bool isActive)
            : this()
        {
            log.LogMethodEntry(accountingCalendarMasterId, accountingCalenderDate, month, day, year, quarter, weekMonth, dayYear, weekYear,
                                dayWeek, dayQtr, monthQtr ,accountingCalenderDay, isActive);
            this.accountingCalendarMasterId = accountingCalendarMasterId;
            this.accountingCalenderDate = accountingCalenderDate;
            this.month = month;
            this.day = day;
            this.year = year;
            this.quarter = quarter;
            this.weekMonth = weekMonth;
            this.dayYear = dayYear;
            this.weekYear = weekYear;
            this.dayWeek = dayWeek;
            this.dayQtr = dayQtr;
            this.monthQtr = monthQtr;
            this.accountingCalenderDay = accountingCalenderDay;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AccountingCalendarMasterDTO(int accountingCalendarMasterId, DateTime accountingCalenderDate, int? month,
                                           int? day, int? year, int? quarter, int? weekMonth, int? dayYear, int? weekYear,
                                           int? dayWeek, int? dayQtr, int? monthQtr,
                                           string accountingCalenderDay, bool isActive, string createdBy, DateTime creationDate,
                                           string lastUpdatedBy, DateTime lastUpdateDate, string guid, int siteId,
                                           bool synchStatus, int masterEntityId)
            : this(accountingCalendarMasterId, accountingCalenderDate, month, day, year, quarter, weekMonth, dayYear, weekYear,
                   dayWeek , dayQtr, monthQtr, accountingCalenderDay, isActive)
        {
            log.LogMethodEntry(accountingCalendarMasterId, accountingCalenderDate, month, day, year, quarter, weekMonth, dayYear,
                                weekYear, accountingCalenderDay, isActive, createdBy, creationDate, lastUpdatedBy, lastUpdateDate,
                                guid, siteId, synchStatus, masterEntityId);
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
        /// Get/Set method of the AccountingCalendarMasterId field
        /// </summary>
        public int AccountingCalendarMasterId { get { return accountingCalendarMasterId; } set { this.IsChanged = true; accountingCalendarMasterId = value; } }


        /// <summary>
        /// Get/Set method of the accountingCalenderDate field
        /// </summary>
        public DateTime AccountingCalenderDate { get { return accountingCalenderDate; } set { this.IsChanged = true; accountingCalenderDate = value; } }


        /// <summary>
        /// Get/Set method of the month field
        /// </summary>
        public int? Month { get { return month; } set { this.IsChanged = true; month = value; } }


        /// <summary>
        /// Get/Set method of the day field
        /// </summary>
        public int? Day { get { return day; } set { this.IsChanged = true; day = value; } }


        /// <summary>
        /// Get/Set method of the year field
        /// </summary>
        public int? Year { get { return year; } set { this.IsChanged = true; year = value; } }

        /// <summary>
        /// Get/Set method of the quarter field
        /// </summary>
        public int? Quarter { get { return quarter; } set { this.IsChanged = true; quarter = value; } }

        /// <summary>
        /// Get/Set method of the WeekMonth field
        /// </summary>
        public int? WeekMonth { get { return weekMonth; } set { this.IsChanged = true; weekMonth = value; } }

        /// <summary>
        /// Get/Set method of the DayYear field
        /// </summary>
        public int? DayYear { get { return dayYear; } set { this.IsChanged = true; dayYear = value; } }

        /// <summary>
        /// Get/Set method of the WeekYear field
        /// </summary>
        public int? WeekYear { get { return weekYear; } set { this.IsChanged = true; weekYear = value; } }


        /// <summary>
        /// Get/Set method of the WeekYear field
        /// </summary>
        public int? DayWeek { get { return dayWeek; } set { this.IsChanged = true; dayWeek = value; } }


        /// <summary>
        /// Get/Set method of the WeekYear field
        /// </summary>
        public int? DayQtr { get { return dayQtr; } set { this.IsChanged = true; dayQtr = value; } }


        /// <summary>
        /// Get/Set method of the WeekYear field
        /// </summary>
        public int? MonthQtr { get { return monthQtr; } set { this.IsChanged = true; monthQtr = value; } }

        /// <summary>
        /// Get/Set method of the accountingCalenderDay field
        /// </summary>
        public string AccountingCalenderDay { get { return accountingCalenderDay; } set { this.IsChanged = true; accountingCalenderDay = value; } }


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
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

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
                    return notifyingObjectIsChanged || accountingCalendarMasterId < 0;
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
