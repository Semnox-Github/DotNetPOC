/********************************************************************************************
 * Project Name - Game System                                                                          
 * Description  - GameSystem represents the environment of the game machines. This is primarily 
 * to hold the configuration parameters which could be set for the entire environment.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Dec-2015   Kiran          Created 
 *2.40        04-09-2018    Jagan          Created method GetMachineAttributes and return master attributes for reader configuration.
 *                                         Added GameSystem constructor with parameters : moduleName,moduleRowId,executionContext.
 *                                         Added GameSystemList constructor with parameters : executioncontext,systemattributes,modulename,commonid.
 *                                         Added new method for save attributes for all AttributeContext : SaveGameSystemList()
 *2.50.0      19-dec-2018   Jagan          DeleteMachineAttribute for Reset Link the configurauration for particular attribute againest EntityId
 *2.60        06-Apr-2019   Jagan          Created method GetMachineAttributeDTOList()
 *2.80.0      10-Mar-2020   Girish Kundar  Modified :  DBAudit Table added for machineAttributes 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.GenericUtilities;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// GameSystem represents the environment of the game machines. This is primarily to hold the configuration 
    /// parameters which could be set for the entire environment.
    /// </summary>
    public class GameSystem
    {
        private List<MachineAttributeDTO> systemAttributes;
        private MachineAttributeDataHandler machineAttributeDataHandler;
        private ExecutionContext executionContext;
        private SqlTransaction sqlTransaction;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor of GameSystem class
        /// Initializes the data handler 
        /// </summary>
        public GameSystem()
        {
            log.LogMethodEntry();
            machineAttributeDataHandler = new MachineAttributeDataHandler();
            executionContext = ExecutionContext.GetExecutionContext();
            systemAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.SYSTEM, -1, executionContext.GetSiteId());
            log.LogMethodExit(systemAttributes);
        }

        /// <summary>
        /// constructor with executionContext as a parameter
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public GameSystem(ExecutionContext executionContext, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, sqlTransaction);
            machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
            this.executionContext = executionContext != null ? executionContext : ExecutionContext.GetExecutionContext();
            systemAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.SYSTEM, -1, executionContext.GetSiteId());
            log.LogMethodExit(systemAttributes);
        }
        /// <summary>
        /// Gets the machine attribute values for each entity
        /// </summary>
        /// <param name="moduleName">moduleName</param>
        /// <param name="moduleRowId">moduleRowId</param>
        /// <param name="executionContext">executionContext</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Assigning the machine attribute values to "systemAttributes" which is declared in the constructor</returns>
        public GameSystem(string moduleName, int moduleRowId, ExecutionContext executionContext, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(moduleName, moduleRowId, executionContext, sqlTransaction);
            this.executionContext = executionContext;
            machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
            int siteId = this.executionContext.GetSiteId();
            switch (moduleName)
            {
                case "SYSTEM":
                    systemAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.SYSTEM, moduleRowId, siteId);
                    break;
                case "GAMES":
                    systemAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.GAME, moduleRowId, siteId);
                    break;

                case "GAME_PROFILE":
                    systemAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.GAME_PROFILE, moduleRowId, siteId);
                    break;

                case "MACHINES":
                    systemAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.MACHINE, moduleRowId, siteId);
                    break;
            }
            log.LogMethodExit(systemAttributes);
        }

        /// <summary>
        /// Gets the machine attributes set at the system level
        /// </summary>
        /// <param name="attribute">The machine attribute, the value of which is being requested</param>
        /// <returns>Returns the machine attribute value</returns>
        public MachineAttributeDTO GetMachineAttribute(MachineAttributeDTO.MachineAttribute attribute)
        {
            log.LogMethodEntry(attribute);
            foreach (MachineAttributeDTO currAttribute in systemAttributes)
            {
                if (currAttribute.AttributeName == attribute)
                {
                    log.LogMethodExit(currAttribute);
                    return currAttribute;
                }
            }
            // Ideally there should not be a case where a parameter is defined and it does not exist even at system level. 
            // Could only happen in case the DB is at the lower patch level than the system. 
            log.Info("Ends-GetMachineAttribute(attribute) Method.By throwing manual exception:\"The game system attribute by name  " + attribute + "  does not exist. Please check the system setup\"");
            throw new Exception("The game system attribute by name " + attribute + " does not exist. Please check the system setup");
        }

        /// <summary>
        /// Creates or updates the machine attributes at the system level
        /// If the attribute id is -1, then the attribute is created else update the attribute value        
        /// </summary>
        public void SaveSystemAttribute()
        {
            try
            {
                log.LogMethodEntry();
                string userId = executionContext.GetUserId();
                int siteId = executionContext.GetSiteId();
                MachineAttributeDTO machineAttributeDTO = new MachineAttributeDTO();
                foreach (MachineAttributeDTO currMachineAttribute in systemAttributes)
                {
                    machineAttributeDTO = currMachineAttribute;
                    if (machineAttributeDTO.AttributeId == -1)
                    {
                        machineAttributeDTO = machineAttributeDataHandler.InsertMachineAttribute(machineAttributeDTO, MachineAttributeDTO.AttributeContext.SYSTEM, -1, userId, siteId);
                        machineAttributeDTO.AcceptChanges();
                    }
                    else
                    {
                        if (machineAttributeDTO.IsChanged == true)
                        {
                            machineAttributeDTO = machineAttributeDataHandler.UpdateMachineAttribute(machineAttributeDTO, MachineAttributeDTO.AttributeContext.SYSTEM, -1, userId, siteId);
                            machineAttributeDTO.AcceptChanges();
                        }
                    }
                    if (!string.IsNullOrEmpty(machineAttributeDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("GameProfileattributevalues", machineAttributeDTO.Guid, sqlTransaction);
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
        }

        /// <summary>
        /// Delete existing attribute for particular attribute against entity.
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="entityId"></param>
        /// <param name="siteId"></param>
        /// <param name="attributeContext"></param>
        /// <param name="sqlTransaction"></param> 
        /// <returns></returns>
        public int DeleteMachineAttribute(int attributeId, int entityId, int siteId, MachineAttributeDTO.AttributeContext attributeContext, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(attributeId, entityId, siteId, attributeContext);
            machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
            int rowsDeleted = machineAttributeDataHandler.DeleteMachineAttribute(attributeId, entityId, siteId, attributeContext);
            log.LogMethodExit(rowsDeleted);
            return rowsDeleted;
        }

        /// <summary>
        /// Get the Reader Configuration Details
        /// </summary>        
        /// <returns>List</returns>
        public List<MachineAttributeDTO> GetMachineAttributes()
        {
            log.LogMethodEntry();
            try
            {
                log.LogMethodExit(systemAttributes);
                return systemAttributes;
            }
            catch (Exception ex)
            {
                // Ideally there should not be a case where a parameter is defined and it does not exist even at system level. 
                // Could only happen in case the DB is at the lower patch level than the system. 
                log.Info("Ends-GetMachineAttribute() Method.By throwing manual exception:\"The game system attributes");
                throw new Exception("The game system attribute by name " + ex.Message + " does not exist. Please check the system setup");
            }
        }

    }

    public class GameSystemList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<MachineAttributeDTO> systemAttributes;
        private MachineAttributeDataHandler machineAttributeDataHandler;
        private SqlTransaction sqlTransaction;
        private ExecutionContext executionContext;
        string moduleName;
        int commonId;
        private MachineAttributeDTO machineAttributeDTO;

        public GameSystemList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.systemAttributes = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public GameSystemList(ExecutionContext executionContext, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, sqlTransaction);
            this.systemAttributes = null;
            this.sqlTransaction = sqlTransaction;
            this.executionContext = executionContext;
            this.machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
            log.LogMethodExit();
        }

        public GameSystemList(ExecutionContext executionContext, List<MachineAttributeDTO> systemAttributes, string modulename, int commonid, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, systemAttributes, modulename, commonId, sqlTransaction);
            this.machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
            this.executionContext = executionContext;
            this.systemAttributes = systemAttributes;
            this.moduleName = modulename;
            this.commonId = commonid;
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public GameSystemList(MachineAttributeDTO machineAttributeDTO, ExecutionContext executionContext)
        {
            log.LogMethodEntry(machineAttributeDTO, executionContext);
            this.machineAttributeDTO = machineAttributeDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get the machine attributes list based on the searchParameters
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns>Returns the machine attribute dto list</returns> 

        public List<MachineAttributeDTO> GetMachineAttributeDTOList(List<KeyValuePair<MachineAttributeDTO.SearchByParameters, string>> searchParameters, MachineAttributeDTO.AttributeContext attributeContext)
        {
            log.LogMethodEntry(searchParameters, attributeContext);
            this.machineAttributeDataHandler = new MachineAttributeDataHandler();
            log.LogMethodExit();
            return machineAttributeDataHandler.GetMachineAttributeList(searchParameters, attributeContext);
        }

        /// Insert or Update the Profile attributes for WebManagementStudio
        /// </summary>
        /// <param name="systemAttributes">systemAttributes</param>
        /// <returns>No.Of Rows Inserterd/Updated</returns>

        public int SaveGameSystemList()
        {
            SqlConnection sqlConnection = null;
            SqlTransaction parafaitDBTrx =null;
            try
            {
                if (sqlTransaction == null)
                {
                    Utilities utilities = new Utilities();
                    sqlConnection = utilities.createConnection();
                    parafaitDBTrx = sqlConnection.BeginTransaction();
                }
                else
                {
                    parafaitDBTrx = sqlTransaction;
                }
                log.LogMethodEntry();

                string userId = executionContext.GetUserId();
                int siteId = executionContext.GetSiteId();
                int rowsUpdated = 0;
                GameSystem gameSystem = new GameSystem();
                machineAttributeDataHandler = new MachineAttributeDataHandler(parafaitDBTrx);
                List<MachineAttributeDTO> machineAttributeDTOList = new List<MachineAttributeDTO>();
                machineAttributeDTOList = systemAttributes.Where(attribute  => attribute.IsChanged == true).ToList();
                if (machineAttributeDTOList != null && machineAttributeDTOList.Any())
                {
                    foreach (MachineAttributeDTO currAttribute in machineAttributeDTOList)
                    {
                        int attributeId = machineAttributeDataHandler.GetAttributeId(currAttribute.AttributeName.ToString(), siteId);
                        List<KeyValuePair<MachineAttributeDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<MachineAttributeDTO.SearchByParameters, string>>();
                        searchByParameters.Add(new KeyValuePair<MachineAttributeDTO.SearchByParameters, string>(MachineAttributeDTO.SearchByParameters.ATTRIBUTE_ID, Convert.ToString(attributeId)));
                        int rowInserted = 0;
                        int rowUpdated = 0;
                        MachineAttributeDTO machineAttributeDTO = new MachineAttributeDTO();
                        switch (moduleName)
                        {
                            case "SYSTEM":
                                machineAttributeDTO = machineAttributeDataHandler.UpdateMachineAttribute(currAttribute, MachineAttributeDTO.AttributeContext.SYSTEM, -1, userId, siteId);
                                if (!string.IsNullOrEmpty(machineAttributeDTO.Guid))
                                {
                                    AuditLog auditLog = new AuditLog(executionContext);
                                    auditLog.AuditTable("GameProfileattributevalues", machineAttributeDTO.Guid, parafaitDBTrx);
                                }
                                break;

                            case "GAMES":
                                if (currAttribute.IsChanged == true)
                                {
                                    MachineAttributeDTO.AttributeContext contextName = (MachineAttributeDTO.AttributeContext)Enum.Parse(typeof(MachineAttributeDTO.AttributeContext), "GAME", true);
                                    searchByParameters.Add(new KeyValuePair<MachineAttributeDTO.SearchByParameters, string>(MachineAttributeDTO.SearchByParameters.GAME_ID, Convert.ToString(commonId)));

                                    /// Jagan Mohan 10-10-2018
                                    /// The below function GetEntityIDs returns id after checking  AttributeId and EntityID existed or not.
                                    /// For example, GameProfileID/Gameid/MachineId and attribute id is already there, then it return count as 1 or else 0
                                    /// Based on 1 or 0, Insert or Update will be called.
                                    attributeId = machineAttributeDataHandler.GetEntityIDs(searchByParameters);
                                    if (attributeId == 0 && MachineAttributeDTO.AttributeContext.GAME == contextName)
                                    {
                                        machineAttributeDTO = machineAttributeDataHandler.InsertMachineAttribute(currAttribute, MachineAttributeDTO.AttributeContext.GAME, commonId, userId, siteId);

                                    }
                                    else
                                    {
                                        if (currAttribute.IsChanged == true)
                                        {
                                            currAttribute.ContextOfAttribute = MachineAttributeDTO.AttributeContext.GAME;
                                            machineAttributeDTO = machineAttributeDataHandler.UpdateMachineAttribute(currAttribute, MachineAttributeDTO.AttributeContext.GAME, commonId, userId, siteId);
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(machineAttributeDTO.Guid))
                                    {
                                        AuditLog auditLog = new AuditLog(executionContext);
                                        auditLog.AuditTable("GameProfileattributevalues", machineAttributeDTO.Guid, parafaitDBTrx);
                                    }
                                }
                                break;

                            case "GAME_PROFILE":
                                if (currAttribute.IsChanged == true)
                                {
                                    MachineAttributeDTO.AttributeContext contextName = (MachineAttributeDTO.AttributeContext)Enum.Parse(typeof(MachineAttributeDTO.AttributeContext), moduleName, true);
                                    searchByParameters.Add(new KeyValuePair<MachineAttributeDTO.SearchByParameters, string>(MachineAttributeDTO.SearchByParameters.GAME_PROFILE_ID, Convert.ToString(commonId)));
                                    attributeId = machineAttributeDataHandler.GetEntityIDs(searchByParameters);
                                    if (attributeId == 0 && MachineAttributeDTO.AttributeContext.GAME_PROFILE == contextName)
                                    {
                                        machineAttributeDTO = machineAttributeDataHandler.InsertMachineAttribute(currAttribute, MachineAttributeDTO.AttributeContext.GAME_PROFILE, commonId, userId, siteId);
                                    }
                                    else
                                    {
                                        if (currAttribute.IsChanged == true)
                                        {
                                            currAttribute.ContextOfAttribute = MachineAttributeDTO.AttributeContext.GAME_PROFILE;
                                            machineAttributeDTO = machineAttributeDataHandler.UpdateMachineAttribute(currAttribute, MachineAttributeDTO.AttributeContext.GAME_PROFILE, commonId, userId, siteId);
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(machineAttributeDTO.Guid))
                                    {
                                        AuditLog auditLog = new AuditLog(executionContext);
                                        auditLog.AuditTable("GameProfileattributevalues", machineAttributeDTO.Guid, parafaitDBTrx);
                                    }
                                }
                                break;

                            case "MACHINES":
                                if (currAttribute.IsChanged == true)
                                {
                                    MachineAttributeDTO.AttributeContext contextName = (MachineAttributeDTO.AttributeContext)Enum.Parse(typeof(MachineAttributeDTO.AttributeContext), "MACHINE", true);
                                    searchByParameters.Add(new KeyValuePair<MachineAttributeDTO.SearchByParameters, string>(MachineAttributeDTO.SearchByParameters.MACHINE_ID, Convert.ToString(commonId)));
                                    attributeId = machineAttributeDataHandler.GetEntityIDs(searchByParameters);
                                    if (attributeId == 0 && MachineAttributeDTO.AttributeContext.MACHINE == contextName)
                                    {
                                        currAttribute.ContextOfAttribute = MachineAttributeDTO.AttributeContext.MACHINE;
                                        machineAttributeDTO = machineAttributeDataHandler.InsertMachineAttribute(currAttribute, MachineAttributeDTO.AttributeContext.MACHINE, commonId, userId, siteId);
                                    }
                                    else
                                    {
                                        if (currAttribute.IsChanged == true)
                                        {
                                            currAttribute.ContextOfAttribute = MachineAttributeDTO.AttributeContext.MACHINE;
                                            machineAttributeDTO = machineAttributeDataHandler.UpdateMachineAttribute(currAttribute, MachineAttributeDTO.AttributeContext.MACHINE, commonId, userId, siteId);
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(machineAttributeDTO.Guid))
                                    {
                                        AuditLog auditLog = new AuditLog(executionContext);
                                        auditLog.AuditTable("GameProfileattributevalues", machineAttributeDTO.Guid, parafaitDBTrx);
                                    }
                                    if (machineAttributeDTO.AttributeName == MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE)
                                    {
                                        MachineAttributeLogListBL machineAttributeLogListBL = new MachineAttributeLogListBL(executionContext, machineAttributeDTO.MachineAttributeLogDTOList);
                                        machineAttributeLogListBL.Save(sqlTransaction);
                                    }
                                }
                                break;
                        }
                        if (rowInserted != 0)
                            log.Info("Ends-SaveGameSystemList() MethodAttribute name " + currAttribute.AttributeName + ". Attribute Id: " + rowInserted);
                        if (rowUpdated != 0)
                            log.Info("Ends-SaveGameSystemList() MethodAttribute name " + currAttribute.AttributeName + ". Attribute Id: " + rowUpdated);
                    }
                    if (sqlTransaction == null)
                    {
                        parafaitDBTrx.Commit();
                        sqlConnection.Close();
                    }
                }
                log.LogMethodExit(rowsUpdated);

                return rowsUpdated;
            }
            catch (Exception ex)
            {
                if (sqlTransaction == null)  //SQLTransaction handled locally
                {
                    parafaitDBTrx.Rollback();
                }
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }

        }
    }
}

