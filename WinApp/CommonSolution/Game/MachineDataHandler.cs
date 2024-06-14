/********************************************************************************************
 * Project Name - Machine Data Handler                                                                          
 * Description  - Data handler of the machine class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Dec-2015   Kiran          Created 
 *1.10        11-Jan-2016   Mathew         Call to machine attribute is done only if context 
 *                                         is machine
 *********************************************************************************************
 *1.20        19-Oct-2016   Raghuveera          Modified 
 *********************************************************************************************
 *1.30        28-Dec-2016   Soumya          Added GetOutNonInventoryLocationMachines method to 
 *                                          get list of machines that are not mapped to inventory 
 *                                          location
 **********************************************************************************************
 * 1.40       16-May-2016   Lakshminarayana Added CustomDataSetId and ExternalMachineReference as a new search parameter and added a new attribute 'ExternalMachineReference'
 * **********************************************************************************************
 * 2.40       09-Sept-2018  Jagan           For Machine API Modifications for insert/update,delete.
 *                                          Added New Mathods - Save
 * 2.40       30-Oct-2018   Jagan           Added MachineTransfer method for Inter-Site Transfer machine based on particular machine
 * 2.41       07-Nov-2018   Rajiv           Modified existing logic to handle null values.
 * 2.50.0     12-dec-2018   Guru S A        Who column changes
 * 2.60       26-Mar-2019   Jagan           changed IsActvie property from bool to sting and Updated Guid in update statement as per In Tranasit Machine transfer
 * 2.60.2     31-May-2019   Jagan           Added the new DeleteMachine() 
 * 2.70.2     29-Jul-2019   Deeksha         Added GetSQLParameter() ,Modified Insert/Update functions to return DTO.SQL injection issue Fix.
 * 2.70.3     21-Dec-2019   Archana         ReferenceMachineId  condition is commented while getting out of service machine list in GetOutOfServiceMachines() method
 * 2.80.0     21-Mar-2020   Girish Kundar   DBAuditLog changes
 * 2.80       22-Jan-2020   Indrajeet K     Added MachineArrivalDate in insert and update query
 * 2.100      22-Oct-2020  Girish Kundar    Modified : Implemented pagination for POS UI redesign
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Linq;
using Newtonsoft.Json;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// Machinbe data handler - Handles insert, update and select of machine data objects
    /// </summary>
    public class MachineDataHandler
    {
        private const string SELECT_QUERY = @"SELECT * FROM machines AS m ";
        private static readonly Dictionary<MachineDTO.SearchByMachineParameters, string> DBSearchParameters = new Dictionary<MachineDTO.SearchByMachineParameters, string>
            {
                {MachineDTO.SearchByMachineParameters.SITE_ID, "m.site_id"},
                {MachineDTO.SearchByMachineParameters.GAME_ID, "m.game_id"},
                {MachineDTO.SearchByMachineParameters.MACHINE_NAME, "m.machine_name"},
                {MachineDTO.SearchByMachineParameters.IS_ACTIVE, "m.active_flag"},
                {MachineDTO.SearchByMachineParameters.MACHINE_ID, "m.machine_id"},
                {MachineDTO.SearchByMachineParameters.MACADDRESS, "m.MACAddress"},
                {MachineDTO.SearchByMachineParameters.CUSTOM_DATA_SET_ID, "m.CustomDataSetId"},
                {MachineDTO.SearchByMachineParameters.EXTERNAL_MACHINE_REFERENCE, "m.ExternalMachineReference"},
                {MachineDTO.SearchByMachineParameters.MASTER_ID, "m.master_id"},
                {MachineDTO.SearchByMachineParameters.IS_VIRTUAL_ARCADE, ""},
                {MachineDTO.SearchByMachineParameters.REFERENCE_MACHINE_ID, "m.ReferenceMachineId"},
                {MachineDTO.SearchByMachineParameters.MASTER_ENTITY_ID, "m.MasterEntityId"},
                {MachineDTO.SearchByMachineParameters.QR_PLAY_IDENTIFIER, "m.QRPlayIdentifier"},
                {MachineDTO.SearchByMachineParameters.ALLOWED_MACHINE_ID, "m.AllowedMachineId"},
                 {MachineDTO.SearchByMachineParameters.ALLOWED_MACHINE_ID_LIST, "m.AllowedMachineId"},
            };
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction = null;
        private static DBUtils DBUtils;
        private List<SqlParameter> filterParameters = new List<SqlParameter>();
        /// <summary>
        /// Default constructor of MachineDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public MachineDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating RedemptionCurrencyDataHandler Record.
        /// </summary>
        /// <param name="MachineDTO">MachineDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(MachineDTO machineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(machineDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@machineId", machineDTO.MachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gameId", machineDTO.GameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@machineName", machineDTO.MachineName));
            parameters.Add(new SqlParameter("@machineAddress", machineDTO.MachineAddress)); // not nullable 
            parameters.Add(dataAccessHandler.GetSQLParameter("@notes", string.IsNullOrEmpty(machineDTO.Notes) ? DBNull.Value : (object)(machineDTO.Notes)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@activeFlag", string.IsNullOrEmpty(machineDTO.IsActive) ? DBNull.Value : (object)(machineDTO.IsActive)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ticketMode", string.IsNullOrEmpty(machineDTO.TicketMode) ? DBNull.Value : (object)(machineDTO.TicketMode)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@showAd", string.IsNullOrEmpty(machineDTO.ShowAd) ? DBNull.Value : (object)(machineDTO.ShowAd)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@guid", string.IsNullOrEmpty(machineDTO.Guid) ? DBNull.Value : (object)(machineDTO.Guid)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ipAddress", string.IsNullOrEmpty(machineDTO.IPAddress) ? DBNull.Value : (object)(machineDTO.IPAddress)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@macAddress", string.IsNullOrEmpty(machineDTO.MacAddress) ? DBNull.Value : (object)(machineDTO.MacAddress)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@description", string.IsNullOrEmpty(machineDTO.Description) ? DBNull.Value : (object)(machineDTO.Description)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@serialNumber", string.IsNullOrEmpty(machineDTO.SerialNumber) ? DBNull.Value : (object)(machineDTO.SerialNumber)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@softwareVersion", string.IsNullOrEmpty(machineDTO.SoftwareVersion) ? DBNull.Value : (object)(machineDTO.SoftwareVersion)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExternalMachineReference", string.IsNullOrEmpty(machineDTO.ExternalMachineReference) ? DBNull.Value : (object)(machineDTO.ExternalMachineReference)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@machineTag", string.IsNullOrEmpty(machineDTO.MachineTag) ? DBNull.Value : (object)(machineDTO.MachineTag)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ticketAllowed", machineDTO.TicketAllowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterId", machineDTO.MasterId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@previousMachineId", machineDTO.PreviousMachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@nextMachineId", machineDTO.NextMachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@timerMachine", machineDTO.TimerMachine));
            parameters.Add(dataAccessHandler.GetSQLParameter("@timerInterval", machineDTO.TimerInterval <= 0 ? DBNull.Value : (object)(machineDTO.TimerInterval)));
            parameters.Add(new SqlParameter("@groupTimer",string.IsNullOrEmpty(machineDTO.GroupTimer)? DBNull.Value: (object)machineDTO.GroupTimer));
            parameters.Add(dataAccessHandler.GetSQLParameter("@numberOfCoins", machineDTO.NumberOfCoins <= 0 ? DBNull.Value : (object)machineDTO.NumberOfCoins));
            parameters.Add(dataAccessHandler.GetSQLParameter("@customDataSetId", machineDTO.CustomDataSetId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@themeId", machineDTO.ThemeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@themeNumber", machineDTO.ThemeNumber <= 0 ? DBNull.Value : (object)(machineDTO.ThemeNumber)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@tcpPort", machineDTO.TCPPort <= 0 ? DBNull.Value : (object)(machineDTO.TCPPort)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@purchasePrice", machineDTO.PurchasePrice <= 0 ? DBNull.Value : (object)(machineDTO.PurchasePrice)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@readerType", machineDTO.ReaderType <= 0 ? DBNull.Value : (object)(machineDTO.ReaderType)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@payoutCost", machineDTO.PayoutCost <= 0 ? DBNull.Value : (object)(machineDTO.PayoutCost)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@inventoryLocationId", machineDTO.InventoryLocationId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@referenceMachineId", machineDTO.ReferenceMachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", machineDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@communicationSuccessRatio", machineDTO.CommunicationSuccessRatio));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@machineCharacteristics", machineDTO.MachineCharacteristics));
            parameters.Add(dataAccessHandler.GetSQLParameter("@attribute1", machineDTO.Attribute1));
            parameters.Add(dataAccessHandler.GetSQLParameter("@attribute2", machineDTO.Attribute2));
            parameters.Add(dataAccessHandler.GetSQLParameter("@attribute3", machineDTO.Attribute3));
            parameters.Add(dataAccessHandler.GetSQLParameter("@synchStatus", machineDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@machineArrivalDate", machineDTO.MachineArrivalDate == DateTime.MinValue ? DBNull.Value : (object)machineDTO.MachineArrivalDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@qRPlayIdentifier", machineDTO.QRPlayIdentifier));
            parameters.Add(dataAccessHandler.GetSQLParameter("@eraseQRPlayIdentifier", machineDTO.EraseQRPlayIdentifier));
            parameters.Add(dataAccessHandler.GetSQLParameter("AllowedMachineId", machineDTO.AllowedMachineID, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the machine data
        /// </summary>
        /// <param name="gameMachine">MachineDTO</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public MachineDTO InsertMachine(MachineDTO gameMachine, string loginId, int siteId)
        {
            log.LogMethodEntry(gameMachine, loginId, siteId);
            string insertMachineQuery = @"insert into machines 
                                                               (machine_name,
                                                                machine_address, 
                                                                game_id,
                                                                master_id,
                                                                notes,
                                                                last_updated_date,
                                                                last_updated_user,
                                                                ticket_allowed,
                                                                active_flag,
                                                                timer_machine, 
                                                                timer_interval, 
                                                                group_timer,
                                                                number_of_coins,
                                                                ticket_mode, 
                                                                CustomDataSetId, 
                                                                ThemeId,
                                                                ThemeNumber,
                                                                ShowAd,
                                                                Guid,
                                                                site_id,
                                                                IPAddress, 
                                                                TCPPort,
                                                                MACAddress,
                                                                Description,
                                                                SerialNumber,
                                                                SoftwareVersion, 
                                                                PurchasePrice, 
                                                                ReaderType,
                                                                PayoutCost,
                                                                InventoryLocationId, 
                                                                ReferenceMachineId,
                                                                MasterEntityId,
                                                                ExternalMachineReference,
                                                                MachineTag,
                                                                PreviousMachineId,
                                                                NextMachineId,
                                                                creationdate,
                                                                createdby,
                                                                CommunicationSuccessRatio,
                                                                MachineArrivalDate,
                                                                MachineCharacteristics,
                                                                Attribute1,
                                                                Attribute2,
                                                                Attribute3,
                                                                QRPlayIdentifier,
                                                                EraseQRPlayIdentifier,
                                                                AllowedMachineId
                                                                ) 
                                                        values (
                                                                @machineName,
                                                                @machineAddress,
                                                                @gameId,
                                                                @masterId,
                                                                @notes,
                                                                GETDATE(),
                                                                @lastUpdatedUser,
                                                                @ticketAllowed,
                                                                @activeFlag,
                                                                @timerMachine, 
                                                                @timerInterval,
                                                                @groupTimer,
                                                                @numberOfCoins,
                                                                @ticketMode,
                                                                @customDataSetId,
                                                                @themeId,
                                                                @themeNumber,
                                                                @showAd,
                                                                NEWID(),
                                                                @siteId,
                                                                @ipAddress, 
                                                                @tcpPort,
                                                                @macAddress,
                                                                @description,
                                                                @serialNumber,
                                                                @softwareVersion, 
                                                                @purchasePrice,
                                                                @readerType,
                                                                @payoutCost,
                                                                @inventoryLocationId, 
                                                                @referenceMachineId,
                                                                @masterEntityId,
                                                                @ExternalMachineReference,
                                                                @machineTag,
                                                                @previousMachineId,
                                                                @nextMachineId,
                                                                GETDATE(),
                                                                @createdBy,
                                                                @communicationSuccessRatio,
                                                                @machineArrivalDate,
                                                                @machineCharacteristics,
                                                                @attribute1,
                                                                @attribute2,
                                                                @attribute3,
                                                                @qRPlayIdentifier,
                                                                @eraseQRPlayIdentifier,
                                                                @AllowedMachineId
                                                                 ) 
                                         SELECT * FROM machines WHERE machine_id = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertMachineQuery, GetSQLParameters(gameMachine, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMachineDTO(gameMachine, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting gameMachine", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(gameMachine);
            return gameMachine;

        }

        /// <summary>
        /// Updates machine record
        /// </summary>
        /// <param name="gameMachine">MachineDTO</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows.</returns>
        public MachineDTO UpdateMachine(MachineDTO gameMachine, string userId, int siteId)
        {
            log.LogMethodEntry(gameMachine, userId, siteId);
            string updateMachineQuery = @"update machines 
                                             set machine_name = @machineName, 
                                                 machine_Address = @machineAddress, 
                                                 game_id = @gameId, 
                                                 master_id = @masterId,
                                                 notes = @notes,
                                                 last_updated_date = GETDATE(),
                                                 last_updated_user = @lastUpdatedUser,
                                                 ticket_allowed = @ticketAllowed,
                                                 active_flag = @activeFlag,
                                                 timer_machine = @timerMachine,  	
                                                 timer_interval = @timerInterval, 
                                                 group_timer = @groupTimer, 
                                                 number_of_coins = @numberOfCoins, 
                                                 ticket_mode = @ticketMode,
                                                 CustomDataSetId = @customDataSetId,
                                                 ThemeId = @themeId, 
                                                 ThemeNumber = @themeNumber, 
                                                 ShowAd = @showAd, 
                                                 Guid = case when @guid IS NOT NULL then @guid else guid end,
                                                 site_id = @siteId, 
                                                 IPAddress = @ipAddress, 
                                                 TCPPort = @tcpPort, 
                                                 MACAddress = @macAddress, 
                                                 Description = @description, 
                                                 SerialNumber = @serialNumber, 
                                                 SoftwareVersion = @softwareVersion,
                                                 PurchasePrice = @purchasePrice, 
                                                 ReaderType = @readerType, 
                                                 PayoutCost = @payoutCost, 
                                                 InventoryLocationId = @inventoryLocationId,
                                                 ReferenceMachineId = @referenceMachineId,
                                                 MasterEntityId = @masterEntityId,
                                                 ExternalMachineReference = @ExternalMachineReference,
                                                 MachineTag = @machineTag,
                                                 PreviousMachineId=@previousMachineId,
                                                 NextMachineId=@nextMachineId,
                                                 CommunicationSuccessRatio = @communicationSuccessRatio,
                                                 MachineArrivalDate=@machineArrivalDate,
                                                  MachineCharacteristics =@machineCharacteristics,
                                                 QRPlayIdentifier = @qRPlayIdentifier,
                                                 EraseQRPlayIdentifier = @eraseQRPlayIdentifier,
                                                 Attribute1= @attribute1,
                                                 Attribute2 =@attribute2,
                                                 Attribute3 = @attribute3,
                                                 AllowedMachineId=@AllowedMachineId
                                                 where machine_id = @machineId
                        SELECT * FROM machines WHERE machine_id = @machineId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateMachineQuery, GetSQLParameters(gameMachine, userId, siteId).ToArray(), sqlTransaction);
                MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
                RefreshMachineDTO(gameMachine, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating gameMachine", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(gameMachine);
            return gameMachine;
        }



        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="gameMachine">MachineDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshMachineDTO(MachineDTO gameMachine, DataTable dt)
        {
            log.LogMethodEntry(gameMachine, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                gameMachine.MachineId = Convert.ToInt32(dt.Rows[0]["machine_id"]);
                gameMachine.LastUpdateDate = dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]);
                gameMachine.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                gameMachine.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                gameMachine.LastUpdatedBy = dataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_updated_user"]);
                gameMachine.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                gameMachine.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        internal int GetActiveDisplayThemeId(int machineId, int gameId, int gameProfileId)
        {
            log.LogMethodEntry(machineId, gameId, gameProfileId);
            int result = -1;
            if (IsOutOfService(machineId))
            {
                result = GetOutOfServiceTheme(machineId);
                log.LogMethodExit(result, "Machine out of service");
                return result;
            }
            string query = @"with calendar(machineId, value1, enabledOutOfService, sort) 
                            as(select machineid, rt.Id, ISNULL(EnabledOutOfService, 0), 1 sort
	                            from GenericCalendar gc, Theme rt
	                            where (ISNULL(day, -1) = -1 or
			                            DAY = DATEPART(W, GETDATE() - 1) or
			                            DAY - 1000 = DATEPART(DAY, GETDATE()))
	                            and (Date is null or (GETDATE() >= Date and GETDATE() < Date + 1))
	                            and (
								      (ISNULL(FromTime, 0) <= DATEPART(HOUR, GETDATE()) + (DATEPART(mi,getdate())/100.00)
									    AND ISNULL(DAY, -1) != -1
									  )
									  or
									  (CASE WHEN convert(int,round(ISNULL(FromTime, 0), 0)) >  convert(int,round(isnull(Totime, 24), 0))
											  and datepart(hour, getdate()) BETWEEN 0 AND convert(int,round(isnull(Totime, 24), 0))
											THEN dateadd(MINUTE,Convert(int,right(ISNULL(FromTime, 0),2)), dateadd(HOUR, convert(int,round(ISNULL(FromTime, 0), 0)), convert(datetime,convert(date,getdate() - 1))))
											ELSE  dateadd(MINUTE,Convert(int,right(ISNULL(FromTime, 0),2)), dateadd(HOUR, convert(int,round(ISNULL(FromTime, 0), 0)), convert(datetime,convert(date,getdate()))))
										END <= GETDATE()
										AND ISNULL(DAY, -1) = -1
									  )
									)
	                            and dateadd(MINUTE,Convert(int,right(isnull(Totime, 24),2)), dateadd(HOUR, convert(int,round(isnull(Totime, 24), 0)), convert(datetime,convert(date,getdate()))))
										 > GETDATE()
								and gc.ThemeId is not null
	                            and gc.ThemeId = rt.Id
	                            and CalendarType = 'THEME'
	                            and MachineId is not null
	                            union all
	                            select machine_id, rh.Id, 0, 2
  	                                from machines m, Theme rh
	                                where m.ThemeId = rh.Id
	                            union all
	                            select MachineGroupMachines.MachineId, rt.Id, ISNULL(EnabledOutOfService, 0), 3
	                                from GenericCalendar, MachineGroupMachines, Theme rt
	                                where GenericCalendar.MachineGroupId = MachineGroupMachines.MachineGroupId 
	                                and (ISNULL(day, -1) = -1 or
			                            DAY = DATEPART(W, GETDATE() - 1) or
			                            DAY - 1000 = DATEPART(DAY, GETDATE()))
		                            and (Date is null or (GETDATE() >= Date and GETDATE() < Date + 1))
		                            and (
								      (ISNULL(FromTime, 0) <= DATEPART(HOUR, GETDATE()) + (DATEPART(mi,getdate())/100.00)
									    AND ISNULL(DAY, -1) != -1
									  )
									  or
									  (CASE WHEN convert(int,round(ISNULL(FromTime, 0), 0)) >  convert(int,round(isnull(Totime, 24), 0))
											  and datepart(hour, getdate()) BETWEEN 0 AND convert(int,round(isnull(Totime, 24), 0))
											THEN dateadd(MINUTE,Convert(int,right(ISNULL(FromTime, 0),2)), dateadd(HOUR, convert(int,round(ISNULL(FromTime, 0), 0)), convert(datetime,convert(date,getdate() - 1))))
											ELSE  dateadd(MINUTE,Convert(int,right(ISNULL(FromTime, 0),2)), dateadd(HOUR, convert(int,round(ISNULL(FromTime, 0), 0)), convert(datetime,convert(date,getdate()))))
										END <= GETDATE()
										AND ISNULL(DAY, -1) = -1
									  )
									)
	                            and dateadd(MINUTE,Convert(int,right(isnull(Totime, 24),2)), dateadd(HOUR, convert(int,round(isnull(Totime, 24), 0)), convert(datetime,convert(date,getdate()))))
										 > GETDATE()
								and GenericCalendar.ThemeId is not null
		                            and GenericCalendar.ThemeId = rt.Id
		                            and CalendarType = 'THEME'
	                            union all
	                            select machines.machine_id, rt.Id, ISNULL(EnabledOutOfService, 0), 4
  	                                from GenericCalendar, games, machines, Theme rt
	                                where GenericCalendar.GameProfileId = games.game_profile_id
	                                and machines.game_id = games.game_id
	                                and (ISNULL(day, -1) = -1 or
			                            DAY = DATEPART(W, GETDATE() - 1) or
			                            DAY - 1000 = DATEPART(DAY, GETDATE()))
	                                and (Date is null or (GETDATE() >= Date and GETDATE() < Date + 1))
	                                and (
								      (ISNULL(FromTime, 0) <= DATEPART(HOUR, GETDATE()) + (DATEPART(mi,getdate())/100.00)
									    AND ISNULL(DAY, -1) != -1
									  )
									  or
									  (CASE WHEN convert(int,round(ISNULL(FromTime, 0), 0)) >  convert(int,round(isnull(Totime, 24), 0))
											  and datepart(hour, getdate()) BETWEEN 0 AND convert(int,round(isnull(Totime, 24), 0))
											THEN dateadd(MINUTE,Convert(int,right(ISNULL(FromTime, 0),2)), dateadd(HOUR, convert(int,round(ISNULL(FromTime, 0), 0)), convert(datetime,convert(date,getdate() - 1))))
											ELSE  dateadd(MINUTE,Convert(int,right(ISNULL(FromTime, 0),2)), dateadd(HOUR, convert(int,round(ISNULL(FromTime, 0), 0)), convert(datetime,convert(date,getdate()))))
										END <= GETDATE()
										AND ISNULL(DAY, -1) = -1
									  )
									)
	                            and dateadd(MINUTE,Convert(int,right(isnull(Totime, 24),2)), dateadd(HOUR, convert(int,round(isnull(Totime, 24), 0)), convert(datetime,convert(date,getdate()))))
										 > GETDATE()
								and GenericCalendar.ThemeId is not null
	                                and GenericCalendar.ThemeId = rt.Id
	                                and CalendarType = 'THEME' 
	                            union all
	                            select m.machine_id, rh.Id, 0, 5
	                            from game_profile gp, games g, machines m, Theme rh
	                            where gp.ThemeId = rh.Id
	                            and gp.game_profile_id = g.game_profile_id
	                            and m.game_id = g.game_id)
                            select m.game_id, m.machine_address, m.machine_name, number_of_coins, group_timer, 
                            m.IPAddress, m.TCPPort, machine_id, m.MACAddress, 
                            isnull(isnull(g.play_credits, gp.play_credits), 0) play_credits, 
                            isnull(isnull(g.vip_play_credits, gp.vip_play_credits), 0) vip_play_credits, 
                            gp.game_profile_id, isnull(m.timer_machine, 'N') timer_machine, isnull(m.timer_interval, 0) timer_interval, 
                            gp.TokenRedemption, isnull(gp.TokenPrice, 0) TokenPrice, gp.PhysicalToken, 
                            gp.RedeemTokenTo, m.ticket_mode, gp.TicketEater, 
                            gp.credit_allowed, gp.bonus_allowed, gp.courtesy_allowed, gp.time_allowed, m.ReaderType, 
                            isnull(case m.ShowAd when 'D' then gp.showAd else m.ShowAd end, gp.ShowAd) ShowAd, 
                            gp.UserIdentifier GPUserIdentifier, g.UserIdentifier GameUserIdentifier, 
                            m.InventoryLocationId, m.ReferenceMachineId, isnull(gp.ForceRedeemToCard,0) ForceRedeemToCard,
                            (select value1 
                                from (select top 1 * 
                                        from (select * from calendar) v1 
	                                where m.machine_id = v1.MachineId
	                            order by sort) v2) ThemeId,
                            (select EnabledOutOfService 
                                from (select top 1 * 
                                        from (select * from calendar) v1 
	                                where m.machine_id = v1.MachineId
	                            order by sort) v2) EnabledOutOfService 
                        from machines m, game_profile gp, games g 
                        where m.game_id = g.game_id 
                        and m.active_flag = 'Y' 
                        and g.game_profile_id = gp.game_profile_id 
                        and m.machine_id = @machineId
                        order by m.machine_address";
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { new SqlParameter("@machineId", machineId) }, sqlTransaction);
            if (dataTable.Rows.Count <= 0)
            {
                log.LogMethodExit(-1, "Machine not found");
                return -1;
            }
            if (dataTable.Rows[0]["ThemeId"] != DBNull.Value)
            {
                result = Convert.ToInt32(dataTable.Rows[0]["ThemeId"]);
            }
            query = @"select v.*,rt.Id ThemeId, rt.ThemeNumber, rtv.ThemeNumber VisualizationThemeNumber, case when @membershipId < 0 then r.MembershipId else case when r.MembershipId = @membershipId then -2 else isnull(r.MembershipId, -1) end end sort1, 1 sort2 
                                from PromotionView v
                                left outer join PromotionRule r
                                    on r.promotion_id = v.promotion_id
                                left outer join Theme rt
                                    on rt.Id = v.ThemeId
                                left outer join Theme rtv
                                    on rtv.Id = v.VisualizationThemeId
                              where v.game_id = @game_id
                              union all
                              select v.*, rt.Id ThemeId, rt.ThemeNumber, rtv.ThemeNumber VisualizationThemeNumber, case when @membershipId < 0 then r.MembershipId else case when r.MembershipId = @membershipId then -2 else isnull(r.MembershipId, -1) end end, 2 
                                from PromotionView v
                                left outer join PromotionRule r
                                    on r.promotion_id = v.promotion_id
                                left outer join Theme rt
                                    on rt.Id = v.ThemeId
                                left outer join Theme rtv
                                    on rtv.Id = v.VisualizationThemeId
                              where v.game_profile_id = @game_profile_id
                              union all
                              select v.*, rt.Id ThemeId, rt.ThemeNumber, rtv.ThemeNumber VisualizationThemeNumber, case when @membershipId < 0 then r.MembershipId else case when r.MembershipId = @membershipId then -2 else isnull(r.MembershipId, -1) end end, 3 
                                from PromotionView v
                                left outer join PromotionRule r
                                    on r.promotion_id = v.promotion_id
                                left outer join Theme rt
                                    on rt.Id = v.ThemeId
                                left outer join Theme rtv
                                    on rtv.Id = v.VisualizationThemeId
                              where v.game_id is null 
                                and v.game_profile_id is null
                                and v.PromotionType = 'G'
                                order by sort1, sort2";
            dataAccessHandler = new DataAccessHandler();
            DataTable dt = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { new SqlParameter("@game_id", gameId), new SqlParameter("@game_profile_id", gameProfileId), new SqlParameter("@membershipId", -2) }, sqlTransaction);
            if (dt.Rows.Count <= 0)
            {
                log.LogMethodExit(result);
                return result;
            }
            dataAccessHandler = new DataAccessHandler();
            Object o = dataAccessHandler.executeScalar(@"select top 1 1
                                                            from PromotionRule
                                                            where promotion_id = @promoId", new SqlParameter[] { new SqlParameter("@promoId", dt.Rows[0]["promotion_id"]) }, sqlTransaction);

            if (o != null && o != DBNull.Value)
            {
                log.LogMethodExit(result);
                return result;
            }
            if (dt.Rows[0]["ThemeId"] != DBNull.Value)
            {
                result = Convert.ToInt32(dt.Rows[0]["ThemeId"]);
            }
            log.LogMethodExit(result);
            return result;
        }

        private int GetOutOfServiceTheme(int machineId)
        {
            log.LogMethodEntry(machineId);
            int result = -1;
            string query = @"select Id from theme where ThemeNumber = (
                            select top 1 isnull(gpa.attributeValue, defaults.attributeValue) value
                            from (
                                    select attribute, isnull(g1.attributeValue, isnull(g2.attributeValue, g3.attributeValue)) attributeValue, a.attributeId
                                    from GameProfileAttributes a 
                                            left outer join GameProfileAttributeValues g1
                                            on g1.game_id = (select game_id from machines where machine_id = @machine_id)
                                            and a.attributeId = g1.attributeId
                                            left outer join GameProfileAttributeValues g2
                                            on (g2.game_profile_id = (select game_profile_id from games g, machines m
                                                                    where m.machine_id = @machine_id and g.game_id = m.game_id))
                                            and a.attributeId = g2.attributeId
                                            left outer join GameProfileAttributeValues g3
                                            on a.attributeId = g3.attributeId 
                                                and g3.game_profile_id is null 
                                                and g3.game_id is null 
                                                and g3.machine_id is null) defaults
                            left outer join GameProfileAttributeValues gpa
                                on defaults.attributeId = gpa.attributeId
                                and machine_id = @machine_id
	                            where Attribute = 'OUT_OF_SERVICE_THEME')";
            dataAccessHandler = new DataAccessHandler();
            try
            {
                object o = dataAccessHandler.executeScalar(query, new SqlParameter[] { new SqlParameter("@machine_id", machineId) }, sqlTransaction);
                if (o != DBNull.Value)
                {
                    result = Convert.ToInt32(o);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while computing out of service", ex);
                result = -1;
            }
            log.LogMethodExit(result);
            return result;

        }

        private bool IsOutOfService(int machineId)
        {
            log.LogMethodEntry(machineId);
            bool result = false;
            string query = @"select isnull(gpa.attributeValue, defaults.attributeValue) value
                            from (
                                    select attribute, isnull(g1.attributeValue, isnull(g2.attributeValue, g3.attributeValue)) attributeValue, a.attributeId
                                    from GameProfileAttributes a 
                                            left outer join GameProfileAttributeValues g1
                                            on g1.game_id = (select game_id from machines where machine_id = @machine_id)
                                            and a.attributeId = g1.attributeId
                                            left outer join GameProfileAttributeValues g2
                                            on (g2.game_profile_id = (select game_profile_id from games g, machines m
                                                                    where m.machine_id = @machine_id and g.game_id = m.game_id))
                                            and a.attributeId = g2.attributeId
                                            left outer join GameProfileAttributeValues g3
                                            on a.attributeId = g3.attributeId 
                                                and g3.game_profile_id is null 
                                                and g3.game_id is null 
                                                and g3.machine_id is null) defaults
                            left outer join GameProfileAttributeValues gpa
                                on defaults.attributeId = gpa.attributeId
                                and machine_id = @machine_id
	                            where Attribute = 'OUT_OF_SERVICE'";
            dataAccessHandler = new DataAccessHandler();
            try
            {
                object o = dataAccessHandler.executeScalar(query, new SqlParameter[] { new SqlParameter("@machine_id", machineId) }, sqlTransaction);
                if (o != DBNull.Value && Convert.ToInt32(o) == 1)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while computing out of service", ex);
                result = false;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Converts the Data row object to MachineDTO calss type
        /// </summary>
        /// <param name="machineDataRow">Machine DataRow</param>
        /// <returns>Returns MachineDTO</returns>
        private MachineDTO GetMachineDTO(DataRow machineDataRow)
        {
            log.LogMethodEntry(machineDataRow);
            MachineDTO machineDataObject = new MachineDTO(Convert.ToInt32(machineDataRow["machine_id"]),
                                                    machineDataRow["machine_name"] == DBNull.Value ? string.Empty : Convert.ToString(machineDataRow["machine_name"]),
                                                    machineDataRow["machine_Address"] == DBNull.Value ? string.Empty : Convert.ToString(machineDataRow["machine_Address"]),
                                                    machineDataRow["game_id"] == DBNull.Value ? -1 : Convert.ToInt32(machineDataRow["game_id"]),//Starts:Modification on 19-Oct-2016 for handling DBNull case
                                                    machineDataRow["master_id"] == DBNull.Value ? -1 : Convert.ToInt32(machineDataRow["master_id"]),//Ends:Modification on 19-Oct-2016 for handling DBNull case
                                                    machineDataRow["notes"] == DBNull.Value ? string.Empty : Convert.ToString(machineDataRow["notes"]),
                                                    machineDataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(machineDataRow["last_updated_date"]),
                                                    machineDataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(machineDataRow["last_updated_user"]),
                                                    machineDataRow["ticket_allowed"] == DBNull.Value ? string.Empty : Convert.ToString(machineDataRow["ticket_allowed"]),
                                                    machineDataRow["active_flag"].ToString(),
                                                    machineDataRow["timer_machine"] == DBNull.Value ? string.Empty : Convert.ToString(machineDataRow["timer_machine"]),
                                                    machineDataRow["timer_interval"] == DBNull.Value ? -1 : Convert.ToInt32(machineDataRow["timer_interval"]),
                                                    machineDataRow["group_timer"] == DBNull.Value ? string.Empty : Convert.ToString(machineDataRow["group_timer"]),
                                                    machineDataRow["number_of_coins"] == DBNull.Value ? -1 : Convert.ToInt32(machineDataRow["number_of_coins"]),
                                                    machineDataRow["ticket_mode"] == DBNull.Value ? string.Empty : Convert.ToString(machineDataRow["ticket_mode"]),
                                                    machineDataRow["CustomDataSetId"] == DBNull.Value ? -1 : Convert.ToInt32(machineDataRow["CustomDataSetId"]),
                                                    machineDataRow["ThemeId"] == DBNull.Value ? -1 : Convert.ToInt32(machineDataRow["ThemeId"]),
                                                    machineDataRow["ThemeNumber"] == DBNull.Value ? -1 : Convert.ToInt32(machineDataRow["ThemeNumber"]),
                                                    machineDataRow["ShowAd"] == DBNull.Value ? string.Empty : Convert.ToString(machineDataRow["ShowAd"]),
                                                    machineDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(machineDataRow["Guid"]),
                                                    machineDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(machineDataRow["site_id"]),
                                                    machineDataRow["IPAddress"] == DBNull.Value ? string.Empty : Convert.ToString(machineDataRow["IPAddress"]),
                                                    machineDataRow["TCPPort"] == DBNull.Value ? -1 : Convert.ToInt32(machineDataRow["TCPPort"]),
                                                    machineDataRow["MACAddress"] == DBNull.Value ? string.Empty : Convert.ToString(machineDataRow["MACAddress"]),
                                                    machineDataRow["Description"] == DBNull.Value ? string.Empty : Convert.ToString(machineDataRow["Description"]),
                                                    machineDataRow["SerialNumber"] == DBNull.Value ? "" : Convert.ToString(machineDataRow["SerialNumber"]),
                                                    machineDataRow["SoftwareVersion"] == DBNull.Value ? string.Empty : Convert.ToString(machineDataRow["SoftwareVersion"]),
                                                    machineDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(machineDataRow["SynchStatus"]),
                                                    machineDataRow["PurchasePrice"] == DBNull.Value ? -1 : Convert.ToInt32(machineDataRow["PurchasePrice"]),
                                                    machineDataRow["ReaderType"] == DBNull.Value ? -1 : Convert.ToInt32(machineDataRow["ReaderType"]),
                                                    machineDataRow["PayoutCost"] == DBNull.Value ? -1 : Convert.ToDouble(machineDataRow["PayoutCost"]),
                                                    machineDataRow["InventoryLocationId"] == DBNull.Value ? -1 : Convert.ToInt32(machineDataRow["InventoryLocationId"]),
                                                    machineDataRow["ReferenceMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(machineDataRow["ReferenceMachineId"]),
                                                    machineDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(machineDataRow["MasterEntityId"]),
                                                    machineDataRow["ExternalMachineReference"] == DBNull.Value ? "" : Convert.ToString(machineDataRow["ExternalMachineReference"]),
                                                    machineDataRow["MachineTag"] == DBNull.Value ? string.Empty : Convert.ToString(machineDataRow["MachineTag"]),
                                                    machineDataRow["CommunicationSuccessRatio"] == DBNull.Value ? 0 : Convert.ToInt32(machineDataRow["CommunicationSuccessRatio"]),
                                                    machineDataRow["PreviousMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(machineDataRow["PreviousMachineId"]),
                                                    machineDataRow["NextMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(machineDataRow["NextMachineId"]),
                                                    machineDataRow["creationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(machineDataRow["creationDate"]),
                                                    machineDataRow["createdBy"] == DBNull.Value ? string.Empty : Convert.ToString(machineDataRow["createdBy"]),
                                                    machineDataRow["MachineArrivalDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(machineDataRow["MachineArrivalDate"]),
                                                    machineDataRow["MachineCharacteristics"] == DBNull.Value ? string.Empty : GetDeSerializedString(Convert.ToString(machineDataRow["MachineCharacteristics"])),
                                                    machineDataRow["Attribute1"] == DBNull.Value ? string.Empty : Convert.ToString(machineDataRow["Attribute1"]),
                                                    machineDataRow["Attribute2"] == DBNull.Value ? string.Empty : Convert.ToString(machineDataRow["Attribute2"]),
                                                    machineDataRow["Attribute3"] == DBNull.Value ? string.Empty : Convert.ToString(machineDataRow["Attribute3"]),
                                                    machineDataRow["QRPlayIdentifier"] == DBNull.Value ? string.Empty : Convert.ToString(machineDataRow["QRPlayIdentifier"]),
                                                    machineDataRow["EraseQRPlayIdentifier"] == DBNull.Value ? false : Convert.ToBoolean(machineDataRow["EraseQRPlayIdentifier"]),
                                                    machineDataRow["AllowedMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(machineDataRow["AllowedMachineId"])
                                                   );
            log.LogMethodExit(machineDataObject);
            return machineDataObject;
        }

        private GameCustomDTO GetGameCustomDTO(DataRow gameCustomDataRow)
        {
            log.LogMethodEntry(gameCustomDataRow);
            GameCustomDTO gameCustomDTO = new GameCustomDTO(gameCustomDataRow["game_id"] == DBNull.Value ? -1 : Convert.ToInt32(gameCustomDataRow["game_id"]),
                                                            gameCustomDataRow["machine_id"] == DBNull.Value ? -1 : Convert.ToInt32(gameCustomDataRow["machine_id"]),
                                                            gameCustomDataRow["machine_name"] == DBNull.Value ? string.Empty : gameCustomDataRow["machine_name"].ToString()
                                                            , gameCustomDataRow["machine_address"] == DBNull.Value ? string.Empty : gameCustomDataRow["machine_address"].ToString(),
                                                            gameCustomDataRow["game_profile_id"] == DBNull.Value ? -1 : Convert.ToInt32(gameCustomDataRow["game_profile_id"])
                                                            , gameCustomDataRow["play_credits"] == DBNull.Value ? -1 : Convert.ToDouble(gameCustomDataRow["play_credits"]),
                                                            gameCustomDataRow["vip_play_credits"] == DBNull.Value ? -1 : Convert.ToDouble(gameCustomDataRow["vip_play_credits"])
                                                            , gameCustomDataRow["MACAddress"] == DBNull.Value ? string.Empty : gameCustomDataRow["MACAddress"].ToString(),
                                                            gameCustomDataRow["credit_allowed"] == DBNull.Value ? "N" : gameCustomDataRow["credit_allowed"].ToString()
                                                            , gameCustomDataRow["bonus_allowed"] == DBNull.Value ? string.Empty : gameCustomDataRow["bonus_allowed"].ToString(),
                                                            gameCustomDataRow["courtesy_allowed"] == DBNull.Value ? "N" : gameCustomDataRow["courtesy_allowed"].ToString()
                                                            , gameCustomDataRow["time_allowed"] == DBNull.Value ? string.Empty : gameCustomDataRow["time_allowed"].ToString(),
                                                            gameCustomDataRow["group_timer"] == DBNull.Value ? "N" : gameCustomDataRow["group_timer"].ToString()
                                                            , gameCustomDataRow["number_of_coins"] == DBNull.Value ? "3" : gameCustomDataRow["number_of_coins"].ToString(),
                                                            gameCustomDataRow["IPAddress"] == DBNull.Value ? string.Empty : gameCustomDataRow["IPAddress"].ToString()
                                                            , gameCustomDataRow["TCPPort"] == DBNull.Value ? -1 : Convert.ToInt32(gameCustomDataRow["TCPPort"]),
                                                            gameCustomDataRow["TokenPrice"] == DBNull.Value ? 0 : Convert.ToDouble(gameCustomDataRow["TokenPrice"])
                                                            , gameCustomDataRow["TokenRedemption"] == DBNull.Value ? "N" : gameCustomDataRow["TokenRedemption"].ToString(),
                                                            gameCustomDataRow["timer_Machine"] == DBNull.Value ? "N" : gameCustomDataRow["timer_Machine"].ToString()
                                                            , gameCustomDataRow["timer_interval"] == DBNull.Value ? -1 : Convert.ToInt32(gameCustomDataRow["timer_interval"]),
                                                            gameCustomDataRow["PhysicalToken"] == DBNull.Value ? "N" : gameCustomDataRow["PhysicalToken"].ToString()
                                                            , gameCustomDataRow["RedeemTokenTo"] == DBNull.Value ? "" : gameCustomDataRow["RedeemTokenTo"].ToString(),
                                                            gameCustomDataRow["ticket_mode"] == DBNull.Value ? "D" : gameCustomDataRow["ticket_mode"].ToString()
                                                            , gameCustomDataRow["ticketEater"] == DBNull.Value ? "N" : gameCustomDataRow["ticketEater"].ToString(),
                                                            gameCustomDataRow["readerType"] == DBNull.Value ? -1 : Convert.ToInt32(gameCustomDataRow["readerType"])
                                                            , gameCustomDataRow["ShowAd"] == DBNull.Value ? "N" : gameCustomDataRow["ShowAd"].ToString(),
                                                            gameCustomDataRow["GPUserIdentifier"] == DBNull.Value ? -1 : Convert.ToInt32(gameCustomDataRow["GPUserIdentifier"])
                                                            , gameCustomDataRow["GameUserIdentifier"] == DBNull.Value ? -1 : Convert.ToInt32(gameCustomDataRow["GameUserIdentifier"]),
                                                            gameCustomDataRow["InventoryLocationId"] == DBNull.Value ? -1 : Convert.ToInt32(gameCustomDataRow["InventoryLocationId"])
                                                            , gameCustomDataRow["ReferenceMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(gameCustomDataRow["ReferenceMachineId"]),
                                                            gameCustomDataRow["ForceRedeemToCard"] == DBNull.Value ? false : Convert.ToBoolean(gameCustomDataRow["ForceRedeemToCard"])
                                                            , gameCustomDataRow["ThemeNumber"] == DBNull.Value ? "0" : gameCustomDataRow["ThemeNumber"].ToString(),
                                                            gameCustomDataRow["EnabledOutOfService"] == DBNull.Value ? false : Convert.ToBoolean(gameCustomDataRow["EnabledOutOfService"])
                                                            , gameCustomDataRow["QRPlayIdentifier"] == DBNull.Value ? string.Empty : gameCustomDataRow["QRPlayIdentifier"].ToString(),
                                                            gameCustomDataRow["EraseQRPlayIdentifier"] == DBNull.Value ? false : Convert.ToBoolean(gameCustomDataRow["EraseQRPlayIdentifier"])
                                                            , gameCustomDataRow["ExternalMachineReference"] == DBNull.Value ? string.Empty : gameCustomDataRow["ExternalMachineReference"].ToString(),
                                                            gameCustomDataRow["gameUrl"] == DBNull.Value ? string.Empty : gameCustomDataRow["gameUrl"].ToString(),
                                                            gameCustomDataRow["IsExternalGame"] == DBNull.Value ? false : Convert.ToBoolean(gameCustomDataRow["IsExternalGame"]),
                                                            false, //IsPromotionConfigValueChanged flag
                                                            new List<MachineInputDevicesDTO>() //MachineInputDevicesDTO List
                                                            );
            log.LogMethodExit(gameCustomDTO);
            return gameCustomDTO;
        }

        private string GetDeSerializedString(string jsonString)
        {
            log.LogMethodEntry(jsonString);
            var result = JsonConvert.DeserializeObject<string>(jsonString);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the machine data of passed machine id
        /// </summary>
        /// <param name="machineId">Machine Id</param>
        /// <returns>Returns MachineDTO</returns>
        public MachineDTO GetMachine(int machineId)
        {
            log.LogMethodEntry(machineId);
            string selectMachineQuery = @"select *
                                            from machines
                                           where machine_id = @machineId";
            SqlParameter[] selectMachineParameters = new SqlParameter[1];
            selectMachineParameters[0] = new SqlParameter("@machineId", machineId);
            DataTable machineData = dataAccessHandler.executeSelectQuery(selectMachineQuery, selectMachineParameters, sqlTransaction);
            if (machineData.Rows.Count > 0)
            {
                DataRow machineDataRow = machineData.Rows[0];
                MachineDTO machineDataObject = GetMachineDTO(machineDataRow);

                log.LogMethodExit(machineDataObject);
                return machineDataObject;
            }
            else
            {
                log.LogMethodExit("Returning null.");
                return null;
            }
        }

        /// <summary>
        /// Gets max machine address of passed master id (Hub)
        /// </summary>
        /// <param name="masterId">Parameter of the type integer,holds Hub id. </param>
        /// <returns>Returns the object of the type MachineDTO class</returns>
        public MachineDTO GetMaxMachineByAddress(int masterId, int siteId)
        {
            log.LogMethodEntry(masterId, siteId);
            string selectMachineQuery = @"select * from machines
                                           where machine_address = (select isnull(max(machine_address),'0000') 
                                                                      from machines
                                                                     where (master_id = @masterId or ( @masterId = -1 and master_id is NULL )) 
                                                                       and (site_id = @site_id or @site_id = -1)) 
                                             and (site_id = @site_id or @site_id = -1)";

            SqlParameter[] selectMachineParameters = new SqlParameter[2];
            selectMachineParameters[0] = new SqlParameter("@masterId", masterId);
            selectMachineParameters[1] = new SqlParameter("@site_id", siteId);
            DataTable machineData = dataAccessHandler.executeSelectQuery(selectMachineQuery, selectMachineParameters, sqlTransaction);

            if (machineData.Rows.Count > 0)
            {
                DataRow machineDataRow = machineData.Rows[0];
                MachineDTO machineDataObject = GetMachineDTO(machineDataRow);
                MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
                List<MachineAttributeDTO> machineAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.MACHINE, machineDataObject.MachineId, siteId);
                machineDataObject.SetAttributeList(machineAttributes);
                log.LogMethodExit(machineDataObject);
                return machineDataObject;
            }
            else
            {
                log.LogMethodExit("Returning null.");
                return null;
            }
        }

        /// <summary>
        /// GetMachineList method to fetch the Machine list.
        /// </summary>
        /// <param name="selectMachineQuery"></param>
        /// <returns></returns>
        private List<MachineDTO> GetMachineList(string selectMachineQuery, bool loadAttributes = true, int siteId = -1)
        {
            log.LogMethodEntry(selectMachineQuery, loadAttributes, siteId);
            DataTable machineData = dataAccessHandler.executeSelectQuery(selectMachineQuery, null, sqlTransaction);
            if (machineData.Rows.Count > 0)
            {
                List<MachineDTO> machineList = new List<MachineDTO>();
                MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);

                foreach (DataRow machineDataRow in machineData.Rows)
                {
                    MachineDTO machineDataObject = GetMachineDTO(machineDataRow);
                    if (loadAttributes)
                    {
                        List<MachineAttributeDTO> machineAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.MACHINE, machineDataObject.MachineId, siteId);
                        machineDataObject.SetAttributeList(machineAttributes);
                    }
                    machineList.Add(machineDataObject);
                }
                log.LogMethodExit(machineList);
                return machineList;
            }
            else
            {
                log.LogMethodExit("Returning null.");
                return null;
            }
        }

        public List<MachineDTO> GetMachineDTOList(List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters, int pageNumber = 0, int pageSize = 0)
        {
            log.LogMethodEntry(searchParameters, pageNumber, pageSize);
            List<MachineDTO> machineDTOList = null;
            string selectQuery = SELECT_QUERY;
            selectQuery += GetMachinesFilterQuery(searchParameters);
            if (pageNumber >= 0 && pageSize > 0)
            {
                selectQuery += " ORDER BY m.machine_id OFFSET " + (pageNumber * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, filterParameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                machineDTOList = new List<MachineDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    MachineDTO machineDTO = GetMachineDTO(dataRow);
                    machineDTOList.Add(machineDTO);
                }
            }
            log.LogMethodExit(machineDTOList);
            return machineDTOList;
        }


        public string GetMachinesFilterQuery(List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            StringBuilder query = new StringBuilder("");
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                filterParameters.Clear();
                string selectMachineQuery = string.Empty;
                bool virtualArcadeSearch = false;
                string gameQuery = "left outer join games g on m.game_id = g.game_id  ";
                query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<MachineDTO.SearchByMachineParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == MachineDTO.SearchByMachineParameters.GAME_ID ||
                            searchParameter.Key == MachineDTO.SearchByMachineParameters.MACHINE_ID ||
                            searchParameter.Key == MachineDTO.SearchByMachineParameters.CUSTOM_DATA_SET_ID ||
                            searchParameter.Key == MachineDTO.SearchByMachineParameters.REFERENCE_MACHINE_ID ||
                            searchParameter.Key == MachineDTO.SearchByMachineParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == MachineDTO.SearchByMachineParameters.MASTER_ID ||
                            searchParameter.Key == MachineDTO.SearchByMachineParameters.ALLOWED_MACHINE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            filterParameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MachineDTO.SearchByMachineParameters.EXTERNAL_MACHINE_REFERENCE
                                || searchParameter.Key == MachineDTO.SearchByMachineParameters.MACHINE_NAME
                                || searchParameter.Key == MachineDTO.SearchByMachineParameters.QR_PLAY_IDENTIFIER
                                || searchParameter.Key == MachineDTO.SearchByMachineParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            filterParameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == MachineDTO.SearchByMachineParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            filterParameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MachineDTO.SearchByMachineParameters.IS_VIRTUAL_ARCADE)
                        {
                            query.Append(" and g.IsVirtualGame = 1 ");
                            virtualArcadeSearch = true;
                        }
                        else if (searchParameter.Key == MachineDTO.SearchByMachineParameters.ALLOWED_MACHINE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            filterParameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            filterParameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    count++;
                }
                if (virtualArcadeSearch)
                {
                    gameQuery = gameQuery + query;
                    log.LogMethodExit(gameQuery);
                    return gameQuery;
                }
            }
            log.LogMethodExit(query);
            return query.ToString();
        }


        /// <summary>
        /// Gets the MachineDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MachineDTO matching the search criteria</returns>
        public List<MachineDTO> GetMachineList(List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<MachineDTO> machinesList = new List<MachineDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectMachineQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                string gameQuery = " left outer join games g on m.game_id = g.game_id ";
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<MachineDTO.SearchByMachineParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == MachineDTO.SearchByMachineParameters.GAME_ID ||
                            searchParameter.Key == MachineDTO.SearchByMachineParameters.MACHINE_ID ||
                            searchParameter.Key == MachineDTO.SearchByMachineParameters.CUSTOM_DATA_SET_ID ||
                            searchParameter.Key == MachineDTO.SearchByMachineParameters.REFERENCE_MACHINE_ID ||
                            searchParameter.Key == MachineDTO.SearchByMachineParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == MachineDTO.SearchByMachineParameters.MASTER_ID ||
                            searchParameter.Key == MachineDTO.SearchByMachineParameters.ALLOWED_MACHINE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MachineDTO.SearchByMachineParameters.EXTERNAL_MACHINE_REFERENCE
                                || searchParameter.Key == MachineDTO.SearchByMachineParameters.MACHINE_NAME
                                || searchParameter.Key == MachineDTO.SearchByMachineParameters.QR_PLAY_IDENTIFIER
                                || searchParameter.Key == MachineDTO.SearchByMachineParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == MachineDTO.SearchByMachineParameters.IS_VIRTUAL_ARCADE)
                        {
                            selectMachineQuery += gameQuery;
                            query.Append(" and g.IsVirtualGame = 1 ");
                        }
                        else if (searchParameter.Key == MachineDTO.SearchByMachineParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    count++;
                }
                if ((searchParameters != null) && (searchParameters.Count > 0))
                {
                    selectMachineQuery = selectMachineQuery + query;
                }
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectMachineQuery, parameters.ToArray(), sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    MachineDTO machineDTO = GetMachineDTO(dataRow);
                    machinesList.Add(machineDTO);
                }
            }
            log.LogMethodExit(machinesList);
            return machinesList;
        }




        /// <summary>
        /// Gets the Out of service machine list of the type MachineDTO
        /// </summary>
        /// <returns>Returns the list of the type MachineDTO class</returns>
        public List<MachineDTO> GetOutOfServiceMachines(int siteId = -1)
        {
            log.LogMethodEntry();
            string selectMachineQuery = @"SELECT m.*  
                                          FROM GameProfileAttributeValues gpav,
	                                           GameProfileAttributes gpa,
	                                           MACHINES M
                                         WHERE gpav.attributeid=gpa.attributeid
                                           AND gpav.attributevalue = 1
                                           AND m.active_flag = 'Y'
                                           --AND m.ReferenceMachineID IS NULL
                                           AND gpav.machine_id = m.machine_id
                                           AND gpa.attribute = 'OUT_OF_SERVICE'";
            List<MachineDTO> machineDTOList = GetMachineList(selectMachineQuery, true, siteId);
            log.LogMethodExit(machineDTOList);
            return machineDTOList;
        }

        /// <summary>
        /// Gets the no game play marked out of service machine list of the type MachineDTO
        /// </summary>
        /// <returns>Returns the list of the type MachineDTO class</returns>
        public List<MachineDTO> GetNoGamePlayMarkedOutOfServiceMachines(int siteId = -1)
        {
            log.LogMethodEntry();
            string selectMachineQuery = @"SELECT m.*
                                            FROM games g,game_profile gp, 
                                                machines m join (SELECT GPAV.attributevalue,gpav.machine_id  
                                                                   FROM GameProfileAttributeValues gpav,
											                            GameProfileAttributes gpa
											                      WHERE gpav.attributeid=gpa.attributeid
											                        AND isnull(gpav.attributevalue,0) = 1
											                        AND gpa.attribute='OUT_OF_SERVICE') gpav on gpav.machine_id=m.machine_id
                                            WHERE m.game_id=g.game_id
                                            AND m.ReferenceMachineId IS NULL
                                            AND g.game_profile_id=gp.game_profile_id
                                            AND m.active_flag = 'Y'
                                            AND gp.profile_name != 'Token Dispenser' 
                                            AND gp.profile_name != 'Check Balance'
                                            AND gp.profile_name != 'Demo Reader'
                                            AND NOT EXISTS (SELECT 'x' 
                                                              FROM GameMetricView gv
                                                             WHERE gv.machine_id=m.machine_id
					                                           AND GV.technician_card='N'
					                                           AND play_date > dateadd(hh,-48,getdate())
				                                            )";
            List<MachineDTO> machineDTOList = GetMachineList(selectMachineQuery, true, siteId);
            log.LogMethodExit(machineDTOList);
            return machineDTOList;
        }

        /// <summary>
        /// Gets the No game play not marked as out of service machine list of the type MachineDTO
        /// </summary>
        /// <returns>Returns the list of the type MachineDTO class</returns>
        public List<MachineDTO> GetNoGamePlayNotMarkedOutOfServiceMachines(int siteId = -1)
        {
            log.LogMethodEntry();
            string selectMachineQuery = @"SELECT m.*
                                            FROM games g,game_profile gp, 
                                                machines m left outer join (SELECT GPAV.attributevalue,gpav.machine_id  
                                                                              FROM GameProfileAttributeValues gpav,
										                                            GameProfileAttributes gpa
									                                         WHERE gpav.attributeid=gpa.attributeid
									                                           AND gpa.attribute='OUT_OF_SERVICE') gpav on gpav.machine_id=m.machine_id
                                            WHERE m.game_id=g.game_id
                                              AND m.ReferenceMachineId IS NULL
                                              AND g.game_profile_id=gp.game_profile_id
                                              AND m.active_flag = 'Y'
                                              AND CASE 
	                                              WHEN isnull(gpav.attributevalue,0) = 0 
	                                              THEN 'Y' 
	                                              ELSE 'N' 
	                                               END = 'Y'
                                              AND gp.profile_name != 'Token Dispenser' 
                                              AND gp.profile_name != 'Check Balance'
                                              AND gp.profile_name != 'Demo Reader'
                                              AND NOT EXISTS (SELECT 'x' 
                                                                FROM GameMetricView gv
                                                               WHERE gv.machine_id=m.machine_id
					                                             AND GV.technician_card = 'N'					  
					                                             AND play_date > dateadd(hh,-48,getdate())
				                                            )";
            List<MachineDTO> machineDTOList = GetMachineList(selectMachineQuery, true, siteId);
            log.LogMethodExit(machineDTOList);
            return machineDTOList;
        }

        /// <summary>
        /// Gets machine list of the type MachineDTO with ticket dispensing issue
        /// </summary>
        /// <returns>Returns the list of the type MachineDTO class</returns>
        public List<MachineDTO> GetMachinesWithTicketDispensingIssues(int siteId = -1)
        {
            log.LogMethodEntry();
            string selectMachineQuery = @"select *
                                            from machines";
            List<MachineDTO> machineDTOList = GetMachineList(selectMachineQuery, true, siteId);
            log.LogMethodExit(machineDTOList);
            return machineDTOList;
        }

        /// <summary>
        /// Gets machine list of the type MachineDTO with No coin drop
        /// </summary>
        /// <returns>Returns the list of the type MachineDTO class</returns>
        public List<MachineDTO> GetMachinesWithNoCoinDrops(int siteId = -1)
        {
            log.LogMethodEntry();
            string selectMachineQuery = @"SELECT m.*
                                            FROM MACHINES M
                                            WHERE machine_name not like 'Check%Balance%'
                                              AND ReferenceMachineID IS NULL
                                              AND active_flag = 'Y'
                                              AND NOT EXISTS (SELECT 'X' 
                                                              FROM GameMetricViewHopper gp 
                                                             WHERE gp.machine_id = m.machine_id
					                                           AND play_date between dateadd(hour,6,convert(datetime,convert(date,getdate()))) and GETDATE()
                                                            )";
            List<MachineDTO> machineDTOList = GetMachineList(selectMachineQuery, true, siteId);
            log.LogMethodExit(machineDTOList);
            return machineDTOList;
        }

        /// <summary>
        /// Gets machine list of the type MachineDTO with one coin drop
        /// </summary>
        /// <returns>Returns the list of the type MachineDTO class</returns>
        public List<MachineDTO> GetMachinesWithOneCoinDrop(int siteId = -1)
        {
            log.LogMethodEntry();
            string selectMachineQuery = @"select m.*
                                            from machines m,
                                                (
                                                SELECT gp.machine_id,COUNT(case when gp.technician_card='Y' then gameplay_id else null end) gameplay_count
                                                  FROM GameMetricViewHopper gp
                                                 WHERE PLAY_DATE BETWEEN dateadd(hour,6,convert(datetime,convert(date,getdate()))) and GETDATE()
                                                   GROUP BY gp.machine_id
                                                 ) inline
                                           WHERE M.MACHINE_ID = INLINE.MACHINE_ID
                                             AND M.ReferenceMachineID IS NULL
                                             AND M.active_flag = 'Y'
                                             AND M.machine_name not like 'Check%Balance%'
                                             AND (CASE 
                                                  WHEN gameplay_count = 1 
	                                              THEN 1 
	                                              ELSE 0 
	                                               END) = 1";
            List<MachineDTO> machineDTOList = GetMachineList(selectMachineQuery, true, siteId);
            log.LogMethodExit(machineDTOList);
            return machineDTOList;
        }

        /// <summary>
        /// Gets machine list of the type MachineDTO with two coin drop
        /// </summary>
        /// <returns>Returns the list of the type MachineDTO class</returns>
        public List<MachineDTO> GetMachinesWithTwoCoinDrops(int siteId = -1)
        {
            log.LogMethodEntry();
            string selectMachineQuery = @"SELECT m.*
                                            FROM machines m,
                                                 (
                                                  SELECT gp.machine_id,COUNT(CASE WHEN gp.technician_card='Y' THEN gameplay_id ELSE null END) gameplay_count
                                                    FROM GameMetricViewHopper gp
                                                    WHERE PLAY_DATE BETWEEN dateadd(hour, 6, convert(datetime,convert(date,getdate()))) and GETDATE()
                                                    GROUP BY gp.machine_id
                                                  ) inline
                                            WHERE M.MACHINE_ID = INLINE.MACHINE_ID
                                              AND M.ReferenceMachineID IS NULL
                                              AND M.active_flag = 'Y'
                                              AND M.machine_name not like 'Check%Balance%'
                                              AND (CASE 
                                                   WHEN gameplay_count >= 2 
	                                               THEN 1 
	                                               ELSE 0 
	                                               END) = 1";
            List<MachineDTO> machineDTOList = GetMachineList(selectMachineQuery, true, siteId);
            log.LogMethodExit(machineDTOList);
            return machineDTOList;
        }

        /// <summary>
        /// Gets list of machines that are not inventory locations
        /// </summary>
        /// <returns>Returns the list of the type MachineDTO class</returns>
        public List<MachineDTO> GetNonInventoryLocationMachines()
        {
            log.LogMethodEntry();
            List<MachineDTO> machineDTOList = GetNonInventoryLocationMachines(-1);
            log.LogMethodExit(machineDTOList);
            return machineDTOList;
        }

        /// <summary>
        /// Gets list of machines that are not inventory locations
        /// </summary>
        /// <returns>Returns the list of the type MachineDTO class</returns>
        public List<MachineDTO> GetNonInventoryLocationMachines(int SiteID)
        {
            log.LogMethodEntry(SiteID);
            string selectMachineQuery = @"SELECT m.*  
                                          FROM machines m
                                          where not exists(select *
                                                             from location
                                                             where m.inventorylocationid = locationid
                                                                and IsActive = 'Y')
                                          and active_flag = 'Y'
                                          and (site_id = " + SiteID.ToString() + " or " + SiteID.ToString() + " = -1)";
            List<MachineDTO> machineDTOList = GetMachineList(selectMachineQuery, true, SiteID);
            log.LogMethodExit(machineDTOList);
            return machineDTOList;
        }
        /// <summary>
        /// Deletes the Machine record of passed Machine Id
        /// </summary>
        /// <param name="masterId">integer type parameter</param>
        public void DeleteMachine(int machineId)
        {
            log.LogMethodEntry(machineId);
            string query = @"DELETE FROM [machines] WHERE [machine_id] = @machineId";

            try
            {
                SqlParameter parameter = new SqlParameter("@machineId", machineId);
                dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error(query);
                throw;
            }
            log.LogMethodExit();
        }

        public DateTime? GetMachineModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate 
                            FROM (
                            select max(last_updated_date) LastUpdatedDate from machines WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdatedDate from GameProfileAttributes WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdatedDate) LastUpdatedDate from GameProfileAttributeValues WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(last_updated_date) LastUpdatedDate from Games WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(last_updated_date) LastUpdatedDate from Game_Profile WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdatedDate) LastUpdatedDate from GamePriceTier WHERE (site_id = @siteId or @siteId = -1)) a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }

        public int GetMachinesCount(List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            int machinesCount = 0;
            List<MachineDTO> machines = GetMachineList(searchParameters);
            if (machines != null && machines.Any())
            {
                machinesCount = Convert.ToInt32(machines.Count);
            }
            log.LogMethodExit(machinesCount);
            return machinesCount;
        }

        public GameCustomDTO RefreshMachine(int machineId = -1)
        {
            log.LogMethodEntry();
            GameCustomDTO gameCustomDTO = null;
            #region selectMachineQuery
            string selectMachineQuery = @"with calendar(machineId, value1, enabledOutOfService, sort) 
                            as(select machineid, rt.ThemeNumber, ISNULL(EnabledOutOfService, 0), 1 sort
	                            from GenericCalendar gc, Theme rt
	                            where (ISNULL(day, -1) = -1 or
			                            DAY = DATEPART(W, GETDATE() - 1) or
			                            DAY - 1000 = DATEPART(DAY, GETDATE()))
	                            and (Date is null or (GETDATE() >= Date and GETDATE() < Date + 1))
	                            and (
								      (ISNULL(FromTime, 0) <= DATEPART(HOUR, GETDATE()) + (DATEPART(mi,getdate())/100.00)
									    AND ISNULL(DAY, -1) != -1
									  )
									  or
									  (CASE WHEN convert(int,round(ISNULL(FromTime, 0), 0)) >  convert(int,round(isnull(Totime, 24), 0))
						                        and datepart(hour, getdate()) BETWEEN 0 AND convert(int,round(isnull(Totime, 24), 0))
					                        THEN dateadd(MINUTE,Convert(int,right(ISNULL(FromTime, 0),2)), dateadd(HOUR, convert(int,round(ISNULL(FromTime, 0), 0)), convert(datetime,convert(date,getdate() - 1))))
					                        ELSE  dateadd(MINUTE,Convert(int,right(ISNULL(FromTime, 0),2)), dateadd(HOUR, convert(int,round(ISNULL(FromTime, 0), 0)), convert(datetime,convert(date,getdate()))))
				                        END <= GETDATE()
										AND ISNULL(DAY, -1) = -1
									  )
									)
	                            and CASE WHEN convert(int,round(ISNULL(FromTime, 0), 0)) >  convert(int,round(isnull(Totime, 24), 0))
						                        and datepart(hour, getdate()) NOT BETWEEN 0 and convert(int,round(isnull(Totime, 24), 0))
				                         THEN dateadd(MINUTE,Convert(int,right(isnull(Totime, 24),2)), 
		                                                    dateadd(HOUR, convert(int,round(isnull(Totime, 24), 0)), 
									                                 convert(datetime,convert(date,getdate() + 1))))
				                         else dateadd(MINUTE,Convert(int,right(isnull(Totime, 24),2)), 
		                                                    dateadd(HOUR, convert(int,round(isnull(Totime, 24), 0)), 
									                                 convert(datetime,convert(date,getdate()))))
			                          END > GETDATE()
								and gc.ThemeId is not null
	                            and gc.ThemeId = rt.Id
	                            and CalendarType = 'THEME'
	                            and MachineId is not null
	                            union all
	                            select machine_id, rh.themeNumber, 0, 2
  	                                from machines m, Theme rh
	                                where m.ThemeId = rh.Id
	                            union all
	                            select MachineGroupMachines.MachineId, rt.ThemeNumber, ISNULL(EnabledOutOfService, 0), 3
	                                from GenericCalendar, MachineGroupMachines, Theme rt
	                                where GenericCalendar.MachineGroupId = MachineGroupMachines.MachineGroupId 
	                                and (ISNULL(day, -1) = -1 or
			                            DAY = DATEPART(W, GETDATE() - 1) or
			                            DAY - 1000 = DATEPART(DAY, GETDATE()))
		                            and (Date is null or (GETDATE() >= Date and GETDATE() < Date + 1))
		                            and (
								      (ISNULL(FromTime, 0) <= DATEPART(HOUR, GETDATE()) + (DATEPART(mi,getdate())/100.00)
									    AND ISNULL(DAY, -1) != -1
									  )
									  or
									  (CASE WHEN convert(int,round(ISNULL(FromTime, 0), 0)) >  convert(int,round(isnull(Totime, 24), 0))
						                        and datepart(hour, getdate()) BETWEEN 0 AND convert(int,round(isnull(Totime, 24), 0))
					                        THEN dateadd(MINUTE,Convert(int,right(ISNULL(FromTime, 0),2)), dateadd(HOUR, convert(int,round(ISNULL(FromTime, 0), 0)), convert(datetime,convert(date,getdate() - 1))))
					                        ELSE  dateadd(MINUTE,Convert(int,right(ISNULL(FromTime, 0),2)), dateadd(HOUR, convert(int,round(ISNULL(FromTime, 0), 0)), convert(datetime,convert(date,getdate()))))
				                        END <= GETDATE()
										AND ISNULL(DAY, -1) = -1
									  )
									)
	                            and CASE WHEN convert(int,round(ISNULL(FromTime, 0), 0)) >  convert(int,round(isnull(Totime, 24), 0))
						                        and datepart(hour, getdate()) NOT BETWEEN 0 and convert(int,round(isnull(Totime, 24), 0))
				                         THEN dateadd(MINUTE,Convert(int,right(isnull(Totime, 24),2)), 
		                                                    dateadd(HOUR, convert(int,round(isnull(Totime, 24), 0)), 
									                                 convert(datetime,convert(date,getdate() + 1))))
				                         else dateadd(MINUTE,Convert(int,right(isnull(Totime, 24),2)), 
		                                                    dateadd(HOUR, convert(int,round(isnull(Totime, 24), 0)), 
									                                 convert(datetime,convert(date,getdate()))))
			                          END > GETDATE()
								and GenericCalendar.ThemeId is not null
		                            and GenericCalendar.ThemeId = rt.Id
		                            and CalendarType = 'THEME'
	                            union all
	                            select machines.machine_id, rt.ThemeNumber, ISNULL(EnabledOutOfService, 0), 4
  	                                from GenericCalendar, games, machines, Theme rt
	                                where GenericCalendar.GameProfileId = games.game_profile_id
	                                and machines.game_id = games.game_id
	                                and (ISNULL(day, -1) = -1 or
			                            DAY = DATEPART(W, GETDATE() - 1) or
			                            DAY - 1000 = DATEPART(DAY, GETDATE()))
	                                and (Date is null or (GETDATE() >= Date and GETDATE() < Date + 1))
	                                and (
								      (ISNULL(FromTime, 0) <= DATEPART(HOUR, GETDATE()) + (DATEPART(mi,getdate())/100.00)
									    AND ISNULL(DAY, -1) != -1
									  )
									  or
									  (CASE WHEN convert(int,round(ISNULL(FromTime, 0), 0)) >  convert(int,round(isnull(Totime, 24), 0))
						                        and datepart(hour, getdate()) BETWEEN 0 AND convert(int,round(isnull(Totime, 24), 0))
					                        THEN dateadd(MINUTE,Convert(int,right(ISNULL(FromTime, 0),2)), dateadd(HOUR, convert(int,round(ISNULL(FromTime, 0), 0)), convert(datetime,convert(date,getdate() - 1))))
					                        ELSE  dateadd(MINUTE,Convert(int,right(ISNULL(FromTime, 0),2)), dateadd(HOUR, convert(int,round(ISNULL(FromTime, 0), 0)), convert(datetime,convert(date,getdate()))))
				                        END <= GETDATE()
										AND ISNULL(DAY, -1) = -1
									  )
									)
	                            and CASE WHEN convert(int,round(ISNULL(FromTime, 0), 0)) >  convert(int,round(isnull(Totime, 24), 0))
						                        and datepart(hour, getdate()) NOT BETWEEN 0 and convert(int,round(isnull(Totime, 24), 0))
				                         THEN dateadd(MINUTE,Convert(int,right(isnull(Totime, 24),2)), 
		                                                    dateadd(HOUR, convert(int,round(isnull(Totime, 24), 0)), 
									                                 convert(datetime,convert(date,getdate() + 1))))
				                         else dateadd(MINUTE,Convert(int,right(isnull(Totime, 24),2)), 
		                                                    dateadd(HOUR, convert(int,round(isnull(Totime, 24), 0)), 
									                                 convert(datetime,convert(date,getdate()))))
			                          END	 > GETDATE()
								and GenericCalendar.ThemeId is not null
	                                and GenericCalendar.ThemeId = rt.Id
	                                and CalendarType = 'THEME' 
	                            union all
	                            select m.machine_id, rh.ThemeNumber, 0, 5
	                            from game_profile gp, games g, machines m, Theme rh
	                            where gp.ThemeId = rh.Id
	                            and gp.game_profile_id = g.game_profile_id
	                            and m.game_id = g.game_id)
                            select m.game_id, m.machine_address, m.machine_name, number_of_coins, group_timer, 
                            m.IPAddress, m.TCPPort, machine_id, m.MACAddress, 
                            isnull(isnull(g.play_credits, gp.play_credits), 0) play_credits, 
                            isnull(isnull(g.vip_play_credits, gp.vip_play_credits), 0) vip_play_credits, 
                            gp.game_profile_id, isnull(m.timer_machine, 'N') timer_machine, isnull(m.timer_interval, 0) timer_interval, 
                            gp.TokenRedemption, isnull(gp.TokenPrice, 0) TokenPrice, gp.PhysicalToken, 
                            gp.RedeemTokenTo, m.ticket_mode, gp.TicketEater, 
                            gp.credit_allowed, gp.bonus_allowed, gp.courtesy_allowed, gp.time_allowed, m.ReaderType, 
                            isnull(case m.ShowAd when 'D' then gp.showAd else m.ShowAd end, gp.ShowAd) ShowAd, 
                            gp.UserIdentifier GPUserIdentifier, g.UserIdentifier GameUserIdentifier, 
                            m.InventoryLocationId, m.ReferenceMachineId, isnull(gp.ForceRedeemToCard,0) ForceRedeemToCard,
                            (select value1 
                                from (select top 1 * 
                                        from (select * from calendar) v1 
	                                where m.machine_id = v1.MachineId
	                            order by sort) v2) ThemeNumber,
                            (select EnabledOutOfService 
                                from (select top 1 * 
                                        from (select * from calendar) v1 
	                                where m.machine_id = v1.MachineId
	                            order by sort) v2) EnabledOutOfService,
							ISNULL(g.GameUrl, '') GameURL, 
							ISNULL(g.IsExternalGame, 0) IsExternalGame,
							ISNULL(m.externalMachineReference, '') ExternalMachineReference,
							ISNULL(m.QRPlayIdentifier, '') QRPlayIdentifier,
							ISNULL(m.EraseQRPlayIdentifier, 0) EraseQRPlayIdentifier
                        from machines m, game_profile gp, games g 
                        where m.game_id = g.game_id 
                        and m.active_flag = 'Y' 
                        and g.game_profile_id = gp.game_profile_id 
                        and m.machine_id = @machineId
                        order by m.machine_address";
            #endregion 
            SqlParameter parameter = new SqlParameter("@machineId", machineId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectMachineQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                DataRow machineDataRow = dataTable.Rows[0];
                gameCustomDTO = GetGameCustomDTO(machineDataRow);

                log.LogMethodExit(gameCustomDTO);
                return gameCustomDTO;
            }
            else
            {
                log.LogMethodExit("Returning null.");
                return null;
            }
        }
    }
}