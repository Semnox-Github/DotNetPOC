/********************************************************************************************
 * Project Name - POSProductExclusions Data handler
 * Description  - Data handler for POSProductExclusions 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60.0      08-Mar-2019   Archana                 Created 
 *2.60.2      10-Jun-2019   Akshay Gulaganji        Code merge from Development to WebManagementStudio
 *2.70.0      29-June-2019  Jagan Mohana            Created DeletePOSproductExclusions() method.
 *2.70.2        10-Dec-2019   Jinto Thomas            Removed siteid from update query
 *2.80        10-Mar-2020   Vikas Dwivedi           Modified as per the Standards for RESTApi Phase 1 Changes.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class POSProductExclusionsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;

        /// <summary>
        /// Dictionary for searching Parameters for the ProductExclusion object.
        /// </summary>
        private static readonly Dictionary<POSProductExclusionsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<POSProductExclusionsDTO.SearchByParameters, string>
            {
                {POSProductExclusionsDTO.SearchByParameters.EXCLUSION_ID, "pe.ExclusionId"},
                {POSProductExclusionsDTO.SearchByParameters.POS_MACHINE_ID_LIST, "pe.POSMachineId"},
                {POSProductExclusionsDTO.SearchByParameters.POS_MACHINE_ID, "pe.POSMachineId"},
                {POSProductExclusionsDTO.SearchByParameters.PRODUCT_DISPLAY_GROUP_FORMAT_ID, "pe.ProductDisplayGroupFormatId"},
                {POSProductExclusionsDTO.SearchByParameters.POS_TYPE_ID, "pe.PTypeId"},
                {POSProductExclusionsDTO.SearchByParameters.ACTIVE_FLAG, "pe.IsActive"},
                {POSProductExclusionsDTO.SearchByParameters.MASTER_ENTITY_ID,"pe.MasterEntityId"},
                {POSProductExclusionsDTO.SearchByParameters.SITE_ID, "pe.site_id"}
            };

        /// <summary>
        /// Default constructor of POSProductExclusionsDataHandler class
        /// </summary>
        public POSProductExclusionsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating POSProductExclusion Record.
        /// </summary>
        /// <param name="POSProductExclusionsDTO">POSProductExclusionsDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(POSProductExclusionsDTO posProductExclusionsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(posProductExclusionsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExclusionId", posProductExclusionsDTO.ExclusionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSMachineId", posProductExclusionsDTO.PosMachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductDisplayGroupFormatId", posProductExclusionsDTO.ProductDisplayGroupFormatId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSTypeId", posProductExclusionsDTO.PosTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductGroup", posProductExclusionsDTO.ProductGroup));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", posProductExclusionsDTO.IsActive ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", posProductExclusionsDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the POSProductExclusions record to the database
        /// </summary>
        /// <param name="POSProductExclusionsDTO">POSProductExclusionsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted POSProductExclusionsDTO</returns>
        public POSProductExclusionsDTO Insert(POSProductExclusionsDTO posProductExclusionsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(posProductExclusionsDTO, loginId, siteId);
            string query = @"INSERT INTO POSProductExclusions 
                                        ( 
                                            [POSMachineId],
                                            [ProductGroup],
                                            [POSTypeId],
                                            [ProductDisplayGroupFormatId],
                                            [IsActive],
                                            [CreatedBy],
                                            [CreationDate],
                                            [LastUpdatedBy],
                                            [LastUpdateDate],
                                            [site_id],
                                            [MasterEntityId]
                                        ) 
                                VALUES 
                                        (
                                            @POSMachineId,
                                            @ProductGroup,
                                            @POSTypeId,
                                            @ProductDisplayGroupFormatId,
                                            @IsActive,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @MasterEntityId
                                           )SELECT * FROM POSProductExclusions WHERE ExclusionId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(posProductExclusionsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPOSProductExclusionsDTO(posProductExclusionsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(posProductExclusionsDTO);
            return posProductExclusionsDTO;
        }

        /// <summary>
        /// Updates the product exclusion record
        /// </summary>
        /// <param name="POSProductExclusionsDTO">POSProductExclusionsDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the updated POSProductExclusionsDTO</returns>
        public POSProductExclusionsDTO Update(POSProductExclusionsDTO posProductExclusionsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(posProductExclusionsDTO, loginId, siteId);
            string query = @"UPDATE POSProductExclusions 
                             SET [POSMachineId] = @POSMachineId,
                                 [ProductGroup] = @ProductGroup,
                                 [POSTypeId] = @POSTypeId,
                                 [ProductDisplayGroupFormatId] = @ProductDisplayGroupFormatId,
                                 [IsActive] = @IsActive,
                                 [LastUpdatedBy] = @LastUpdatedBy,
                                 [LastUpdateDate] = GETDATE()
                                 -- [site_id] = @site_id
                             WHERE ExclusionId = @ExclusionId
                             SELECT * FROM POSProductExclusions WHERE ExclusionId = @ExclusionId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(posProductExclusionsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPOSProductExclusionsDTO(posProductExclusionsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(posProductExclusionsDTO);
            return posProductExclusionsDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="pOSProductExclusionsDTO">POSProductExclusionsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshPOSProductExclusionsDTO(POSProductExclusionsDTO pOSProductExclusionsDTO, DataTable dt)
        {
            log.LogMethodEntry(pOSProductExclusionsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                pOSProductExclusionsDTO.ExclusionId = Convert.ToInt32(dt.Rows[0]["ExclusionId"]);
                pOSProductExclusionsDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                pOSProductExclusionsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                pOSProductExclusionsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                pOSProductExclusionsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                pOSProductExclusionsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                pOSProductExclusionsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the product exclusion record
        /// </summary>
        /// <param name="POSProductExclusionsDTO">POSProductExclusionsDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public void Delete(int posProductExclusionsId)
        {
            log.LogMethodEntry(posProductExclusionsId);
            try
            {
                string query = @"Delete FROM POSProductExclusions WHERE ExclusionId = @ExclusionId";
                SqlParameter parameters = new SqlParameter("@ExclusionId", posProductExclusionsId);
                dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameters }, sqlTransaction);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
        }

        internal List<POSProductExclusionsDTO> GetPOSProductExclusionsDTOList(List<int> pOSMachineIdList, bool activeRecords)
        {
            log.LogMethodEntry(pOSMachineIdList);
            List<POSProductExclusionsDTO> pOSProductExclusionsDTOList = new List<POSProductExclusionsDTO>();
            //string query = @"SELECT ProductGroup as DisplayGroup ,* from POSProductExclusions, @POSMachineIdList List
            // WHERE POSMachineId = List.Id ";
            string query = @"SELECT pe.*, pdg.DisplayGroup as DisplayGroup FROM @POSMachineIdList List ,POSProductExclusions as pe
                            left outer join ProductDisplayGroupFormat as pdg on pe.ProductDisplayGroupFormatId = pdg.Id
                            WHERE pe.POSMachineId = List.Id";
            if (activeRecords)
            {
                query += " AND isnull(pe.isActive,'Y') = 'Y' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@POSMachineIdList", pOSMachineIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                pOSProductExclusionsDTOList = table.Rows.Cast<DataRow>().Select(x => GetPOSProductExclusionsDTO(x)).ToList();
            }
            log.LogMethodExit(pOSProductExclusionsDTOList);
            return pOSProductExclusionsDTOList;
        }

        /// <summary>
        /// Converts the Data row object to POSProductExclusionsDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns POSProductExclusionsDTO</returns>
        private POSProductExclusionsDTO GetPOSProductExclusionsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            POSProductExclusionsDTO POSProductExclusionsDTO = new POSProductExclusionsDTO(Convert.ToInt32(dataRow["ExclusionId"]),
                                            dataRow["POSMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSMachineId"]),
                                            dataRow["DisplayGroup"] == DBNull.Value ? string.Empty : dataRow["DisplayGroup"].ToString(),
                                            dataRow["IsActive"] == DBNull.Value ? true : dataRow["IsActive"].ToString() == "Y" ? true : false,
                                            dataRow["POSTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSTypeId"]),
                                            dataRow["ProductDisplayGroupFormatId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductDisplayGroupFormatId"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString()
                                            );
            log.LogMethodExit(POSProductExclusionsDTO);
            return POSProductExclusionsDTO;
        }

        /// <summary>
        /// Gets the POSProductExclusions data of passed POSProductExclusions Id
        /// </summary>
        /// <param name="exclusionId">integer type parameter</param>
        /// <returns>Returns POSProductExclusionsDTO</returns>
        public POSProductExclusionsDTO GetPOSProductExclusionsDTO(int exclusionId)
        {
            log.LogMethodEntry(exclusionId);
            POSProductExclusionsDTO pOSProductExclusionsDTO = null;
            string query = @"SELECT pe.*, pdg.DisplayGroup as DisplayGroup FROM POSProductExclusions as pe 
                                  left outer join ProductDisplayGroupFormat as pdg on pe.ProductDisplayGroupFormatId = pdg.Id
                                  WHERE pe.ExclusionId = @ExclusionId";
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { dataAccessHandler.GetSQLParameter("@ExclusionId", exclusionId, true) }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                pOSProductExclusionsDTO = GetPOSProductExclusionsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(pOSProductExclusionsDTO);
            return pOSProductExclusionsDTO;
        }

        /// <summary>
        /// Gets the POSProductExclusionsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of POSProductExclusionsDTO matching the search criteria</returns>
        public List<POSProductExclusionsDTO> GetPOSProductExclusionsDTOList(List<KeyValuePair<POSProductExclusionsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<POSProductExclusionsDTO> pOSProductExclusionsDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = @"SELECT pe.*, pdg.DisplayGroup as DisplayGroup FROM POSProductExclusions as pe ";
            StringBuilder joinQuery = new StringBuilder("left outer join ProductDisplayGroupFormat as pdg on pe.ProductDisplayGroupFormatId = pdg.Id");
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<POSProductExclusionsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == POSProductExclusionsDTO.SearchByParameters.EXCLUSION_ID ||
                            searchParameter.Key == POSProductExclusionsDTO.SearchByParameters.POS_MACHINE_ID ||
                            searchParameter.Key == POSProductExclusionsDTO.SearchByParameters.POS_TYPE_ID ||
                            searchParameter.Key == POSProductExclusionsDTO.SearchByParameters.PRODUCT_DISPLAY_GROUP_FORMAT_ID ||
                            searchParameter.Key == POSProductExclusionsDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(POSProductExclusionsDTO.SearchByParameters.POS_MACHINE_ID_LIST))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == POSProductExclusionsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == POSProductExclusionsDTO.SearchByParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
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
                selectQuery = selectQuery + joinQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                pOSProductExclusionsDTOList = new List<POSProductExclusionsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    POSProductExclusionsDTO POSProductExclusionsDTO = GetPOSProductExclusionsDTO(dataRow);
                    pOSProductExclusionsDTOList.Add(POSProductExclusionsDTO);
                }
            }
            log.LogMethodExit(pOSProductExclusionsDTOList);
            return pOSProductExclusionsDTOList;
        }
    }
}

