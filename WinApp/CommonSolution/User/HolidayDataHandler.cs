/********************************************************************************************
 * Project Name - User
 * Description  - Data Handler File for Holiday 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        10-June-2019   Divya A                 Created 
 *2.90        20-May-2020   Vikas Dwivedi           Modified as per the Standard CheckList
 ********************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Collections.Generic;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Holiday Data Handler - Handles insert, update and select of Holiday object
    /// </summary>
    public class HolidayDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM Holiday as h ";

        /// <summary>
        /// Dictionary for searching Parameters for the Holiday object.
        /// </summary>
        private static readonly Dictionary<HolidayDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<HolidayDTO.SearchByParameters, string>
        {
            { HolidayDTO.SearchByParameters.HOLIDAY_ID,"h.HolidayId"},
            { HolidayDTO.SearchByParameters.NAME,"h.Name"},
            { HolidayDTO.SearchByParameters.DATE,"h.Date"},
            { HolidayDTO.SearchByParameters.SITE_ID,"h.site_id"},
            { HolidayDTO.SearchByParameters.IS_ACTIVE,"h.IsActive"},
            { HolidayDTO.SearchByParameters.MASTER_ENTITY_ID,"h.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for Holiday.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction objects</param>
        public HolidayDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Holiday Record.
        /// </summary>
        /// <param name="holidayDTO">HolidayDTO object is passed as parameter </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(HolidayDTO holidayDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(holidayDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@HolidayId", holidayDTO.HolidayId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", holidayDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Date", holidayDTO.Date));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", holidayDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", holidayDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Converts the Data row object to HolidayDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of HolidayDTO</returns>
        private HolidayDTO GetHolidayDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            HolidayDTO holidayDTO = new HolidayDTO(dataRow["HolidayId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["HolidayId"]),
                                                dataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Name"]),
                                                dataRow["Date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["Date"]),
                                                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]));
            return holidayDTO;
        }

        /// <summary>
        /// Gets the Holiday data of passed Holiday ID
        /// </summary>
        /// <param name="holidayId">holidayId of HolidayDTO is passed as parameter</param>
        /// <returns>Returns HolidayDTO</returns>
        public HolidayDTO GetHolidayDTO(int holidayId)
        {
            log.LogMethodEntry(holidayId);
            HolidayDTO result = null;
            string query = SELECT_QUERY + @" WHERE h.HolidayId = @HolidayId";
            SqlParameter parameter = new SqlParameter("@HolidayId", holidayId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetHolidayDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="holidayDTO">HolidayDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        private void RefreshHolidayDTO(HolidayDTO holidayDTO, DataTable dt)
        {
            log.LogMethodEntry(holidayDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                holidayDTO.HolidayId = Convert.ToInt32(dt.Rows[0]["HolidayId"]);
                holidayDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                holidayDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                holidayDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                holidayDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                holidayDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                holidayDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
        
        /// <summary>
        ///  Inserts the record to the Holiday Table. 
        /// </summary>
        /// <param name="holidayDTO">HolidayDTO object is passed as parameter </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated HolidayDTO</returns>
        public HolidayDTO Insert(HolidayDTO holidayDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(holidayDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[Holiday]
                            (
                            Name,
                            Date,
                            LastUpdatedBy,
                            LastUpdatedDate,
                            site_id,
                            Guid,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            IsActive
                            )
                            VALUES
                            (
                            @Name,
                            @Date,
                            @LastUpdatedBy,
                            GETDATE(),
                            @site_id,
                            NEWID(),
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @IsActive
                            )
                            SELECT * FROM Holiday WHERE HolidayId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(holidayDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshHolidayDTO(holidayDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting HolidayDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(holidayDTO);
            return holidayDTO;
        }

        /// <summary>
        /// Update the record in the Holiday Table. 
        /// </summary>
        /// <param name="holidayDTO">HolidayDTO object is passed as parameter </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated HolidayDTO</returns>
        public HolidayDTO Update(HolidayDTO holidayDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(holidayDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[Holiday]
                             SET
                            Name =  @Name,
                            Date =  @Date,
                            LastUpdatedBy =  @LastUpdatedBy,
                            LastUpdatedDate =  GETDATE(),
                            --site_id =  @site_id,
                            MasterEntityId =  @MasterEntityId,
                            IsActive  = @IsActive
                            WHERE HolidayId = @HolidayId
                            SELECT * FROM Holiday WHERE HolidayId = @HolidayId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(holidayDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshHolidayDTO(holidayDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating HolidayDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(holidayDTO);
            return holidayDTO;
        }

        /// <summary>
        /// Returns the List of HolidayDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the list of HolidayDTO </returns>
        public List<HolidayDTO> GetHolidayDTOList(List<KeyValuePair<HolidayDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<HolidayDTO> holidayDTOList = new List<HolidayDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<HolidayDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == HolidayDTO.SearchByParameters.HOLIDAY_ID ||
                            searchParameter.Key == HolidayDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == HolidayDTO.SearchByParameters.NAME)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == HolidayDTO.SearchByParameters.DATE)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == HolidayDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == HolidayDTO.SearchByParameters.IS_ACTIVE) // bit
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
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
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    HolidayDTO holidayDTO = GetHolidayDTO(dataRow);
                    holidayDTOList.Add(holidayDTO);
                }
            }
            log.LogMethodExit(holidayDTOList);
            return holidayDTOList;
        }

        /// <summary>
        /// Delete Method for Holiday
        /// </summary>
        /// <param name="holidayId"></param>
        /// <returns></returns>
        internal int DeleteHoliday(int holidayId)
        {
            log.LogMethodEntry(holidayId);
            try
            {
                string deleteQuery = @"delete from holiday where HolidayId = @holidayId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@holidayId", holidayId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Get Year
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<int> GetYear(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                List<int> year = new List<int>();
                string getYearQuery = @"select DATEPART(YEAR, getdate()) - datepart(YEAR, isnull(min(Date), getdate())) from Holiday";
                SqlParameter[] parameters = new SqlParameter[0];
                //parameters[0] = new SqlParameter("@site_id", siteId);
                int o = (int)dataAccessHandler.executeScalar(getYearQuery, parameters, sqlTransaction);

                for (int i = -Math.Max(o, 0); i < 3; i++)
                {
                    year.Add(DateTime.Now.Year + i);
                }
                log.LogMethodExit(year);
                return year;
            }
            catch(Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
    }
}
