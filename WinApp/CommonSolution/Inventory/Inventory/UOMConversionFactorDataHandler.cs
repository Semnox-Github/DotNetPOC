/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data Handler object of UOM Conversion Factor
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0       24-Jul-2020   Deeksha             Created for Recipe Management enhancement.
 *2.120.0     11-May-2021   Mushahid Faizan Modified for Web Inventory 
 *********************************************************************************************/
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// UOM Conversion Factor DataHandler
    /// </summary>
    public class UOMConversionFactorDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM UOMConversionFactor AS ucf ";

        private static readonly Dictionary<UOMConversionFactorDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<UOMConversionFactorDTO.SearchByParameters, string>
            {
                {UOMConversionFactorDTO.SearchByParameters.UOM_CONVERSION_FACTOR_ID, "ucf.UOMConversionFactorId"},
                {UOMConversionFactorDTO.SearchByParameters.UOM_ID, "ucf.UOMId"},
                {UOMConversionFactorDTO.SearchByParameters.BASE_UOM_ID, "ucf.BaseUOMId"},
                {UOMConversionFactorDTO.SearchByParameters.IS_ACTIVE, "ucf.IsActive"},
                {UOMConversionFactorDTO.SearchByParameters.MASTER_ENTITY_ID, "ucf.MasterEntityId"},
                {UOMConversionFactorDTO.SearchByParameters.SITE_ID, "ucf.site_id"},
            };

        /// <summary>
        /// Parameterized Constructor for UOMConversionFactorDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public UOMConversionFactorDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }


        private List<SqlParameter> GetSQLParameters(UOMConversionFactorDTO uomConversionFactorDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(uomConversionFactorDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@UOMConversionFactorId", uomConversionFactorDTO.UOMConversionFactorId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UOMId", uomConversionFactorDTO.UOMId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BaseUOMId", uomConversionFactorDTO.BaseUOMId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ConversionFactor", uomConversionFactorDTO.ConversionFactor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", uomConversionFactorDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", uomConversionFactorDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }


        private UOMConversionFactorDTO GetUOMConversionFactorDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            UOMConversionFactorDTO uomConversionFactorDTO = new UOMConversionFactorDTO(
                dataRow["UOMConversionFactorId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UOMConversionFactorId"]),
                Convert.ToInt32(dataRow["UOMId"]),//Not nullable 
                Convert.ToInt32(dataRow["BaseUOMId"]),
                Convert.ToDouble(dataRow["ConversionFactor"]),
                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                );
            log.LogMethodExit(uomConversionFactorDTO);
            return uomConversionFactorDTO;
        }

        internal UOMConversionFactorDTO GetUOMConversionFactorId(int baseUOMId)
        {
            log.LogMethodEntry(baseUOMId);
            UOMConversionFactorDTO result = null;
            string query = SELECT_QUERY + @" WHERE ucf.BaseUOMId = @UOMConversionFactorId";
            SqlParameter parameter = new SqlParameter("@UOMConversionFactorId", baseUOMId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetUOMConversionFactorDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }


        private void Delete(UOMConversionFactorDTO uomConversionFactorDTO)
        {
            log.LogMethodEntry(uomConversionFactorDTO);
            string query = @"DELETE  
                             FROM UOMConversionFactor
                             WHERE UOMConversionFactor.UOMConversionFactorId = @UOMConversionFactorId";
            SqlParameter parameter = new SqlParameter("@UOMConversionFactorId", uomConversionFactorDTO.UOMConversionFactorId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            uomConversionFactorDTO.AcceptChanges();
            log.LogMethodExit();
        }

        internal List<UOMConversionFactorDTO> GetUOMConversionFactorDTOListOfUOM(List<int> uomIdList, bool activeRecords)
        {
            log.LogMethodEntry(uomIdList);
            List<UOMConversionFactorDTO> uomConversionFactorDTOList = new List<UOMConversionFactorDTO>();
            string query = @"SELECT *
                            FROM UOMConversionFactor, @UomIdList List
                            WHERE BaseUOMId = List.Id or UOMId = List.Id";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@UomIdList", uomIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                uomConversionFactorDTOList = table.Rows.Cast<DataRow>().Select(x => GetUOMConversionFactorDTO(x)).ToList();
            }
            log.LogMethodExit(uomConversionFactorDTOList);
            return uomConversionFactorDTOList;
        }

        internal List<UOMConversionFactorDTO> GetUOMConversionFactorDTOList(List<int> uomIdList, bool activeRecords)
        {
            log.LogMethodEntry(uomIdList);
            List<UOMConversionFactorDTO> uomConversionFactorDTOList = new List<UOMConversionFactorDTO>();
            string query = @"SELECT *
                            FROM UOMConversionFactor, @UomIdList List
                            WHERE BaseUOMId = List.Id";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@UomIdList", uomIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                uomConversionFactorDTOList = table.Rows.Cast<DataRow>().Select(x => GetUOMConversionFactorDTO(x)).ToList();
            }
            log.LogMethodExit(uomConversionFactorDTOList);
            return uomConversionFactorDTOList;
        }


        private void RefreshUOMConversionFactorDTO(UOMConversionFactorDTO uomConversionFactorDTO, DataTable dt)
        {
            log.LogMethodEntry(uomConversionFactorDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                uomConversionFactorDTO.UOMConversionFactorId = Convert.ToInt32(dt.Rows[0]["UOMConversionFactorId"]);
                uomConversionFactorDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                uomConversionFactorDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                uomConversionFactorDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                uomConversionFactorDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                uomConversionFactorDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                uomConversionFactorDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        internal UOMConversionFactorDTO Insert(UOMConversionFactorDTO uomConversionFactorDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(uomConversionFactorDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[UOMConversionFactor]
                               (  [UOMId],
                                  [BaseUOMId],
                                  [ConversionFactor],
                                  [IsActive],
                                  [CreatedBy],
                                  [CreationDate],
                                  [LastUpdatedBy],
                                  [LastUpdateDate],
                                  [site_id],
                                  [Guid],
                                  [MasterEntityId])
                               
                         VALUES
                               (
                                    @UOMId,
                                    @BaseUOMId,
                                    @ConversionFactor,
                                    @IsActive,
                                    @CreatedBy,
                                    GETDATE(),
                                    @LastUpdatedBy,
                                    GETDATE(),
                                    @SiteId,
                                    NEWID(), 
                                    @MasterEntityId )
                                SELECT * FROM UOMConversionFactor WHERE UOMConversionFactorId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(uomConversionFactorDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUOMConversionFactorDTO(uomConversionFactorDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(uomConversionFactorDTO);
            return uomConversionFactorDTO;
        }


        internal UOMConversionFactorDTO Update(UOMConversionFactorDTO uomConversionFactorDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(uomConversionFactorDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[UOMConversionFactor] set
                               [UOMId]               =@UOMId,
                               [BaseUOMId]           = @BaseUOMId,
                               [ConversionFactor]    = @ConversionFactor,
                               [IsActive]            = @IsActive,
                               [LastUpdatedBy]       = @LastUpdatedBy,
                               [MasterEntityId]      = @MasterEntityId,
                               [LastUpdateDate]      = GETDATE()
                               where UOMConversionFactorId = @UOMConversionFactorId
                             SELECT * FROM UOMConversionFactor WHERE UOMConversionFactorId = @UOMConversionFactorId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(uomConversionFactorDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUOMConversionFactorDTO(uomConversionFactorDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(uomConversionFactorDTO);
            return uomConversionFactorDTO;
        }

        public DateTime? GetUOMConversionModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdateDate) LastUpdateDate
                            FROM (
                            select max(LastUpdateDate) LastUpdateDate from UOMConversionFactor WHERE (site_id = @siteId or @siteId = -1)) LastUpdateDate";
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
        /// Gets the uomConversionFactorDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of uomConversionFactorDTO matching the search criteria</returns>
        public List<UOMConversionFactorDTO> GetUOMConversionFactorDTOList(List<KeyValuePair<UOMConversionFactorDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<UOMConversionFactorDTO> uOMConversionFactorDTOList = new List<UOMConversionFactorDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Any()))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<UOMConversionFactorDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == UOMConversionFactorDTO.SearchByParameters.UOM_CONVERSION_FACTOR_ID
                            || searchParameter.Key == UOMConversionFactorDTO.SearchByParameters.UOM_ID
                            || searchParameter.Key == UOMConversionFactorDTO.SearchByParameters.BASE_UOM_ID
                            || searchParameter.Key == UOMConversionFactorDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == UOMConversionFactorDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else if (searchParameter.Key == UOMConversionFactorDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
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
            if (dataTable != null && dataTable.Rows.Cast<DataRow>().Any())
            {
                uOMConversionFactorDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetUOMConversionFactorDTO(x)).ToList();
            }
            log.LogMethodExit(uOMConversionFactorDTOList);
            return uOMConversionFactorDTOList;
        }
    }
}
