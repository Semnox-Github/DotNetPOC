/********************************************************************************************
 * Project Name - Sales Offer Group Data Handler
 * Description  - The sales offer group data handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        18-Apr-2017   Raghuveera     Created 
 *2.70.2        10-Dec-2019   Jinto Thomas   Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Product
{
    // <summary>
    /// Sales Offer Group Data Handler - Handles insert, update and select of sales offer group  objects
    /// </summary>
    public class SalesOfferGroupDataHandler
    {
         Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string> DBSearchParameters = new Dictionary<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>
            {
                {SalesOfferGroupDTO.SearchBySalesOfferGroupParameters.SALE_GROUP_ID, "SaleGroupId"},
                {SalesOfferGroupDTO.SearchBySalesOfferGroupParameters.NAME, "Name"},
                {SalesOfferGroupDTO.SearchBySalesOfferGroupParameters.IS_ACTIVE, "IsActive"},
                {SalesOfferGroupDTO.SearchBySalesOfferGroupParameters.IS_UPSELL, "IsUpsell"},
                {SalesOfferGroupDTO.SearchBySalesOfferGroupParameters.MASTER_ENTITY_ID,"MasterEntityId"},                
                {SalesOfferGroupDTO.SearchBySalesOfferGroupParameters.SITE_ID, "site_id"}
            };
         DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of SalesOfferGroupDataHandler class
        /// </summary>
        public SalesOfferGroupDataHandler()
        {
            log.Debug("Starts-SalesOfferGroupDataHandler() default constructor.");
            dataAccessHandler = new  DataAccessHandler();
            log.Debug("Ends-SalesOfferGroupDataHandler() default constructor.");
        }

        /// <summary>
        /// Inserts the sales offer group record to the database
        /// </summary>
        /// <param name="salesOfferGroup">SalesOfferGroupDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTrxn">Sql transaction</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertSalesOfferGroup(SalesOfferGroupDTO salesOfferGroup, string userId, int siteId, SqlTransaction sqlTrxn)
        {
            log.Debug("Starts-InsertSalesOfferGroup(salesOfferGroup, userId, siteId,sqlTrxn) method.");
            string insertSalesOfferGroupQuery = @"INSERT INTO SalesOfferGroup 
                                                        ( 
                                                        Name,
                                                        IsUpsell,
                                                        IsActive,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedDate,
                                                        LastUpdatedUser,
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId
                                                        ) 
                                                values 
                                                        (                                                         
                                                        @name,
                                                        @isUpsell,
                                                        @isActive,
                                                        @createdBy,
                                                        Getdate(),
                                                        Getdate(),                                                         
                                                        @lastUpdatedBy,
                                                        NEWID(),
                                                        @siteid,
                                                        @masterEntityId
                                                        )SELECT CAST(scope_identity() AS int)";
            List<SqlParameter> updateSalesOfferGroupParameters = new List<SqlParameter>();
            if (string.IsNullOrEmpty(salesOfferGroup.Name))
            {
                throw new Exception("Name should not be null.");
            }
            else
            {
                updateSalesOfferGroupParameters.Add(new SqlParameter("@name", salesOfferGroup.Name));
            }

            updateSalesOfferGroupParameters.Add(new SqlParameter("@isUpsell", salesOfferGroup.IsUpsell));
            updateSalesOfferGroupParameters.Add(new SqlParameter("@isActive", salesOfferGroup.IsActive));
            if (salesOfferGroup.MasterEntityId == -1)
            {
                updateSalesOfferGroupParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateSalesOfferGroupParameters.Add(new SqlParameter("@masterEntityId", salesOfferGroup.MasterEntityId));
            }
            updateSalesOfferGroupParameters.Add(new SqlParameter("@createdBy", userId));
            updateSalesOfferGroupParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
                updateSalesOfferGroupParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            else
                updateSalesOfferGroupParameters.Add(new SqlParameter("@siteId", siteId));
            
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertSalesOfferGroupQuery, updateSalesOfferGroupParameters.ToArray(), sqlTrxn);
            log.Debug("Ends-InsertSalesOfferGroup(salesOfferGroup, userId, siteId,sqlTrxn) method.");
            return idOfRowInserted;
        }
        

        /// <summary>
        /// Updates the sales offer group record
        /// </summary>
        /// <param name="salesOfferGroup">SalesOfferGroupDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTrxn">Sql transaction</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateSalesOfferGroup(SalesOfferGroupDTO salesOfferGroup, string userId, int siteId, SqlTransaction sqlTrxn)
        {
            log.Debug("Starts-UpdateSalesOfferGroup(salesOfferGroup, userId, siteId,sqlTrxn) method.");
            string updateSalesOfferGroupQuery = @"UPDATE SalesOfferGroup 
                                         set Name = @name,
                                             IsUpsell = @isUpsell,
                                             IsActive = @isActive,
                                             LastUpdatedUser = @lastUpdatedBy,
                                             LastUpdatedDate = getdate(),
                                             -- site_id = @siteid,
                                             MasterEntityId = @masterEntityId                                           
                                       WHERE SaleGroupId = @saleGroupId";
            List<SqlParameter> updateSalesOfferGroupParameters = new List<SqlParameter>();
            updateSalesOfferGroupParameters.Add(new SqlParameter("@saleGroupId", salesOfferGroup.SaleGroupId));
            if (string.IsNullOrEmpty(salesOfferGroup.Name))
            {
                throw new Exception("Name should not be null.");
            }
            else
            {
                updateSalesOfferGroupParameters.Add(new SqlParameter("@name", salesOfferGroup.Name));
            }
            updateSalesOfferGroupParameters.Add(new SqlParameter("@isUpsell", salesOfferGroup.IsUpsell));
            updateSalesOfferGroupParameters.Add(new SqlParameter("@isActive", salesOfferGroup.IsActive));
            if (salesOfferGroup.MasterEntityId == -1)
            {
                updateSalesOfferGroupParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateSalesOfferGroupParameters.Add(new SqlParameter("@masterEntityId", salesOfferGroup.MasterEntityId));
            }
            updateSalesOfferGroupParameters.Add(new SqlParameter("@createdBy", userId));
            updateSalesOfferGroupParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
                updateSalesOfferGroupParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            else
                updateSalesOfferGroupParameters.Add(new SqlParameter("@siteId", siteId));            
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateSalesOfferGroupQuery, updateSalesOfferGroupParameters.ToArray(), sqlTrxn);
            log.Debug("Ends-UpdateSalesOfferGroup(salesOfferGroup, userId, siteId,sqlTrxn) method.");
            return rowsUpdated;
        }        

        /// <summary>
        /// Converts the Data row object to SalesOfferGroupDTO class type
        /// </summary>
        /// <param name="salesOfferGroupDataRow">SalesOfferGroup DataRow</param>
        /// <returns>Returns SalesOfferGroup</returns>
        private SalesOfferGroupDTO GetSalesOfferGroupDTO(DataRow salesOfferGroupDataRow)
        {
            log.Debug("Starts-GetSalesOfferGroupDTO(salesOfferGroupDataRow) method.");
            SalesOfferGroupDTO salesOfferGroupDataObject = new SalesOfferGroupDTO(Convert.ToInt32(salesOfferGroupDataRow["SaleGroupId"]),
                                            salesOfferGroupDataRow["Name"].ToString(),
                                            salesOfferGroupDataRow["IsUpsell"] == DBNull.Value ? false : Convert.ToBoolean(salesOfferGroupDataRow["IsUpsell"]),
                                            salesOfferGroupDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(salesOfferGroupDataRow["IsActive"]),
                                            salesOfferGroupDataRow["CreatedBy"].ToString(),
                                            salesOfferGroupDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(salesOfferGroupDataRow["CreationDate"]),
                                            salesOfferGroupDataRow["LastUpdatedUser"].ToString(),
                                            salesOfferGroupDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(salesOfferGroupDataRow["LastUpdatedDate"]),
                                            salesOfferGroupDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(salesOfferGroupDataRow["site_id"]),
                                            salesOfferGroupDataRow["Guid"].ToString(),                                            
                                            salesOfferGroupDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(salesOfferGroupDataRow["SynchStatus"]),
                                            salesOfferGroupDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(salesOfferGroupDataRow["MasterEntityId"])                                            
                                            );
            log.Debug("Ends-GetSalesOfferGroupDTO(salesOfferGroupDataRow) method.");
            return salesOfferGroupDataObject;
        }

        /// <summary>
        /// Gets the sales offer group record which matches the locker card id  
        /// </summary>
        /// <param name="saleGroupId">integer type parameter</param>
        /// <returns>Returns SalesOfferGroupDTO</returns>
        public SalesOfferGroupDTO GetSalesOfferGroup(int saleGroupId)
        {
            log.Debug("Starts-GetSalesOfferGroupOnCardId(saleGroupId) method.");
            string selectSalesOfferGroupQuery = @"SELECT *
                                                   FROM SalesOfferGroup
                                                   WHERE SaleGroupId = @saleGroupId";
            SqlParameter[] selectSalesOfferGroupParameters = new SqlParameter[1];
            selectSalesOfferGroupParameters[0] = new SqlParameter("@saleGroupId", saleGroupId);
            DataTable salesOfferGroup = dataAccessHandler.executeSelectQuery(selectSalesOfferGroupQuery, selectSalesOfferGroupParameters);
            if (salesOfferGroup.Rows.Count > 0)
            {
                DataRow salesOfferGroupRow = salesOfferGroup.Rows[0];
                SalesOfferGroupDTO salesOfferGroupDataObject = GetSalesOfferGroupDTO(salesOfferGroupRow);
                log.Debug("Ends-GetSalesOfferGroupOnCardId(saleGroupId) method by returning salesOfferGroupDataObject.");
                return salesOfferGroupDataObject;
            }
            else
            {
                log.Debug("Ends-GetSalesOfferGroupOnCardId(saleGroupId) method by returning null.");
                return null;
            }
        }

        /// <summary>
        /// Gets the SalesOfferGroupDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of SalesOfferGroupDTO matching the search criteria</returns>
        public List<SalesOfferGroupDTO> GetSalesOfferGroupList(List<KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetSalesOfferGroupList(searchParameters) method.");
            int count = 0;
            string selectSalesOfferGroupQuery = @"SELECT *
                                              FROM SalesOfferGroup";
            if (searchParameters != null)
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == SalesOfferGroupDTO.SearchBySalesOfferGroupParameters.SALE_GROUP_ID
                            || searchParameter.Key == SalesOfferGroupDTO.SearchBySalesOfferGroupParameters.MASTER_ENTITY_ID                            
                            || searchParameter.Key == SalesOfferGroupDTO.SearchBySalesOfferGroupParameters.IS_UPSELL)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == SalesOfferGroupDTO.SearchBySalesOfferGroupParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if(searchParameter.Key == SalesOfferGroupDTO.SearchBySalesOfferGroupParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + " ISNULL(" + DBSearchParameters[searchParameter.Key] + ", '1')  = '" + searchParameter.Value + "'");
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetSalesOfferGroupList(searchParameters) method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectSalesOfferGroupQuery = selectSalesOfferGroupQuery + query;
            }
            DataTable salesOfferGroupData = dataAccessHandler.executeSelectQuery(selectSalesOfferGroupQuery, null);
            if (salesOfferGroupData.Rows.Count > 0)
            {
                List<SalesOfferGroupDTO> salesOfferGroupList = new List<SalesOfferGroupDTO>();
                foreach (DataRow salesOfferGroupDataRow in salesOfferGroupData.Rows)
                {
                    SalesOfferGroupDTO salesOfferGroupDataObject = GetSalesOfferGroupDTO(salesOfferGroupDataRow);
                    salesOfferGroupList.Add(salesOfferGroupDataObject);
                }
                log.Debug("Ends-GetSalesOfferGroupList(searchParameters) method by returning salesOfferGroupList.");
                return salesOfferGroupList;
            }
            else
            {
                log.Debug("Ends-GetSalesOfferGroupList(searchParameters) method by returning null.");
                return null;
            }
        }
    }
}
