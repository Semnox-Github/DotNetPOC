/********************************************************************************************
 * Project Name - DiscountCoupons Data Handler
 * Description  - Data handler of the DiscountCoupons class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        16-Jul-2017   Lakshminarayana     Created
 *2.60        17-Mar-2019   Akshay Gulaganji    Modified isActive(string to bool) 
 *            19-Apr-2019   Raghuveera          Batch functions added
 *2.60.2      29-May-2019   Jagan Mohan         Code merge from Development to WebManagementStudio
 *2.70        08-Jul-2019   Akshay G            Merged from Development to Web branch
 *            25-Jul-2019   Mushahid Faizan     Added DeleteDiscountCoupons() method.
 *2.70.2        31-Jul-2019   Girish Kundar       Added LogMethodEcnty() and LogMethodExit().
 *2.70.2       06-Dec-2019   Jinto Thomas         Removed siteid from update query 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    ///  DiscountCoupons Data Handler - Handles insert, update and select of  DiscountCoupons objects
    /// </summary>
    public class DiscountCouponsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<DiscountCouponsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<DiscountCouponsDTO.SearchByParameters, string>
            {
                {DiscountCouponsDTO.SearchByParameters.COUPON_SET_ID, "CouponSetId"},
                {DiscountCouponsDTO.SearchByParameters.IS_ACTIVE, "IsActive"},
                {DiscountCouponsDTO.SearchByParameters.DISCOUNT_COUPONS_HEADER_ID, "DiscountCouponHeaderId"},
                {DiscountCouponsDTO.SearchByParameters.MASTER_HEADER_ID, "MasterEntityId"},
                {DiscountCouponsDTO.SearchByParameters.TRANSACTION_ID, "TransactionId"},
                {DiscountCouponsDTO.SearchByParameters.LINE_ID, "LineId"},
                {DiscountCouponsDTO.SearchByParameters.COUPON_NUMBER, "FromNumber"},
                {DiscountCouponsDTO.SearchByParameters.MASTER_ENTITY_ID,"MasterEntityId"},
                {DiscountCouponsDTO.SearchByParameters.EXPIRY_DATE_GREATER_THAN,"ExpiryDate"},
                {DiscountCouponsDTO.SearchByParameters.EXPIRY_DATE_LESS_THAN,"ExpiryDate"},
                {DiscountCouponsDTO.SearchByParameters.START_DATE_GREATER_THAN,"StartDate"},
                {DiscountCouponsDTO.SearchByParameters.START_DATE_LESS_THAN,"StartDate"},
                {DiscountCouponsDTO.SearchByParameters.DISCOUNT_ID,"Discount_id"},
                {DiscountCouponsDTO.SearchByParameters.PAYMENT_MODE_ID,"PaymentModeId"},
                {DiscountCouponsDTO.SearchByParameters.SITE_ID, "site_id"},
                {DiscountCouponsDTO.SearchByParameters.DISCOUNT_ID_LIST, "Discount_id"},
            };
        private readonly DataAccessHandler dataAccessHandler;
        SqlTransaction sqlTransaction = null;
        #region MERGE_QUERY
        private const string MERGE_QUERY = @"DECLARE @Output AS DiscountCouponsType;
                                            MERGE INTO DiscountCoupons tbl
                                            USING @discountCouponsList AS src
                                            ON src.CouponSetId = tbl.CouponSetId
                                            WHEN MATCHED THEN
                                            UPDATE SET
                                            Discount_id = src.Discount_id,
                                            FromNumber = src.FromNumber,
                                            ToNumber = src.ToNumber,
                                            Count= src.Count,
                                            StartDate= src.StartDate,
                                            ExpiryDate= src.ExpiryDate,
                                            CouponValue = src.CouponValue,
                                            DiscountCouponHeaderId= src.DiscountCouponHeaderId,
                                            TransactionId = src.TransactionId,
                                            LineId= src.LineId,
                                            PaymentModeId=src.PaymentModeId,
                                            UsedCount= src.UsedCount,
                                            isActive = src.isActive,
                                            LastUpdatedBy = src.LastUpdatedBy,
                                            last_updated_date = GETDATE(),
                                            MasterEntityId = src.MasterEntityId
                                            -- site_id = src.site_id
                                            WHEN NOT MATCHED THEN INSERT (
                                            Discount_id,
                                            FromNumber,
                                            ToNumber,
                                            Count,
                                            StartDate,
                                            ExpiryDate,
                                            CouponValue,
                                            DiscountCouponHeaderId,
                                            TransactionId,
                                            LineId,
                                            PaymentModeId,                                            
                                            UsedCount,
                                            isActive,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            last_updated_date,
                                            site_id,
                                            Guid,
                                            MasterEntityId
                                            )VALUES (
                                            src.Discount_id,
                                            src.FromNumber,
                                            src.ToNumber,
                                            src.Count,
                                            src.StartDate,
                                            src.ExpiryDate,
                                            src.CouponValue,
                                            src.DiscountCouponHeaderId,
                                            src.TransactionId,
                                            src.LineId,
                                            src.PaymentModeId,
                                            src.UsedCount,
                                            src.isActive,
                                            src.CreatedBy,
                                            getdate(),
                                            src.LastUpdatedBy,
                                            getdate(),
                                            src.site_id,
                                            src.Guid,
                                            src.MasterEntityId
                                            )
                                            OUTPUT
                                            inserted.CouponSetId,
                                            inserted.CreatedBy,
                                            inserted.CreationDate,
                                            inserted.last_updated_date,
                                            inserted.LastUpdatedBy,
                                            inserted.site_id,
                                            inserted.Guid
                                            INTO @Output(
                                            CouponSetId,
                                            CreatedBy, 
                                            CreationDate, 
                                            last_updated_date, 
                                            LastUpdatedBy, 
                                            site_id, 
                                            Guid);
                                            if((Select count(1) from site)>1)
											begin
											Insert into DBSynchLog(Operation,Guid,TableName,TimeStamp,site_id)
											select 'I', src.Guid, 'DiscountCoupons', getdate(), s.site_id from @discountCouponsList src, site s
                                                                       where s.site_id in(select site_id from discounts 
																	                         where MasterEntityId = (select MasterEntityId from discounts 
																							                         where Discount_id = src.Discount_id)
																							) 
											end
                                            SELECT * FROM @Output;";
        #endregion

        /// <summary>
        /// Default constructor of DiscountCouponsDataHandler class
        /// </summary>
        public DiscountCouponsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the DiscountCoupon record to the database
        /// </summary>
        /// <param name="discountCouponsDTO">DiscountCouponsDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(DiscountCouponsDTO discountCouponsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(discountCouponsDTO, userId, siteId);
            Save(new List<DiscountCouponsDTO>() { discountCouponsDTO }, userId, siteId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the DiscountCoupon record to the database
        /// </summary>
        /// <param name="discountCouponsDTOList">List of DiscountCouponsDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(List<DiscountCouponsDTO> discountCouponsDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(discountCouponsDTOList, userId, siteId);
            Dictionary<string, DiscountCouponsDTO> discountCouponsDTOGuidMap = GetDiscountCouponsDTOGuidMap(discountCouponsDTOList);
            List<SqlDataRecord> sqlDataRecords = GetSqlDataRecords(discountCouponsDTOList, userId, siteId);
            DataTable dataTable = dataAccessHandler.BatchSave(sqlDataRecords,
                                                                sqlTransaction,
                                                                MERGE_QUERY,
                                                                "DiscountCouponsType",
                                                                "@discountCouponsList");
            UpdateDiscountCouponsDTOList(discountCouponsDTOGuidMap, dataTable);
            log.LogMethodExit();
        }

        private List<SqlDataRecord> GetSqlDataRecords(List<DiscountCouponsDTO> discountCouponsDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(discountCouponsDTOList, userId, siteId);
            List<SqlDataRecord> result = new List<SqlDataRecord>();
            SqlMetaData[] columnStructures = new SqlMetaData[22];
            columnStructures[0] = new SqlMetaData("CouponSetId", SqlDbType.Int);
            columnStructures[1] = new SqlMetaData("Discount_id", SqlDbType.Int);
            columnStructures[2] = new SqlMetaData("FromNumber", SqlDbType.NVarChar, 40);
            columnStructures[3] = new SqlMetaData("ToNumber", SqlDbType.NVarChar, 40);
            columnStructures[4] = new SqlMetaData("Count", SqlDbType.Int);
            columnStructures[5] = new SqlMetaData("Guid", SqlDbType.UniqueIdentifier);
            columnStructures[6] = new SqlMetaData("SynchStatus", SqlDbType.Bit);
            columnStructures[7] = new SqlMetaData("site_id", SqlDbType.Int);
            columnStructures[8] = new SqlMetaData("MasterEntityId", SqlDbType.Int);
            columnStructures[9] = new SqlMetaData("StartDate", SqlDbType.DateTime);
            columnStructures[10] = new SqlMetaData("ExpiryDate", SqlDbType.DateTime);
            columnStructures[11] = new SqlMetaData("last_updated_date", SqlDbType.DateTime);
            columnStructures[12] = new SqlMetaData("DiscountCouponHeaderId", SqlDbType.Int);
            columnStructures[13] = new SqlMetaData("IsActive", SqlDbType.Char, 1);
            columnStructures[14] = new SqlMetaData("TransactionId", SqlDbType.Int);
            columnStructures[15] = new SqlMetaData("LineId", SqlDbType.Int);
            columnStructures[16] = new SqlMetaData("CreatedBy", SqlDbType.NVarChar, 50);
            columnStructures[17] = new SqlMetaData("CreationDate", SqlDbType.DateTime);
            columnStructures[18] = new SqlMetaData("LastUpdatedBy", SqlDbType.NVarChar, 50);
            columnStructures[19] = new SqlMetaData("UsedCount", SqlDbType.Int);
            columnStructures[20] = new SqlMetaData("PaymentModeId", SqlDbType.Int);
            columnStructures[21] = new SqlMetaData("CouponValue", SqlDbType.Decimal, 18, 4);
            for (int i = 0; i < discountCouponsDTOList.Count; i++)
            {
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures);
                dataRecord.SetValue(0, dataAccessHandler.GetParameterValue(discountCouponsDTOList[i].CouponSetId, true));
                dataRecord.SetValue(1, dataAccessHandler.GetParameterValue(discountCouponsDTOList[i].DiscountId, true));
                dataRecord.SetValue(2, dataAccessHandler.GetParameterValue(discountCouponsDTOList[i].FromNumber));
                dataRecord.SetValue(3, dataAccessHandler.GetParameterValue(discountCouponsDTOList[i].ToNumber));
                dataRecord.SetValue(4, dataAccessHandler.GetParameterValue(discountCouponsDTOList[i].Count));
                dataRecord.SetValue(5, dataAccessHandler.GetParameterValue(Guid.Parse(discountCouponsDTOList[i].Guid)));
                dataRecord.SetValue(6, dataAccessHandler.GetParameterValue(discountCouponsDTOList[i].SynchStatus));
                dataRecord.SetValue(7, dataAccessHandler.GetParameterValue(siteId, true));
                dataRecord.SetValue(8, dataAccessHandler.GetParameterValue(discountCouponsDTOList[i].MasterEntityId, true));
                dataRecord.SetValue(9, dataAccessHandler.GetParameterValue(discountCouponsDTOList[i].StartDate));
                dataRecord.SetValue(10, dataAccessHandler.GetParameterValue(discountCouponsDTOList[i].ExpiryDate));
                dataRecord.SetValue(11, dataAccessHandler.GetParameterValue(discountCouponsDTOList[i].LastUpdatedDate));
                dataRecord.SetValue(12, dataAccessHandler.GetParameterValue(discountCouponsDTOList[i].DiscountCouponHeaderId, true));
                dataRecord.SetValue(13, dataAccessHandler.GetParameterValue(discountCouponsDTOList[i].IsActive ? "Y" : "N"));
                dataRecord.SetValue(14, dataAccessHandler.GetParameterValue(discountCouponsDTOList[i].TransactionId, true));
                dataRecord.SetValue(15, dataAccessHandler.GetParameterValue(discountCouponsDTOList[i].LineId, true));
                dataRecord.SetValue(16, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(17, dataAccessHandler.GetParameterValue(discountCouponsDTOList[i].CreationDate));
                dataRecord.SetValue(18, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(19, dataAccessHandler.GetParameterValue(discountCouponsDTOList[i].UsedCount));
                dataRecord.SetValue(20, dataAccessHandler.GetParameterValue(discountCouponsDTOList[i].PaymentModeId, true));
                dataRecord.SetValue(21, dataAccessHandler.GetParameterValue(discountCouponsDTOList[i].CouponValue));
                result.Add(dataRecord);
            }
            log.LogMethodExit(result);
            return result;
        }

        private Dictionary<string, DiscountCouponsDTO> GetDiscountCouponsDTOGuidMap(List<DiscountCouponsDTO> discountCouponsDTOList)
        {
            Dictionary<string, DiscountCouponsDTO> result = new Dictionary<string, DiscountCouponsDTO>();
            for (int i = 0; i < discountCouponsDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(discountCouponsDTOList[i].Guid))
                {
                    discountCouponsDTOList[i].Guid = Guid.NewGuid().ToString();
                }
                result.Add(discountCouponsDTOList[i].Guid, discountCouponsDTOList[i]);
            }
            return result;
        }

        private void UpdateDiscountCouponsDTOList(Dictionary<string, DiscountCouponsDTO> discountCouponsDTOGuidMap, DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                DiscountCouponsDTO discountCouponsDTO = discountCouponsDTOGuidMap[Convert.ToString(row["Guid"])];
                discountCouponsDTO.CouponSetId = row["CouponSetId"] == DBNull.Value ? -1 : Convert.ToInt32(row["CouponSetId"]);
                discountCouponsDTO.CreatedBy = row["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["CreatedBy"]);
                discountCouponsDTO.CreationDate = row["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreationDate"]);
                discountCouponsDTO.LastUpdatedBy = row["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["LastUpdatedBy"]);
                discountCouponsDTO.LastUpdatedDate = row["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["last_updated_date"]);
                discountCouponsDTO.SiteId = row["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["site_id"]);
                discountCouponsDTO.AcceptChanges();
            }
        }

        /// <summary>
        /// Converts the Data row object to DiscountCouponsDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns DiscountCouponsDTO</returns>
        private DiscountCouponsDTO GetDiscountCouponsDTO(DataRow dataRow, int siteId = -1)
        {
            log.LogMethodEntry(dataRow);
            DiscountCouponsDTO discountCouponsDTO = new DiscountCouponsDTO(dataRow["CouponSetId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CouponSetId"]),
                                            dataRow["DiscountCouponHeaderId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DiscountCouponHeaderId"]),
                                            siteId == -1 ? (dataRow["Discount_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Discount_id"])) : (dataRow["TranslatedDiscountId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TranslatedDiscountId"])),
                                            dataRow["TransactionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TransactionId"]),
                                            dataRow["LineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LineId"]),
                                            dataRow["Count"] == DBNull.Value ? 1 : Convert.ToInt32(dataRow["Count"]),
                                            dataRow["UsedCount"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["UsedCount"]),
                                            dataRow["PaymentModeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentModeId"]),
                                            dataRow["FromNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["FromNumber"]),
                                            dataRow["ToNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ToNumber"]),
                                            dataRow["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["StartDate"]),
                                            dataRow["ExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["ExpiryDate"]),
                                            dataRow["couponValue"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["couponValue"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToString(dataRow["IsActive"]) == "Y",
                                            dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString()
                                            );
            log.LogMethodExit(discountCouponsDTO);
            return discountCouponsDTO;
        }

        /// <summary>
        ///  SQL parameter generator for Insert and Update Queries.
        /// </summary>
        /// <param name="parameters">parameters</param>
        /// <param name="parameterName">parameterName</param>
        /// <param name="value">value</param>
        /// <param name="negetiveValueNull">negetiveValueNull</param>
        private void ParameterHelper(List<SqlParameter> parameters, string parameterName, object value, bool negetiveValueNull = false)
        {
            log.LogMethodEntry(parameters, parameterName, value, negetiveValueNull);
            if (parameters != null && !string.IsNullOrEmpty(parameterName))
            {
                if (value is int)
                {
                    if (negetiveValueNull && ((int)value) < 0)
                    {
                        parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(parameterName, value));
                    }
                }
                else if (value is string)
                {
                    if (string.IsNullOrEmpty(value as string))
                    {
                        parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(parameterName, value));
                    }
                }
                else
                {
                    if (value == null)
                    {
                        parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(parameterName, value));
                    }
                }
            }
            log.LogMethodExit();
        }

        internal List<DiscountCouponsDTO> GetDiscountCouponHeaderDTOListOfDiscountCouponHeader(List<int> discountCouponSetHeaderIdList, bool activeRecords)//added
        {
            log.LogMethodEntry(discountCouponSetHeaderIdList, activeRecords);
            List<DiscountCouponsDTO> discountCouponsDTOList = new List<DiscountCouponsDTO>();
            string query = "Select * from DiscountCoupons" + @" , @discountCouponSetHeaderIdList List
                            WHERE DiscountCouponHeaderId = List.Id ";
            if (activeRecords)
            {
                query += " AND IsActive = 'Y' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@discountCouponSetHeaderIdList", discountCouponSetHeaderIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                discountCouponsDTOList = table.Rows.Cast<DataRow>().Select(x => GetDiscountCouponsDTO(x)).ToList();
            }
            log.LogMethodExit(discountCouponsDTOList);
            return discountCouponsDTOList;
        }



        /// <summary>
        /// Returns the reference count of the discount record.
        /// <param name="id">DiscountCoupons Id</param>
        /// </summary>
        /// <returns>Returns reference count</returns>
        public int GetDiscountCouponsReferenceCount(int id)
        {
            log.LogMethodEntry(id);
            int refrenceCount = 0;
            string query = @"SELECT COUNT(1) AS ReferenceCount
                             FROM DiscountCouponsUsed 
                             WHERE DiscountCouponsUsed.CouponSetId = @Id AND IsActive = 'Y'";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                refrenceCount = Convert.ToInt32(dataTable.Rows[0]["ReferenceCount"]);
            }
            log.LogMethodExit(refrenceCount);
            return refrenceCount;
        }

        /// <summary>
        /// Gets the DiscountCoupons data of passed DiscountCoupons Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns DiscountCouponsDTO</returns>
        public DiscountCouponsDTO GetDiscountCouponsDTO(int id, int siteid = -1)
        {
            log.LogMethodEntry(id);
            DiscountCouponsDTO result = null;
            string query;
            if (siteid > -1)
            {
                query = @"SELECT DiscountCoupons.*, 
                                (SELECT TOP 1 discount_id 
                                 From Discounts 
                                 where (masterEntityId IS NOT NULL AND masterEntityId = (SELECT TOP 1 MasterEntityId From discounts where discount_id = DiscountCoupons.discount_id)
                                      and Discounts.site_id = @siteId) 
                                        OR
                                        (masterEntityId IS NULL AND Discounts.discount_id = DiscountCoupons.discount_id and (Discounts.site_id = @siteId or @siteId = -1))
                                ) TranslatedDiscountId
                            FROM DiscountCoupons
                            WHERE CouponSetId = @CouponSetId";
            }
            else
            {
                query = @"SELECT *
                            FROM DiscountCoupons
                            WHERE CouponSetId = @CouponSetId";
            }
            DataTable table = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { dataAccessHandler.GetSQLParameter("@CouponSetId", id, true), dataAccessHandler.GetSQLParameter("@siteId", siteid, false) }, sqlTransaction);
            if (table != null && table.Rows.Count > 0)
            {
                var list = table.Rows.Cast<DataRow>().Select(x => GetDiscountCouponsDTO(x));
                if (list != null)
                {
                    result = list.FirstOrDefault();
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Gets the coupon number Usage count of passed couponNumber
        /// </summary>
        /// <param name="transactionId">transactionId</param>
        /// <param name="couponNumber">couponNumber</param>
        /// <returns>UsageCount</returns>
        public bool IsCouponNumberUsedInAnotherTransaction(int transactionId, string couponNumber)
        {
            log.LogMethodEntry(transactionId, couponNumber);
            bool result = false;
            string query = @"SELECT COUNT(DISTINCT ISNULL(TrxId, site_id)) AS UsageCount
                             FROM DiscountCouponsUsed
                             WHERE CouponNumber = @CouponNumber
                             AND IsActive = 'Y' AND
                             (TrxId != @TrxId OR TrxId IS NULL)";
            List<SqlParameter> parameters = new List<SqlParameter>();
            ParameterHelper(parameters, "@CouponNumber", couponNumber);
            ParameterHelper(parameters, "@TrxId", transactionId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                int usageCount = Convert.ToInt32(dataTable.Rows[0]["UsageCount"]);
                result = usageCount > 0;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Gets the coupon number used or not 
        /// </summary>
        /// <param name="transactionId">transactionId</param>
        /// <param name="couponNumber">couponNumber</param>
        /// <returns>Boolean</returns>
        public bool IsCouponNumberUsedInTransaction(int transactionId, string couponNumber)
        {
            log.LogMethodEntry(transactionId, couponNumber);
            bool isCouponUsed = false;
            string query = @"SELECT (Case When COUNT(DISTINCT ISNULL(TrxId, site_id))>0 then 1 else 0 end) IsUsed 
                             FROM DiscountCouponsUsed
                             WHERE CouponNumber = @CouponNumber
                             AND IsActive = 'Y' AND
                             TrxId = @TrxId";
            List<SqlParameter> parameters = new List<SqlParameter>();
            ParameterHelper(parameters, "@CouponNumber", couponNumber);
            ParameterHelper(parameters, "@TrxId", transactionId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                isCouponUsed = Convert.ToBoolean(dataTable.Rows[0]["IsUsed"]);
            }
            log.LogMethodExit(isCouponUsed);
            return isCouponUsed;
        }

        /// <summary>
        /// Validates DiscountCouponsDTO
        /// </summary>
        /// <param name="discountCouponsDTOList">discountCouponsDTOList</param>
        /// <param name="userId">userId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>List of DiscountCouponsDTO</returns>
        public List<DiscountCouponsDTO> ValidateDuplicate(List<DiscountCouponsDTO> discountCouponsDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(discountCouponsDTOList);
            dataAccessHandler.CommandTimeOut = 0;
            List<DiscountCouponsDTO> discountCouponsDTOs = new List<DiscountCouponsDTO>();
            string selectQuery = @"SELECT * FROM @discountCouponsList dcl WHERE EXISTS(SELECT TOP 1 dc.CouponSetId
                                   FROM DiscountCoupons dc WHERE dc.IsActive = 'Y' AND (dc.FromNumber IS NOT NULL AND dc.ToNumber IS NOT NULL AND (
                                   (len(dcl.fromNumber) = len(dc.FromNumber) AND dcl.fromNumber between dc.FromNumber and dc.ToNumber) 
                                    OR (len(dcl.toNumber) = len(dc.FromNumber) AND dcl.toNumber between dc.FromNumber and dc.ToNumber)))
                                   OR
                                   (
                                    (dc.FromNumber IS NOT NULL AND dcl.fromNumber IS NOT NULL AND dc.FromNumber = dcl.fromNumber) OR 
                                    (dc.FromNumber IS NOT NULL AND dcl.toNumber IS NOT NULL AND dc.FromNumber = dcl.toNumber) OR 
                                    (dc.ToNumber IS NOT NULL AND dcl.fromNumber IS NOT NULL AND dc.ToNumber = dcl.fromNumber) OR 
                                    (dc.ToNumber IS NOT NULL AND dcl.toNumber IS NOT NULL AND dc.ToNumber = dcl.toNumber)
                                    ))";

            int batchSize = 5000;
            int totalNoOfRecords = discountCouponsDTOList.Count;
            int noOfBatches = totalNoOfRecords / batchSize;
            if (totalNoOfRecords % batchSize > 0)
            {
                noOfBatches++;
            }
            for (int i = 0; i < noOfBatches; i++)
            {
                int index = i * batchSize;
                int count = batchSize;
                if (index + count > totalNoOfRecords)
                {
                    count = totalNoOfRecords - index;
                }
                if (count <= 0)
                {
                    continue;
                }
                List<DiscountCouponsDTO> subset = discountCouponsDTOList.GetRange(index, count);
                foreach (DiscountCouponsDTO discountCoupons in subset)
                {
                    if (string.IsNullOrEmpty(discountCoupons.Guid))
                    {
                        discountCoupons.Guid = Guid.NewGuid().ToString();
                    }
                }
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@discountCouponsList", SqlDbType.Structured));
                parameterList[0].TypeName = "DiscountCouponsType";
                parameterList[0].Value = GetSqlDataRecords(subset, userId, siteId);
                List<DiscountCouponsDTO> list;
                DataTable table = dataAccessHandler.executeSelectQuery(selectQuery, parameterList.ToArray(), sqlTransaction);
                if (table != null && table.Rows.Cast<DataRow>().Any())
                {
                    list = table.Rows.Cast<DataRow>().Select(x => GetDiscountCouponsDTO(x)).ToList();
                    if (list != null)
                    {
                        discountCouponsDTOs.AddRange(list);
                    }
                }
            }
            log.LogMethodExit(discountCouponsDTOs);
            return discountCouponsDTOs;
        }

        /// <summary>
        /// Gets the DiscountCoupons data of passed coupon number
        /// </summary>
        /// <param name="couponNumber">coupon numbers</param>
        /// <returns>Returns DiscountCouponsDTO</returns>
        public DiscountCouponsDTO GetDiscountCouponsDTO(string couponNumber, int siteid = -1)
        {
            log.LogMethodEntry(couponNumber);
            DiscountCouponsDTO returnValue = null;
            string query = @"SELECT TOP 1 dc.CouponSetId
                             FROM DiscountCoupons dc
                             WHERE ((Tonumber is null and FromNumber = @CouponNumber)
                                or Tonumber is not null
                                    and len(@couponNumber) = len(FromNumber) 
                                    and @CouponNumber between isnull(FromNumber, '') and isnull(ToNumber, 'zzzzzzzzzzzzzzzzzzzz')
                                    or (FromNumber is null and ToNumber is null and dc.Count is not null))";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@CouponNumber", couponNumber));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                int couponSetId = Convert.ToInt32(dataTable.Rows[0][0]);
                returnValue = GetDiscountCouponsDTO(couponSetId, siteid);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the DiscountCouponsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of DiscountCouponsDTO matching the search criteria</returns>
        public List<DiscountCouponsDTO> GetDiscountCouponsDTOList(List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<DiscountCouponsDTO> list = null;
            int count = 0;
            string selectQuery = @"SELECT * FROM DiscountCoupons";
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<DiscountCouponsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == DiscountCouponsDTO.SearchByParameters.COUPON_SET_ID ||
                            searchParameter.Key == DiscountCouponsDTO.SearchByParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == DiscountCouponsDTO.SearchByParameters.DISCOUNT_COUPONS_HEADER_ID ||
                            searchParameter.Key == DiscountCouponsDTO.SearchByParameters.TRANSACTION_ID ||
                            searchParameter.Key == DiscountCouponsDTO.SearchByParameters.PAYMENT_MODE_ID ||
                            searchParameter.Key == DiscountCouponsDTO.SearchByParameters.LINE_ID)
                        {

                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DiscountCouponsDTO.SearchByParameters.SITE_ID)
                        {

                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DiscountCouponsDTO.SearchByParameters.COUPON_NUMBER)
                        {
                            query.Append(joiner + @"((Tonumber is null and FromNumber like'" + "%"+ searchParameter.Value +"%"+
                                "') or Tonumber is not null  and len('" + searchParameter.Value +
                                "') = len(FromNumber) and '" + searchParameter.Value +
                                @"' between isnull(FromNumber, '') and isnull(ToNumber, 'zzzzzzzzzzzzzzzzzzzz') 
                                 or(FromNumber is null and ToNumber is null and Count is not null))");
                        }
                        else if (searchParameter.Key == DiscountCouponsDTO.SearchByParameters.EXPIRY_DATE_GREATER_THAN ||
                                searchParameter.Key == DiscountCouponsDTO.SearchByParameters.START_DATE_GREATER_THAN)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE())>=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == DiscountCouponsDTO.SearchByParameters.EXPIRY_DATE_LESS_THAN ||
                                searchParameter.Key == DiscountCouponsDTO.SearchByParameters.START_DATE_LESS_THAN)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE())<=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == DiscountCouponsDTO.SearchByParameters.IS_ACTIVE) //char
                        {

                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N"));
                        }
                        else if (searchParameter.Key == DiscountCouponsDTO.SearchByParameters.MASTER_HEADER_ID) //int
                        {
                            query.Append(joiner + "DiscountCouponHeaderId in(SELECT dch.Id from DiscountCouponsHeader dch where dch.MasterEntityId =" + dataAccessHandler.GetParameterName(searchParameter.Key) + ")");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DiscountCouponsDTO.SearchByParameters.DISCOUNT_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
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
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }

                }

                selectQuery = selectQuery + query;
            }


            DataTable table = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetDiscountCouponsDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the ProductBarcodeDTO List for ProductBarcodeSet Id List
        /// </summary>
        /// <param name="couponSetIdList">integer list parameter</param>
        /// <returns>Returns List of ProductBarcodeSetDTO</returns>
        public List<DiscountCouponsDTO> GetDiscountCouponsDTOList(List<int> couponSetIdList, bool activeRecords)
        {
            log.LogMethodEntry(couponSetIdList);
            List<DiscountCouponsDTO> list = new List<DiscountCouponsDTO>();
            string query = @"SELECT DiscountCoupons.*
                            FROM DiscountCoupons, @couponSetIdList List
                            WHERE DiscountCoupons.CouponSetId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = 'Y' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@couponSetIdList", couponSetIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetDiscountCouponsDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        ///// <summary>
        /////  Deletes the Discount Coupons record
        ///// </summary>
        ///// <param name="couponSetId">couponSetId is passed as parameter</param>
        //internal void DeleteDiscountCoupons(int couponSetId)
        //{
        //    log.LogMethodEntry(couponSetId);
        //    string query = @"DELETE  
        //                     FROM DiscountCoupons
        //                     WHERE CouponSetId = @couponSetId";
        //    SqlParameter parameter = new SqlParameter("@couponSetId", couponSetId);
        //    dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
        //    log.LogMethodExit();
        //}
    }
}
