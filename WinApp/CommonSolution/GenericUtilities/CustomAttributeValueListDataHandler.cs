/********************************************************************************************
* Project Name - CustomAttributeValueList Data Handler
* Description  - Data handler of the CustomAttributeValueList class
* 
**************
**Version Log
**************
*Version     Date          Modified By         Remarks          
*********************************************************************************************
*1.00        15-May-2017   Lakshminarayana     Created 
*2.50.0      12-dec-2018   Guru S A            Who column changes
*2.70.2        25-Jul-2019   Dakshakh raj        Modified : added GetSQLParameters(), 
*                                                          SQL injection Issue Fix.
*            02-Aug-2019   Mushahid Faizan     Added Delete method for Hard-Deletion.
*2.70.2        06-Dec-2019   Jinto Thomas            Removed siteid from update query             
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
    ///  CustomAttributeValueList Data Handler - Handles insert, update and select of  CustomAttributeValueList objects
    /// </summary>
    public class CustomAttributeValueListDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction = null;
        DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CustomAttributeValueList as cav ";

        /// <summary>
        /// Dictionary for searching Parameters for the CustomAttributeValueList object.
        /// </summary>
        private static readonly Dictionary<CustomAttributeValueListDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CustomAttributeValueListDTO.SearchByParameters, string>
            {
                {CustomAttributeValueListDTO.SearchByParameters.VALUE_ID, "cav.ValueId"},
                {CustomAttributeValueListDTO.SearchByParameters.CUSTOM_ATTRIBUTE_ID, "cav.CustomAttributeId"},
                {CustomAttributeValueListDTO.SearchByParameters.CUSTOM_ATTRIBUTE_ID_LIST, "cav.CustomAttributeId"},
                {CustomAttributeValueListDTO.SearchByParameters.VALUE, "cav.Value"},
                {CustomAttributeValueListDTO.SearchByParameters.SITE_ID, "cav.site_id"},
                {CustomAttributeValueListDTO.SearchByParameters.MASTER_ENTITY_ID, "cav.MasterEntityId"},
                {CustomAttributeValueListDTO.SearchByParameters.IS_ACTIVE, "cav.IsActive"}
            };

        /// <summary>
        /// Default constructor of CustomAttributeValueListDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CustomAttributeValueListDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit(null);
        }

        /// <summary>
        ///Builds the SQL Parameter list used for inserting and updating CustomAttributeValueList Record.
        /// </summary>
        /// <param name="customAttributeValueListDTO">customAttributeValueListDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns> Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(CustomAttributeValueListDTO customAttributeValueListDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customAttributeValueListDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ValueId", customAttributeValueListDTO.ValueId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomAttributeId", customAttributeValueListDTO.CustomAttributeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Value", customAttributeValueListDTO.Value));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isDefault", customAttributeValueListDTO.IsDefault));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", customAttributeValueListDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", customAttributeValueListDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the CustomAttributeValueList record to the database
        /// </summary>
        /// <param name="customAttributeValueListDTO">CustomAttributeValueListDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public CustomAttributeValueListDTO InsertCustomAttributeValueList(CustomAttributeValueListDTO customAttributeValueListDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customAttributeValueListDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[CustomAttributeValueList] 
                                        (   Value,
                                            CustomAttributeId,
                                            isDefault,
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
                                            @Value,
                                            @CustomAttributeId,
                                            @isDefault,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @MasterEntityId,
                                            @IsActive
                                        )SELECT * FROM CustomAttributeValueList WHERE ValueId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customAttributeValueListDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomAttributeValueListDTO(customAttributeValueListDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting CustomAttributeValueListDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customAttributeValueListDTO);
            return customAttributeValueListDTO;
        }

        /// <summary>
        /// Updates the CustomAttributeValueList record
        /// </summary>
        /// <param name="customAttributeValueListDTO">CustomAttributeValueListDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public CustomAttributeValueListDTO UpdateCustomAttributeValueList(CustomAttributeValueListDTO customAttributeValueListDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customAttributeValueListDTO, loginId, siteId);
            string query = @"UPDATE CustomAttributeValueList 
                             SET Value=@Value,
                                 CustomAttributeId=@CustomAttributeId,
                                 isDefault=@isDefault,
                                 LastUpdatedBy = @LastUpdatedBy,
                                 LastUpdateDate = GETDATE(),
                                 --site_id = @site_id,
                                 MasterEntityId = @MasterEntityId,
                                 IsActive = @IsActive
                             WHERE ValueId = @ValueId
                             SELECT* FROM CustomAttributeValueList WHERE  ValueId = @ValueId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customAttributeValueListDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomAttributeValueListDTO(customAttributeValueListDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating CustomAttributeValueListDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customAttributeValueListDTO);
            return customAttributeValueListDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="customAttributeValueListDTO">customAttributeValueListDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshCustomAttributeValueListDTO(CustomAttributeValueListDTO customAttributeValueListDTO, DataTable dt)
        {
            log.LogMethodEntry(customAttributeValueListDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                customAttributeValueListDTO.ValueId = Convert.ToInt32(dt.Rows[0]["ValueId"]);
                customAttributeValueListDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                customAttributeValueListDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                customAttributeValueListDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                customAttributeValueListDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                customAttributeValueListDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                customAttributeValueListDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to CustomAttributeValueListDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns CustomAttributeValueListDTO</returns>
        private CustomAttributeValueListDTO GetCustomAttributeValueListDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CustomAttributeValueListDTO customAttributeValueListDTO = new CustomAttributeValueListDTO(Convert.ToInt32(dataRow["ValueId"]),
                                            dataRow["Value"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Value"]),
                                            dataRow["CustomAttributeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomAttributeId"]),
                                            dataRow["isDefault"] == DBNull.Value ? "N" : Convert.ToString(dataRow["isDefault"]),
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
            log.LogMethodExit(customAttributeValueListDTO);
            return customAttributeValueListDTO;
        }

        /// <summary>
        /// Gets the CustomAttributeValueList data of passed CustomAttributeValueList Id
        /// </summary>
        /// <param name="valueId">integer type parameter</param>
        /// <returns>Returns CustomAttributeValueListDTO</returns>
        public CustomAttributeValueListDTO GetCustomAttributeValueListDTO(int valueId)
        {
            log.LogMethodEntry(valueId);
            CustomAttributeValueListDTO returnValue = null;
            string query = SELECT_QUERY + @" WHERE cav.ValueId = @ValueId";
            SqlParameter parameter = new SqlParameter("@ValueId", valueId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetCustomAttributeValueListDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the CustomAttributeValueListDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CustomAttributeValueListDTO matching the search criteria</returns>
        public List<CustomAttributeValueListDTO> GetCustomAttributeValueListDTOList(List<KeyValuePair<CustomAttributeValueListDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CustomAttributeValueListDTO> customAttributeValueListDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CustomAttributeValueListDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CustomAttributeValueListDTO.SearchByParameters.VALUE_ID
                            || searchParameter.Key == CustomAttributeValueListDTO.SearchByParameters.CUSTOM_ATTRIBUTE_ID
                            || searchParameter.Key == CustomAttributeValueListDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomAttributeValueListDTO.SearchByParameters.CUSTOM_ATTRIBUTE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN( " + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomAttributeValueListDTO.SearchByParameters.VALUE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'')= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomAttributeValueListDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomAttributeValueListDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
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
                customAttributeValueListDTOList = new List<CustomAttributeValueListDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CustomAttributeValueListDTO customAttributeValueListDTO  = GetCustomAttributeValueListDTO(dataRow);
                    customAttributeValueListDTOList.Add(customAttributeValueListDTO);
                }
            }
            log.LogMethodExit(customAttributeValueListDTOList);
            return customAttributeValueListDTOList;
        }
        /// <summary>
        /// Based on the valueId, appropriate CustomAttributeValueList record will be deleted
        /// This is for hard deletion. In future, when we implement soft deletion this method may not be required 
        /// </summary>
        /// <param name="valueId">valueId is passed as parameter</param>
        internal void Delete(int valueId)
        {
            log.LogMethodEntry(valueId);
            string query = @"DELETE  
                             FROM CustomAttributeValueList
                             WHERE CustomAttributeValueList.ValueId = @valueId";
            SqlParameter parameter = new SqlParameter("@valueId", valueId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }
    }
}
