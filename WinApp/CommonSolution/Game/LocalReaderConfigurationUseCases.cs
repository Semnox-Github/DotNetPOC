/********************************************************************************************
 * Project Name - Game
 * Description  - LocalReaderConfigurationUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By                Remarks          
 *********************************************************************************************
 2.110.0      15-Dec-2020      Prajwal S                  Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game
{
    class LocalReaderConfigurationUseCases : IReaderConfigurationUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalReaderConfigurationUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<MachineAttributeDTO>> GetMachineAttributes(List<KeyValuePair<string, string>> searchParameters, SqlTransaction sqlTransaction = null)

        {
            return await Task<List<MachineAttributeDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                string moduleName = string.Empty;
                int moduleRowId = -1;
                if (searchParameters != null && searchParameters.Exists(x => x.Key == "moduleName"))
                {
                    moduleName = searchParameters.Find(x => x.Key == "moduleName").Value.ToString();
                    moduleRowId = Convert.ToInt32(searchParameters.Find(x => x.Key == "moduleRowId").Value);
                }

                GameSystem gameSystem = new GameSystem(moduleName, moduleRowId, executionContext);
                List<MachineAttributeDTO> machineAttributeDTOList = gameSystem.GetMachineAttributes();

                log.LogMethodExit(machineAttributeDTOList);
                return machineAttributeDTOList;
            });
        }

        public async Task<string> SaveMachineAttributes(List<MachineAttributeDTO> machineAttributesDTOList, string moduleName, string moduleId)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = "Falied";
                log.LogMethodEntry(machineAttributesDTOList);
                if (machineAttributesDTOList == null)
                {
                    throw new ValidationException("GameDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {

                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        GameSystemList machineAttributeList = new GameSystemList(executionContext, machineAttributesDTOList, moduleName, Convert.ToInt32(moduleId));
                        machineAttributeList.SaveGameSystemList();
                        parafaitDBTrx.EndTransaction();
                    }
                    catch (ValidationException valEx)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(valEx);
                        throw valEx;
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw ex;
                    }

                }
                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }


        public async Task<ReaderConfigurationContainerDTOCollection> GetMachineAttributeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<ReaderConfigurationContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    ReaderConfigurationContainerList.Rebuild(siteId);
                }
                List<ReaderConfigurationContainerDTO> readerConfigurationContainerDTOList = ReaderConfigurationContainerList.GetReaderConfigurationContainerDTOList(siteId);
                ReaderConfigurationContainerDTOCollection result = new ReaderConfigurationContainerDTOCollection(readerConfigurationContainerDTOList);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<MachineAttributeDTO> DeleteMachineAttributes(MachineAttributeDTO machineAttributeDTO, string entityName, string entityId)
        {
            return await Task<MachineAttributeDTO>.Factory.StartNew(() =>
            {
                MachineAttributeDTO result = null;
                try
                {
                    log.LogMethodEntry(machineAttributeDTO);
                    if(machineAttributeDTO == null || string.IsNullOrWhiteSpace(entityName) ||  string.IsNullOrWhiteSpace(entityId))
                    {
                        throw new ValidationException("entityName or entityId is null or Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            MachineAttributeDTO machineAttribute = new MachineAttributeDTO();
                            if (machineAttributeDTO != null)
                            {
                                GameSystem gameSystem = new GameSystem(executionContext, parafaitDBTrx.SQLTrx);
                                MachineAttributeDTO.MachineAttribute enumDisplayStatus = (MachineAttributeDTO.MachineAttribute)machineAttributeDTO.AttributeName;
                                string attributeName = enumDisplayStatus.ToString();
                                string attributeEntityName = string.Empty;
                                if (entityName.ToUpper().ToString() == "GAMES")
                                {
                                    attributeEntityName = "GAME";
                                }
                                else if (entityName.ToUpper().ToString() == "GAME_PROFILE")
                                {
                                    attributeEntityName = "GAME_PROFILE";
                                }
                                else if (entityName.ToUpper().ToString() == "MACHINES")
                                {
                                    attributeEntityName = "MACHINE";
                                }
                                else
                                {
                                    attributeEntityName = "SYSTEM";
                                }
                                MachineAttributeDTO.AttributeContext contextOfAttribute = (MachineAttributeDTO.AttributeContext)Enum.Parse(typeof(MachineAttributeDTO.AttributeContext), attributeEntityName, true);
                                if (entityName.ToUpper().ToString() != "SYSTEM")
                                {
                                    gameSystem.DeleteMachineAttribute(machineAttributeDTO.AttributeId, Convert.ToInt32(entityId), executionContext.GetSiteId(), contextOfAttribute, parafaitDBTrx.SQLTrx);
                                }
                                gameSystem = new GameSystem(entityName, Convert.ToInt32(entityId), executionContext, parafaitDBTrx.SQLTrx);
                                List<MachineAttributeDTO> machineAttributeDTOList = gameSystem.GetMachineAttributes();
                                if (machineAttributeDTOList != null)
                                {
                                    foreach (MachineAttributeDTO maDTO in machineAttributeDTOList)
                                    {
                                        if (maDTO.AttributeName == (MachineAttributeDTO.MachineAttribute)machineAttributeDTO.AttributeName)
                                        {
                                            machineAttribute.ContextOfAttribute = (MachineAttributeDTO.AttributeContext)Enum.Parse(typeof(MachineAttributeDTO.AttributeContext), attributeEntityName, true);
                                            machineAttribute.AttributeName = maDTO.AttributeName;
                                            machineAttribute.AttributeNameText = maDTO.AttributeName.ToString().Replace("_", " ");
                                            machineAttribute.AttributeId = maDTO.AttributeId;
                                            machineAttribute.AttributeValue = maDTO.AttributeValue;
                                            machineAttribute.IsFlag = maDTO.IsFlag;
                                            result = machineAttribute;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ex;
                        }

                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}

        
    


    