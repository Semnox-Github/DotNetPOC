/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data Handler - VendorItemDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70     04-Jun-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the VendorItemDataHandler data object class. This acts as data holder for the VendorItem business object
    /// </summary>
    public class VendorItemDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM VendorItem AS vit";
        /// <summary>
        /// Dictionary for searching Parameters for the VendorItem object.
        /// </summary>
        private static readonly Dictionary<VendorItemDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<VendorItemDTO.SearchByParameters, string>
        {
            {VendorItemDTO.SearchByParameters.VENDOR_ITEM_ID , "vit.VendorItemId"},
            {VendorItemDTO.SearchByParameters.PRODUCT_ID,"vit.ProductId"},
            {VendorItemDTO.SearchByParameters.VENDOR_ID,"vit.VendorId"},
            {VendorItemDTO.SearchByParameters.SITE_ID,"vit.site_id"},
            {VendorItemDTO.SearchByParameters.MASTER_ENTITY_ID,"vit.MasterEntityId"},
            {VendorItemDTO.SearchByParameters.VENDOR_ITEM_CODE,"VenodrItemCode"}
        };

        /// <summary>
        /// Parameterized Constructor for VendorItemDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public VendorItemDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating vit Record.
        /// </summary>
        /// <param name="vendorItemDTO">vendorItemDTO object passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId">site Id of the user </param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(VendorItemDTO vendorItemDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(vendorItemDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@VendorItemId", vendorItemDTO.VendorItemId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Cost", vendorItemDTO.Cost));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", vendorItemDTO.ProductId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VendorId", vendorItemDTO.VendorId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VendorItemCode", vendorItemDTO.VendorItemCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", vendorItemDTO.LastModUserId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", vendorItemDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        ///  Converts the Data row object to VendorItemDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the ReorderStockDTO</returns>
        private VendorItemDTO GetVendorItemDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            VendorItemDTO vendorItemDTO = new VendorItemDTO(dataRow["VendorItemId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["VendorItemId"]),
                                          dataRow["VendorId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["VendorId"]),
                                          dataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductId"]),
                                          dataRow["VenodrItemCode"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["VenodrItemCode"]),
                                          dataRow["Cost"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Cost"]),
                                          dataRow["LastModUserId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LastModUserId"]),
                                          dataRow["LastModDttm"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastModDttm"]),
                                          dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                          dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                          dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                          dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                          dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                          dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                          );
            log.LogMethodExit(vendorItemDTO);
            return vendorItemDTO;
        }

        /// <summary>
        /// Gets the VendorItemDTO data of passed VendorItemId 
        /// </summary>
        /// <param name="vendorItemDTO">vendorItemDTO object of VendorItemDTO</param>
        /// <returns>Returns VendorItemDTO</returns>
        public VendorItemDTO GetVendorItemDTO(int vitId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(vitId);
            VendorItemDTO result = null;
            string query = SELECT_QUERY + @" WHERE vit.VendorItemId = @VendorItemId";
            SqlParameter parameter = new SqlParameter("@VendorItemId", vitId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetVendorItemDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        ///  Inserts the record to the VendorItem Table.
        /// </summary>
        /// <param name="vendorItemDTO">vendorItemDTO object passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId">site Id of the user </param>
        /// <returns> Returns the VendorItemDTO</returns>
        public VendorItemDTO Insert(VendorItemDTO vendorItemDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(vendorItemDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[VendorItem]
                           (VendorId,
                            ProductId,
                            VenodrItemCode,
                            Cost,
                            LastModUserId,
                            LastModDttm,
                            site_id,
                            Guid,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate)
                     VALUES
                           (@VendorId,
                            @ProductId,
                            @VendorItemCode,
                            @Cost,
                            @LastUpdatedBy,
                            GETDATE(),
                            @site_id,
                            NEWID(),
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE() )
                                    SELECT * FROM VendorItem WHERE VendorItemId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(vendorItemDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshVendorItemDTO(vendorItemDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting VendorItemDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(vendorItemDTO);
            return vendorItemDTO;
        }

        /// <summary>
        ///  Updates the record to the VendorItem Table.
        /// </summary>
        /// <param name="vendorItemDTO">vendorItemDTO object passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId">site Id of the user </param>
        /// <returns>Returns the  VendorItemDTO</returns>
        public VendorItemDTO Update(VendorItemDTO vendorItemDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(vendorItemDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[VendorItem]
                           SET 
                            VendorId       = @VendorId,
                            ProductId      = @ProductId,
                            VenodrItemCode = @VendorItemCode,
                            Cost           = @Cost,
                            LastModUserId  = @LastUpdatedBy,
                            LastModDttm    = GETDATE(),
                            MasterEntityId = @MasterEntityId
                            WHERE VendorItemId = @VendorItemId
                           SELECT * FROM VendorItem WHERE VendorItemId = @VendorItemId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(vendorItemDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshVendorItemDTO(vendorItemDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating VendorItemDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(vendorItemDTO);
            return vendorItemDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured
        /// </summary>
        /// <param name="vendorItemDTO">VendorItemDTO object passed as parameter</param>
        /// <param name="dt">dt is an object of DataTable</param>
        /// <param name="loginId">login Id of user</param>
        /// <param name="siteId">site  Id  of user</param>
        private void RefreshVendorItemDTO(VendorItemDTO vendorItemDTO, DataTable dt)
        {
            log.LogMethodEntry(vendorItemDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                vendorItemDTO.VendorItemId = Convert.ToInt32(dt.Rows[0]["VendorItemId"]);
                vendorItemDTO.LastModDate = dataRow["LastModDttm"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastModDttm"]);
                vendorItemDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                vendorItemDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                vendorItemDTO.LastModUserId = dataRow["LastModUserId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LastModUserId"]);
                vendorItemDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                vendorItemDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of VendorItemDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the  lIst of  VendorItemDTO</returns>
        public List<VendorItemDTO> GetVendorItemDTOList(List<KeyValuePair<VendorItemDTO.SearchByParameters, string>> searchParameters ,
                                                        SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<VendorItemDTO> vendorItemDTOList = new List<VendorItemDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<VendorItemDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == VendorItemDTO.SearchByParameters.VENDOR_ITEM_ID
                            || searchParameter.Key == VendorItemDTO.SearchByParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == VendorItemDTO.SearchByParameters.PRODUCT_ID
                            || searchParameter.Key == VendorItemDTO.SearchByParameters.VENDOR_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == VendorItemDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        if (searchParameter.Key == VendorItemDTO.SearchByParameters.VENDOR_ITEM_CODE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    VendorItemDTO vendorItemDTO = GetVendorItemDTO(dataRow);
                    vendorItemDTOList.Add(vendorItemDTO);
                }
            }
            log.LogMethodExit(vendorItemDTOList);
            return vendorItemDTOList;
        }

    }
}
