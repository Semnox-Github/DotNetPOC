/********************************************************************************************
* Project Name - CustomAttributes Data Handler
* Description  - Data handler of the CustomAttributes class
* 
**************
**Version Log
**************
*Version     Date          Modified By         Remarks          
*********************************************************************************************
*1.00        15-May-2017   Lakshminarayana     Created 
*2.50.0      12-dec-2018   Guru S A            Who column changes
*2.70.2      25-Jul-2019   Dakshakh raj        Modified : added GetSQLParameters(), 
*                                                          SQL injection Issue Fix.
*            02-Aug-2019   Mushahid Faizan     Added Delete method for Hard-Deletion
*2.70.2      06-Dec-2019   Jinto Thomas        Removed siteid from update query             
*2.80.0      30-Apr-2020   Akshay G            Added NAME_LIST as searchParameter
*2.130.0     27-Apr-2020   Mushahid Faizan     Modified :- POS UI redesign changes.
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    ///  CustomAttributes Data Handler - Handles insert, update and select of  CustomAttributes objects
    /// </summary>
    public class CustomAttributesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction = null;
        DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CustomAttributes as ca ";

        /// <summary>
        /// Dictionary for searching Parameters for the CustomAttributes object.
        /// </summary>
        private static readonly Dictionary<CustomAttributesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CustomAttributesDTO.SearchByParameters, string>
            {
                {CustomAttributesDTO.SearchByParameters.CUSTOM_ATTRIBUTE_ID, "ca.CustomAttributeId"},
                {CustomAttributesDTO.SearchByParameters.NAME, "ca.Name"},
                {CustomAttributesDTO.SearchByParameters.TYPE, "ca.Type"},
                {CustomAttributesDTO.SearchByParameters.APPLICABILITY, "ca.Applicability"},
                {CustomAttributesDTO.SearchByParameters.ACCESS, "ca.Access"},
                {CustomAttributesDTO.SearchByParameters.SITE_ID, "ca.site_id"},
                {CustomAttributesDTO.SearchByParameters.MASTER_ENTITY_ID, "ca.MasterEntityId"},
                {CustomAttributesDTO.SearchByParameters.IS_ACTIVE, "ca.IsActive"},
                {CustomAttributesDTO.SearchByParameters.NAME_LIST, "ca.Name"}
            };

        /// <summary>
        /// Default constructor of CustomAttributesDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CustomAttributesDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CustomAttributes Record.
        /// </summary>
        /// <param name="customAttributesDTO">CustomAttributesDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CustomAttributesDTO customAttributesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customAttributesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomAttributeId", customAttributesDTO.CustomAttributeId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", customAttributesDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Sequence", customAttributesDTO.Sequence, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Type", customAttributesDTO.Type));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Access", customAttributesDTO.Access));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Applicability", customAttributesDTO.Applicability));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", customAttributesDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", customAttributesDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the CustomAttributes record to the database
        /// </summary>
        /// <param name="customAttributesDTO">CustomAttributesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public CustomAttributesDTO InsertCustomAttributes(CustomAttributesDTO customAttributesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customAttributesDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[CustomAttributes]  
                                        (   Name,
                                            Sequence,
                                            Type,
                                            Applicability,
                                            Access,  
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            site_id,
                                            MasterEntityId,
                                            IsActive
                                        ) 
                                VALUES 
                                        (
                                            @Name,
                                            @Sequence,
                                            @Type,
                                            @Applicability,
                                            @Access, 
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @MasterEntityId,
                                            @IsActive
                                        )SELECT * FROM CustomAttributes WHERE CustomAttributeId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customAttributesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomAttributesDTO(customAttributesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting customAttributes", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customAttributesDTO);
            return customAttributesDTO;
        }

        /// <summary>
        /// Updates the CustomAttributes record
        /// </summary>
        /// <param name="customAttributesDTO">CustomAttributesDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public CustomAttributesDTO UpdateCustomAttributes(CustomAttributesDTO customAttributesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customAttributesDTO, loginId, siteId);
            string query = @"UPDATE CustomAttributes 
                             SET Name=@Name,
                                 Sequence=@Sequence,
                                 Type=@Type,
                                 Applicability=@Applicability,
                                 Access=@Access,
                                 LastUpdatedBy = @LastUpdatedBy,
                                 LastUpdateDate = GETDATE(),
                                -- site_id = @site_id,
                                 MasterEntityId = @MasterEntityId,
                                 IsActive = @IsActive
                             WHERE CustomAttributeId = @CustomAttributeId
                             SELECT* FROM CustomAttributes WHERE  CustomAttributeId = @CustomAttributeId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customAttributesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomAttributesDTO(customAttributesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating CustomAttributesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customAttributesDTO);
            return customAttributesDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="customAttributesDTO">customAttributesDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshCustomAttributesDTO(CustomAttributesDTO customAttributesDTO, DataTable dt)
        {
            log.LogMethodEntry(customAttributesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                customAttributesDTO.CustomAttributeId = Convert.ToInt32(dt.Rows[0]["CustomAttributeId"]);
                customAttributesDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                customAttributesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                customAttributesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                customAttributesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                customAttributesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                customAttributesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to CustomAttributesDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns CustomAttributesDTO</returns>
        private CustomAttributesDTO GetCustomAttributesDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CustomAttributesDTO customAttributesDTO = new CustomAttributesDTO(Convert.ToInt32(dataRow["CustomAttributeId"]),
                                            dataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Name"]),
                                            dataRow["Sequence"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Sequence"]),
                                            dataRow["Type"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Type"]),
                                            dataRow["Applicability"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Applicability"]),
                                            dataRow["Access"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Access"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])
                                            );
            log.LogMethodExit(customAttributesDTO);
            return customAttributesDTO;
        }

        /// <summary>
        /// Gets the CustomAttributes data of passed CustomAttributes Id
        /// </summary>
        /// <param name="customAttributeId">integer type parameter</param>
        /// <returns>Returns CustomAttributesDTO</returns>
        public CustomAttributesDTO GetCustomAttributesDTO(int customAttributeId)
        {
            log.LogMethodEntry(customAttributeId);
            CustomAttributesDTO returnValue = null;
            string query = SELECT_QUERY + @" WHERE ca.CustomAttributeId = @CustomAttributeId";
            SqlParameter parameter = new SqlParameter("@CustomAttributeId", customAttributeId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetCustomAttributesDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the CustomAttributesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CustomAttributesDTO matching the search criteria</returns>
        public List<CustomAttributesDTO> GetCustomAttributesDTOList(List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CustomAttributesDTO> customAttributesDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CustomAttributesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CustomAttributesDTO.SearchByParameters.CUSTOM_ATTRIBUTE_ID
                            || searchParameter.Key == CustomAttributesDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomAttributesDTO.SearchByParameters.TYPE
                                 || searchParameter.Key == CustomAttributesDTO.SearchByParameters.APPLICABILITY
                                 || searchParameter.Key == CustomAttributesDTO.SearchByParameters.ACCESS)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomAttributesDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomAttributesDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));

                        }
                        else if (searchParameter.Key == CustomAttributesDTO.SearchByParameters.NAME_LIST) // value - NAME list 
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + "))");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
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
                customAttributesDTOList = new List<CustomAttributesDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CustomAttributesDTO customAttributesDTO = GetCustomAttributesDTO(dataRow);
                    customAttributesDTOList.Add(customAttributesDTO);
                }
            }
            log.LogMethodExit(customAttributesDTOList);
            return customAttributesDTOList;
        }

        internal DateTime? GetcustomAttributesLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdateDate) LastUpdateDate 
                            FROM (
                            select max(LastUpdateDate) LastUpdateDate from CustomAttributes WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdatedDate from CustomAttributeValueList WHERE (site_id = @siteId or @siteId = -1)
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

        /// <summary>
        /// Based on the customAttributeId, appropriate CustomAttributes record will be deleted
        /// This is for hard deletion. In future, when we implement soft deletion this method may not be required
        /// </summary>
        /// <param name="customAttributeId">customAttributeId is passed as parameter</param>
        internal void Delete(int customAttributeId)
        {
            log.LogMethodEntry(customAttributeId);
            string query = @"DELETE  
                             FROM CustomAttributes
                             WHERE CustomAttributes.CustomAttributeId = @customAttributeId";
            SqlParameter parameter = new SqlParameter("@customAttributeId", customAttributeId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }
    }
}
