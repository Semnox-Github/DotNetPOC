/********************************************************************************************
 * Project Name - Machine Groups
 * Description  - Bussiness logic of machine groups
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        13-May-2019   Jagan Mohana Rao    Created
 *2.70.2        31-Jul-2019   Deeksha             Modified:Save method returns DTO.
 *2.90        03-Jun-2020    Mushahid Faizan          Modified: 3 tier changes for Rest API.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Game
{
    public class MachineGroups
    {
        private MachineGroupsDTO machineGroupsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private MachineGroups(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="machineGroupsDTO">machineGroupsDTO</param>
        public MachineGroups(ExecutionContext executionContext, MachineGroupsDTO machineGroupsDTO)
        {
            log.LogMethodEntry(executionContext, machineGroupsDTO);
            this.machineGroupsDTO = machineGroupsDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="machineGroupId">Parameter of the type machineGroupId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public MachineGroups(ExecutionContext executionContext, int machineGroupId, bool loadChildRecords = true,
           bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, machineGroupsDTO, sqlTransaction);
            MachineGroupsDataHandler machineGroupsDataHandler = new MachineGroupsDataHandler(sqlTransaction);
            machineGroupsDTO = machineGroupsDataHandler.GetMachineGroups(machineGroupId);
            if (machineGroupsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "MachineGroups", machineGroupId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(machineGroupsDTO);
        }

        /// <summary>
        /// Builds the child records for MachineGroups object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction"></param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            MachineGroupMachinesList machineGroupMachinesList = new MachineGroupMachinesList(executionContext);
            List<KeyValuePair<MachineGroupMachinesDTO.SearchByParameters, string>> searchByParams = new List<KeyValuePair<MachineGroupMachinesDTO.SearchByParameters, string>>();
            searchByParams.Add(new KeyValuePair<MachineGroupMachinesDTO.SearchByParameters, string>(MachineGroupMachinesDTO.SearchByParameters.MACHINE_GROUP_ID, machineGroupsDTO.MachineGroupId.ToString()));
            if (activeChildRecords)
            {
                searchByParams.Add(new KeyValuePair<MachineGroupMachinesDTO.SearchByParameters, string>(MachineGroupMachinesDTO.SearchByParameters.ISACTIVE, "1"));
            }
            machineGroupsDTO.MachineGroupMachinesList = machineGroupMachinesList.GetAllMachineGroupMachines(searchByParams, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the machine group
        /// machine group will be inserted if machine group is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            MachineGroupsDataHandler machineGroupsDataHandler = new MachineGroupsDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (machineGroupsDTO.IsActive)
            {
                if (machineGroupsDTO.MachineGroupId <= 0)
                {
                    machineGroupsDTO = machineGroupsDataHandler.InsertMachineGroups(machineGroupsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    machineGroupsDTO.AcceptChanges();
                }
                else if (machineGroupsDTO.IsChanged)
                {
                    machineGroupsDTO = machineGroupsDataHandler.UpdateMachineGroups(machineGroupsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    machineGroupsDTO.AcceptChanges();
                }
                SaveMachineGroupMachine(sqlTransaction);
            }
            else
            {
                if (machineGroupsDTO.MachineGroupMachinesList != null && machineGroupsDTO.MachineGroupMachinesList.Any(x => x.IsActive))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new ForeignKeyException(message);
                }
                SaveMachineGroupMachine(sqlTransaction);
                if (machineGroupsDTO.MachineGroupId >= 0)
                {
                    machineGroupsDataHandler.Delete(machineGroupsDTO);
                }
                machineGroupsDTO.AcceptChanges();
            }

            log.LogMethodExit();
        }


        private void SaveMachineGroupMachine(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (machineGroupsDTO.MachineGroupMachinesList != null &&
                machineGroupsDTO.MachineGroupMachinesList.Any())
            {
                List<MachineGroupMachinesDTO> updatedMachineGroupMachinesDTOList = new List<MachineGroupMachinesDTO>();
                foreach (var machineGroupMachinesDTO in machineGroupsDTO.MachineGroupMachinesList)
                {
                    if (machineGroupMachinesDTO.MachineGroupId != machineGroupsDTO.MachineGroupId)
                    {
                        machineGroupMachinesDTO.MachineGroupId = machineGroupsDTO.MachineGroupId;
                    }
                    if (machineGroupMachinesDTO.IsChanged)
                    {
                        updatedMachineGroupMachinesDTOList.Add(machineGroupMachinesDTO);
                    }
                }
                if (updatedMachineGroupMachinesDTOList.Any())
                {
                    log.LogVariableState("UpdatedMachineGroupMachinesDTOList", updatedMachineGroupMachinesDTOList);
                    MachineGroupMachinesList machineGroupMachinesList = new MachineGroupMachinesList(executionContext, updatedMachineGroupMachinesDTOList);
                    machineGroupMachinesList.Save(sqlTransaction);
                }
            }
        }

        /// <summary>
        /// Validates the MachineGroupsDTOList and MachineGroupMachinesDTOList- child 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ValidationError validationError = null;

            // Validation Logic here
            if (string.IsNullOrWhiteSpace(machineGroupsDTO.GroupName))
            {
                validationErrorList.Add(new ValidationError("MachineGroup", "GroupName", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Group Name"))));
            }

            if (!string.IsNullOrWhiteSpace(machineGroupsDTO.GroupName) && machineGroupsDTO.GroupName.Length > 50)
            {
                validationErrorList.Add(new ValidationError("MachineGroup", "GroupName", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Group Name"), 50)));
            }

            // validate Child list
            if (machineGroupsDTO.MachineGroupMachinesList != null &&
                machineGroupsDTO.MachineGroupMachinesList.Count > 0)
            {
                foreach (MachineGroupMachinesDTO machineGroupMachinesDTO in machineGroupsDTO.MachineGroupMachinesList)
                {
                    MachineGroupMachines machineGroupMachines = new MachineGroupMachines(executionContext, machineGroupMachinesDTO);
                    validationErrorList.AddRange(machineGroupMachines.Validate());
                }
            }
            return validationErrorList;
        }
        /// <summary>
        /// get MachineGroupsDTO Object
        /// </summary>
        public MachineGroupsDTO GetMachineGroupsDTO
        {
            get { return machineGroupsDTO; }
        }
        /// <summary>
        /// set MachineGroupsDTO Object        
        /// </summary>
        public MachineGroupsDTO SetMachineGroupsDTO
        {
            set { machineGroupsDTO = value; }
        }

    }

    /// <summary>
    /// Manages the list of MachineGroups
    /// </summary>
    public class MachineGroupsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<MachineGroupsDTO> machineGroupsDTOList = new List<MachineGroupsDTO>();
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public MachineGroupsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="machineGroupsDTOList">machineGroupsDTOList</param>
        /// <param name="executionContext">executionContext</param>
        public MachineGroupsList(ExecutionContext executionContext, List<MachineGroupsDTO> machineGroupsDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, machineGroupsDTOList);
            this.machineGroupsDTOList = machineGroupsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Machine Groups list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>machineGroupsDTOList</returns>
        public List<MachineGroupsDTO> GetAllMachineGroups(List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            MachineGroupsDataHandler machineGroupsDataHandler = new MachineGroupsDataHandler(sqlTransaction);
            List<MachineGroupsDTO> machineGroupsDTOList = machineGroupsDataHandler.GetMachineGroupsList(searchParameters);
            log.LogMethodExit(machineGroupsDTOList);
            return machineGroupsDTOList;
        }

        /// <summary>
        /// Returns the Machine Groups list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="loadChildRecords">loadChildRecords</param>
        /// <param name="activeChildRecords">activeChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public List<MachineGroupsDTO> GetAllMachineGroupsDTOList(List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false,
                                         bool activeChildRecords = false, SqlTransaction sqlTransaction = null,
                                         int currentPage = 0, int pageSize = 0)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            MachineGroupsDataHandler machineGroupsDataHandler = new MachineGroupsDataHandler(sqlTransaction);
            log.LogMethodExit();
            List<MachineGroupsDTO> machineGroupsDTOList = machineGroupsDataHandler.GetMachineGroupsList(searchParameters, currentPage,pageSize);
            if (machineGroupsDTOList != null && machineGroupsDTOList.Any() && loadChildRecords)
            {
                Build(machineGroupsDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(machineGroupsDTOList);
            return machineGroupsDTOList;
        }

        /// <summary>
        /// Builds the List of MachineGroups object based on the list of MachineGroup id.
        /// </summary>
        /// <param name="machineGroupsDTOList"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        private void Build(List<MachineGroupsDTO> machineGroupsDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(machineGroupsDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, MachineGroupsDTO> machineGroupsIdMachineGroupDictionary = new Dictionary<int, MachineGroupsDTO>();
            StringBuilder sb = new StringBuilder(string.Empty);
            string machineGroupIdSet;
            for (int i = 0; i < machineGroupsDTOList.Count; i++)
            {
                if (machineGroupsDTOList[i].MachineGroupId == -1 ||
                    machineGroupsIdMachineGroupDictionary.ContainsKey(machineGroupsDTOList[i].MachineGroupId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(machineGroupsDTOList[i].MachineGroupId);
                machineGroupsIdMachineGroupDictionary.Add(machineGroupsDTOList[i].MachineGroupId, machineGroupsDTOList[i]);
            }

            machineGroupIdSet = sb.ToString();

            // Build child Records - MachineGroupMachine
            MachineGroupMachinesList machineGroupMachinesList = new MachineGroupMachinesList(executionContext);
            List<KeyValuePair<MachineGroupMachinesDTO.SearchByParameters, string>> searchMachineGroupMachinesParams = new List<KeyValuePair<MachineGroupMachinesDTO.SearchByParameters, string>>();
            searchMachineGroupMachinesParams.Add(new KeyValuePair<MachineGroupMachinesDTO.SearchByParameters, string>(MachineGroupMachinesDTO.SearchByParameters.MACHINE_GROUP_ID_LIST, machineGroupIdSet.ToString()));
            if (activeChildRecords)
            {
                searchMachineGroupMachinesParams.Add(new KeyValuePair<MachineGroupMachinesDTO.SearchByParameters, string>(MachineGroupMachinesDTO.SearchByParameters.ISACTIVE, "1"));
            }
            List<MachineGroupMachinesDTO> machineGroupMachinesDTOList = machineGroupMachinesList.GetAllMachineGroupMachines(searchMachineGroupMachinesParams, sqlTransaction);
            if (machineGroupMachinesDTOList != null && machineGroupMachinesDTOList.Any())
            {
                log.LogVariableState("MachineGroupMachinesDTOList", machineGroupMachinesDTOList);
                foreach (var machineGroupMachinesDTO in machineGroupMachinesDTOList)
                {
                    if (machineGroupsIdMachineGroupDictionary.ContainsKey(machineGroupMachinesDTO.MachineGroupId))
                    {
                        if (machineGroupsIdMachineGroupDictionary[machineGroupMachinesDTO.MachineGroupId].MachineGroupMachinesList == null)
                        {
                            machineGroupsIdMachineGroupDictionary[machineGroupMachinesDTO.MachineGroupId].MachineGroupMachinesList = new List<MachineGroupMachinesDTO>();
                        }
                        machineGroupsIdMachineGroupDictionary[machineGroupMachinesDTO.MachineGroupId].MachineGroupMachinesList.Add(machineGroupMachinesDTO);
                    }
                }
            }
            log.LogMethodExit();
        }
        public int GetMachineGroupsCount(List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            MachineGroupsDataHandler machineGroupsDataHandler = new MachineGroupsDataHandler(sqlTransaction);
            int count = machineGroupsDataHandler.GetMachineGroupsCount(searchParameters);
            log.LogMethodExit(count);
            return count;
        }

        //public List<MachineGroupsDTO> GetMachineGroupsDTOList(List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false, bool activeChildRecords = false, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        //{
        //    log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
        //    MachineGroupsDataHandler machineGroupsDataHandler = new MachineGroupsDataHandler(sqlTransaction);
        //    log.LogMethodExit();
        //    List<MachineGroupsDTO> machineGroupsDTOList = machineGroupsDataHandler.GetMachineGroups(searchParameters, currentPage, pageSize);
        //    if (machineGroupsDTOList != null && machineGroupsDTOList.Any() && loadChildRecords)
        //    {
        //        Build(machineGroupsDTOList, activeChildRecords, sqlTransaction);
        //    }
        //    log.LogMethodExit(machineGroupsDTOList);
        //    return machineGroupsDTOList;
        //}
        /// <summary>
        /// Save or update machine groups for Web Management Studio
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (machineGroupsDTOList == null ||
                machineGroupsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < machineGroupsDTOList.Count; i++)
            {
                var machineGroupsDTO = machineGroupsDTOList[i];
                if (machineGroupsDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    MachineGroups machineGroups = new MachineGroups(executionContext, machineGroupsDTO);
                    machineGroups.Save(sqlTransaction);
                }
                catch (SqlException sqlEx)
                {
                    log.Error(sqlEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                    if (sqlEx.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (ValidationException valEx)
                {
                    log.Error(valEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving MachineGroupsDTO", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("MachineGroupsDTO", machineGroupsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}