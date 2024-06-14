/********************************************************************************************
 * Project Name - ParafaitFunctionsDataHandler
 * Description  - Data handler of the ParafaitFunctions 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     09-Dec-2020    Fiona            Created for Subscription changes                                                                               
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data;

namespace Semnox.Parafait.Communication
{
    public class ParafaitFunctionsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ParafaitFunctions AS pf ";
        private static readonly Dictionary<ParafaitFunctionsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ParafaitFunctionsDTO.SearchByParameters, string>
            {
                {ParafaitFunctionsDTO.SearchByParameters.PARAFAIT_FUNCTION_ID, "pf.ParafaitFunctionId"},
                { ParafaitFunctionsDTO.SearchByParameters.PARAFAIT_FUNCTION_NAME, "pf.ParafaitFunctionName"},
                { ParafaitFunctionsDTO.SearchByParameters.IS_ACTIVE, "pf.IsActive"},
                {ParafaitFunctionsDTO.SearchByParameters.MASTER_ENTITY_ID, "pf.MasterEntityId"},
                {ParafaitFunctionsDTO.SearchByParameters.SITE_ID, "pf.site_id"}

            };
        /// <summary>
        /// Parameterized Constructor for ParafaitFunctionsDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public ParafaitFunctionsDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(ParafaitFunctionsDTO parafaitFunctionsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(parafaitFunctionsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParafaitFunctionId", parafaitFunctionsDTO.ParafaitFunctionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParafaitFunctionName", parafaitFunctionsDTO.ParafaitFunctionName.ToString()));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParafaitFunctionDescription", parafaitFunctionsDTO.ParafaitFunctionDescription));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", parafaitFunctionsDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", parafaitFunctionsDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", parafaitFunctionsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }
        //internal ParafaitFunctionsDTO Insert(ParafaitFunctionsDTO parafaitFunctionsDTO, string loginId, int siteId)
        //{
        //    log.LogMethodEntry(parafaitFunctionsDTO, loginId, siteId);
        //    string query = @"INSERT INTO[dbo].ParafaitFunctions
        //                       (  
        //                                ParafaitFunctionName,
        //                                ParafaitFunctionDescription,
        //                                IsActive,
        //                                SynchStatus,
        //                                site_id,
        //                                MasterEntityId,
        //                                CreatedBy,
        //                                CreationDate,
        //                                LastUpdatedBy,
        //                                LastUpdatedDate,
        //                                Guid 
        //                        )
        //                 VALUES
        //                       (    @ParafaitFunctionName,
        //                            @ParafaitFunctionDescription,
        //                            @IsActive,
        //                            @SynchStatus,
        //                            @SiteId,
        //                            @MasterEntityId
        //                            @CreatedBy,
        //                            GETDATE(),
        //                            @LastUpdatedBy,
        //                            GETDATE()
        //                            NEWID(), 
        //                        )
        //                        SELECT * FROM ParafaitFunctions WHERE ParafaitFunctionId = scope_identity()";
        //    try
        //    {
        //        DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(parafaitFunctionsDTO, loginId, siteId).ToArray(), sqlTransaction);
        //        RefreshParafaitFunctionsDTO(parafaitFunctionsDTO, dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        log.LogMethodExit(null, "Throwing exception - " + ex.Message);
        //        throw;
        //    }
        //    log.LogMethodExit(parafaitFunctionsDTO);
        //    return parafaitFunctionsDTO;
        //}
        //internal ParafaitFunctionsDTO Update(ParafaitFunctionsDTO parafaitFunctionsDTO, string loginId, int siteId)
        //{
        //    log.LogMethodEntry(parafaitFunctionsDTO, loginId, siteId);
        //    string query = @"UPDATE [dbo].ParafaitFunctions set
        //                       ParafaitFunctionName = @ParafaitFunctionName,
        //                       ParafaitFunctionDescription= @ParafaitFunctionDescription,
        //                       MasterEntityId = @MasterEntityId,
        //                       IsActive = @IsActive, 
        //                       site_id  = @SiteId,
        //                       LastUpdatedBy = @LastUpdatedBy,
        //                       LastUpdatedDate = GETDATE()
        //                       where ParafaitFunctionId = @ParafaitFunctionId
        //                     SELECT * FROM ParafaitFunctions WHERE ParafaitFunctionId = @ParafaitFunctionId";
        //    try
        //    {
        //        DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(parafaitFunctionsDTO, loginId, siteId).ToArray(), sqlTransaction);
        //        RefreshParafaitFunctionsDTO(parafaitFunctionsDTO, dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        log.LogMethodExit(null, "Throwing exception - " + ex.Message);
        //        throw;
        //    }
        //    log.LogMethodExit(parafaitFunctionsDTO);
        //    return parafaitFunctionsDTO;
        //}
        //private void RefreshParafaitFunctionsDTO(ParafaitFunctionsDTO parafaitFunctionsDTO, DataTable dt)
        //{
        //    log.LogMethodEntry(parafaitFunctionsDTO, dt);
        //    if (dt.Rows.Count > 0)
        //    {
        //        DataRow dataRow = dt.Rows[0];
        //        parafaitFunctionsDTO.ParafaitFunctionId = Convert.ToInt32(dt.Rows[0]["ParafaitFunctionId"]);
        //        parafaitFunctionsDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
        //        parafaitFunctionsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
        //        parafaitFunctionsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
        //        parafaitFunctionsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
        //        parafaitFunctionsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
        //        parafaitFunctionsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
        //    }
        //    log.LogMethodExit();
        //}
        internal ParafaitFunctionsDTO GetParafaitFunctionsDTO(int parafaitFunctionId)
        {
            log.LogMethodEntry(parafaitFunctionId);
            ParafaitFunctionsDTO result = null;
            string query = SELECT_QUERY + @" WHERE pf.ParafaitFunctionId = @ParafaitFunctionId";
            SqlParameter parameter = new SqlParameter("@ParafaitFunctionId", parafaitFunctionId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetParafaitFunctionsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        internal ParafaitFunctionsDTO GetParafaitFunctionsDTO(ParafaitFunctions parafaitFunctionName)
        {
            log.LogMethodEntry(parafaitFunctionName);
            ParafaitFunctionsDTO result = null;
            string query = SELECT_QUERY + @" WHERE pf.parafaitFunctionName = @parafaitFunctionName";
            SqlParameter parameter = new SqlParameter("@parafaitFunctionName", parafaitFunctionName.ToString());
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetParafaitFunctionsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        private ParafaitFunctionsDTO GetParafaitFunctionsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ParafaitFunctionsDTO parafaitFunctionsDTO = new ParafaitFunctionsDTO(
                dataRow["ParafaitFunctionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParafaitFunctionId"]),
                dataRow["ParafaitFunctionName"] == DBNull.Value ? ParafaitFunctions.NONE : GetParafaitFunctions(Convert.ToString(dataRow["ParafaitFunctionName"])),
                dataRow["ParafaitFunctionDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ParafaitFunctionDescription"]),
                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"])
                );
            log.LogMethodExit(parafaitFunctionsDTO);
            return parafaitFunctionsDTO;
        }
        internal List<ParafaitFunctionsDTO> GetParafaitFunctionsDTOList(List<KeyValuePair<ParafaitFunctionsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ParafaitFunctionsDTO> parafaitFunctionsDTOList = new List<ParafaitFunctionsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Any()))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ParafaitFunctionsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ParafaitFunctionsDTO.SearchByParameters.PARAFAIT_FUNCTION_ID ||
                            searchParameter.Key == ParafaitFunctionsDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ParafaitFunctionsDTO.SearchByParameters.PARAFAIT_FUNCTION_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ParafaitFunctionsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ParafaitFunctionsDTO.SearchByParameters.IS_ACTIVE)
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
            if (dataTable != null && dataTable.Rows.Cast<DataRow>().Any())
            {
                parafaitFunctionsDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetParafaitFunctionsDTO(x)).ToList();
            }
            log.LogMethodExit(parafaitFunctionsDTOList);
            return parafaitFunctionsDTOList;
        }

        /// <summary>
        ///  Gets the ParafaitFunctions by passing parafaitFunction string
        /// </summary>
        /// <param name="parafaitFunctionString">parafaitFunctionString</param>
        /// <returns>ParafaitFunctionEvents</returns>
        private ParafaitFunctions GetParafaitFunctions(string parafaitFunctionString)
        {
            log.LogMethodEntry(parafaitFunctionString);
            ParafaitFunctions parafaitFunctions = ParafaitFunctions.NONE;
            try
            {
                parafaitFunctions = (ParafaitFunctions)Enum.Parse(typeof(ParafaitFunctions), parafaitFunctionString, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while parsing the parafaitFunctions", ex);
                throw ex;
            }
            log.LogMethodExit(parafaitFunctions);
            return parafaitFunctions;
        }
    }

}
