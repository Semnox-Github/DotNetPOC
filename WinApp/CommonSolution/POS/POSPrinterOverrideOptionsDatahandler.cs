/********************************************************************************************
 * Project Name - POSPrinterOverrideOptions Datahandler
 * Description  - Datahandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *1.00        09-Dec-2020      Dakshakh Raj     Created for Peru Invoice Enhancement
 ********************************************************************************************/
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;

namespace Semnox.Parafait.POS
{
    /// <summary>
    /// POS Printer Override Options DataHandler
    /// </summary>
    public class POSPrinterOverrideOptionsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM POSPrinterOverrideOptions AS pp ";

        private static readonly Dictionary<POSPrinterOverrideOptionsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<POSPrinterOverrideOptionsDTO.SearchByParameters, string>
            {
                {POSPrinterOverrideOptionsDTO.SearchByParameters.POS_PRINTER_OVERRIDE_OPTION_ID, "pp.POSPrinterOverrideOptionId"},
                {POSPrinterOverrideOptionsDTO.SearchByParameters.OPTION_NAME, "pp.OptionName"},
                {POSPrinterOverrideOptionsDTO.SearchByParameters.OPTION_DESCRIPTION, "pp.OptionDescription"},
                {POSPrinterOverrideOptionsDTO.SearchByParameters.IS_ACTIVE, "pp.IsActive"},
                {POSPrinterOverrideOptionsDTO.SearchByParameters.MASTER_ENTITY_ID, "pp.MasterEntityId"},
                {POSPrinterOverrideOptionsDTO.SearchByParameters.SITE_ID, "pp.site_id"},
                {POSPrinterOverrideOptionsDTO.SearchByParameters.POS_PRINTER_OVERRIDE_OPTION_ID_LIST, "pp.POSPrinterOverrideOptionId"},
            };

        /// <summary>
        /// Parameterized Constructor for POS Printer Override Options Datahandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public POSPrinterOverrideOptionsDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Get SQL Parameters
        /// </summary>
        /// <param name="pOSPrinterOverrideOptionsDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(POSPrinterOverrideOptionsDTO pOSPrinterOverrideOptionsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(pOSPrinterOverrideOptionsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSPrinterOverrideOptionId", pOSPrinterOverrideOptionsDTO.POSPrinterOverrideOptionId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OptionName", pOSPrinterOverrideOptionsDTO.OptionName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OptionDescription", pOSPrinterOverrideOptionsDTO.OptionDescription));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", pOSPrinterOverrideOptionsDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", pOSPrinterOverrideOptionsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastupdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private POSPrinterOverrideOptionsDTO GetPOSPrinterOverrideOptionsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            POSPrinterOverrideOptionsDTO pOSPrinterOverrideOptionsDTO = new POSPrinterOverrideOptionsDTO(
                dataRow["POSPrinterOverrideOptionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSPrinterOverrideOptionId"]),
                dataRow["OptionName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["OptionName"]),
                dataRow["OptionDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["OptionDescription"]),
                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                );
            log.LogMethodExit(pOSPrinterOverrideOptionsDTO);
            return pOSPrinterOverrideOptionsDTO;
        }

        internal POSPrinterOverrideOptionsDTO GetPOSPrinterOverrideOptionsDTO(int pOSPrinterOverrideOptionId)
        {
            log.LogMethodEntry(pOSPrinterOverrideOptionId);
            POSPrinterOverrideOptionsDTO result = null;
            string query = SELECT_QUERY + @" WHERE pp.POSPrinterOverrideOptionId = @POSPrinterOverrideOptionId";
            SqlParameter parameter = new SqlParameter("@POSPrinterOverrideOptionId", pOSPrinterOverrideOptionId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetPOSPrinterOverrideOptionsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the List of POSPrinterOverrideOptionsDTO based on the POSPrinterOverrideOptions Id List
        /// </summary>
        /// <param name="pOSPrinterOverrideOptionsIdList">List of POSPrinterOverrideOptions Ids </param>
        /// <param name="activeRecords">activeRecords </param>
        /// <returns>returns the POSPrinterOverrideOptionsDTO List</returns>
        internal List<POSPrinterOverrideOptionsDTO> GetPOSPrinterOverrideOptionsDTOList(List<int> pOSPrinterOverrideOptionsIdList, bool activeRecords)
        {
            log.LogMethodEntry(pOSPrinterOverrideOptionsIdList, activeRecords);
            List<POSPrinterOverrideOptionsDTO> pOSPrinterOverrideOptionsDTOList = new List<POSPrinterOverrideOptionsDTO>();
            string query = @"SELECT *
                            FROM POSPrinterOverrideOptions, @POSPrinterOverrideOptionId List
                            WHERE POSPrinterOverrideOptionId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@POSPrinterOverrideOptionId", pOSPrinterOverrideOptionsIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                pOSPrinterOverrideOptionsDTOList = table.Rows.Cast<DataRow>().Select(x => GetPOSPrinterOverrideOptionsDTO(x)).ToList();
            }
            log.LogMethodExit(pOSPrinterOverrideOptionsDTOList);
            return pOSPrinterOverrideOptionsDTOList;
        }

        internal void Delete(POSPrinterOverrideOptionsDTO pOSPrinterOverrideOptionsDTO)
        {
            log.LogMethodEntry(pOSPrinterOverrideOptionsDTO);
            string query = @"DELETE  
                             FROM POSPrinterOverrideOptions
                             WHERE POSPrinterOverrideOptionId = @POSPrinterOverrideOptionId";
            SqlParameter parameter = new SqlParameter("@POSPrinterOverrideOptionId", pOSPrinterOverrideOptionsDTO.POSPrinterOverrideOptionId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }
        private void RefreshPOSPrinterOverrideOptionsDTO(POSPrinterOverrideOptionsDTO pOSPrinterOverrideOptionsDTO, DataTable dt)
        {
            log.LogMethodEntry(pOSPrinterOverrideOptionsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                pOSPrinterOverrideOptionsDTO.POSPrinterOverrideOptionId = Convert.ToInt32(dt.Rows[0]["POSPrinterOverrideOptionId"]);
                pOSPrinterOverrideOptionsDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                pOSPrinterOverrideOptionsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                pOSPrinterOverrideOptionsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                pOSPrinterOverrideOptionsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                pOSPrinterOverrideOptionsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                pOSPrinterOverrideOptionsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
        internal POSPrinterOverrideOptionsDTO Insert(POSPrinterOverrideOptionsDTO pOSPrinterOverrideOptionsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(pOSPrinterOverrideOptionsDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[POSPrinterOverrideOptions]
                               ([OptionName]
                               ,[OptionDescription]
                               ,[IsActive]
                               ,[CreatedBy]
                               ,[CreationDate]
                               ,[LastUpdatedBy]
                               ,[LastUpdatedDate]
                               ,[Guid]
                               ,[site_id]
                               ,[MasterEntityId]) 
                         VALUES
                               (
                                @OptionName,
                                @OptionDescription,
                                @IsActive,
                                @CreatedBy,
                                GETDATE(),
                                @LastUpdatedBy,
                                GETDATE(),
                                NEWID(), 
                                @SiteId,
                                @MasterEntityId
                                 )
                                SELECT * FROM POSPrinterOverrideOptions WHERE POSPrinterOverrideOptionId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(pOSPrinterOverrideOptionsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPOSPrinterOverrideOptionsDTO(pOSPrinterOverrideOptionsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(pOSPrinterOverrideOptionsDTO);
            return pOSPrinterOverrideOptionsDTO;
        }


        internal POSPrinterOverrideOptionsDTO Update(POSPrinterOverrideOptionsDTO pOSPrinterOverrideOptionsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(pOSPrinterOverrideOptionsDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[POSPrinterOverrideOptions] set 
                                    [OptionName]             = @OptionName,
                                    [OptionDescription]      = @OptionDescription,
                                    [IsActive]               = @IsActive,
                                    [MasterEntityId]         = @MasterEntityId,
                                    [LastUpdatedBy]          = @LastUpdatedBy,
                                    [LastUpdatedDate]        = GETDATE()
                                    where POSPrinterOverrideOptionId = @POSPrinterOverrideOptionId
                                    SELECT * FROM POSPrinterOverrideOptions WHERE POSPrinterOverrideOptionId = @POSPrinterOverrideOptionId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(pOSPrinterOverrideOptionsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPOSPrinterOverrideOptionsDTO(pOSPrinterOverrideOptionsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(pOSPrinterOverrideOptionsDTO);
            return pOSPrinterOverrideOptionsDTO;
        }

        internal List<POSPrinterOverrideOptionsDTO> GetPOSPrinterOverrideOptionsDTOList(List<KeyValuePair<POSPrinterOverrideOptionsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<POSPrinterOverrideOptionsDTO> pOSPrinterOverrideOptionsDTOList = new List<POSPrinterOverrideOptionsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<POSPrinterOverrideOptionsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == POSPrinterOverrideOptionsDTO.SearchByParameters.POS_PRINTER_OVERRIDE_OPTION_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(POSPrinterOverrideOptionsDTO.SearchByParameters.OPTION_NAME) ||
                                 searchParameter.Key.Equals(POSPrinterOverrideOptionsDTO.SearchByParameters.OPTION_DESCRIPTION))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == POSPrinterOverrideOptionsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == POSPrinterOverrideOptionsDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == POSPrinterOverrideOptionsDTO.SearchByParameters.POS_PRINTER_OVERRIDE_OPTION_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
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
                pOSPrinterOverrideOptionsDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetPOSPrinterOverrideOptionsDTO(x)).ToList();
            }
            log.LogMethodExit(pOSPrinterOverrideOptionsDTOList);
            return pOSPrinterOverrideOptionsDTOList;
        }
    }
    
}
