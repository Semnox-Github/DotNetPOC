/********************************************************************************************
* Project Name - Inventory 
* Description  - Inventory Wastage Summary Data Handler 
* 
**************
**Version Log
**************
*Version     Date          Modified By         Remarks          
*********************************************************************************************
*2.70.2     27-Nov-2019    Girish kundar       Created 
*2.100.0    14-Aug-2020    Deeksha             Modified for Recipe Management Enhancement 
*2.100.0    29-Des-2020    Abhishek             Modified for web API changes 
*2.120.0    12-Apr-2021    Mushahid Faizan       Web Inventory changes
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    public class InventoryWastageSummaryDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        List<SqlParameter> parameters = new List<SqlParameter>();
        private const string SELECT_QUERY = @" SELECT ia.* , c.Name, u.UOM,p.Code,pb.Barcode, p.ProductName, p.Description, 
                                                      l.Name as LocationName , i.InventoryId, i.Quantity,i.LotId as LotId,
                                                      c.CategoryId ,i.UOMId,il.LotNumber
                                                 FROM InventoryAdjustments ia
                                                      left outer join Product p on ia.ProductId = p.Productid
                                                     left outer join (select * 
	                                                    from (
	                                                        select *, row_number() over(partition by productid order by productid, LastUpdatedDate desc) as num 
	                                                        from productbarcode 
	                                                        where isactive = 'Y')v 
	                                                    where num = 1) pb on p.productid = pb.productid
                                                      left outer join UOM u on u.UOMId = p.UomId
                                                      left outer join Category c on c.CategoryId = p.CategoryId
                                                      left outer join Location l on l.locationId = ia.ToLocationId
                                                      left outer join InventoryLot il on isnull(il.LotId,-1) = isnull(ia.LotID,-1) 
                                                      left outer join Inventory i on (i.ProductId = ia.ProductId 
                                                                                      and i.LocationId = ia.FromLocationId
                                                                                      and isnull(i.LotId,-1) = isnull(ia.LotID,-1))
                                                      where  l.Name = 'Wastage' ";
        /// <summary>
        /// Dictionary for searching Parameters for the Inventory object.
        /// </summary>
        private static readonly Dictionary<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string> DBSearchParameters = new Dictionary<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>
               {
                    {InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.INVENTORY_ID, "i.InventoryId"},
                    {InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.PRODUCT_ID, "p.ProductId"},
                    {InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.LOCATION_ID, "l.LocationId"},
                    {InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.SITE_ID, "ia.site_id"},
                    {InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.CATEGORY, "c.Name"},
                    {InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.WASTAGE_FROM_DATE, "ia.Timestamp"},
                    {InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.WASTAGE_TO_DATE, "ia.Timestamp"}, 
                    {InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.CODE, "p.Code"},
                    {InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.PRODUCT_DESCRIPTION, "p.Description"},
                    {InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.BARCODE, "pb.Barcode"}
               };
        /// <summary>
        /// Parameterized constructor of InventoryWastageSummaryDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public InventoryWastageSummaryDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to InventoryWastageSummaryDTO class type
        /// </summary>
        /// <param name="inventoryWastageSummaryDataRow">InventoryWastageSummaryDTO DataRow</param>
        /// <returns>Returns InventoryDTO</returns>
        private InventoryWastageSummaryDTO GetInventoryWastageSummaryDTO(DataRow inventoryWastageSummaryDataRow)
        {
            log.LogMethodEntry(inventoryWastageSummaryDataRow);
            InventoryWastageSummaryDTO inventoryDataObject = new InventoryWastageSummaryDTO(
                                             inventoryWastageSummaryDataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryWastageSummaryDataRow["ProductId"]),
                                             inventoryWastageSummaryDataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryWastageSummaryDataRow["Name"]),
                                             inventoryWastageSummaryDataRow["UOM"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryWastageSummaryDataRow["UOM"]),
                                             -1,
                                             //inventoryWastageSummaryDataRow["AdjustmentId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryWastageSummaryDataRow["AdjustmentId"]),
                                             inventoryWastageSummaryDataRow["InventoryId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryWastageSummaryDataRow["InventoryId"]),
                                             inventoryWastageSummaryDataRow["Code"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryWastageSummaryDataRow["Code"]),
                                             inventoryWastageSummaryDataRow["Description"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryWastageSummaryDataRow["Description"]),
                                             inventoryWastageSummaryDataRow["FromLocationId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryWastageSummaryDataRow["FromLocationId"]),
                                             inventoryWastageSummaryDataRow["LocationName"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryWastageSummaryDataRow["LocationName"]),
                                             0, // wastage quantity always set to zero while loading 
                                             //inventoryWastageSummaryDataRow["AdjustmentQuantity"] == DBNull.Value ? 0 : Convert.ToDecimal(inventoryWastageSummaryDataRow["AdjustmentQuantity"]),
                                             inventoryWastageSummaryDataRow["Quantity"] == DBNull.Value ? 0: Convert.ToDecimal(inventoryWastageSummaryDataRow["Quantity"]),
                                             inventoryWastageSummaryDataRow["AdjustmentQuantity"] == DBNull.Value ? 0 : Convert.ToDecimal(inventoryWastageSummaryDataRow["AdjustmentQuantity"]),
                                             inventoryWastageSummaryDataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryWastageSummaryDataRow["Remarks"]),
                                             inventoryWastageSummaryDataRow["LotId1"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryWastageSummaryDataRow["LotId1"]),
                                             inventoryWastageSummaryDataRow["LastupdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryWastageSummaryDataRow["LastupdatedBy"]),
                                             inventoryWastageSummaryDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryWastageSummaryDataRow["LastUpdateDate"]),
                                             inventoryWastageSummaryDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryWastageSummaryDataRow["site_id"]),
                                             inventoryWastageSummaryDataRow["CategoryId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryWastageSummaryDataRow["CategoryId"]),
                                             inventoryWastageSummaryDataRow["UOMId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryWastageSummaryDataRow["UOMId"]),
                                             inventoryWastageSummaryDataRow["Timestamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryWastageSummaryDataRow["Timestamp"]),
                                             inventoryWastageSummaryDataRow["AdjustmentQuantity"] == DBNull.Value ? 0 : Convert.ToDecimal(inventoryWastageSummaryDataRow["AdjustmentQuantity"]), // wastage for today
                                             inventoryWastageSummaryDataRow["LotNumber"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryWastageSummaryDataRow["LotNumber"]),
                                             inventoryWastageSummaryDataRow["AdjustmentTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryWastageSummaryDataRow["AdjustmentTypeId"])
                                             );
            log.LogMethodExit(inventoryDataObject);
            return inventoryDataObject;
        }

        /// <summary>
        /// Gets the InventoryWastageSummaryDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTrxn">SqlTransaction object </param>
        /// <returns>Returns the list of InventoryWastageSummaryDTO matching the search criteria</returns>
        public List<InventoryWastageSummaryDTO> GetInventoryWastageSummaryDTOList(List<KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<InventoryWastageSummaryDTO> inventoryWastageSummaryDTOList = new List<InventoryWastageSummaryDTO>();
            parameters.Clear();
            string selectInventoryDocumentTypeQuery = SELECT_QUERY + GetFilterQuery(searchParameters);           
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectInventoryDocumentTypeQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                inventoryWastageSummaryDTOList = new List<InventoryWastageSummaryDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    InventoryWastageSummaryDTO inventoryDocumentTypeDTO = GetInventoryWastageSummaryDTO(dataRow);
                    inventoryWastageSummaryDTOList.Add(inventoryDocumentTypeDTO);
                }
            }
            log.LogMethodExit(inventoryWastageSummaryDTOList);
            return inventoryWastageSummaryDTOList;
        }
        
        private string GetFilterQuery(List<KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            StringBuilder query = new StringBuilder("");
            List<InventoryWastageSummaryDTO> inventoryWastageSummaryDTOList = new List<InventoryWastageSummaryDTO>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                query = new StringBuilder();
                foreach (KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string> searchParameter in searchParameters)
                {
                    joiner = " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.PRODUCT_ID
                            || searchParameter.Key == InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.INVENTORY_ID
                            || searchParameter.Key == InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.LOCATION_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.CODE
                                || searchParameter.Key == InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.CATEGORY
                                || searchParameter.Key == InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.BARCODE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.WASTAGE_TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.WASTAGE_FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.SITE_ID)
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
            }
            log.LogMethodExit();
            return query.ToString();
        }

        /// <summary>
        /// Gets the InventoryWastageSummaryDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of InventoryWastageSummaryDTO matching the search criteria</returns>
        public List<InventoryWastageSummaryDTO> GetInventoryWastageSummaryList(List<KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>> searchParameters, int currentPage = 0, int pageSize = 0)//added
        {
            log.LogMethodEntry(searchParameters);
            List<InventoryWastageSummaryDTO> inventoryWastageSummaryDTOList = null;
            parameters.Clear();
            string selectQuery = SELECT_QUERY + GetFilterQuery(searchParameters);
            if (currentPage > 0 || pageSize > 0)
            {
                selectQuery += " Order by ia.TimeStamp desc OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                inventoryWastageSummaryDTOList = new List<InventoryWastageSummaryDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    InventoryWastageSummaryDTO inventoryWastageSummaryDTO = GetInventoryWastageSummaryDTO(dataRow);
                    inventoryWastageSummaryDTOList.Add(inventoryWastageSummaryDTO);
                }
            }
            log.LogMethodExit(inventoryWastageSummaryDTOList);
            return inventoryWastageSummaryDTOList;
        }


        /// <summary>
        /// Returns the no of Inventory Wastages matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetInventoryWastagesCount(List<KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int inventoryWastagesDTOCount = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery = selectQuery + GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                inventoryWastagesDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(inventoryWastagesDTOCount);
            return inventoryWastagesDTOCount;
        }
    }
}
