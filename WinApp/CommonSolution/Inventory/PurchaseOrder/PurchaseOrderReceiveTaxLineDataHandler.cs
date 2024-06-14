/********************************************************************************************
 * Project Name - PurchaseOrderReceiveTaxLineDataHandler
 * Description  -Data Handler
 * 
 **************
 **Version Log
 **************
 *Version      Date             Modified By        Remarks          
 **********************************************************************************************
 *2.60        11-Apr-2019       Girish Kundar     Created
 *2.70.2        16-jul-2019       Deeksha           Modified:Added GetSqlParameter(),SQL injection issue Fix
 *2.70.2        09-Dec-2019   Jinto Thomas     Removed siteid from update query 
 *2.110.0     29-Dec-2020     Prajwal S         Added : GetPurchaseOrderReceiveTaxLineDTOList.
  ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Inventory
{
    class PurchaseOrderReceiveTaxLineDataHandler
    {
        
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM PurchaseOrderReceiveTaxLine AS po ";
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  Dictionary for searching Parameters for the PurchaseOrderReceiveTaxLine object.
        /// </summary>
        private static readonly Dictionary<PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters, string> DBSearchParameters = new Dictionary<PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters, string>
            {
                {PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters.PO_RECV_TAX_LINE_ID, "po.purchaseOrderReceiveLineId" },
                {PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters.PO_RECEIVE_LINE_ID, "po.purchaseOrderReceiveLineId"},
                {PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters.RECEIPT_ID, "po.receiptId"},
                {PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters.TAX_STRUCTURE_ID, "po.taxStructureId"},
                {PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters.PRODUCT_ID, "po.productId"},
                {PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters.PURCHASE_TAX_ID, "po.purchaseTaxId"},
                {PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters.PURCHASE_TAX_NAME, "po.purchaseTaxName"},
                {PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters.ISACTIVE_FLAG, "po.isActive"},
                {PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters.SITEID, "po.Site_id"},
                {PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters.MASTER_ENTIT_ID, "po.MasterEntityId"},
            };
    

        /// <summary>
        /// Default constructor of PurchaseOrderTaxLineDataHandler class
        /// </summary>
        public PurchaseOrderReceiveTaxLineDataHandler(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            this.sqlTransaction = sqlTrx;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating PurchaseOrderReceiveTaxLineDataHandler Record.
        /// </summary>
        /// <param name="purchaseOrderReceiveTaxLineDTO">PurchaseOrderReceiveTaxLineDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(PurchaseOrderReceiveTaxLineDTO purchaseOrderReceiveTaxLineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(purchaseOrderReceiveTaxLineDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", purchaseOrderReceiveTaxLineDTO.PurchaseOrderReceiveTaxLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@receiptId", purchaseOrderReceiveTaxLineDTO.ReceiptId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@purchaseOrderReceiveLineId", purchaseOrderReceiveTaxLineDTO.PurchaseOrderReceiveLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@purchaseTaxId", purchaseOrderReceiveTaxLineDTO.PurchaseTaxId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@purchaseTaxName", string.IsNullOrEmpty(purchaseOrderReceiveTaxLineDTO.PurchaseTaxName) ? DBNull.Value : (object)purchaseOrderReceiveTaxLineDTO.PurchaseTaxName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@taxStructureId", purchaseOrderReceiveTaxLineDTO.TaxStructureId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@taxStructureName", string.IsNullOrEmpty(purchaseOrderReceiveTaxLineDTO.TaxStructureName) ? DBNull.Value : (object)purchaseOrderReceiveTaxLineDTO.TaxStructureName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@productId", purchaseOrderReceiveTaxLineDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@taxPercentage", string.IsNullOrEmpty(purchaseOrderReceiveTaxLineDTO.TaxPercentage.ToString()) ? DBNull.Value : (object)purchaseOrderReceiveTaxLineDTO.TaxPercentage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@taxAmount", string.IsNullOrEmpty(purchaseOrderReceiveTaxLineDTO.TaxAmount.ToString()) ? DBNull.Value : (object)purchaseOrderReceiveTaxLineDTO.TaxAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", purchaseOrderReceiveTaxLineDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", purchaseOrderReceiveTaxLineDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Updates the PurchaseOrderReceiveTaxLineDTO DTO objects with IsActive flag is set to false.
        /// </summary>
        /// <param name="purchaseOrderTaxLineDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="SQLTrx"></param>
        public PurchaseOrderReceiveTaxLineDTO Update(PurchaseOrderReceiveTaxLineDTO purchaseOrderReceiveTaxLineDTO , string loginId, int siteId)
        {
            log.LogMethodEntry(purchaseOrderReceiveTaxLineDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[PurchaseOrderReceiveTaxLine]
                                    SET 
                                                         ReceiptId =  @receiptId,
                                                         PurchaseOrderReceiveLineId = @purchaseOrderReceiveLineId ,
                                                         PurchaseTaxId =  @purchaseTaxId ,
                                                         PurchaseTaxName =  @purchaseTaxName,
                                                         TaxStructureId =   @taxStructureId ,
                                                         TaxStructureName =  @taxStructureName,
                                                         ProductId =  @productId,
                                                         TaxPercentage =  @taxPercentage ,
                                                         TaxAmount =   @taxAmount  ,
                                                         IsActive =   @isActive ,
                                                         LastUpdatedBy = @lastUpdatedBy ,
                                                         --site_id = @site_id,
                                                         LastUpdateDate  = GETDATE(),
                                                         MasterEntityId = @masterEntityId
                                                         where Id = @id
                                                SELECT* FROM PurchaseOrderReceiveTaxLine WHERE Id = @id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(purchaseOrderReceiveTaxLineDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPurchaseOrderReceiveTaxLineDTO(purchaseOrderReceiveTaxLineDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating purchaseOrderReceiveTaxLineDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(purchaseOrderReceiveTaxLineDTO);
            return purchaseOrderReceiveTaxLineDTO;
        }

        /// <summary>
        /// Inserts the PurchaseOrderTaxLine Object to Database.
        /// </summary>
        /// <param name="PurchaseOrderReceiveTaxLineDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <param name="SQLTrx"></param>
        /// <returns>idOfRowInserted </returns>

        public PurchaseOrderReceiveTaxLineDTO Insert(PurchaseOrderReceiveTaxLineDTO purchaseOrderReceiveTaxLineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(purchaseOrderReceiveTaxLineDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[PurchaseOrderReceiveTaxLine]
                                                        ( 
                                                         ReceiptId ,
                                                         PurchaseOrderReceiveLineId ,
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
                                                         @receiptId ,
                                                         @purchaseOrderReceiveLineId ,
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
                                                         @createdBy ,
                                                         Getdate(),
                                                         NEWID(),
                                                         @site_id,
                                                         @masterEntityId
                                                         )SELECT* FROM PurchaseOrderReceiveTaxLine WHERE Id = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(purchaseOrderReceiveTaxLineDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPurchaseOrderReceiveTaxLineDTO(purchaseOrderReceiveTaxLineDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting purchaseOrderReceiveTaxLineDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);

                throw;
            }
            log.LogMethodExit(purchaseOrderReceiveTaxLineDTO);
            return purchaseOrderReceiveTaxLineDTO;
        }


        /// <summary>
        /// Delete the record from the PurchaseOrderReceiveTaxLine database based on Id
        /// </summary>
        /// <returns>return the int </returns>
        internal int Delete(int id)
        {
            log.LogMethodEntry(id);
            string query = @"DELETE  
                             FROM PurchaseOrderReceiveTaxLine
                             WHERE PurchaseOrderReceiveTaxLine.Id = @id";
            SqlParameter parameter = new SqlParameter("@id", id);
            int id1 = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id1);
            return id1;
        }

        /// <summary>
        /// Gets the PurchaseOrderReceiveTaxLineDTO List for purchaseOrderReceiveLineIdList
        /// </summary>
        /// <param name="purchaseOrderReceiveLineIdList">integer list parameter</param>
        /// <returns>Returns List of PurchaseOrderReceiveTaxLineDTO List</returns>
        public List<PurchaseOrderReceiveTaxLineDTO> GetPurchaseOrderReceiveTaxLineDTOList(List<int> purchaseOrderReceiveLineIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(purchaseOrderReceiveLineIdList);
            List<PurchaseOrderReceiveTaxLineDTO> list = new List<PurchaseOrderReceiveTaxLineDTO>();
            string query = @"SELECT PurchaseOrderReceiveTaxLine.*
                            FROM PurchaseOrderReceiveTaxLine, @purchaseOrderReceiveLineIdList List
                            WHERE PurchaseOrderReceiveLineId = List.Id ";
            if (activeRecords)
            {
                query += " AND IsActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@purchaseOrderReceiveLineIdList", purchaseOrderReceiveLineIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetPurchaseOrderReceiveTaxLineHeaderDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="achievementClassDTO">AchievementClassDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshPurchaseOrderReceiveTaxLineDTO(PurchaseOrderReceiveTaxLineDTO purchaseOrderReceiveTaxLineDTO, DataTable dt)
        {
            log.LogMethodEntry(purchaseOrderReceiveTaxLineDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                purchaseOrderReceiveTaxLineDTO.PurchaseOrderReceiveTaxLineId = Convert.ToInt32(dt.Rows[0]["Id"]);
                purchaseOrderReceiveTaxLineDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                purchaseOrderReceiveTaxLineDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                purchaseOrderReceiveTaxLineDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                purchaseOrderReceiveTaxLineDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                purchaseOrderReceiveTaxLineDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                purchaseOrderReceiveTaxLineDTO.site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the PurchaseOrderReceiveTaxLineDTO data of passed id 
        /// </summary>
        /// <param name="id">id of PurchaseOrderReceiveTaxLineDTO is passed as parameter</param>
        /// <returns>Returns PurchaseOrderReceiveTaxLineDTO</returns>
        public PurchaseOrderReceiveTaxLineDTO PurchaseOrderReceiveTaxLine(int id)
        {
            log.LogMethodEntry(id);
            PurchaseOrderReceiveTaxLineDTO result = null;
            string query = SELECT_QUERY + @" WHERE po.Id= @id";
            SqlParameter parameter = new SqlParameter("@id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetPurchaseOrderReceiveTaxLineHeaderDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Converts the Data row object to PurchaseOrderReceiveTaxLineDTO class type
        /// </summary>
        /// <param name="purchaseOrderReceiveTaxLineDataRow">PurchaseOrderReceiveTaxLine DataRow</param>
        /// <returns>Returns purchaseOrderReceiveTaxLine</returns>
        private PurchaseOrderReceiveTaxLineDTO GetPurchaseOrderReceiveTaxLineHeaderDTO(DataRow purchaseOrderReceiveTaxLineDataRow)
        {
            log.LogMethodEntry(purchaseOrderReceiveTaxLineDataRow);
            PurchaseOrderReceiveTaxLineDTO purchaseOrderTaxLIneDataObject = new PurchaseOrderReceiveTaxLineDTO(Convert.ToInt32(purchaseOrderReceiveTaxLineDataRow["Id"]),
                                            purchaseOrderReceiveTaxLineDataRow["ReceiptId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderReceiveTaxLineDataRow["ReceiptId"]),
                                            purchaseOrderReceiveTaxLineDataRow["PurchaseOrderReceiveLineId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderReceiveTaxLineDataRow["PurchaseOrderReceiveLineId"]),
                                            purchaseOrderReceiveTaxLineDataRow["PurchaseTaxId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderReceiveTaxLineDataRow["PurchaseTaxId"]),
                                            purchaseOrderReceiveTaxLineDataRow["PurchaseTaxName"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderReceiveTaxLineDataRow["PurchaseTaxName"]),
                                            purchaseOrderReceiveTaxLineDataRow["TaxStructureId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderReceiveTaxLineDataRow["TaxStructureId"]),
                                            purchaseOrderReceiveTaxLineDataRow["TaxStructureName"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderReceiveTaxLineDataRow["TaxStructureName"]),
                                            purchaseOrderReceiveTaxLineDataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderReceiveTaxLineDataRow["ProductId"]),
                                            purchaseOrderReceiveTaxLineDataRow["TaxPercentage"] == DBNull.Value ? -1 : Convert.ToDecimal(purchaseOrderReceiveTaxLineDataRow["TaxPercentage"]),
                                            purchaseOrderReceiveTaxLineDataRow["TaxAmount"] == DBNull.Value ? -1 : Convert.ToDecimal(purchaseOrderReceiveTaxLineDataRow["TaxAmount"]),
                                            purchaseOrderReceiveTaxLineDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(purchaseOrderReceiveTaxLineDataRow["IsActive"]),
                                            purchaseOrderReceiveTaxLineDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderReceiveTaxLineDataRow["CreatedBy"]),
                                            purchaseOrderReceiveTaxLineDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(purchaseOrderReceiveTaxLineDataRow["CreationDate"]),
                                            purchaseOrderReceiveTaxLineDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderReceiveTaxLineDataRow["LastUpdatedBy"]),
                                            purchaseOrderReceiveTaxLineDataRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(purchaseOrderReceiveTaxLineDataRow["LastupdateDate"]),
                                            purchaseOrderReceiveTaxLineDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(purchaseOrderReceiveTaxLineDataRow["Guid"]),
                                            purchaseOrderReceiveTaxLineDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(purchaseOrderReceiveTaxLineDataRow["SynchStatus"]),
                                            purchaseOrderReceiveTaxLineDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderReceiveTaxLineDataRow["site_id"]),
                                            purchaseOrderReceiveTaxLineDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseOrderReceiveTaxLineDataRow["MasterEntityId"])
                                           );
            log.LogMethodExit(purchaseOrderTaxLIneDataObject);
            return purchaseOrderTaxLIneDataObject;
        }
        /// <summary>
        /// Gets the PurchaseOrderReceiveTaxLineDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTrxn">SqlTransaction object</param>
        /// <returns>Returns the list of PurchaseOrderTaxLineDTO matching the search criteria</returns>
        public List<PurchaseOrderReceiveTaxLineDTO> GetPurchaseOrderReceiveTaxLines(List<KeyValuePair<PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<PurchaseOrderReceiveTaxLineDTO> purchaseOrderReceiveTaxLineDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectPurchaseOrderTaxLineQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key == PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters.PO_RECV_TAX_LINE_ID
                            || searchParameter.Key == PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters.PO_RECEIVE_LINE_ID
                            || searchParameter.Key == PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters.RECEIPT_ID
                            || searchParameter.Key == PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters.PRODUCT_ID
                            || searchParameter.Key == PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters.PURCHASE_TAX_ID
                            || searchParameter.Key == PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters.MASTER_ENTIT_ID
                            || searchParameter.Key == PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters.TAX_STRUCTURE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters.PURCHASE_TAX_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters.ISACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters.SITEID)
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
                purchaseOrderReceiveTaxLineDTOList = new List<PurchaseOrderReceiveTaxLineDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    PurchaseOrderReceiveTaxLineDTO purchaseOrderReceiveTaxLineDTO = GetPurchaseOrderReceiveTaxLineHeaderDTO(dataRow);
                    purchaseOrderReceiveTaxLineDTOList.Add(purchaseOrderReceiveTaxLineDTO);
                }
            }
            log.LogMethodExit(purchaseOrderReceiveTaxLineDTOList);
            return purchaseOrderReceiveTaxLineDTOList;
        }
    }

}

