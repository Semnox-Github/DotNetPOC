/********************************************************************************************
 * Project Name - TableAttributeSetup
 * Description  - TableAttributeSetupDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.140.0      23-Aug-2021    Fiona         Created
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
    public class TableAttributeSetupDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM TableAttributeSetup AS tas";
        private static readonly Dictionary<TableAttributeSetupDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TableAttributeSetupDTO.SearchByParameters, string>
            {
                {TableAttributeSetupDTO.SearchByParameters.TABLE_ATTRIBUTE_SETUP_ID, "tas.TableAttributeSetupId"},
                {TableAttributeSetupDTO.SearchByParameters.IS_ACTIVE, "tas.IsActive"},
                {TableAttributeSetupDTO.SearchByParameters.MASTER_ENTITY_ID, "tas.MasterEntityId"},
                {TableAttributeSetupDTO.SearchByParameters.SITE_ID, "tas.site_id"}
            };
        public TableAttributeSetupDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
        private List<SqlParameter> BuildSQLParameters(TableAttributeSetupDTO tableAttributeSetupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(tableAttributeSetupDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@TableAttributeSetupId", tableAttributeSetupDTO.TableAttributeSetupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AttributeEnabledTableId", tableAttributeSetupDTO.AttributeEnabledTableId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DisplayName", tableAttributeSetupDTO.DisplayName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ColumnName", tableAttributeSetupDTO.ColumnName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DataSourceType", TableAttributeSetupDTO.DataSourceTypeToString(tableAttributeSetupDTO.DataSourceType)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DataType", TableAttributeSetupDTO.DataTypeToString(tableAttributeSetupDTO.DataType)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LookupId", tableAttributeSetupDTO.LookupId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SQLSource", tableAttributeSetupDTO.SQLSource));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SQLDisplayMember", tableAttributeSetupDTO.SQLDisplayMember));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SQLValueMember", tableAttributeSetupDTO.SQLValueMember));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", tableAttributeSetupDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", tableAttributeSetupDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }
        public TableAttributeSetupDTO Insert(TableAttributeSetupDTO tableAttributeSetupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(tableAttributeSetupDTO, loginId, siteId);
            string insertQuery = @"insert into TableAttributeSetup 
                                                     (                                                         
	                                                    AttributeEnabledTableId ,  
	                                                    ColumnName ,
                                                        DisplayName ,  
	                                                    DataSourceType , 
                                                        DataType ,  
	                                                    LookupId ,
                                                        SQLSource ,  
	                                                    SQLDisplayMember ,
                                                        SQLValueMember , 
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
                                                      @AttributeEnabledTableId ,  
	                                                   @ColumnName ,
                                                       @DisplayName ,  
	                                                   @DataSourceType , 
                                                       @DataType ,  
	                                                   @LookupId ,
                                                       @SQLSource ,  
	                                                   @SQLDisplayMember ,
                                                       @SQLValueMember , 
                                                       @IsActive ,
                                                       @CreatedBy , 
                                                       GetDate(),
                                                       @LastUpdatedBy,
                                                       GetDate(),
                                                       NewId(),
                                                       @SiteId,
                                                       @MasterEntityId 
                                          )SELECT  * from TableAttributeSetup where TableAttributeSetupId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, BuildSQLParameters(tableAttributeSetupDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTableAttributeSetupDTO(tableAttributeSetupDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting TableAttributeSetupDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(tableAttributeSetupDTO);
            return tableAttributeSetupDTO;
        }
        private void RefreshTableAttributeSetupDTO(TableAttributeSetupDTO tableAttributeSetupDTO, DataTable dt)
        {
            log.LogMethodEntry(tableAttributeSetupDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                tableAttributeSetupDTO.TableAttributeSetupId = Convert.ToInt32(dt.Rows[0]["TableAttributeSetupId"]);
                tableAttributeSetupDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                tableAttributeSetupDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                tableAttributeSetupDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                tableAttributeSetupDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                tableAttributeSetupDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                tableAttributeSetupDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
        public TableAttributeSetupDTO Update(TableAttributeSetupDTO tableAttributeSetupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(tableAttributeSetupDTO, loginId, siteId);
            string updateQuery = @"update TableAttributeSetup 
                                         set 
                                            AttributeEnabledTableId=@AttributeEnabledTableId ,  
	                                                    ColumnName=@ColumnName ,
                                                        DisplayName=@DisplayName ,  
	                                                    DataSourceType=@DataSourceType , 
                                                        DataType=@DataType ,  
	                                                    LookupId=@LookupId ,
                                                        SQLSource=@SQLSource ,  
	                                                    SQLDisplayMember=@SQLDisplayMember ,
                                                        SQLValueMember=@SQLValueMember , 
                                            IsActive = @IsActive,
                                            LastUpdatedBy = @LastUpdatedBy,
                                            LastUpdatedDate = GetDate()
                                               where   TableAttributeSetupId =  @TableAttributeSetupId  
                                        SELECT  * from TableAttributeSetup where TableAttributeSetupId = @TableAttributeSetupId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(tableAttributeSetupDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTableAttributeSetupDTO(tableAttributeSetupDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating TableAttributeSetupDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(tableAttributeSetupDTO);
            return tableAttributeSetupDTO;
        }
        internal TableAttributeSetupDTO GetTableAttributeSetupDTO(int id)
        {
            log.LogMethodEntry(id);
            TableAttributeSetupDTO TableAttributeSetupDTO = null;
            string selectUserQuery = SELECT_QUERY + "   where tas.TableAttributeSetupId = @TableAttributeSetupId";
            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@TableAttributeSetupId", id);
            DataTable deliveryChannelIdTable = dataAccessHandler.executeSelectQuery(selectUserQuery, selectParameters, sqlTransaction);
            if (deliveryChannelIdTable.Rows.Count > 0)
            {
                DataRow dataRow = deliveryChannelIdTable.Rows[0];
                TableAttributeSetupDTO = GetTableAttributeSetupDTO(dataRow);
            }
            log.LogMethodExit(TableAttributeSetupDTO);
            return TableAttributeSetupDTO;

        }
        private TableAttributeSetupDTO GetTableAttributeSetupDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TableAttributeSetupDTO tableAttributeSetupDTO = new TableAttributeSetupDTO(Convert.ToInt32(dataRow["TableAttributeSetupId"]),
                                                    dataRow["AttributeEnabledTableId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AttributeEnabledTableId"]),
                                                    dataRow["ColumnName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ColumnName"]),
                                                    dataRow["DisplayName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["DisplayName"]),
                                                    dataRow["DataSourceType"] == DBNull.Value ? TableAttributeSetupDTO.DataSourceTypeEnum.NONE
                                                                            : TableAttributeSetupDTO.DataSourceTypeFromString(Convert.ToString(dataRow["DataSourceType"])),
                                                    dataRow["DataType"] == DBNull.Value ? TableAttributeSetupDTO.DataTypeEnum.NONE
                                                                               : TableAttributeSetupDTO.DataTypeFromString(Convert.ToString(dataRow["DataType"])),
                                                    dataRow["LookupId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LookupId"]),
                                                    dataRow["SQLSource"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["SQLSource"]),
                                                    dataRow["SQLDisplayMember"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["SQLDisplayMember"]),
                                                    dataRow["SQLValueMember"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["SQLValueMember"]),
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
            log.LogMethodExit(tableAttributeSetupDTO);
            return tableAttributeSetupDTO;
        }
        internal List<TableAttributeSetupDTO> GetTableAttributeSetupDTOList(List<int> attributeEnabledTableIdList, bool activeChildRecords)
        {
            log.LogMethodEntry(attributeEnabledTableIdList, activeChildRecords);
            List<TableAttributeSetupDTO> tableAttributeSetupDTOList = new List<TableAttributeSetupDTO>();
            string query = SELECT_QUERY + @" , @attributeEnabledTableIdList List
                            WHERE AttributeEnabledTableId = List.Id ";
            if (activeChildRecords)
            {
                query += " AND isActive = '1' ";
            }

            DataTable table = dataAccessHandler.BatchSelect(query, "@attributeEnabledTableIdList", attributeEnabledTableIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                tableAttributeSetupDTOList = table.Rows.Cast<DataRow>().Select(x => GetTableAttributeSetupDTO(x)).ToList();
            }
            log.LogMethodExit(tableAttributeSetupDTOList);
            return tableAttributeSetupDTOList;
        }
        public List<TableAttributeSetupDTO> GetTableAttributeSetupDTOList(List<KeyValuePair<TableAttributeSetupDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<TableAttributeSetupDTO> tableAttributeSetupDTOList = null;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<TableAttributeSetupDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == TableAttributeSetupDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TableAttributeSetupDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key.Equals(TableAttributeSetupDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                                  searchParameter.Key.Equals(TableAttributeSetupDTO.SearchByParameters.TABLE_ATTRIBUTE_SETUP_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        //else if (searchParameter.Key == TableAttributeSetupDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE)
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
                tableAttributeSetupDTOList = new List<TableAttributeSetupDTO>();
                foreach (DataRow dataRow in data.Rows)
                {
                    TableAttributeSetupDTO TableAttributeSetupDTO = GetTableAttributeSetupDTO(dataRow);
                    tableAttributeSetupDTOList.Add(TableAttributeSetupDTO);
                }
            }
            log.LogMethodExit(tableAttributeSetupDTOList);
            return tableAttributeSetupDTOList;
        }

    }
}
