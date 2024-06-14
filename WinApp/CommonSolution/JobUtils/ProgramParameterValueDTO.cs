/********************************************************************************************
 * Project Name - Concurrent Programs Parameter Values DTO
 * Description  - DTO class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *2.120.1       26-Apr-2021       Deeksha             Created as part of AWS Concurrent Programs enhancements
 *2.155.1       13-Aug-2023       Guru S A            Modified for Chile fiscaliation
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.JobUtils
{
    public class ProgramParameterValueDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by Program Parameter Value Id
            /// </summary>
            PROGRAM_PARAMETER_VALUE_ID,
            /// <summary>
            /// Search by Concurrent schedule Id
            /// </summary>
            CONCURRENTPROGRAM_SCHEDULE_ID,
            /// <summary>
            /// Search by Program Id
            /// </summary>
            PROGRAM_ID,
            /// <summary>
            /// Search by Parameter Id
            /// </summary>
            PARAMETER_ID,
            /// <summary>
            /// Search by is active
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by site_id
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by PROGRAM_EXECUTABLE_NAME
            /// </summary>
            PROGRAM_EXECUTABLE_NAME,
            /// <summary>
            /// Search by PARAMETER_NAME
            /// </summary>
            PARAMETER_NAME,
            /// <summary>
            /// Search by REQUEST_IS_IN_WIP
            /// </summary>
            PROGRAM_REQUEST_IS_IN_WIP
        }

        private int programParameterValueId;
        private int programId;
        private int parameterId;
        private string parameterValue;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private bool synchStatus;
        private int siteId;
        private int masterEntityId;
        private int concurrentProgramScheduleId;

        List<ProgramParameterInListValuesDTO> programParameterInListValuesDTOList = new List<ProgramParameterInListValuesDTO>();
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <returns></returns>
        public ProgramParameterValueDTO()
        {
            log.LogMethodEntry();
            programParameterValueId = -1;
            concurrentProgramScheduleId = -1;
            programId = -1;
            parameterId = -1;
            parameterValue = string.Empty;
            isActive = true;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with Required parameter
        /// </summary>
        public ProgramParameterValueDTO(int programParameterValueId, int concurrentProgramScheduleId, int programId, int parameterId, string parameterValue, bool isActive)
            : this()
        {
            log.LogMethodEntry(programParameterValueId, concurrentProgramScheduleId, programId, parameterId,
                                parameterValue, isActive);
            this.programParameterValueId = programParameterValueId;
            this.concurrentProgramScheduleId = concurrentProgramScheduleId;
            this.programId = programId;
            this.parameterValue = parameterValue;
            this.parameterId = parameterId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ProgramParameterValueDTO(int programParameterValueId, int concurrentProgramScheduleId, int programId, int parameterId, string parameterValue, bool isActive, string createdBy, DateTime creationDate,
                                               string lastUpdatedBy, DateTime lastUpdatedDate, string guid, bool synchStatus,
                                               int siteId, int masterEntityId)
            : this(programParameterValueId, concurrentProgramScheduleId, programId, parameterId,
                                parameterValue, isActive)
        {
            log.LogMethodEntry(programParameterValueId, concurrentProgramScheduleId, programId, parameterId,
                                parameterValue, isActive, createdBy, creationDate,
                               lastUpdatedBy, lastUpdatedDate, guid, siteId, masterEntityId, synchStatus);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ProgramParameterValueId field
        /// </summary>
        public int ProgramParameterValueId { get { return programParameterValueId; } set { this.IsChanged = true; programParameterValueId = value; } }

        /// <summary>
        /// Get/Set method of the ConcurrentProgramScheduleId field
        /// </summary>
        public int ConcurrentProgramScheduleId { get { return concurrentProgramScheduleId; } set { this.IsChanged = true; concurrentProgramScheduleId = value; } }

        /// <summary>
        /// Get/Set method of the ProgramId field
        /// </summary>
        public int ProgramId { get { return programId; } set { this.IsChanged = true; programId = value; } }

        /// <summary>
        /// Get/Set method of the ParameterId field
        /// </summary>
        public int ParameterId { get { return parameterId; } set { this.IsChanged = true; parameterId = value; } }

        /// <summary>
        /// Get/Set method of the ParameterValue field
        /// </summary>
        public string ParameterValue { get { return parameterValue; } set { this.IsChanged = true; parameterValue = value; } }

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

        public List<ProgramParameterInListValuesDTO> ProgramParameterInListValuesDTOList { get { return programParameterInListValuesDTOList; } set { programParameterInListValuesDTOList = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || programParameterValueId < 0;
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
        /// Returns whether the AdsDTO changed or any of its AdBroadcastDTO DTO  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (programParameterInListValuesDTOList != null &&
                  programParameterInListValuesDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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