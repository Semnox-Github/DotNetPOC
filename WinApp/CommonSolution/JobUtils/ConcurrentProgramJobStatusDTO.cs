/********************************************************************************************
 * Project Name - Concurrent Program Job status
 * Description  - Data object of the concurrent Programs Arguments
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        15-Mar-2017    Amaresh          Created
 *2.70.2      24-Jul-2019    Dakshakh raj     Modified 
 *2.120.1     09-Jun-2021    Deeksha          Modified: As part of AWS concurrent program enhancements
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// class of ConcurrentProgramJobStatusDTO
    /// </summary>
    public class ConcurrentProgramJobStatusDTO : IChangeTracking
    {
         private Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByConcurrentProgramJobStatusParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByConcurrentProgramJobStatusParameters
        {
            /// <summary>
            /// Search by ProgramId field
            /// </summary>
            PROGRAM_ID,

            /// <summary>
            /// Search by Phase field
            /// </summary>
            PHASE,

            /// <summary>
            /// Search by Status field
            /// </summary>
            STATUS,

            /// <summary>
            /// Search by Program Name field
            /// </summary>
            PROGRAM_NAME,

            /// <summary>
            /// Search by StartTime field
            /// </summary>
            START_TIME,

            /// <summary>
            /// Search by StartTime field
            /// </summary>
            END_TIME,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID
        }


        private int programId;
        private int requestedId;
        private string programName;
        private string executableName;
        private DateTime? startTime = null;
        private DateTime? endTime = null;
        private string phase;
        private string status;
        private string errorNotificationMailId;
        private string successNotificationMailId;
        private string arguments;
        private string errorCount;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ConcurrentProgramJobStatusDTO()
        {
            log.LogMethodEntry();
            programId = -1;
            requestedId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ConcurrentProgramJobStatusDTO(int programId, int requestedId, string programName, string executableName, DateTime startTime, DateTime endTime, string phase, 
                                            string status, string errorNotificationMailId, string arguments, string successNotificationMailId, string errorCount)
        {
            log.LogMethodEntry();
            this.programId = programId;
            this.requestedId = requestedId;
            this.programName = programName;
            this.executableName = executableName;
            this.startTime = startTime;

            if (startTime == DateTime.MinValue)
            {
                this.startTime = null;
            }
            else
            {
                this.startTime = startTime;
            }

            if(endTime == DateTime.MinValue)
            {
                this.endTime = null;
            }
            else
            {
                this.endTime = endTime;
            }
           
            this.phase = phase;
            this.status = status;
            this.errorNotificationMailId = errorNotificationMailId;
            this.successNotificationMailId = successNotificationMailId;
            this.arguments = arguments;
            this.errorCount = errorCount;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ProgramId field
        /// </summary>
        [Browsable(false)]
        [DisplayName("Program Id")]
        public int ProgramId { get { return programId; } set { programId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RequestedId field
        /// </summary>
        [DisplayName("Argument Id")]
        public int RequestedId { get { return requestedId; } set { requestedId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ProgramName field
        /// </summary>
        [DisplayName("Program Name")]
        public string ProgramName { get { return programName; } set { programName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ExecutableName field
        /// </summary>
        [DisplayName("Executable Name")]
        public string ExecutableName { get { return executableName; } set { executableName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the StartTime field
        /// </summary>
        [DisplayName("StartTime")]
        public DateTime ? StartTime { get { return startTime; } set { startTime = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the EndTime field
        /// </summary>
        [DisplayName("EndTime")]
        public DateTime ? EndTime { get { return endTime; } set { endTime = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Phase field
        /// </summary>
        [DisplayName("Phase")]
        public string Phase { get { return phase; } set { phase = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        [DisplayName("Status")]
        public string Status { get { return status; } set { status = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ErrorNotificationMailId field
        /// </summary>
        [DisplayName("Error Notification Email Id")]
        public string ErrorNotificationMailId { get { return errorNotificationMailId; } set { errorNotificationMailId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ErrorNotificationMailId field
        /// </summary>
        [DisplayName("Success Notification Email Id")]
        public string SuccessNotificationMailId { get { return successNotificationMailId; } set { successNotificationMailId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Arguments field
        /// </summary>
        [DisplayName("Arguments")]
        public string Arguments { get { return arguments; } set { arguments = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ErrorCount field
        /// </summary>
        [DisplayName("Error Count")]
        public string ErrorCount { get { return errorCount; } set { errorCount = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || programId < 0;
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


