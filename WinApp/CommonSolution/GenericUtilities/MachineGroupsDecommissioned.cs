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
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    public class MachineGroups
    {
        private MachineGroupsDTO machineGroupsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public MachineGroups(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.machineGroupsDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="machineGroupsDTO"></param>
        public MachineGroups(ExecutionContext executionContext, MachineGroupsDTO machineGroupsDTO)
        {
            log.LogMethodEntry(executionContext, machineGroupsDTO);
            this.machineGroupsDTO = machineGroupsDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with id
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="machineGroupId"></param>
        public MachineGroups(ExecutionContext executionContext, int machineGroupId)
        {
            log.LogMethodEntry(machineGroupsDTO);
            this.executionContext = executionContext;
            MachineGroupsDataHandler machineGroupsDataHandler = new MachineGroupsDataHandler();
            this.machineGroupsDTO = machineGroupsDataHandler.GetMachineGroups(machineGroupId);
            log.LogMethodExit(machineGroupsDTO);
        }

        /// <summary>
        /// Saves the machine group
        /// machine group will be inserted if machine group is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();
            MachineGroupsDataHandler machineGroupsDataHandler = new MachineGroupsDataHandler();
            if (machineGroupsDTO.MachineGroupId < 0)
            {
                int machineGroupId = machineGroupsDataHandler.InsertMachineGroups(machineGroupsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                machineGroupsDTO.MachineGroupId = machineGroupId;
            }
            else
            {
                if (machineGroupsDTO.IsChanged == true)
                {
                    machineGroupsDataHandler.UpdateMachineGroups(machineGroupsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    machineGroupsDTO.AcceptChanges();
                }
            }
            if (machineGroupsDTO.MachineGroupMachinesList != null && machineGroupsDTO.MachineGroupMachinesList.Count != 0)
            {
                foreach (MachineGroupMachinesDTO machineGroupMachinesDTO in machineGroupsDTO.MachineGroupMachinesList)
                {
                    if (machineGroupMachinesDTO.IsChanged)
                    {
                        MachineGroupMachines machineGroupMachines = new MachineGroupMachines(executionContext, machineGroupMachinesDTO);
                        machineGroupMachines.Save();
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// get MachineGroupsDTO Object
        /// </summary>
        public MachineGroupsDTO GetMachineGroupsDTO
        {
            get { return machineGroupsDTO; }
        }
        ///// <summary>
        ///// set MachineGroupsDTO Object        
        ///// </summary>
        //public MachineGroupsDTO SetMachineGroupsDTO
        //{
        //    set { machineGroupsDTO = value; }
        //}

    }
    /// <summary>
    /// Manages the list of monitor asset
    /// </summary>
    public class MachineGroupsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<MachineGroupsDTO> machineGroupsDTOList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public MachineGroupsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.machineGroupsDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="machineGroupsDTOList"></param>
        /// <param name="executionContext"></param>
        public MachineGroupsList(ExecutionContext executionContext, List<MachineGroupsDTO> machineGroupsDTOList)
        {
            log.LogMethodEntry(machineGroupsDTOList, executionContext);
            this.machineGroupsDTOList = machineGroupsDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Machine Groups list
        /// </summary>
        public List<MachineGroupsDTO> GetAllMachineGroups(List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            MachineGroupsDataHandler machineGroupsDataHandler = new MachineGroupsDataHandler();
            log.LogMethodExit();
            List<MachineGroupsDTO> machineGroupsDTOList = machineGroupsDataHandler.GetMachineGroupsList(searchParameters);
            log.LogMethodExit(machineGroupsDTOList);
            return machineGroupsDTOList;
        }

        /// <summary>
        /// Returns the Machine Groups list
        /// </summary>
        public List<MachineGroupsDTO> GetAllMachineGroupsDTOList(List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>> searchParameters, bool activeChildRecords = false)
        {
            log.LogMethodEntry(searchParameters);
            MachineGroupsDataHandler machineGroupsDataHandler = new MachineGroupsDataHandler();
            log.LogMethodExit();
            List<MachineGroupsDTO> machineGroupsDTOList = machineGroupsDataHandler.GetMachineGroupsList(searchParameters);
            if (machineGroupsDTOList != null && machineGroupsDTOList.Count != 0)
            {
                MachineGroupMachinesList machineGroupMachinesList = new MachineGroupMachinesList(executionContext);

                foreach (MachineGroupsDTO machineGroupsDTO in machineGroupsDTOList)
                {
                    List<KeyValuePair<MachineGroupMachinesDTO.SearchByParameters, string>> searchByParams = new List<KeyValuePair<MachineGroupMachinesDTO.SearchByParameters, string>>();
                    searchByParams.Add(new KeyValuePair<MachineGroupMachinesDTO.SearchByParameters, string>(MachineGroupMachinesDTO.SearchByParameters.MACHINE_GROUP_ID, machineGroupsDTO.MachineGroupId.ToString()));
                    searchByParams.Add(new KeyValuePair<MachineGroupMachinesDTO.SearchByParameters, string>(MachineGroupMachinesDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                    if (activeChildRecords)
                    {
                        searchByParams.Add(new KeyValuePair<MachineGroupMachinesDTO.SearchByParameters, string>(MachineGroupMachinesDTO.SearchByParameters.ISACTIVE, "1"));
                    }                   
                    List<MachineGroupMachinesDTO> machineGroupMachinesDTOList = machineGroupMachinesList.GetAllMachineGroupMachines(searchByParams);
                    if (machineGroupMachinesDTOList != null)
                    {
                        machineGroupsDTO.MachineGroupMachinesList = new List<MachineGroupMachinesDTO>(machineGroupMachinesDTOList);
                    }
                }
            }
            log.LogMethodExit(machineGroupsDTOList);
            return machineGroupsDTOList;
        }
        /// <summary>
        /// Save or update machine groups for Web Management Studio
        /// </summary>
        public void SaveUpdateMachineGroups()
        {
            try
            {
                log.LogMethodEntry();
                if (machineGroupsDTOList != null)
                {
                    foreach (MachineGroupsDTO machineGroupsDTO in machineGroupsDTOList)
                    {
                        MachineGroups machineGroups = new MachineGroups(executionContext, machineGroupsDTO);
                        machineGroups.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
    }
}