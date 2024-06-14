/********************************************************************************************
 * Project Name - POS
 * Description  - POSCashdrawerDataHandler 
 * 
 **************
 **Version Log
 **************
 *Version      Date             Modified By    Remarks          
 *********************************************************************************************
 *2.130.0     11-Aug-2021      Girish Kundar     Created 

 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Drawing;
using Semnox.Core.Utilities;
using Semnox.Parafait.DisplayGroup;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.POS
{
    /// <summary>
    /// PrinterDataHandler class to get all the details about printers configured with system
    /// </summary>
    public class POSCashdrawerDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM POSCashdrawers AS pch ";
        /// <summary>
        /// Dictionary for searching Parameters for the AchievementClass object.
        /// </summary>
        private static readonly Dictionary<POSCashdrawerDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<POSCashdrawerDTO.SearchByParameters, string>
        {
                {POSCashdrawerDTO.SearchByParameters.POS_CASHDRAWER_ID,"pch.POSCashdrawerId"},
                {POSCashdrawerDTO.SearchByParameters.CASHDRAWER_ID,"pch.CashdrawerId"},
                {POSCashdrawerDTO.SearchByParameters.IS_ACTIVE, "pch.IsActive"},
                {POSCashdrawerDTO.SearchByParameters.POS_MACHINE_ID, "pch.POSMachineId"},
                {POSCashdrawerDTO.SearchByParameters.POS_MACHINE_ID_LIST, "pch.POSMachineId"},
                {POSCashdrawerDTO.SearchByParameters.SITE_ID, "pch.site_id"},
                {POSCashdrawerDTO.SearchByParameters.MASTER_ENTITY_ID, "pch.MasterEntityId"}
        };
        /// <summary>
        /// Default constructor of POSCashdrawerDataHandler class
        /// </summary>
        public POSCashdrawerDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating POSPrinter Record.
        /// </summary>
        /// <param name="posCashdrawerDTO">POSCashdrawerDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(POSCashdrawerDTO posCashdrawerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(posCashdrawerDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSCashdrawerId", posCashdrawerDTO.POSCashdrawerId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CashdrawerId", posCashdrawerDTO.CashdrawerId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@posMachineId", posCashdrawerDTO.POSMachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", posCashdrawerDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", posCashdrawerDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the POSCashdrawerDTO record to the database
        /// </summary>
        public POSCashdrawerDTO Insert(POSCashdrawerDTO posCashdrawerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(posCashdrawerDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[POSCashdrawers]
                                                       ( CashdrawerId,
                                                         POSMachineId,
                                                         IsActive,
                                                         CreatedBy,
                                                         CreationDate,
                                                         LastUpdatedDate,
                                                         LastUpdatedBy,
                                                         GUID,
                                                         site_id,
                                                         MasterEntityId )
                                               values(
                                                         @CashdrawerId,
                                                         @posMachineId,
                                                         @isActive,
                                                         @createdBy,
                                                         Getdate(),
                                                         Getdate(),
                                                         @lastUpdatedBy,
                                                         newId(),
                                                         @siteId,
                                                         @masterEntityId)
                             SELECT* FROM POSCashdrawers WHERE POSCashdrawerId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(posCashdrawerDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPOSCashdrawerDTO(posCashdrawerDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting POSCashdrawerDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(posCashdrawerDTO);
            return posCashdrawerDTO;
        }


        /// <summary>
        /// Updates the POSCashdrawerDTO record to the database
        /// </summary>
        public POSCashdrawerDTO Update(POSCashdrawerDTO posCashdrawerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(posCashdrawerDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[POSCashdrawers]
                           SET 
                                                  CashdrawerId = @CashdrawerId,
                                                  POSMachineId = @posMachineId,
                                                  IsActive = @isActive,
                                                  LastUpdatedDate = Getdate(),
                                                  LastUpdatedBy = @lastUpdatedBy
                                                  WHERE POSCashdrawerId =@POSCashdrawerId 
                                                    SELECT * FROM POSCashdrawers WHERE POSCashdrawerId = @POSCashdrawerId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(posCashdrawerDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPOSCashdrawerDTO(posCashdrawerDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating POSCashdrawerDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(posCashdrawerDTO);
            return posCashdrawerDTO;
        }
        /// <summary>
        /// Converts datatable to DTO
        /// </summary>
        private POSCashdrawerDTO GetPOSCashdrawerDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            POSCashdrawerDTO posCashdrawerDTO = new POSCashdrawerDTO(Convert.ToInt32(dataRow["POSCashdrawerId"]),
                                            dataRow["CashdrawerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CashdrawerId"]),
                                            dataRow["POSMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSMachineId"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(posCashdrawerDTO);
            return posCashdrawerDTO;
        }

        /// <summary>
        /// Gets the POS Printer data of passed POSPrinterId
        /// </summary>
        /// <param name="POSPrinterId">integer type parameter</param>
        /// <returns>Returns PrinterDTO</returns>
        public POSCashdrawerDTO GetPOSCashdrawer(int posCashDrawerId)
        {
            log.LogMethodEntry(posCashDrawerId);
            POSCashdrawerDTO result = null;
            string query = SELECT_QUERY + @" WHERE pch.POSCashDrawerId= @posCashDrawerId";
            SqlParameter parameter = new SqlParameter("@posCashDrawerId", posCashDrawerId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetPOSCashdrawerDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal List<POSCashdrawerDTO> GetPOSCashdrawerDTOList(List<int> pOSMachineIdList, bool activeRecords)
        {
            log.LogMethodEntry(pOSMachineIdList);
            List<POSCashdrawerDTO> pOSCashdrawerDTOList = new List<POSCashdrawerDTO>();
            string query = @"SELECT *
                            FROM POSCashdrawers, @POSMachineIdList List
                            WHERE POSMachineId = List.Id ";
            if (activeRecords)
            {
                query += " AND IsActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@POSMachineIdList", pOSMachineIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                pOSCashdrawerDTOList = table.Rows.Cast<DataRow>().Select(x => GetPOSCashdrawerDTO(x)).ToList();
            }
            log.LogMethodExit(pOSCashdrawerDTOList);
            return pOSCashdrawerDTOList;
        }

        private void RefreshPOSCashdrawerDTO(POSCashdrawerDTO posCashdrawerDTO, DataTable dt)
        {
            log.LogMethodEntry(posCashdrawerDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                posCashdrawerDTO.POSCashdrawerId = Convert.ToInt32(dt.Rows[0]["POSCashdrawerId"]);
                posCashdrawerDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                posCashdrawerDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                posCashdrawerDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                posCashdrawerDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                posCashdrawerDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                posCashdrawerDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the POSCashdrawerDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of POSCashdrawerDTO matching the search criteria</returns>
        public List<POSCashdrawerDTO> GetPOSCashdrawerList(List<KeyValuePair<POSCashdrawerDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            int count = 0;
            string selectPOSCashdrawerQuery = SELECT_QUERY;
            List<POSCashdrawerDTO> posCashdrawerList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<POSCashdrawerDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joiner = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key == POSCashdrawerDTO.SearchByParameters.POS_CASHDRAWER_ID
                             || searchParameter.Key == POSCashdrawerDTO.SearchByParameters.CASHDRAWER_ID
                             || searchParameter.Key == POSCashdrawerDTO.SearchByParameters.POS_MACHINE_ID
                             || searchParameter.Key == POSCashdrawerDTO.SearchByParameters.MASTER_ENTITY_ID
                            )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                      
                        else if (searchParameter.Key == POSCashdrawerDTO.SearchByParameters.POS_MACHINE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == POSCashdrawerDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == POSCashdrawerDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
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
                if (searchParameters.Count > 0)
                    selectPOSCashdrawerQuery = selectPOSCashdrawerQuery + query;
                selectPOSCashdrawerQuery = selectPOSCashdrawerQuery + " Order by POSCashdrawerId";
            }


            DataTable POSCashdrawerData = dataAccessHandler.executeSelectQuery(selectPOSCashdrawerQuery, parameters.ToArray(), sqlTransaction);
            if (POSCashdrawerData.Rows.Count > 0)
            {
                posCashdrawerList = new List<POSCashdrawerDTO>();
                foreach (DataRow dataRow in POSCashdrawerData.Rows)
                {
                    POSCashdrawerDTO posCashdrawerDTO = GetPOSCashdrawerDTO(dataRow);
                    posCashdrawerList.Add(posCashdrawerDTO);
                }
            }
            log.LogMethodExit(posCashdrawerList);
            return posCashdrawerList;
        }
    }
}
