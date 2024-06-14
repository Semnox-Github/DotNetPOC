/********************************************************************************************
 * Project Name - Semnox.Parafait.Game - AllowedMachineNamesBL
 * Description  - AllowedMachineNamesBL Business object
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 ********************************************************************************************* 
 *2.160.0    02-Feb-2023       Roshan devadiga        Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Game
{
    public class AllowedMachineNamesBL
    {
        private AllowedMachineNamesDTO allowedMachineNamesDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        public AllowedMachineNamesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parametrized Constructor 
        /// </summary>
        /// <param name="AllowedMachineNamesDTO"></param>
        /// <param name="executionContext"></param>
        public AllowedMachineNamesBL(ExecutionContext executionContext, AllowedMachineNamesDTO allowedMachineNamesDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(allowedMachineNamesDTO, executionContext);
            this.executionContext = executionContext;
            this.allowedMachineNamesDTO = allowedMachineNamesDTO;
            allowedMachineNamesDTO.MachineName = allowedMachineNamesDTO.MachineName.Trim();
            log.LogMethodExit();
        }
        public AllowedMachineNamesBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)

        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            AllowedMachineNamesDataHandler AllowedMachineNamesDataHandler = new AllowedMachineNamesDataHandler(sqlTransaction);
            allowedMachineNamesDTO = AllowedMachineNamesDataHandler.GetAllowedMachineNames(id);
            if (allowedMachineNamesDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AllowedMachineNamesDTO", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new Exception(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// save and updates the record 
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            AllowedMachineNamesDataHandler AllowedMachineNamesDataHandler = new AllowedMachineNamesDataHandler(sqlTransaction);
            Validation(sqlTransaction);
            if (allowedMachineNamesDTO.AllowedMachineId < 0)
            {
                allowedMachineNamesDTO = AllowedMachineNamesDataHandler.Insert(allowedMachineNamesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                if (!string.IsNullOrEmpty(allowedMachineNamesDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("AllowedMachineNames", allowedMachineNamesDTO.Guid, sqlTransaction);
                }
                allowedMachineNamesDTO.AcceptChanges();
            }
            else
            {
                if (allowedMachineNamesDTO.AllowedMachineId >= 0 && allowedMachineNamesDTO.IsChanged)
                {
                    allowedMachineNamesDTO = AllowedMachineNamesDataHandler.Update(allowedMachineNamesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    if (!string.IsNullOrEmpty(allowedMachineNamesDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("AllowedMachineNames", allowedMachineNamesDTO.Guid, sqlTransaction);
                    }
                    allowedMachineNamesDTO.AcceptChanges();
                    MachineList machineList = new MachineList(executionContext);
                    List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> mSearchParams = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
                    mSearchParams.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.ALLOWED_MACHINE_ID, allowedMachineNamesDTO.AllowedMachineId.ToString()));
                    List<MachineDTO> machineDTOList = machineList.GetMachineList(mSearchParams, false, sqlTransaction);
                    if (machineDTOList != null && machineDTOList.Any())//to update machine name on updating allowed machine name if exists
                    {
                        MachineDTO machineDTO = machineDTOList.FirstOrDefault();
                        if (machineDTO.MachineName != allowedMachineNamesDTO.MachineName)
                        {
                            machineDTO.MachineName = allowedMachineNamesDTO.MachineName;
                            Machine machine = new Machine(machineDTO, executionContext);
                            machine.Save(sqlTransaction);
                        }

                    }
                }
            }
            log.LogMethodExit();
        }

        public AllowedMachineNamesDTO GetAllowedMachineNamesDTO { get { return allowedMachineNamesDTO; } }

        private void Validation(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                if (string.IsNullOrWhiteSpace(allowedMachineNamesDTO.MachineName))
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2798));
                }
                GameDTO gameDTO = null;
                if (allowedMachineNamesDTO.GameId >= 0)
                {
                    Game game = new Game(allowedMachineNamesDTO.GameId, executionContext);
                    gameDTO = game.GetGameDTO;
                }
                if (allowedMachineNamesDTO.GameId < 0 || gameDTO == null)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2196, MessageContainerList.GetMessage(executionContext, "Game"), allowedMachineNamesDTO.GameId));//Unable to find a &1 with id &2
                }
                MachineList machineListBL = new MachineList(executionContext);
                List<KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>(AllowedMachineNamesDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                searchParameters.Add(new KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>(AllowedMachineNamesDTO.SearchByParameters.MACHINE_NAME, allowedMachineNamesDTO.MachineName));
                AllowedMachineNamesDataHandler AllowedMachineNamesDataHandler = new AllowedMachineNamesDataHandler(sqlTransaction);
                List<AllowedMachineNamesDTO> allowedMachineDTOList = AllowedMachineNamesDataHandler.GetAllAllowedMachineNamesDTOList(searchParameters);
                if (allowedMachineDTOList != null && allowedMachineDTOList.Any())
                { 
                    var isAllowedMachineNameExist = allowedMachineDTOList.Find(m => m.MachineName.ToLower() == allowedMachineNamesDTO.MachineName.ToLower() && m.AllowedMachineId != allowedMachineNamesDTO.AllowedMachineId);
                    if (isAllowedMachineNameExist != null)
                    {
                        GameDTO existingGameDTO = null;
                        int gameId = allowedMachineDTOList.Find(m => m.MachineName.ToLower() == allowedMachineNamesDTO.MachineName.ToLower() && m.AllowedMachineId != allowedMachineNamesDTO.AllowedMachineId).GameId;
                        if (allowedMachineNamesDTO.GameId >= 0)
                        {
                            Game game = new Game(gameId, executionContext);
                            existingGameDTO = game.GetGameDTO;
                        }
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 5005, allowedMachineNamesDTO.MachineName, existingGameDTO.GameName));//&1 already exists for game &2. Please enter a unique machine name.
                    }
                }

                List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameter = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
                searchParameter.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                searchParameter.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.ALLOWED_MACHINE_ID, allowedMachineNamesDTO.AllowedMachineId.ToString()));
                
                List<MachineDTO> machineDTOList = machineListBL.GetMachineList(searchParameter, false, sqlTransaction);
                if (machineDTOList != null && machineDTOList.Any() && allowedMachineNamesDTO != null)
                {
                    if (allowedMachineNamesDTO.IsActive == false && machineDTOList.Any(x => x.IsActive.ToLower() == "y" || x.IsActive.ToLower() == "t"))
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 5124, allowedMachineNamesDTO.MachineName);
                        log.Error(errorMessage);
                        throw new ValidationException(errorMessage);

                    }

                }
                SiteContainerDTO siteContainerDTO = SiteContainerList.GetCurrentSiteContainerDTO(executionContext);
                if (siteContainerDTO != null)
                {
                    if (siteContainerDTO.IsMasterSite && allowedMachineNamesDTO.IsActive == false && allowedMachineNamesDTO.MasterEntityId > -1)//case when Inactivating allowedmachine from master site with Isused true in other sites
                    {
                        List<KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>(AllowedMachineNamesDTO.SearchByParameters.IS_ACTIVE, "1"));
                        searchParam.Add(new KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>(AllowedMachineNamesDTO.SearchByParameters.MASTER_ENTITY_ID, allowedMachineNamesDTO.MasterEntityId.ToString()));
                        List<AllowedMachineNamesDTO> allowedMachineNamesDTOList = AllowedMachineNamesDataHandler.GetAllAllowedMachineNamesDTOList(searchParam);
                        allowedMachineNamesDTOList.RemoveAll(x => x.SiteId == siteContainerDTO.SiteId);
                        if (allowedMachineNamesDTOList != null && allowedMachineNamesDTOList.Any())
                        {
                            foreach (AllowedMachineNamesDTO allowedMachineNamesDTOS in allowedMachineNamesDTOList)//used to search machine is active for the Allowedmachine
                            {
                                List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchMachineParam = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
                                searchMachineParam.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.ALLOWED_MACHINE_ID, allowedMachineNamesDTOS.AllowedMachineId.ToString()));
                                MachineDTO machinesDTO = null;
                                MachineList machineList = new MachineList(executionContext);
                                List<MachineDTO> machinesDTOList = machineList.GetMachineList(searchMachineParam, false, sqlTransaction);
                                if (machinesDTOList != null && machinesDTOList.Any())
                                {
                                    machinesDTO = machinesDTOList.FirstOrDefault();
                                    if (machinesDTO != null && (machinesDTO.IsActive.ToLower() == "y" || machinesDTO.IsActive.ToLower() == "t"))
                                    {
                                        List<SiteContainerDTO> siteContainerDTOList = SiteContainerList.GetSiteContainerDTOList();
                                        if (siteContainerDTOList != null && siteContainerDTOList.Any())
                                        {
                                            string siteName = siteContainerDTOList.Where(x => x.SiteId == allowedMachineNamesDTOS.SiteId).FirstOrDefault().SiteName;
                                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 5156, allowedMachineNamesDTOS.MachineName, siteName));//Cannot Inactivate AllowedMachineName &1 as it is Used in the store &2.
                                        }

                                    }
                                }
                            }
                        }
                    }
                }

                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
        }

    }
    public class AllowedMachineNamesListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<AllowedMachineNamesDTO> AllowedMachineNames;
        /// <summary>
        /// Parameterized Constructor 
        /// </summary>
        public AllowedMachineNamesListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="AllowedMachineNames"></param>
        /// <param name="executionContext"></param>
        public AllowedMachineNamesListBL(ExecutionContext executionContext, List<AllowedMachineNamesDTO> AllowedMachineNames)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, AllowedMachineNames);
            this.executionContext = executionContext;
            this.AllowedMachineNames = AllowedMachineNames;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Latest AllowedMachineNamesDTO list with respect to search Parameters
        /// </summary>
        /// <param name="searchParemeters"></param>
        /// <returns></returns>
        public List<AllowedMachineNamesDTO> GetAllowedMachineNamesList(List<KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>> searchParemeters,
                                          SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParemeters);
            AllowedMachineNamesDataHandler AllowedMachineNamesDataHandler = new AllowedMachineNamesDataHandler(sqlTransaction);
            List<AllowedMachineNamesDTO> allowedMachineDTOList = AllowedMachineNamesDataHandler.GetAllAllowedMachineNamesDTOList(searchParemeters);
            log.LogMethodExit(allowedMachineDTOList);
            return allowedMachineDTOList;
        }
        public List<AllowedMachineNamesDTO> GetAllowedMachineNamesListOfGames(List<int> gameIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(gameIdList, activeRecords, sqlTransaction);
            AllowedMachineNamesDataHandler AllowedMachineNamesDataHandler = new AllowedMachineNamesDataHandler(sqlTransaction);
            List<AllowedMachineNamesDTO> allowedMachineDTOList = AllowedMachineNamesDataHandler.GetAllowedMachineNamesDTOListOfGame(gameIdList, activeRecords);
            log.LogMethodExit(allowedMachineDTOList);
            return allowedMachineDTOList;
        }
        /// <summary>
        ///  Save and Update Method For AllowedMachineNames
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            try
            {
                List<AllowedMachineNamesDTO> allowedMachineNamesDTOList = new List<AllowedMachineNamesDTO>();
                log.LogMethodEntry(sqlTransaction);
                if (AllowedMachineNames != null && AllowedMachineNames.Any())
                {
                    foreach (AllowedMachineNamesDTO allowedMachineNamesDTO in AllowedMachineNames)
                    {
                        allowedMachineNamesDTO.MachineName.Trim();
                        AllowedMachineNamesBL allowedMachineNamesBL = new AllowedMachineNamesBL(executionContext, allowedMachineNamesDTO);
                        allowedMachineNamesBL.Save(sqlTransaction);
                        allowedMachineNamesDTOList.Add(allowedMachineNamesBL.GetAllowedMachineNamesDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

    }
}
