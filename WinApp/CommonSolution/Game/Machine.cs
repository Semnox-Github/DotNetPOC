/********************************************************************************************
 * Project Name - Game Machine                                                                          
 * Description  - Manages the game machine object. 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Dec-2015   Kiran          Created 
 *1.10        11-Jan-2016   Mathew         Updated to handle hierarchy based Machine attribute
 *                                         assignment
 *1.20        04-Apr-2016   Mathew         Logging for Out of Service and IN Service done to 
 *                                         machineLog instead of Event Log
 *1.30        27-Sep-2016   Raghuveera     Added one method PutOutOfService() with one bolean parameter                           
 *1.30        12-Dec-2016   Vivek          New Generic Method BuildMachineListParams added
 *                                         Builds MachineDTO.SearchByMachineParameters from MachineParams
 *                                         Added New Method GetGameMachine
 *1.40        22-Jun-2017   Jeevan         New Generic Method for Play game for Achivements and readers
  ********************************************************************************************
 *2.40       09-Sept-2018  Jagan           For Machine API Modifications for insert/update,delete.
 *                                         Added New Mathods - Save
 *2.40       30-Oct-2018   Jagan           Added MachineTransfer method for Inter-Site Transfer machine based on particular machine.
 *2.41        07-Nov-2018   Rajiv           Commented existing logic which does not required anymore.
 *2.60        4-Mar-2019    Lakshminarayana Added Logic to get the active theme for the machine
 *           05-Apr-2019   Jagan           Added MachineTransferLog in save method Update statement while Is_Active status change. 
             15-Apr-2019   Akshay Gulaganji updated log.LogMethodEntry() and log.LogMethodExit()
             16-Mar-2019   Muhammed Mehraj  Added SaveMachine(),Validate(),ValidateStringField(),ValidateForeignKeyField()
 *2.60.2     27-May-2019   Muhammed Mehraj  Added AuditLog code to Save() for saving the Audits to DB Table DBAuditLog
 *           31-May-2019   Jagan            Added the new DeleteMachine()
 *2.70       07-Jul-2019   Mehraj           Added ValidateIsActiveFeild() Method
 *2.70.2       29-Jul-2019   deeksha          Modified -save method returns DTO
 *           25-Sept-2019  Jagan Mohana    Added AuditLog code to Save() for saving the Audits to DB Table DBAuditLog
 *2.70.3     20-Dec-2019   Archana         Added reference machine validation check in Validation() and MachineBuilderBL class
 *2.80.0      10-Mar-2020   Girish Kundar  Modified :  DBAudit Table added for machineAttributes 
 *2.90.0     13-08-2020     Nitin Pai      Fixes: Commando Fixes - Active flag was send as '1' instead of 'Y'
 *2.100      22-Oct-2020  Girish Kundar  Modified : for POS UI redesign
 *2.110.0     22-Dec-2020  Prajwal S        Modified : Changes for API GET. 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using Semnox.Parafait.Game;
using Semnox.Parafait.Communication;
using System.Data.SqlClient;
using Semnox.Parafait.GenericUtilities;
using System.Linq;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// Machine represents the physical game machine
    /// </summary>
    public class Machine : Game
    {
        /// <summary>
        /// Machine DTO object
        /// </summary>
        private MachineDTO machineDTO;
        private MachineDataHandler machineDataHandler;
        private readonly ExecutionContext machineUserContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<CoreKeyValueStruct> gameDetailsList;
        private SqlTransaction sqlTransaction;

        /// <summary>
        /// Default constructor of GameProfile class
        /// </summary>
        public Machine() : base()
        {
            log.LogMethodEntry();
            this.sqlTransaction = null;
            this.machineDataHandler = new MachineDataHandler();
            this.machineUserContext = ExecutionContext.GetExecutionContext();
            this.machineDTO = null;
            this.gameDetailsList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor with executionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public Machine(ExecutionContext executionContext) : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.machineUserContext = executionContext;
            this.sqlTransaction = null;
            this.machineDataHandler = new MachineDataHandler();
            this.machineDTO = null;
            this.gameDetailsList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the machine id as the parameter
        /// Would fetch the machine object from the database based on the id passed. 
        /// </summary>
        /// <param name="machineId">Machine id</param>
        /// <param name="loadChildRecords">loadChildRecords</param>
        public Machine(int machineId, bool loadChildRecords = false) : this()
        {
            log.LogMethodEntry(machineId, loadChildRecords);
            this.sqlTransaction = null;
            this.machineDTO = machineDataHandler.GetMachine(machineId);
            if (machineDTO != null)
            {
                MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
                List<MachineAttributeDTO> machineAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.MACHINE, machineId, machineUserContext.GetSiteId());
                machineDTO.SetAttributeList(machineAttributes);
            }
            base.GetGame(machineDTO.GameId);
            if (base.GetGameDTO != null)
            {
                this.machineDTO.GameName = base.GetGameDTO.GameName;
            }
            if (loadChildRecords)
            {
                MachineCommunicationLogList machineCommunicationLogList = new MachineCommunicationLogList(machineUserContext);
                List<KeyValuePair<MachineCommunicationLogDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<MachineCommunicationLogDTO.SearchByParameters, string>>();
                searchParams.Add(new KeyValuePair<MachineCommunicationLogDTO.SearchByParameters, string>(MachineCommunicationLogDTO.SearchByParameters.MACHINE_ID, machineId.ToString()));
                List<MachineCommunicationLogDTO> machineCommunicationLogDTOList = machineCommunicationLogList.GetMachineCommunicationLogDTOList(searchParams);
                if (machineCommunicationLogDTOList != null && machineCommunicationLogDTOList.Count > 0)
                    machineDTO.MachineCommunicationLogDTO = machineCommunicationLogDTOList[0];
                else
                {
                    machineDTO.MachineCommunicationLogDTO = new MachineCommunicationLogDTO();
                    machineDTO.MachineCommunicationLogDTO.MachineId = machineId;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates game machibe object using the MachineDTO
        /// </summary>
        /// <param name="machine">MachineDTO</param>
        public Machine(MachineDTO machine, bool loadChildRecords = false) : this()
        {
            log.LogMethodEntry(machine, loadChildRecords);
            this.machineDTO = machine;
            this.sqlTransaction = null;
            base.GetGame(machine.GameId);
            if (base.GetGameDTO != null)
            {
                this.machineDTO.GameName = base.GetGameDTO.GameName;
            }
            if (loadChildRecords)
            {
                MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
                List<MachineAttributeDTO> machineAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.MACHINE, machine.MachineId, machineUserContext.GetSiteId());
                this.machineDTO.GameMachineAttributes = machineAttributes;
                HubList hubList = new HubList(machineUserContext);
                List<KeyValuePair<HubDTO.SearchByHubParameters, string>> hubSearchParameters = new List<KeyValuePair<HubDTO.SearchByHubParameters, string>>
                {
                    new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.IS_ACTIVE, "Y"),
                    new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.SITE_ID, machineUserContext.GetSiteId().ToString())
                };
                List<HubDTO> hubDTOList = hubList.GetHubSearchList(hubSearchParameters);

                MachineBuilderBL machineBuilderBL = new MachineBuilderBL(machineUserContext);
                machineBuilderBL.UpdateMachineListToGetHubName(this.machineDTO, hubDTOList);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates game machibe object using the MachineDTO
        /// </summary>
        /// <param name="machine">MachineDTO</param>
        /// <param name="executionContext">executionContext</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public Machine(MachineDTO machine, ExecutionContext executionContext, SqlTransaction sqlTransaction = null, bool loadChildRecords = false) : this()
        {
            log.LogMethodEntry(machine, executionContext, sqlTransaction, loadChildRecords);
            this.machineUserContext = executionContext;
            this.machineDTO = machine;
            this.sqlTransaction = sqlTransaction;
            base.GetGame(machine.GameId);
            if (base.GetGameDTO != null)
            {
                this.machineDTO.GameName = base.GetGameDTO.GameName;
            }
            if (loadChildRecords)
            {
                MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
                List<MachineAttributeDTO> machineAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.MACHINE, machine.MachineId, machineUserContext.GetSiteId());
                this.machineDTO.GameMachineAttributes = machineAttributes;
                HubList hubList = new HubList(machineUserContext);
                List<KeyValuePair<HubDTO.SearchByHubParameters, string>> hubSearchParameters = new List<KeyValuePair<HubDTO.SearchByHubParameters, string>>
                {
                    new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.IS_ACTIVE, "Y"),
                    new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.SITE_ID, machineUserContext.GetSiteId().ToString())
                };
                List<HubDTO> hubDTOList = hubList.GetHubSearchList(hubSearchParameters);
                MachineBuilderBL machineBuilderBL = new MachineBuilderBL(executionContext);
                machineBuilderBL.UpdateMachineListToGetHubName(this.machineDTO, hubDTOList);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the machine id as the parameter
        /// Would fetch the machine object from the database based on the id passed. 
        /// </summary>
        /// <param name="machineId">Machine id</param>
        /// <param name="executionContext">executionContext/param>
        public Machine(int machineId, ExecutionContext executionContext, bool loadAttribute = false) : this()
        {
            log.LogMethodEntry(machineId, executionContext, loadAttribute);
            this.machineUserContext = executionContext;
            this.machineDTO = machineDataHandler.GetMachine(machineId);
            if (machineDTO != null)
            {
                base.GetGame(machineDTO.GameId);
            }
            if (base.GetGameDTO != null)
            {
                this.machineDTO.GameName = base.GetGameDTO.GameName;
            }
            if (machineDTO != null && loadAttribute)
            {
                MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
                List<MachineAttributeDTO> machineAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.MACHINE, machineId, machineUserContext.GetSiteId());
                machineDTO.SetAttributeList(machineAttributes);
            }
            log.LogMethodExit(machineDTO);
        }

        /// <summary>
        /// Gets the machine attributes set at the machine level
        /// </summary>
        /// <param name="attribute">The machine attribute, the value of which is being requested</param>
        /// <returns>Returns the machine attribute value</returns>
        public MachineAttributeDTO GetMachineMachineAttribute(MachineAttributeDTO.MachineAttribute attribute)
        {
            log.LogMethodEntry(attribute);

            MachineAttributeDTO machineAttributeDTO = machineDTO.GameMachineAttributes.Where(x => x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE && x.AttributeName == attribute).FirstOrDefault();
            if (machineAttributeDTO != null)
            {
                log.LogMethodExit(machineAttributeDTO);
                return machineAttributeDTO;
            }
            else
            {
                foreach (MachineAttributeDTO currAttribute in machineDTO.GameMachineAttributes)
                {
                    if (currAttribute.AttributeName == attribute)
                    {
                        log.LogMethodExit(currAttribute);
                        return currAttribute;
                    }
                }
            }

            log.Error("The game system attribute by name " + attribute + " does not exist for machine " + machineDTO.MachineName + ". Please check the system setup");
            //11-Jan-2016 - Throw exception if attribute not found
            throw new Exception("The game system attribute by name " + attribute + " does not exist for machine " + machineDTO.MachineName + ". Please check the system setup");
            //return base.GetGameMachineAttribute(attribute); //11-Jan-2016
        }

        /// <summary>
        /// Method to update attributes values for passed attribute
        /// </summary>
        /// <param name="attribute">Machine attribute</param>
        /// <param name="attributeValue">Machine attribute value to be updated</param>
        public void UpdateMachineAttribute(MachineAttributeDTO.MachineAttribute attribute, string attributeValue)
        {
            log.LogMethodEntry(attribute, attributeValue);
            MachineAttributeDTO machineAttrib = GetMachineMachineAttribute(attribute);
            if (machineAttrib.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)
                machineAttrib.AttributeValue = attributeValue;
            else
                machineDTO.AddToAttributes(machineAttrib.AttributeName, attributeValue);
            log.LogMethodExit();
        }

        /// <summary>
        /// Updates OutOfService
        /// </summary>
        /// <param name="outOfServiceValue">outOfServiceValue</param>
        private void UpdateOutOfService(string outOfServiceValue)
        {
            log.LogMethodEntry(outOfServiceValue);
            MachineAttributeDTO outOfServiceAttrib = GetMachineMachineAttribute(MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE);
            if (outOfServiceAttrib.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)
            {
                outOfServiceAttrib.AttributeValue = outOfServiceValue;
                Save();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Puts the game machine out of service
        /// </summary>
        /// <param name="reason">Reason code for putting the machine out of service</param>
        /// <param name="remarks">Remarks</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void PutOutOfService(string reason, string remarks, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(reason, remarks, sqlTransaction);
            UpdateOutOfService("1");
            string userId = machineUserContext.GetUserId();
            int siteId = machineUserContext.GetSiteId();
            Users user = new Users(machineUserContext, userId);
            UsersDTO userDTO = user.UserDTO;
            MachineAttributeLogDTO machineAttributeLogDTO = new MachineAttributeLogDTO(-1, machineDTO.MachineId, machineUserContext.GetMachineId(),
                machineUserContext.POSMachineName, "PutOutOfService", reason, remarks, true, DateTime.Now.ToUniversalTime(), MachineAttributeLogDTO.UpdateTypes.OUT_OF_SERVICE.ToString());
            MachineAttributeLogBL machineAttributeLogBL = new MachineAttributeLogBL(machineUserContext, machineAttributeLogDTO);
            machineAttributeLogBL.Save(sqlTransaction);

            log.LogMethodExit();
        }

        /// <summary>
        /// GetMachineDTO method
        /// returns MachineDTO object
        /// </summary>
        public MachineDTO GetMachineDTO { get { return machineDTO; } }


        /// <summary>
        /// Bring back the machine from out of service
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// </summary>
        public void BringBackInService(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            UpdateOutOfService("0");
            string userId = machineUserContext.GetUserId();
            int siteId = machineUserContext.GetSiteId();
            Users user = new Users(machineUserContext, userId);
            UsersDTO userDTO = user.UserDTO;
            MachineAttributeLogDTO machineAttributeLogDTO = new MachineAttributeLogDTO(-1, machineDTO.MachineId, machineUserContext.GetMachineId(),
                machineUserContext.POSMachineName, "BringBackInService", "", "", false, DateTime.Now.ToUniversalTime(), MachineAttributeLogDTO.UpdateTypes.IN_TO_SERVICE.ToString());
            MachineAttributeLogBL machineAttributeLogBL = new MachineAttributeLogBL(machineUserContext, machineAttributeLogDTO);
            machineAttributeLogBL.Save(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Added for machine bulkupload validation
        /// </summary>
        public void SaveMachine()
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = Validate();
            if (validationErrorList.Count > 0)
            {
                throw new ValidationException("Validation failed", validationErrorList);
            }
            AllowedMachineNamesListBL allowedMachineNamesListBL = new AllowedMachineNamesListBL(machineUserContext);
            List<KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>> allowedMachinesearchParameters = new List<KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>>();
            allowedMachinesearchParameters.Add(new KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>(AllowedMachineNamesDTO.SearchByParameters.MACHINE_NAME, machineDTO.MachineName.ToString()));
            allowedMachinesearchParameters.Add(new KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>(AllowedMachineNamesDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            AllowedMachineNamesDTO allowedMachineNamesdto = allowedMachineNamesListBL.GetAllowedMachineNamesList(allowedMachinesearchParameters, sqlTransaction).FirstOrDefault(x => x.MachineName == machineDTO.MachineName);
            if (allowedMachineNamesdto != null)
            {
                machineDTO.AllowedMachineID = allowedMachineNamesdto.AllowedMachineId;
            }
            Save();
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the machine details
        /// Checks if the machine id is -1, if it is, 
        ///     then the record does not exist in the database and the machine record is created
        /// If the machine id is not -1, then updates the machine record
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(sqlTransaction);
                bool enableUserEntryOfMachine = ParafaitDefaultContainerList.GetParafaitDefault<bool>(machineUserContext, "ENABLE_USER_ENTRY_OF_MACHINE");
                log.LogVariableState("Default-Enable User Entry of Machine Name", enableUserEntryOfMachine);
                AllowedMachineNamesListBL allowedMachineNamesListBL = new AllowedMachineNamesListBL(machineUserContext);
                List<KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>> allowedMachinesearchParameters = new List<KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>>();
                string userId = machineUserContext.GetUserId();
                int siteId = machineUserContext.GetSiteId();
                string machineAddress = string.Empty;
                AllowedMachineNamesDTO allowedMachineNamesdto = null;
                machineDataHandler = new MachineDataHandler(sqlTransaction);
                if (machineDTO != null && machineDTO.AllowedMachineID > -1)
                {
                    allowedMachinesearchParameters.Add(new KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>(AllowedMachineNamesDTO.SearchByParameters.ALLOWED_MACHINE_ID, machineDTO.AllowedMachineID.ToString()));//Get allowedmachine based on Allowedmachineid
                    allowedMachineNamesdto = allowedMachineNamesListBL.GetAllowedMachineNamesList(allowedMachinesearchParameters, sqlTransaction).FirstOrDefault();

                }
                List<ValidationError> validationErrorList = Validation(allowedMachineNamesdto);
                if (string.IsNullOrWhiteSpace(machineDTO.MachineName))//case machineDTO machine is not passed but allowedmachine name exists  or passed
                {
                    if (allowedMachineNamesdto != null)
                    {
                        machineDTO.MachineName = allowedMachineNamesdto.MachineName;
                    }
                    else
                    {
                        if (enableUserEntryOfMachine)
                        {
                            validationErrorList.Add(new ValidationError("Machine", "Machine Name", MessageContainerList.GetMessage(machineUserContext, 5125, machineDTO.MachineName))); //Machine &1 does not exist in AllowedMachineNames
                        }
                    }
                }
                if (validationErrorList != null && validationErrorList.Count != 0)
                {
                    throw new ValidationException("Validation Failed", validationErrorList);
                }

                if (machineDTO.MachineId < 0)
                {
                    machineAddress = GetMachineAddress(machineDTO.MasterId);
                    machineDTO.MachineAddress = machineAddress;
                    //check for Fk 
                    if (enableUserEntryOfMachine == true)//config Y
                    {
                        if (allowedMachineNamesdto == null && !string.IsNullOrWhiteSpace(machineDTO.MachineName))
                        {
                            allowedMachinesearchParameters.Clear();
                            allowedMachinesearchParameters.Add(new KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>(AllowedMachineNamesDTO.SearchByParameters.MACHINE_NAME, machineDTO.MachineName.ToString()));
                            allowedMachinesearchParameters.Add(new KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>(AllowedMachineNamesDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                            allowedMachineNamesdto = allowedMachineNamesListBL.GetAllowedMachineNamesList(allowedMachinesearchParameters, sqlTransaction).Where(x => x.MachineName == machineDTO.MachineName).FirstOrDefault();
                        }
                        if (allowedMachineNamesdto != null)
                        {
                            GameValidation(allowedMachineNamesdto, machineDTO, machineUserContext);
                            if (allowedMachineNamesdto.IsActive == false)
                            {
                                allowedMachineNamesdto.IsActive = true;
                            }
                            AllowedMachineNamesBL allowedMachineNamesBL = new AllowedMachineNamesBL(machineUserContext, allowedMachineNamesdto);
                            allowedMachineNamesBL.Save(sqlTransaction);
                            machineDTO.AllowedMachineID = allowedMachineNamesdto.AllowedMachineId;
                        }
                        else
                        {
                            allowedMachineNamesdto = new AllowedMachineNamesDTO(-1, machineDTO.GameId, machineDTO.MachineName, true);
                            AllowedMachineNamesBL allowedMachineNamesBL = new AllowedMachineNamesBL(machineUserContext, allowedMachineNamesdto);
                            allowedMachineNamesBL.Save(sqlTransaction);
                            allowedMachineNamesdto = allowedMachineNamesBL.GetAllowedMachineNamesDTO;
                            machineDTO.AllowedMachineID = allowedMachineNamesdto.AllowedMachineId;
                        }
                    }
                    machineDTO = machineDataHandler.InsertMachine(machineDTO, userId, siteId);
                    SaveAndUpdateMachineAttribute(userId, siteId, sqlTransaction);
                    log.Debug("Saving Machine Details");
                    machineDTO.AcceptChanges();
                }
                else if (machineDTO.MachineId >= 0 && (machineDTO.IsChanged || machineDTO.IsChangedRecursive))
                {
                    AllowedMachineNamesDTO allowedMachineNamesDTOExisting = null;
                    if (machineDTO != null)
                    {
                        MachineDTO machineDTOExisting = null;
                        MachineDataHandler machineDataHandler = new MachineDataHandler(sqlTransaction);
                        machineDTOExisting = machineDataHandler.GetMachine(machineDTO.MachineId);
                        if (machineDTOExisting != null)
                        {
                            if (machineDTOExisting != null && machineDTOExisting.AllowedMachineID > -1)
                            {
                                AllowedMachineNamesBL allowedMachineNamesBL = new AllowedMachineNamesBL(machineUserContext, machineDTOExisting.AllowedMachineID, sqlTransaction);
                                allowedMachineNamesDTOExisting = allowedMachineNamesBL.GetAllowedMachineNamesDTO;//get the old Allowedmachinename
                            }
                        }
                    }

                    if (enableUserEntryOfMachine == true)
                    {
                        if (machineDTO.IsActive.ToLower() == "y" && allowedMachineNamesdto!=null && allowedMachineNamesdto.IsActive == false)
                        {
                            allowedMachineNamesdto.IsActive = true;
                        }
                        if (machineDTO.IsActive.ToLower() == "n" && allowedMachineNamesdto!=null && allowedMachineNamesdto.IsActive == true)
                        {
                            allowedMachineNamesdto.IsActive = false;
                        }
                    }
                    if (enableUserEntryOfMachine == false && allowedMachineNamesdto!=null && allowedMachineNamesdto.AllowedMachineId != allowedMachineNamesDTOExisting.AllowedMachineId)
                    {
                        machineDTO.AllowedMachineID = allowedMachineNamesdto.AllowedMachineId;
                        machineDTO.MachineName = allowedMachineNamesdto.MachineName;
                    }
                    if (enableUserEntryOfMachine == true && allowedMachineNamesdto != null && allowedMachineNamesdto.MachineName != machineDTO.MachineName) //for machine name edit
                    {
                        if (!string.IsNullOrWhiteSpace(machineDTO.MachineName))
                        {
                            allowedMachinesearchParameters.Clear();
                            allowedMachinesearchParameters.Add(new KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>(AllowedMachineNamesDTO.SearchByParameters.MACHINE_NAME, machineDTO.MachineName.ToString()));
                            allowedMachinesearchParameters.Add(new KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>(AllowedMachineNamesDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                            AllowedMachineNamesDTO allowedMachineNamesdtos = allowedMachineNamesListBL.GetAllowedMachineNamesList(allowedMachinesearchParameters, sqlTransaction).Where(x => x.MachineName == machineDTO.MachineName).FirstOrDefault();
                            if (allowedMachineNamesdtos != null)
                            {
                                if (allowedMachineNamesdtos.IsActive == false)
                                {
                                    allowedMachineNamesdtos.IsActive = true;
                                }
                                AllowedMachineNamesBL allowedMachineNamesBL = new AllowedMachineNamesBL(machineUserContext, allowedMachineNamesdtos);
                                allowedMachineNamesBL.Save(sqlTransaction);
                                allowedMachineNamesdtos = allowedMachineNamesBL.GetAllowedMachineNamesDTO;
                                machineDTO.AllowedMachineID = allowedMachineNamesdtos.AllowedMachineId;

                            }
                            else
                            {
                                allowedMachineNamesdto.MachineName = machineDTO.MachineName;//with updating machine name ->it updates allowedmachinename's machinename
                            }
                        }
                    }

                    MachineDTO sourceMachineDTO = machineDataHandler.GetMachine(machineDTO.MachineId);
                    Hub hub = new Hub(sourceMachineDTO.MasterId);
                    HubDTO hubDetails = hub.GetHubDTO;
                    /// The below if condition is to check for incoming machine Dto Vs existing machine Dto. It will check whether the master id is same or not
                    /// If master id same, the incoming machine address will be updated. Else, it will generate the a machine address and assigned to incoming machine Dto
                    bool isCorporate = machineUserContext.GetIsCorporate();
                    if (sourceMachineDTO.MasterId != machineDTO.MasterId || (machineDTO.SiteId != siteId && isCorporate))
                    {
                        machineAddress = GetMachineAddress(machineDTO.MasterId);
                        machineDTO.MachineAddress = machineAddress;
                    }
                    else if (hubDetails != null && hubDetails.DirectMode == "Y") // If hub is Direct mode then get the address from DB else if non- directed mode then Machine Address is sent from the UI update the address as it is.
                    {
                        machineDTO.MachineAddress = sourceMachineDTO.MachineAddress;
                    }
                    machineDTO = machineDataHandler.UpdateMachine(machineDTO, userId, siteId);
                    SaveAndUpdateMachineAttribute(userId, siteId, sqlTransaction);
                    machineDTO.AcceptChanges();
                    if (allowedMachineNamesdto != null)
                    {
                        AllowedMachineNamesBL allowedMachineBL = new AllowedMachineNamesBL(machineUserContext, allowedMachineNamesdto);//save-updating machine name for wms it should update allowemachinename table also
                        allowedMachineBL.Save(sqlTransaction);
                    }
                }

                if (machineDTO.MachineCommunicationLogDTO != null)
                {
                    machineDTO.MachineCommunicationLogDTO.MachineId = machineDTO.MachineId;
                    MachineCommunicationLogBL machineCommunicationLogBL = new MachineCommunicationLogBL(machineUserContext, machineDTO.MachineCommunicationLogDTO);
                    machineCommunicationLogBL.Save(sqlTransaction);
                }
                log.LogMethodExit();
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Validation Exception -" + ex.Message);
                throw;

            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
        private GameDTO GetGame(int gameId, ExecutionContext executionContext, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            GameDTO gameDTO = null;
            if (gameId >= 0)
            {
                Game machineGame = new Game(gameId, executionContext, sqlTransaction);
                gameDTO = machineGame.GetGameDTO;
            }
            log.LogMethodExit(gameDTO);
            return gameDTO;
        }
        private void GameValidation(AllowedMachineNamesDTO allowedMachineNamesDTO, MachineDTO machineDTO, ExecutionContext machineUserContext)
        {
            log.LogMethodEntry(allowedMachineNamesDTO, machineDTO, machineUserContext);
            if (machineUserContext != null && allowedMachineNamesDTO != null && machineDTO != null)
            {
                if (allowedMachineNamesDTO.GameId != machineDTO.GameId)
                {
                    GameDTO allowedmachineGameDTO = GetGame(allowedMachineNamesDTO.GameId, machineUserContext, sqlTransaction);
                    if (allowedmachineGameDTO == null || allowedmachineGameDTO.GameId < 0)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 2196, MessageContainerList.GetMessage(machineUserContext, "Game"), allowedmachineGameDTO.GameId));//Unable to find a &1 with id &2
                    }
                    log.LogVariableState("AllowedmachineGameDTO", allowedmachineGameDTO);
                    GameDTO machineGameDTO = GetGame(machineDTO.GameId, machineUserContext, sqlTransaction);
                    log.LogVariableState("MachineGameDTO", machineGameDTO);
                    if (machineGameDTO == null || machineGameDTO.GameId < 0)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 2196, MessageContainerList.GetMessage(machineUserContext, "Game"), machineGameDTO.GameId));//Unable to find a &1 with id &2
                    }
                    throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 5131, allowedMachineNamesDTO.MachineName, allowedmachineGameDTO.GameName, machineGameDTO.GameName));//Allowed Machine "&1" is set up for Game "&2". But selected game is "&3"
                }
            }
            log.LogMethodExit();
        }
        private void SaveAndUpdateMachineAttribute(string userId, int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(userId, siteId, sqlTransaction);
            // Saves the GameMachineAttributes and updates the DBAudit Table
            if (machineDTO.GameMachineAttributes != null && machineDTO.GameMachineAttributes.Any())
            {
                MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
                MachineAttributeDTO machineAttributeDTO = new MachineAttributeDTO();
                foreach (MachineAttributeDTO currAttribute in machineDTO.GameMachineAttributes)
                {
                    machineAttributeDTO = currAttribute;
                    if (machineAttributeDTO.AttributeId == -1 && machineAttributeDTO.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)
                    {
                        machineAttributeDTO = machineAttributeDataHandler.InsertMachineAttribute(machineAttributeDTO, MachineAttributeDTO.AttributeContext.MACHINE, machineDTO.MachineId, userId, siteId);
                        if (!string.IsNullOrEmpty(machineAttributeDTO.Guid))
                        {
                            AuditLog auditLog = new AuditLog(machineUserContext);
                            auditLog.AuditTable("GameProfileattributevalues", machineAttributeDTO.Guid, sqlTransaction);
                        }
                        machineAttributeDTO.AcceptChanges();
                    }
                    else
                    {
                        if (machineAttributeDTO.IsChanged == true && machineAttributeDTO.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE) //11-Jan-2016 - Checking for context
                        {
                            machineAttributeDTO = machineAttributeDataHandler.UpdateMachineAttribute(machineAttributeDTO, MachineAttributeDTO.AttributeContext.MACHINE, machineDTO.MachineId, userId, siteId);
                            if (!string.IsNullOrEmpty(machineAttributeDTO.Guid))
                            {
                                AuditLog auditLog = new AuditLog(machineUserContext);
                                auditLog.AuditTable("GameProfileattributevalues", machineAttributeDTO.Guid, sqlTransaction);
                            }
                            machineAttributeDTO.AcceptChanges();
                        }
                    }

                }
            }
            if (!string.IsNullOrEmpty(machineDTO.Guid))
            {
                AuditLog auditLog = new AuditLog(machineUserContext);
                auditLog.AuditTable("machines", machineDTO.Guid, sqlTransaction);
            }
            log.LogMethodExit();

        }

        /// <summary>
        // Validates the Machines returns validation errors if any of the field values not not valid.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate()
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ValidationError validationError = new ValidationError();
            validationError = ValidateStringField("Machine", "MACHINENAME", "MachineName", machineDTO.MachineName, "Machine Name");
            if (validationError != null)
            {
                validationErrorList.Add(validationError);
            }
            validationError = ValidateStringField("Machine", "MACHINEADDRESS", "MachineAddress", machineDTO.MachineAddress, "Machine Address");
            if (validationError != null)
            {
                validationErrorList.Add(validationError);
            }
            validationError = ValidateForeignKeyField("Machine", "GAMENAME", "GameId", machineDTO.GameId, "Game Name");
            if (validationError != null)
            {
                validationErrorList.Add(validationError);
            }
            validationError = ValidateIsActiveFeild("Machine", "ISACTIVE", "isActive", machineDTO.IsActive, "IsActive");
            if (validationError != null)
            {
                validationErrorList.Add(validationError);
            }
            if (machineDTO.GameMachineAttributes != null)
            {
                GameSystem gameSystem = new GameSystem("GAMES", machineDTO.GameId, machineUserContext);
                List<MachineAttributeDTO> gamesAttributeDTOList = gameSystem.GetMachineAttributes();
                foreach (MachineAttributeDTO machineAttributeDTO in machineDTO.GameMachineAttributes)
                {
                    foreach (MachineAttributeDTO gameAttributeDTO in gamesAttributeDTOList)
                    {

                        if (machineAttributeDTO.AttributeName == gameAttributeDTO.AttributeName && gameAttributeDTO.IsFlag == "Y" && !string.IsNullOrEmpty(machineAttributeDTO.AttributeValue) && Convert.ToInt32(machineAttributeDTO.AttributeValue) > 1)
                        {
                            validationError = new ValidationError("Machine", machineAttributeDTO.AttributeName.ToString(), machineAttributeDTO.AttributeName.ToString() + " " + MessageContainerList.GetMessage(machineUserContext, 1638));
                            validationErrorList.Add(validationError);
                            break;
                        }
                    }
                }
            }
            return validationErrorList;
        }

        /// <summary>
        /// Validates the Machines returns validation errors if any of the machine address and mac address is not not valid.
        /// </summary>
        /// <returns></returns>
        private List<ValidationError> Validation(AllowedMachineNamesDTO allowedMachineNamesdto = null)
        {
            log.LogMethodEntry(allowedMachineNamesdto);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            bool enableUserEntryOfMachine = ParafaitDefaultContainerList.GetParafaitDefault<bool>(machineUserContext, "ENABLE_USER_ENTRY_OF_MACHINE");
            if (enableUserEntryOfMachine == false)
            {
                if (machineDTO.AllowedMachineID == -1 || allowedMachineNamesdto == null)//case only for game haveing AllowedMachine set up
                {
                    log.Error("Allowed Machine name does not exist");
                    validationErrorList.Add(new ValidationError("Machine", "AllowedMachineID", MessageContainerList.GetMessage(machineUserContext, 5125, machineDTO.MachineName)));//Machine &1 does not exist in AllowedMachineNames
                }
                if (machineDTO.IsActive.ToLower() == "y" && allowedMachineNamesdto != null && allowedMachineNamesdto.IsActive == false)//case only for game haveing AllowedMachine set up
                {
                    log.Error("Allowed Machine name is inactive");
                    validationErrorList.Add(new ValidationError("Machine", "AllowedMachineID", MessageContainerList.GetMessage(machineUserContext, 5126, machineDTO.MachineName))); //AllowedMachineName & 1 is inactive.Please activate it from management studio and proceed with activating machine.' 
                }
            }
            GameValidation(allowedMachineNamesdto, machineDTO, machineUserContext);
            if (machineDTO.MachineId != -1 && machineDTO.ReferenceMachineId != -1 && machineDTO.MachineId == machineDTO.ReferenceMachineId)
            {
                validationErrorList.Add(new ValidationError("Machine", "Reference Machine", MessageContainerList.GetMessage(machineUserContext, 4791)));
            }
            List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
            searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.SITE_ID, Convert.ToString(machineUserContext.GetSiteId())));
            MachineDataHandler machineDataHandler = new MachineDataHandler();
            MachineList machineListBL = new MachineList(machineUserContext);
            List<MachineDTO> machineDTOList = machineDataHandler.GetMachineList(searchParameters);
            if (machineDTOList != null && machineDTOList.Count != 0)
            {
                var isMachineNameExist = machineDTOList.Find(m => m.MachineName.ToLower() == machineDTO.MachineName.ToLower());
                var isMachineAddressExist = machineDTOList.Find(m => m.MachineAddress == machineDTO.MachineAddress);
                List<MachineDTO> updatedMachineDTOList = machineDTOList.Where(m => !String.IsNullOrEmpty(machineDTO.MacAddress) && m.MacAddress.ToLower() == machineDTO.MacAddress.ToLower()).ToList();
                if (machineDTO.MachineId < 0)
                {
                    if (isMachineAddressExist != null)
                    {
                        validationErrorList.Add(new ValidationError("Machine", "Machine Address", MessageContainerList.GetMessage(machineUserContext, 1913))); /// Please enter unique machine address
                    }
                    if (updatedMachineDTOList != null && updatedMachineDTOList.Count != 0)
                    {
                        validationErrorList.Add(new ValidationError("Machine", "Mac Address", MessageContainerList.GetMessage(machineUserContext, 1912, updatedMachineDTOList[0].MachineName))); // Mac address already exists for &1, please enter a unique mac address or change the mac address of the mentioned machine.
                    }
                    if (isMachineNameExist != null)
                    {
                        validationErrorList.Add(new ValidationError("Machine", "Machine Name", MessageContainerList.GetMessage(machineUserContext, 5233))); //Please enter unique machine name.
                    }

                }
                else
                {
                    if (updatedMachineDTOList != null && updatedMachineDTOList.Count != 0)
                    {
                        MachineDTO machineDTOExist = machineDTOList.Find(m => m.MacAddress.ToLower() == machineDTO.MacAddress.ToLower() && m.MachineId != machineDTO.MachineId);
                        if (machineDTOExist != null)
                        {
                            validationErrorList.Add(new ValidationError("Machine", "Mac Address", MessageContainerList.GetMessage(machineUserContext, 1912, updatedMachineDTOList[0].MachineName))); /// Mac address already exists, Please enter unique mac address
                        }
                    }
                    if (isMachineNameExist != null)
                    {
                        MachineDTO machineDTONameExist = machineDTOList.Find(m => m.MachineName.ToLower() == machineDTO.MachineName.ToLower() && m.MachineId != machineDTO.MachineId);
                        if (machineDTONameExist != null)
                        {
                            validationErrorList.Add(new ValidationError("Machine", "Machine Name", MessageContainerList.GetMessage(machineUserContext, 5233))); //Please enter unique machine name.
                        }
                    }
                }
                List<MachineDTO> machineDTOListIPExist = machineDTOList.Where(m => !string.IsNullOrWhiteSpace(machineDTO.IPAddress) 
                && m.IPAddress == machineDTO.IPAddress &&m.MachineId != machineDTO.MachineId).ToList();
                if (machineDTOListIPExist != null && machineDTOListIPExist.Any())
                {
                    validationErrorList.Add(new ValidationError("Machine", "Ip Address", MessageContainerList.GetMessage(machineUserContext, 5210, machineDTOListIPExist[0].MachineName))); //IP address already exists for &1, please enter a unique ip address or change the ip address of the mentioned machine.
                }
                if (machineDTO.ReferenceMachineId != -1 && machineDTOList.Find(m => m.MachineId == machineDTO.ReferenceMachineId).ReferenceMachineId != -1)
                {
                    validationErrorList.Add(new ValidationError("Machine", "Reference Machine", MessageContainerList.GetMessage(machineUserContext, 2077)));
                }
                if (machineDTOList.Exists(m => m.ReferenceMachineId == machineDTO.MachineId && m.ReferenceMachineId != -1))
                {
                    validationErrorList.Add(new ValidationError("Machine", "Reference Machine", MessageContainerList.GetMessage(machineUserContext, 4798)));
                }
                if (!string.IsNullOrEmpty(machineDTO.QRPlayIdentifier))
                {
                    bool isQRPlayIdentifierExists =
                        machineDTOList.Exists(x => !string.IsNullOrEmpty(x.QRPlayIdentifier)
                                              && x.QRPlayIdentifier == machineDTO.QRPlayIdentifier
                                              && x.IsActive == "Y"
                                              && x.MachineId != machineDTO.MachineId);
                    if (isQRPlayIdentifierExists)
                    {
                        validationErrorList.Add(new ValidationError("Machine", "Duplicate QR Play", MessageContainerList.GetMessage(machineUserContext, 3084)));
                    }
                }
                if (machineDTOList != null && machineDTOList.Any())
                {
                    if (allowedMachineNamesdto != null)
                    {
                        var allowedmachineExist = machineDTOList.Find(x => x.AllowedMachineID == allowedMachineNamesdto.AllowedMachineId);
                        if (allowedmachineExist != null && machineDTO.MachineId < 0)//machine insertion if the allowedmachine is already used by otther machine
                        {
                            validationErrorList.Add(new ValidationError("Machine", "Machine Name", MessageContainerList.GetMessage(machineUserContext, 5160, allowedMachineNamesdto.MachineName))); //AllowedMachineName &1 is used on another machine.
                        }
                    }
                }
            }
            return validationErrorList;
        }

        /// <summary>
        /// Validate method to check string value
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="defaultValueName">defaultValueName</param>
        /// <param name="attributeName">attributeName</param>
        /// <param name="attributeValue">attributeValue</param>
        /// <param name="displayName">displayName</param>
        /// <returns>validationError</returns>
        private ValidationError ValidateStringField(string entity, string defaultValueName, string attributeName, string attributeValue, string displayName)
        {
            log.LogMethodEntry(entity, defaultValueName, attributeName, attributeValue, displayName);
            ValidationError validationError = null;
            if (string.IsNullOrWhiteSpace(attributeValue))
            {
                string errorMessage = MessageContainerList.GetMessage(machineUserContext, 249, MessageContainerList.GetMessage(machineUserContext, displayName));
                validationError = new ValidationError(entity, attributeName, errorMessage);
            }
            log.LogMethodExit(validationError);
            return validationError;
        }

        /// <summary>
        /// Checking isActive feild for 0 or 1
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="defaultValueName">defaultValueName</param>
        /// <param name="attributeName">attributeName</param>
        /// <param name="attributeValue">attributeValue</param>
        /// <param name="displayName">validationError</param>
        /// <returns></returns>
        private ValidationError ValidateIsActiveFeild(string entity, string defaultValueName, string attributeName, string attributeValue, string displayName)
        {
            log.LogMethodEntry(entity, defaultValueName, attributeName, attributeValue, displayName);
            ValidationError validationError = null;
            if (attributeValue == "1" || attributeValue == "0")
            {
                string errorMessage = MessageContainerList.GetMessage(machineUserContext, 1870, MessageContainerList.GetMessage(machineUserContext, displayName));
                validationError = new ValidationError(entity, attributeName, errorMessage);
            }
            log.LogMethodExit(validationError);
            return validationError;
        }

        /// <summary>
        /// Validate Flags
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="defaultValueName">defaultValueName</param>
        /// <param name="attributeName">attributeName</param>
        /// <param name="attributeValue">attributeValue</param>
        /// <param name="displayName">displayName</param>
        /// <returns>validationError</returns>
        private ValidationError ValidateForeignKeyField(string entity, string defaultValueName, string attributeName, int attributeValue, string displayName)
        {
            log.LogMethodEntry(entity, defaultValueName, attributeName, attributeValue, displayName);
            ValidationError validationError = null;
            if (attributeValue == -1)
            {
                string errorMessage = MessageContainerList.GetMessage(machineUserContext, 249, MessageContainerList.GetMessage(machineUserContext, displayName));
                validationError = new ValidationError(entity, attributeName, errorMessage);
            }
            log.LogMethodExit(validationError);
            return validationError;
        }

        /// <summary>
        /// Gets Machine Address
        /// </summary>
        /// <param name="masterId">masterId</param>
        /// <returns>address</returns>
        public string GetMachineAddress(int masterId)
        {
            log.LogMethodEntry(masterId);
            string prefix = "99";
            string suffix = "01";
            string address = null;
            try
            {
                MachineDTO machineAddress = machineDataHandler.GetMaxMachineByAddress(masterId, machineUserContext.GetSiteId());
                if (machineAddress != null)
                {
                    if (machineAddress.MachineAddress.Length >= 4)
                    {
                        prefix = machineAddress.MachineAddress.Substring(0, 2);
                        suffix = machineAddress.MachineAddress.Substring(2);
                    }
                    else
                    {
                        prefix = "99";
                        suffix = "01";
                    }
                    if (masterId > 0)
                    {
                        Hub hub = new Hub(masterId);
                        HubDTO hubDetails = hub.GetHubDTO;
                        if (hubDetails != null && hubDetails.DirectMode == "N")
                        {
                            if (machineDTO != null && machineDTO.MachineAddress == null)
                            {
                                log.LogMethodExit();
                                throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 2614));//Machine address is null for the selected non direct mode hub. Please specify the machine address
                            }
                            else
                            {
                                log.LogMethodExit(machineDTO.MachineAddress);
                                return address = machineDTO.MachineAddress;
                            }
                        }
                    }
                }
                else
                {
                    Hub hub = new Hub(masterId);
                    HubDTO hubDetails = hub.GetHubDTO;
                    if (hubDetails != null && hubDetails.DirectMode != "N")
                    {
                        prefix = hubDetails.Address.ToString();
                        suffix = "00";
                    }
                    else
                    {
                        if (machineDTO != null && machineDTO.MachineAddress == null)
                        {
                            log.LogMethodExit();
                            throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 2614));
                        }
                        else
                        {
                            log.LogMethodExit(machineDTO.MachineAddress);
                            return address = machineDTO.MachineAddress;
                        }
                    }

                }
                address = prefix + (Convert.ToInt32(suffix) + 1).ToString().PadLeft(2, '0');
                if (address.Length > 4)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 1853));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
            log.LogMethodExit(address);
            return address;
        }

        public MachineDTO SetMachineDTO
        {
            set { machineDTO = value; }
        }

        /// <summary>
        /// Return the active theme Id to be displayed on the machine.
        /// </summary>
        /// <returns>Return the active theme Id</returns>
        public int GetActiveDisplayThemeId()
        {
            log.LogMethodEntry();
            int result = -1;
            MachineDataHandler machineDataHandler = new MachineDataHandler(sqlTransaction);
            result = machineDataHandler.GetActiveDisplayThemeId(machineDTO.MachineId, machineDTO.GameId, GetGameDTO.GameProfileId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Deletes the machine
        /// </summary>
        /// <param name="machineId">machineId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        internal void DeleteMachine(int machineId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(machineId, sqlTransaction);
            try
            {
                this.machineDataHandler = new MachineDataHandler(sqlTransaction);
                machineDataHandler.DeleteMachine(machineId);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}


/// <summary>
/// Manages the list of machines
/// </summary>
public class MachineList
{
    private List<MachineDTO> machineDTOList;
    private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private readonly ExecutionContext executionContext;

    /// <summary>
    /// Default constructor of MachineList class.Does MachineDataHandler object initialization
    /// </summary>
    public MachineList()
    {
        log.LogMethodEntry();
        this.executionContext = ExecutionContext.GetExecutionContext();
        this.machineDTOList = null;
        log.LogMethodExit();
    }

    /// <summary>
    /// parameterized Constructor with executionContext
    /// <param name="executionContext">executionContext</param>
    /// </summary>
    public MachineList(ExecutionContext executionContext)
    {
        log.LogMethodEntry(executionContext);
        this.executionContext = executionContext;
        this.machineDTOList = null;
        log.LogMethodExit();
    }

    /// <summary>
    /// parameterized Constructor with machineDtoList and executionContext
    /// </summary>
    /// <param name="machineDTOList">machineDTOList</param>
    /// <param name="executionContext">executionContext</param>
    public MachineList(ExecutionContext executionContext, List<MachineDTO> machineDTOList)
    {
        log.LogMethodEntry(machineDTOList, executionContext);
        this.executionContext = executionContext;
        this.machineDTOList = machineDTOList;
        log.LogMethodExit();
    }

    /// <summary>
    /// Gets the machine list matching with search key
    /// </summary>
    /// <param name="searchParameters">Hold the values [MachineDTO.SearchByGameParameters,string] type as search key</param>
    /// <param name="loadAttributes">loadAttributes</param>
    /// <param name="sqlTransaction">sqlTransaction</param>
    public List<MachineDTO> GetMachineList(List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters,
        bool loadAttributes = true, SqlTransaction sqlTransaction = null, int currentPage = 0, int pageSize = 0)
    {
        log.LogMethodEntry(searchParameters, loadAttributes, sqlTransaction);
        MachineDataHandler machineDataHandler = new MachineDataHandler(sqlTransaction);
        List<MachineDTO> machineList = machineDataHandler.GetMachineDTOList(searchParameters, currentPage, pageSize); // Pagination implemented
        if (machineList != null && machineList.Any() && loadAttributes)
        {
            MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
            foreach (MachineDTO machine in machineList)
            {
                List<MachineAttributeDTO> machineAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.MACHINE, machine.MachineId, executionContext.GetSiteId());
                machine.SetAttributeList(machineAttributes);
            }
        }
        HubList hubList = new HubList(executionContext);
        List<KeyValuePair<HubDTO.SearchByHubParameters, string>> hubSearchParameters = new List<KeyValuePair<HubDTO.SearchByHubParameters, string>>
            {
                new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.IS_ACTIVE, "Y"),
                new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.SITE_ID, executionContext.GetSiteId().ToString())
            };
        List<HubDTO> hubDTOList = hubList.GetHubSearchList(hubSearchParameters);
        MachineBuilderBL machineBuilderBL = new MachineBuilderBL(executionContext);
        if (machineList != null && machineList.Any())
        {
            foreach (MachineDTO machineDTO in machineList)
            {
                machineBuilderBL.UpdateMachineListToGetHubName(machineDTO, hubDTOList);
                if (loadAttributes)
                {
                    MachineCommunicationLogList machineCommunicationLogList = new MachineCommunicationLogList(executionContext);
                    List<KeyValuePair<MachineCommunicationLogDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<MachineCommunicationLogDTO.SearchByParameters, string>>();
                    searchParams.Add(new KeyValuePair<MachineCommunicationLogDTO.SearchByParameters, string>(MachineCommunicationLogDTO.SearchByParameters.MACHINE_ID, machineDTO.MachineId.ToString()));
                    List<MachineCommunicationLogDTO> machineCommunicationLogDTOList = machineCommunicationLogList.GetMachineCommunicationLogDTOList(searchParams);
                    if (machineCommunicationLogDTOList != null && machineCommunicationLogDTOList.Count > 0)
                        machineDTO.MachineCommunicationLogDTO = machineCommunicationLogDTOList[0];
                    else
                    {
                        machineDTO.MachineCommunicationLogDTO = new MachineCommunicationLogDTO();
                        machineDTO.MachineCommunicationLogDTO.MachineId = machineDTO.MachineId;
                    }
                }
            }
            machineBuilderBL.UpdateMachineDTOListToGetGameName(machineList);
        }
        log.LogMethodExit(machineList);
        return machineList;
    }

    /// <summary>
    /// Gets the machine matching with id
    /// </summary>
    /// <param name="machineId">Holds the machine id</param>
    /// <param name="sqlTransaction">sqlTransaction</param>
    public MachineDTO GetMachine(int machineId, SqlTransaction sqlTransaction = null)
    {
        log.LogMethodEntry(machineId, sqlTransaction);
        MachineDataHandler machineDataHandler = new MachineDataHandler(sqlTransaction);
        MachineDTO machineDTO = machineDataHandler.GetMachine(machineId);
        log.LogMethodExit(machineDTO);
        return machineDTO;
    }

    /// <summary>
    /// Gets Machine List matching machineSearchParams
    /// </summary>
    /// <param name="machineSearchParams"></param>
    /// <param name="sqlTransaction">sqlTransaction</param>
    /// <returns>machineList</returns>
    public List<MachineDTO> GetMachineList(MachineParams machineSearchParams, SqlTransaction sqlTransaction = null)
    {
        log.LogMethodEntry(machineSearchParams);
        List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters = BuildMachineSearchParameters(machineSearchParams);
        MachineDataHandler machineDataHandler = new MachineDataHandler(sqlTransaction);
        List<MachineDTO> machineList = machineDataHandler.GetMachineList(searchParameters);
        if (machineList != null && machineList.Any())
        {
            MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
            foreach (MachineDTO machine in machineList)
            {
                List<MachineAttributeDTO> machineAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.MACHINE, machine.MachineId, executionContext.GetSiteId());
                machine.SetAttributeList(machineAttributes);
            }
        }
        HubList hubList = new HubList(executionContext);
        List<KeyValuePair<HubDTO.SearchByHubParameters, string>> hubSearchParameters = new List<KeyValuePair<HubDTO.SearchByHubParameters, string>>
                {
                    new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.IS_ACTIVE, "Y"),
                    new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.SITE_ID, executionContext.GetSiteId().ToString())
                };
        List<HubDTO> hubDTOList = hubList.GetHubSearchList(hubSearchParameters);
        MachineBuilderBL machineBuilderBL = new MachineBuilderBL(executionContext);
        if (machineList != null && machineList.Any())
        {
            foreach (MachineDTO machineDto in machineList)
            {
                machineBuilderBL.UpdateMachineListToGetHubName(machineDto, hubDTOList);
            }
        }
        machineBuilderBL.UpdateMachineDTOListToGetGameName(machineList);
        if (machineList != null &&
           machineList.Count > 0 &&
           machineSearchParams.CalculateActiveTheme)
        {
            foreach (var machineDTO in machineList)
            {
                Machine machine = new Machine(machineDTO);
                machineDTO.ActiveDisplayThemeId = machine.GetActiveDisplayThemeId();
                machineDTO.AcceptChanges();
            }
        }
        log.LogMethodExit(machineList);
        return machineList;
    }



    /// <summary>
    ///Takes MachineParams as parameter
    /// </summary>
    /// <param name="machineSearchParams">Hold the values [MachineDTO.SearchByGameParameters,string] type as search key</param>
    /// <returns>Returns List KeyValuePair(MachineDTO.SearchByMachineParameters, string) by converting MachineParams</returns>
    public List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> BuildMachineSearchParameters(MachineParams machineSearchParams)
    {
        log.LogMethodEntry(machineSearchParams);
        List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> mSearchParams = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
        if (machineSearchParams == null)
        {
            mSearchParams.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.MACHINE_NAME, ""));
            mSearchParams.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.IS_ACTIVE, "Y"));
        }
        else
        {
            if (machineSearchParams.GameId != -1)
                mSearchParams.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.GAME_ID, Convert.ToString(machineSearchParams.GameId)));
            if (!String.IsNullOrEmpty(machineSearchParams.MachineName))
                mSearchParams.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.MACHINE_NAME, machineSearchParams.MachineName));
            if (machineSearchParams.ActiveFlag != -1)
                mSearchParams.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.IS_ACTIVE, (machineSearchParams.ActiveFlag == 1) ? ("Y") : ("N")));
            if (string.IsNullOrWhiteSpace(machineSearchParams.MacAddress) == false)
            {
                mSearchParams.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.MACADDRESS, machineSearchParams.MacAddress));
            }
        }
        log.LogMethodExit(mSearchParams);
        return mSearchParams;
    }



    /// <summary>
    /// Gets the list of out of service machines
    /// </summary>
    /// <returns>MachineDTO list of out of service machines</returns>
    /// <param name="sqlTransaction">sqlTransaction</param>
    public List<MachineDTO> GetOutOfServiceMachines(SqlTransaction sqlTransaction = null)
    {
        log.LogMethodEntry(sqlTransaction);
        MachineDataHandler machineDataHandler = new MachineDataHandler(sqlTransaction);
        List<MachineDTO> machineDTOs = new List<MachineDTO>();
        machineDTOs = machineDataHandler.GetOutOfServiceMachines(executionContext.GetSiteId());
        MachineBuilderBL machineBuilderBL = new MachineBuilderBL(executionContext);
        if (machineDTOs != null && machineDTOs.Count > 0)
        {
            HubList hubList = new HubList(executionContext);
            List<KeyValuePair<HubDTO.SearchByHubParameters, string>> hubSearchParameters = new List<KeyValuePair<HubDTO.SearchByHubParameters, string>>
                {
                    new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.IS_ACTIVE, "Y"),
                    new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.SITE_ID, executionContext.GetSiteId().ToString())
                };
            List<HubDTO> hubDTOList = hubList.GetHubSearchList(hubSearchParameters);
            foreach (MachineDTO machineDto in machineDTOs)
            {
                machineBuilderBL.UpdateMachineListToGetHubName(machineDto, hubDTOList);
            }
            machineBuilderBL.UpdateMachineDTOListToGetGameName(machineDTOs);
        }
        log.LogMethodExit(machineDTOs);
        return machineDTOs;
    }

    /// <summary>
    /// Gets the list of machines where there are no game plays and have been marked out of service 
    /// </summary>
    /// <returns>MachineDTO typed list of machine marked as game play out of service machine</returns>
    public List<MachineDTO> GetNoGamePlayMarkedOutOfServiceMachines(SqlTransaction sqlTransaction = null)
    {
        log.LogMethodEntry(sqlTransaction);
        MachineDataHandler machineDataHandler = new MachineDataHandler(sqlTransaction);
        List<MachineDTO> machineDTOs = new List<MachineDTO>();
        machineDTOs = machineDataHandler.GetNoGamePlayMarkedOutOfServiceMachines(executionContext.GetSiteId());
        log.LogMethodExit(machineDTOs);
        return machineDTOs;
    }

    /// <summary>
    /// Gets the list of machines where there are no game plays but have not been marked as out of service machines
    /// </summary>
    /// <param name="sqlTransaction">sqlTransaction</param>
    /// <returns>MachineDTO list</returns>
    public List<MachineDTO> GetNoGamePlayNotMarkedOutOfServiceMachines(SqlTransaction sqlTransaction = null)
    {
        log.LogMethodEntry(sqlTransaction);
        MachineDataHandler machineDataHandler = new MachineDataHandler(sqlTransaction);
        List<MachineDTO> machineDTOs = new List<MachineDTO>();
        machineDTOs = machineDataHandler.GetNoGamePlayNotMarkedOutOfServiceMachines(executionContext.GetSiteId());
        log.LogMethodExit(machineDTOs);
        return machineDTOs;
    }

    /// <summary>
    /// Gets the machine list with ticket dispensing issues
    /// </summary>
    /// <param name="sqlTransaction">sqlTransaction</param>
    /// <returns>MachineDTO list</returns>
    public List<MachineDTO> GetMachinesWithTicketDispensingIssues(SqlTransaction sqlTransaction = null)
    {
        log.LogMethodEntry(sqlTransaction);
        MachineDataHandler machineDataHandler = new MachineDataHandler(sqlTransaction);
        List<MachineDTO> machineDTOs = new List<MachineDTO>();
        machineDTOs = machineDataHandler.GetMachinesWithTicketDispensingIssues(executionContext.GetSiteId());
        log.LogMethodExit(machineDTOs);
        return machineDTOs;
    }

    /// <summary>
    /// Get machines with no coin drops
    /// </summary>
    /// <param name="sqlTransaction">sqlTransaction</param>
    /// <returns>MachineDTO list</returns>
    public List<MachineDTO> GetMachinesWithNoCoinDrops(SqlTransaction sqlTransaction = null)
    {
        log.LogMethodEntry(sqlTransaction);
        MachineDataHandler machineDataHandler = new MachineDataHandler(sqlTransaction);
        List<MachineDTO> machineDTOs = new List<MachineDTO>();
        machineDTOs = machineDataHandler.GetMachinesWithNoCoinDrops(executionContext.GetSiteId());
        log.LogMethodExit(machineDTOs);
        return machineDTOs;
    }

    /// <summary>
    /// Get machines with one coin drop
    /// </summary>
    /// <param name="sqlTransaction">sqlTransaction</param>
    /// <returns>MachineDTO list</returns>
    public List<MachineDTO> GetMachinesWithOneCoinDrop(SqlTransaction sqlTransaction = null)
    {
        log.LogMethodEntry(sqlTransaction);
        MachineDataHandler machineDataHandler = new MachineDataHandler(sqlTransaction);
        List<MachineDTO> machineDTOs = new List<MachineDTO>();
        machineDTOs = machineDataHandler.GetMachinesWithOneCoinDrop(executionContext.GetSiteId());
        log.LogMethodExit(machineDTOs);
        return machineDTOs;
    }

    /// <summary>
    /// Get machines with two coin drops
    /// </summary>
    /// <param name="sqlTransaction">sqlTransaction</param>
    /// <returns>MachineDTO list</returns>
    public List<MachineDTO> GetMachinesWithTwoCoinDrops(SqlTransaction sqlTransaction = null)
    {
        log.LogMethodEntry(sqlTransaction);
        MachineDataHandler machineDataHandler = new MachineDataHandler(sqlTransaction);
        List<MachineDTO> machineDTOs = new List<MachineDTO>();
        machineDTOs = machineDataHandler.GetMachinesWithTwoCoinDrops(executionContext.GetSiteId());
        log.LogMethodExit(machineDTOs);
        return machineDTOs;
    }

    /// <summary>
    /// Get machines with that have not been mapped to inventory locations
    /// </summary>
    /// <returns>MachineDTO list</returns>
    public List<MachineDTO> GetNonInventoryLocationMachines(SqlTransaction sqlTransaction = null)
    {
        log.LogMethodEntry(sqlTransaction);
        MachineDataHandler machineDataHandler = new MachineDataHandler(sqlTransaction);
        List<MachineDTO> machineDTOs = new List<MachineDTO>();
        machineDTOs = machineDataHandler.GetNonInventoryLocationMachines();
        log.LogMethodExit(machineDTOs);
        return machineDTOs;
    }
    /// <summary>
    /// Get machines with that have not been mapped to inventory locations
    /// </summary>
    /// <returns>MachineDTO list</returns>
    public List<MachineDTO> GetNonInventoryLocationMachines(int siteID, SqlTransaction sqlTransaction = null)
    {
        log.LogMethodEntry(siteID, sqlTransaction);
        MachineDataHandler machineDataHandler = new MachineDataHandler(sqlTransaction);
        List<MachineDTO> machineDTOs = new List<MachineDTO>();
        machineDTOs = machineDataHandler.GetNonInventoryLocationMachines(siteID);
        log.LogMethodExit(machineDTOs);
        return machineDTOs;
    }

    /// <summary>
    /// Returns MachineDTO of the machine matching the macAddress parameter
    /// </summary>
    /// <returns> MachineDTO </returns>
    public MachineDTO GetGameMachine(string macAddress)
    {
        log.LogMethodEntry(macAddress);
        try
        {
            MachineParams machineParams = new MachineParams(-1, "", 1, macAddress);
            List<MachineDTO> machinesList = GetMachineList(BuildMachineSearchParameters(machineParams));

            if (machinesList == null)
                throw new Exception("Game machine is not setup in the Parafait system");
            else
                if (machinesList.Count > 1)
                throw new Exception("Multiple game machines setup with the same address");
            else
            {
                MachineDTO machineDTO = machinesList[0];
                log.LogMethodExit(machineDTO);
                return machineDTO;
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
            throw;
        }
    }
    /// <summary>
    /// Save or update the machines for WebManagement Studio
    /// The Incoming list will have MachineId as negative and positive IDs
    /// -1, -2 represents for records which are added newly. 
    /// The Positive MachineId will be for the records which are edited and IsChanged=true
    /// </summary>
    public void SaveUpdateMachinesList(SqlTransaction sqlTransaction = null)
    {
        log.LogMethodEntry();
        try
        {
            log.LogMethodEntry();
            if (machineDTOList != null && machineDTOList.Any())
            {
                foreach (MachineDTO machineDTO in machineDTOList)
                {
                    Machine machine = new Machine(executionContext);

                    /// Below condition is to check whether machine is having child machines or not using the ReferenceMachineId and the value is machineId
                    /// If exists, set the status to the child machines as per the status of the parent machine(example : Y or T).

                    if (machineDTO.MachineId > 0 && machineDTO.IsChanged == true)
                    {
                        List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
                        searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.REFERENCE_MACHINE_ID, Convert.ToString(machineDTO.MachineId)));

                        MachineList machineListBL = new MachineList(executionContext);
                        List<MachineDTO> machineList = machineListBL.GetMachineList(searchParameters, true, sqlTransaction);
                        if (machineList != null)
                        {
                            foreach (MachineDTO referenceMachineDTO in machineList)
                            {
                                if (referenceMachineDTO.IsActive != "N")
                                {
                                    referenceMachineDTO.IsActive = machineDTO.IsActive;
                                    machine.SetMachineDTO = referenceMachineDTO;
                                    machine.Save(sqlTransaction);
                                }
                            }
                        }
                    }
                    /// Above save() in the loop is for saving the child machines. Below save() is for parent machine
                    machine.SetMachineDTO = machineDTO;
                    machine.Save(sqlTransaction);
                    log.LogMethodExit();
                }
            }
            log.LogMethodExit();
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            log.Error(ex);
            if (ex.Number == 2601)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 655));
            }
            else if (ex.Number == 547)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 744));
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
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
    /// <summary>
    /// Deletes the machine records
    /// </summary>
    public void DeleteMachineList()
    {
        log.LogMethodEntry();
        try
        {
            log.LogMethodEntry();
            if (machineDTOList != null && machineDTOList.Any())
            {
                foreach (MachineDTO machineDTO in machineDTOList)
                {
                    Machine machine = new Machine();
                    machine.DeleteMachine(machineDTO.MachineId);
                }
            }
            log.LogMethodExit();
        }
        catch (SqlException ex)
        {
            log.Error(ex);
            if (ex.Number == 2601)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
            }
            else if (ex.Number == 547)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
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

    public DateTime? GetMachineModuleLastUpdateTime(int siteId)
    {
        log.LogMethodEntry(siteId);
        MachineDataHandler machineDataHandler = new MachineDataHandler();
        DateTime? result = machineDataHandler.GetMachineModuleLastUpdateTime(siteId);
        log.LogMethodExit(result);
        return result;
    }

    /// <summary>
    /// Returns the no of Machine matching the search Parameters
    /// </summary>
    /// <param name="searchParameters"> search criteria</param>
    /// <param name="sqlTransaction">Optional sql transaction</param>
    /// <returns></returns>
    public int GetMachineCount(List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)//added
    {
        log.LogMethodEntry(searchParameters, sqlTransaction);
        MachineDataHandler machineDataHandler = new MachineDataHandler(sqlTransaction);
        int machinesCount = machineDataHandler.GetMachinesCount(searchParameters, sqlTransaction);
        log.LogMethodExit(machinesCount);
        return machinesCount;
    }

    public GameCustomDTO RefreshMachine(int machineId, SqlTransaction sqlTransaction = null)
    {
        log.LogMethodEntry(machineId, sqlTransaction);
        MachineDataHandler machineDataHandler = new MachineDataHandler(sqlTransaction);
        GameCustomDTO gameCustomDTO = machineDataHandler.RefreshMachine(machineId);
        log.LogMethodExit(gameCustomDTO);
        return gameCustomDTO;
    }
}

/// <summary>
/// Machine builder class 
/// </summary>
public class MachineBuilderBL
{
    private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private readonly ExecutionContext executionContext;
    public MachineBuilderBL(ExecutionContext executionContext)
    {
        log.LogMethodEntry(executionContext);
        this.executionContext = executionContext;
        log.LogMethodExit();
    }

    /// <summary>
    /// Method which updates the HubName field for each machineDTO in the machineDTOList
    /// </summary>
    /// <param name="machineDTOList"></param>
    /// <returns>machineDTOList</returns>
    public MachineDTO UpdateMachineListToGetHubName(MachineDTO machineDTO, List<HubDTO> hubDTOList)
    {
        log.LogMethodEntry(machineDTO, hubDTOList);
        try
        {
            if (hubDTOList != null && hubDTOList.Any())
            {
                if (hubDTOList.Exists(x => x.MasterId == machineDTO.MasterId))
                {
                    machineDTO.HubName = hubDTOList.Find(x => x.MasterId == machineDTO.MasterId).MasterName;
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        log.LogMethodExit(machineDTO);
        return machineDTO;
    }
    /// <summary>
    /// Method which updates the GameName field for each machineDTO in the machineDTOList
    /// </summary>
    /// <param name="machineDTOList"></param>
    /// <returns></returns>
    public List<MachineDTO> UpdateMachineDTOListToGetGameName(List<MachineDTO> machineDTOList)
    {
        log.LogMethodEntry(machineDTOList);
        try
        {
            GameList gameList = new GameList(executionContext);
            List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters = new List<KeyValuePair<GameDTO.SearchByGameParameters, string>>();
            searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.IS_ACTIVE, "Y"));
            searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<GameDTO> gameDTOList = gameList.GetGameList(searchParameters, false);
            if (gameDTOList != null && gameDTOList.Any())
            {
                foreach (GameDTO game in gameDTOList)
                {
                    var machineList = machineDTOList.FindAll(x => x.GameId == game.GameId).ToList();
                    machineList.ForEach(machineDTO => machineDTO.GameName = game.GameName);
                }
            }
        }
        catch (Exception ex)
        {
            log.Fatal(ex);
        }
        log.LogMethodExit(machineDTOList);
        return machineDTOList;
    }

}
