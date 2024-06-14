using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Product
{
    public class PurchaseTaxDataHandlerTemp
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<PurchaseTaxDTOTemp.SearchByPurchaseTaxParameters, string> DBSearchParameters = new Dictionary<PurchaseTaxDTOTemp.SearchByPurchaseTaxParameters, string>
               {
                    {PurchaseTaxDTOTemp.SearchByPurchaseTaxParameters.TAXID, "TaxId"},
                    {PurchaseTaxDTOTemp.SearchByPurchaseTaxParameters.TAXNAME, "TaxName"},
                    {PurchaseTaxDTOTemp.SearchByPurchaseTaxParameters.SITEID, "site_id"},
                    {PurchaseTaxDTOTemp.SearchByPurchaseTaxParameters.ACTIVEFLAG,"ActiveFlag"}
               };

       DataAccessHandler dataAccessHandler;

        public PurchaseTaxDataHandlerTemp()
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to PurchaseTaxDTO class type
        /// </summary>
        /// <param name="purchaseTaxDataRow">PurchaseTaxDTO DataRow</param>
        /// <returns>Returns PurchaseTaxDTO</returns>
        private PurchaseTaxDTOTemp GetPurchaseTaxDTO(DataRow purchaseTaxDataRow)
        {
            log.LogMethodEntry(purchaseTaxDataRow);

            PurchaseTaxDTOTemp purchaseTaxDataObject = new PurchaseTaxDTOTemp(Convert.ToInt32(purchaseTaxDataRow["TaxId"]),
                                            purchaseTaxDataRow["TaxName"].ToString(),
                                            purchaseTaxDataRow["TaxPercentage"] == DBNull.Value ? -1 : Convert.ToDouble(purchaseTaxDataRow["TaxPercentage"]),
                                            purchaseTaxDataRow["ActiveFlag"] == DBNull.Value ? "N" : Convert.ToString(purchaseTaxDataRow["ActiveFlag"]),
                                            purchaseTaxDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseTaxDataRow["site_id"]),
                                            purchaseTaxDataRow["Guid"].ToString(),
                                            purchaseTaxDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(purchaseTaxDataRow["SynchStatus"]),
                                            purchaseTaxDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(purchaseTaxDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(purchaseTaxDataObject);
            return purchaseTaxDataObject;
        }
        /// <summary>
        /// Gets the vendor data of passed vendor id
        /// </summary>
        /// <param name="UOMId">integer type parameter</param>
        /// <returns>Returns UOMDTO</returns>
        public PurchaseTaxDTOTemp GetPurchaseTax(int TaxId)
        {
            log.LogMethodEntry(TaxId);
            string selectPurchaseTaxQuery = @"select *
                                         from purchasetax
                                        where TaxId = @TaxId";
            SqlParameter[] selectPurchaseTaxParameters = new SqlParameter[1];
            selectPurchaseTaxParameters[0] = new SqlParameter("@TaxId", TaxId);
            DataTable purchaseTax = dataAccessHandler.executeSelectQuery(selectPurchaseTaxQuery, selectPurchaseTaxParameters);
            if (purchaseTax.Rows.Count > 0)
            {
                DataRow purchaseTaxRow = purchaseTax.Rows[0];
                PurchaseTaxDTOTemp purchaseTaxRowDataObject = GetPurchaseTaxDTO(purchaseTaxRow);
                log.LogMethodExit(purchaseTaxRowDataObject);
                return purchaseTaxRowDataObject;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }
        /// <summary>
        /// Returns the purchasetax table columns
        /// </summary>
        /// <returns></returns>
        public DataTable GetPurchaseTaxColumns()
        {
            string selectPurchaseTaxQuery = "Select columns from(Select '' as columns UNION ALL Select COLUMN_NAME as columns from INFORMATION_SCHEMA.COLUMNS  Where TABLE_NAME='PurchaseTax') a order by columns";
            DataTable purchaseTaxTableColumns = dataAccessHandler.executeSelectQuery(selectPurchaseTaxQuery, null);
            return purchaseTaxTableColumns;
        }
        /// <summary>
        /// Retriving uom by passing query
        /// </summary>
        /// <param name="sqlQuery">Query passed for retriving the vendor</param>
        /// <returns> List of PurchaseTaxDTO </returns>
        public List<PurchaseTaxDTOTemp> GetPurchaseTaxList(string sqlQuery)
        {
            log.LogMethodEntry(sqlQuery);
            string Query = sqlQuery.ToUpper();
            if (Query.Contains("DROP") || Query.Contains("UPDATE") || Query.Contains("DELETE"))
            {
                log.LogMethodExit(null);
                return null;
            }
            DataTable purchaseTaxData = dataAccessHandler.executeSelectQuery(sqlQuery, null);
            if (purchaseTaxData.Rows.Count > 0)
            {
                List<PurchaseTaxDTOTemp> purchaseTaxList = new List<PurchaseTaxDTOTemp>();
                foreach (DataRow purchaseTaxDataRow in purchaseTaxData.Rows)
                {
                    PurchaseTaxDTOTemp purchaseTaxDataObject = GetPurchaseTaxDTO(purchaseTaxDataRow);
                    purchaseTaxList.Add(purchaseTaxDataObject);
                }
                log.LogMethodExit(purchaseTaxList);
                return purchaseTaxList; ;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }
        /// <summary>
        /// Gets the UOMDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of VendorDTO matching the search criteria</returns>
        public List<PurchaseTaxDTOTemp> GetPurchaseTaxList(List<KeyValuePair<PurchaseTaxDTOTemp.SearchByPurchaseTaxParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectPurchaseTaxQuery = @"select *
                                         from purchasetax";
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<PurchaseTaxDTOTemp.SearchByPurchaseTaxParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (count == 0)
                        {
                            if (searchParameter.Key.Equals(PurchaseTaxDTOTemp.SearchByPurchaseTaxParameters.TAXID))
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key == PurchaseTaxDTOTemp.SearchByPurchaseTaxParameters.SITEID)
                            {
                                query.Append(" (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else
                            {
                                query.Append("Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') = N'" + searchParameter.Value + "'");
                            }
                        }
                        else
                        {
                            if (searchParameter.Key.Equals(PurchaseTaxDTOTemp.SearchByPurchaseTaxParameters.TAXID))
                            {
                                query.Append(" and " + DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key == PurchaseTaxDTOTemp.SearchByPurchaseTaxParameters.SITEID)
                            {
                                query.Append(" and (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else
                            {
                                query.Append(" and Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') = N'" + searchParameter.Value + "'");
                            }
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit(searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectPurchaseTaxQuery = selectPurchaseTaxQuery + query;
            }

            DataTable purchaseTaxData = dataAccessHandler.executeSelectQuery(selectPurchaseTaxQuery, null);
            if (purchaseTaxData.Rows.Count > 0)
            {
                List<PurchaseTaxDTOTemp> purchaseTaxList = new List<PurchaseTaxDTOTemp>();
                foreach (DataRow purchaseTaxDataRow in purchaseTaxData.Rows)
                {
                    PurchaseTaxDTOTemp purchaseTaxDataObject = GetPurchaseTaxDTO(purchaseTaxDataRow);
                    purchaseTaxList.Add(purchaseTaxDataObject);
                }
                log.LogMethodExit(purchaseTaxList);
                return purchaseTaxList;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }
        /// <summary>
        /// 
        public int InsertPurchaseTax(PurchaseTaxDTOTemp purchaseTax, int siteId, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(purchaseTax, siteId);
            string insertPurchaseTaxQuery = @"INSERT INTO [PurchaseTax]
                                                   ([TaxName]
                                                   ,[TaxPercentage]
                                                   ,[ActiveFlag]
                                                   ,[site_id]
                                                   ,[Guid]
                                                   ,[SynchStatus]
                                                   ,[MasterEntityId])
                                             VALUES
                                                   (@TaxName
                                                   ,@TaxPercentage
                                                   ,@ActiveFlag
                                                   ,@site_id
                                                   ,NewID()
                                                   ,@SynchStatus
                                                   ,@MasterEntityId) SELECT CAST(scope_identity() AS int)";
            List<SqlParameter> updatePurchaseTaxParameters = new List<SqlParameter>();
            updatePurchaseTaxParameters.Add(new SqlParameter("@TaxName", string.IsNullOrEmpty(purchaseTax.TaxName) ? "" : purchaseTax.TaxName));
            updatePurchaseTaxParameters.Add(new SqlParameter("@TaxPercentage", purchaseTax.TaxPercentage == Double.NaN ? 0 : purchaseTax.TaxPercentage));
            updatePurchaseTaxParameters.Add(new SqlParameter("@ActiveFlag", string.IsNullOrEmpty(purchaseTax.ActiveFlag) ? "N" : purchaseTax.ActiveFlag));
            if (siteId == -1)
            {
                updatePurchaseTaxParameters.Add(new SqlParameter("@site_id", DBNull.Value));
            }
            else
            {
                updatePurchaseTaxParameters.Add(new SqlParameter("@site_id", siteId));
            }
            if (purchaseTax.SynchStatus)
            {
                updatePurchaseTaxParameters.Add(new SqlParameter("@SynchStatus", purchaseTax.SynchStatus));
            }
            else
            {
                updatePurchaseTaxParameters.Add(new SqlParameter("@SynchStatus", DBNull.Value));
            }
            if (purchaseTax.MasterEntityId == -1)
            {
                updatePurchaseTaxParameters.Add(new SqlParameter("@MasterEntityID", DBNull.Value));
            }
            else
            {
                updatePurchaseTaxParameters.Add(new SqlParameter("@MasterEntityID", purchaseTax.MasterEntityId));
            }
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertPurchaseTaxQuery, updatePurchaseTaxParameters.ToArray());
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }
        /// <summary>
        /// 
        public int UpdatePurchaseTax(PurchaseTaxDTOTemp purchaseTax, int siteId, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(purchaseTax, siteId);
            string updatePurchaseTaxQuery = @"UPDATE [dbo].[PurchaseTax]
                                       SET [TaxName] = @TaxName
                                          ,[TaxPercentage] = @TaxPercentage
                                          ,[ActiveFlag] = @ActiveFlag
                                          -- ,[site_id] = @site_id
                                          ,[SynchStatus] = @SynchStatus
                                          ,[MasterEntityId] = @MasterEntityId
                                     WHERE TaxId = @TaxId";
            List<SqlParameter> updatePurchaseTaxParameters = new List<SqlParameter>();
            updatePurchaseTaxParameters.Add(new SqlParameter("@TaxId", purchaseTax.TaxId));
            updatePurchaseTaxParameters.Add(new SqlParameter("@TaxName", string.IsNullOrEmpty(purchaseTax.TaxName) ? "" : purchaseTax.TaxName));
            updatePurchaseTaxParameters.Add(new SqlParameter("@TaxPercentage", purchaseTax.TaxPercentage == Double.NaN ? 0 : purchaseTax.TaxPercentage));
            updatePurchaseTaxParameters.Add(new SqlParameter("@ActiveFlag", string.IsNullOrEmpty(purchaseTax.ActiveFlag) ? "N" : purchaseTax.ActiveFlag));
            if (siteId == -1)
            {
                updatePurchaseTaxParameters.Add(new SqlParameter("@site_id", DBNull.Value));
            }
            else
            {
                updatePurchaseTaxParameters.Add(new SqlParameter("@site_id", siteId));
            }
            if (purchaseTax.SynchStatus)
            {
                updatePurchaseTaxParameters.Add(new SqlParameter("@SynchStatus", purchaseTax.SynchStatus));
            }
            else
            {
                updatePurchaseTaxParameters.Add(new SqlParameter("@SynchStatus", DBNull.Value));
            }
            if (purchaseTax.MasterEntityId == -1)
            {
                updatePurchaseTaxParameters.Add(new SqlParameter("@MasterEntityID", DBNull.Value));
            }
            else
            {
                updatePurchaseTaxParameters.Add(new SqlParameter("@MasterEntityID", purchaseTax.MasterEntityId));
            }
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updatePurchaseTaxQuery, updatePurchaseTaxParameters.ToArray());
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }
    }
}
