/********************************************************************************************
 * Project Name - TaxStructureDataHandler
 * Description  -Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 **********************************************************************************************
*2.50        30-Jan-2019   Mushahid Faizan     Created
*2.50        17-Mar-2019   Manoj Durgam        Added ExecutionContext to the constructor of TaxStructureBL
*2.60        11-Apr-2019   Girish Kundar       Copied this file to Inventory Module 
*2.70        02-Apr-2019   Akshay Gulaganji    Added isActive and handled in this handler and modified Insert and Update Method 
*2.70        15-Jul-2019   Mehraj              Added Delete() method
*2.70.2        10-Dec-2019   Jinto Thomas         Removed siteid from update query
*2.110.0     08-Oct-2020   Mushahid Faizan     Added GetTaxStructureDTOList for pagination.
**********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;


namespace Semnox.Parafait.Product
{
    public class TaxStructureDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM TaxStructure AS txs ";
        private static readonly Dictionary<TaxStructureDTO.SearchByTaxStructureParameters, string> DBSearchParameters = new Dictionary<TaxStructureDTO.SearchByTaxStructureParameters, string>
            {
                {TaxStructureDTO.SearchByTaxStructureParameters.TAX_STRUCTURE_ID,"txs.taxStructureId"},
                {TaxStructureDTO.SearchByTaxStructureParameters.TAX_ID, "txs.taxId"},
                {TaxStructureDTO.SearchByTaxStructureParameters.PARENT_STRUCTURE_ID, "txs.parentStructureId"},
                {TaxStructureDTO.SearchByTaxStructureParameters.MASTER_ENTITY_ID,"txs.MasterEntityId"},//starts:Modification on 18-Jul-2016 for publish feature
                {TaxStructureDTO.SearchByTaxStructureParameters.SITE_ID, "txs.site_Id"},
                {TaxStructureDTO.SearchByTaxStructureParameters.TAX_STRUCTURE_NAME, "txs.taxStructurename"},
                {TaxStructureDTO.SearchByTaxStructureParameters.TAX_PERCENTAGE, "txs.taxPercentage"}, //Ends:Modification on 18-Jul-2016 for publish feature
                {TaxStructureDTO.SearchByTaxStructureParameters.IS_ACTIVE,"txs.IsActive"}
            };

        /// <summary>
        /// Default constructor of TaxStructureHandler class
        /// </summary>
        public TaxStructureDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Address Record.
        /// </summary>
        /// <param name="TaxStructureDTO">TaxStructureDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(TaxStructureDTO taxStructureDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(taxStructureDTO, loginId, siteId);

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@TaxStructureId", taxStructureDTO.TaxStructureId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TaxId", taxStructureDTO.TaxId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@StructureName", taxStructureDTO.StructureName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Percentage", taxStructureDTO.Percentage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParentStructureId", taxStructureDTO.ParentStructureId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", taxStructureDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", taxStructureDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Insert the TaxStructureDTO record to the database
        /// </summary>
        /// <param name="taxStructureDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public TaxStructureDTO InsertTaxStructure(TaxStructureDTO taxStructureDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(taxStructureDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[TaxStructure]
                                (
                                            TaxId,
                                            StructureName,
                                            Percentage,
                                            ParentStructureId,
                                            Guid, 
                                            site_id,
                                            MasterEntityId,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate, 
                                            IsActive 
                                        ) 
                                VALUES 
                                        (
                                            @TaxId,
                                            @StructureName,                                                        
                                            @Percentage,
                                            @ParentStructureId,
                                            newid(), 
                                            @site_id,
                                            @masterEntityId,
                                            @createdBy,
                                            GETDATE(),
                                            @lastUpdatedBy,
                                            GETDATE(),
                                            @isActive
                                         ) SELECT * FROM TaxStructure WHERE TaxStructureId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(taxStructureDTO, loginId, siteId).ToArray(), sqlTransaction);
                if (dt.Rows.Count > 0)
                {
                    DataRow dataRow = dt.Rows[0];
                    taxStructureDTO.TaxStructureId = Convert.ToInt32(dt.Rows[0]["TaxStructureId"]);
                    taxStructureDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                    taxStructureDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                    taxStructureDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                    taxStructureDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                    taxStructureDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                    taxStructureDTO.site_Id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(taxStructureDTO);
            return taxStructureDTO;
        }
        /// <summary>
        /// Updates the TaxStructureDTO record to the database
        /// </summary>
        /// <param name="taxStructureDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public TaxStructureDTO UpdateTaxStructure(TaxStructureDTO taxStructureDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(taxStructureDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[TaxStructure] set   
                                            TaxId=@TaxId,
                                            StructureName=@StructureName,
                                            Percentage=@Percentage,
                                            ParentStructureId=@ParentStructureId,
                                            -- site_id=@site_id,
                                            MasterEntityId=@masterEntityId,
                                            LastUpdatedBy=@lastUpdatedBy,
                                            LastUpdateDate= GETDATE(),
                                            IsActive=@isActive
                             where TaxStructureId = @TaxStructureId
                             SELECT * FROM TaxStructure WHERE TaxStructureId = @TaxStructureId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(taxStructureDTO, loginId, siteId).ToArray(), sqlTransaction);
                if (dt.Rows.Count > 0)
                {
                    DataRow dataRow = dt.Rows[0];
                    taxStructureDTO.TaxStructureId = Convert.ToInt32(dt.Rows[0]["TaxStructureId"]);
                    taxStructureDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                    taxStructureDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                    taxStructureDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                    taxStructureDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                    taxStructureDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                    taxStructureDTO.site_Id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(taxStructureDTO);
            return taxStructureDTO;
        }

        public TaxStructureDTO GetTaxStructureDTO(int taxStructureId)
        {
            log.LogMethodEntry(taxStructureId);
            TaxStructureDTO TaxStructureDTO = null;
            string query = SELECT_QUERY + @" WHERE txs.TaxStructureId = @TaxStructureId";
            SqlParameter parameter = new SqlParameter("@TaxStructureId", taxStructureId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                TaxStructureDTO = GetTaxStructureDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(TaxStructureDTO);
            return TaxStructureDTO;
        }
        /// <summary>
        /// Deletes the TaxStructureDTO record to the database
        /// </summary>
        /// <param name="taxStructureDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public TaxStructureDTO DeleteTaxStructure(TaxStructureDTO taxStructureDTO, string userId, int siteId)
        {
            log.LogMethodEntry(taxStructureDTO, userId, siteId);
            try
            {
                string deleteQuery = @"delete from TaxStructure where TaxStructureId = @TaxStructureId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@TaxStructureId", taxStructureDTO.TaxStructureId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(taxStructureDTO);
            return taxStructureDTO;
        }
        /// <summary>
        /// Gets the TaxStructureDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of TaxStructureDTO matching the search criteria</returns>
        public List<TaxStructureDTO> GetTaxStructureList(List<KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<TaxStructureDTO> taxStructureDTOList = new List<TaxStructureDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectTaxQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int count = 0;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count > 0) ? " and " : " ";

                        if (searchParameter.Key == TaxStructureDTO.SearchByTaxStructureParameters.TAX_ID
                            || searchParameter.Key == TaxStructureDTO.SearchByTaxStructureParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == TaxStructureDTO.SearchByTaxStructureParameters.TAX_PERCENTAGE
                            || searchParameter.Key == TaxStructureDTO.SearchByTaxStructureParameters.TAX_STRUCTURE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TaxStructureDTO.SearchByTaxStructureParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TaxStructureDTO.SearchByTaxStructureParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
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
                selectTaxQuery = selectTaxQuery + query;
            }
            DataTable assetTaxStructure = dataAccessHandler.executeSelectQuery(selectTaxQuery, parameters.ToArray(), sqlTransaction);
            if (assetTaxStructure.Rows.Count > 0)
            {
                List<TaxStructureDTO> taxStructureDtoList = new List<TaxStructureDTO>();
                foreach (DataRow taxStructureRow in assetTaxStructure.Rows)
                {
                    TaxStructureDTO taxStructureObject = GetTaxStructureDTO(taxStructureRow);
                    taxStructureDtoList.Add(taxStructureObject);
                }
                log.LogMethodExit(taxStructureDtoList);
                return taxStructureDtoList;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }
        /// <summary>
        /// Converts the Data row object to TaxStructureDTO class type
        /// </summary>
        /// <param name="taxDataRow"> DataRow</param>
        /// <returns>Returns </returns>
        private TaxStructureDTO GetTaxStructureDTO(DataRow taxDataRow)
        {
            log.LogMethodEntry(taxDataRow);
            TaxStructureDTO taxStructureDTO = new TaxStructureDTO(Convert.ToInt32(taxDataRow["TaxStructureId"]),
                                            Convert.ToInt32(taxDataRow["TaxId"]),
                                            taxDataRow["StructureName"].ToString(),
                                            taxDataRow["Percentage"] == DBNull.Value ? -1 : Convert.ToDouble(taxDataRow["Percentage"]),
                                            taxDataRow["ParentStructureId"] == DBNull.Value ? -1 : Convert.ToInt32(taxDataRow["ParentStructureId"]),
                                            taxDataRow["Guid"].ToString(),
                                            taxDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(taxDataRow["SynchStatus"]),
                                            taxDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(taxDataRow["site_id"]),
                                            taxDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(taxDataRow["MasterEntityId"]),
                                            taxDataRow["CreatedBy"].ToString(),
                                            taxDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(taxDataRow["CreationDate"]),
                                            taxDataRow["LastUpdatedBy"].ToString(),
                                            taxDataRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(taxDataRow["LastupdateDate"]),
                                            taxDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(taxDataRow["IsActive"])
                                            );
            log.LogMethodExit(taxStructureDTO);
            return taxStructureDTO;
        }

        /// <summary>
        /// Gets the TaxStructureDTO List for tax Id List
        /// </summary>
        /// <param name="dsLookupIdList">integer list parameter</param>
        /// <returns>Returns List of TaxStructureDTOList</returns>
        public List<TaxStructureDTO> GetTaxStructureDTOList(List<int> taxIdList, bool activeRecords, int currentPage, int pageSize)
        {
            log.LogMethodEntry(taxIdList);
            List<TaxStructureDTO> list = new List<TaxStructureDTO>();
            string query = @"SELECT TaxStructure.*
                            FROM TaxStructure, @taxIdList List
                            WHERE TaxId = List.Id ";
            if (activeRecords)
            {
                query += " AND Isnull(IsActive,'1') = 1 ";
            }
            if (currentPage > 0 && pageSize > 0)
            {
                query += " ORDER BY TaxStructure.TaxId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                query += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@taxIdList", taxIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetTaxStructureDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

    }
}
