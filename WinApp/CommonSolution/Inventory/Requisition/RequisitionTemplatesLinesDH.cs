/********************************************************************************************
* Project Name - RequisitionTemplate Data Handler
* Description  - Data handler of the RequisitionTemplate type class
* 
**************
**Version LogRequisitionTemplatesLinesDH
**************
*Version     Date          Modified By         Remarks          
*********************************************************************************************
*1.00        10-Aug-2016   Suneetha.S          Created 
*2.70        16-Jul-2019   Dakshakh raj        Modified : added GetSQLParameters() and 
*                                                         SQL injection Issue Fix
*2.70.2      26-Nov-2019    Deeksha            Inventory Next Rel Enhancement changes
*2.100.0     07-Aug-2020    Deeksha            Modified for recipe Management enhancement.
**2.110.00   15-Dec-2020   Mushahid Faizan    Modified : Web Inventory Changes
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Requisition Template Type Data Handler - Handles insert, update and select of Requisition type objects
    /// </summary>
    public class RequisitionTemplatesLinesDH
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT rtl.*, p.Code ,u.UOM, c.Name as CategoryName from RequisitionTemplateLines rtl
	                                           left outer join product p on rtl.ProductId = p.ProductId 
	                                           left outer join uom u on u.UOMId = p.UomId
											   left outer join Category c on c.CategoryId = p.CategoryId";

        /// <summary>
        /// Dictionary for searching Parameters for the RequisitionTemplatesLines object.
        /// </summary>
        private static readonly Dictionary<RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters, string> DBSearchParameters = new Dictionary<RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters, string>
            {
                {RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters.TEMPLATE_ID, "rtl.TemplateId"},
                {RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters.TEMPLATE_NAME, "rtl.Name"},
                {RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters.ACTIVE_FLAG, "rtl.IsActive"},
                {RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters.REQUISITION_TYPE, "rtl.RequisitionType"},
                {RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters.STATUS, "rtl.Status"},
                {RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters.MASTER_ENTITY_ID, "rtl.MasterEntityId"},
                {RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters.SITEID, "rtl.Site_id"},
                {RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters.UOM_ID, "rtl.UOMId"}
            };

        /// <summary>
        /// Default constructor of ReqTemplatesTypeDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public RequisitionTemplatesLinesDH(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
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
        private List<SqlParameter> GetSQLParameters(RequisitionTemplateLinesDTO requisitionTemplateLinesDTO , string loginId, int siteId)
        {  
            log.LogMethodEntry(requisitionTemplateLinesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@templateLineId", requisitionTemplateLinesDTO.TemplateLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@templateId", requisitionTemplateLinesDTO.TemplateId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@productId", requisitionTemplateLinesDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@name", string.IsNullOrEmpty(requisitionTemplateLinesDTO.Description) ? DBNull.Value : (object)requisitionTemplateLinesDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@requestedQnty", requisitionTemplateLinesDTO.RequestedQuantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@requiredByDate", requisitionTemplateLinesDTO.RequiredByDate == DateTime.MinValue ? DBNull.Value : (object)requisitionTemplateLinesDTO.RequiredByDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@remarks", string.IsNullOrEmpty(requisitionTemplateLinesDTO.Remarks) ? DBNull.Value : (object)requisitionTemplateLinesDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@status", string.IsNullOrEmpty(requisitionTemplateLinesDTO.Status) ? DBNull.Value : (object)requisitionTemplateLinesDTO.Status));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", requisitionTemplateLinesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", requisitionTemplateLinesDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@uomId", requisitionTemplateLinesDTO.UOMId,true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Req Templates type record to the database
        /// </summary>
        /// <param name="reqTemplatesLinesDTO">RequisitionTemplatesLinesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>RequisitionTemplateLinesDTO</returns>
        public RequisitionTemplateLinesDTO Insert(RequisitionTemplateLinesDTO reqTemplatesLinesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reqTemplatesLinesDTO, loginId, siteId);
            string insertReqTemplateLinesQuery = @"INSERT INTO[dbo].[RequisitionTemplateLines] 
                                                        (                                                         
                                                        TemplateId,
                                                        ProductId, 
                                                        Description,                                                     
                                                        RequestedQuantity,
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
                                                         @templateId,
                                                         @productId,
                                                         @name,
                                                         @requestedQnty,
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
                                                         @uomId
                                                         )SELECT * FROM RequisitionTemplateLines WHERE TemplateLinesId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertReqTemplateLinesQuery, GetSQLParameters(reqTemplatesLinesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRequisitionTemplateLinesDTO(reqTemplatesLinesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ReqTemplatesLinesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(reqTemplatesLinesDTO);
            return reqTemplatesLinesDTO;
        }

        /// <summary>
        /// Updates the requisition template lines 
        /// </summary>
        /// <param name="reqTemplateLinesDTO">RequisitionTemplateLinesDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>RequisitionTemplateLinesDTO</returns>
        public RequisitionTemplateLinesDTO Update(RequisitionTemplateLinesDTO reqTemplateLinesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reqTemplateLinesDTO, loginId, siteId);
            string updateReqTemplateLinesQry = @"update RequisitionTemplateLines 
		                                            set 
		                                            Description = @name,
		                                            RequestedQuantity = @requestedQnty,
					                                ExpectedReceiptDate=@requiredByDate,
		                                            Remarks = @remarks,
		                                            IsActive = @isActive,
		                                            Status = @status,                    
		                                            UOMId = @uomId,                    
		                                            LastUpdatedBy = @lastUpdatedBy, 
		                                            LastupdatedDate = Getdate()
		                                            where TemplateLinesId=@templateLineId 
							                                and TemplateId = @templateId 
							                                and ProductId = @productId
                                                    SELECT* FROM RequisitionTemplateLines WHERE TemplateLinesId=@templateLineId 
							                                and TemplateId = @templateId 
							                                and ProductId = @productId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateReqTemplateLinesQry, GetSQLParameters(reqTemplateLinesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRequisitionTemplateLinesDTO(reqTemplateLinesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating RequisitionTemplateLinesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(reqTemplateLinesDTO);
            return reqTemplateLinesDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="requisitionTemplateLinesDTO">requisitionTemplateLinesDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshRequisitionTemplateLinesDTO(RequisitionTemplateLinesDTO requisitionTemplateLinesDTO , DataTable dt)
        {
            log.LogMethodEntry(requisitionTemplateLinesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                requisitionTemplateLinesDTO.TemplateLineId = Convert.ToInt32(dt.Rows[0]["TemplateLinesId"]);
                requisitionTemplateLinesDTO.LastUpdateDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                requisitionTemplateLinesDTO.CreatedDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                requisitionTemplateLinesDTO.GUID = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                requisitionTemplateLinesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                requisitionTemplateLinesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                requisitionTemplateLinesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to RequisitionTemplateLinesDTO class type
        /// </summary>
        /// <param name="requisitionDataRow">RequisitionTemplateLinesDTO DataRow</param>
        /// <returns>Returns RequisitionTemplateLinesDTO</returns>
        private RequisitionTemplateLinesDTO GetRequisitionTemplateLinesDTO(DataRow requisitionDataRow)
        {
            log.LogMethodEntry(requisitionDataRow);

            RequisitionTemplateLinesDTO requisitionTemplateLinesDTO = new RequisitionTemplateLinesDTO(Convert.ToInt32(requisitionDataRow["TemplateLinesId"]),
                                                          Convert.ToInt32(requisitionDataRow["TemplateId"]),
                                                          Convert.ToInt32(requisitionDataRow["ProductId"]),
                                                          requisitionDataRow["Code"].ToString(),
                                                          requisitionDataRow["Description"].ToString(),
                                                          requisitionDataRow["RequestedQuantity"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionDataRow["RequestedQuantity"]),
                                                          requisitionDataRow["ExpectedReceiptDate"] == DBNull.Value ? ServerDateTime.Now : Convert.ToDateTime(requisitionDataRow["ExpectedReceiptDate"]),
                                                          Convert.ToBoolean(requisitionDataRow["IsActive"]),
                                                          requisitionDataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(requisitionDataRow["Remarks"]),
                                                          requisitionDataRow["Status"] == DBNull.Value ? string.Empty : Convert.ToString(requisitionDataRow["Status"]),
                                                          requisitionDataRow["UOM"].ToString(),
                                                          -1,
                                                          0,
                                                          requisitionDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(requisitionDataRow["CreatedBy"]),
                                                          requisitionDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(requisitionDataRow["CreationDate"]),
                                                          requisitionDataRow["LastUpdatedBy"].ToString(),
                                                          requisitionDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(requisitionDataRow["LastUpdatedDate"]),
                                                          requisitionDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(requisitionDataRow["Guid"]),
                                                          requisitionDataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionDataRow["Site_id"]),
                                                          requisitionDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(requisitionDataRow["SynchStatus"]),
                                                          requisitionDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionDataRow["MasterEntityId"]),
                                                          requisitionDataRow["CategoryName"].ToString(),
                                                          requisitionDataRow["UOMId"] == DBNull.Value ? -1 :Convert.ToInt32(requisitionDataRow["UOMId"])
                                                          );
            log.LogMethodExit(requisitionTemplateLinesDTO);
            return requisitionTemplateLinesDTO;
        }

        /// <summary>
        /// Converts the Data row object to RequisitionTemplateLinesDTO class type
        /// </summary>
        /// <param name="requisitionDataRow">RequisitionTemplateLinesDTO DataRow</param>
        /// <returns>Returns RequisitionTemplateLinesDTO</returns>
        private RequisitionTemplateLinesDTO GetReqTemplateLineDTOForLoad(DataRow requisitionDataRow)
        {
            log.LogMethodEntry(requisitionDataRow);

            RequisitionTemplateLinesDTO requisitionTemplateLinesDTO = new RequisitionTemplateLinesDTO(Convert.ToInt32(requisitionDataRow["TemplateLinesId"]),
                                                          Convert.ToInt32(requisitionDataRow["TemplateId"]),
                                                          Convert.ToInt32(requisitionDataRow["ProductId"]),
                                                          Convert.ToString(requisitionDataRow["Code"]),
                                                          requisitionDataRow["Description"].ToString(),
                                                          requisitionDataRow["RequestedQuantity"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionDataRow["RequestedQuantity"]),
                                                          requisitionDataRow["ExpectedReceiptDate"] == DBNull.Value ? ServerDateTime.Now : Convert.ToDateTime(requisitionDataRow["ExpectedReceiptDate"]),
                                                          Convert.ToBoolean(requisitionDataRow["IsActive"]),
                                                          requisitionDataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(requisitionDataRow["Remarks"]),
                                                          requisitionDataRow["Status"] == DBNull.Value ? string.Empty : Convert.ToString(requisitionDataRow["Status"]),
                                                          requisitionDataRow["UOM"] == DBNull.Value ? string.Empty : Convert.ToString(requisitionDataRow["UOM"]),
                                                          requisitionDataRow["Quantity"] == DBNull.Value ? 0 : Convert.ToInt32(requisitionDataRow["Quantity"]), // generics.GetProductStock(Convert.ToInt32(requisitionDataRow["ProductId"]), Convert.ToInt32(requisitionDataRow["RequisitionId"])),
                                                          requisitionDataRow["Cost"] == DBNull.Value ? 0 : Convert.ToDouble(requisitionDataRow["Cost"]),
                                                          requisitionDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(requisitionDataRow["CreatedBy"]),
                                                          requisitionDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(requisitionDataRow["CreationDate"]),
                                                          requisitionDataRow["LastUpdatedBy"].ToString(),
                                                          requisitionDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(requisitionDataRow["LastUpdatedDate"]),
                                                          requisitionDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(requisitionDataRow["Guid"]),
                                                          requisitionDataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionDataRow["Site_id"]),
                                                          requisitionDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(requisitionDataRow["SynchStatus"]),
                                                          requisitionDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionDataRow["MasterEntityId"]),
                                                          requisitionDataRow["CategoryName"] == DBNull.Value ? string.Empty : Convert.ToString(requisitionDataRow["CategoryName"]),
                                                          requisitionDataRow["UOMId"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionDataRow["UOMId"])
                                                         );
            log.LogMethodExit(requisitionTemplateLinesDTO);
            return requisitionTemplateLinesDTO;
        }

        /// <summary>
        /// Gets the requisition data of passed patch asset application id
        /// </summary>
        /// <param name="templateId">integer type parameter</param>
        /// <param name="isActive">bool type parameter</param>
        /// <param name="locationId">integer type parameter</param>
        /// <returns>Returns List RequisitionTemplateLinesDTO</returns>
        public List<RequisitionTemplateLinesDTO> GetRequisitionTemplateLineList(int templateId, bool isActive, int locationId)
        {
            log.LogMethodEntry(templateId, isActive, locationId);
            List<RequisitionTemplateLinesDTO> requisitionsList = null;
            string selectRequisitionquery = @"select IR.*,p.Code, u.UOM, I.Quantity, p.cost, c.Name as CategoryName
                                                from RequisitionTemplateLines IR 
                                                left outer join Product p on IR.ProductId = p.ProductId 
                                                left outer join UOM u on u.UOMId = p.UomId 
                                                left outer join Inventory I on I.ProductId = p.ProductId and I.LocationId = @locationId
                                                left outer join category c on c.categoryId = p.categoryId
                                                where IR.TemplateId = @requisitionId and IR.IsActive = @isActive";
            SqlParameter[] selectReqParameters = new SqlParameter[3];
            selectReqParameters[0] = new SqlParameter("@requisitionId", templateId);
            selectReqParameters[1] = new SqlParameter("@isActive", isActive);
            selectReqParameters[2] = new SqlParameter("@locationId", locationId);
            DataTable requisition = dataAccessHandler.executeSelectQuery(selectRequisitionquery, selectReqParameters, sqlTransaction);
            if (requisition.Rows.Count > 0)
            {
                requisitionsList = new List<RequisitionTemplateLinesDTO>();
                foreach (DataRow templateRow in requisition.Rows)
                {
                    RequisitionTemplateLinesDTO requisitionDataObject = GetReqTemplateLineDTOForLoad(templateRow);
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
        /// Gets the requisition data of passed patch asset application id
        /// </summary>
        /// <param name="templateLineId">integer type parameter</param>
        /// <returns>Returns RequisitionTemplateLinesDTO</returns>
        public RequisitionTemplateLinesDTO GetRequisitionTemplateLine(int templateLineId)
        {
            log.LogMethodEntry(templateLineId);
            string selectRequisitionquery = SELECT_QUERY + " where rtl.TemplateLinesId = @templateLineId";
            SqlParameter[] selectProductParameters = new SqlParameter[1];
            selectProductParameters[0] = new SqlParameter("@templateLineId", templateLineId);
            DataTable templaates = dataAccessHandler.executeSelectQuery(selectRequisitionquery, selectProductParameters, sqlTransaction);
            if (templaates.Rows.Count > 0)
            {
                DataRow templateRow = templaates.Rows[0];
                RequisitionTemplateLinesDTO requisitionDataObject = GetRequisitionTemplateLinesDTO(templateRow);
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
        /// Gets the RequisitionTemplateLinesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of RequisitionTemplateLinesDTO matching the search criteria</returns>
        public List<RequisitionTemplateLinesDTO> GetRequisitionTemplateList(List<KeyValuePair<RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<RequisitionTemplateLinesDTO> requisitionTemplateLinesDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;

            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters.TEMPLATE_ID
                            || searchParameter.Key == RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters.UOM_ID
                            || searchParameter.Key == RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters.REQUISITION_TYPE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters.SITEID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                requisitionTemplateLinesDTOList = new List<RequisitionTemplateLinesDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    RequisitionTemplateLinesDTO requisitionTemplateLinesDTO  = GetRequisitionTemplateLinesDTO(dataRow);
                    requisitionTemplateLinesDTOList.Add(requisitionTemplateLinesDTO);
                }
            }
            log.LogMethodExit(requisitionTemplateLinesDTOList);
            return requisitionTemplateLinesDTOList;
        }

        /// <summary>
        /// Gets the RequisitionTemplateLinesDTO List for requisitionTemplateIdList Id List
        /// </summary>
        /// <param name="requisitionTemplateIdList">integer list parameter</param>
        /// <returns>Returns List of RequisitionTemplateLinesDTO</returns>
        public List<RequisitionTemplateLinesDTO> GetRequisitionTemplateLinesDTOList(List<int> requisitionTemplateIdList, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(requisitionTemplateIdList);
            List<RequisitionTemplateLinesDTO> requisitionTemplateLinesDTOList = new List<RequisitionTemplateLinesDTO>();
            string query = @"SELECT rtl.*, p.Code ,u.UOM, c.Name as CategoryName from @RequisitionTemplatesIdList List, RequisitionTemplateLines rtl
	                                           left outer join product p on rtl.ProductId = p.ProductId 
	                                           left outer join uom u on u.UOMId = p.UomId
											   left outer join Category c on c.CategoryId = p.CategoryId where rtl.TemplateId = List.Id";

            DataTable table = dataAccessHandler.BatchSelect(query, "@RequisitionTemplatesIdList", requisitionTemplateIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                requisitionTemplateLinesDTOList = table.Rows.Cast<DataRow>().Select(x => GetRequisitionTemplateLinesDTO(x)).ToList();
            }
            log.LogMethodExit(requisitionTemplateLinesDTOList);
            return requisitionTemplateLinesDTOList;
        }
    }
}
