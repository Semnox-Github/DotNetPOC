/********************************************************************************************
 * Project Name - Machine                                                                          
 * Description  - Manages the machine object. 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60       26-Mar-2019   Jagan           Created
 *                                         Implemented the method HandleMachineMovement() for the site to site transfer and 
 *                                         if there is any referenceMachineId for the current machine, again call the same method to parent machine move process
 *           16-Mar-2019   Mehraj          Added Bulkupload method  and BuildTemplete method                           
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.DigitalSignage;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace Semnox.Parafait.Game
{
    public class MachineServices
    {
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction = null;
        private Sheet errorSheet;
        private Sheet responseSheet;
        private int sourceMasterEntityId = -1;
        private MachineDTO updatedMachineDTO;
        private int referenceMachineId = -1;
        public MachineServices(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// The below function insert a record in the tables machines and gameprofile attributevalues.
        /// It will also update the GUID, SiteID, Status while moving the machine from one site to another site
        /// Destination will be the actual machine record and source will be a duplicate record.
        /// The function creates a duplicate record, and set the site as the original siteid. The original record will be pointed to the destination siteid.
        /// If there are any child machines(based on the referenceMachineId), the child will also be moved to source siteID
        /// <param name="sourceMachineId">sourceMachineId</param>
        /// <param name="destinationSiteId">destinationSiteId</param>
        /// <param name="remarks">remarks</param>
        /// <returns></returns>
        public string HandleMachineMovement(int sourceMachineId, int destinationSiteId, string remarks)
        {
            log.LogMethodEntry(sourceMachineId, destinationSiteId, remarks);
            string message = string.Empty;
            ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction();
            try
            {
                parafaitDBTrx.BeginTransaction();
                sqlTransaction = parafaitDBTrx.SQLTrx; 
                /// By passing the source machine id, machine dto will be retrived.
                Machine machine = new Machine(sourceMachineId, executionContext);

                MachineDTO existingMachineDTO = machine.GetMachineDTO;
                if (existingMachineDTO != null && existingMachineDTO.IsActive == "T")
                {
                    /// Below condition is to check whether the machine is parent or child by checking the ReferenceMachineId
                    if (existingMachineDTO.ReferenceMachineId == -1)
                    {
                        TransferMachine(existingMachineDTO, destinationSiteId, remarks);

                        /// The below functionality, To check any child machine exits(by passing REFERENCE_MACHINE_ID in search params) for the parent machine
                        /// Based on the sourceMachineId as referenceMachineId , The list of child machines will be retrieved
                        /// Each child will be moved by calling the function TransferMachine()  
                        List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
                        searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.REFERENCE_MACHINE_ID, Convert.ToString(sourceMachineId)));

                        MachineList machineListBL = new MachineList(executionContext);
                        List<MachineDTO> machineList = machineListBL.GetMachineList(searchParameters);
                        if (machineList != null)
                        {
                            referenceMachineId = updatedMachineDTO.MachineId;
                            foreach (MachineDTO referenceMachineDTO in machineList)
                            {
                                /// Child machines will be transfererd when their status are !="N" or !="T - Tarnsit".
                                if (referenceMachineDTO.IsActive != "N" && referenceMachineDTO.IsActive == "T")
                                {
                                    referenceMachineDTO.MasterEntityId = sourceMasterEntityId;
                                    sourceMasterEntityId = -1;
                                    TransferMachine(referenceMachineDTO, destinationSiteId, remarks);
                                }
                            }
                        }
                        parafaitDBTrx.EndTransaction();
                        message = MessageContainerList.GetMessage(executionContext, 1845);/// Machine moved successfully
                        log.LogMethodExit(message);
                        return message;
                    }
                    else
                    {
                        message = MessageContainerList.GetMessage(executionContext, 1846); /// There is a parent machine reference, Please move parent machine.
                        log.LogMethodExit(null, "Throwing Validation Exception - " + message);
                        throw new ValidationException(message);
                    }
                }
                else
                {
                    message = MessageContainerList.GetMessage(executionContext, 1850); /// The machine is not an In-transit.
                    log.LogMethodExit(null, "Throwing Validation Exception - " + message);
                    throw new ValidationException(message);
                }
            }
            catch (ValidationException ex)
            {
                parafaitDBTrx.RollBack();
                log.Error(ex);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                parafaitDBTrx.RollBack();
                throw;
            }
            finally
            {
                parafaitDBTrx.Dispose();
            }
        }
        /// <summary>
        /// Transfer Machine from one site to another site
        /// </summary>
        /// <param name="destinationMachineDTO"></param>
        /// <param name="destinationSiteId"></param>
        /// <param name="remarks"></param>        
        private void TransferMachine(MachineDTO destinationMachineDTO, int destinationSiteId, string remarks)
        {
            log.LogMethodEntry(destinationMachineDTO, destinationSiteId, remarks);
            try
            {
                string userId = executionContext.GetUserId();
                int sourceSiteId = executionContext.GetSiteId();
                string destinationGuid = destinationMachineDTO.Guid;
                ExecutionContext sourceExecutionContext = new ExecutionContext(executionContext.GetUserId(), sourceSiteId, -1, -1, executionContext.GetIsCorporate(), executionContext.GetLanguageId());
                ExecutionContext destinationExecutionContext = new ExecutionContext(executionContext.GetUserId(), destinationSiteId, -1, -1, executionContext.GetIsCorporate(), -1);

                /// Below code is to check the machine name exists or not in the destionation site
                List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
                searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.MACHINE_NAME, destinationMachineDTO.MachineName));
                searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.SITE_ID, Convert.ToString(destinationSiteId)));
                MachineList machineList = new MachineList(sourceExecutionContext);
                List<MachineDTO> machineDTOList = machineList.GetMachineList(searchParameters);
                if (machineDTOList != null && machineDTOList.Count != 0)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(sourceExecutionContext, 1848)); /// The machine name is already exist in the destionation site.
                }

                /// By passing the GameId, Game object will be created and masterEntityID will be retrived from this object
                GameDTO gameDTO = GetGameDTO(destinationMachineDTO.GameId, destinationSiteId);

                /// The below condition will check whether game has been published or not. If published, MasterEntityId will be retrieved.
                /// By passing MasterEntityId and Destination SiteId, we will get the published Game object if it is there. If the record not there, it is not published to the destination site
                if (gameDTO != null && gameDTO.MasterEntityId > 0)
                {
                    /// constructor with all parameters and insert new record with source site id

                    MachineDTO sourceMachineDTO = new MachineDTO(-1, destinationMachineDTO.MachineName, destinationMachineDTO.MachineAddress, destinationMachineDTO.GameId, destinationMachineDTO.MasterId, destinationMachineDTO.Notes, DateTime.Now, userId, destinationMachineDTO.TicketAllowed, "N", destinationMachineDTO.TimerMachine,
                        destinationMachineDTO.TimerInterval, destinationMachineDTO.GroupTimer, destinationMachineDTO.NumberOfCoins, destinationMachineDTO.TicketMode, -1, destinationMachineDTO.ThemeId, -1, destinationMachineDTO.ShowAd, null, destinationSiteId, destinationMachineDTO.IPAddress, destinationMachineDTO.TCPPort, destinationMachineDTO.MacAddress,
                        destinationMachineDTO.Description, destinationMachineDTO.SerialNumber, destinationMachineDTO.SoftwareVersion, destinationMachineDTO.SynchStatus, destinationMachineDTO.PurchasePrice, destinationMachineDTO.ReaderType, destinationMachineDTO.PayoutCost, -1, -1, destinationMachineDTO.MasterEntityId,
                        destinationMachineDTO.ExternalMachineReference, destinationMachineDTO.MachineTag, destinationMachineDTO.CommunicationSuccessRatio, -1, -1, DateTime.Now, userId, DateTime.MinValue,"","","","", "", false, destinationMachineDTO.AllowedMachineID);

                    /// By passing the ThemeId, Theme object will be created and masterEntityID will be retrived from this object
                    ThemeDTO themeDTO = new ThemeDTO();
                    if (destinationMachineDTO.ThemeId > 0)
                    {
                        themeDTO = GetThemeDTO(destinationMachineDTO.ThemeId, destinationSiteId);
                        if (themeDTO != null)
                        {
                            destinationMachineDTO.ThemeId = themeDTO.Id;
                        }
                        else
                        {
                            destinationMachineDTO.ThemeId = -1;
                        }
                    }
                    /// By passing the MasterId, Hub object will be created and masterEntityID will be retrived from this object
                    HubDTO newHubDTO = new HubDTO();
                    if (destinationMachineDTO.MasterId > 0)
                    {
                        newHubDTO = GetHubDTO(destinationMachineDTO.MasterId, destinationSiteId);
                        if (newHubDTO != null)
                        {
                            destinationMachineDTO.MasterId = newHubDTO.MasterId;
                        }
                        else
                        {
                            destinationMachineDTO.MasterId = -1;
                        }
                    }

                    ///Getting the machine attribute list for source machine if any values created for current site
                    List<MachineAttributeDTO> sourceAttributeDTOList = GetMachineAttributeDTO(destinationMachineDTO.MachineId, executionContext, sqlTransaction);

                    /// Hold the machine attribute list for source machine for updating the guid
                    List<MachineAttributeDTO> updateAttributeDTOList = new List<MachineAttributeDTO>();
                    /// Updating the New Guid to the profile attribute list
                    if (sourceAttributeDTOList != null)
                    {
                        foreach (var machineAttributeDTO in sourceAttributeDTOList)
                        {
                            Guid attributeGuid = Guid.NewGuid();
                            //machineAttributeDTO.AttributeId = -1;
                            MachineAttributeDTO updateMachineAttributeDTO = new MachineAttributeDTO(machineAttributeDTO.AttributeId, machineAttributeDTO.AttributeName, machineAttributeDTO.AttributeValue, machineAttributeDTO.IsFlag, machineAttributeDTO.IsSoftwareAttribute, machineAttributeDTO.ContextOfAttribute, attributeGuid.ToString(), machineAttributeDTO.SynchStatus, executionContext.GetSiteId(), executionContext.GetUserId(),DateTime.Now, machineAttributeDTO.MasterEntityId, executionContext.GetUserId(),DateTime.Now);
                            updateMachineAttributeDTO.IsChanged = true;
                            updateAttributeDTOList.Add(updateMachineAttributeDTO);
                        }
                    }
                    /// Update with new GUID with destination siteid 
                    destinationMachineDTO.GameMachineAttributes = updateAttributeDTOList;
                    Guid guid = Guid.NewGuid();
                    destinationMachineDTO.Guid = guid.ToString();
                    destinationMachineDTO.GameId = gameDTO.GameId;
                    destinationMachineDTO.ThemeNumber = -1;
                    destinationMachineDTO.CustomDataSetId = -1;
                    destinationMachineDTO.NextMachineId = -1;
                    destinationMachineDTO.PreviousMachineId = -1;
                    destinationMachineDTO.InventoryLocationId = -1;
                    destinationMachineDTO.MachineCommunicationLogDTO = null;
                    Machine destinationMachine = new Machine(destinationMachineDTO, destinationExecutionContext, sqlTransaction);
                    destinationMachine.Save();

                    /// Insert the source machine dto
                    /// Insert the profile attributes for source machine
                    if (sourceAttributeDTOList != null)
                    {
                        foreach (MachineAttributeDTO machineAttributeDTO in sourceAttributeDTOList)
                        {
                            machineAttributeDTO.AttributeId = -1;
                        }
                    }
                    sourceMachineDTO.GameMachineAttributes = sourceAttributeDTOList;
                    /// Set the reference machineId for child records. or else it will set the referece machineId is -1
                    if (referenceMachineId > 0)
                    {
                        sourceMachineDTO.ReferenceMachineId = referenceMachineId;
                    }
                    sourceMachineDTO.MachineCommunicationLogDTO = null;
                    Machine sourceMachine = new Machine(sourceMachineDTO, sourceExecutionContext, sqlTransaction);
                    sourceMachine.Save();
                    sourceMachineDTO = sourceMachine.GetMachineDTO;
                    /// The below condition is for keeping the parent machineId. This id will be updated to the column "ReferenceMachineId" for  the child machines
                    if (sourceMasterEntityId < 0)
                    {
                        sourceMasterEntityId = sourceMachineDTO.MachineId;
                    }

                    ///Getting the machine attribute list for destination machine if any values created for current site
                    List<MachineAttributeDTO> destinationAttributeDTOList = GetMachineAttributeDTO(sourceMachineDTO.MachineId, sourceExecutionContext, sqlTransaction);

                    if (destinationAttributeDTOList != null && sourceAttributeDTOList != null)
                    {
                        foreach (MachineAttributeDTO sourceAttributeDTO in sourceAttributeDTOList)
                        {
                            foreach (MachineAttributeDTO destinationAttributeDTO in destinationAttributeDTOList)
                            {
                                if (sourceAttributeDTO.AttributeName == destinationAttributeDTO.AttributeName)
                                {
                                    destinationAttributeDTO.Guid = sourceAttributeDTO.Guid;
                                    break;
                                }
                            }
                        }
                        sourceMachineDTO.GameMachineAttributes = destinationAttributeDTOList;
                    }
                    else
                    {
                        sourceMachineDTO.GameMachineAttributes = null;
                    }
                    /// Updated the source machine guid                    
                    sourceMachineDTO.Guid = destinationGuid;

                    sourceMachineDTO.MachineName = sourceMachineDTO.MachineName + DateTime.Now.ToString("ddMMyyHHmm");
                    sourceMachineDTO.MachineCommunicationLogDTO = null;
                    sourceMachine = new Machine(sourceMachineDTO, sourceExecutionContext, sqlTransaction);
                    sourceMachine.Save();
                    updatedMachineDTO = sourceMachine.GetMachineDTO;
                    /// Logging in to the table "MachineTransferLog" Source Machine
                    MachineTransferLogDTO machineTransferLogDTO = new MachineTransferLogDTO(-1, destinationMachineDTO.MachineId, executionContext.GetSiteId(), destinationSiteId, DateTime.Now, userId,
                      "E", "T", sourceMachineDTO.Guid, destinationMachineDTO.Guid, remarks, destinationSiteId, null, true, destinationMachineDTO.MachineId, userId, DateTime.Now, userId, DateTime.Now, true);

                    MachineTransferLog machineTransferLog = new MachineTransferLog(destinationExecutionContext, machineTransferLogDTO, sqlTransaction);
                    machineTransferLog.Save();

                    /// Logging in to the table "MachineTransferLog" Destination Machine
                    machineTransferLogDTO = new MachineTransferLogDTO(-1, sourceMachineDTO.MachineId, destinationSiteId, executionContext.GetSiteId(), DateTime.Now, userId,
                    "T", "N", destinationMachineDTO.Guid, sourceMachineDTO.Guid, remarks, sourceSiteId, null, true, sourceMachineDTO.MachineId, userId, DateTime.Now, userId, DateTime.Now, true);
                    machineTransferLog = new MachineTransferLog(sourceExecutionContext, machineTransferLogDTO, sqlTransaction);
                    machineTransferLog.Save();

                    log.LogMethodExit();
                }
                else
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1847);// The machine related game not been published.
                    log.LogMethodExit(null, "Throwing Validation Exception - " + message);
                    throw new ValidationException(message);
                }
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Get the gameDTO based on the destination site and gameId i.e master entity id
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="destinationSiteId"></param>
        /// <returns>GameDTO</returns>
        private GameDTO GetGameDTO(int gameId, int destinationSiteId)
        {
            log.LogMethodEntry(gameId, destinationSiteId);
            Game game = new Game(gameId);
            List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters = new List<KeyValuePair<GameDTO.SearchByGameParameters, string>>();
            searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.SITE_ID, Convert.ToString(destinationSiteId)));
            searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.MASTER_ENTITY_ID, Convert.ToString(game.GetGameDTO.MasterEntityId)));
            GameList gameList = new GameList();
            List<GameDTO> gameDTOList = gameList.GetGameList(searchParameters, false);
            GameDTO newGameDTO = null;
            if (gameDTOList != null)
            {
                newGameDTO = gameDTOList.FirstOrDefault();
            }
            log.LogMethodExit(newGameDTO);
            return newGameDTO;
        }
        /// <summary>
        /// Get the ThemeDTO based on the destination site and themeId i.e master entity id
        /// </summary>
        /// <param name="themeId"></param>
        /// <param name="destinationSiteId"></param>
        /// <returns></returns>
        private ThemeDTO GetThemeDTO(int themeId, int destinationSiteId)
        {
            log.LogMethodEntry(themeId, destinationSiteId);
            ThemeBL themeBL = new ThemeBL(executionContext, themeId);
            ThemeDTO themeDTO = null;
            if (themeBL.ThemeDTO != null && themeBL.ThemeDTO.MasterEntityId > 0)
            {
                List<KeyValuePair<ThemeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ThemeDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.SITE_ID, Convert.ToString(destinationSiteId)));
                searchParameters.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.MASTER_ENTITY_ID, Convert.ToString(themeBL.ThemeDTO.MasterEntityId)));
                ThemeListBL themeList = new ThemeListBL(executionContext);
                List<ThemeDTO> themeDTOList = themeList.GetThemeDTOList(searchParameters, true, true);
                if (themeDTOList != null)
                {
                    themeDTO = themeDTOList.FirstOrDefault();
                }
            }
            log.LogMethodExit(themeDTO);
            return themeDTO;
        }
        /// <summary>
        /// Get the HubDTO based on the destination site and hubId i.e master entity id
        /// </summary>
        /// <param name="hubId"></param>
        /// <param name="destinationSiteId"></param>
        /// <returns></returns>
        private HubDTO GetHubDTO(int hubId, int destinationSiteId)
        {
            log.LogMethodEntry(hubId, destinationSiteId);
            Hub hub = new Hub(hubId);
            HubDTO newHubDTO = null;
            if (hub.GetHubDTO != null && hub.GetHubDTO.MasterEntityId > 0)
            {
                List<KeyValuePair<HubDTO.SearchByHubParameters, string>> searchHubParameters = new List<KeyValuePair<HubDTO.SearchByHubParameters, string>>();
                searchHubParameters.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.SITE_ID, Convert.ToString(destinationSiteId)));
                searchHubParameters.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.MASTER_ENTITY_ID, Convert.ToString(hub.GetHubDTO.MasterEntityId)));
                HubList hubList = new HubList(executionContext);
                List<HubDTO> hubDTOList = hubList.GetHubSearchList(searchHubParameters);
                if (hubDTOList != null)
                {
                    newHubDTO = hubDTOList.FirstOrDefault();
                }
            }
            log.LogMethodExit(newHubDTO);
            return newHubDTO;
        }
        /// <summary>
        /// Get the machine attributes list
        /// </summary>
        /// <param name="machineId"></param>
        /// <param name="executionContext"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>Retruns the list of Machine Attributes DTO</returns>
        private List<MachineAttributeDTO> GetMachineAttributeDTO(int machineId, ExecutionContext executionContext, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(machineId, executionContext, sqlTransaction);
            /// Getting the machine attribute list for source machine if any values created for current site
            GameSystemList gameSystemList = new GameSystemList(executionContext, sqlTransaction);
            List<KeyValuePair<MachineAttributeDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<MachineAttributeDTO.SearchByParameters, string>>();
            searchByParameters.Add(new KeyValuePair<MachineAttributeDTO.SearchByParameters, string>(MachineAttributeDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
            if (machineId > 0)
            {
                searchByParameters.Add(new KeyValuePair<MachineAttributeDTO.SearchByParameters, string>(MachineAttributeDTO.SearchByParameters.MACHINE_ID, Convert.ToString(machineId)));
            }
            List<MachineAttributeDTO> sourceAttributeDTOList = gameSystemList.GetMachineAttributeDTOList(searchByParameters, MachineAttributeDTO.AttributeContext.MACHINE);
            log.LogMethodExit(sourceAttributeDTOList);
            return sourceAttributeDTOList;
        }

        /// <summary>
        /// Build the templete with column headings and default values for attributes
        /// </summary>
        /// <returns></returns>
        public Sheet BuildTemplete()
        {
            try
            {
                log.LogMethodEntry();
                Sheet sheet = new Sheet();
                ///All column Headings are in a headerRow object
                Row headerRow = new Row();
                ///All defaultvalues for attributes are in defaultValueRow object
                Row defaultValueRow = new Row();
                ///Mapper class thats map sheet object
                MachineDTODefination machineDTODefination = new MachineDTODefination(executionContext, "");
                ///Building headers from MachineDTODefination attributes
                machineDTODefination.BuildHeaderRow(headerRow);
                sheet.AddRow(headerRow);
                GameSystem gameSystem = new GameSystem("MACHINES", -1, executionContext);
                ///Getting all parent level machine attributes
                List<MachineAttributeDTO> machineAttributeDTOList = gameSystem.GetMachineAttributes();
                ///Creation of row with defaultvalues
                if (machineAttributeDTOList != null)
                {
                    foreach (Row row in sheet.RowList)
                    {
                        foreach (Cell cell in row.CellList)
                        {
                            string defaultValue = string.Empty;
                            foreach (MachineAttributeDTO machineAttributeDTO in machineAttributeDTOList)
                            {
                                if (cell.Value == machineAttributeDTO.AttributeName.ToString())
                                {
                                    ///Adding default value based on attribute name
                                    defaultValue = machineAttributeDTO.AttributeValue;
                                }
                            }
                            ///Creating a cell with default value
                            defaultValueRow.AddCell(new Cell(defaultValue));
                        }
                    }
                }
                ///Adding Row of defaultvalues to sheet
                sheet.AddRow(defaultValueRow);
                log.LogMethodExit();
                return sheet;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }

        }

        /// <summary>
        /// The below funciton reads the sheet object and create individual MachineDTOs
        /// Machine attribute and game attributes will be compared and if there is any difference in the value, then the value for the machine attribute will be updated
        /// if the machinename is already exists, the game refernce will be verified and updated.  machine attributes will also be updated. If the machine name does nto exists, it will be new machine insertion
        /// </summary>
        /// <returns></returns>

        public Sheet BulkUpload(Sheet sheet)
        {
            log.LogMethodEntry(sheet);
            //Mapper class initialization. This class does all the converstions for sheet
            MachineDTODefination machineDTODefination = new MachineDTODefination(executionContext, "");
            int errorMachineCount = 0;

            ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction();
            //looping the sheet object. Sheet will have  multiple roews. Top row will be column heading
            for (int i = 1; i < sheet.Rows.Count; i++)
            {
                int index = 0;
                try
                {
                    ///send first row 9header row) and the next row.
                    /// Deserialize will return a machine dto.
                    MachineDTO rowMachineDTO = (MachineDTO)machineDTODefination.Deserialize(sheet[0], sheet[i], ref index);
                    if (rowMachineDTO != null)
                    {
                        parafaitDBTrx.BeginTransaction();
                        sqlTransaction = parafaitDBTrx.SQLTrx;
                        try
                        {
                            List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
                            searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                            searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.MACHINE_NAME, rowMachineDTO.MachineName));
                            MachineList machineList = new MachineList(executionContext);
                            ///This method will return MachineDTO collection if already exists
                            List<MachineDTO> exsitingMachineList = machineList.GetMachineList(searchParameters, false);
                            /// Get all the attrbutes for Games. These attributes will be compared with the machine attributes. 
                            /// If there is any diffference in the value, then only the machine attribute value will be inserted a for a machine.
                            GameSystem gameSystem = new GameSystem("GAMES", rowMachineDTO.GameId, executionContext);
                            List<MachineAttributeDTO> gameAttributeDTOList = gameSystem.GetMachineAttributes();
                            List<MachineAttributeDTO> savemachineAttributesList = new List<MachineAttributeDTO>();
                            if (exsitingMachineList != null)
                            {
                                ///in machineList.GetMachineList(), it is a like operator. There may be a multiple machineDTO. Hence filtering has happening in the below code
                                ///sheetMachineDTO will have the machine name. Appropriate machineDTO will be retrived form exsitingMachineList.
                                MachineDTO machine = exsitingMachineList.Where(m => m.MachineName == rowMachineDTO.MachineName).FirstOrDefault();
                                if (machine != null)
                                {
                                    rowMachineDTO.MachineId = machine.MachineId;
                                    rowMachineDTO.MachineAddress = machine.MachineAddress; //assigning MachineAddress for update case 
                                    /// Getting machine attributes for the current machineDTO(from sheet object)                                   
                                    List<MachineAttributeDTO> machineAttributes = GetMachineAttributeDTO(rowMachineDTO.MachineId, executionContext, sqlTransaction); //Getting All MachineAttributes for particular Machine 
                                    if (gameAttributeDTOList != null)
                                    {
                                        foreach (MachineAttributeDTO machineAttributeDTO in rowMachineDTO.GameMachineAttributes)
                                        {
                                            /// the below collection gameAttributeDTOList is having attributes for a game which belongs to the machine
                                            /// Basically to compare Machine attributes and Game attributes. Record will be inserted only if there is nay difference in the value
                                            foreach (MachineAttributeDTO gameProfileAttributeDTO in gameAttributeDTOList)
                                            {
                                                /// Check the the attribute name from current machine dto and the gameprofileattributes. If the attribute name macthes and the value does not match, then it will be inseran attribute in the Gameprofileattributes table.
                                                /// This will be over ridden value for a machine
                                                if (machineAttributeDTO.AttributeName == gameProfileAttributeDTO.AttributeName && machineAttributeDTO.AttributeValue != gameProfileAttributeDTO.AttributeValue)
                                                {
                                                    ///savemachineAttributesList is a colelction which will hold the attributes which have to be inserted
                                                    ///Later, these attributes from  the collection will be inserted to gameprofileattributes table
                                                    ///if the value differes then it will be insert and set Id as -1 and these attributes will be added to a collection
                                                    machineAttributeDTO.AttributeId = -1;
                                                    savemachineAttributesList.Add(machineAttributeDTO);// adding the attribute to a savemachineAttributesList collection which of type MachineAttributeDTO
                                                    break;
                                                }

                                            }
                                        }
                                    }
                                    /// machineAttributes contains attributes for a the cuurenct machine i.e rowMachineDTO
                                    /// savemachineAttributesList has the attributes and values which were supposed to be inserted
                                    if (savemachineAttributesList.Count > 0)
                                    {
                                        if (machineAttributes != null)
                                        {
                                            foreach (MachineAttributeDTO machineAttributesList in savemachineAttributesList)
                                            {
                                                foreach (MachineAttributeDTO insertedMachineAttributesList in machineAttributes)
                                                {
                                                    if (machineAttributesList.AttributeName == insertedMachineAttributesList.AttributeName && machineAttributesList.AttributeId != insertedMachineAttributesList.AttributeId)
                                                    {
                                                        machineAttributesList.AttributeId = insertedMachineAttributesList.AttributeId;//assigning back the AttributeId for update in case of -1 it will be an insert
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        rowMachineDTO.GameMachineAttributes = savemachineAttributesList;
                                    }
                                    else
                                    {
                                        rowMachineDTO.GameMachineAttributes = null;
                                    }
                                }
                            }
                            // if machine is not there from rowMachineDTO(sheet object), then insert a new machine and machine attributes
                            else
                            {
                                if (gameAttributeDTOList != null)
                                {
                                    /// Setting machineId -1 in case of new machine
                                    rowMachineDTO.MachineId = -1;
                                    if (rowMachineDTO.GameMachineAttributes != null)
                                    {
                                        foreach (MachineAttributeDTO machineProfileAttributeDTO in rowMachineDTO.GameMachineAttributes)
                                        {
                                            foreach (MachineAttributeDTO gameProfileAttributeDTO in gameAttributeDTOList)
                                            {
                                                /// Check the the attribute name from current machine dto and the gameprofileattributes. If the attribute name macthes and the value does not match, then it will be inseran attribute in the Gameprofileattributes table.
                                                /// This will be over ridden value for a machine
                                                if (machineProfileAttributeDTO.AttributeName == gameProfileAttributeDTO.AttributeName && machineProfileAttributeDTO.AttributeValue != gameProfileAttributeDTO.AttributeValue)
                                                {
                                                    ///savemachineAttributesList is a colelction which will hold the attributes which have to be inserted
                                                    ///Later, these attributes from  the collection will be inserted to gameprofileattributes table
                                                    ///if the value differes then it will be insert and set Id as -1 and these attributes will be added to a collection
                                                    machineProfileAttributeDTO.AttributeId = -1; //if there are any overriden values setting AttributeId to -1
                                                    savemachineAttributesList.Add(machineProfileAttributeDTO);// adding the attribute to a savemachineAttributesList collection which of type MachineAttributeDTO
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (savemachineAttributesList.Count > 0)
                            {
                                rowMachineDTO.GameMachineAttributes = savemachineAttributesList;
                            }
                            else
                            {
                                rowMachineDTO.GameMachineAttributes = null;
                            }
                            rowMachineDTO.MachineCommunicationLogDTO = null;
                            Machine machines = new Machine(rowMachineDTO, executionContext, sqlTransaction);
                            machines.SaveMachine();//Saving the MachineDTO
                            sqlTransaction.Commit();
                            //In case of successful insert we add a cell to status object with cell value saved
                            if (responseSheet == null)
                            {
                                responseSheet = new Sheet();
                                responseSheet.AddRow(sheet[0]);
                                responseSheet[0].AddCell(new Cell(MessageContainerList.GetMessage(executionContext, "Status")));
                            }
                            responseSheet.AddRow(sheet[i]);
                            responseSheet[responseSheet.Rows.Count - 1].AddCell(new Cell(MessageContainerList.GetMessage(executionContext, "SUCCESS")));

                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            throw;
                        }

                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogVariableState("Row", sheet[i]);
                    //In case of exception we add a cell to status object with cell value saved
                    if (responseSheet == null)
                    {
                        responseSheet = new Sheet();
                        responseSheet.AddRow(sheet[0]);
                        responseSheet[0].AddCell(new Cell(MessageContainerList.GetMessage(executionContext, "Status")));
                    }
                    responseSheet.AddRow(sheet[i]);
                    string errorMessage = "";
                    string seperator = "";
                    if (ex is ValidationException)
                    {
                        foreach (var validationError in (ex as ValidationException).ValidationErrorList)
                        {
                            errorMessage += seperator;
                            errorMessage += validationError.Message;
                            seperator = ", ";
                        }
                    }
                    else
                    {
                        errorMessage = ex.Message;
                    }
                    responseSheet[responseSheet.Rows.Count - 1].AddCell(new Cell("Failed:" + errorMessage));
                    errorMachineCount++;
                    log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                }
            }
            parafaitDBTrx.Dispose();
            log.LogMethodExit(responseSheet);
            return responseSheet;
        }
    }
}
