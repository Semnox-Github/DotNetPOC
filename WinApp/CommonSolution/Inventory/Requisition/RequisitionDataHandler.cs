/********************************************************************************************
* Project Name - Requisition Data Handler
* Description  - Data handler of the Requisition type class
* 
**************
**Version Log
**************
*Version     Date           Modified By       Remarks          
*********************************************************************************************
*1.00        10-Aug-2016    Suneetha.S        Created 
*2.70        15-Jul-2019    Dakshakh raj      Modified : added GetSQLParameters(),
 *                                                      SQL injection Issue Fix
*2.70.2      09-Dec-2019    Jinto Thomas      Removed siteid from update query                                                     
*2.70        15-Jul-2019    Dakshakh raj      Modified : added GetSQLParameters(),
                                                         SQL injection Issue Fix
*2.70.2      24-Oct-2019    Dakshakh          Cobra issue fix, Sql  
*2.100.0     07-Aug-2020    Deeksha           Modified to Handle UOMId field in RequisitionLinesDTO().
*2.110.0     11-Dec-2020    Mushahid Faizan   Modified : Inventory redesign changes
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Requisition Template Type Data Handler - Handles insert, update and select of Requisition type objects
    /// </summary>
    public class RequisitionDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        List<SqlParameter> parameters = new List<SqlParameter>();
        private const string SELECT_QUERY = @"SELECT * FROM InventoryRequisition AS ir ";

        /// <summary>
        /// Dictionary for searching Parameters for the Requisition object.
        /// </summary>
        private static readonly Dictionary<RequisitionDTO.SearchByRequisitionParameters, string> DBSearchParameters = new Dictionary<RequisitionDTO.SearchByRequisitionParameters, string>
            {
                {RequisitionDTO.SearchByRequisitionParameters.REQUISITION_ID, "ir.RequisitionId"},
                {RequisitionDTO.SearchByRequisitionParameters.REQUISITION_ID_LIST, "ir.RequisitionId"},
                {RequisitionDTO.SearchByRequisitionParameters.REQUISITION_NUMBER, "ir.RequisitionNumber"},
                {RequisitionDTO.SearchByRequisitionParameters.ACTIVE_FLAG, "ir.IsActive"},
                {RequisitionDTO.SearchByRequisitionParameters.REQUISITION_TYPE, "ir.RequisitionType"},
                {RequisitionDTO.SearchByRequisitionParameters.STATUS, "ir.Status"},
                {RequisitionDTO.SearchByRequisitionParameters.SITE_ID, "ir.Site_id"},
                {RequisitionDTO.SearchByRequisitionParameters.EXPECTED_RECEIPT_DATE, "ir.RequiredByDate"},
                {RequisitionDTO.SearchByRequisitionParameters.GUID, "ir.Guid"},
                {RequisitionDTO.SearchByRequisitionParameters.GUID_ID_LIST, "ir.Guid"},
                {RequisitionDTO.SearchByRequisitionParameters.FROM_SITE_ID, "ir.FromSiteId"},
                {RequisitionDTO.SearchByRequisitionParameters.TO_SITE_ID, "ir.ToSiteId"},
                {RequisitionDTO.SearchByRequisitionParameters.MASTER_ENTITY_ID, "ir.MasterEntityId"},
                {RequisitionDTO.SearchByRequisitionParameters.ORIGINAL_REFERENCE_GUID, "ir.OriginalReferenceGUID"}
            };

        /// <summary>
        /// Default constructor of ReqTemplatesTypeDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public RequisitionDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodEntry();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Requisition parameters Record.
        /// </summary>
        /// <param name="requisitionDTO">requisitionDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns> Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(RequisitionDTO requisitionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(requisitionDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@requisitionId", requisitionDTO.RequisitionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@requisitionNo", string.IsNullOrEmpty(requisitionDTO.RequisitionNo) ? DBNull.Value : (object)requisitionDTO.RequisitionNo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@reqTemplateTypeId", requisitionDTO.RequisitionType, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@requestingDept", requisitionDTO.RequestingDept, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@fromDepartment", requisitionDTO.FromDepartment, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@toDepartment", requisitionDTO.ToDepartment, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@templateId", requisitionDTO.TemplateId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@value", requisitionDTO.EstimatedValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@remarks", string.IsNullOrEmpty(requisitionDTO.Remarks) ? DBNull.Value : (object)requisitionDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@status", string.IsNullOrEmpty(requisitionDTO.Status) ? DBNull.Value : (object)requisitionDTO.Status));
            parameters.Add(dataAccessHandler.GetSQLParameter("@requiredByDate", requisitionDTO.RequiredByDate == DateTime.MinValue ? DBNull.Value : (object)requisitionDTO.RequiredByDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", requisitionDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", requisitionDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@fromSiteId", requisitionDTO.FromSiteId == -1 ? DBNull.Value : (object)requisitionDTO.FromSiteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@toSiteId", requisitionDTO.ToSiteId == -1 ? DBNull.Value : (object)requisitionDTO.ToSiteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@originalReferenceGUID", requisitionDTO.OriginalReferenceGUID));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Req Templates type record to the database
        /// </summary>
        /// <param name="requisitionsDTO">RequisitionDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns RequisitionDTO</returns>
        public RequisitionDTO Insert(RequisitionDTO requisitionsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(requisitionsDTO, loginId, siteId);
            string insertReqTemplatesQuery = @"INSERT INTO [dbo].[InventoryRequisition] 
                                                        (        
                                                        RequisitionNumber,                                                 
                                                        RequisitionType,
                                                        TemplateId,
                                                        RequestingDept,
                                                        FromDepartment,
                                                        ToDepartment,
                                                        Remarks,
                                                        Status,                                                       
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdatedDate,
                                                        Guid,
                                                        site_id,
                                                        IsActive,
                                                        RequiredByDate,
                                                        FromSiteId,
                                                        ToSiteId,
                                                        OriginalReferenceGUID,
                                                        MasterEntityId
                                                        ) 
                                                values 
                                                        (
                                                         @requisitionNo,
                                                         @reqTemplateTypeId,
                                                         @templateId,
                                                         @requestingDept,
                                                         @fromDepartment,   
                                                         @toDepartment,
                                                         @remarks,
                                                         @status,                                                      
                                                         @createdBy,
                                                         Getdate(),                                                         
                                                         @createdBy,
                                                         Getdate(),                                                         
                                                         NEWID(),
                                                         @siteid,
                                                         @isActive,
                                                         @requiredByDate,
                                                         @fromSiteId,
                                                         @toSiteId,
                                                         NEWID(),
                                                         @masterEntityId
                                                        )SELECT * FROM InventoryRequisition WHERE RequisitionId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertReqTemplatesQuery, GetSQLParameters(requisitionsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRequisitionDTO(requisitionsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting RequisitionsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(requisitionsDTO);
            return requisitionsDTO;
        }

        /// <summary>
        /// Converts the Data row object to RequisitionsDTO class type
        /// </summary>
        /// <param name="requisitionDataRow">RequisitionsDTO DataRow</param>
        /// <returns>Returns RequisitionsDTO</returns>
        private RequisitionDTO GetRequisitionsDTO(DataRow requisitionDataRow)
        {
            log.LogMethodEntry(requisitionDataRow);
            RequisitionDTO productDataObject = new RequisitionDTO(
                                                          Convert.ToInt32(requisitionDataRow["RequisitionId"]),
                                                          Convert.ToString(requisitionDataRow["RequisitionNumber"]),
                                                          requisitionDataRow["TemplateId"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionDataRow["TemplateId"]),
                                                          Convert.ToInt32(requisitionDataRow["RequisitionType"]),
                                                          requisitionDataRow["RequestingDept"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionDataRow["RequestingDept"]),
                                                          requisitionDataRow["FromDepartment"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionDataRow["FromDepartment"]),
                                                          requisitionDataRow["ToDepartment"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionDataRow["ToDepartment"]),
                                                          requisitionDataRow["EstimatedValue"] == DBNull.Value ? 0 : Convert.ToDouble(requisitionDataRow["EstimatedValue"]),
                                                          Convert.ToBoolean(requisitionDataRow["IsActive"]),
                                                          requisitionDataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(requisitionDataRow["Remarks"]),
                                                          requisitionDataRow["Status"].ToString(),
                                                          requisitionDataRow["RequiredByDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(requisitionDataRow["RequiredByDate"]),
                                                          requisitionDataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionDataRow["Site_id"]),
                                                          GetDocumentTypeName(Convert.ToInt32(requisitionDataRow["RequisitionType"])),
                                                          requisitionDataRow["FromSiteId"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionDataRow["FromSiteId"]),
                                                          requisitionDataRow["ToSiteId"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionDataRow["ToSiteId"]),
                                                          requisitionDataRow["OriginalReferenceGUID"].ToString(),
                                                          requisitionDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(requisitionDataRow["CreatedBy"]),
                                                          requisitionDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(requisitionDataRow["CreationDate"]),
                                                          requisitionDataRow["LastUpdatedBy"].ToString(),
                                                          requisitionDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(requisitionDataRow["LastupdatedDate"]),
                                                          requisitionDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(requisitionDataRow["SynchStatus"]),
                                                          requisitionDataRow["Guid"].ToString(),
                                                          requisitionDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionDataRow["MasterEntityId"]),
                                                          new List<RequisitionLinesDTO>()
                                                         );
            log.LogMethodExit(productDataObject);
            return productDataObject;
        }

        /// <summary>
        /// Converts the Data row object to RequisitionsDTO class type
        /// </summary>
        /// <param name="typeId">RequisitionsDTO DataRow</param>
        /// <returns>Returns string document name</returns>
        public string GetDocumentTypeName(int typeId)
        {
            log.LogMethodEntry(typeId);
            string name = string.Empty;
            try
            {
                InventoryDocumentTypeList documentTypeList = new InventoryDocumentTypeList(null);
                InventoryDocumentTypeDTO inventoryDocumentTypeDTO = documentTypeList.GetInventoryDocumentType(typeId);
                if (inventoryDocumentTypeDTO != null)
                {
                    name = inventoryDocumentTypeDTO.Name.ToString();
                }
            }
            catch (Exception ex)
            {
                log.LogMethodExit("Throwing exception -" + ex.Message);
            }
            log.LogMethodExit(name);
            return name;
        }

        /// <summary>
        /// Updates the requisition template header 
        /// </summary>
        /// <param name="requisitionDTO">RequisitionDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the RequisitionDTO</returns>
        public RequisitionDTO Update(RequisitionDTO requisitionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(requisitionDTO, loginId, siteId);
            string updateReqTemplateHeaderQuery = @"update InventoryRequisition 
		                                                set RequisitionType = @reqTemplateTypeId,
		                                                RequestingDept = @requestingDept,
                                                        TemplateId = @templateId,
		                                                FromDepartment = @fromDepartment,
		                                                ToDepartment = @toDepartment,
		                                                Remarks = @remarks,
		                                                IsActive = @isActive,
		                                                Status = @status,
                                                        EstimatedValue=@value,
                                                        RequiredByDate=@requiredByDate,
		                                                LastUpdatedBy = @lastUpdatedBy, 
		                                                LastupdatedDate = Getdate(),
		                                                --site_id=@siteid,
                                                        FromSiteId = @fromSiteId,
                                                        ToSiteId = @toSiteId,
                                                        OriginalReferenceGUID = @originalReferenceGUID                                            
		                                                where RequisitionId = @requisitionId 
                                                        SELECT* FROM InventoryRequisition WHERE RequisitionId = @requisitionId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateReqTemplateHeaderQuery, GetSQLParameters(requisitionDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRequisitionDTO(requisitionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating RequisitionDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(requisitionDTO);
            return requisitionDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="requisitionDTO">requisitionDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshRequisitionDTO(RequisitionDTO requisitionDTO, DataTable dt)
        {
            log.LogMethodEntry(requisitionDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                requisitionDTO.RequisitionId = Convert.ToInt32(dt.Rows[0]["RequisitionId"]);
                requisitionDTO.LastUpdatedAt = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                requisitionDTO.CreatedDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                requisitionDTO.GUID = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                requisitionDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                requisitionDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                requisitionDTO.SiteId = dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]);
            }
            log.LogMethodExit();
        }

        public RequisitionDTO GetRequisitionsDTO(int requisitionId)
        {
            log.LogMethodEntry(requisitionId);
            RequisitionDTO requisitionDTO = null;
            string selectRequisitionquery = SELECT_QUERY + @"where ir.RequisitionId = @requisitionId";
            SqlParameter parameter = new SqlParameter("@requisitionId", requisitionId);
            DataTable requisitions = dataAccessHandler.executeSelectQuery(selectRequisitionquery, new SqlParameter[] { parameter }, sqlTransaction);
            if (requisitions.Rows.Count > 0)
            {
                requisitionDTO = GetRequisitionsDTO(requisitions.Rows[0]);
            }
            log.LogMethodExit(requisitionDTO);
            return requisitionDTO;
        }

        /// <summary>
        /// Gets the RequisitionDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of RequisitionDTO matching the search criteria</returns>
        public List<RequisitionDTO> GetRequisitionsList(List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<RequisitionDTO> requisitionDTOList = null;
            parameters.Clear();
            string selectQuery = SELECT_QUERY;
            selectQuery = selectQuery + GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                requisitionDTOList = new List<RequisitionDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    RequisitionDTO requisitionDTO = GetRequisitionsDTO(dataRow);
                    requisitionDTOList.Add(requisitionDTO);
                }
            }
            log.LogMethodExit(requisitionDTOList);
            return requisitionDTOList;
        }

        private string GetFilterQuery(List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            StringBuilder query = new StringBuilder("");
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == RequisitionDTO.SearchByRequisitionParameters.REQUISITION_ID
                            || searchParameter.Key == RequisitionDTO.SearchByRequisitionParameters.REQUISITION_TYPE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RequisitionDTO.SearchByRequisitionParameters.GUID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == RequisitionDTO.SearchByRequisitionParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == RequisitionDTO.SearchByRequisitionParameters.STATUS)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == RequisitionDTO.SearchByRequisitionParameters.REQUISITION_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == RequisitionDTO.SearchByRequisitionParameters.GUID_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == RequisitionDTO.SearchByRequisitionParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RequisitionDTO.SearchByRequisitionParameters.EXPECTED_RECEIPT_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                            //parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
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
                query.Append(" Order by ir.RequisitionId ASC ");
            }
            log.LogMethodExit();
            return query.ToString();
        }


        /// <summary>
        /// Gets Product id by passing barcode
        /// </summary>
        /// <param name="barcodeNumber">string type parameter</param>
        /// <returns>Returns Product Id</returns>
        public int GetProduct(string barcodeNumber)
        {
            log.LogMethodEntry(barcodeNumber);
            int productId = -1;
            string selectProductQry = @"select * from ProductBarcode where BarCode = @barcode";
            SqlParameter[] selectProductIdParameters = new SqlParameter[1];
            selectProductIdParameters[0] = new SqlParameter("@barcode", barcodeNumber);
            DataTable requisitions = dataAccessHandler.executeSelectQuery(selectProductQry, selectProductIdParameters, sqlTransaction);
            if (requisitions.Rows.Count > 0)
            {
                DataRow requisitionRow = requisitions.Rows[0];
                productId = Convert.ToInt32(requisitionRow["ProductId"]);
                log.LogMethodExit(productId);
                return productId;
            }
            else
            {
                log.LogMethodExit(productId);
                return productId;
            }
        }

        /// <summary>
        /// Gets list of requisitions to create a PO.
        /// </summary>
        /// <param name="siteId">siteId</param>
        /// <param name="applicability">applicability</param>
        /// <param name="toSiteId">toSiteId</param>
        /// <returns>Returns requisitionData</returns>
        public DataTable GetRequistionsToCreatePO(int siteId, string applicability, int toSiteId)
        {
            log.LogMethodEntry(siteId, applicability, toSiteId);
            List<InventoryDocumentTypeDTO> inventoryDocumentTypeDTOList = new List<InventoryDocumentTypeDTO>();
            InventoryDocumentTypeDataHandler inventoryDocumentTypeDataHandler = new InventoryDocumentTypeDataHandler();
            string documentTypeID = "(-1";
            if (!applicability.Equals("ISSUE") && toSiteId == -1)
            {
                inventoryDocumentTypeDTOList = inventoryDocumentTypeDataHandler.GetInventoryDocumentType("Purchase Requisition", siteId);
                if (inventoryDocumentTypeDTOList != null && inventoryDocumentTypeDTOList.Count > 0)
                {
                    foreach (InventoryDocumentTypeDTO inventoryDocumentTypeDTO in inventoryDocumentTypeDTOList)
                        documentTypeID += ", " + inventoryDocumentTypeDTO.DocumentTypeId;
                }
            }
            if (!applicability.Equals("RECEIVE") && toSiteId != -1)
            {
                inventoryDocumentTypeDTOList = inventoryDocumentTypeDataHandler.GetInventoryDocumentType("Inter Store Requisition", toSiteId);
                if (inventoryDocumentTypeDTOList != null && inventoryDocumentTypeDTOList.Count > 0)
                {
                    foreach (InventoryDocumentTypeDTO inventoryDocumentTypeDTO in inventoryDocumentTypeDTOList)
                    {
                        documentTypeID += ", " + inventoryDocumentTypeDTO.DocumentTypeId;
                    }
                }
            }
            documentTypeID += ")";
            string selectRequisitionsQuery = @"select requisitionid,
	                                            requisitionnumber,
	                                            RequestingDept.Name RequestingDept,
	                                            FromDepartment.Name FromDepartment, 
	                                            ToDepartment.Name ToDepartment,
	                                            Remarks,
	                                            s.site_Name as 'Site Name',
	                                            ir.CreatedBy,
	                                            ir.CreationDate,
	                                            idt.Name 'Type'
                                            from InventoryRequisition ir left outer join Location RequestingDept on ir.RequestingDept = RequestingDept.LocationId
	                                            left outer join Location FromDepartment on ir.FromDepartment = FromDepartment.LocationId
	                                            left outer join Location ToDepartment on ir.ToDepartment = ToDepartment.LocationId
	                                            left outer join site s on s.site_id = ir.FromSiteId
	                                            left join InventoryDocumentType idt on idt.DocumentTypeId=ir.RequisitionType
                                            where ir.status = 'Submitted' and ir.RequiredByDate >= Convert(date, GETDATE())
	                                            and RequisitionType in" + documentTypeID +
                                                @"and (ToSiteId = @siteid or ToSiteId is null) 
	                                            and isnull(ir.IsActive, 0) = 1  
                                                and not exists (select *
                                                                from purchaseorder_line
                                                                where requisitionid = ir.requisitionid)";
            SqlParameter[] selectRequisitionParameters = new SqlParameter[1];
            selectRequisitionParameters[0] = new SqlParameter("@siteid", siteId);
            DataTable requisitionData = dataAccessHandler.executeSelectQuery(selectRequisitionsQuery, selectRequisitionParameters, sqlTransaction);
            log.LogMethodExit(requisitionData);
            return requisitionData;
        }

        /// <summary>
        /// Gets the RequisitionDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of RequisitionDTO matching the search criteria</returns>
        public List<RequisitionDTO> GetRequisitionByRequisitionLineInfo(List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int counter = 0;
            string joiner;
            List<RequisitionDTO> requisitionDTOList = new List<RequisitionDTO>();


            string selectRequisitionquery = @"select *
                                                from InventoryRequisition ir
                                               where ir.RequisitionId in (Select RequisitionId from InventoryRequisitionLines ";
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string> searchParameter in searchParameters)
                {   //template id is not on lines

                    if (DBSearchParameters.ContainsKey(searchParameter.Key) && !searchParameter.Key.Equals(RequisitionDTO.SearchByRequisitionParameters.TEMPLATE_ID))
                    {
                        joiner = counter == 0 ? string.Empty : " and ";
                        {
                            if (searchParameter.Key.Equals(RequisitionDTO.SearchByRequisitionParameters.REQUISITION_ID) ||
                                searchParameter.Key.Equals(RequisitionDTO.SearchByRequisitionParameters.REQUISITION_TYPE) ||
                                searchParameter.Key.Equals(RequisitionDTO.SearchByRequisitionParameters.FROM_SITE_ID) ||
                                searchParameter.Key.Equals(RequisitionDTO.SearchByRequisitionParameters.TO_SITE_ID))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key.Equals(RequisitionDTO.SearchByRequisitionParameters.ORIGINAL_REFERENCE_GUID))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
                            else if (searchParameter.Key == RequisitionDTO.SearchByRequisitionParameters.SITE_ID)
                            {
                                query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key.Equals(RequisitionDTO.SearchByRequisitionParameters.ACTIVE_FLAG))
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                            }
                            else if (searchParameter.Key.Equals(RequisitionDTO.SearchByRequisitionParameters.REQUISITION_NUMBER))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
                            else if (searchParameter.Key == RequisitionDTO.SearchByRequisitionParameters.EXPECTED_RECEIPT_DATE)
                            {
                                query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                            }
                            else if (searchParameter.Key.Equals(RequisitionDTO.SearchByRequisitionParameters.STATUS))//Starts:Modified On 22-08-2016
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
                        counter++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                query.Append(" ) Order by ir.RequisitionId ASC");
                if (searchParameters.Count > 0)
                    selectRequisitionquery = selectRequisitionquery + query;
            }

            DataTable requisitionData = dataAccessHandler.executeSelectQuery(selectRequisitionquery, parameters.ToArray(), sqlTransaction);
            if (requisitionData.Rows.Count > 0)
            {
                List<RequisitionDTO> requisitionList = new List<RequisitionDTO>();
                string requisitionIds = "";
                Dictionary<int, RequisitionDTO> mapRequisitions = new Dictionary<int, RequisitionDTO>();
                foreach (DataRow requisitionDataRow in requisitionData.Rows)
                {
                    RequisitionDTO requisitionDataObject = GetRequisitionsDTO(requisitionDataRow);
                    requisitionList.Add(requisitionDataObject);
                    if (requisitionIds == "")
                        requisitionIds = requisitionDataObject.RequisitionId.ToString();
                    else
                        requisitionIds = requisitionIds + " , " + requisitionDataObject.RequisitionId.ToString();
                    if (!mapRequisitions.ContainsKey(requisitionDataObject.RequisitionId))
                    {
                        mapRequisitions.Add(requisitionDataObject.RequisitionId, requisitionDataObject);
                    }

                }
                if (requisitionIds != "")
                {
                    string selectRequisitionLinequery = @"Select * from InventoryRequisitionLines where requisitionId in ( " + requisitionIds + " ) Order by RequisitionId ASC";
                    DataTable requisitionLineData = dataAccessHandler.executeSelectQuery(selectRequisitionLinequery, null, sqlTransaction);
                    if (requisitionLineData.Rows.Count > 0)
                    {
                        foreach (DataRow requisitionLineDataRow in requisitionLineData.Rows)
                        {
                            RequisitionLinesDTO requisitionLinesDTOObj = new RequisitionLinesDTO(Convert.ToInt32(requisitionLineDataRow["RequisitionId"]),
                                                          Convert.ToInt32(requisitionLineDataRow["RequisitionLineId"]),
                                                          Convert.ToString(requisitionLineDataRow["RequisitionNumber"]),
                                                          Convert.ToInt32(requisitionLineDataRow["ProductId"]),
                                                          string.Empty,
                                                          requisitionLineDataRow["Description"].ToString(),
                                                          requisitionLineDataRow["RequestedQuantity"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionLineDataRow["RequestedQuantity"]),
                                                          requisitionLineDataRow["ApprovedQuantity"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionLineDataRow["ApprovedQuantity"]),
                                                          requisitionLineDataRow["ExpectedReceiptDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(requisitionLineDataRow["ExpectedReceiptDate"]),
                                                          Convert.ToBoolean(requisitionLineDataRow["IsActive"]),
                                                          requisitionLineDataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(requisitionLineDataRow["Remarks"]),
                                                          requisitionLineDataRow["Status"] == DBNull.Value ? string.Empty : Convert.ToString(requisitionLineDataRow["Status"]),
                                                          requisitionLineDataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionLineDataRow["Site_id"]),
                                                          string.Empty, 0, 0,
                                                          requisitionLineDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionLineDataRow["MasterEntityId"]),
                                                          requisitionLineDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(requisitionLineDataRow["CreatedBy"]),
                                                          requisitionLineDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(requisitionLineDataRow["CreationDate"]),
                                                          requisitionLineDataRow["LastUpdatedBy"].ToString(),
                                                          requisitionLineDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(requisitionLineDataRow["LastUpdatedDate"]),
                                                          requisitionLineDataRow["Guid"].ToString(),
                                                          requisitionLineDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(requisitionLineDataRow["SynchStatus"]),
                                                          string.Empty,
                                                          requisitionLineDataRow["UOMId"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionLineDataRow["UOMId"]),
                                                          0
                                                       );
                            mapRequisitions[requisitionLinesDTOObj.RequisitionId].RequisitionLinesListDTO.Add(requisitionLinesDTOObj);
                        }
                    }

                }
                log.LogMethodExit(requisitionList);
                return requisitionList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the RequisitionDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of RequisitionDTO matching the search criteria</returns>
        public List<RequisitionDTO> GetRequisitionList(List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> searchParameters, int currentPage = 0, int pageSize = 0)//added
        {
            log.LogMethodEntry(searchParameters);
            List<RequisitionDTO> requisitionDTOList = new List<RequisitionDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY + GetFilterQuery(searchParameters);
            if (currentPage > 0 || pageSize > 0)
            {
                selectQuery += " OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                requisitionDTOList = new List<RequisitionDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    RequisitionDTO requisitionDTO = GetRequisitionsDTO(dataRow);
                    requisitionDTOList.Add(requisitionDTO);
                }
            }
            log.LogMethodExit(requisitionDTOList);
            return requisitionDTOList;
        }

        /// <summary>
        /// Returns the no of Requisition matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetRequisitionsCount(List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int requisitionDTOCount = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery = selectQuery + GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                requisitionDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(requisitionDTOCount);
            return requisitionDTOCount;
        }
    }
}
