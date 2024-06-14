/********************************************************************************************
 * Project Name - DiscountCouponsUsed Data Handler
 * Description  - Data handler of the DiscountCouponsUsed class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        16-Jul-2017   Lakshminarayana     Created
 * 2.60       18-Mar-2019   Akshay Gulaganji    Modified IsActive (string to bool) 
 *2.70        06-Jun-2019   Akshay Gulaganji    Code merge from Development to WebManagementStudio
 *2.70.2      06-Dec-2019   Jinto Thomas        Removed siteid from update query 
 *2.120.2     12-May-2021   Mushahid Faizan      WMS issues for ambigious IsActive column
 *2.140       14-Sep-2021   Fiona                Modified: Issue fixes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    ///  DiscountCouponsUsed Data Handler - Handles insert, update and select of  DiscountCouponsUsed objects
    /// </summary>
    public class DiscountCouponsUsedDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<DiscountCouponsUsedDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<DiscountCouponsUsedDTO.SearchByParameters, string>
            {
                {DiscountCouponsUsedDTO.SearchByParameters.ID, "DiscountCouponsUsed.Id"},
                {DiscountCouponsUsedDTO.SearchByParameters.COUPON_SET_ID, "DiscountCouponsUsed.CouponSetId"},
                {DiscountCouponsUsedDTO.SearchByParameters.DISCOUNT_COUPON_HEADER_ID, "DiscountCouponsUsed.DiscountCouponHeaderId"},
                {DiscountCouponsUsedDTO.SearchByParameters.COUPON_NUMBER, "DiscountCouponsUsed.CouponNumber"},
                {DiscountCouponsUsedDTO.SearchByParameters.TRANSACTION_ID, "DiscountCouponsUsed.TrxId"},
                {DiscountCouponsUsedDTO.SearchByParameters.LINE_ID, "DiscountCouponsUsed.LineId"},
                {DiscountCouponsUsedDTO.SearchByParameters.IS_ACTIVE, "DiscountCouponsUsed.IsActive"},
                {DiscountCouponsUsedDTO.SearchByParameters.MASTER_ENTITY_ID,"DiscountCouponsUsed.MasterEntityId"},
                {DiscountCouponsUsedDTO.SearchByParameters.SITE_ID, "DiscountCouponsUsed.site_id"}
            };
        private readonly DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = @"SELECT DiscountCouponsUsed.*, DiscountCouponsHeader.DiscountId
                                              FROM DiscountCouponsUsed
                                              INNER JOIN DiscountCouponsHeader ON DiscountCouponsUsed.DiscountCouponHeaderId = DiscountCouponsHeader.ID ";
        #region MERGE_QUERY
        private const string MERGE_QUERY = @"DECLARE @Output AS DiscountCouponsUsedType
                                            MERGE INTO DiscountCouponsUsed tbl
                                            USING @discountCouponsUsedList AS src
                                            ON src.Id = tbl.Id
                                            WHEN MATCHED THEN
                                            UPDATE SET
                                            CouponSetId = src.CouponSetId,
                                            CouponNumber = src.CouponNumber,
                                            TrxId = src.TransactionId,
                                            LineId = src.LineId,
                                            Guid = src.Guid,
                                            -- site_id = src.site_id,
                                            MasterEntityId = src.MasterEntityId,
                                            IsActive = src.IsActive,
                                            DiscountCouponHeaderId = src.DiscountCouponHeaderId,
                                            LastUpdatedBy = src.LastUpdatedBy,
                                            LastUpdateDate = GETDATE()

                                            WHEN NOT MATCHED THEN INSERT (
                                            CouponSetId,
                                            CouponNumber,
                                            TrxId,
                                            LineId,
                                            Guid,
                                            site_id,
                                            MasterEntityId,
                                            IsActive,
                                            DiscountCouponHeaderId,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate
                                            )VALUES (
                                            src.CouponSetId,
                                            src.CouponNumber,
                                            src.TransactionId,
                                            src.LineId,
                                            src.Guid,
                                            src.site_id,
                                            src.MasterEntityId,
                                            src.IsActive,
                                            src.DiscountCouponHeaderId,
                                            src.CreatedBy,
                                            src.CreationDate,
                                            src.LastUpdatedBy,
                                            src.LastUpdateDate
                                            )
                                            OUTPUT
                                            inserted.Id,
                                            inserted.CreatedBy,
                                            inserted.CreationDate,
                                            inserted.LastUpdateDate,
                                            inserted.LastUpdatedBy,
                                            inserted.site_id,
                                            inserted.Guid
                                            INTO @Output(
                                            Id,
                                            CreatedBy, 
                                            CreationDate, 
                                            LastUpdateDate, 
                                            LastUpdatedBy, 
                                            site_id, 
                                            Guid);
                                            SELECT * FROM @Output;";
        #endregion

        /// <summary>
        ///  Default constructor of DiscountCouponsUsedDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public DiscountCouponsUsedDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        internal List<DiscountCouponsUsedDTO> GetDiscountCouponsDTOListOfDiscountCoupons(List<int> couponSetIdList, bool activeRecords)//added
        {
            log.LogMethodEntry(couponSetIdList, activeRecords);
            List<DiscountCouponsUsedDTO> discountCouponsUsedDTOList = new List<DiscountCouponsUsedDTO>();
            string query = "Select * from DiscountCouponsUsed" + @" , @couponSetIdList List
                            WHERE CouponSetId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@couponSetIdList", couponSetIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                discountCouponsUsedDTOList = table.Rows.Cast<DataRow>().Select(x => GetDiscountCouponsUsedDTO(x)).ToList();
            }
            log.LogMethodExit(discountCouponsUsedDTOList);
            return discountCouponsUsedDTOList;
        }

        /// <summary>
        /// Inserts the DiscountCoupon record to the database
        /// </summary>
        /// <param name="discountCouponsUsedDTO">DiscountCouponsUsedDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(DiscountCouponsUsedDTO discountCouponsUsedDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(discountCouponsUsedDTO, loginId, siteId);
            Save(new List<DiscountCouponsUsedDTO>() { discountCouponsUsedDTO }, loginId, siteId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the DiscountCoupon record to the database
        /// </summary>
        /// <param name="discountCouponsUsedDTOList">List of DiscountCouponsUsedDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(List<DiscountCouponsUsedDTO> discountCouponsUsedDTOList, string loginId, int siteId)
        {
            log.LogMethodEntry(discountCouponsUsedDTOList, loginId, siteId);
            Dictionary<string, DiscountCouponsUsedDTO> discountCouponsDTOGuidMap = GetDiscountCouponsUsedDTOGuidMap(discountCouponsUsedDTOList);
            List<SqlDataRecord> sqlDataRecords = GetSqlDataRecords(discountCouponsUsedDTOList, loginId, siteId);
            DataTable dataTable = dataAccessHandler.BatchSave(sqlDataRecords,
                                        sqlTransaction,
                                        MERGE_QUERY,
                                        "DiscountCouponsUsedType",
                                        "@DiscountCouponsUsedList");
            UpdateDiscountCouponsUsedDTOList(discountCouponsDTOGuidMap, dataTable);
            log.LogMethodExit();
        }


        /// <summary>
        /// Get Sql DataRecords
        /// </summary>
        /// <param name="discountCouponsUsedDTOList">discountCouponsUsedDTOList</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>List of SqlDataRecord</returns>
        private List<SqlDataRecord> GetSqlDataRecords(List<DiscountCouponsUsedDTO> discountCouponsUsedDTOList, string loginId, int siteId)
        {
            log.LogMethodEntry(discountCouponsUsedDTOList, loginId, siteId);
            List<SqlDataRecord> result = new List<SqlDataRecord>();
            SqlMetaData[] columnStructures = new SqlMetaData[15];
            columnStructures[0] = new SqlMetaData("Id", SqlDbType.Int);
            columnStructures[1] = new SqlMetaData("CouponSetId", SqlDbType.Int);
            columnStructures[2] = new SqlMetaData("CouponNumber", SqlDbType.NVarChar, 40);
            columnStructures[3] = new SqlMetaData("TransactionId", SqlDbType.Int);
            columnStructures[4] = new SqlMetaData("LineId", SqlDbType.Int);
            columnStructures[5] = new SqlMetaData("DiscountCouponHeaderId", SqlDbType.Int);
            columnStructures[6] = new SqlMetaData("Guid", SqlDbType.UniqueIdentifier);
            columnStructures[7] = new SqlMetaData("SynchStatus", SqlDbType.Bit);
            columnStructures[8] = new SqlMetaData("site_id", SqlDbType.Int);
            columnStructures[9] = new SqlMetaData("MasterEntityId", SqlDbType.Int);
            columnStructures[10] = new SqlMetaData("IsActive", SqlDbType.Char, 1);
            columnStructures[11] = new SqlMetaData("CreatedBy", SqlDbType.NVarChar, 50);
            columnStructures[12] = new SqlMetaData("CreationDate", SqlDbType.DateTime);
            columnStructures[13] = new SqlMetaData("LastUpdatedBy", SqlDbType.NVarChar, 50);
            columnStructures[14] = new SqlMetaData("LastUpdateDate", SqlDbType.DateTime);
            for (int i = 0; i < discountCouponsUsedDTOList.Count; i++)
            {
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures);
                dataRecord.SetValue(0, dataAccessHandler.GetParameterValue(discountCouponsUsedDTOList[i].Id, true));
                dataRecord.SetValue(1, dataAccessHandler.GetParameterValue(discountCouponsUsedDTOList[i].CouponSetId, true));
                dataRecord.SetValue(2, dataAccessHandler.GetParameterValue(discountCouponsUsedDTOList[i].CouponNumber));
                dataRecord.SetValue(3, dataAccessHandler.GetParameterValue(discountCouponsUsedDTOList[i].TransactionId, true));
                dataRecord.SetValue(4, dataAccessHandler.GetParameterValue(discountCouponsUsedDTOList[i].LineId, true));
                dataRecord.SetValue(5, dataAccessHandler.GetParameterValue(discountCouponsUsedDTOList[i].DiscountCouponHeaderId, true));
                dataRecord.SetValue(6, dataAccessHandler.GetParameterValue(Guid.Parse(discountCouponsUsedDTOList[i].Guid)));
                dataRecord.SetValue(7, dataAccessHandler.GetParameterValue(discountCouponsUsedDTOList[i].SynchStatus));
                dataRecord.SetValue(8, dataAccessHandler.GetParameterValue(siteId, true));
                dataRecord.SetValue(9, dataAccessHandler.GetParameterValue(discountCouponsUsedDTOList[i].MasterEntityId, true));
                dataRecord.SetValue(10, dataAccessHandler.GetParameterValue(discountCouponsUsedDTOList[i].IsActive == true ? "Y" : "N"));//modified
                dataRecord.SetValue(11, dataAccessHandler.GetParameterValue(loginId));
                dataRecord.SetValue(12, dataAccessHandler.GetParameterValue(discountCouponsUsedDTOList[i].CreationDate));
                dataRecord.SetValue(13, dataAccessHandler.GetParameterValue(loginId));
                dataRecord.SetValue(14, dataAccessHandler.GetParameterValue(discountCouponsUsedDTOList[i].LastUpdatedDate));
                result.Add(dataRecord);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the DiscountCouponsUsed DTO Guid Map
        /// </summary>
        /// <param name="discountCouponsUsedDTOList">discountCouponsUsedDTOList</param>
        /// <returns>Dictionary of string, DiscountCouponsUsedDTO </returns>
        private Dictionary<string, DiscountCouponsUsedDTO> GetDiscountCouponsUsedDTOGuidMap(List<DiscountCouponsUsedDTO> discountCouponsUsedDTOList)
        {
            Dictionary<string, DiscountCouponsUsedDTO> result = new Dictionary<string, DiscountCouponsUsedDTO>();
            for (int i = 0; i < discountCouponsUsedDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(discountCouponsUsedDTOList[i].Guid))
                {
                    discountCouponsUsedDTOList[i].Guid = Guid.NewGuid().ToString();
                }
                result.Add(discountCouponsUsedDTOList[i].Guid, discountCouponsUsedDTOList[i]);
            }
            return result;
        }

        /// <summary>
        /// Update DiscountCouponsUsed DTO List
        /// </summary>
        /// <param name="discountCouponsUsedDTOGuidMap">discountCouponsUsedDTOGuidMap</param>
        /// <param name="table">table</param>
        private void UpdateDiscountCouponsUsedDTOList(Dictionary<string, DiscountCouponsUsedDTO> discountCouponsUsedDTOGuidMap, DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                DiscountCouponsUsedDTO discountCouponsUsedDTO = discountCouponsUsedDTOGuidMap[Convert.ToString(row["Guid"])];
                discountCouponsUsedDTO.Id = row["Id"] == DBNull.Value ? -1 : Convert.ToInt32(row["Id"]);
                discountCouponsUsedDTO.CreatedBy = row["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["CreatedBy"]);
                discountCouponsUsedDTO.CreationDate = row["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreationDate"]);
                discountCouponsUsedDTO.LastUpdatedBy = row["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["LastUpdatedBy"]);
                discountCouponsUsedDTO.LastUpdatedDate = row["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["LastUpdateDate"]);
                discountCouponsUsedDTO.SiteId = row["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["site_id"]);
                discountCouponsUsedDTO.AcceptChanges();
            }
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

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating DiscountCouponsUsed Record.
        /// </summary>
        /// <param name="discountCouponsUsedDTO">DiscountCouponsUsedDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(DiscountCouponsUsedDTO discountCouponsUsedDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(discountCouponsUsedDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            ParameterHelper(parameters, "@Id", discountCouponsUsedDTO.Id, true);
            ParameterHelper(parameters, "@CouponSetId", discountCouponsUsedDTO.CouponSetId, true);
            ParameterHelper(parameters, "@DiscountCouponHeaderId", discountCouponsUsedDTO.DiscountCouponHeaderId, true);
            ParameterHelper(parameters, "@CouponNumber", discountCouponsUsedDTO.CouponNumber);
            ParameterHelper(parameters, "@TrxId", discountCouponsUsedDTO.TransactionId, true);
            ParameterHelper(parameters, "@LineId", discountCouponsUsedDTO.LineId, true);
            ParameterHelper(parameters, "@IsActive", discountCouponsUsedDTO.IsActive ? "Y" : "N");
            ParameterHelper(parameters, "@site_id", siteId, true);
            ParameterHelper(parameters, "@CreatedBy", loginId);
            ParameterHelper(parameters, "@LastUpdatedBy", loginId);
            ParameterHelper(parameters, "@MasterEntityId", discountCouponsUsedDTO.MasterEntityId, true);
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the DiscountCouponsUsed record to the database
        /// </summary>
        /// <param name="discountCouponsUsedDTO">DiscountCouponsUsedDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns DiscountCouponsUsedDTO</returns>
        public DiscountCouponsUsedDTO InsertDiscountCouponsUsed(DiscountCouponsUsedDTO discountCouponsUsedDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(discountCouponsUsedDTO, loginId, siteId);
            string query = @"INSERT INTO DiscountCouponsUsed 
                                        ( 
                                            CouponSetId,
                                            DiscountCouponHeaderId,
                                            CouponNumber,
                                            TrxId,
                                            LineId,
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
                                            @CouponSetId,
                                            @DiscountCouponHeaderId,
                                            @CouponNumber,
                                            @TrxId,
                                            @LineId,
                                            @IsActive,
                                            @site_id,
                                            @MasterEntityId,
                                            @CreatedBy,
                                            GetDate(),
                                            @LastUpdatedBy,
                                            GetDate()
                                        ) SELECT* from DiscountCouponsUsed where Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, BuildSQLParameters(discountCouponsUsedDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDiscountCouponsUsedDTO(discountCouponsUsedDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting discountCouponsUsedDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(discountCouponsUsedDTO);
            return discountCouponsUsedDTO;
        }

        /// <summary>
        /// Updates the DiscountCouponsUsed record
        /// </summary>
        /// <param name="discountCouponsUsedDTO">DiscountCouponsUsedDTO type parameter</param>
        /// <param name="loginId">User updating the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns DiscountCouponsUsedDTO</returns>
        public DiscountCouponsUsedDTO UpdateDiscountCouponsUsed(DiscountCouponsUsedDTO discountCouponsUsedDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(discountCouponsUsedDTO, loginId, siteId);
            string query = @"UPDATE DiscountCouponsUsed 
                             SET CouponSetId = @CouponSetId,
                                 DiscountCouponHeaderId=@DiscountCouponHeaderId,
                                 CouponNumber=@CouponNumber,
                                 TrxId=@TrxId,
                                 LineId=@LineId,
                                 IsActive=@IsActive,
                                 MasterEntityId=@MasterEntityId,
                                 LastUpdatedBy=@LastUpdatedBy,
                                 LastUpdateDate = getDate()
                             WHERE  Id = @Id 
                            SELECT * from DiscountCouponsUsed where Id = @Id ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, BuildSQLParameters(discountCouponsUsedDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDiscountCouponsUsedDTO(discountCouponsUsedDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating discountCouponsUsedDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(discountCouponsUsedDTO);
            return discountCouponsUsedDTO;
        }


        /// <summary>
        ///  updates the DiscountCouponsUsedDTO with Id ,who columns values for further process.
        /// </summary>
        /// <param name="discountCouponsUsedDTO">DiscountCouponsUsedDTO object is passed</param>
        /// <param name="dt">dt an object of DataTable</param>
        private void RefreshDiscountCouponsUsedDTO(DiscountCouponsUsedDTO discountCouponsUsedDTO, DataTable dt)
        {
            log.LogMethodEntry(discountCouponsUsedDTO, dt);
            if (dt.Rows.Count > 0)
            {
               discountCouponsUsedDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
               discountCouponsUsedDTO.LastUpdatedDate = dt.Rows[0]["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["LastUpdateDate"]);
               discountCouponsUsedDTO.Guid = dt.Rows[0]["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["Guid"]);
               discountCouponsUsedDTO.LastUpdatedBy = dt.Rows[0]["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["LastUpdatedBy"]);
               discountCouponsUsedDTO.SiteId = dt.Rows[0]["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[0]["site_id"]);
               discountCouponsUsedDTO.CreatedBy = dt.Rows[0]["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["CreatedBy"]);
                discountCouponsUsedDTO.CreationDate = dt.Rows[0]["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Deletes the DiscountCouponsUsed data of passed Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns no of rows deleted</returns>
        public int Delete(int id)
        {
            log.LogMethodEntry(id);
            string query = @"DELETE FROM DiscountCouponsUsed
                            WHERE Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            int noOfRowsDeleted = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(noOfRowsDeleted);
            return noOfRowsDeleted;
        }

        /// <summary>
        /// Converts the Data row object to DiscountCouponsUsedDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns DiscountCouponsUsedDTO</returns>
        private DiscountCouponsUsedDTO GetDiscountCouponsUsedDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            DiscountCouponsUsedDTO discountCouponsUsedDTO = new DiscountCouponsUsedDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["CouponSetId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CouponSetId"]),
                                            dataRow["DiscountId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DiscountId"]),
                                            dataRow["DiscountCouponHeaderId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DiscountCouponHeaderId"]),
                                            dataRow["CouponNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["CouponNumber"]),
                                            dataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxId"]),
                                            dataRow["LineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LineId"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToString(dataRow["IsActive"]) == "Y",
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString(),
                                            dataRow["CreatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                            );
            log.LogMethodExit();
            return discountCouponsUsedDTO;
        }

        /// <summary>
        /// Gets the DiscountCouponsUsed data of passed DiscountCouponsUsed Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns DiscountCouponsUsedDTO</returns>
        public DiscountCouponsUsedDTO GetDiscountCouponsUsedDTO(int id)
        {
            log.LogMethodEntry(id);
            DiscountCouponsUsedDTO returnValue = null;
            string query = SELECT_QUERY + @" WHERE Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetDiscountCouponsUsedDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue, "returnValue");
            return returnValue;
        }

        /// <summary>
        /// Gets the DiscountCouponsUsedDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of DiscountCouponsUsedDTO matching the search criteria</returns>
        public List<DiscountCouponsUsedDTO> GetDiscountCouponsUsedDTOList(List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<DiscountCouponsUsedDTO> list = null;
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == DiscountCouponsUsedDTO.SearchByParameters.ID ||
                            searchParameter.Key == DiscountCouponsUsedDTO.SearchByParameters.COUPON_SET_ID ||
                            searchParameter.Key == DiscountCouponsUsedDTO.SearchByParameters.DISCOUNT_COUPON_HEADER_ID ||
                            searchParameter.Key == DiscountCouponsUsedDTO.SearchByParameters.TRANSACTION_ID ||
                            searchParameter.Key == DiscountCouponsUsedDTO.SearchByParameters.LINE_ID ||
                            searchParameter.Key == DiscountCouponsUsedDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {

                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DiscountCouponsUsedDTO.SearchByParameters.COUPON_NUMBER)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",'') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == DiscountCouponsUsedDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DiscountCouponsUsedDTO.SearchByParameters.IS_ACTIVE) //char
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
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

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<DiscountCouponsUsedDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DiscountCouponsUsedDTO discountCouponsUsedDTO = GetDiscountCouponsUsedDTO(dataRow);
                    list.Add(discountCouponsUsedDTO);
                }
                log.LogMethodExit();
            }
            else
            {
                log.LogMethodExit();
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
