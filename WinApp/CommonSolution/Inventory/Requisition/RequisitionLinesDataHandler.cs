/********************************************************************************************
* Project Name - Requisition Lines Data Handler
* Description  - Data handler of the Requisition Lines type class
* 
**************
**Version Log
**************
*Version     Date          Modified By      Remarks          
*********************************************************************************************
*1.00        14-Aug-2016   Suneetha.S       Created 
*2.70        16-Jul-2019   Dakshakh raj     Modified : added GetSQLParameters() and
*                                                      SQL injection Issue Fix
*2.70.2      21-Nov-2019   Deeksha          Inventory Next Rel Enhancement changes
*2.100.0     27-Jul-2020   Deeksha          Modified : Added UOMId field.
*2.110.0     11-Dec-2020   Mushahid Faizan    Modified : Web Inventory Changes
*2.120.0     08-Apr-2020   Mushahid Faizan   Added PriceInTickets field as part of Inventory enhancement
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Requisition Template Type Data Handler - Handles insert, update and select of Requisition type objects
    /// </summary>
    public class RequisitionLinesDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT irl.*, p.code,p.priceInTickets, p.Description, p.cost, u.UOM , null as Quantity, c.Name as CategoryName
                                                FROM InventoryRequisitionLines irl 
                                                     left outer join product p on irl.ProductId = p.ProductId 
	                                                 left outer join uom u on u.UOMId = p.UomId
													 left outer join Category c on c.CategoryId=p.CategoryId";

        /// <summary>
        /// Dictionary for searching Parameters for the RequisitionLines object.
        /// </summary>
        private static readonly Dictionary<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string> DBSearchParameters = new Dictionary<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>
            {
                {RequisitionLinesDTO.SearchByRequisitionLinesParameters.REQUISITION_NUMBER, "irl.RequisitionNo"},
                {RequisitionLinesDTO.SearchByRequisitionLinesParameters.REQUISITION_ID, "irl.RequisitionId"},
                {RequisitionLinesDTO.SearchByRequisitionLinesParameters.ACTIVE_FLAG, "irl.IsActive"},
                {RequisitionLinesDTO.SearchByRequisitionLinesParameters.PRODUCT_ID, "irl.ProductId"},
                {RequisitionLinesDTO.SearchByRequisitionLinesParameters.REQUISITION_TYPE, "irl.RequisitionType"},
                {RequisitionLinesDTO.SearchByRequisitionLinesParameters.STATUS, "irl.Status"},
                {RequisitionLinesDTO.SearchByRequisitionLinesParameters.SITE_ID, "irl.Site_id"},
                {RequisitionLinesDTO.SearchByRequisitionLinesParameters.EXPECTED_RECEIPT_DATE, "irl.ExpectedReceiptDate"},
                {RequisitionLinesDTO.SearchByRequisitionLinesParameters.REQUISITION_IDS_STRING, "irl.RequisitionIDs"},
                {RequisitionLinesDTO.SearchByRequisitionLinesParameters.MASTER_ENTITY_ID, "irl.MasterEntityId"},
                {RequisitionLinesDTO.SearchByRequisitionLinesParameters.UOM_ID, "irl.UOMId"}
            };

        /// <summary>
        ///  Default constructor of ReqTemplatesTypeDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public RequisitionLinesDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodEntry();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Report parameters Record.
        /// </summary>
        /// <param name="requisitionLinesDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(RequisitionLinesDTO requisitionLinesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(requisitionLinesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@requisitionLineId", requisitionLinesDTO.RequisitionLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@requisitionId", requisitionLinesDTO.RequisitionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@requisitionNo", requisitionLinesDTO.RequisitionNo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@productId", requisitionLinesDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@name", string.IsNullOrEmpty(requisitionLinesDTO.Description) ? DBNull.Value : (object)requisitionLinesDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@approvedQuantity", requisitionLinesDTO.ApprovedQuantity < 0 ? DBNull.Value : (object)requisitionLinesDTO.ApprovedQuantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@requestedQnty", requisitionLinesDTO.RequestedQuantity < 0 ? DBNull.Value : (object)requisitionLinesDTO.RequestedQuantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@requiredByDate", requisitionLinesDTO.RequiredByDate == DateTime.MinValue ? DBNull.Value : (object)requisitionLinesDTO.RequiredByDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@remarks", string.IsNullOrEmpty(requisitionLinesDTO.Remarks) ? DBNull.Value : (object)requisitionLinesDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@status", string.IsNullOrEmpty(requisitionLinesDTO.Status) ? DBNull.Value : (object)requisitionLinesDTO.Status));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", requisitionLinesDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", requisitionLinesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UOMId", requisitionLinesDTO.UOMId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the Requisition Lines record to the database
        /// </summary>
        /// <param name="requisitionLinesDTO">RequisitionLinesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>RequisitionLinesDTO</returns>
        public RequisitionLinesDTO Insert(RequisitionLinesDTO requisitionLinesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(requisitionLinesDTO, loginId, siteId);
            string insertRequisitionLinesQuery = @"INSERT INTO[dbo].[InventoryRequisitionLines] 
                                                        (                                                 
                                                        RequisitionId,
                                                        RequisitionNumber,
                                                        ProductId, 
                                                        Description,                                                     
                                                        RequestedQuantity,
                                                        ApprovedQuantity,
                                                        ExpectedReceiptDate,
                                                        Remarks,
                                                        Status,                                                        
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdatedDate,
                                                        Guid,
                                                        site_id,
                                                        IsActive,
                                                        MasterEntityId,
                                                        UOMId
                                                        ) 
                                                values 
                                                        (
                                                         @requisitionId,
                                                         @requisitionNo,
                                                         @productId,
                                                         @name,
                                                         @requestedQnty,
                                                         @approvedQuantity,
                                                         @requiredByDate,
                                                         @remarks,
                                                         @status,                                                         
                                                         @createdBy,
                                                         Getdate(),                                                         
                                                         @createdBy,
                                                         Getdate(),                                                         
                                                         NEWID(),
                                                         @siteid,
                                                         @isActive,
                                                         @masterEntityId,
                                                         @UOMId
                                                        )SELECT * FROM InventoryRequisitionLines WHERE RequisitionLineId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertRequisitionLinesQuery, GetSQLParameters(requisitionLinesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRequisitionLinesDTO(requisitionLinesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting RequisitionLinesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(requisitionLinesDTO);
            return requisitionLinesDTO;
        }


        /// <summary>
        /// Updates the requisition template lines 
        /// </summary>
        /// <param name="requisitionLinesDTO">RequisitionLinesDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>RequisitionLinesDTO</returns>
        public RequisitionLinesDTO Update(RequisitionLinesDTO requisitionLinesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(requisitionLinesDTO, loginId, siteId);
            string updateRequisitionLinesQry = @"update InventoryRequisitionLines 
		                                            set 
		                                            Description = @name,
		                                            RequestedQuantity = @requestedQnty,
                                                    ApprovedQuantity = @approvedQuantity,
					                                ExpectedReceiptDate=@requiredByDate,
		                                            Remarks = @remarks,
		                                            IsActive = @isActive,
		                                            Status = @status,                    
		                                            LastUpdatedBy = @lastUpdatedBy, 
		                                            LastupdatedDate = Getdate(),
		                                            UOMId = @UOMId
		                                            where RequisitionId=@requisitionId 
							                          and RequisitionLineId = @requisitionLineId 
							                          and ProductId = @productId 
                                                    SELECT * FROM InventoryRequisitionLines WHERE RequisitionId=@requisitionId 
							                                and RequisitionLineId = @requisitionLineId 
							                                and ProductId = @productId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateRequisitionLinesQry, GetSQLParameters(requisitionLinesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRequisitionLinesDTO(requisitionLinesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating RequisitionLinesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(requisitionLinesDTO);
            return requisitionLinesDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="requisitionLinesDTO"></param>
        /// <param name="dt"></param>
        private void RefreshRequisitionLinesDTO(RequisitionLinesDTO requisitionLinesDTO, DataTable dt)
        {
            log.LogMethodEntry(requisitionLinesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                requisitionLinesDTO.RequisitionLineId = Convert.ToInt32(dt.Rows[0]["RequisitionLineId"]);
                requisitionLinesDTO.LastUpdatedAt = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                requisitionLinesDTO.CreatedDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                requisitionLinesDTO.GUID = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                requisitionLinesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                requisitionLinesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                requisitionLinesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to RequisitionLinesDTO class type
        /// </summary>
        /// <param name="requisitionDataRow">RequisitionLinesDTO DataRow</param>
        /// <returns>Returns RequisitionLinesDTO</returns>
        private RequisitionLinesDTO GetRequisitionLinesDTO(DataRow requisitionDataRow)
        {
            log.LogMethodEntry(requisitionDataRow);
            string uomvalue = string.Empty;
            double price = 0;
            double stockAtLocation = 0;
            string productCode = string.Empty;
            requisitionDataRow["Quantity"] = stockAtLocation;
            RequisitionLinesDTO productDataObject = GetRequisitionLine(requisitionDataRow);
            log.LogMethodExit(productDataObject);
            return productDataObject;
        }

        /// <summary>
        /// Converts the Data row object to RequisitionLinesDTO class type
        /// </summary>
        /// <param name="requisitionDataRow">RequisitionLinesDTO DataRow</param>
        /// <returns>Returns RequisitionLinesDTO</returns>
        private RequisitionLinesDTO GetRequisitionLine(DataRow requisitionDataRow)
        {
            log.LogMethodEntry(requisitionDataRow);
            RequisitionLinesDTO productDataObject = new RequisitionLinesDTO(Convert.ToInt32(requisitionDataRow["RequisitionId"]),
                                                      Convert.ToInt32(requisitionDataRow["RequisitionLineId"]),
                                                      Convert.ToString(requisitionDataRow["RequisitionNumber"]),
                                                      Convert.ToInt32(requisitionDataRow["ProductId"]),
                                                      requisitionDataRow["code"].ToString(),
                                                      requisitionDataRow["Description"].ToString(),
                                                      requisitionDataRow["RequestedQuantity"] == DBNull.Value ? 0 : Convert.ToDouble(requisitionDataRow["RequestedQuantity"]),
                                                      requisitionDataRow["ApprovedQuantity"] == DBNull.Value ? 0 : Convert.ToDouble(requisitionDataRow["ApprovedQuantity"]),
                                                      requisitionDataRow["ExpectedReceiptDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(requisitionDataRow["ExpectedReceiptDate"]),
                                                      Convert.ToBoolean(requisitionDataRow["IsActive"]),
                                                      requisitionDataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(requisitionDataRow["Remarks"]),
                                                      requisitionDataRow["Status"] == DBNull.Value ? string.Empty : Convert.ToString(requisitionDataRow["Status"]),
                                                      requisitionDataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionDataRow["Site_id"]),
                                                      requisitionDataRow["UOM"] == DBNull.Value ? string.Empty : Convert.ToString(requisitionDataRow["UOM"]),
                                                      requisitionDataRow["Quantity"] == DBNull.Value ? 0 : Convert.ToDouble(requisitionDataRow["Quantity"]),
                                                      requisitionDataRow["Cost"] == DBNull.Value ? 0 : Convert.ToDouble(requisitionDataRow["Cost"]),
                                                      requisitionDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionDataRow["MasterEntityId"]),
                                                      requisitionDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(requisitionDataRow["CreatedBy"]),
                                                      requisitionDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(requisitionDataRow["CreationDate"]),
                                                      requisitionDataRow["LastUpdatedBy"].ToString(),
                                                      requisitionDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(requisitionDataRow["LastUpdatedDate"]),
                                                      requisitionDataRow["Guid"].ToString(),
                                                      requisitionDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(requisitionDataRow["SynchStatus"]),
                                                      requisitionDataRow["CategoryName"] == DBNull.Value ? string.Empty : Convert.ToString(requisitionDataRow["CategoryName"]),
                                                      requisitionDataRow["UOMId"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionDataRow["UOMId"]),
                                                      Convert.ToDouble(requisitionDataRow["PriceInTickets"])
                                                     );
            log.LogMethodExit(productDataObject);
            return productDataObject;
        }

        /// <summary>
        /// Gets the requisition data of passed patch asset application id
        /// </summary>
        /// <param name="requisitionLineId">integer type parameter</param>
        /// <returns>Returns RequisitionLinesDTO</returns>
        public RequisitionLinesDTO GetRequisitionLine(int requisitionLineId)
        {
            log.LogMethodEntry(requisitionLineId);
            string selectRequisitionquery = SELECT_QUERY + @" WHERE irl.RequisitionLineId = @requisitionLineId";
            SqlParameter[] selectReqParameters = new SqlParameter[1];
            selectReqParameters[0] = new SqlParameter("@requisitionLineId", requisitionLineId);
            DataTable requisition = dataAccessHandler.executeSelectQuery(selectRequisitionquery, selectReqParameters, sqlTransaction);
            if (requisition.Rows.Count > 0)
            {
                DataRow templateRow = requisition.Rows[0];
                RequisitionLinesDTO requisitionDataObject = GetRequisitionLinesDTO(templateRow);
                log.LogMethodExit(requisitionDataObject);
                return requisitionDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the requisition data of passed patch asset application id
        /// </summary>
        /// <param name="requisitionId">integer type parameter</param>
        /// <param name="isActive">bool type parameter</param>
        /// <param name="locationId">integer type parameter</param>
        /// <returns>Returns List of RequisitionLinesDTO objects </returns>
        public List<RequisitionLinesDTO> GetRequisitionLineList(int requisitionId, bool isActive, int locationId)
        {
            log.LogMethodEntry(requisitionId, isActive, locationId);
            List<RequisitionLinesDTO> requisitionsList = null;
            string selectRequisitionquery = @"select IR.*,u.UOM, (select sum(I.Quantity) from Inventory I where I.ProductId = p.ProductId and I.LocationId =  @locationId) as Quantity, 
                                                p.cost ,p.Code ,p.priceInTickets, c.Name as CategoryName
                                                from InventoryRequisitionLines IR 
                                                left outer join Product p on IR.ProductId = p.ProductId
                                                left outer join UOM u on u.UOMId = p.UomId
												left outer join Category c on c.CategoryId = p.CategoryId
                                                where IR.RequisitionId = @requisitionId and IR.IsActive = @isActive";
            SqlParameter[] selectReqParameters = new SqlParameter[3];
            selectReqParameters[0] = new SqlParameter("@requisitionId", requisitionId);
            selectReqParameters[1] = new SqlParameter("@isActive", isActive);
            selectReqParameters[2] = new SqlParameter("@locationId", locationId);
            DataTable requisition = dataAccessHandler.executeSelectQuery(selectRequisitionquery, selectReqParameters, sqlTransaction);
            if (requisition.Rows.Count > 0)
            {
                requisitionsList = new List<RequisitionLinesDTO>();
                foreach (DataRow templateRow in requisition.Rows)
                {
                    RequisitionLinesDTO requisitionDataObject = GetRequisitionLine(templateRow);
                    requisitionsList.Add(requisitionDataObject);
                    log.LogMethodExit();
                }
            }
            else
            {
                log.LogMethodExit();

            }
            log.LogMethodExit(requisitionsList);
            return requisitionsList;
        }

        /// <summary>
        /// Gets the RequisitionLinesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of RequisitionLinesDTO matching the search criteria</returns>
        public List<RequisitionLinesDTO> GetRequisitionLinesList(List<KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<RequisitionLinesDTO> requisitionLinesList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;

            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == RequisitionLinesDTO.SearchByRequisitionLinesParameters.REQUISITION_ID
                            || searchParameter.Key == RequisitionLinesDTO.SearchByRequisitionLinesParameters.PRODUCT_ID
                            || searchParameter.Key == RequisitionLinesDTO.SearchByRequisitionLinesParameters.UOM_ID
                            || searchParameter.Key == RequisitionLinesDTO.SearchByRequisitionLinesParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RequisitionLinesDTO.SearchByRequisitionLinesParameters.REQUISITION_TYPE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == RequisitionLinesDTO.SearchByRequisitionLinesParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RequisitionLinesDTO.SearchByRequisitionLinesParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == RequisitionLinesDTO.SearchByRequisitionLinesParameters.EXPECTED_RECEIPT_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == RequisitionLinesDTO.SearchByRequisitionLinesParameters.STATUS)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == RequisitionLinesDTO.SearchByRequisitionLinesParameters.REQUISITION_IDS_STRING)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
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
                selectQuery = selectQuery + query + " Order by RequisitionLineId ASC";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                requisitionLinesList = new List<RequisitionLinesDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    RequisitionLinesDTO requisitionLinesDTO = GetRequisitionLinesDTO(dataRow);
                    requisitionLinesList.Add(requisitionLinesDTO);
                }
            }
            log.LogMethodExit(requisitionLinesList);
            return requisitionLinesList;
        }

        /// <summary>
        /// Gets the RequisitionLinesDTO List for requisitionIdList Id List
        /// </summary>
        /// <param name="requisitionIdList">integer list parameter</param>
        /// <returns>Returns List of RequisitionLinesDTO</returns>
        public List<RequisitionLinesDTO> GetRequisitionLinesDTOList(List<int> requisitionIdList, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(requisitionIdList);
            List<RequisitionLinesDTO> requisitionLinesDTOList = new List<RequisitionLinesDTO>();
            string query = @"SELECT irl.*, p.code, p.Description, p.cost, p.priceInTickets, u.UOM , null as Quantity, c.Name as CategoryName
                                                FROM @RequisitionIdList List, InventoryRequisitionLines irl
                                                     left outer join product p on irl.ProductId = p.ProductId
                                                     left outer join uom u on u.UOMId = p.UomId
                                                     left outer join Category c on c.CategoryId = p.CategoryId
                                                       where irl.RequisitionId = List.Id";

            DataTable table = dataAccessHandler.BatchSelect(query, "@RequisitionIdList", requisitionIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                requisitionLinesDTOList = table.Rows.Cast<DataRow>().Select(x => GetRequisitionLine(x)).ToList();
            }
            log.LogMethodExit(requisitionLinesDTOList);
            return requisitionLinesDTOList;
        }
    }
}
