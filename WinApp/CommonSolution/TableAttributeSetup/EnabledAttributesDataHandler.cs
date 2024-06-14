/********************************************************************************************
 * Project Name - TableAttributeSetup                                                                       
 * Description  - EnabledAttibutesDataHandler
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.140.0     16-Aug-2021     Fiona             Created
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
    public class EnabledAttributesDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM EnabledAttibutes AS ea ";
        private static readonly Dictionary<EnabledAttributesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<EnabledAttributesDTO.SearchByParameters, string>
            {
                {EnabledAttributesDTO.SearchByParameters.ENABLED_ATTRIBUTE_ID, "ea.EnabledAttibuteId"},
                {EnabledAttributesDTO.SearchByParameters.IS_ACTIVE, "ea.IsActive"},
                {EnabledAttributesDTO.SearchByParameters.MASTER_ENTITY_ID, "ea.MasterEntityId"},
                {EnabledAttributesDTO.SearchByParameters.SITE_ID, "ea.site_id"},
                {EnabledAttributesDTO.SearchByParameters.TABLE_NAME, "ea.TableName"},
                {EnabledAttributesDTO.SearchByParameters.RECORD_GUID, "ea.RecordGuid"},
                {EnabledAttributesDTO.SearchByParameters.ENABLED_ATTRIBUTE_NAME, "ea.EnabledAttributeName"},
                {EnabledAttributesDTO.SearchByParameters.MANDATORY_OR_OPTIONAL, "ea.MandatoryOrOptional"}
            };
        /// <summary>
        /// Default constructor of EnabledAttibutesDataHandler class
        /// </summary>
        public EnabledAttributesDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
        private List<SqlParameter> BuildSQLParameters(EnabledAttributesDTO enabledAttibutesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(enabledAttibutesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@EnabledAttibuteId", enabledAttibutesDTO.EnabledAttibuteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TableName", enabledAttibutesDTO.TableName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RecordGuid", enabledAttibutesDTO.RecordGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EnabledAttributeName", enabledAttibutesDTO.EnabledAttributeName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DefaultValue", enabledAttibutesDTO.DefaultValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MandatoryOrOptional", EnabledAttributesDTO.IsMandatoryOrOptionalToString(enabledAttibutesDTO.MandatoryOrOptional)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", enabledAttibutesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", enabledAttibutesDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }
        public EnabledAttributesDTO Insert(EnabledAttributesDTO enabledAttibutesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(enabledAttibutesDTO, loginId, siteId);
            string insertQuery = @"insert into EnabledAttibutes 
                                                     (                                                         
	                                                    TableName , 
                                                        RecordGuid ,   
	                                                    EnabledAttributeName ,
	                                                    MandatoryOrOptional ,
                                                        DefaultValue, 
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
                                                       @RecordGuid,
                                                       @EnabledAttributeName,
                                                       @MandatoryOrOptional,
                                                       @DefaultValue,
                                                       @IsActive ,
                                                       @CreatedBy , 
                                                       GetDate(),
                                                       @LastUpdatedBy,
                                                       GetDate(),
                                                       NewId(),
                                                       @SiteId,
                                                       @MasterEntityId 
                                          )SELECT  * from EnabledAttibutes where EnabledAttibuteId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, BuildSQLParameters(enabledAttibutesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTransctionOrderDispensingDTO(enabledAttibutesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting EnabledAttibutesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(enabledAttibutesDTO);
            return enabledAttibutesDTO;
        }
        public EnabledAttributesDTO Update(EnabledAttributesDTO enabledAttibutesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(enabledAttibutesDTO, loginId, siteId);
            string updateQuery = @"update EnabledAttibutes 
                                         set 
	                                        TableName = @TableName, 
                                            RecordGuid = @RecordGuid,   
	                                        EnabledAttributeName = @EnabledAttributeName,
	                                        MandatoryOrOptional = @MandatoryOrOptional,
                                            DefaultValue = @DefaultValue,
                                            IsActive = @IsActive,
                                            LastUpdatedBy = @LastUpdatedBy,
                                            LastUpdatedDate = GetDate(),
                                            MasterEntityId =  @MasterEntityId 
                                               where   EnabledAttibuteId =  @EnabledAttibuteId  
                                        SELECT  * from EnabledAttibutes where EnabledAttibuteId = @EnabledAttibuteId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(enabledAttibutesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTransctionOrderDispensingDTO(enabledAttibutesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating EnabledAttibutesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(enabledAttibutesDTO);
            return enabledAttibutesDTO;
        }

        private void RefreshTransctionOrderDispensingDTO(EnabledAttributesDTO enabledAttibutesDTO, DataTable dt)
        {
            log.LogMethodEntry(enabledAttibutesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                enabledAttibutesDTO.EnabledAttibuteId = Convert.ToInt32(dt.Rows[0]["EnabledAttibuteId"]);
                enabledAttibutesDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                enabledAttibutesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                enabledAttibutesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                enabledAttibutesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                enabledAttibutesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                enabledAttibutesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        private EnabledAttributesDTO GetEnabledAttibutesDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            EnabledAttributesDTO enabledAttibutesDTO = new EnabledAttributesDTO(Convert.ToInt32(dataRow["EnabledAttibuteId"]),
                                                    dataRow["TableName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TableName"]),
                                                    dataRow["RecordGuid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["RecordGuid"]),
                                                    dataRow["EnabledAttributeName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["EnabledAttributeName"]),
                                                    dataRow["MandatoryOrOptional"] == DBNull.Value ? EnabledAttributesDTO.IsMandatoryOrOptional.Optional : EnabledAttributesDTO.IsMandatoryOrOptionalFromString(Convert.ToString(dataRow["MandatoryOrOptional"])),
                                                    dataRow["DefaultValue"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["DefaultValue"]),
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
            log.LogMethodExit(enabledAttibutesDTO);
            return enabledAttibutesDTO;
        }

        internal DateTime? GetEnabledAttributesModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"select max(LastUpdatedDate) LastUpdatedDate from EnabledAttibutes WHERE (site_id = @siteId or @siteId = -1)";
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

        internal EnabledAttributesDTO GetEnabledAttibutesDTO(int id)
        {
            log.LogMethodEntry(id);
            EnabledAttributesDTO enabledAttibutesDTO = null;
            string selectUserQuery = SELECT_QUERY + "   where ea.EnabledAttibuteId = @EnabledAttibuteId";
            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@EnabledAttibuteId", id);
            DataTable table = dataAccessHandler.executeSelectQuery(selectUserQuery, selectParameters, sqlTransaction);
            if (table.Rows.Count > 0)
            {
                DataRow dataRow = table.Rows[0];
                enabledAttibutesDTO = GetEnabledAttibutesDTO(dataRow);
            }
            log.LogMethodExit(enabledAttibutesDTO);
            return enabledAttibutesDTO;
        }
        internal List<EnabledAttributesDTO> GetEnabledAttibutesDTOList(List<int> enabledAttibutesDTOIdList, bool activeChildRecords)
        {
            log.LogMethodEntry(enabledAttibutesDTOIdList, activeChildRecords);
            List<EnabledAttributesDTO> enabledAttibutesDTOList = new List<EnabledAttributesDTO>();
            string query = SELECT_QUERY + @" , @enabledAttibutesDTOIdList List
                            WHERE EnabledAttibuteId = List.Id ";
            if (activeChildRecords)
            {
                query += " AND isActive = '1' ";
            }

            DataTable table = dataAccessHandler.BatchSelect(query, "@enabledAttibutesDTOIdList", enabledAttibutesDTOIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                enabledAttibutesDTOList = table.Rows.Cast<DataRow>().Select(x => GetEnabledAttibutesDTO(x)).ToList();
            }
            log.LogMethodExit(enabledAttibutesDTOList);
            return enabledAttibutesDTOList;
        }
        public List<EnabledAttributesDTO> GetEnabledAttibutesDTOList(List<KeyValuePair<EnabledAttributesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<EnabledAttributesDTO> enabledAttibutesDTOList = null;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<EnabledAttributesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == EnabledAttributesDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == EnabledAttributesDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key.Equals(EnabledAttributesDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                                  searchParameter.Key.Equals(EnabledAttributesDTO.SearchByParameters.ENABLED_ATTRIBUTE_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        //else if (searchParameter.Key == EnabledAttibutesDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE)
                        //{
                        //    query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                        //    parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        //}
                        else if (searchParameter.Key == EnabledAttributesDTO.SearchByParameters.TABLE_NAME
                            || searchParameter.Key == EnabledAttributesDTO.SearchByParameters.ENABLED_ATTRIBUTE_NAME
                            || searchParameter.Key == EnabledAttributesDTO.SearchByParameters.MANDATORY_OR_OPTIONAL)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(EnabledAttributesDTO.SearchByParameters.RECORD_GUID))
                        {
                            query.Append(joiner + "CONVERT(varchar(200), " + DBSearchParameters[searchParameter.Key] + ") = '" + searchParameter.Value + "' ");
                        }
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
                enabledAttibutesDTOList = new List<EnabledAttributesDTO>();
                foreach (DataRow dataRow in data.Rows)
                {
                    EnabledAttributesDTO enabledAttibutesDTO = GetEnabledAttibutesDTO(dataRow);
                    enabledAttibutesDTOList.Add(enabledAttibutesDTO);
                }
            }
            log.LogMethodExit(enabledAttibutesDTOList);
            return enabledAttibutesDTOList;
        }

    }

}

