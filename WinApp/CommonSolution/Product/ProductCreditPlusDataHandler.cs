/********************************************************************************************
 * Project Name - ProductCreditPlus Data Handler
 * Description  - Data handler of the ProductCreditPlus
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By              Remarks          
 **********************************************************************************************
 *2.70        31-Jan-2019      Indrajeet Kumar          Created 
 *            26-Mar-2019      Akshay Gulaganji         modified InsertProductCreditPlus(), UpdateProductCreditPlus() 
 *                                                      and added GetSQLParameters(), log.MethodEntry(), log.MethodExit() and exceptions
 *            29-June-2019     Indrajeet Kumar          Created DeleteProductCreditPlus() for Hard Deletion.
 *2.70.2      10-Dec-2019      Jinto Thomas            Removed siteid from update query
 *2.80.0      03-Feb-2020      Girish Kundar            Added EffectiveAfterMinutes field to the table and Modified as per 3 tier standard
 *2.80.0      04-May-2020      Akshay Gulaganji         Added search parameter - PRODUCTCREDITPLUS_ID_LIST  
 **********************************************************************************************/
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
    public class ProductCreditPlusDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;

        private static readonly Dictionary<ProductCreditPlusDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ProductCreditPlusDTO.SearchByParameters, string>
        {
            {ProductCreditPlusDTO.SearchByParameters.PRODUCT_ID, "Product_id"},
            {ProductCreditPlusDTO.SearchByParameters.PRODUCTCREDITPLUS_ID, "ProductCreditPlusId"},
            {ProductCreditPlusDTO.SearchByParameters.SITE_ID,"site_id"},
            {ProductCreditPlusDTO.SearchByParameters.ISACTIVE,"IsActive"},
            {ProductCreditPlusDTO.SearchByParameters.MASTERENTITY_ID,"MasterEntityId"},
             {ProductCreditPlusDTO.SearchByParameters.PRODUCT_ID_LIST, "Product_id"},
            {ProductCreditPlusDTO.SearchByParameters.PRODUCTCREDITPLUS_ID_LIST,"ProductCreditPlusId"}
        };
        /// <summary>
        /// default Constructor
        /// </summary>
        public ProductCreditPlusDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ProductCreditPlusDTO class type
        /// </summary>
        /// <param name="productCreditPlusDataRow">ProductCreditPlusDTO DataRow</param>
        /// <returns>ProductCreditPlusDTO</returns>
        private ProductCreditPlusDTO GetProductCreditPlusDTO(DataRow productCreditPlusDataRow)
        {
            log.LogMethodEntry(productCreditPlusDataRow);
            try
            {
                ProductCreditPlusDTO productCreditPlusDTODataObject = new ProductCreditPlusDTO(
                                                Convert.ToInt32(productCreditPlusDataRow["ProductCreditPlusId"]),
                                                productCreditPlusDataRow["CreditPlus"] == DBNull.Value ? 0 : Convert.ToDecimal(productCreditPlusDataRow["CreditPlus"]),
                                                productCreditPlusDataRow["Refundable"].ToString(),
                                                productCreditPlusDataRow["Remarks"].ToString(),
                                                Convert.ToInt32(productCreditPlusDataRow["Product_id"]),
                                                productCreditPlusDataRow["CreditPlusType"].ToString(),
                                                productCreditPlusDataRow["Guid"].ToString(),
                                                productCreditPlusDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(productCreditPlusDataRow["site_id"]),
                                                productCreditPlusDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(Convert.ToInt32(productCreditPlusDataRow["SynchStatus"])),
                                                productCreditPlusDataRow["PeriodFrom"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(productCreditPlusDataRow["PeriodFrom"]),
                                                productCreditPlusDataRow["PeriodTo"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(productCreditPlusDataRow["PeriodTo"]),
                                                productCreditPlusDataRow["ValidForDays"] == DBNull.Value ? -1 : Convert.ToInt32(productCreditPlusDataRow["ValidForDays"]),
                                                productCreditPlusDataRow["ExtendOnReload"].ToString(),
                                                productCreditPlusDataRow["TimeFrom"] == DBNull.Value ? -1 : Convert.ToDecimal(productCreditPlusDataRow["TimeFrom"]),
                                                productCreditPlusDataRow["TimeTo"] == DBNull.Value ? -1 : Convert.ToDecimal(productCreditPlusDataRow["TimeTo"]),
                                                productCreditPlusDataRow["Minutes"] == DBNull.Value ? -1 : Convert.ToInt32(productCreditPlusDataRow["Minutes"]),
                                                productCreditPlusDataRow["Monday"] == DBNull.Value ? false : Convert.ToBoolean(Convert.ToInt32(productCreditPlusDataRow["Monday"])),
                                                productCreditPlusDataRow["Tuesday"] == DBNull.Value ? false : Convert.ToBoolean(Convert.ToInt32(productCreditPlusDataRow["Tuesday"])),
                                                productCreditPlusDataRow["Wednesday"] == DBNull.Value ? false : Convert.ToBoolean(Convert.ToInt32(productCreditPlusDataRow["Wednesday"])),
                                                productCreditPlusDataRow["Thursday"] == DBNull.Value ? false : Convert.ToBoolean(Convert.ToInt32(productCreditPlusDataRow["Thursday"])),
                                                productCreditPlusDataRow["Friday"] == DBNull.Value ? false : Convert.ToBoolean(Convert.ToInt32(productCreditPlusDataRow["Friday"])),
                                                productCreditPlusDataRow["Saturday"] == DBNull.Value ? false : Convert.ToBoolean(Convert.ToInt32(productCreditPlusDataRow["Saturday"])),
                                                productCreditPlusDataRow["Sunday"] == DBNull.Value ? false : Convert.ToBoolean(Convert.ToInt32(productCreditPlusDataRow["Sunday"])),
                                                productCreditPlusDataRow["TicketAllowed"] == DBNull.Value ? false : Convert.ToBoolean(Convert.ToInt32(productCreditPlusDataRow["TicketAllowed"])),
                                                productCreditPlusDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(productCreditPlusDataRow["MasterEntityId"]),
                                                productCreditPlusDataRow["Frequency"].ToString(),
                                                productCreditPlusDataRow["PauseAllowed"] == DBNull.Value ? true : Convert.ToBoolean(productCreditPlusDataRow["PauseAllowed"]),
                                                productCreditPlusDataRow["CreatedBy"].ToString(),
                                                productCreditPlusDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(productCreditPlusDataRow["CreationDate"]),
                                                productCreditPlusDataRow["LastUpdatedBy"].ToString(),
                                                productCreditPlusDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(productCreditPlusDataRow["LastUpdateDate"]),
                                                productCreditPlusDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(productCreditPlusDataRow["IsActive"]),
                                                productCreditPlusDataRow["EffectiveAfterMinutes"] == DBNull.Value ? 0 : Convert.ToInt32(productCreditPlusDataRow["EffectiveAfterMinutes"])
                                                );
                log.LogMethodExit(productCreditPlusDTODataObject);
                return productCreditPlusDTODataObject;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing Converting the Data row object to ProductCreditPlusDTO class type", ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Product CreditPlus Record.
        /// </summary>
        /// <param name="ProductCreditPlusDTO">ProductCreditPlusDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ProductCreditPlusDTO productCreditPlusDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productCreditPlusDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductCreditPlusId", productCreditPlusDTO.ProductCreditPlusId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreditPlus", productCreditPlusDTO.CreditPlus, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Refundable", (productCreditPlusDTO.Refundable.ToUpper() == "YES" || productCreditPlusDTO.Refundable.ToUpper() == "Y") ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Remarks", productCreditPlusDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Product_id", productCreditPlusDTO.Product_id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreditPlusType", productCreditPlusDTO.CreditPlusType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PeriodFrom", productCreditPlusDTO.PeriodFrom));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PeriodTo", productCreditPlusDTO.PeriodTo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ValidForDays", productCreditPlusDTO.ValidForDays, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExtendOnReload", (productCreditPlusDTO.ExtendOnReload.ToUpper() == "YES" || productCreditPlusDTO.ExtendOnReload.ToUpper() == "Y") ? "Y" : "N"));
            if (productCreditPlusDTO.TimeFrom == -1)
            {
                parameters.Add(dataAccessHandler.GetSQLParameter("@TimeFrom", DBNull.Value));
            }
            else
            {
                parameters.Add(dataAccessHandler.GetSQLParameter("@TimeFrom", productCreditPlusDTO.TimeFrom));
            }
            if (productCreditPlusDTO.TimeTo == -1)
            {
                parameters.Add(dataAccessHandler.GetSQLParameter("@TimeTo", DBNull.Value));
            }
            else
            {
                parameters.Add(dataAccessHandler.GetSQLParameter("@TimeTo", productCreditPlusDTO.TimeTo));
            }            
            parameters.Add(dataAccessHandler.GetSQLParameter("@Minutes", productCreditPlusDTO.Minutes, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Monday", productCreditPlusDTO.Monday));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Tuesday", productCreditPlusDTO.Tuesday));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Wednesday", productCreditPlusDTO.Wednesday));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Thursday", productCreditPlusDTO.Thursday));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Friday", productCreditPlusDTO.Friday));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Saturday", productCreditPlusDTO.Saturday));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Sunday", productCreditPlusDTO.Sunday));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TicketAllowed", productCreditPlusDTO.TicketAllowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", productCreditPlusDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Frequency", productCreditPlusDTO.Frequency));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PauseAllowed", productCreditPlusDTO.PauseAllowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", productCreditPlusDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EffectiveAfterMinutes", productCreditPlusDTO.EffectiveAfterMinutes));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Inserts the Product CreditPlus record to the database
        /// </summary>
        /// <param name="productCreditPlusDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public ProductCreditPlusDTO InsertProductCreditPlus(ProductCreditPlusDTO productCreditPlusDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productCreditPlusDTO, loginId, siteId);
            string insertProductCreditPlusQuery = @"Insert into ProductCreditPlus 
                                                         (
                                                          CreditPlus,
                                                          Refundable,
                                                          Remarks,
                                                          Product_id,
                                                          CreditPlusType,
                                                          Guid,
                                                          site_id,
                                                          PeriodFrom,
                                                          PeriodTo,
                                                          ValidForDays,
                                                          ExtendOnReload,
                                                          TimeFrom,
                                                          TimeTo,
                                                          Minutes,
                                                          Monday,
                                                          Tuesday,
                                                          Wednesday,
                                                          Thursday,
                                                          Friday,
                                                          Saturday,
                                                          Sunday,
                                                          TicketAllowed,
                                                          MasterEntityId,
                                                          Frequency,
                                                          PauseAllowed,
                                                          CreatedBy,
                                                          CreationDate,
                                                          LastUpdatedBy,
                                                          LastUpdateDate,
                                                          IsActive,
                                                          EffectiveAfterMinutes
                                                         ) 
                                                         values
                                                         (
                                                          @CreditPlus,
                                                          @Refundable,
                                                          @Remarks,
                                                          @Product_id,
                                                          @CreditPlusType,
                                                          NEWID(),
                                                          @site_id,
                                                          @PeriodFrom,
                                                          @PeriodTo,
                                                          @ValidForDays,
                                                          @ExtendOnReload,
                                                          @TimeFrom,
                                                          @TimeTo,
                                                          @Minutes,
                                                          @Monday,
                                                          @Tuesday,
                                                          @Wednesday,
                                                          @Thursday,
                                                          @Friday,
                                                          @Saturday,
                                                          @Sunday,
                                                          @TicketAllowed,
                                                          @MasterEntityId,
                                                          @Frequency,
                                                          @PauseAllowed,
                                                          @LastUpdatedBy,
                                                          getdate(),
                                                          @LastUpdatedBy,
                                                          getdate(),
                                                          @IsActive,
                                                          @EffectiveAfterMinutes
                                                         ) 
                                    SELECT * FROM ProductCreditPlus WHERE ProductCreditPlusId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertProductCreditPlusQuery, GetSQLParameters(productCreditPlusDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductCreditPlusDTO(productCreditPlusDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting productCreditPlusDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productCreditPlusDTO);
            return productCreditPlusDTO;
        }

        private void RefreshProductCreditPlusDTO(ProductCreditPlusDTO productCreditPlusDTO, DataTable dt, string loginId, int siteId)
        {
            log.LogMethodEntry(productCreditPlusDTO, dt, loginId, siteId);
            if (dt.Rows.Count > 0)
            {
                productCreditPlusDTO.ProductCreditPlusId = Convert.ToInt32(dt.Rows[0]["ProductCreditPlusId"]);
                productCreditPlusDTO.LastUpdateDate = dt.Rows[0]["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["LastUpdateDate"]);
                productCreditPlusDTO.CreationDate = dt.Rows[0]["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                productCreditPlusDTO.Guid = dt.Rows[0]["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["Guid"]);
                productCreditPlusDTO.LastUpdatedBy = dt.Rows[0]["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["LastUpdatedBy"]);
                productCreditPlusDTO.CreatedBy = dt.Rows[0]["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["CreatedBy"]);
                productCreditPlusDTO.Site_id = dt.Rows[0]["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[0]["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Update the Product CreditPlus record to the database
        /// </summary>
        /// <param name="productCreditPlusDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public ProductCreditPlusDTO UpdateProductCreditPlus(ProductCreditPlusDTO productCreditPlusDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productCreditPlusDTO, loginId, siteId);
            string updateProductCreditPlusQuery = @"Update ProductCreditPlus 
                                                            set
				                                                    CreditPlus	    =	@CreditPlus,
				                                                    Refundable	    =	@Refundable,
				                                                    Remarks 	    =	@Remarks,
				                                                    Product_id	    =	@Product_id,
				                                                    CreditPlusType	=	@CreditPlusType,
				                                                    -- site_id	        =	@site_id,
				                                                    PeriodFrom	    =	@PeriodFrom,
				                                                    PeriodTo	    =	@PeriodTo,
				                                                    ValidForDays	=	@ValidForDays,
				                                                    ExtendOnReload	=	@ExtendOnReload,
				                                                    TimeFrom	    =	@TimeFrom,
				                                                    TimeTo	        =	@TimeTo,
				                                                    Minutes     	=	@Minutes,
				                                                    Monday	        =	@Monday,
				                                                    Tuesday	        =	@Tuesday,
				                                                    Wednesday	    =	@Wednesday,
				                                                    Thursday	    =	@Thursday,
				                                                    Friday	        =	@Friday,
				                                                    Saturday	    =	@Saturday,
				                                                    Sunday	        =	@Sunday,
				                                                    TicketAllowed	=	@TicketAllowed,
				                                                    MasterEntityId	=	@MasterEntityId,
				                                                    Frequency	    =	@Frequency,
				                                                    PauseAllowed	=	@PauseAllowed,
                                                                    LastUpdatedBy   =   @LastUpdatedBy,
                                                                    LastUpdateDate  =   getDate(),
                                                                    IsActive        =   @IsActive,
                                                                    EffectiveAfterMinutes = @EffectiveAfterMinutes
                                                          WHERE 
				                                                    ProductCreditPlusId = @ProductCreditPlusId
				                                          and 
				                                                    Product_id = @Product_id  
                                                           SELECT * FROM ProductCreditPlus WHERE ProductCreditPlusId = @ProductCreditPlusId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateProductCreditPlusQuery, GetSQLParameters(productCreditPlusDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductCreditPlusDTO(productCreditPlusDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating productCreditPlusDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productCreditPlusDTO);
            return productCreditPlusDTO;
        }

        /// <summary>
        /// Gets the ProductCreditPlus
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<ProductCreditPlusDTO> GetAllProductCreditPlusList(List<KeyValuePair<ProductCreditPlusDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectProductCreditPlusQuery = @"select * from  ProductCreditPlus";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ProductCreditPlusDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = count == 0 ? string.Empty : " and ";
                        if (searchParameter.Key == ProductCreditPlusDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(ProductCreditPlusDTO.SearchByParameters.PRODUCTCREDITPLUS_ID) ||
                                 searchParameter.Key.Equals(ProductCreditPlusDTO.SearchByParameters.PRODUCT_ID)||
                                 searchParameter.Key.Equals(ProductCreditPlusDTO.SearchByParameters.MASTERENTITY_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProductCreditPlusDTO.SearchByParameters.ISACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? true : false)));
                        }
                        else if (searchParameter.Key == ProductCreditPlusDTO.SearchByParameters.PRODUCT_ID_LIST ||
                                searchParameter.Key == ProductCreditPlusDTO.SearchByParameters.PRODUCTCREDITPLUS_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
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
                        log.Debug("Ends-GetAllProductCreditPlusList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        log.LogMethodExit(null, "The query parameter does not exist " + searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectProductCreditPlusQuery = selectProductCreditPlusQuery + query;
                selectProductCreditPlusQuery = selectProductCreditPlusQuery + "   Order by ProductCreditPlusId";
            }
            DataTable productCreditPlusDataTable = dataAccessHandler.executeSelectQuery(selectProductCreditPlusQuery, parameters.ToArray(),sqlTransaction);
            List<ProductCreditPlusDTO> productCreditPlusList = new List<ProductCreditPlusDTO>();
            if (productCreditPlusDataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in productCreditPlusDataTable.Rows)
                {
                    ProductCreditPlusDTO productCreditPlusObject = GetProductCreditPlusDTO(dataRow);
                    productCreditPlusList.Add(productCreditPlusObject);
                }
            }
            log.LogMethodExit(productCreditPlusList);
            return productCreditPlusList;
        }

        /// <summary>
        /// Delete the ProductCreditPlus record based on productCreditPlusId - Hard Deletion
        /// </summary>
        /// <param name="productCreditPlusId"></param>
        /// <returns></returns>
        public int DeleteProductCreditPlus(int productCreditPlusId)
        {
            log.LogMethodEntry(productCreditPlusId);
            try
            {
                string deleteQuery = @"delete from ProductCreditPlus where ProductCreditPlusId = @productCreditPlusId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@productCreditPlusId", productCreditPlusId);
                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
        }
        internal List<ProductCreditPlusDTO> GetProductCreditPlusDTOList(List<int> productIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(productIdList);
            List<ProductCreditPlusDTO> productIdListDetailsDTOList = new List<ProductCreditPlusDTO>();
            string query = @"SELECT *
                            FROM ProductCreditPlus, @productIdList List
                            WHERE Product_id = List.Id ";
            if (activeRecords)
            {
                query += " AND (IsActive = 1 or IsActive is null) ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@productIdList", productIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                productIdListDetailsDTOList = table.Rows.Cast<DataRow>().Select(x => GetProductCreditPlusDTO(x)).ToList();
            }
            log.LogMethodExit(productIdListDetailsDTOList);
            return productIdListDetailsDTOList;
        }
    }
}
