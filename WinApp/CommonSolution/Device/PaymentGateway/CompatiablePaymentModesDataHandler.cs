/********************************************************************************************
 * Project Name - Device
 * Description  - CompatiablePaymentModesDataHandler
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

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class CompatiablePaymentModesDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CompatiablePaymentModes AS cpm ";
        private static readonly Dictionary<CompatiablePaymentModesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CompatiablePaymentModesDTO.SearchByParameters, string>
            {
                {CompatiablePaymentModesDTO.SearchByParameters.ID, "cpm.Id"},
                {CompatiablePaymentModesDTO.SearchByParameters.IS_ACTIVE, "cpm.IsActive"},
                {CompatiablePaymentModesDTO.SearchByParameters.MASTER_ENTITY_ID, "cpm.MasterEntityId"},
                {CompatiablePaymentModesDTO.SearchByParameters.SITE_ID, "cpm.site_id"},
                {CompatiablePaymentModesDTO.SearchByParameters.PAYMENT_MODE_ID, "cpm.PaymentModeId"}
            };
        /// <summary>
        /// Default constructor of CompatiablePaymentModesDataHandler class
        /// </summary>
        internal CompatiablePaymentModesDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
        private List<SqlParameter> BuildSQLParameters(CompatiablePaymentModesDTO CompatiablePaymentModesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(CompatiablePaymentModesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", CompatiablePaymentModesDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PaymentModeId", CompatiablePaymentModesDTO.PaymentModeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CompatiablePaymentModeId", CompatiablePaymentModesDTO.CompatiablePaymentModeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", CompatiablePaymentModesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", CompatiablePaymentModesDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }
        internal CompatiablePaymentModesDTO Insert(CompatiablePaymentModesDTO compatiablePaymentModesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(compatiablePaymentModesDTO, loginId, siteId);
            string insertQuery = @"insert into CompatiablePaymentModes 
                                                     (                                                         
	                                                    PaymentModeId ,  
	                                                    CompatiablePaymentModeId , 
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
                                                       @PaymentModeId,
                                                       @CompatiablePaymentModeId,
                                                       @IsActive ,
                                                       @CreatedBy , 
                                                       GetDate(),
                                                       @LastUpdatedBy,
                                                       GetDate(),
                                                       NewId(),
                                                       @SiteId,
                                                       @MasterEntityId 
                                          )SELECT  * from CompatiablePaymentModes where Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, BuildSQLParameters(compatiablePaymentModesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCompatiablePaymentModesDTO(compatiablePaymentModesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting CompatiablePaymentModesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(compatiablePaymentModesDTO);
            return compatiablePaymentModesDTO;
        }
        internal CompatiablePaymentModesDTO Update(CompatiablePaymentModesDTO compatiablePaymentModesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(compatiablePaymentModesDTO, loginId, siteId);
            string updateQuery = @"update CompatiablePaymentModes 
                                         set 
                                            PaymentModeId = @PaymentModeId,  
	                                        CompatiablePaymentModeId = @CompatiablePaymentModeId,
                                            IsActive = @IsActive,
                                            LastUpdatedBy = @LastUpdatedBy,
                                            LastUpdatedDate = GetDate()
                                               where   Id =  @Id  
                                        SELECT  * from CompatiablePaymentModes where Id = @Id ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(compatiablePaymentModesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCompatiablePaymentModesDTO(compatiablePaymentModesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating CompatiablePaymentModesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(compatiablePaymentModesDTO);
            return compatiablePaymentModesDTO;
        }
        private void RefreshCompatiablePaymentModesDTO(CompatiablePaymentModesDTO compatiablePaymentModesDTO, DataTable dt)
        {
            log.LogMethodEntry();
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                compatiablePaymentModesDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                compatiablePaymentModesDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                compatiablePaymentModesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                compatiablePaymentModesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                compatiablePaymentModesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                compatiablePaymentModesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                compatiablePaymentModesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
        private CompatiablePaymentModesDTO GetCompatiablePaymentModesDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CompatiablePaymentModesDTO compatiablePaymentModesDTO = new CompatiablePaymentModesDTO(Convert.ToInt32(dataRow["Id"]),
                                                    dataRow["PaymentModeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentModeId"]),
                                                    dataRow["CompatiablePaymentModeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CompatiablePaymentModeId"]),
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
            log.LogMethodExit(compatiablePaymentModesDTO);
            return compatiablePaymentModesDTO;
        }
        internal CompatiablePaymentModesDTO GetCompatiablePaymentModesDTO(int id)
        {
            log.LogMethodEntry(id);
            CompatiablePaymentModesDTO compatiablePaymentModesDTO = null;
            string selectUserQuery = SELECT_QUERY + "   where cpm.Id = @Id";
            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@Id", id);
            DataTable deliveryChannelIdTable = dataAccessHandler.executeSelectQuery(selectUserQuery, selectParameters, sqlTransaction);
            if (deliveryChannelIdTable.Rows.Count > 0)
            {
                DataRow dataRow = deliveryChannelIdTable.Rows[0];
                compatiablePaymentModesDTO = GetCompatiablePaymentModesDTO(dataRow);
            }
            log.LogMethodExit(compatiablePaymentModesDTO);
            return compatiablePaymentModesDTO;

        }
        internal List<CompatiablePaymentModesDTO> GetCompatiablePaymentModesDTOList(List<int> compatiablePaymentModesDTOIdList, bool activeChildRecords)
        {
            log.LogMethodEntry(compatiablePaymentModesDTOIdList, activeChildRecords);
            List<CompatiablePaymentModesDTO> compatiablePaymentModesDTOList = new List<CompatiablePaymentModesDTO>();
            string query = SELECT_QUERY + @" , @compatiablePaymentModesDTOIdList List
                            WHERE (PaymentModeId = List.Id OR CompatiablePaymentModeId = List.Id)";
            if (activeChildRecords)
            {
                query += " AND isActive = '1' ";
            }

            DataTable table = dataAccessHandler.BatchSelect(query, "@compatiablePaymentModesDTOIdList", compatiablePaymentModesDTOIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                compatiablePaymentModesDTOList = table.Rows.Cast<DataRow>().Select(x => GetCompatiablePaymentModesDTO(x)).ToList();
            }
            log.LogMethodExit(compatiablePaymentModesDTOList);
            return compatiablePaymentModesDTOList;
        }
        internal List<CompatiablePaymentModesDTO> GetCompatiablePaymentModesDTOList(List<KeyValuePair<CompatiablePaymentModesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<CompatiablePaymentModesDTO> compatiablePaymentModesDTOList = null;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CompatiablePaymentModesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CompatiablePaymentModesDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CompatiablePaymentModesDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key.Equals(CompatiablePaymentModesDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                                  searchParameter.Key.Equals(CompatiablePaymentModesDTO.SearchByParameters.ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(CompatiablePaymentModesDTO.SearchByParameters.PAYMENT_MODE_ID))
                        {
                            query.Append(joiner + " ( cpm.PaymentModeId = " + dataAccessHandler.GetParameterName(searchParameter.Key)
                                                  + " OR cpm.CompatiablePaymentModeId = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " ) ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        //else if (searchParameter.Key == CompatiablePaymentModesDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE)
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
                compatiablePaymentModesDTOList = new List<CompatiablePaymentModesDTO>();
                foreach (DataRow dataRow in data.Rows)
                {
                    CompatiablePaymentModesDTO compatiablePaymentModesDTO = GetCompatiablePaymentModesDTO(dataRow);
                    compatiablePaymentModesDTOList.Add(compatiablePaymentModesDTO);
                }
            }
            log.LogMethodExit(compatiablePaymentModesDTOList);
            return compatiablePaymentModesDTOList;
        }
    }
}
