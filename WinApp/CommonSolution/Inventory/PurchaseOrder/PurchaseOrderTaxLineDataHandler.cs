/********************************************************************************************
 * Project Name - PurchaseOrderReceiveTaxLineDataHandler
 * Description  -Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By       Remarks          
 **********************************************************************************************
 *2.60        11-Apr-2019      Girish Kundar     Created
 *2.70.2        16-jul-2019      Deeksha           Modified:Added GetSqlParameter(),SQL injection issue Fix
 *2.70.2        09-Dec-2019      Jinto Thomas     Removed siteid from update query 
  ********************************************************************************************/

using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.Inventory
{
    public class PurchaseOrderTaxLineDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM PurchaseOrderTaxLine AS pt ";
        /// <summary>
        /// Dictionary for searching Parameters for the Purchase OrderTaxLine  object.
        /// </summary>
        private static readonly Dictionary<PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters, string> DBSearchParameters = new Dictionary<PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters, string>
            {
                {PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.PO_TAX_LINE_ID, "pt.purchaseOrderTaxLineId"},
                {PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.PO_LINE_ID, "pt.purchaseOrderLineId"},
                {PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.PO_ID, "pt.purchaseOrderId"},
                {PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.TAX_STRUCTURE_ID, "pt.taxStructureId"},
                {PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.TAX_STRUCTURE_NAME, "pt.taxtructureName"},
                {PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.PRODUCT_ID, "pt.productId"},
                {PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.PURCHASE_TAX_ID, "pt.purchaseTaxId"},
                {PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.PURCHASE_TAX_NAME, "pt.purchaseTaxName"},
                {PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.ISACTIVE_FLAG, "pt.isActive"},
                {PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.SITEID, "pt.Site_id"},
                {PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.MASTER_ENTITY_ID, "pt.MasterEntityId"},

            };

        /// <summary>
        /// Default constructor of PurchaseOrderTaxLineDataHandler class
        /// </summary>
        public PurchaseOrderTaxLineDataHandler(SqlTransaction sqlTrxn)
        {
            log.LogMethodEntry(sqlTrxn);
            sqlTransaction = sqlTrxn;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating PurchaseOrderTaxLineDataHandler Record.
        /// </summary>
        /// <param name="purchaseOrderTaxLineDTO">PurchaseOrderTaxLineDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(PurchaseOrderTaxLineDTO purchaseOrderTaxLineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(purchaseOrderTaxLineDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", purchaseOrderTaxLineDTO.PurchaseOrderTaxLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@purchaseOrderId", purchaseOrderTaxLineDTO.PurchaseOrderId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@purchaseOrderLineId", purchaseOrderTaxLineDTO.PurchaseOrderLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@purchaseTaxId", purchaseOrderTaxLineDTO.PurchaseTaxId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@purchaseTaxName", string.IsNullOrEmpty(purchaseOrderTaxLineDTO.PurchaseTaxName) ? DBNull.Value : (object)purchaseOrderTaxLineDTO.PurchaseTaxName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@taxStructureId", purchaseOrderTaxLineDTO.TaxStructureId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@taxStructureName", string.IsNullOrEmpty(purchaseOrderTaxLineDTO.TaxStructureName) ? DBNull.Value : (object)purchaseOrderTaxLineDTO.TaxStructureName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@productId", purchaseOrderTaxLineDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@taxPercentage", purchaseOrderTaxLineDTO.TaxPercentage <= 0 ? DBNull.Value:(object) purchaseOrderTaxLineDTO.TaxPercentage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@taxAmount", purchaseOrderTaxLineDTO.TaxAmount <= 0 ? DBNull.Value :(object) purchaseOrderTaxLineDTO.TaxAmount, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", purchaseOrderTaxLineDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", purchaseOrderTaxLineDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Updates PurchaseOrderTaxLineDTO
        /// </summary>
        /// <param name="purchaseOrderTaxLineDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        public PurchaseOrderTaxLineDTO Update(PurchaseOrderTaxLineDTO purchaseOrderTaxLineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(purchaseOrderTaxLineDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[PurchaseOrderTaxLine]
                                    SET 
                                                         PurchaseOrderId =  @purchaseOrderId ,
                                                         PurchaseOrderLineId = @purchaseOrderLineId ,
                                                         PurchaseTaxId =   @purchaseTaxId ,
                                                         PurchaseTaxName = @purchaseTaxName,
                                                         TaxStructureId =  @taxStructureId ,
                                                         TaxStructureName =  @taxStructureName ,
                                                         ProductId =   @productId ,
                                                         TaxPercentage =  @taxPercentage ,
                                                         TaxAmount =  @taxAmount ,
                                                         IsActive =  @isActive ,
                                                         --site_id =@site_id ,
                                                         LastUpdateDate = GETDATE(),
                                                         LastUpdatedBy = @lastUpdatedBy,
                                                         MasterEntityId=@masterEntityId

                                                          where Id = @id
                                            SELECT * FROM PurchaseOrderTaxLine WHERE Id = @id";

       

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(purchaseOrderTaxLineDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPurchaseOrderTaxLineDTO(purchaseOrderTaxLineDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating purchaseOrderTaxLineDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(purchaseOrderTaxLineDTO);
            return purchaseOrderTaxLineDTO;
        }

        /// <summary>
        /// Inserts the PurchaseOrderTaxLine Object to Database.
        /// </summary>
        /// <param name="purchaseOrderTaxLineDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <param name="SQLTrx"></param>
        /// <returns>idOfRowInserted</returns>
        public PurchaseOrderTaxLineDTO Insert(PurchaseOrderTaxLineDTO purchaseOrderTaxLineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(purchaseOrderTaxLineDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[PurchaseOrderTaxLine]
                                                        ( 
                                                         PurchaseOrderId ,
                                                         PurchaseOrderLineId ,
                                                         PurchaseTaxId ,
                                                         PurchaseTaxName,
                                                         TaxStructureId ,
                                                         TaxStructureName ,
                                                         ProductId ,
                                                         TaxPercentage ,
                                                         TaxAmount ,
                                                         IsActive ,
                                                         CreatedBy ,
                                                         CreationDate ,
                                                         LastUpdatedBy ,
                                                         LastUpdateDate ,
                                                         Guid,
                                                         site_id,
                                                         MasterEntityId
                                                       
                                                         ) 
                                                values 
                                                        (                                                         
                                                         @purchaseOrderId ,
                                                         @purchaseOrderLineId ,
                                                         @purchaseTaxId ,
                                                         @purchaseTaxName,
                                                         @taxStructureId ,
                                                         @taxStructureName ,
                                                         @productId ,
                                                         @taxPercentage ,
                                                         @taxAmount ,
                                                         @isActive ,
                                                         @createdBy ,
                                                         Getdate() ,
                                                         @lastUpdatedBy ,
                                                         Getdate(),
                                                         NEWID(),
                                                         @site_id,
                                                         @masterEntityId
                                                         ) SELECT* FROM PurchaseOrderTaxLine WHERE Id = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(purchaseOrderTaxLineDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPurchaseOrderTaxLineDTO(purchaseOrderTaxLineDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting purchaseOrderTaxLineDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);

                throw;
            }
            log.LogMethodExit(purchaseOrderTaxLineDTO);
            return purchaseOrderTaxLineDTO;
        }


        /// <summary>
        /// Delete the record from the purchaseOrderTaxLine database based on Id
        /// </summary>
        /// <returns>return the int </returns>
        internal int Delete(int id)
        {
            log.LogMethodEntry(id);
            string query = @"DELETE  
                             FROM PurchaseOrderTaxLine
                             WHERE PurchaseOrderTaxLine.Id = @id";
            SqlParameter parameter = new SqlParameter("@id", id);
            int id1 = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id1);
            return id1;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="purchaseOrderTaxLineDTO">purchaseOrderTaxLineDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshPurchaseOrderTaxLineDTO(PurchaseOrderTaxLineDTO purchaseOrderTaxLineDTO, DataTable dt)
        {
            log.LogMethodEntry(purchaseOrderTaxLineDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                purchaseOrderTaxLineDTO.PurchaseOrderTaxLineId = Convert.ToInt32(dt.Rows[0]["Id"]);
                purchaseOrderTaxLineDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                purchaseOrderTaxLineDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                purchaseOrderTaxLineDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                purchaseOrderTaxLineDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                purchaseOrderTaxLineDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                purchaseOrderTaxLineDTO.site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the PurchaseOrderTaxLine data of passed id 
        /// </summary>
        /// <param name="id">id of PurchaseOrderTaxLine is passed as parameter</param>
        /// <returns>Returns PurchaseOrderTaxLine</returns>
        public PurchaseOrderTaxLineDTO GetPurchaseOrderTaxLineDTO(int id)
        {
            log.LogMethodEntry(id);
            PurchaseOrderTaxLineDTO result = null;
            string query = SELECT_QUERY + @" WHERE pt.Id= @id";
            SqlParameter parameter = new SqlParameter("@id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetPurchaseOrderTaxLineHeaderDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
       

        private PurchaseOrderTaxLineDTO GetPurchaseOrderTaxLineHeaderDTO(DataRow purchaseOrderTaxLineDataRow)
        {
            log.LogMethodEntry(purchaseOrderTaxLineDataRow);
            PurchaseOrderTaxLineDTO purchaseOrderTaxLIneDataObject = new PurchaseOrderTaxLineDTO(Convert.ToInt32(purchaseOrderTaxLineDataRow["Id"]),
                                            purchaseOrderTaxLineDataRow["PurchaseOrderId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderTaxLineDataRow["PurchaseOrderId"]),
                                            purchaseOrderTaxLineDataRow["PurchaseOrderLineId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderTaxLineDataRow["PurchaseOrderLineId"]),
                                            purchaseOrderTaxLineDataRow["PurchaseTaxId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderTaxLineDataRow["PurchaseTaxId"]),
                                            purchaseOrderTaxLineDataRow["PurchaseTaxName"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderTaxLineDataRow["PurchaseTaxName"]),
                                            purchaseOrderTaxLineDataRow["TaxStructureId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderTaxLineDataRow["TaxStructureId"]),
                                            purchaseOrderTaxLineDataRow["TaxStructureName"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderTaxLineDataRow["TaxStructureName"]),
                                            purchaseOrderTaxLineDataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderTaxLineDataRow["ProductId"]),
                                            purchaseOrderTaxLineDataRow["TaxPercentage"] == DBNull.Value ? -1 : Convert.ToDecimal(purchaseOrderTaxLineDataRow["TaxPercentage"]),
                                            purchaseOrderTaxLineDataRow["TaxAmount"] == DBNull.Value ? -1 : Convert.ToDecimal(purchaseOrderTaxLineDataRow["TaxAmount"]),
                                            purchaseOrderTaxLineDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(purchaseOrderTaxLineDataRow["IsActive"]),
                                            purchaseOrderTaxLineDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderTaxLineDataRow["CreatedBy"]),
                                            purchaseOrderTaxLineDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(purchaseOrderTaxLineDataRow["CreationDate"]),
                                            purchaseOrderTaxLineDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderTaxLineDataRow["LastUpdatedBy"]),
                                            purchaseOrderTaxLineDataRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(purchaseOrderTaxLineDataRow["LastupdateDate"]),
                                            purchaseOrderTaxLineDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderTaxLineDataRow["Guid"]),
                                            purchaseOrderTaxLineDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(purchaseOrderTaxLineDataRow["SynchStatus"]),
                                            purchaseOrderTaxLineDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderTaxLineDataRow["site_id"]),
                                            purchaseOrderTaxLineDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderTaxLineDataRow["MasterEntityId"])
                                           );
            log.LogMethodExit(purchaseOrderTaxLIneDataObject);
            return purchaseOrderTaxLIneDataObject;
        }
        /// <summary>
        /// Gets the PurchaseOrderTaxLineDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTrxn">SqlTransaction object</param>
        /// <returns>Returns the list of PurchaseOrderTaxLineDTO matching the search criteria</returns>
        public List<PurchaseOrderTaxLineDTO> GetPurchaseOrderTaxLines(List<KeyValuePair<PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters, string>> searchParameters)
        {

            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<PurchaseOrderTaxLineDTO> purchaseOrderTaxLineDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectPurchaseOrderTaxLineQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key == PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.PO_TAX_LINE_ID
                            || searchParameter.Key == PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.PO_ID
                            || searchParameter.Key == PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.PO_LINE_ID
                            || searchParameter.Key == PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.PRODUCT_ID
                            || searchParameter.Key == PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.PURCHASE_TAX_ID
                            || searchParameter.Key == PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.TAX_STRUCTURE_ID
                            || searchParameter.Key == PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.PURCHASE_TAX_NAME
                            || searchParameter.Key == PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.TAX_STRUCTURE_NAME) 
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }

                        else if (searchParameter.Key == PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.ISACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));

                        }

                        else if (searchParameter.Key == PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.SITEID)
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
                    count++;
                }
                selectPurchaseOrderTaxLineQuery = selectPurchaseOrderTaxLineQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectPurchaseOrderTaxLineQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                purchaseOrderTaxLineDTOList = new List<PurchaseOrderTaxLineDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    PurchaseOrderTaxLineDTO purchaseOrderTaxLineDTO = GetPurchaseOrderTaxLineHeaderDTO(dataRow);
                    purchaseOrderTaxLineDTOList.Add(purchaseOrderTaxLineDTO);
                }
            }
            log.LogMethodExit(purchaseOrderTaxLineDTOList);
            return purchaseOrderTaxLineDTOList;
        }
    }

}


    
