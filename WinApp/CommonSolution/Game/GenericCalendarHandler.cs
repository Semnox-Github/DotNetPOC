/********************************************************************************************
 * Project Name - Generic Calendar Handler                                                                         
 * Description  - Handler of the GenericCalendar class
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.40        06-Oct-2018    Jagan Mohana Rao     Created new class to fetch and update Generic Calendar.
 *2.50.0      12-dec-2018    Guru S A             Who column changes
 *2.70.2        29-Jul-2019    Deeksha              Modified:Added getsqlParameter(),Insert/Update Method returns DTO.
 *                                                Added GetGenericCalender function which  Returns the list of GenericCalendarDTO matching the search criteria
 *            21-Oct-2019    Jagan Mohana         GetGenericCalendarRow() code changed for genericColId and moduleName
 *2.90        22-jul-2020    Mushahid Faizan      Modified : GetSQLParameters and added  case statement for MACHINEGROUPS in  GetGenericCalendar method.
 *2.110.0     8-Dec-2020      Prajwal S           Modified : Get using Searchparameters. Added GenericCalendar Handler Constructor with sqltransaction constructor.
 *                                                Added GetGenericcalendar to get single DTO.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Game
{
    class GenericCalendarHandler
    {
        private const string SELECT_QUERY = @"SELECT * FROM GenericCalendar AS g ";
        private static readonly Dictionary<GenericCalendarDTO.SearchByGenericCalendarParameters, string> DBSearchParameters = new Dictionary<GenericCalendarDTO.SearchByGenericCalendarParameters, string>
            {
                {GenericCalendarDTO.SearchByGenericCalendarParameters.SITE_ID, "g.site_id"},
                {GenericCalendarDTO.SearchByGenericCalendarParameters.IS_ACTIVE, "g.ISActive"},
                {GenericCalendarDTO.SearchByGenericCalendarParameters.MASTER_ENTITY_ID, "g.MasterEntityId"},
                {GenericCalendarDTO.SearchByGenericCalendarParameters.CALENDAR_ID, "g.CalendarId"}
            };
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private string connstring;
        private GenericCalendarDTO genericCalendar;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor.
        /// </summary>
        /// <param name="executioncontext">executioncontext</param>
        /// <param name="genericcalendardto">genericcalendardto</param>
        public GenericCalendarHandler(ExecutionContext executioncontext, GenericCalendarDTO genericcalendardto, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, genericcalendardto, sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            executionContext = executioncontext;
            genericCalendar = genericcalendardto;
            connstring = dataAccessHandler.ConnectionString;
            log.LogMethodExit();
        }

        public GenericCalendarHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating GamePlayInfo Record.
        /// </summary>
        /// <param name="genericCalendarDTO">genericCalendarDTO object</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(GenericCalendarDTO genericCalendarDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(genericCalendarDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CalendarId", genericCalendarDTO.CalendarId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CalendarType", genericCalendarDTO.CalendarType));
            if (genericCalendarDTO.Entity == "MACHINES")
            {
                parameters.Add(dataAccessHandler.GetSQLParameter("@MachineId", genericCalendarDTO.EntityId,true));
                parameters.Add(new SqlParameter("@GameProfileId", DBNull.Value));
                parameters.Add(new SqlParameter("@MachineGroupId", DBNull.Value));
            }
            else if (genericCalendarDTO.Entity == "GAME_PROFILE")
            {
                parameters.Add(dataAccessHandler.GetSQLParameter("@GameProfileId", genericCalendarDTO.EntityId,true));
                parameters.Add(new SqlParameter("@MachineId", DBNull.Value));
                parameters.Add(new SqlParameter("@MachineGroupId", DBNull.Value));
            }
            else if (genericCalendarDTO.Entity == "MACHINEGROUPS")
            {
                parameters.Add(dataAccessHandler.GetSQLParameter("@MachineGroupId", genericCalendarDTO.EntityId, true));
                parameters.Add(new SqlParameter("@MachineId", DBNull.Value));
                parameters.Add(new SqlParameter("@GameProfileId", DBNull.Value));
            }
            //parameters.Add(dataAccessHandler.GetSQLParameter("@MachineGroupId", genericCalendarDTO.MachineGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Day", genericCalendarDTO.Day));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Date", genericCalendarDTO.Date == null ? DBNull.Value : (object)genericCalendarDTO.Date));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FromTime", string.IsNullOrEmpty(genericCalendarDTO.FromTime) ? DBNull.Value : (object)genericCalendarDTO.FromTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ToTime", string.IsNullOrEmpty(genericCalendarDTO.ToTime) ? DBNull.Value : (object)genericCalendarDTO.ToTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Value1", (genericCalendarDTO.Value1 == string.Empty || genericCalendarDTO.Value1 == null) ? DBNull.Value : (object)genericCalendarDTO.Value1));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Value2", (genericCalendarDTO.Value2 == string.Empty || genericCalendarDTO.Value2 == null) ? DBNull.Value : (object)genericCalendarDTO.Value2));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Value3", (genericCalendarDTO.Value3 == string.Empty || genericCalendarDTO.Value3 == null) ? DBNull.Value : (object)genericCalendarDTO.Value3));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Value4", (genericCalendarDTO.Value4 == string.Empty || genericCalendarDTO.Value4 == null) ? DBNull.Value : (object)genericCalendarDTO.Value4));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ThemeId", genericCalendarDTO.ThemeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EnabledOutOfService", genericCalendarDTO.EnabledOutOfService));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", genericCalendarDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", genericCalendarDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdateBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the InsertGenericCalendar
        /// </summary>
        /// <param name="game">GameDTO</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>        
        public GenericCalendarDTO InsertGenericCalendar(GenericCalendarDTO genericCalendarDTO)
        {
            log.LogMethodEntry(genericCalendarDTO);
            string loginId = executionContext.GetUserId();
            int siteId = executionContext.GetSiteId();
            string insertGenericCalendarQuery = @"insert into GenericCalendar 
                                                            (
                                                              CalendarType, 
                                                              MachineId,
                                                              GameProfileId,
                                                              MachineGroupId, 
                                                              Day,
                                                              Date,
                                                              FromTime, 
                                                              ToTime, 
                                                              Value1,
                                                              Value2,
                                                              Value3,
                                                              Value4,
                                                              Guid,
                                                              site_id,
                                                              MasterEntityId, 
                                                              isActive,
                                                              EnabledOutOfService,
                                                              ThemeId,
                                                              CreatedBy,
                                                              CreationDate,
                                                              LastUpdatedBy,
                                                              LastUpdateDate
                                                            ) 
                                                    values 
                                                            (
                                                              @CalendarType,
                                                              @MachineId,
                                                              @GameProfileId,
                                                              @MachineGroupId,
                                                              @Day,
                                                              @Date,
                                                              @FromTime,
                                                              @ToTime,
                                                              @Value1, 
                                                              @Value2, 
                                                              @Value3,
                                                              @Value4,
                                                              NEWID(),
                                                              @site_id,
                                                              @MasterEntityId, 
                                                              @isActive,
                                                              @EnabledOutOfService,
                                                              @ThemeId,
                                                              @CreatedBy,
                                                              getdate(),
                                                              @CreatedBy,
                                                              getdate()
                                                            ) SELECT * FROM GenericCalendar WHERE CalendarId = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertGenericCalendarQuery, GetSQLParameters(genericCalendarDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshGenericCalendarDTO(genericCalendarDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting genericCalendarDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(genericCalendarDTO);
            return genericCalendarDTO;
        }

        /// <summary>
        /// Update Generic Calendar
        /// </summary>
        /// <param name="genericCalendar">GenericCalendarDTO</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public GenericCalendarDTO UpdateGenericCalendar(GenericCalendarDTO genericCalendarDTO)
        {
            log.LogMethodEntry(genericCalendarDTO);

            string loginId = executionContext.GetUserId();
            int siteId = executionContext.GetSiteId();
            string updateGameQuery = @"update GenericCalendar 
                                         set Day = @Day,
                                             Date = @Date,
                                             FromTime = @FromTime,
                                             ToTime = @ToTime,
                                             Value1 = @Value1, 
                                             EnabledOutOfService = @EnabledOutOfService,
                                             ThemeId = @ThemeId,
                                             isActive = @IsActive,
                                             LastUpdatedBy = @CreatedBy,
                                             LastUpdateDate = getdate()
                                       where CalendarId = @CalendarId
                            SELECT * FROM GenericCalendar WHERE CalendarId = @CalendarId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateGameQuery, GetSQLParameters(genericCalendarDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshGenericCalendarDTO(genericCalendarDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating genericCalendarDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(genericCalendarDTO);
            return genericCalendarDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="genericCalendarDTO">genericCalendarDTO+ object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshGenericCalendarDTO(GenericCalendarDTO genericCalendarDTO, DataTable dt)
        {
            log.LogMethodEntry(genericCalendarDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                genericCalendarDTO.CalendarId = Convert.ToInt32(dt.Rows[0]["CalendarId"]);
                genericCalendarDTO.LastupdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                genericCalendarDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                genericCalendarDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                genericCalendarDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                genericCalendarDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                genericCalendarDTO.site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to GameDTO class type
        /// </summary>
        /// <param name="GenericCalendarDTO">DataRow genericCalendarDataRow</param>
        /// <returns>Returns GenericCalendarDTO</returns>
        private GenericCalendarDTO GetGenericCalendarRow(DataRow genericCalendarDataRow, string genericColId, string moduleName)
        {
            log.LogMethodEntry(genericCalendarDataRow, genericColId, moduleName);
            GenericCalendarDTO gameDataObject = new GenericCalendarDTO(Convert.ToInt32(genericCalendarDataRow["CalendarId"]),
                                            genericCalendarDataRow["CalendarType"] == DBNull.Value ? string.Empty : genericCalendarDataRow["CalendarType"].ToString(),
                                            genericCalendarDataRow[genericColId] == DBNull.Value ? -1 : Convert.ToInt32(genericCalendarDataRow[genericColId]),
                                            genericCalendarDataRow[moduleName] == DBNull.Value ? string.Empty : Convert.ToString(genericCalendarDataRow[moduleName]),
                                            genericCalendarDataRow["MachineGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(genericCalendarDataRow["MachineGroupId"]),
                                            genericCalendarDataRow["Day"] == DBNull.Value ? -1 : Convert.ToInt32(genericCalendarDataRow["Day"]),
                                            genericCalendarDataRow["Date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(genericCalendarDataRow["Date"]),
                                            genericCalendarDataRow["FromTime"] == DBNull.Value ? string.Empty : genericCalendarDataRow["FromTime"].ToString(),
                                            genericCalendarDataRow["ToTime"] == DBNull.Value ? string.Empty : genericCalendarDataRow["ToTime"].ToString(),
                                            genericCalendarDataRow["Value1"] == DBNull.Value ? string.Empty : genericCalendarDataRow["Value1"].ToString(),
                                            genericCalendarDataRow["Value2"] == DBNull.Value ? string.Empty : genericCalendarDataRow["Value2"].ToString(),
                                            genericCalendarDataRow["Value3"] == DBNull.Value ? string.Empty : genericCalendarDataRow["Value3"].ToString(),
                                            genericCalendarDataRow["Value4"] == DBNull.Value ? string.Empty : genericCalendarDataRow["Value4"].ToString(),
                                            genericCalendarDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(genericCalendarDataRow["Guid"]),
                                            genericCalendarDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(genericCalendarDataRow["site_id"]),
                                            genericCalendarDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(genericCalendarDataRow["SynchStatus"]),
                                            genericCalendarDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(genericCalendarDataRow["MasterEntityId"]),
                                            genericCalendarDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(genericCalendarDataRow["IsActive"]),
                                            genericCalendarDataRow["EnabledOutOfService"] == DBNull.Value ? true : Convert.ToBoolean(genericCalendarDataRow["EnabledOutOfService"]),
                                            genericCalendarDataRow["ThemeId"] == DBNull.Value ? -1 : Convert.ToInt32(genericCalendarDataRow["ThemeId"]),
                                            genericCalendarDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(genericCalendarDataRow["CreatedBy"]),
                                            genericCalendarDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(genericCalendarDataRow["CreationDate"]),
                                            genericCalendarDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(genericCalendarDataRow["LastUpdatedBy"]),
                                            genericCalendarDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(genericCalendarDataRow["LastUpdateDate"])
                                            );
            log.LogMethodExit(gameDataObject);
            return gameDataObject;
        }

        /// <summary>
        /// Gets the game data of passed game id
        /// </summary>
        /// <param name="genericId">generic Id</param>
        /// <returns>Returns GenericCalendarDTO</returns>
        public List<GenericCalendarDTO> GetGenericCalendarList(string moduleName, int genericId, string activeFlag = "")
        {
            log.LogMethodEntry(moduleName, genericId);
            try
            {
                List<GenericCalendarDTO> genericCalendar = new List<GenericCalendarDTO>();

                string queryFields = "";
                string Query = "";
                string name = "";
                string isActive = "";
                if (activeFlag.ToString() == "1")
                {
                    isActive = activeFlag;
                }
                switch (moduleName.ToUpper().ToString())
                {
                    case "GAME_PROFILE":
                        if (isActive.ToString() == "1")
                        {
                            Query = string.Format("Select g.*,gp.profile_name from GenericCalendar g inner join game_profile gp on gp.game_profile_id = g.GameProfileId where gp.game_profile_id = {0} and g.ISActive = {1}", genericId, isActive.ToString() == "1" ? 1 : 0);
                        }
                        else
                        {
                            Query = string.Format("Select g.*,gp.profile_name from GenericCalendar g inner join game_profile gp on gp.game_profile_id = g.GameProfileId where gp.game_profile_id = {0}", genericId);
                        }
                        name = "profile_name";
                        queryFields = "GameProfileId";
                        break;

                    case "MACHINES":
                        queryFields = "MachineId";
                        name = "machine_name";
                        if (isActive.ToString() == "1")
                        {
                            Query = string.Format("Select g.*,m.machine_name from GenericCalendar g inner join machines m on g.MachineId = m.machine_id where g.MachineId = {0} and g.ISActive = {1}", genericId, isActive.ToString() == "1" ? 1 : 0);
                        }
                        else
                        {
                            Query = string.Format("Select g.*,m.machine_name from GenericCalendar g inner join machines m on g.MachineId = m.machine_id where g.MachineId = {0}", genericId);
                        }
                        break;
                    case "MACHINEGROUPS":
                        queryFields = "MachineGroupId";
                        name = "GroupName";
                        if (isActive.ToString() == "1")
                        {
                            Query = string.Format("Select g.*,m.GroupName from GenericCalendar g inner join MachineGroups m on g.MachineGroupId = m.MachineGroupId where g.MachineGroupId = {0} and g.ISActive = {1}", genericId, isActive.ToString() == "1" ? 1 : 0);
                        }
                        else
                        {
                            Query = string.Format("Select g.*,m.GroupName from GenericCalendar g inner join MachineGroups m on g.MachineGroupId = m.MachineGroupId where g.MachineGroupId = {0}", genericId);
                        }
                        break;
                }

                DataTable lookUpListData = dataAccessHandler.executeSelectQuery(Query, null);
                if (lookUpListData.Rows.Count > 0)
                {
                    foreach (DataRow lookUpMasterDataRow in lookUpListData.Rows)
                    {
                        GenericCalendarDTO dto = GetGenericCalendarRow(lookUpMasterDataRow, queryFields, name);
                        genericCalendar.Add(dto);
                    }
                }
                log.LogMethodExit(genericCalendar);
                return genericCalendar;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw ;
            }
        }
        /// <summary>
        /// Gets the game data of passed game id
        /// </summary>
        /// <param name="genericId">generic Id</param>
        /// <returns>Returns GenericCalendarDTO</returns>

        public GenericCalendarDTO GetGenericCalendar(string moduleName, int genericId, int Id, string activeFlag = "")
        {
            log.LogMethodEntry(moduleName, genericId, Id);
            try
            {
                GenericCalendarDTO genericCalendar = null;

                string Query = "";
                string name = "";
                string queryFields = "";
                string isActive = "";
                if (activeFlag.ToString() == "1")
                {
                    isActive = activeFlag;
                }
                switch (moduleName.ToUpper().ToString())
                {
                    case "GAME_PROFILE":
                        if (isActive.ToString() == "1")
                        {
                            Query = string.Format("Select g.*,gp.profile_name from GenericCalendar g inner join game_profile gp on gp.game_profile_id = g.GameProfileId where gp.game_profile_id = {0} and g.ISActive = {1} and g.CalendarId = {2}", genericId, isActive.ToString() == "1" ? 1 : 0, Id);
                        }
                        else
                        {
                            Query = string.Format("Select g.*,gp.profile_name from GenericCalendar g inner join game_profile gp on gp.game_profile_id = g.GameProfileId where gp.game_profile_id = {0} and g.CalendarId = {1}", genericId, Id);
                        }
                        name = "profile_name";
                        queryFields = "GameProfileId";
                        break;

                    case "MACHINES":
                        queryFields = "MachineId";
                        name = "machine_name";
                        if (isActive.ToString() == "1")
                        {
                            Query = string.Format("Select g.*,m.machine_name from GenericCalendar g inner join machines m on g.MachineId = m.machine_id where g.MachineId = {0} and g.ISActive = {1} and g.CalendarId = {2}", genericId, isActive.ToString() == "1" ? 1 : 0, Id);
                        }
                        else
                        {
                            Query = string.Format("Select g.*,m.machine_name from GenericCalendar g inner join machines m on g.MachineId = m.machine_id where g.MachineId = {0} and g.CalendarId = {1}", genericId, Id);
                        }
                        break;
                    case "MACHINEGROUPS":
                        queryFields = "MachineGroupId";
                        name = "GroupName";
                        if (isActive.ToString() == "1")
                        {
                            Query = string.Format("Select g.*,m.GroupName from GenericCalendar g inner join MachineGroups m on g.MachineGroupId = m.MachineGroupId where g.MachineGroupId = {0} and g.ISActive = {1} and g.CalendarId = {2}", genericId, isActive.ToString() == "1" ? 1 : 0, Id);
                        }
                        else
                        {
                            Query = string.Format("Select g.*,m.GroupName from GenericCalendar g inner join MachineGroups m on g.MachineGroupId = m.MachineGroupId where g.MachineGroupId = {0} and g.CalendarId = {1}", genericId, Id);
                        }
                        break;
                }

                DataTable lookUpListData = dataAccessHandler.executeSelectQuery(Query, null, sqlTransaction);
                if (lookUpListData.Rows.Count > 0)
                {
                    foreach (DataRow lookUpMasterDataRow in lookUpListData.Rows)
                    {
                        genericCalendar = GetGenericCalendarRow(lookUpMasterDataRow, queryFields, name);
                    }
                }
                log.LogMethodExit(genericCalendar);
                return genericCalendar;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
        }

        /// <summary>
        /// Gets the GenericCalendarDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTrxn">sqlTrxn</param>
        /// <returns>Returns the list of GenericCalendarDTO matching the search criteria</returns>
        public List<GenericCalendarDTO> GetGenereicCalendar(List<KeyValuePair<GenericCalendarDTO.SearchByGenericCalendarParameters, string>> searchParameters, string genericColId, string moduleName, int genericId, SqlTransaction sqlTransaction = null) // Modified Code
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<GenericCalendarDTO> genericCalendarsList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string Query = "";
            if (genericColId == "GameProfileId" && moduleName == "profile_name")
            {
                Query = string.Format("Select g.*,gp.profile_name from GenericCalendar g inner join game_profile gp on gp.game_profile_id = g.GameProfileId where gp.game_profile_id = {0}", genericId);
            }
            else if (genericColId == "MachineId" && moduleName == "machine_name")
            {
                Query = string.Format("Select g.*,m.machine_name from GenericCalendar g inner join machines m on g.MachineId = m.machine_id where g.MachineId = {0}", genericId);
            }
            else if (genericColId == "MachineGroupId" && moduleName == "GroupName")
            {
                Query = string.Format("Select g.*,m.GroupName from GenericCalendar g inner join MachineGroups m on g.MachineGroupId = m.MachineGroupId where g.MachineGroupId = {0}", genericId);
            }
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                StringBuilder query = new StringBuilder(" and ");
                foreach (KeyValuePair<GenericCalendarDTO.SearchByGenericCalendarParameters, string> searchParameter in searchParameters)
                {
                    joiner = (count == 0) ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key.Equals(GenericCalendarDTO.SearchByGenericCalendarParameters.CALENDAR_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == GenericCalendarDTO.SearchByGenericCalendarParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == GenericCalendarDTO.SearchByGenericCalendarParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == GenericCalendarDTO.SearchByGenericCalendarParameters.IS_ACTIVE) // bit
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));

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
                if (searchParameters.Count > 0)
                {
                    Query = Query + query;

                }

            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(Query, parameters.ToArray(), sqlTransaction);
            genericCalendarsList = new List<GenericCalendarDTO>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    GenericCalendarDTO genericCalendarDTO = GetGenericCalendarRow(dataRow, genericColId, moduleName);
                    genericCalendarsList.Add(genericCalendarDTO);
                }
            }
            log.LogMethodExit(genericCalendarsList);
            return genericCalendarsList;
        }


        public int Delete(int calendarId)
        {
            log.LogMethodEntry(calendarId);
            try
            {
                string deleteQuery = @"delete from GenericCalendar where CalendarId = @CalendarId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@CalendarId", calendarId);
                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
        }
    }
}