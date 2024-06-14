/********************************************************************************************
 * Project Name - ProductSubscriptionDataHandler
 * Description  - Data handler of the ProductSubscription 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     09-Dec-2020    Fiona            Created for Subscription changes                                                                               
 *2.120.0     18-Mar-2021    Guru S A         For Subscription phase 2 changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using System.Globalization;
using Microsoft.SqlServer.Server;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// ProductSubscriptionDataHandler
    /// </summary>
    public class ProductSubscriptionDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ProductSubscription AS ps ";
        private static readonly Dictionary<ProductSubscriptionDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ProductSubscriptionDTO.SearchByParameters, string>
            {
                {ProductSubscriptionDTO.SearchByParameters.PRODUCTS_ID, "ps.ProductsId"},
                {ProductSubscriptionDTO.SearchByParameters.IS_ACTIVE, "ps.IsActive"},
                {ProductSubscriptionDTO.SearchByParameters.ID, "ps.ProductSubscriptionId"},
                {ProductSubscriptionDTO.SearchByParameters.MASTER_ENTITY_ID, "ps.MasterEntityId"},
                {ProductSubscriptionDTO.SearchByParameters.SITE_ID, "ps.site_id"},
                {ProductSubscriptionDTO.SearchByParameters.PRODUCTS_ID_LIST, "ps.ProductsId"}

            }; 
                                            

        private const string MERGE_QUERY = @"DECLARE @Output AS ProductSubscriptionType;
                                            MERGE INTO ProductSubscription tbl
                                            USING @ProductSubscriptionList AS src
                                            ON src.ProductSubscriptionId = tbl.ProductSubscriptionId
                                            WHEN MATCHED THEN
                                            UPDATE SET
                                            ProductsId = src.ProductsId,
                                            ProductSubscriptionName = src.ProductSubscriptionName,
                                            ProductSubscriptionDescription = src.ProductSubscriptionDescription,
                                            SubscriptionPrice = src.SubscriptionPrice,
                                            SubscriptionCycle = src.SubscriptionCycle,
                                            UnitOfSubscriptionCycle = src.UnitOfSubscriptionCycle,
                                            SubscriptionCycleValidity = src.SubscriptionCycleValidity,
                                            --SeasonalSubscription = src.SeasonalSubscription,
                                            SeasonStartDate = src.SeasonStartDate,
                                            --SeasonEndDate = src.SeasonEndDate,
                                            FreeTrialPeriodCycle = src.FreeTrialPeriodCycle,
                                            AllowPause = src.AllowPause,
                                            BillInAdvance = src.BillInAdvance,
                                            PaymentCollectionMode = src.PaymentCollectionMode, 
                                            AutoRenew = src.AutoRenew,
                                            AutoRenewalMarkupPercent = src.AutoRenewalMarkupPercent,
                                            RenewalGracePeriodCycle = src.RenewalGracePeriodCycle,
                                            NoOfRenewalReminders = src.NoOfRenewalReminders,
                                            ReminderFrequencyInDays=src.ReminderFrequencyInDays,
                                            SendFirstReminderBeforeXDays=src.SendFirstReminderBeforeXDays,   
                                            CancellationOption=src.CancellationOption,                                            
                                            IsActive=src.IsActive,                                            
                                            LastUpdatedBy=src.LastUpdatedBy,
                                            LastUpdatedDate=GETDATE(),
                                            SynchStatus=src.SynchStatus,                                            
                                            site_id=src.site_id,
                                            MasterEntityId = src.MasterEntityId
                                            
                                            WHEN NOT MATCHED THEN INSERT (
                                            ProductsId,
                                            ProductSubscriptionName,
                                            ProductSubscriptionDescription,
                                            SubscriptionPrice,
                                            SubscriptionCycle,
                                            UnitOfSubscriptionCycle,
                                            SubscriptionCycleValidity,
                                            --SeasonalSubscription,
                                            SeasonStartDate,
                                            --SeasonEndDate,
                                            FreeTrialPeriodCycle,
                                            AllowPause,
                                            BillInAdvance,
                                            PaymentCollectionMode,
                                            AutoRenew,
                                            AutoRenewalMarkupPercent,
                                            RenewalGracePeriodCycle,
                                            NoOfRenewalReminders,
                                            ReminderFrequencyInDays,
                                            SendFirstReminderBeforeXDays, 
                                            CancellationOption,
                                            IsActive,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdatedDate,
                                            SynchStatus,                                            
                                            site_id,
                                            MasterEntityId,
                                            Guid
                                            )VALUES (
                                            src.ProductsId,
                                            src.ProductSubscriptionName,
                                            src.ProductSubscriptionDescription,
                                            src.SubscriptionPrice,
                                            src.SubscriptionCycle,
                                            src.UnitOfSubscriptionCycle,
                                            src.SubscriptionCycleValidity,
                                            --src.SeasonalSubscription,
                                            src.SeasonStartDate,
                                            --src.SeasonEndDate,
                                            src.FreeTrialPeriodCycle,
                                            src.AllowPause,
                                            src.BillInAdvance,
                                            src.PaymentCollectionMode,
                                            src.AutoRenew,
                                            src.AutoRenewalMarkupPercent, 
                                            src.RenewalGracePeriodCycle,
                                            src.NoOfRenewalReminders,
                                            src.ReminderFrequencyInDays,
                                            src.SendFirstReminderBeforeXDays, 
                                            src.CancellationOption,
                                            src.IsActive,
                                            src.CreatedBy,
                                            GETDATE(),
                                            src.LastUpdatedBy,
                                            GETDATE(),
                                            src.SynchStatus,                                            
                                            src.Site_id,
                                            src.MasterEntityId,
                                            src.Guid
                                            )
                                            OUTPUT
                                            inserted.ProductSubscriptionId,
                                            inserted.CreatedBy,
                                            inserted.CreationDate,
                                            inserted.LastUpdatedDate,
                                            inserted.LastUpdatedBy,
                                            inserted.site_id,
                                            inserted.Guid
                                            INTO @Output(
                                            ProductSubscriptionId,
                                            CreatedBy, 
                                            CreationDate, 
                                            LastUpdatedDate, 
                                            LastUpdatedBy, 
                                            site_id, 
                                            Guid);
                                            SELECT * FROM @Output;";
        /// <summary>
        /// Parameterized Constructor for ProductSubscriptionDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public ProductSubscriptionDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

       /// <summary>
       /// Save
       /// </summary>
       /// <param name="productSubscriptionDTOList"></param>
       /// <param name="userId"></param>
       /// <param name="siteId"></param>

        public void Save(List<ProductSubscriptionDTO> productSubscriptionDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(productSubscriptionDTOList, userId, siteId);
            Dictionary<string, ProductSubscriptionDTO> ProductSubscriptionDTOGuidMap = GetProductSubscriptionDTOGuidMap(productSubscriptionDTOList);
            List<SqlDataRecord> sqlDataRecords = GetSqlDataRecords(productSubscriptionDTOList, userId, siteId);
            DataTable dataTable = dataAccessHandler.BatchSave(sqlDataRecords,
                                                                sqlTransaction,
                                                                MERGE_QUERY,
                                                                "ProductSubscriptionType",
                                                                "@ProductSubscriptionList");//
            UpdateProductSubscriptionDTOList(ProductSubscriptionDTOGuidMap, dataTable);
            log.LogMethodExit();
        }
        /// <summary>
        /// Inserts the productSubscription record to the database
        /// </summary>
        /// <param name="productSubscriptionDTO">ProductSubscriptionDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(ProductSubscriptionDTO productSubscriptionDTO, string userId, int siteId)
        {
            log.LogMethodEntry(productSubscriptionDTO, userId, siteId);
            Save(new List<ProductSubscriptionDTO>() { productSubscriptionDTO }, userId, siteId);
            log.LogMethodExit();
        }

        private List<SqlDataRecord> GetSqlDataRecords(List<ProductSubscriptionDTO> productSubscriptionDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(productSubscriptionDTOList, userId, siteId);
            List<SqlDataRecord> result = new List<SqlDataRecord>();
            SqlMetaData[] columnStructures = new SqlMetaData[29]; 
            columnStructures[0] = new SqlMetaData("ProductSubscriptionId", SqlDbType.Int);
            columnStructures[1] = new SqlMetaData("ProductsId", SqlDbType.Int);
            columnStructures[2] = new SqlMetaData("ProductSubscriptionName", SqlDbType.NVarChar, 100);
            columnStructures[3] = new SqlMetaData("ProductSubscriptionDescription", SqlDbType.NVarChar, 2000);
            columnStructures[4] = new SqlMetaData("SubscriptionPrice", SqlDbType.Decimal, 18, 4);
            columnStructures[5] = new SqlMetaData("SubscriptionCycle", SqlDbType.Int);
            columnStructures[6] = new SqlMetaData("UnitOfSubscriptionCycle", SqlDbType.Char, 1);
            columnStructures[7] = new SqlMetaData("SubscriptionCycleValidity", SqlDbType.Int);
            //columnStructures[8] = new SqlMetaData("SeasonalSubscription", SqlDbType.Bit);
            columnStructures[8] = new SqlMetaData("SeasonStartDate", SqlDbType.DateTime);
            //columnStructures[10] = new SqlMetaData("SeasonEndDate", SqlDbType.DateTime);
            columnStructures[9] = new SqlMetaData("FreeTrialPeriodCycle", SqlDbType.Int);
            columnStructures[10] = new SqlMetaData("AllowPause", SqlDbType.Bit);
            columnStructures[11] = new SqlMetaData("BillInAdvance", SqlDbType.Bit);
            columnStructures[12] = new SqlMetaData("PaymentCollectionMode", SqlDbType.Char, 1);
            columnStructures[13] = new SqlMetaData("AutoRenew", SqlDbType.Bit);
            columnStructures[14] = new SqlMetaData("AutoRenewalMarkupPercent", SqlDbType.Decimal, 18, 4);
            columnStructures[15] = new SqlMetaData("RenewalGracePeriodCycle", SqlDbType.Int);
            columnStructures[16] = new SqlMetaData("NoOfRenewalReminders", SqlDbType.Int);
            columnStructures[17] = new SqlMetaData("ReminderFrequencyInDays", SqlDbType.Int);
            columnStructures[18] = new SqlMetaData("SendFirstReminderBeforeXDays", SqlDbType.Int);
            columnStructures[19] = new SqlMetaData("CancellationOption", SqlDbType.Char, 1);

            columnStructures[20] = new SqlMetaData("IsActive", SqlDbType.Bit);
            columnStructures[21] = new SqlMetaData("CreatedBy", SqlDbType.NVarChar, 50);
            columnStructures[22] = new SqlMetaData("CreationDate", SqlDbType.DateTime);
            columnStructures[23] = new SqlMetaData("LastUpdatedBy", SqlDbType.NVarChar, 50);
            columnStructures[24] = new SqlMetaData("LastUpdatedDate", SqlDbType.DateTime);

            columnStructures[25] = new SqlMetaData("Guid", SqlDbType.UniqueIdentifier);
            columnStructures[26] = new SqlMetaData("SynchStatus", SqlDbType.Bit);
            columnStructures[27] = new SqlMetaData("site_id", SqlDbType.Int);
            columnStructures[28] = new SqlMetaData("MasterEntityId", SqlDbType.Int);
            for (int i = 0; i < productSubscriptionDTOList.Count; i++)
            {
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures); 

                dataRecord.SetValue(0, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].ProductSubscriptionId, true));
                dataRecord.SetValue(1, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].ProductsId, true));
                dataRecord.SetValue(2, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].ProductSubscriptionName));
                dataRecord.SetValue(3, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].ProductSubscriptionDescription));
                dataRecord.SetValue(4, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].SubscriptionPrice));
                dataRecord.SetValue(5, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].SubscriptionCycle));
                dataRecord.SetValue(6, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].UnitOfSubscriptionCycle));
                dataRecord.SetValue(7, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].SubscriptionCycleValidity));
                //dataRecord.SetValue(8, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].SeasonalSubscription));
                dataRecord.SetValue(8, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].SeasonStartDate));
               // dataRecord.SetValue(10, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].SeasonEndDate));
                dataRecord.SetValue(9, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].FreeTrialPeriodCycle)); 
                dataRecord.SetValue(10, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].AllowPause));
                dataRecord.SetValue(11, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].BillInAdvance));
                dataRecord.SetValue(12, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].PaymentCollectionMode));
                dataRecord.SetValue(13, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].AutoRenew));
                dataRecord.SetValue(14, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].AutoRenewalMarkupPercent)); 
                dataRecord.SetValue(15, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].RenewalGracePeriodCycle));
                dataRecord.SetValue(16, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].NoOfRenewalReminders));
                dataRecord.SetValue(17, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].ReminderFrequencyInDays));
                dataRecord.SetValue(18, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].SendFirstReminderBeforeXDays));
                dataRecord.SetValue(19, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].CancellationOption));


                dataRecord.SetValue(20, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].IsActive));
                dataRecord.SetValue(21, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].CreatedBy));
                dataRecord.SetValue(22, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].CreationDate));
                dataRecord.SetValue(23, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].LastUpdatedBy));
                dataRecord.SetValue(24, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].LastUpdateDate));

                dataRecord.SetValue(25, dataAccessHandler.GetParameterValue(Guid.Parse(productSubscriptionDTOList[i].Guid)));
                dataRecord.SetValue(26, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].SynchStatus));                
                dataRecord.SetValue(27, dataAccessHandler.GetParameterValue(siteId, true));
                dataRecord.SetValue(28, dataAccessHandler.GetParameterValue(productSubscriptionDTOList[i].MasterEntityId, true));
              
                result.Add(dataRecord);
            }
            log.LogMethodExit(result);
            return result;
        }

        private Dictionary<string, ProductSubscriptionDTO> GetProductSubscriptionDTOGuidMap(List<ProductSubscriptionDTO> productSubscriptionDTOList)
        {
            Dictionary<string, ProductSubscriptionDTO> result = new Dictionary<string, ProductSubscriptionDTO>();
            for (int i = 0; i < productSubscriptionDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(productSubscriptionDTOList[i].Guid))
                {
                    productSubscriptionDTOList[i].Guid = Guid.NewGuid().ToString();
                }
                result.Add(productSubscriptionDTOList[i].Guid, productSubscriptionDTOList[i]);
            }
            return result;
        }
        private void UpdateProductSubscriptionDTOList(Dictionary<string, ProductSubscriptionDTO> ProductSubscriptionDTOGuidMap, DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                ProductSubscriptionDTO productSubscriptionDTO = ProductSubscriptionDTOGuidMap[Convert.ToString(row["Guid"])];
                productSubscriptionDTO.ProductSubscriptionId = row["ProductSubscriptionId"] == DBNull.Value ? -1 : Convert.ToInt32(row["ProductSubscriptionId"]);
                productSubscriptionDTO.CreatedBy = row["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["CreatedBy"]);
                productSubscriptionDTO.CreationDate = row["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreationDate"]);
                productSubscriptionDTO.LastUpdatedBy = row["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["LastUpdatedBy"]);
                productSubscriptionDTO.LastUpdateDate = row["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["LastUpdatedDate"]);
                productSubscriptionDTO.SiteId = row["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["site_id"]);
                productSubscriptionDTO.AcceptChanges();
            }
        }
        /// <summary>
        /// Gets the ProductSubscription data of passed ProductSubscription Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns ProductSubscriptionDTO</returns>
        public ProductSubscriptionDTO GetProductSubscriptionDTO(int id)
        {
            log.LogMethodEntry(id);
            ProductSubscriptionDTO result = null;
            string query = @"SELECT *
                            FROM ProductSubscription
                            WHERE ProductSubscriptionId = @ProductSubscriptionId";
            DataTable table = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { dataAccessHandler.GetSQLParameter("@ProductSubscriptionId", id, true) }, sqlTransaction);
            if (table != null && table.Rows.Count > 0)
            {
                var list = table.Rows.Cast<DataRow>().Select(x => GetProductSubscriptionDTO(x));
                if (list != null)
                {
                    result = list.FirstOrDefault();
                }
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Converts the Data row object to ProductSubscriptionDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns ProductSubscriptionDTO</returns>
        private ProductSubscriptionDTO GetProductSubscriptionDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow); 
            ProductSubscriptionDTO ProductSubscriptionDTO = new ProductSubscriptionDTO(dataRow["ProductSubscriptionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductSubscriptionId"]),
                                            dataRow["ProductsId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductsId"]),
                                            dataRow["ProductSubscriptionName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ProductSubscriptionName"]),
                                            dataRow["ProductSubscriptionDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ProductSubscriptionDescription"]),
                                            dataRow["SubscriptionPrice"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["SubscriptionPrice"]),
                                            dataRow["SubscriptionCycle"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SubscriptionCycle"]),
                                            dataRow["UnitOfSubscriptionCycle"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["UnitOfSubscriptionCycle"]),
                                            dataRow["SubscriptionCycleValidity"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SubscriptionCycleValidity"]),
                                            //dataRow["SeasonalSubscription"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SeasonalSubscription"]),
                                            dataRow["SeasonStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["SeasonStartDate"]),
                                            //dataRow["SeasonEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["StartDate"]),
                                            dataRow["FreeTrialPeriodCycle"] == DBNull.Value ? 0: Convert.ToInt32(dataRow["FreeTrialPeriodCycle"]),
                                            dataRow["AllowPause"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["AllowPause"]),
                                            dataRow["BillInAdvance"] == DBNull.Value ? false: Convert.ToBoolean(dataRow["BillInAdvance"]),
                                            dataRow["PaymentCollectionMode"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["PaymentCollectionMode"]),
                                            dataRow["AutoRenew"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["AutoRenew"]),
                                            dataRow["AutoRenewalMarkupPercent"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["AutoRenewalMarkupPercent"]),
                                            dataRow["RenewalGracePeriodCycle"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["RenewalGracePeriodCycle"]),
                                            dataRow["NoOfRenewalReminders"] == DBNull.Value ? 0: Convert.ToInt32(dataRow["NoOfRenewalReminders"]),
                                            dataRow["SendFirstReminderBeforeXDays"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SendFirstReminderBeforeXDays"]),
                                            dataRow["ReminderFrequencyInDays"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ReminderFrequencyInDays"]),
                                            dataRow["CancellationOption"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CancellationOption"]),
                                            dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString() 
                                            );
            log.LogMethodExit(ProductSubscriptionDTO);
            return ProductSubscriptionDTO;
        }
        /// <summary>
        /// Gets the ProductSubscriptionDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ProductSubscriptionDTO matching the search criteria</returns>
        public List<ProductSubscriptionDTO> GetProductSubscriptionDTOList(List<KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ProductSubscriptionDTO> list = null;
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == ProductSubscriptionDTO.SearchByParameters.ID ||
                            searchParameter.Key == ProductSubscriptionDTO.SearchByParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == ProductSubscriptionDTO.SearchByParameters.PRODUCTS_ID)
                            
                        {

                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProductSubscriptionDTO.SearchByParameters.SITE_ID)
                        {

                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProductSubscriptionDTO.SearchByParameters.PRODUCTS_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }

                        else if (searchParameter.Key == ProductSubscriptionDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
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
                list = table.Rows.Cast<DataRow>().Select(x => GetProductSubscriptionDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the ProductSubscriptionDTO List for Products Id List
        /// </summary>
        /// <param name="productsIdList">integer list parameter</param>
        /// <returns>Returns List of ProductSubscriptionDTO</returns>
        public List<ProductSubscriptionDTO> GetProductSubscriptionDTOList(List<int> productsIdList)
        {
            log.LogMethodEntry(productsIdList);
            List<ProductSubscriptionDTO> list = new List<ProductSubscriptionDTO>();
            string query = @"SELECT ps.* 
                              FROM ProductSubscription AS ps
                                  inner join @productsIdList List on ps.ProductsId = List.Id ";

            DataTable table = dataAccessHandler.BatchSelect(query, "@productsIdList", productsIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetProductSubscriptionDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
