/********************************************************************************************
* Project Name - Product
* Description  - DataHandler - DiscountedProducts
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.170.0     05-Jul-2023      Lakshminarayana     Created
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Discounts
{
    class DiscountedProductsDataHandler
    {
        private Semnox.Parafait.logging.Logger log;
        private readonly ExecutionContext executionContext;
        private readonly UnitOfWork unitOfWork;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT dp.*
                                              FROM DiscountedProducts AS dp";

        private const string INSERT_QUERY = @"INSERT INTO DiscountedProducts 
                                                    ( 
                                                        DiscountId,
                                                        ProductId,
                                                        Discounted,
                                                        CategoryId,
                                                        ProductGroupId,
                                                        Quantity,
                                                        DiscountPercentage,
                                                        DiscountAmount,
                                                        DiscountedPrice,
                                                        IsActive,
                                                        site_id,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdateDate
                                                    ) 
                                            VALUES 
                                                    (
                                                        @DiscountId,
                                                        @ProductId,
                                                        @Discounted,
                                                        @CategoryId,
                                                        @ProductGroupId,
                                                        @Quantity,
                                                        @DiscountPercentage,
                                                        @DiscountAmount,
                                                        @DiscountedPrice,
                                                        @IsActive,
                                                        @site_id,
                                                        @MasterEntityId,
                                                        @CreatedBy,
                                                        GetDate(),
                                                        @LastUpdatedBy,
                                                        GetDate()
                                                    )  SELECT* from DiscountedProducts where Id = scope_identity()";
        private const string UPDATE_QUERY = @"UPDATE DiscountedProducts 
                                                SET DiscountId=@DiscountId,
                                                    ProductId=@ProductId,
                                                    Discounted=@Discounted,
                                                    IsActive=@IsActive,
                                                    Quantity=@Quantity,
                                                    CategoryId=@CategoryId,
                                                    ProductGroupId = @ProductGroupId,
                                                    DiscountPercentage=@DiscountPercentage,
                                                    DiscountAmount=@DiscountAmount,
                                                    DiscountedPrice=@DiscountedPrice,
                                                    MasterEntityId=@MasterEntityId,
                                                    LastUpdatedBy=@LastUpdatedBy,
                                                    LastUpdateDate = getDate()
                                                    WHERE Id = @Id 
                                                    SELECT* from DiscountedProducts where Id = @Id";
        /// <summary>
        /// Default constructor of DiscountedProductsDataHandler class
        /// </summary>
        public DiscountedProductsDataHandler(ExecutionContext executionContext, UnitOfWork unitOfWork)
        {
            log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.LogMethodEntry(executionContext, unitOfWork);
            this.executionContext = executionContext;
            this.unitOfWork = unitOfWork;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(DiscountedProductsDTO discountedProductsDTO)
        {
            log.LogMethodEntry(discountedProductsDTO);
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                dataAccessHandler.GetSQLParameter("@Id", discountedProductsDTO.Id, true),
                dataAccessHandler.GetSQLParameter("@DiscountId", discountedProductsDTO.DiscountId, true),
                dataAccessHandler.GetSQLParameter("@ProductId", discountedProductsDTO.ProductId, true),
                dataAccessHandler.GetSQLParameter("@CategoryId", discountedProductsDTO.CategoryId, true),
                dataAccessHandler.GetSQLParameter("@ProductGroupId", discountedProductsDTO.ProductGroupId, true),
                dataAccessHandler.GetSQLParameter("@Quantity", discountedProductsDTO.Quantity),
                dataAccessHandler.GetSQLParameter("@DiscountPercentage", discountedProductsDTO.DiscountPercentage),
                dataAccessHandler.GetSQLParameter("@DiscountAmount", discountedProductsDTO.DiscountAmount),
                dataAccessHandler.GetSQLParameter("@DiscountedPrice", discountedProductsDTO.DiscountedPrice),
                dataAccessHandler.GetSQLParameter("@Discounted", discountedProductsDTO.Discounted),
                dataAccessHandler.GetSQLParameter("@IsActive", discountedProductsDTO.IsActive ? "Y" : "N"),
                dataAccessHandler.GetSQLParameter("@site_id", executionContext.SiteId, true),
                dataAccessHandler.GetSQLParameter("@MasterEntityId", discountedProductsDTO.MasterEntityId, true),
                dataAccessHandler.GetSQLParameter("@LastUpdatedBy", executionContext.UserId),
                dataAccessHandler.GetSQLParameter("@CreatedBy", executionContext.UserId)
            };
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the DiscountedProducts record to the database
        /// </summary>
        /// <param name="discountedProductsDTO">DiscountedProductsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted DiscountedProductsDTO</returns>
        internal DiscountedProductsDTO Save(DiscountedProductsDTO discountedProductsDTO)
        {
            log.LogMethodEntry(discountedProductsDTO);
            string query = discountedProductsDTO.Id <= -1 ? INSERT_QUERY : UPDATE_QUERY;

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(discountedProductsDTO).ToArray(), unitOfWork.SQLTrx);
                DataRow dataRow = dt.Rows[0];
                discountedProductsDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                discountedProductsDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                discountedProductsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                discountedProductsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                discountedProductsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                discountedProductsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                discountedProductsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(discountedProductsDTO);
            return discountedProductsDTO;
        }

        /// <summary>
        /// Gets the GetDiscountedProducts data
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns DiscountedProductsDTO</returns>
        internal DiscountedProductsDTO GetDiscountedProducts(int id)
        {
            log.LogMethodEntry(id);
            DiscountedProductsDTO result = null;
            string query = SELECT_QUERY + @" WHERE dp.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, unitOfWork.SQLTrx);
            if (dataTable.Rows.Count > 0)
            {
                result = GetDiscountedProductsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Converts the Data row object to DiscountedProductsDTO class type
        /// </summary>
        /// <param name="dataRow">DiscountedProducts DataRow</param>
        /// <returns>Returns DiscountedProductsDTO</returns>
        private DiscountedProductsDTO GetDiscountedProductsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            DiscountedProductsDTO discountedProductsDTO = new DiscountedProductsDTO(Convert.ToInt32(dataRow["Id"]),
                                                                                                    dataRow["DiscountId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DiscountId"]),
                                                                                                    dataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductId"]),
                                                                                                    dataRow["Discounted"] == DBNull.Value ? "N" : Convert.ToString(dataRow["Discounted"]),
                                                                                                    dataRow["IsActive"] == DBNull.Value ? true : Convert.ToString(dataRow["IsActive"]) == "Y",
                                                                                                    dataRow["CategoryId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CategoryId"]),
                                                                                                    dataRow["ProductGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductGroupId"]),
                                                                                                    dataRow["Quantity"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Quantity"]),
                                                                                                    dataRow["DiscountPercentage"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["DiscountPercentage"]),
                                                                                                    dataRow["DiscountAmount"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["DiscountAmount"]),
                                                                                                    dataRow["DiscountedPrice"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["DiscountedPrice"]),
                                                                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                                                                    dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                                                                    dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                                                                                    dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                                                                    dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                                                                    dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                                                                                    );
            log.LogMethodExit(discountedProductsDTO);
            return discountedProductsDTO;
        }

        /// <summary>
        /// Gets the DiscountedProductsDTO List for Discount Id List
        /// </summary>
        /// <param name="discountIdList">integer list parameter</param>
        /// <returns>Returns List of DiscountedProductsDTO</returns>
        public List<DiscountedProductsDTO> GetDiscountedProductsDTOListOfDiscounts(List<int> discountIdList, 
                                                                                   bool activeRecords, 
                                                                                   bool onlyDiscountedChildRecord)
        {
            log.LogMethodEntry(discountIdList, activeRecords, onlyDiscountedChildRecord);
            List<DiscountedProductsDTO> list = new List<DiscountedProductsDTO>();
            string query = SELECT_QUERY + @", @DiscountIdList List WHERE dp.DiscountId = List.Id ";
            if (activeRecords)
            {
                query += " AND dp.IsActive = 'Y' ";
            }
            if (onlyDiscountedChildRecord)
            {
                query += " AND dp.Discounted = 'Y' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@DiscountIdList", discountIdList, null, unitOfWork.SQLTrx);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetDiscountedProductsDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

    }

}