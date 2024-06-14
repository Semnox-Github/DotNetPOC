/********************************************************************************************
 * Project Name - ParafaitOptionValuesDataHandler                                                                      
 * Description  - DataHandler for the ParafaitOptionValues tables.
 *
 **************
 **Version Log
  *Version     Date             Modified  By                  Remarks          
 *********************************************************************************************
 *2.40.1       25-Jan-2019    Flavia Jyothi Dsouza         Created new DataHandler class 
 *2.70         04-Jul-2019    Girish Kundar                Modified : Changed the structure of Insert /Update methods.
 *                                                         SQL injection Issue Fix. 
 *2.70         08-Feb-2019    Nagesh Badiger               Modified IsActive Parameter to after changing isActive DataType in DTO
 *2.70         03-Apr-2019    Akshay Gulaganji             Added POSMachineId searchParameter in GetParafaitOptionValuesDTOList() method and modified GetSQLParameters(), InsertUtility() and UpdateUtility()
 *2.70         29-Jun-2019    Akshay Gulaganji             Added DeleteParafaitOptionValues() method
 *2.70.2         11-Dec-2019    Jinto Thomas                 Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Semnox.Core.Utilities
{
    public class ParafaitOptionValuesDataHandler
    {
        /// <summary>
        ///  Data Handler - Selection  of ParafaitOptionValues objects
        /// </summary>
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ParafaitOptionValues AS pov ";

        private static readonly Dictionary<ParafaitOptionValuesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ParafaitOptionValuesDTO.SearchByParameters, string>
            {
                {ParafaitOptionValuesDTO.SearchByParameters. OPTION_VALUE_ID,"pov.OptionValueId"},
                {ParafaitOptionValuesDTO.SearchByParameters. OPTION_ID, "pov.OptionId"},
                {ParafaitOptionValuesDTO.SearchByParameters. OPTION_VALUE, "pov.OptionValue"},
                {ParafaitOptionValuesDTO.SearchByParameters.IS_ACTIVE, "pov.ActiveFlag"},
                {ParafaitOptionValuesDTO.SearchByParameters.SITE_ID, "pov.site_id"},
                {ParafaitOptionValuesDTO.SearchByParameters. POSMACHINEID , "pov.POSMachineId"},
                {ParafaitOptionValuesDTO.SearchByParameters. USER_ID , "pov.UserId"},
                {ParafaitOptionValuesDTO.SearchByParameters.MASTER_ENTITY_ID,"pov.MasterEntityId"},

             };

        /// <summary>
        /// Default constructor of ParafaitOptionsValuesDataHandler class
        /// </summary>
        public ParafaitOptionValuesDataHandler()
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parametrized constructor for ParafaitOptionValuesDataHandler
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ParafaitOptionValuesDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();

        }

        /// <summary>
        /// Builds the SqlParameter for ParafaitOptionValuesDTO
        /// </summary>
        /// <param name="parafaitOptionValuesDTO">ParafaitOptionValuesDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>SqlParameter</returns>
        private List<SqlParameter> GetSQLParameters(ParafaitOptionValuesDTO parafaitOptionValuesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(parafaitOptionValuesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@optionValueId", parafaitOptionValuesDTO.OptionValueId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@optionId ", parafaitOptionValuesDTO.OptionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@optionValue", parafaitOptionValuesDTO.OptionValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", (parafaitOptionValuesDTO.IsActive ? 'Y' : 'N')));
            parameters.Add(dataAccessHandler.GetSQLParameter("@posMachineId", parafaitOptionValuesDTO.PosMachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@loginId ", parafaitOptionValuesDTO.UserId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", parafaitOptionValuesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;

        }

        /// <summary>
        /// Inserts the  record to the database
        /// </summary>
        /// <param name="parafaitOptionValuesDTO">parafaitOptionValuesDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>ParafaitOptionValuesDTO</returns>
        public ParafaitOptionValuesDTO InsertUtility(ParafaitOptionValuesDTO parafaitOptionValuesDTO, String loginId, int siteId)
        {
            log.LogMethodEntry(parafaitOptionValuesDTO, loginId, siteId);
            string query = @"INSERT  INTO  ParafaitOptionValues
                                                            (
                                                             OptionId,
                                                             OptionValue,
                                                             ActiveFlag,
                                                             POSMachineId,
                                                             UserId,
                                                             site_id,
                                                             LastUpdatedBy,
                                                             LastUpdatedDate,
                                                             MasterEntityId,
                                                             CreatedBy,
                                                             CreationDate)
                                                               
                                                     VALUES
                                                         ( 
                                                           @optionId,
                                                           @optionValue,
                                                           @isActive,
                                                           @posMachineId,
                                                           @loginId ,                                                          
                                                           @siteId,
                                                           @lastUpdatedBy,
                                                           GETDATE(),
                                                           @masterEntityId, 
                                                           @createdBy,
                                                           GETDATE()
                                                          )
                                           SELECT * FROM ParafaitOptionValues WHERE OptionValueId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(parafaitOptionValuesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshParafaitOptionValuesDTO(parafaitOptionValuesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting parafaitOptionValuesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(parafaitOptionValuesDTO);
            return parafaitOptionValuesDTO;

        }

        /// <summary>
        /// updates the ParafaitOptionValuesDTO with Id ,who columns values for further process.
        /// </summary>
        /// <param name="dailyCardBalanceDTO">ParafaitOptionValuesDTO</param>
        /// <param name="dt">dt object of DataTable</param>
        private void RefreshParafaitOptionValuesDTO(ParafaitOptionValuesDTO parafaitOptionValuesDTO, DataTable dt)
        {
            log.LogMethodEntry(parafaitOptionValuesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                parafaitOptionValuesDTO.OptionValueId = Convert.ToInt32(dt.Rows[0]["OptionValueId"]);
                parafaitOptionValuesDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                parafaitOptionValuesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                parafaitOptionValuesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                parafaitOptionValuesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
                parafaitOptionValuesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                parafaitOptionValuesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Updates the record in database 
        /// </summary>
        /// <param name="parafaitOptionValuesDTO">ParafaitOptionValuesDTO</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>ParafaitOptionValuesDTO</returns>
        public ParafaitOptionValuesDTO UpdateUtility(ParafaitOptionValuesDTO parafaitOptionValuesDTO, String loginId, int siteId)
        {

            log.LogMethodEntry(parafaitOptionValuesDTO, loginId, siteId);
            string query = @"UPDATE ParafaitOptionValues
                             SET 
                                OptionId=@optionId,
                                OptionValue=@optionValue,
                                ActiveFlag= @isActive,
                                POSMachineId= @posMachineId,
                                UserId=@loginId, 
                                -- site_id=@siteId,
                                LastUpdatedBy= @lastUpdatedBy,
                                LastUpdatedDate= GETDATE(),
                                MasterEntityId=@masterEntityId 
                             WHERE OptionValueId= @optionValueId
                           SELECT * FROM ParafaitOptionValues WHERE OptionValueId = optionValueId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(parafaitOptionValuesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshParafaitOptionValuesDTO(parafaitOptionValuesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting parafaitOptionValuesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(parafaitOptionValuesDTO);
            return parafaitOptionValuesDTO;

        }

        /// <summary>
        /// Converts the Data row object to ParafaitOptionValuseDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow</param>
        /// <returns>ParafaitOptionValuesDTO</returns>
        private ParafaitOptionValuesDTO GetParafaitOptionValuesDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ParafaitOptionValuesDTO parafaitOptionValuesDTO = new ParafaitOptionValuesDTO(
                                                              Convert.ToInt32(dataRow["OptionValueId"]),
                                                              dataRow["OptionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OptionId"]),
                                                              dataRow["OptionValue"] == DBNull.Value ? string.Empty : dataRow["OptionValue"].ToString(),
                                                              dataRow["ActiveFlag"] == DBNull.Value ? true : dataRow["ActiveFlag"].ToString() == "Y",
                                                              dataRow["POSMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSMachineId"]),
                                                              dataRow["UserId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UserId"]),
                                                              dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                              dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                                              dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                              dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                              dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                                              dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                              dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                                              dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                                                                             );
            log.LogMethodExit(parafaitOptionValuesDTO);
            return parafaitOptionValuesDTO;
        }

        /// <summary>
        /// Returns the ParafaitOptionValuesDTO based on the OptionValueId
        /// </summary>
        /// <param name="OptionValueId">OptionValueId is passed as parameter</param>
        /// <returns>ParafaitOptionValuesDTO</returns>
        public ParafaitOptionValuesDTO GetParafaitOptionValuesDTO(int OptionValueId)
        {
            log.LogMethodEntry(OptionValueId);
            ParafaitOptionValuesDTO returnValue = null;
            string query1 = SELECT_QUERY + " WHERE pov.OptionValueId= @optionValueId ";
            SqlParameter parameter = new SqlParameter("@optionValueId", OptionValueId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query1, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetParafaitOptionValuesDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        /// <summary>
        /// Starts-GetParafaitOptionValuesDTOList(searchParameters) Method
        /// </summary>
        /// <param name="searchParameters">searchParameters </param>
        /// <returns>List of ParafaitOptionValuesDTO</returns>
        public List<ParafaitOptionValuesDTO> GetParafaitOptionValuesDTOList(List<KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ParafaitOptionValuesDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == ParafaitOptionValuesDTO.SearchByParameters.OPTION_VALUE_ID ||
                            searchParameter.Key == ParafaitOptionValuesDTO.SearchByParameters.OPTION_ID ||
                            searchParameter.Key == ParafaitOptionValuesDTO.SearchByParameters.POSMACHINEID ||
                             searchParameter.Key == ParafaitOptionValuesDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ParafaitOptionValuesDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + " =-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ParafaitOptionValuesDTO.SearchByParameters.OPTION_VALUE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ParafaitOptionValuesDTO.SearchByParameters.IS_ACTIVE) // char
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value == "Y") ? 'Y' : 'N'));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<ParafaitOptionValuesDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ParafaitOptionValuesDTO parafaitOptionValuesDTO = GetParafaitOptionValuesDTO(dataRow);
                    list.Add(parafaitOptionValuesDTO);
                }
            }
            log.LogMethodEntry(list);
            return list;
        }
        /// <summary>
        /// Deletes the ParafaitOptionValues based on the optionValueId
        /// </summary>
        /// <param name="optionValueId">optionValueId</param>
        /// <returns>return the int</returns>
        public int DeleteParafaitOptionValues(int optionValueId)
        {
            log.LogMethodEntry(optionValueId);
            try
            {
                string deleteQuery = @"delete from ParafaitOptionValues where OptionValueId = @optionValueId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@optionValueId", optionValueId);

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

        public List<ParafaitOptionValuesDTO> GetParafaitOptionValuesDTOList(List<int> idList, bool activeRecords)
        {
            log.LogMethodEntry(idList);
            List<ParafaitOptionValuesDTO> list = new List<ParafaitOptionValuesDTO>();
            string query = @"SELECT ParafaitOptionValues.*
                            FROM ParafaitOptionValues, @defaultIdList List
                            WHERE OptionId = List.id ";
            if (activeRecords)
            {
                query += " AND ActiveFlag = 'Y' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@defaultIdList", idList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetParafaitOptionValuesDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

    }
}
