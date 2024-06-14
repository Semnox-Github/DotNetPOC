/********************************************************************************************
* Project Name - Product
* Description  - DataHandler - DiscountPurchaseCriteria
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
    class DiscountPurchaseCriteriaDataHandler
    {
        private Semnox.Parafait.logging.Logger log;
        private readonly ExecutionContext executionContext;
        private readonly UnitOfWork unitOfWork;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT dpc.*
                                              FROM DiscountPurchaseCriteria AS dpc";

        private const string INSERT_QUERY = @"INSERT INTO DiscountPurchaseCriteria 
                                                    ( 
                                                        DiscountId,
                                                        ProductId,
                                                        CategoryId,
                                                        ProductGroupId,
                                                        MinQuantity,
                                                        LastUpdatedDate,
                                                        LastUpdatedBy,
                                                        IsActive,
                                                        site_id,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate
                                                    ) 
                                            VALUES 
                                                    (
                                                        @DiscountId,
                                                        @ProductId,
                                                        @CategoryId,
                                                        @ProductGroupId,
                                                        @MinQuantity,
                                                        GETDATE(),
                                                        @LastUpdatedBy,
                                                        @IsActive,
                                                        @SiteId,
                                                        @MasterEntityId,
                                                        @CreatedBy,
                                                        GETDATE()
                                                    ) SELECT* from DiscountPurchaseCriteria where CriteriaId = scope_identity()";
        private const string UPDATE_QUERY = @"UPDATE DiscountPurchaseCriteria 
                                            SET DiscountId=@DiscountId,
                                                ProductId=@ProductId,
                                                CategoryId=@CategoryId,
                                                ProductGroupId=@ProductGroupId,
                                                MinQuantity=@MinQuantity,
                                                LastUpdatedBy=@LastUpdatedBy,
                                                LastUpdatedDate=GETDATE(),
                                                IsActive=@IsActive,
                                                MasterEntityId=@MasterEntityId
                                            WHERE CriteriaId = @CriteriaId 
                                            SELECT* from DiscountPurchaseCriteria where CriteriaId = @CriteriaId ";
        /// <summary>
        /// Default constructor of DiscountPurchaseCriteriaDataHandler class
        /// </summary>
        public DiscountPurchaseCriteriaDataHandler(ExecutionContext executionContext, UnitOfWork unitOfWork)
        {
            log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.LogMethodEntry(executionContext, unitOfWork);
            this.executionContext = executionContext;
            this.unitOfWork = unitOfWork;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(DiscountPurchaseCriteriaDTO discountPurchaseCriteriaDTO)
        {
            log.LogMethodEntry(discountPurchaseCriteriaDTO);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CriteriaId", discountPurchaseCriteriaDTO.CriteriaId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DiscountId", discountPurchaseCriteriaDTO.DiscountId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", discountPurchaseCriteriaDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CategoryId", discountPurchaseCriteriaDTO.CategoryId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductGroupId", discountPurchaseCriteriaDTO.ProductGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MinQuantity", discountPurchaseCriteriaDTO.MinQuantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", discountPurchaseCriteriaDTO.IsActive? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", executionContext.UserId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", executionContext.UserId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", executionContext.SiteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", discountPurchaseCriteriaDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the DiscountPurchaseCriteria record to the database
        /// </summary>
        /// <param name="discountPurchaseCriteriaDTO">DiscountPurchaseCriteriaDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted DiscountPurchaseCriteriaDTO</returns>
        internal DiscountPurchaseCriteriaDTO Save(DiscountPurchaseCriteriaDTO discountPurchaseCriteriaDTO)
        {
            log.LogMethodEntry(discountPurchaseCriteriaDTO);
            string query = discountPurchaseCriteriaDTO.CriteriaId <= -1 ? INSERT_QUERY : UPDATE_QUERY;

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(discountPurchaseCriteriaDTO).ToArray(), unitOfWork.SQLTrx);
                DataRow dataRow = dt.Rows[0];
                discountPurchaseCriteriaDTO.CriteriaId = Convert.ToInt32(dt.Rows[0]["CriteriaId"]);
                discountPurchaseCriteriaDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                discountPurchaseCriteriaDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                discountPurchaseCriteriaDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                discountPurchaseCriteriaDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                discountPurchaseCriteriaDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                discountPurchaseCriteriaDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(discountPurchaseCriteriaDTO);
            return discountPurchaseCriteriaDTO;
        }

        /// <summary>
        /// Gets the GetDiscountPurchaseCriteria data
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns DiscountPurchaseCriteriaDTO</returns>
        internal DiscountPurchaseCriteriaDTO GetDiscountPurchaseCriteria(int id)
        {
            log.LogMethodEntry(id);
            DiscountPurchaseCriteriaDTO result = null;
            string query = SELECT_QUERY + @" WHERE dpc.CriteriaId = @CriteriaId";
            SqlParameter parameter = new SqlParameter("@CriteriaId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, unitOfWork.SQLTrx);
            if (dataTable.Rows.Count > 0)
            {
                result = GetDiscountPurchaseCriteriaDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Converts the Data row object to DiscountPurchaseCriteriaDTO class type
        /// </summary>
        /// <param name="dataRow">DiscountPurchaseCriteria DataRow</param>
        /// <returns>Returns DiscountPurchaseCriteriaDTO</returns>
        private DiscountPurchaseCriteriaDTO GetDiscountPurchaseCriteriaDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            DiscountPurchaseCriteriaDTO discountPurchaseCriteriaDTO = new DiscountPurchaseCriteriaDTO(Convert.ToInt32(dataRow["CriteriaId"]),
                                                                                                        dataRow["DiscountId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DiscountId"]),
                                                                                                        dataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductId"]),
                                                                                                        dataRow["CategoryId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CategoryId"]),
                                                                                                        dataRow["ProductGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductGroupId"]),
                                                                                                        dataRow["MinQuantity"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["MinQuantity"]),
                                                                                                        dataRow["LastUpdatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                                                                        dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                                                                        dataRow["IsActive"] == DBNull.Value ? true : Convert.ToString(dataRow["IsActive"]) == "Y",
                                                                                                        dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                                                                        dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                                                                        dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                                                                        dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString(),
                                                                                                        dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                                                                        dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                                                                                        );
            log.LogMethodExit(discountPurchaseCriteriaDTO);
            return discountPurchaseCriteriaDTO;
        }

        /// <summary>
        /// Gets the DiscountPurchaseCriteriaDTO List for Discount Id List
        /// </summary>
        /// <param name="discountIdList">integer list parameter</param>
        /// <returns>Returns List of DiscountPurchaseCriteriaDTO</returns>
        public List<DiscountPurchaseCriteriaDTO> GetDiscountPurchaseCriteriaDTOListOfDiscounts(List<int> discountIdList, bool activeRecords)
        {
            log.LogMethodEntry(discountIdList);
            List<DiscountPurchaseCriteriaDTO> list = new List<DiscountPurchaseCriteriaDTO>();
            string query = SELECT_QUERY + @", @DiscountIdList List
                            WHERE dpc.DiscountId = List.Id ";
            if (activeRecords)
            {
                query += " AND dpc.IsActive = 'Y' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@DiscountIdList", discountIdList, null, unitOfWork.SQLTrx);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetDiscountPurchaseCriteriaDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

    }

}