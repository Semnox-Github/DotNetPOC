/********************************************************************************************
* Project Name - CCStatusPGW Data Handler
* Description  - Data handler of the CCStatusPGW class
* 
**************
**Version Log
**************
*Version     Date          Modified By         Remarks          
*********************************************************************************************
*1.00        20-Jun-2017   Lakshminarayana     Created 
*2.00        03-May-2019   Divya               SQL Injection
*2.70.2        01-Jul-2019   Girish Kundar       Modified : Added missed Columns to Insert/Update
*2.70.2       06-Dec-2019   Jinto Thomas         Removed siteid from update query 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    ///  CCStatusPGW Data Handler - Handles insert, update and select of  CCStatusPGW objects
    /// </summary>
    public class CCStatusPGWDataHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<CCStatusPGWDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CCStatusPGWDTO.SearchByParameters, string>
            {
                {CCStatusPGWDTO.SearchByParameters.STATUS_ID, "ccs.StatusID"},
                {CCStatusPGWDTO.SearchByParameters.STATUS_MESSAGE, "ccs.StatusMessage"},
                {CCStatusPGWDTO.SearchByParameters.MASTER_ENTITY_ID, "ccs.MasterEntityId"},
                {CCStatusPGWDTO.SearchByParameters.SITE_ID, "ccs.site_id"}
            };
        private SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = @"SELECT * FROM CCStatusPGW AS ccs ";
        private DataAccessHandler dataAccessHandler;

        /// <summary>
        ///Default constructor of CCStatusPGWDataHandler class 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public CCStatusPGWDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new  DataAccessHandler ();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// ParameterHelper method for building SQL parameters for query
        /// </summary>
        /// <param name="parameters">parameters</param>
        /// <param name="parameterName">parameterName</param>
        /// <param name="value">value</param>
        /// <param name="negetiveValueNull">negetiveValueNull</param>
        private void ParameterHelper(List<SqlParameter> parameters, string parameterName, object value, bool negetiveValueNull = false)
        {
            log.LogMethodEntry(parameters, parameterName, value, negetiveValueNull);

            if(parameters != null && !string.IsNullOrEmpty(parameterName))
            {
                if(value is int)
                {
                    if(negetiveValueNull && ((int)value) < 0)
                    {
                        parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(parameterName, value));
                    }
                }
                else if(value is string)
                {
                    if(string.IsNullOrEmpty(value as string))
                    {
                        parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(parameterName, value));
                    }
                }
                else
                {
                    if(value == null)
                    {
                        parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(parameterName, value));
                    }
                }
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// GetSQLParameters method to build SQL parameters for Insert/ Update
        /// </summary>
        /// <param name="parameters">parameters</param>
        /// <param name="CCStatusPGWDTO">CCStatusPGWDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        ///  <returns> List of SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters( CCStatusPGWDTO cCStatusPGWDTO, string loginId, int siteId)
        {
            log.LogMethodEntry( cCStatusPGWDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            ParameterHelper(parameters, "@StatusID", cCStatusPGWDTO.StatusId);
            ParameterHelper(parameters, "@StatusMessage", cCStatusPGWDTO.StatusMessage);
            ParameterHelper(parameters, "@MasterEntityId", cCStatusPGWDTO.MasterEntityId, true);
            ParameterHelper(parameters, "@CreatedBy", loginId);
            ParameterHelper(parameters, "@LastUpdatedBy", loginId);
            ParameterHelper(parameters, "@site_id", siteId,true);
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the CCStatusPGW record to the database
        /// </summary>
        /// <param name="cCStatusPGWDTO">CCStatusPGWDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CCStatusPGWDTO/returns>
        public CCStatusPGWDTO InsertCCStatusPGW(CCStatusPGWDTO cCStatusPGWDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cCStatusPGWDTO, loginId, siteId);
            string query = @"INSERT INTO CCStatusPGW 
                                        ( 
                                            StatusMessage,
                                            site_id,
                                            MasterEntityId,
                                            CreatedBy,
                                            CreationDate, 
                                            LastUpdatedBy,
                                            LastUpdateDate
                                        ) 
                                VALUES 
                                        (
                                            @StatusMessage,
                                            @site_id,
                                            @MasterEntityId,
                                            @CreatedBy,
                                            GETDATE(), 
                                            @LastUpdatedBy,
                                            GETDATE()
                                        ) SELECT * FROM CCStatusPGW WHERE StatusID = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(cCStatusPGWDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCCStatusPGWDTO(cCStatusPGWDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting cCStatusPGWDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cCStatusPGWDTO);
            return cCStatusPGWDTO;
        }

        /// <summary>
        /// Updates the CCStatusPGW record
        /// </summary>
        /// <param name="cCStatusPGWDTO">CCStatusPGWDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public CCStatusPGWDTO UpdateCCStatusPGW(CCStatusPGWDTO cCStatusPGWDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cCStatusPGWDTO, loginId, siteId);
            string query = @"UPDATE CCStatusPGW 
                             SET StatusMessage=@StatusMessage,
                                 --site_id=@site_id,
                                 LastUpdatedBy = @LastUpdatedBy,
                                 LastUpdateDate  = GetDate()
                             WHERE StatusID = @StatusID
                             SELECT * FROM CCStatusPGW WHERE StatusID = @StatusID ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(cCStatusPGWDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCCStatusPGWDTO(cCStatusPGWDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating cCStatusPGWDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cCStatusPGWDTO);
            return cCStatusPGWDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="CCStatusPGWDTO">CCStatusPGWDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCCStatusPGWDTO(CCStatusPGWDTO ccStatusPGWDTO, DataTable dt)
        {
            log.LogMethodEntry(ccStatusPGWDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                ccStatusPGWDTO.StatusId = Convert.ToInt32(dt.Rows[0]["StatusID"]);
                ccStatusPGWDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                ccStatusPGWDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                ccStatusPGWDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                ccStatusPGWDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                ccStatusPGWDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                ccStatusPGWDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to CCStatusPGWDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns CCStatusPGWDTO</returns>
        private CCStatusPGWDTO GetCCStatusPGWDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CCStatusPGWDTO cCStatusPGWDTO = new CCStatusPGWDTO(Convert.ToInt32(dataRow["StatusID"]),
                                            dataRow["StatusMessage"] == DBNull.Value ? string.Empty : dataRow["StatusMessage"].ToString(),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                            );

            log.LogMethodExit(cCStatusPGWDTO);
            return cCStatusPGWDTO;
        }

        /// <summary>
        /// Gets the CCStatusPGW data of passed statusId
        /// </summary>
        /// <param name="statusId">integer type parameter</param>
        /// <returns>Returns CCStatusPGWDTO</returns>
        public CCStatusPGWDTO GetCCStatusPGWDTO(int statusId)
        {
            log.LogMethodEntry(statusId);
            CCStatusPGWDTO returnValue = null;
            string query = SELECT_QUERY +"  WHERE ccs.StatusID = @StatusID";
            SqlParameter parameter = new SqlParameter("@StatusID", statusId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if(dataTable.Rows.Count > 0)
            {
                returnValue = GetCCStatusPGWDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the MediaDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CCStatusPGWDTO matching the search criteria</returns>
        public List<CCStatusPGWDTO> GetCCStatusPGWDTOList(List<KeyValuePair<CCStatusPGWDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CCStatusPGWDTO> list = null;
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach(KeyValuePair<CCStatusPGWDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if(DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if(searchParameter.Key == CCStatusPGWDTO.SearchByParameters.STATUS_ID
                            || searchParameter.Key == CCStatusPGWDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if(searchParameter.Key == CCStatusPGWDTO.SearchByParameters.STATUS_MESSAGE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if(searchParameter.Key == CCStatusPGWDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + " =-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
            log.Debug("Search query: " + selectQuery);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<CCStatusPGWDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CCStatusPGWDTO cCStatusPGWDTO = GetCCStatusPGWDTO(dataRow);
                    list.Add(cCStatusPGWDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
