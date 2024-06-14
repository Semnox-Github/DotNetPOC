
/********************************************************************************************
 * Project Name - Product Display GroupFormat Data Handler
 * Description  - Data handler of the ProductDisplayGroupFormat class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00       18-May-2016    Amaresh          Created 
 *2.3.0     25-Jun-2018     Guru S A         Modifications handle products exclusion at user role
 *2.60       6-Feb-2019     Nagesh Badiger   Added isActive 
 *2.60       29-Mar-2019    Akshay Gulaganji modified isActive search Parameter in GetProductDisplayGroupFormatList(), GetOnlyUsedProductDisplayGroupFormatList()
 *                                           and added GetProductGroupInclusionList()
 *2.140.0    25-Nov-2021   Fiona Lishal      Modified : Added Searcch Parameter DATE_OF_LOG
 *2.140.0    25-Nov-2021   Fiona Lishal      Modified : Added Searcch Parameter MASTER_ENTITY_ID
 *2.130.11   13-Oct-2022   Vignesh Bhat      Modified to support BackgroundImageFileName property
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using System.Globalization;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Product Display Group Format DataHandler - Handles insert, update and select of  ProductDisplayGroupFormat objects
    /// </summary>

    public class ProductDisplayGroupFormatDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string> DBSearchParameters = new Dictionary<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>
        {
              {ProductDisplayGroupFormatDTO.SearchByDisplayParameters.DISPLAY_GROUP, "DisplayGroup"} ,
              {ProductDisplayGroupFormatDTO.SearchByDisplayParameters.DISPLAY_GROUP_FORMAT_ID_LIST, "Id"} ,
              {ProductDisplayGroupFormatDTO.SearchByDisplayParameters.DISPLAY_GROUP_ID, "Id"},
              {ProductDisplayGroupFormatDTO.SearchByDisplayParameters.POS_MACHINE_ID, "POSMachineId"},
              {ProductDisplayGroupFormatDTO.SearchByDisplayParameters.SITE_ID, "site_id"},
              {ProductDisplayGroupFormatDTO.SearchByDisplayParameters.IS_ACTIVE, "IsActive"},
              {ProductDisplayGroupFormatDTO.SearchByDisplayParameters.LAST_UPDATED_DATE, "LastUpdatedDate"},
              {ProductDisplayGroupFormatDTO.SearchByDisplayParameters.EXTERNAL_REFERENCE, "ExternalSourceReference"},
              {ProductDisplayGroupFormatDTO.SearchByDisplayParameters.MASTER_ENTITY_ID, "MasterEntityId"} 
        };

        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        /// <summary>
        /// Default constructor of ProductDisplayGroupFormatDataHandler class
        /// </summary>
        public ProductDisplayGroupFormatDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the Concurrent Programs record to the database
        /// </summary>
        /// <param name="productDisplayGroupFormat">productDisplayGroupFormatDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertProductDisplayGroupFormat(ProductDisplayGroupFormatDTO productDisplayGroupFormat, string loginId, int siteId)
        {
            log.LogMethodEntry(productDisplayGroupFormat, loginId, siteId);
            string insertProductDisplayGroupFormatQuery = @"insert into ProductDisplayGroupFormat 
                                                        (                                                 
                                                         DisplayGroup,
                                                         ButtonColor,
                                                         TextColor,
                                                         Font,
                                                         SortOrder,
                                                         ImageFileName,
                                                         Guid,
                                                         SynchStatus,
                                                         site_id,
                                                         LastUpdatedBy,
                                                         LastUpdatedDate,
                                                         Description,
                                                         IsActive,
                                                         ExternalSourceReference,
                                                         MasterEntityId,
                                                         BackgroundImageFileName
                                                        ) 
                                                values 
                                                        (
                                                          @displayGroup,
                                                          @buttonColor,
                                                          @textColor,
                                                          @font,
                                                          @sortOrder,
                                                          @imageFileName,
                                                          Newid(),
                                                          @synchStatus,
                                                          @siteId,
                                                          @userId,
                                                          Getdate(),
                                                          @description,
                                                          @isActive,
                                                          @ExternalSourceReference,
                                                          @MasterEntityId,
                                                          @backgroundImageFileName
                                                         )SELECT CAST(scope_identity() AS int)";

            List<SqlParameter> updateproductDisplayGroupFormatParameters = new List<SqlParameter>();

            if (string.IsNullOrEmpty(productDisplayGroupFormat.DisplayGroup))
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@displayGroup", string.Empty));
            }
            else
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@displayGroup", productDisplayGroupFormat.DisplayGroup));
            }

            if (string.IsNullOrEmpty(productDisplayGroupFormat.ButtonColor))
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@buttonColor", DBNull.Value));
            }
            else
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@buttonColor", productDisplayGroupFormat.ButtonColor));
            }

            if (string.IsNullOrEmpty(productDisplayGroupFormat.Font))
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@font", DBNull.Value));
            }
            else
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@font", productDisplayGroupFormat.Font));
            }

            if (string.IsNullOrEmpty(productDisplayGroupFormat.TextColor))
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@textColor", DBNull.Value));
            }
            else
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@textColor", productDisplayGroupFormat.TextColor));
            }

            if (productDisplayGroupFormat.SortOrder <= 0)
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@sortOrder", GetMaxSortOrder() + 1));
            }
            else
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@sortOrder", productDisplayGroupFormat.SortOrder));
            }

            if (string.IsNullOrEmpty(productDisplayGroupFormat.ImageFileName))
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@imageFileName", DBNull.Value));
            } 
            else
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@imageFileName", productDisplayGroupFormat.ImageFileName));
            }
            if (string.IsNullOrEmpty(productDisplayGroupFormat.ExternalSourceReference))
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@ExternalSourceReference", DBNull.Value));
            } 
            else
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@ExternalSourceReference", productDisplayGroupFormat.ExternalSourceReference));
            }

            if (productDisplayGroupFormat.SynchStatus)
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@synchStatus", productDisplayGroupFormat.SynchStatus));
            }
            else
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            }

            updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@userId", loginId));

            if (siteId == -1)
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            }
            else
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@siteId", siteId));
            }

            if (string.IsNullOrEmpty(productDisplayGroupFormat.Description))
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@description", DBNull.Value));
            }
            else
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@description", productDisplayGroupFormat.Description));
            }

            if (string.IsNullOrWhiteSpace(productDisplayGroupFormat.BackgroundImageFileName))
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@backgroundImageFileName", DBNull.Value));
            }
            else
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@backgroundImageFileName", productDisplayGroupFormat.BackgroundImageFileName));
            }

            updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@isActive", productDisplayGroupFormat.IsActive));
            updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@MasterEntityId", productDisplayGroupFormat.MasterEntityId));

            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertProductDisplayGroupFormatQuery, updateproductDisplayGroupFormatParameters.ToArray(), sqlTransaction);
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }

        private int GetMaxSortOrder()
        {
            log.LogMethodEntry();
            string GetMaxSortOrderQuery = @"Select isnull(max(SortOrder),0) from productDisplayGroupFormat";

            DataTable dt = dataAccessHandler.executeSelectQuery(GetMaxSortOrderQuery, null);

            if (dt != null && dt.Rows.Count == 1)
            {
                log.LogMethodExit(Convert.ToInt32(dt.Rows[0][0].ToString()));
                return Convert.ToInt32(dt.Rows[0][0].ToString());
            }
            log.LogMethodExit();
            return 0;
        }

        /// <summary>
        /// Updates theProductDisplayFormat record
        /// </summary>
        /// <param name="DisplayGroupFormat">ProductDisplayGroupFormatDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateProductDisplayFormat(ProductDisplayGroupFormatDTO productDisplayGroupFormat, string loginId, int siteId)
        {
            log.LogMethodEntry(productDisplayGroupFormat, loginId, siteId);
            string updateProductDisplayGroupFormatQuery = @"update ProductDisplayGroupFormat
                                                          set  DisplayGroup =@displayGroup,
                                                               ButtonColor =@buttonColor,
                                                               TextColor =@textColor,
                                                               Font = @font,
                                                               SortOrder =@sortOrder,
                                                               ImageFileName =@ImageFileName,
                                                               --SynchStatus=@synchStatus, 
                                                               --site_id =@siteId, 
                                                               LastUpdatedBy = @userId,
                                                               LastUpdatedDate = Getdate(),
                                                               Description = @description,
                                                                IsActive=@isActive,
                                                                ExternalSourceReference=@ExternalSourceReference,
                                                                BackgroundImageFileName =@backgroundImageFileName
                                                               where Id=@Id ";

            List<SqlParameter> updateproductDisplayGroupFormatParameters = new List<SqlParameter>();
            updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@Id", productDisplayGroupFormat.Id));
            if (string.IsNullOrEmpty(productDisplayGroupFormat.DisplayGroup))
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@displayGroup", string.Empty));
            }
            else
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@displayGroup", productDisplayGroupFormat.DisplayGroup));
            }

            if (string.IsNullOrEmpty(productDisplayGroupFormat.TextColor))
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@textColor", DBNull.Value));
            }
            else
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@textColor", productDisplayGroupFormat.TextColor));
            }

            if (string.IsNullOrEmpty(productDisplayGroupFormat.ButtonColor))
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@buttonColor", DBNull.Value));
            }
            else
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@buttonColor", productDisplayGroupFormat.ButtonColor));
            }

            if (string.IsNullOrEmpty(productDisplayGroupFormat.Font))
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@font", DBNull.Value));
            }
            else
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@font", productDisplayGroupFormat.Font));
            }

            if (productDisplayGroupFormat.SortOrder <= 0)
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@sortOrder", GetMaxSortOrder() + 1));
            }
            else
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@sortOrder", productDisplayGroupFormat.SortOrder));
            }
            if (string.IsNullOrEmpty(productDisplayGroupFormat.ExternalSourceReference))
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@ExternalSourceReference", DBNull.Value));
            }
            else
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@ExternalSourceReference", productDisplayGroupFormat.ExternalSourceReference));
            }
            if (string.IsNullOrEmpty(productDisplayGroupFormat.ImageFileName))
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@ImageFileName", DBNull.Value));
            }
            else
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@ImageFileName", productDisplayGroupFormat.ImageFileName));
            }

            if (siteId == -1)
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            }
            else
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@siteId", siteId));
            }

            if (productDisplayGroupFormat.SynchStatus)
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@synchStatus", productDisplayGroupFormat.SynchStatus));
            }
            else
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            }

            updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@userId", loginId));

            if (string.IsNullOrEmpty(productDisplayGroupFormat.Description))
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@description", DBNull.Value));
            }
            else
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@description", productDisplayGroupFormat.Description));
            }

            if (string.IsNullOrWhiteSpace(productDisplayGroupFormat.BackgroundImageFileName))
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@backgroundImageFileName", DBNull.Value));
            }
            else
            {
                updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@backgroundImageFileName", productDisplayGroupFormat.BackgroundImageFileName));
            }

            updateproductDisplayGroupFormatParameters.Add(new SqlParameter("@isActive", productDisplayGroupFormat.IsActive));

            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateProductDisplayGroupFormatQuery, updateproductDisplayGroupFormatParameters.ToArray(), sqlTransaction);
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }


        /// <summary>
        /// Delete the record from the database based on displayGroupId
        /// </summary>
        /// <returns>return the int </returns>
        public int Delete(int displayGroupId)
        {
            log.LogMethodEntry(displayGroupId);
            try
            {
                string displayGroupQuery = @"delete  
                                              from ProductDisplayGroupFormat
                                              where Id = @displayGroupId";

                SqlParameter[] displayGroupParameters = new SqlParameter[1];
                displayGroupParameters[0] = new SqlParameter("@displayGroupId", displayGroupId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(displayGroupQuery, displayGroupParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Converts the Data row object to ProductDisplayGroupFormatDTO class type
        /// </summary>
        /// <param name="productDisplayGroupDataRow">ProductDisplayGroup DataRow</param>
        /// <returns>Returns ProductDisplayGroupFormat</returns>
        private ProductDisplayGroupFormatDTO GetProductDisplayGroupFormatDTO(DataRow productDisplayGroupDataRow)
        {
            log.LogMethodEntry(productDisplayGroupDataRow);
            string buttonColor = "";
            if (productDisplayGroupDataRow["ButtonColor"].ToString().CompareTo("") != 0)
            {
                   buttonColor = ColorCodes.GetColorCode(productDisplayGroupDataRow["ButtonColor"].ToString());
            }

            string textColor = "";
            if (productDisplayGroupDataRow["TextColor"].ToString().CompareTo("") != 0)
            {
                textColor = ColorCodes.GetColorCode(productDisplayGroupDataRow["TextColor"].ToString());
            }
            ProductDisplayGroupFormatDTO ProductDisplayFormatDataObject = new ProductDisplayGroupFormatDTO(Convert.ToInt32(productDisplayGroupDataRow["Id"]),
                                                    productDisplayGroupDataRow["DisplayGroup"].ToString(),
                                                    //productDisplayGroupDataRow["ButtonColor"].ToString(),
                                                    //productDisplayGroupDataRow["TextColor"].ToString(),
                                                    buttonColor,
                                                    textColor,
                                                    productDisplayGroupDataRow["Font"].ToString(),
                                                    productDisplayGroupDataRow["SortOrder"] == DBNull.Value ? 0 : Convert.ToInt32(productDisplayGroupDataRow["SortOrder"]),
                                                    productDisplayGroupDataRow["Guid"].ToString(),
                                                    productDisplayGroupDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(productDisplayGroupDataRow["SynchStatus"]),
                                                    productDisplayGroupDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(productDisplayGroupDataRow["site_id"]),
                                                    productDisplayGroupDataRow["ImageFileName"].ToString(),
                                                    productDisplayGroupDataRow["LastUpdatedBy"].ToString(),
                                                    productDisplayGroupDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(productDisplayGroupDataRow["LastUpdatedDate"]),
                                                    productDisplayGroupDataRow["Description"].ToString(),
                                                    productDisplayGroupDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(productDisplayGroupDataRow["IsActive"]),
                                                    productDisplayGroupDataRow["ExternalSourceReference"].ToString(),
                                                    productDisplayGroupDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(productDisplayGroupDataRow["MasterEntityId"]),
                                                    productDisplayGroupDataRow["BackgroundImageFileName"].ToString()
                                                    );
            log.LogMethodExit(ProductDisplayFormatDataObject);
            return ProductDisplayFormatDataObject;
        }

        /// <summary>
        /// Gets the ProductDisplayGroupFormat data of passed displaygroup
        /// </summary>
        /// <param name="displaygroup">integer type parameter</param>
        /// <returns>Returns ProductDisplayGroupFormatDTO</returns>
        public ProductDisplayGroupFormatDTO GetProductDisplayGroupFormat(string displaygroup)
        {
            log.LogMethodEntry(displaygroup);


            string selectProductDisplayGroupFormatQuery = @"select *
                                         from ProductDisplayGroupFormat
                                        where DisplayGroup = @displaygroup";

            SqlParameter[] selectProductDisplayGroupFormatParameters = new SqlParameter[1];
            selectProductDisplayGroupFormatParameters[0] = new SqlParameter("@displaygroup", displaygroup);
            DataTable ProductDisplayGroupFormat = dataAccessHandler.executeSelectQuery(selectProductDisplayGroupFormatQuery, selectProductDisplayGroupFormatParameters, sqlTransaction);

            if (ProductDisplayGroupFormat.Rows.Count > 0)
            {
                DataRow ProductDisplayGroupFormatRow = ProductDisplayGroupFormat.Rows[0];
                ProductDisplayGroupFormatDTO ProductDisplayFormatDataObject = GetProductDisplayGroupFormatDTO(ProductDisplayGroupFormatRow);
                log.LogMethodExit(ProductDisplayFormatDataObject);
                return ProductDisplayFormatDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the ProductDisplayGroupFormatDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ProductDisplayGroupFormatDTO matching the search criteria</returns>
        public List<ProductDisplayGroupFormatDTO> GetProductDisplayGroupFormatList(List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectProductDisplayGroupFormatQuery = @"select *
                                                             from ProductDisplayGroupFormat ";
            string sortByField = "DisplayGroup";
            if (searchParameters != null)
            {
                if (searchParameters.Where(x => x.Key.ToString() == "SORT_ORDER").Count() > 0)
                {
                    sortByField = searchParameters.Where(x => x.Key.ToString() == "SORT_ORDER").FirstOrDefault().Value;
                    var itemtoremove = searchParameters.Where(x => x.Key.ToString() == "SORT_ORDER").First();
                    searchParameters.Remove(itemtoremove);
                }

                StringBuilder query = new StringBuilder(" where ");

                foreach (KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperator = (count == 0 ? "" : " and ");

                        if (searchParameter.Key.Equals(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.DISPLAY_GROUP_ID))
                        {
                            query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key.Equals(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.DISPLAY_GROUP_FORMAT_ID_LIST))
                        {
                            query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + " in ( " + searchParameter.Value + " )");
                        }

                        else if (searchParameter.Key.Equals(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.DISPLAY_GROUP)||
                                 searchParameter.Key.Equals(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.EXTERNAL_REFERENCE))
                        {
                            query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "'");
                        }
                        else if (searchParameter.Key == ProductDisplayGroupFormatDTO.SearchByDisplayParameters.SITE_ID 
                            || searchParameter.Key == ProductDisplayGroupFormatDTO.SearchByDisplayParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joinOperator + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == ProductDisplayGroupFormatDTO.SearchByDisplayParameters.IS_ACTIVE)
                        {
                            query.Append(joinOperator + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + "'" + searchParameter.Value + "'");
                            // query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "'" + (searchParameter.Value == "true" ? (" or " + DBSearchParameters[searchParameter.Key] + " IS Null") : ""));
                        }
                        else if (searchParameter.Key == ProductDisplayGroupFormatDTO.SearchByDisplayParameters.LAST_UPDATED_DATE)
                        {
                            query.Append(joinOperator + " ( ISNULL(LastUpdatedDate, GetDate())) >= "+ "'"+ DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "'");
                            //parameters.Add(new SqlParameter("@LAST_UPDATED_DATE", DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        } 
                        else
                        {
                            query.Append(joinOperator + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetProductDisplayGroupFormatList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectProductDisplayGroupFormatQuery = selectProductDisplayGroupFormatQuery + query;

                selectProductDisplayGroupFormatQuery = selectProductDisplayGroupFormatQuery + " Order by " + sortByField;
            }
            DataTable ProductDisplayData = dataAccessHandler.executeSelectQuery(selectProductDisplayGroupFormatQuery, null, sqlTransaction);
            List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatList = new List<ProductDisplayGroupFormatDTO>();
            if (ProductDisplayData.Rows.Count > 0)
            {
                foreach (DataRow productDisplayGroupFormatDataRow in ProductDisplayData.Rows)
                {
                    ProductDisplayGroupFormatDTO productDisplayGroupFormatDataObject = GetProductDisplayGroupFormatDTO(productDisplayGroupFormatDataRow);
                    productDisplayGroupFormatList.Add(productDisplayGroupFormatDataObject);
                }
                log.LogMethodExit(productDisplayGroupFormatList);

            }
            return productDisplayGroupFormatList;
        }

        /// <summary>
        /// Gets the ProductDisplayGroupFormatDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ProductDisplayGroupFormatDTO matching the search criteria</returns>
        public List<ProductDisplayGroupFormatDTO> GetOnlyUsedProductDisplayGroupFormatList(List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectProductDisplayGroupFormatQuery = @"select * from ProductDisplayGroupFormat";

            if (searchParameters != null)
            {
                string sortByField = "DisplayGroup";
                if (searchParameters.Where(x => x.Key.ToString() == "SORT_ORDER").Count() > 0)
                {
                    sortByField = searchParameters.Where(x => x.Key.ToString() == "SORT_ORDER").FirstOrDefault().Value;
                    var itemtoremove = searchParameters.Where(x => x.Key.ToString() == "SORT_ORDER").First();
                    searchParameters.Remove(itemtoremove);
                }

                StringBuilder query = new StringBuilder(" where ");

                foreach (KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperator = (count == 0 ? "" : " and ");

                        if (searchParameter.Key.Equals(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.DISPLAY_GROUP_ID))
                        {
                            query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key.Equals(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.DISPLAY_GROUP_FORMAT_ID_LIST))
                        {
                            query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + " in ( " + searchParameter.Value + " )");
                        }
                        else if (searchParameter.Key.Equals(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.DISPLAY_GROUP))
                        {
                            query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "'");
                        }
                        else if (searchParameter.Key.Equals(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.POS_MACHINE_ID))
                        {
                            query.Append(joinOperator + " Id NOT IN( SELECT ProductDisplayGroupFormatId FROM POSProductExclusions WHERE " + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + ")");
                        } 
                        else if (searchParameter.Key == ProductDisplayGroupFormatDTO.SearchByDisplayParameters.SITE_ID)
                        {
                            query.Append(joinOperator + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == ProductDisplayGroupFormatDTO.SearchByDisplayParameters.IS_ACTIVE)
                        {
                            query.Append(joinOperator + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + "'" + searchParameter.Value + "'");
                            // query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "'" + (searchParameter.Value == "true" ? (" or " + DBSearchParameters[searchParameter.Key] + " IS Null") : ""));
                        }
                        else
                        {
                            query.Append(joinOperator + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetProductDisplayGroupFormatList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectProductDisplayGroupFormatQuery = selectProductDisplayGroupFormatQuery + query + " AND";
                else
                    selectProductDisplayGroupFormatQuery = selectProductDisplayGroupFormatQuery + " WHERE";

                selectProductDisplayGroupFormatQuery = selectProductDisplayGroupFormatQuery + @" Id in (select distinct(DisplayGroupId) from ProductsDisplayGroup 
                                                             pd inner join products p on p.product_id = pd.ProductId and p.active_flag = 'Y') Order by DisplayGroup";
            }

            DataTable ProductDisplayData = dataAccessHandler.executeSelectQuery(selectProductDisplayGroupFormatQuery, null, sqlTransaction);
            List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatList = new List<ProductDisplayGroupFormatDTO>();
            if (ProductDisplayData.Rows.Count > 0)
            {
                foreach (DataRow productDisplayGroupFormatDataRow in ProductDisplayData.Rows)
                {
                    ProductDisplayGroupFormatDTO productDisplayGroupFormatDataObject = GetProductDisplayGroupFormatDTO(productDisplayGroupFormatDataRow);
                    productDisplayGroupFormatList.Add(productDisplayGroupFormatDataObject);
                }
                log.LogMethodExit(productDisplayGroupFormatList);
            }
            return productDisplayGroupFormatList;
        }

        /// <summary>
        /// Gets the ProductDisplayGroupFormatDTO list configured to passed tablet mac address
        /// </summary>
        /// <param name="macAddress">Tablet MacAddress</param>
        /// <returns>Returns the list of ProductDisplayGroupFormatDTO matching the tablet mac address</returns>
        public List<ProductDisplayGroupFormatDTO> GetConfiguredDisplayGroupList(string macAddress, string loginId)
        {
            log.LogMethodEntry(macAddress, loginId);

            string selectProductDisplayGroupFormatQuery = @"select * from ProductDisplayGroupFormat where Id IN 
                                               (select distinct(PGD.DisplayGroupId) 
											      from ProductsDisplayGroup PGD 
                                                       ,products PRD1   
                                                       ,product_type PDT
                                                       ,POSMachines PM
                                                    where  (PRD1.POSTypeId = PM.POSTypeId or PRD1.POSTypeId is null)
													  and PRD1.product_id = PGD.ProductId
													  and PRD1.product_type_id = PDT.product_type_id  
                                                      and (PM.IPAddress =  @macAddress OR PM.Computer_Name =  @macAddress) 
                                                      and PRD1.active_flag = 'Y' and PRD1.DisplayInPOS='Y'
                                                           --PDT.cardSale !='Y' 
                                                     " + (String.IsNullOrEmpty(loginId) == true ? "" : " and NOT exists (select 1 from UserRoleDisplayGroupExclusions urdge, users usr where urdge.ProductDisplayGroupId = PGD.DisplayGroupId and urdge.role_id = usr.role_id and usr.loginId = @loginId) ") +
                                                      @"and PGD.DisplayGroupId not in 
                                                                             (select PDGF.Id 
                                                                                from POSProductExclusions PPE 
                                                                                      left outer join ProductDisplayGroupFormat PDGF on PPE.ProductDisplayGroupFormatId=PDGF.Id 
                                                                               where POSMachineId IN (select POSMachineId from POSMachines 
                                                                                                       where IPAddress =  @macAddress or Computer_Name =  @macAddress)
                                                                              )
                                                      ) order by SortOrder";

            SqlParameter[] selectProductDisplayGroupFormatParameters = new SqlParameter[2];
            selectProductDisplayGroupFormatParameters[0] = new SqlParameter("@macAddress", macAddress);
            selectProductDisplayGroupFormatParameters[1] = new SqlParameter("@loginId", loginId);
            DataTable ProductDisplayData = dataAccessHandler.executeSelectQuery(selectProductDisplayGroupFormatQuery, selectProductDisplayGroupFormatParameters, sqlTransaction);
            List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatList = new List<ProductDisplayGroupFormatDTO>();
            if (ProductDisplayData.Rows.Count > 0)
            {
                foreach (DataRow productDisplayGroupFormatDataRow in ProductDisplayData.Rows)
                {
                    ProductDisplayGroupFormatDTO productDisplayGroupFormatDataObject = GetProductDisplayGroupFormatDTO(productDisplayGroupFormatDataRow);
                    productDisplayGroupFormatList.Add(productDisplayGroupFormatDataObject);
                }

            }
            log.LogMethodExit(productDisplayGroupFormatList);
            return productDisplayGroupFormatList;
        }
        /// <summary>
        /// Gets the ProductDisplayGroupFormatDTO list configured to login id
        /// </summary>
        /// <param name="loginId">loginId</param>
        /// <returns>Returns the list of ProductDisplayGroupFormatDTO matching the tablet mac address</returns>
        public List<ProductDisplayGroupFormatDTO> GetConfiguredDisplayGroupListForLogin(string loginId)
        {
            log.LogMethodEntry(loginId);

            string selectProductDisplayGroupFormatQuery = @"select * from ProductDisplayGroupFormat where Id IN 
                                               (select distinct(PGD.DisplayGroupId) 
											      from ProductsDisplayGroup PGD 
                                                       ,products PRD1
                                                    where PRD1.product_id = PGD.ProductId
                                                      and PRD1.active_flag = 'Y' 
                                                      and NOT exists (select 1 
                                                                      from UserRoleDisplayGroupExclusions urdge, 
                                                                           users usr 
                                                                      where urdge.ProductDisplayGroupId = PGD.DisplayGroupId 
                                                                      and urdge.role_id = usr.role_id 
                                                                      and usr.loginId = @loginId)
                                                      ) order by SortOrder";

            SqlParameter[] selectProductDisplayGroupFormatParameters = new SqlParameter[1];
            selectProductDisplayGroupFormatParameters[0] = new SqlParameter("@loginId", loginId);
            DataTable ProductDisplayData = dataAccessHandler.executeSelectQuery(selectProductDisplayGroupFormatQuery, selectProductDisplayGroupFormatParameters);
            List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatList = new List<ProductDisplayGroupFormatDTO>();
            if (ProductDisplayData.Rows.Count > 0)
            {
                foreach (DataRow productDisplayGroupFormatDataRow in ProductDisplayData.Rows)
                {
                    ProductDisplayGroupFormatDTO productDisplayGroupFormatDataObject = GetProductDisplayGroupFormatDTO(productDisplayGroupFormatDataRow);
                    productDisplayGroupFormatList.Add(productDisplayGroupFormatDataObject);
                }

            }
            log.Debug("Ends-GetPosConfiguredDisplayGroupList(string macAddress) Method by returning ProductDisplayGroupFormatList.");
            return productDisplayGroupFormatList;
        }

        /// <summary>
        /// Gets the ProductGroupInclusionList list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ProductDisplayGroupFormatDTO matching the search criteria</returns>
        public List<ProductDisplayGroupFormatDTO> GetProductGroupInclusionList(List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = @" SELECT DISTINCT pdf.* 
                                    FROM ProductDisplayGroupFormat pdf
                                    INNER JOIN ProductsDisplayGroup pd ON PD.DisplayGroupId = pdf.Id
                                    INNER JOIN products p ON p.product_id = pd.ProductId
                                    LEFT OUTER JOIN POSMachines pos ON p.POSTypeId = pos.POSTypeId ";

            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == ProductDisplayGroupFormatDTO.SearchByDisplayParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + " ISNULL(p.active_flag, 'Y') = @IS_PRODUCT_ACTIVE AND ISNULL(pdf.IsActive, 1) = @IS_ACTIVE ");
                            parameters.Add(new SqlParameter("@IS_PRODUCT_ACTIVE", searchParameter.Value == "1" || searchParameter.Value == "Y"? "Y" : "N"));
                            parameters.Add(new SqlParameter("@IS_ACTIVE", searchParameter.Value == "1" || searchParameter.Value == "Y"? true: false));
                        }
                        else if (searchParameter.Key == ProductDisplayGroupFormatDTO.SearchByDisplayParameters.POS_MACHINE_ID)
                        {
                            query.Append(joiner + @"  NOT EXISTS(SELECT 1 
				                                                FROM POSProductExclusions e 
			                                                    WHERE e.POSMachineId = @POS_MACHINE_ID
					                                            AND e.ProductDisplayGroupFormatId = pdf.Id)
                                                      AND (p.POSTypeId IS NULL OR pos.POSMachineId = @POS_MACHINE_ID) ");
                            parameters.Add(new SqlParameter("@POS_MACHINE_ID", Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProductDisplayGroupFormatDTO.SearchByDisplayParameters.SITE_ID)
                        {
                            query.Append(joiner + " (p.site_id = @SITE_ID OR @SITE_ID = -1) AND (pdf.site_id = @SITE_ID OR @SITE_ID = -1) " );
                            parameters.Add(new SqlParameter("@SITE_ID", Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProductDisplayGroupFormatDTO.SearchByDisplayParameters.LAST_UPDATED_DATE)
                        {
                            query.Append(joiner + " ( ISNULL(pdf.LastUpdatedDate, GetDate())) >= @LAST_UPDATED_DATE  ");
                            parameters.Add(new SqlParameter("@LAST_UPDATED_DATE", DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectQuery = selectQuery + query;
            }

            DataTable ProductDisplayData = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(),sqlTransaction);
            List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatList = new List<ProductDisplayGroupFormatDTO>();
            if (ProductDisplayData.Rows.Count > 0)
            {
                foreach (DataRow productDisplayGroupFormatDataRow in ProductDisplayData.Rows)
                {
                    ProductDisplayGroupFormatDTO productDisplayGroupFormatDataObject = GetProductDisplayGroupFormatDTO(productDisplayGroupFormatDataRow);
                    productDisplayGroupFormatList.Add(productDisplayGroupFormatDataObject);
                }
            }
            log.LogMethodExit(productDisplayGroupFormatList);
            return productDisplayGroupFormatList;
        }

        internal DateTime? GetProductDisplayGroupFormatModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"select max(LastUpdatedDate) LastUpdatedDate from ProductDisplayGroupFormat WHERE (site_id = @siteId or @siteId = -1)";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }

    }
}
