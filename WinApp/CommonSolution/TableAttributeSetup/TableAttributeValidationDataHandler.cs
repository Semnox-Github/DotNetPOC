/********************************************************************************************
 * Project Name - TableAttributeSetup
 * Description  - TableAttributeValidationDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.140.0     23-Aug-2021    Fiona         Created
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
    public class TableAttributeValidationDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM TableAttributeValidation AS tav ";

        private static readonly Dictionary<TableAttributeValidationDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TableAttributeValidationDTO.SearchByParameters, string>
            {
                {TableAttributeValidationDTO.SearchByParameters.ATTRIBUTE_ENABLED_VALIDATION_ID, "tav.TableAttributeValidationId"},
                {TableAttributeValidationDTO.SearchByParameters.IS_ACTIVE, "tav.IsActive"},
                {TableAttributeValidationDTO.SearchByParameters.MASTER_ENTITY_ID, "tav.MasterEntityId"},
                {TableAttributeValidationDTO.SearchByParameters.SITE_ID, "tav.site_id"}
            };
        public TableAttributeValidationDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
        private List<SqlParameter> BuildSQLParameters(TableAttributeValidationDTO tableAttributeValidationDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(tableAttributeValidationDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@TableAttributeValidationId", tableAttributeValidationDTO.TableAttributeValidationId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TableAttributeSetupId", tableAttributeValidationDTO.TableAttributeSetupId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DataValidationRule", tableAttributeValidationDTO.DataValidationRule));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", tableAttributeValidationDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", tableAttributeValidationDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }
        public TableAttributeValidationDTO Insert(TableAttributeValidationDTO tableAttributeValidationDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(tableAttributeValidationDTO, loginId, siteId);
            string insertQuery = @"insert into TableAttributeValidation 
                                                     (                                                         
	                                                    TableAttributeSetupId ,  
	                                                    DataValidationRule , 
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
                                                       @TableAttributeSetupId,
                                                       @DataValidationRule,
                                                       @IsActive ,
                                                       @CreatedBy , 
                                                       GetDate(),
                                                       @LastUpdatedBy,
                                                       GetDate(),
                                                       NewId(),
                                                       @SiteId,
                                                       @MasterEntityId 
                                          )SELECT  * from TableAttributeValidation where TableAttributeValidationId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, BuildSQLParameters(tableAttributeValidationDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTableAttributeValidationDTO(tableAttributeValidationDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting TableAttributeValidationDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(tableAttributeValidationDTO);
            return tableAttributeValidationDTO;
        }
        private void RefreshTableAttributeValidationDTO(TableAttributeValidationDTO tableAttributeValidationDTO, DataTable dt)
        {
            log.LogMethodEntry(tableAttributeValidationDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                tableAttributeValidationDTO.TableAttributeValidationId = Convert.ToInt32(dt.Rows[0]["TableAttributeValidationId"]);
                tableAttributeValidationDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                tableAttributeValidationDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                tableAttributeValidationDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                tableAttributeValidationDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                tableAttributeValidationDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                tableAttributeValidationDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
        public TableAttributeValidationDTO Update(TableAttributeValidationDTO TableAttributeValidationDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(TableAttributeValidationDTO, loginId, siteId);
            string updateQuery = @"update TableAttributeValidation 
                                         set 
                                            TableAttributeSetupId =@TableAttributeSetupId,  
	                                        DataValidationRule =@DataValidationRule, 
                                            IsActive = @IsActive,
                                            LastUpdatedBy = @LastUpdatedBy,
                                            LastUpdatedDate = GetDate()
                                               where   TableAttributeValidationId =  @TableAttributeValidationId  
                                        SELECT  * from TableAttributeValidation where TableAttributeValidationId = @TableAttributeValidationId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(TableAttributeValidationDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTableAttributeValidationDTO(TableAttributeValidationDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating TableAttributeValidationDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(TableAttributeValidationDTO);
            return TableAttributeValidationDTO;
        }
        internal TableAttributeValidationDTO GetTableAttributeValidationDTO(int id)
        {
            log.LogMethodEntry(id);
            TableAttributeValidationDTO tableAttributeValidationDTO = null;
            string selectUserQuery = SELECT_QUERY + "   where tav.TableAttributeValidationId = @TableAttributeValidationId";
            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@TableAttributeValidationId", id);
            DataTable deliveryChannelIdTable = dataAccessHandler.executeSelectQuery(selectUserQuery, selectParameters, sqlTransaction);
            if (deliveryChannelIdTable.Rows.Count > 0)
            {
                DataRow dataRow = deliveryChannelIdTable.Rows[0];
                tableAttributeValidationDTO = GetTableAttributeValidationDTO(dataRow);
            }
            log.LogMethodExit(tableAttributeValidationDTO);
            return tableAttributeValidationDTO;

        }
        private TableAttributeValidationDTO GetTableAttributeValidationDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TableAttributeValidationDTO TableAttributeValidationDTO = new TableAttributeValidationDTO(Convert.ToInt32(dataRow["TableAttributeValidationId"]),
                                                    dataRow["TableAttributeSetupId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TableAttributeSetupId"]),
                                                    dataRow["DataValidationRule"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["DataValidationRule"]),
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
            log.LogMethodExit(TableAttributeValidationDTO);
            return TableAttributeValidationDTO;
        }
        internal List<TableAttributeValidationDTO> GetTableAttributeValidationDTOList(List<int> TableAttributeSetupIdList, bool activeChildRecords)
        {
            log.LogMethodEntry(TableAttributeSetupIdList, activeChildRecords);
            List<TableAttributeValidationDTO> TableAttributeValidationDTOList = new List<TableAttributeValidationDTO>();
            string query = SELECT_QUERY + @" , @TableAttributeSetupIdList List
                            WHERE TableAttributeSetupId = List.Id ";
            if (activeChildRecords)
            {
                query += " AND isActive = '1' ";
            }

            DataTable table = dataAccessHandler.BatchSelect(query, "@TableAttributeSetupIdList", TableAttributeSetupIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                TableAttributeValidationDTOList = table.Rows.Cast<DataRow>().Select(x => GetTableAttributeValidationDTO(x)).ToList();
            }
            log.LogMethodExit(TableAttributeValidationDTOList);
            return TableAttributeValidationDTOList;
        }
        public List<TableAttributeValidationDTO> GetTableAttributeValidationDTOList(List<KeyValuePair<TableAttributeValidationDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<TableAttributeValidationDTO> tableAttributeValidationDTOList = null;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<TableAttributeValidationDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == TableAttributeValidationDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TableAttributeValidationDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key.Equals(TableAttributeValidationDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                                  searchParameter.Key.Equals(TableAttributeValidationDTO.SearchByParameters.ATTRIBUTE_ENABLED_VALIDATION_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        //else if (searchParameter.Key == TableAttributeValidationDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE)
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
                tableAttributeValidationDTOList = new List<TableAttributeValidationDTO>();
                foreach (DataRow dataRow in data.Rows)
                {
                    TableAttributeValidationDTO tableAttributeValidationDTO = GetTableAttributeValidationDTO(dataRow);
                    tableAttributeValidationDTOList.Add(tableAttributeValidationDTO);
                }
            }
            log.LogMethodExit(tableAttributeValidationDTOList);
            return tableAttributeValidationDTOList;
        }
    }
}
