/********************************************************************************************
 * Project Name - TableAttributeSetup
 * Description  - AttributeEnabledTablesDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.140.0      16-Aug-2021    Fiona         Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.TableAttributeSetup
{
    public class AttributeEnabledTablesDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM AttributeEnabledTables AS aet ";
        private static readonly Dictionary<AttributeEnabledTablesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AttributeEnabledTablesDTO.SearchByParameters, string>
            {
                {AttributeEnabledTablesDTO.SearchByParameters.ATTRIBUTE_ENABLED_TABLE_ID, "aet.AttributeEnabledTableId"},
                {AttributeEnabledTablesDTO.SearchByParameters.IS_ACTIVE, "aet.IsActive"},
                {AttributeEnabledTablesDTO.SearchByParameters.MASTER_ENTITY_ID, "aet.MasterEntityId"},
                {AttributeEnabledTablesDTO.SearchByParameters.SITE_ID, "aet.site_id"}
            };
        /// <summary>
        /// Default constructor of AttributeEnabledTablesDataHandler class
        /// </summary>
        public AttributeEnabledTablesDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
        private List<SqlParameter> BuildSQLParameters(AttributeEnabledTablesDTO attributeEnabledTablesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(attributeEnabledTablesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@AttributeEnabledTableId", attributeEnabledTablesDTO.AttributeEnabledTableId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TableName", attributeEnabledTablesDTO.TableName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Description", attributeEnabledTablesDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", attributeEnabledTablesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", attributeEnabledTablesDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeEnabledTablesDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public AttributeEnabledTablesDTO Insert(AttributeEnabledTablesDTO attributeEnabledTablesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(attributeEnabledTablesDTO, loginId, siteId);
            string insertQuery = @"insert into AttributeEnabledTables 
                                                     (                                                         
	                                                    TableName ,  
	                                                    Description , 
                                                        IsActive ,
                                                        CreatedBy ,
                                                        CreationDate ,
                                                        LastUpdatedBy ,
                                                        LastUpdatedDate ,
                                                        Guid ,
                                                        site_id   ,
                                                        MasterEntityId 
                                                      ) 
                                                values 
                                                        (                                                        
                                                       @TableName,
                                                       @Description,
                                                       @IsActive ,
                                                       @CreatedBy , 
                                                       GetDate(),
                                                       @LastUpdatedBy,
                                                       GetDate(),
                                                       NewId(),
                                                       @SiteId,
                                                       @MasterEntityId 
                                          )SELECT  * from AttributeEnabledTables where AttributeEnabledTableId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, BuildSQLParameters(attributeEnabledTablesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAttributeEnabledTablesDTO(attributeEnabledTablesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting AttributeEnabledTablesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(attributeEnabledTablesDTO);
            return attributeEnabledTablesDTO;
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="attributeEnabledTablesDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public AttributeEnabledTablesDTO Update(AttributeEnabledTablesDTO attributeEnabledTablesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(attributeEnabledTablesDTO, loginId, siteId);
            string updateQuery = @"update AttributeEnabledTables 
                                         set 
                                            TableName =@TableName,  
	                                        Description =@Description, 
                                            IsActive = @IsActive,
                                            LastUpdatedBy = @LastUpdatedBy,
                                            LastUpdatedDate = GetDate()
                                               where   AttributeEnabledTableId =  @AttributeEnabledTableId  
                                        SELECT  * from AttributeEnabledTables where AttributeEnabledTableId = @AttributeEnabledTableId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(attributeEnabledTablesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAttributeEnabledTablesDTO(attributeEnabledTablesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating AttributeEnabledTablesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(attributeEnabledTablesDTO);
            return attributeEnabledTablesDTO;
        }

        private void RefreshAttributeEnabledTablesDTO(AttributeEnabledTablesDTO attributeEnabledTablesDTO, DataTable dt)
        {
            log.LogMethodEntry(attributeEnabledTablesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                attributeEnabledTablesDTO.AttributeEnabledTableId = Convert.ToInt32(dt.Rows[0]["AttributeEnabledTableId"]);
                attributeEnabledTablesDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                attributeEnabledTablesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                attributeEnabledTablesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                attributeEnabledTablesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                attributeEnabledTablesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                attributeEnabledTablesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
        private AttributeEnabledTablesDTO GetAttributeEnabledTablesDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AttributeEnabledTablesDTO attributeEnabledTablesDTO = new AttributeEnabledTablesDTO(Convert.ToInt32(dataRow["AttributeEnabledTableId"]),
                                                    dataRow["TableName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TableName"]),
                                                    dataRow["Description"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Description"]),
                                                    dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                    dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                    dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                    dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                    dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                    dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                                    );
            log.LogMethodExit(attributeEnabledTablesDTO);
            return attributeEnabledTablesDTO;
        }
        internal AttributeEnabledTablesDTO GetAttributeEnabledTablesDTO(int id)
        {
            log.LogMethodEntry(id);
            AttributeEnabledTablesDTO attributeEnabledTablesDTO = null;
            string selectUserQuery = SELECT_QUERY + "   where aet.AttributeEnabledTableId = @AttributeEnabledTableId";
            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@AttributeEnabledTableId", id);
            DataTable deliveryChannelIdTable = dataAccessHandler.executeSelectQuery(selectUserQuery, selectParameters, sqlTransaction);
            if (deliveryChannelIdTable.Rows.Count > 0)
            {
                DataRow dataRow = deliveryChannelIdTable.Rows[0];
                attributeEnabledTablesDTO = GetAttributeEnabledTablesDTO(dataRow);
            }
            log.LogMethodExit(attributeEnabledTablesDTO);
            return attributeEnabledTablesDTO;

        }
        internal List<AttributeEnabledTablesDTO> GetAttributeEnabledTablesDTOList(List<int> attributeEnabledTablesDTOIdList, bool activeChildRecords)
        {
            log.LogMethodEntry(attributeEnabledTablesDTOIdList, activeChildRecords);
            List<AttributeEnabledTablesDTO> attributeEnabledTablesDTOList = new List<AttributeEnabledTablesDTO>();
            string query = SELECT_QUERY + @" , @attributeEnabledTablesDTOIdList List
                            WHERE AttributeEnabledTableId = List.Id ";
            if (activeChildRecords)
            {
                query += " AND isActive = '1' ";
            }
           
            DataTable table = dataAccessHandler.BatchSelect(query, "@attributeEnabledTablesDTOIdList", attributeEnabledTablesDTOIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                attributeEnabledTablesDTOList = table.Rows.Cast<DataRow>().Select(x => GetAttributeEnabledTablesDTO(x)).ToList();
            }
            log.LogMethodExit(attributeEnabledTablesDTOList);
            return attributeEnabledTablesDTOList;
        }
        public List<AttributeEnabledTablesDTO> GetAttributeEnabledTablesDTOList(List<KeyValuePair<AttributeEnabledTablesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<AttributeEnabledTablesDTO> attributeEnabledTablesDTOList = null;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AttributeEnabledTablesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AttributeEnabledTablesDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AttributeEnabledTablesDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key.Equals(AttributeEnabledTablesDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                                  searchParameter.Key.Equals(AttributeEnabledTablesDTO.SearchByParameters.ATTRIBUTE_ENABLED_TABLE_ID) )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        //else if (searchParameter.Key == AttributeEnabledTablesDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE)
                        //{
                        //    query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                        //    parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        //}
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        counter++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }

                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
            }
           
            DataTable data = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (data.Rows.Count > 0)
            {
                attributeEnabledTablesDTOList = new List<AttributeEnabledTablesDTO>();
                foreach (DataRow dataRow in data.Rows)
                {
                    AttributeEnabledTablesDTO attributeEnabledTablesDTO = GetAttributeEnabledTablesDTO(dataRow);
                    attributeEnabledTablesDTOList.Add(attributeEnabledTablesDTO);
                }
            }
            log.LogMethodExit(attributeEnabledTablesDTOList);
            return attributeEnabledTablesDTOList;
        }

        internal DateTime? GetAttributeEnabledTablesLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"select max(LastUpdatedDate) LastUpdatedDate from AttributeEnabledTables WHERE (site_id = @siteId or @siteId = -1)";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
