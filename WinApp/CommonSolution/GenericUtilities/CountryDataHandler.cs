/********************************************************************************************
 * Project Name - ApplicationContent Data Handler
 * Description  - Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By       Remarks          
 *********************************************************************************************
 *2.60         28-Mar-2019   Mushahid Faizan     Renamed Guid_id to Guid in InsertCountryDTO().
 *                                               Added SqlTransaction in Constructor
 *2.70.2        25-Jul-2019      Dakshakh Raj      Modified : added GetSQLParameters(), 
 *                                                          SQL injection Issue Fix and
 *                                                          Added IsActive to insert/update method
 *2.70.2        06-Dec-2019    Jinto Thomas        Removed siteid from update query                                                           
 *2.140.0       02-Nov-2021    Deeksha             Modified to add country code                                                       
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    public class CountryDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM Country as ct ";

        /// <summary>
        /// Dictionary for searching Parameters for the ApplicationContent object.
        /// </summary>
        private static readonly Dictionary<CountryDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CountryDTO.SearchByParameters, string>
            {
                {CountryDTO.SearchByParameters.COUNTRY_ID, "ct.CountryId"},
                {CountryDTO.SearchByParameters.COUNTRY_NAME, "ct.CountryName"},
                {CountryDTO.SearchByParameters.SITE_ID, "ct.site_id"},
                {CountryDTO.SearchByParameters.MASTER_ENTITY_ID, "ct.MasterEntityId"} 
             };
        private SqlTransaction sqlTransaction;

        /// <summary>
        /// Default constructor of CountryDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CountryDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit ();
        }

        /// <summary>
        ///Builds the SQL Parameter list used for inserting and updating countryDTO Reecord.
        /// </summary>
        /// <param name="countryDTO">countryDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns> Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(CountryDTO countryDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(countryDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CountryId", countryDTO.CountryId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@countryName", countryDTO.CountryName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", countryDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", countryDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the Country record to the database
        /// </summary>
        /// <param name="CountryDTO">CountryDTO type object</param>
        /// <param name="loginId"></param>
        /// <param name="siteId">data updated site id</param>
        /// <returns>Returns inserted record id</returns>
        public CountryDTO InsertCountryDTO(CountryDTO countryDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(countryDTO, loginId, siteId);
            string insertCountryDTOQuery = @"insert into Country
                                                            ( 
                                                            CountryName,
                                                            site_id,
                                                            Guid,
                                                            MasterEntityId,
                                                            CreatedBy,
                                                            CreationDate,
                                                            LastUpdatedBy,
                                                            LastUpdateDate,
                                                            IsActive
                                                         )
                                                       values
                                                         ( 
                                                            @countryName,
                                                            @siteId,
                                                            NewId(), 
                                                            @masterEntityId,
                                                            @createdBy,
                                                            GETDATE(),
                                                            @lastUpdatedBy,
                                                            GETDATE(),
                                                            @isActive
                                                           )SELECT * FROM Country WHERE CountryId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertCountryDTOQuery, GetSQLParameters(countryDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCountryDTO(countryDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting CountryDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(countryDTO);
            return countryDTO;
        }


        /// <summary>
        /// Updates the Country record to the database
        /// </summary>
        /// <param name="CountryDTO">CountryDTO type object</param>
        /// <param name="loginId">updated user id number</param>
        /// <param name="siteId">data updated site id</param>
        /// <returns>Returns # of rows updated</returns>
        public CountryDTO UpdateCountryDTO(CountryDTO countryDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(countryDTO, log, siteId);
            string updateCountryDTOQuery = @"update Country
                                                         set
                                                            CountryName = @countryName,
                                                            --site_id = @siteId, 
                                                            MasterEntityId = @masterEntityId, 
                                                            LastUpdatedBy = @lastUpdatedBy,
                                                            LastUpdateDate = GETDATE(),
                                                            IsActive= @isActive
                                                        where 
                                                            CountryId = @CountryId
                                                        SELECT* FROM Country WHERE  CountryId = @CountryId ";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateCountryDTOQuery, GetSQLParameters(countryDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCountryDTO(countryDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating CountryDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(countryDTO);
            return countryDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="applicationContentDTO">applicationContentDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshCountryDTO(CountryDTO countryDTO, DataTable dt)
        {
            log.LogMethodEntry(countryDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                countryDTO.CountryId = Convert.ToInt32(dt.Rows[0]["CountryId"]);
                countryDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                countryDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                countryDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                countryDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                countryDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                countryDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                countryDTO.IsActive  = dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Converts the Data row object to CountryDTO class type
        /// </summary>
        /// <param name="CountryDataRow">CountryDataRow</param>
        /// <returns>Returns ApplicationContentDTO</returns>
        private CountryDTO GetCountryDTO(DataRow CountryDataRow)
        {
            log.LogMethodEntry(CountryDataRow);
            CountryDTO countryDTO = new CountryDTO(
                                 CountryDataRow["CountryId"] == DBNull.Value ? -1 : Convert.ToInt32(CountryDataRow["CountryId"]),
                                 CountryDataRow["CountryName"] == DBNull.Value ? string.Empty : CountryDataRow["CountryName"].ToString(),
                                 CountryDataRow["Guid"].ToString(),
                                 CountryDataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(CountryDataRow["Site_id"]),
                                 CountryDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(CountryDataRow["SynchStatus"]),
                                 CountryDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(CountryDataRow["MasterEntityId"]),
                                 CountryDataRow["CreatedBy"] == DBNull.Value ? string.Empty : CountryDataRow["CreatedBy"].ToString(),
                                 CountryDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(CountryDataRow["CreationDate"]),
                                 CountryDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : CountryDataRow["LastUpdatedBy"].ToString(),
                                 CountryDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(CountryDataRow["LastUpdateDate"]),
                                 CountryDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(CountryDataRow["IsActive"]),
                                 CountryDataRow["countryCode"] == DBNull.Value ? string.Empty : CountryDataRow["countryCode"].ToString()
                                 );
            log.LogMethodExit(countryDTO);
            return countryDTO;
        }


        /// <summary>
        /// Gets the CountryDTO data of passed countryId
        /// </summary>
        /// <param name="countryId">integer type parameter</param>
        /// <returns>Returns CountryDTO</returns>
        public CountryDTO GetCountryDTO(int countryId)
        {
            log.LogMethodEntry(countryId);

            string selectCountryDTOQuery = SELECT_QUERY + @" WHERE ct.CountryId = @countryId";
            SqlParameter[] selectCountryDTOParameters = new SqlParameter[1];
            selectCountryDTOParameters[0] = new SqlParameter("@countryId", countryId);
            DataTable selectedCountryDTO = dataAccessHandler.executeSelectQuery(selectCountryDTOQuery, selectCountryDTOParameters, sqlTransaction);
            CountryDTO CountryDTO = new CountryDTO(); ;
            if (selectedCountryDTO.Rows.Count > 0)
            {
                DataRow countryRow = selectedCountryDTO.Rows[0];
                CountryDTO = GetCountryDTO(countryRow);
            }
            log.LogMethodExit(CountryDTO);
            return CountryDTO;
        }

        /// <summary>
        /// Delete the record from the Country database based on countryId
        /// </summary>
        /// <returns>return the int </returns>
        public int Delete(int countryId)
        {
            log.LogMethodEntry(countryId);
            try
            {
                string deleteCountryQuery = @"delete  
                                                 from Country
                                                where CountryId = @countryId";

                SqlParameter[] deleteCountryDTOParameters = new SqlParameter[1];
                deleteCountryDTOParameters[0] = new SqlParameter("@countryId", countryId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteCountryQuery, deleteCountryDTOParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception expn)
            {
                throw new System.Exception(expn.Message.ToString());
            }
        }

        /// <summary>
        /// Gets the CountryDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CountryDTO matching the search criteria</returns>
        public List<CountryDTO> GetCountryDTOList(List<KeyValuePair<CountryDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectCountryDTOQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<CountryDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == CountryDTO.SearchByParameters.COUNTRY_ID
                            || searchParameter.Key == CountryDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CountryDTO.SearchByParameters.COUNTRY_NAME)
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == CountryDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }

                if (searchParameters.Count > 0)
                    selectCountryDTOQuery = selectCountryDTOQuery + query;
                selectCountryDTOQuery = selectCountryDTOQuery + " Order by countryId";
            }

            DataTable CountryDTOsData = dataAccessHandler.executeSelectQuery(selectCountryDTOQuery, parameters.ToArray(), sqlTransaction);
            List<CountryDTO> CountryDTOsList = new List<CountryDTO>();
            if (CountryDTOsData.Rows.Count > 0)
            {
                foreach (DataRow dataRow in CountryDTOsData.Rows)
                {
                    CountryDTO CountryDTOObject = GetCountryDTO(dataRow);
                    CountryDTOsList.Add(CountryDTOObject);
                }
                log.LogMethodExit( CountryDTOsList);

            }
            return CountryDTOsList;
        }

        /// <summary>
        ///  GetCountryDTOByLookup(string lookupName,int siteId)  method
        /// </summary>
        ///<param name="lookupName">string lookupName</param>
        ///<param name="siteId">int siteId</param>
        /// <returns>returns CountryDTO object</returns>
        public CountryDTO GetCountryDTOByLookup(string lookupName, int siteId)
        {
            log.LogMethodEntry(lookupName, siteId);
            try
            {
                string countryDTOQuery = @"select * from Country where CountryId in
                                            (select  default_value from parafait_defaults 
                                            where site_id=@siteId 
                                            and default_value_name=@lookupName)";


                SqlParameter[] countryDTOParameters = new SqlParameter[2];
                countryDTOParameters[0] = new SqlParameter("@lookupName", lookupName);
                countryDTOParameters[1] = new SqlParameter("@siteId", siteId);

                CountryDTO countryDTO = new CountryDTO();
                DataTable dtCountryDTO = dataAccessHandler.executeSelectQuery(countryDTOQuery, countryDTOParameters, sqlTransaction);
                if (dtCountryDTO.Rows.Count > 0)
                {
                    DataRow countryDTORow = dtCountryDTO.Rows[0];
                    log.Debug("Ends-GetCountryDTOByLookup(string lookupName,int siteId) Method.");
                    countryDTO = GetCountryDTO(countryDTORow);
                }
                log.LogMethodExit(countryDTO);
                return countryDTO;
            }
            catch
            {
                throw;
            }
        }

        internal DateTime? GetCountryLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdateDate) LastUpdateDate 
                            FROM (
                            select max(LastUpdateDate) LastUpdateDate from Country WHERE (site_id = @siteId or @siteId = -1)
                            )a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdateDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdateDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
