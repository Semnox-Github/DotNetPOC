/********************************************************************************************
 * Project Name - Game Audit Handler                                                                          
 * Description  - Data handler of the Game Audit class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.40        18-Sep-2018   Rajiv          Created files to fetch Audit values for games, Machine and gameProfile.
 *2.40        26-Oct-2018   Jagan          Removed the individual gamedto,gameprofiledto,machinedto and added the 
 *                                         generic list logic and returns common dynamic list for all audit pages
 **********************************************************************************************
 *2.60        08-Mar-2019   Akshay         Added DBSearchParameters,searchParameters, GamesAuditHandler() Constructor and SetSelectGuidQuery() method
 *                                         Modified GetDynamicAuditList(columnsDataTable), GetAuditList() methods
 *2.60.2      27-May-2019   Mehraj         Added the new AuditTable() for saving the audit details
 *            27-May-2019   Jagan Mohana   Added the new parameter to the AuditTable()
 ***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.GenericUtilities
{
    public class AuditLogDataHandler
    {
        logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        private string selectQueryforGuid;
        private string tableName;
        private List<KeyValuePair<AuditLogParams.SearchByAuditParameters, string>> searchParameters;
        private static readonly Dictionary<AuditLogParams.SearchByAuditParameters, string> DBSearchParameters = new Dictionary<AuditLogParams.SearchByAuditParameters, string>
            {
                {AuditLogParams.SearchByAuditParameters.AUDIT_ID,  "AuditId"},
                {AuditLogParams.SearchByAuditParameters.RECORD_ID,  "RecordID"},
                {AuditLogParams.SearchByAuditParameters.SITE_ID,  "site_id"},
                {AuditLogParams.SearchByAuditParameters.TABLE_NAME,  "TABLE_NAME"},
                {AuditLogParams.SearchByAuditParameters.FIELD_NAME,  "FieldName"}
            };
        private readonly SqlTransaction sqlTransaction = null;

        /// <summary>
        /// Constructor of the class GamesAuditHandler
        /// </summary>
        public AuditLogDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            selectQueryforGuid = null;
            this.sqlTransaction = sqlTransaction;
            searchParameters = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor to initialize the searchParameters
        /// </summary>
        /// <param name="searchParameters"></param>
        public AuditLogDataHandler(List<KeyValuePair<AuditLogParams.SearchByAuditParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            this.searchParameters = searchParameters;
            this.dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit(searchParameters);
        }
        /// <summary>
        /// Sets the selectQueryforGuid for fetching the Guid using searchParameters
        /// </summary>
        /// <param name="searchParameters"></param>
        private void SetSelectGuidQuery()
        {
            log.LogMethodEntry();
            int count = 0;
            string selectGuidQuery = @"SELECT Guid FROM ";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" ");

                foreach (KeyValuePair<AuditLogParams.SearchByAuditParameters, string> searchParameter in searchParameters)
                {
                    joiner = (count == 0) ? selectGuidQuery : " where ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (count == 0)
                        {
                            if (searchParameter.Key == AuditLogParams.SearchByAuditParameters.TABLE_NAME)
                            {
                                joiner = joiner + searchParameter.Value;
                            }
                            count++;
                        }
                        if (count == 1)
                        {
                            query.Append(joiner);
                            count++;
                        }
                    }
                }
                query.Append(joiner);
                foreach (KeyValuePair<AuditLogParams.SearchByAuditParameters, string> searchParameter in searchParameters)
                {
                    joiner = (count == 2) ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AuditLogParams.SearchByAuditParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            count++;
                        }
                        else if (searchParameter.Key == AuditLogParams.SearchByAuditParameters.FIELD_NAME)
                        {
                            query.Append(joiner + searchParameter.Value + " =");
                        }
                        else if (searchParameter.Key == AuditLogParams.SearchByAuditParameters.AUDIT_ID)
                        {
                            query.Append(searchParameter.Value);
                            count++;
                        }
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                this.selectQueryforGuid = selectQueryforGuid + query;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Fetches the list of Common Audit details for games/game_profile/machines/Products/ProductGames/ProductCreditPlus/ProductCreditPlusConsumption
        /// </summary>
        /// <returns>Returns List<List<string>>retruns the list string </returns>
        public List<List<List<string>>> GetAuditList()
        {
            try
            {
                log.LogMethodEntry();
                int count = 0;
                string selectQuery = @"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS";
                if ((searchParameters != null) && (searchParameters.Count > 0))
                {
                    string joiner = "";
                    StringBuilder query = new StringBuilder(" WHERE ");
                    foreach (KeyValuePair<AuditLogParams.SearchByAuditParameters, string> searchParameter in searchParameters)
                    {
                        if (DBSearchParameters.ContainsKey(searchParameter.Key))
                        {
                            joiner = (count == 0) ? string.Empty : " AND ";
                            if (searchParameter.Key == AuditLogParams.SearchByAuditParameters.TABLE_NAME)
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "'");
                                this.tableName = searchParameter.Value;
                            }
                            count++;
                        }
                    }
                    selectQuery = selectQuery + query;
                }

                DataTable columnsDataTable = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
                SetSelectGuidQuery();
                List<List<List<string>>> auditList = GetDynamicAuditList(columnsDataTable);
                log.LogMethodExit(auditList);
                return auditList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Fetch the list of Common Audit details for games/game_profile/machines/Products/ProductGames/ProductCreditPlus/ProductCreditPlusConsumption
        /// </summary>
        /// <param name="columnsDataTable">columnsDataTable</param>
        /// <returns>returns common dynamic list string for all audit pages</returns>
        private List<List<List<string>>> GetDynamicAuditList(DataTable columnsDataTable)
        {
            log.LogMethodEntry(columnsDataTable);
            try
            {
                log.LogMethodEntry(columnsDataTable);
                DataTable resultAuditTable = new DataTable();
                DataTable auditDataTable = new DataTable();
                List<List<string>> auditList = new List<List<string>>();
                DataTable recordIdDt = dataAccessHandler.executeSelectQuery(selectQueryforGuid, null, sqlTransaction);
                string selectMainQuerytemp = @"SELECT DateOfLog, Type, FieldName, NewValue, UserName
				                                        FROM DBAuditLog
				                                        WHERE TableName = @TableName 
                                                        and RecordId = @RecordId
                                                        order by DateOfLog desc";
                List<SqlParameter> updateGameParameterstemp = new List<SqlParameter>();
                if (recordIdDt.Rows.Count != 0)//Added on 29-Mar-2019 By Akshay Gulaganji
                {
                    updateGameParameterstemp.Add(new SqlParameter("@RecordId", recordIdDt.Rows[0].ItemArray[0]));
                }
                else
                {
                    updateGameParameterstemp.Add(new SqlParameter("@RecordId", DBNull.Value));
                }
                updateGameParameterstemp.Add(new SqlParameter("@TableName", this.tableName));
                auditDataTable = dataAccessHandler.executeSelectQuery(selectMainQuerytemp, updateGameParameterstemp.ToArray(), sqlTransaction);

                resultAuditTable.Columns.Add("DateOfLog");
                resultAuditTable.Columns.Add("Type");
                resultAuditTable.Columns.Add("UserName");
                foreach (DataRow dr in columnsDataTable.Rows)
                {
                    resultAuditTable.Columns.Add(dr[0].ToString());
                }
                // It will add the new value record from audit table -- Jagan Mohana
                DateTime prevDate = DateTime.MinValue;
                foreach (DataRow dr in auditDataTable.Rows)
                {
                    if (!prevDate.Equals((DateTime)dr["DateOfLog"]))
                    {
                        resultAuditTable.Rows.Add();
                        resultAuditTable.Rows[resultAuditTable.Rows.Count - 1]["DateOfLog"] = dr["DateOfLog"];
                        resultAuditTable.Rows[resultAuditTable.Rows.Count - 1]["Type"] = dr["Type"];
                        resultAuditTable.Rows[resultAuditTable.Rows.Count - 1]["Username"] = dr["Username"];
                        prevDate = (DateTime)dr["DateOfLog"];
                    }
                    resultAuditTable.Rows[resultAuditTable.Rows.Count - 1][dr["FieldName"].ToString()] = dr["NewValue"];
                }
                // Dynamically andding the list string columns as header and values
                List<string> auditHeader = new List<string>();
                foreach (DataColumn dc in resultAuditTable.Columns)
                {
                    auditHeader.Add(dc.ColumnName.ToString());
                }
                auditList.Add(auditHeader);

                List<List<string>> tempAuditList = new List<List<string>>();
                List<List<List<string>>> auditmainList = new List<List<List<string>>>();
                foreach (DataRow dr in resultAuditTable.Rows)
                {
                    List<string> auditContent = new List<string>();
                    foreach (DataColumn dc in resultAuditTable.Columns)
                    {
                        auditContent.Add(dr[dc.ColumnName].ToString());
                    }
                    tempAuditList.Add(auditContent);
                }
                auditmainList.Add(auditList);
                if (tempAuditList.Count != 0)
                    auditmainList.Add(tempAuditList);
                log.LogMethodExit(auditmainList);
                return auditmainList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// Save audit log details for all modules
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="guid"></param>
        public void AuditTable(string tableName, string guid, string userName,int siteId = -1)
        {
            try
            {
                log.LogMethodEntry(tableName, guid, userName, siteId);
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(dataAccessHandler.GetSQLParameter("@tableName", tableName));
                parameters.Add(dataAccessHandler.GetSQLParameter("@guid", guid));
                if (siteId > -1)
                {
                    parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
                    dataAccessHandler.executeScalar(@"exec AuditDBTable @tableName, '" + userName + "', @guid, @siteId ", parameters.ToArray(), sqlTransaction);
                }
                else
                {
                    dataAccessHandler.executeScalar(@"exec AuditDBTable @tableName, '" + userName + "', @guid ", parameters.ToArray(), sqlTransaction);
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
    }
}
