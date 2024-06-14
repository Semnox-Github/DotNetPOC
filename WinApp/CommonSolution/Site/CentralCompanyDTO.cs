/********************************************************************************************
 * Project Name - Site
 * Description  - DTO of CentralCompany
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By           Remarks          
 *********************************************************************************************
 *2.120.1     26-Apr-2021   Deeksha               Created as part of AWS Job Scheduler enhancements
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Site
{
    public class CentralCompanyDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchBySiteParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by CentralCompany ID field
            /// </summary>
            CENTRAL_COMPANY_ID,
            /// <summary>
            /// Search by CentralCompany NAME field
            /// </summary>
            CENTRAL_COMPANY_NAME,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE
        }

        private int centralCompanyId;
        private string centralCompanyName;
        private string loginKey;
        private string version;
        private string dbName;
        private string timezone;
        private int offset;
        private decimal? buisnessDayStartTime;
        private string active;
        private string applicationFolderPath;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CentralCompanyDTO()
        {
            log.LogMethodEntry();
            centralCompanyId = -1;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CentralCompanyDTO(int centralCompanyId, string centralCompanyName, string loginKey, string version, string dbName,
                                 string timestamp, int offset, decimal? buisnessDayStartTime, string active , string applicationFolderPath)
        {
            log.LogMethodEntry(centralCompanyName, centralCompanyName, loginKey, version, dbName, timestamp, offset, buisnessDayStartTime, active, applicationFolderPath);
            this.centralCompanyId = centralCompanyId;
            this.centralCompanyName = centralCompanyName;
            this.loginKey = loginKey;
            this.version = version;
            this.dbName = dbName;
            this.timezone = timestamp;
            this.offset = offset;
            this.buisnessDayStartTime = buisnessDayStartTime;
            this.active = active;
            this.applicationFolderPath = applicationFolderPath;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the CentralCompanyId field
        /// </summary>
        public int CentralCompanyId { get { return centralCompanyId; } set { centralCompanyId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CentralCompanyName field
        /// </summary>
        public string CentralCompanyName { get { return centralCompanyName; } set { centralCompanyName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LoginKey field
        /// </summary>
        public string LoginKey { get { return loginKey; } set { loginKey = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Version field
        /// </summary>
        public string Version { get { return version; } set { version = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DBName field
        /// </summary>
        public string DBName { get { return dbName; } set { dbName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TimeStamp field
        /// </summary>
        public string TimeZone { get { return timezone; } set { timezone = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Offset field
        /// </summary>
        public int Offset { get { return offset; } set { offset = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the BuisnessDayStartTime field
        /// </summary>
        public decimal? BuisnessDayStartTime { get { return buisnessDayStartTime; } set { buisnessDayStartTime = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        public string Active { get { return active; } set { active = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ApplicationStartUpPath field
        /// </summary>
        public string ApplicationFolderPath { get { return applicationFolderPath; } set { applicationFolderPath = value; this.IsChanged = true; } }


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
                    return notifyingObjectIsChanged || centralCompanyId < 0;
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
