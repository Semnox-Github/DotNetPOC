/********************************************************************************************
 * Project Name - CreditPlusConsumptionRules DataHandler  
 * Description  - CreditPlusConsumptionRules DataHandlers
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By                 Remarks          
 *********************************************************************************************
 *2.70        01-Feb-2019   Indrajeet Kumar             Created
 *            26-Mar-2019   Akshay Gulaganji            modified InsertCreditPlusConsumptionRules(), UpdateCreditPlusConsumptionRules() 
 *                                                      and added GetSQLParameters(), log.MethodEntry(), log.MethodExit() and exceptions 
 *            29-June-2019  Indrajeet Kumar             Created DeleteCreditPlusConsumptionRule() method - for Hard Deletion
 *2.70.2      10-Dec-2019   Jinto Thomas                Removed siteid from update query            
 *2.80.0      04-May-2020   Akshay Gulaganji            Added search parameter - PRODUCT_ID_LIST            
 *2.100.0     23-Oct-2020   Girish Kundar               Modified : Fix for product duplicate passed the sqltransaction to Getby Id method          
 *********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using System.Data;
using System.Data.SqlClient;

namespace Semnox.Parafait.Product
{
    public class CreditPlusConsumptionRulesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<CreditPlusConsumptionRulesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CreditPlusConsumptionRulesDTO.SearchByParameters, string>
        {
            {CreditPlusConsumptionRulesDTO.SearchByParameters.PKID, "Product_id"},
            {CreditPlusConsumptionRulesDTO.SearchByParameters.PRODUCTCREDITPLUS_ID, "ProductCreditPlusId"},
            {CreditPlusConsumptionRulesDTO.SearchByParameters.SITE_ID,"site_id"},
            {CreditPlusConsumptionRulesDTO.SearchByParameters.ISACTIVE,"IsActive"},
            {CreditPlusConsumptionRulesDTO.SearchByParameters.MASTERENTITY_ID,"MasterEntityId"},
            {CreditPlusConsumptionRulesDTO.SearchByParameters.PRODUCT_ID_LIST,"Product_id"}
        };

        public CreditPlusConsumptionRulesDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private CreditPlusConsumptionRulesDTO GetCreditPlusConsumptionRulesDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            try
            {
                CreditPlusConsumptionRulesDTO creditPlusConsumptionRulesDTODataObject = new CreditPlusConsumptionRulesDTO(
                                                    dataRow["PKId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PKId"]),
                                                    dataRow["ProductCreditPlusId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductCreditPlusId"]),
                                                    dataRow["POSTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSTypeId"]),
                                                    dataRow["ExpiryDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["ExpiryDate"]),
                                                    dataRow["Guid"].ToString(),
                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                    dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                    dataRow["GameId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["GameId"]),
                                                    dataRow["GameProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["GameProfileId"]),
                                                    dataRow["Product_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Product_id"]),
                                                    dataRow["Quantity"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Quantity"]),
                                                    dataRow["QuantityLimit"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["QuantityLimit"]),
                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                    dataRow["CategoryId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CategoryId"]),
                                                    dataRow["DiscountAmount"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["DiscountAmount"]),
                                                    dataRow["DiscountPercentage"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["DiscountPercentage"]),
                                                    dataRow["DiscountedPrice"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["DiscountedPrice"]),
                                                    dataRow["OrderTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OrderTypeId"]),
                                                    dataRow["CreatedBy"].ToString(),
                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                    dataRow["LastUpdatedBy"].ToString(),
                                                    dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                    dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])
                                                    );
                log.LogMethodExit(creditPlusConsumptionRulesDTODataObject);
                return creditPlusConsumptionRulesDTODataObject;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while executing Converting the Data row object to creditPlusConsumptionRulesDTO class type", ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating credit Plus Consumption Rules record.
        /// </summary>
        /// <param name="CreditPlusConsumptionRulesDTO">ProductCreditPlusDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CreditPlusConsumptionRulesDTO creditPlusConsumptionRulesDTO, string userId, int siteId)
        {
            log.LogMethodEntry(creditPlusConsumptionRulesDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@PKId", creditPlusConsumptionRulesDTO.PKId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductCreditPlusId", creditPlusConsumptionRulesDTO.ProductCreditPlusId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSTypeId", creditPlusConsumptionRulesDTO.POSTypeId, true));
            if (creditPlusConsumptionRulesDTO.ExpiryDate == DateTime.MinValue)//If the ExpiryDate is Minimum date then it will insert null value in Database table
            {
                parameters.Add(dataAccessHandler.GetSQLParameter("@ExpiryDate", DBNull.Value));
            }
            else
            {
                parameters.Add(dataAccessHandler.GetSQLParameter("@ExpiryDate", creditPlusConsumptionRulesDTO.ExpiryDate));
            }
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GameId", creditPlusConsumptionRulesDTO.GameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GameProfileId", creditPlusConsumptionRulesDTO.GameProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Product_id", creditPlusConsumptionRulesDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Quantity", creditPlusConsumptionRulesDTO.Quantity, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@QuantityLimit", creditPlusConsumptionRulesDTO.QuantityLimit, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", creditPlusConsumptionRulesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CategoryId", creditPlusConsumptionRulesDTO.CategoryId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DiscountAmount", creditPlusConsumptionRulesDTO.DiscountAmount, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DiscountPercentage", creditPlusConsumptionRulesDTO.DiscountPercentage, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DiscountedPrice", creditPlusConsumptionRulesDTO.DiscountedPrice, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OrderTypeId", creditPlusConsumptionRulesDTO.OrderTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", creditPlusConsumptionRulesDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Product CreditPlus record to the database
        /// </summary>
        /// <param name="creditPlusConsumptionRulesDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public int InsertCreditPlusConsumptionRules(CreditPlusConsumptionRulesDTO creditPlusConsumptionRulesDTO, string userId, int siteId)
        {
            log.LogMethodEntry(creditPlusConsumptionRulesDTO, userId, siteId);
            int idOfRowInserted;
            string insertProductCreditPlusConsumptionQuery = @"Insert into ProductCreditPlusConsumption
                                                                        (
                                                                            ProductCreditPlusId,
                                                                            POSTypeId,
                                                                            ExpiryDate,
                                                                            Guid,
                                                                            site_id,
                                                                            GameId,
                                                                            GameProfileId,
                                                                            Product_id,
                                                                            Quantity,
                                                                            QuantityLimit,
                                                                            MasterEntityId,
                                                                            CategoryId,
                                                                            DiscountAmount,
                                                                            DiscountPercentage,
                                                                            DiscountedPrice,
                                                                            OrderTypeId,
                                                                            CreatedBy,
                                                                            CreationDate,
                                                                            LastUpdatedBy,
                                                                            LastUpdateDate,
                                                                            IsActive
                                                                        )
                                                                         values
                                                                        (
                                                                            @ProductCreditPlusId,
                                                                            @POSTypeId,
                                                                            @ExpiryDate,
                                                                            NEWID(),
                                                                            @site_id,
                                                                            @GameId,
                                                                            @GameProfileId,
                                                                            @Product_id,
                                                                            @Quantity,
                                                                            @QuantityLimit,
                                                                            @MasterEntityId,
                                                                            @CategoryId,
                                                                            @DiscountAmount,
                                                                            @DiscountPercentage,
                                                                            @DiscountedPrice,
                                                                            @OrderTypeId,
                                                                            @LastUpdatedBy,
                                                                            getdate(),
                                                                            @LastUpdatedBy,
                                                                            getdate(),
                                                                            @IsActive
                                                                        )SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(insertProductCreditPlusConsumptionQuery, GetSQLParameters(creditPlusConsumptionRulesDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }

        /// <summary>
        /// Update the CreditPlusConsumptionRules record to the database
        /// </summary>
        /// <param name="creditPlusConsumptionRulesDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public int UpdateCreditPlusConsumptionRules(CreditPlusConsumptionRulesDTO creditPlusConsumptionRulesDTO, string userId, int siteId)
        {
            log.LogMethodEntry(creditPlusConsumptionRulesDTO, userId, siteId);
            int rowsUpdated;
            string UpdateProductCreditPlusConsumptionQuery = @" Update ProductCreditPlusConsumption 
                                                                                        set 
                                                                                                ProductCreditPlusId     =   @ProductCreditPlusId,
                                                                                                POSTypeId	            =   @POSTypeId,
                                                                                                ExpiryDate	            =   @ExpiryDate,                                                                                              
                                                                                                -- site_id	                =   @site_id,
                                                                                                GameId	                =   @GameId,
                                                                                                GameProfileId	        =   @GameProfileId,
                                                                                                Product_id	            =   @Product_id,
                                                                                                Quantity	            =   @Quantity,
                                                                                                QuantityLimit	        =	@QuantityLimit,
                                                                                                MasterEntityId	        =	@MasterEntityId,
                                                                                                CategoryId	            =	@CategoryId	,
                                                                                                DiscountAmount	        =	@DiscountAmount	,
                                                                                                DiscountPercentage	    =	@DiscountPercentage	,
                                                                                                DiscountedPrice	        =	@DiscountedPrice,
                                                                                                OrderTypeId	            =	@OrderTypeId,
                                                                                                IsActive                =   @IsActive
                                                                                        where 
                                                                                                ProductCreditPlusId =@ProductCreditPlusId 
                                                                                        and 
                                                                                                PKId = @PKId";
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(UpdateProductCreditPlusConsumptionQuery, GetSQLParameters(creditPlusConsumptionRulesDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }


        /// <summary>
        /// Gets the CreditPlusConsumptionRules
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<CreditPlusConsumptionRulesDTO> GetAllCreditPlusConsumptionRulesList(List<KeyValuePair<CreditPlusConsumptionRulesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;

            string selectCreditPlusConsumptionRulesQuery = @"select * from  ProductCreditPlusConsumption";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<CreditPlusConsumptionRulesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == CreditPlusConsumptionRulesDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joinOperartor + " (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key.Equals(CreditPlusConsumptionRulesDTO.SearchByParameters.PKID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "' ");
                        }
                        else if (searchParameter.Key.Equals(CreditPlusConsumptionRulesDTO.SearchByParameters.PRODUCTCREDITPLUS_ID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " = '" + Int32.Parse(searchParameter.Value) + "' ");
                        }
                        else if (searchParameter.Key.Equals(CreditPlusConsumptionRulesDTO.SearchByParameters.PRODUCT_ID_LIST))// value - product_ID List 
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " IN ("+ searchParameter.Value + ") ");
                        }
                        else if (searchParameter.Key == CreditPlusConsumptionRulesDTO.SearchByParameters.ISACTIVE)
                        {
                            query.Append(joinOperartor + " ISNULL(" + DBSearchParameters[searchParameter.Key] + ", '1')  = '" + searchParameter.Value + "'");
                        }
                        else
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.Error("The query parameter does not exist " + searchParameter.Key);
                        log.LogMethodExit(null, "The query parameter does not exist " + searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectCreditPlusConsumptionRulesQuery = selectCreditPlusConsumptionRulesQuery + query;
                selectCreditPlusConsumptionRulesQuery = selectCreditPlusConsumptionRulesQuery + "Order by PKId";
            }
            DataTable creditPlusConsumptionRulesDataTable = dataAccessHandler.executeSelectQuery(selectCreditPlusConsumptionRulesQuery, null, sqlTransaction);
            List<CreditPlusConsumptionRulesDTO> creditPlusConsumptionRulesList = new List<CreditPlusConsumptionRulesDTO>();
            if (creditPlusConsumptionRulesDataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in creditPlusConsumptionRulesDataTable.Rows)
                {
                    CreditPlusConsumptionRulesDTO creditPlusConsumptionRulesObject = GetCreditPlusConsumptionRulesDTO(dataRow);
                    creditPlusConsumptionRulesList.Add(creditPlusConsumptionRulesObject);
                }
            }
            log.LogMethodExit(creditPlusConsumptionRulesList);
            return creditPlusConsumptionRulesList;
        }
        /// <summary>
        /// Gets the CreditPlusConsumptionRulesDTO data of passed productCreditPlusId Id
        /// </summary>
        /// <param name="pKId">integer type parameter</param>
        /// <returns>Returns CreditPlusConsumptionRulesDTO</returns>
        public CreditPlusConsumptionRulesDTO GetCreditPlusConsumptionRulesDTO(int pKId)
        {
            log.LogMethodEntry(pKId);
            string selectCreditPlusConsumptionRulesQuery = @"select *
                                         from ProductCreditPlusConsumption
                                        where PKId = @pKId";

            SqlParameter[] selectCreditPlusConsumptionRulesParameters = new SqlParameter[1];
            selectCreditPlusConsumptionRulesParameters[0] = new SqlParameter("@pKId", pKId);
            DataTable creditPlusConsumptionRulesRow = dataAccessHandler.executeSelectQuery(selectCreditPlusConsumptionRulesQuery, selectCreditPlusConsumptionRulesParameters,sqlTransaction);
            if (creditPlusConsumptionRulesRow.Rows.Count > 0)
            {
                DataRow creditPlusConsumptionRulesDataRow = creditPlusConsumptionRulesRow.Rows[0];
                CreditPlusConsumptionRulesDTO creditPlusConsumptionRulesObject = GetCreditPlusConsumptionRulesDTO(creditPlusConsumptionRulesDataRow);
                log.LogMethodExit(creditPlusConsumptionRulesObject);
                return creditPlusConsumptionRulesObject;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }
        /// <summary>
        /// Delete the CreditPlusConsumption based on the pkId
        /// </summary>
        /// <param name="pkId"></param>
        /// <returns></returns>
        public int DeleteCreditPlusConsumptionRule(int pkId)
        {
            log.LogMethodEntry(pkId);
            try
            {
                string deleteQuery = @"delete from ProductCreditPlusConsumption where PKId = @pkId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@pkId", pkId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
        internal List<CreditPlusConsumptionRulesDTO> GetCreditPlusConsumptionRulesDTOList(List<int> productCreditPlusIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(productCreditPlusIdList);
            List<CreditPlusConsumptionRulesDTO> creditPlusConsumptionRulesDTOList = new List<CreditPlusConsumptionRulesDTO>();
            string query = @"SELECT *
                            FROM ProductCreditPlusConsumption, @productCreditPlusIdList List
                            WHERE ProductCreditPlusId = List.Id ";
            if (activeRecords)
            {
                query += " AND (IsActive = 1 or IsActive is null) ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@productCreditPlusIdList", productCreditPlusIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                creditPlusConsumptionRulesDTOList = table.Rows.Cast<DataRow>().Select(x => GetCreditPlusConsumptionRulesDTO(x)).ToList();
            }
            log.LogMethodExit(creditPlusConsumptionRulesDTOList);
            return creditPlusConsumptionRulesDTOList;
        }
    }
}
