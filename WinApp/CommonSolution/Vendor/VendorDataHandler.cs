/********************************************************************************************
* Project Name - Vendor Data Handler
* Description  - Data handler of the vendor data handler class
* 
**************
**Version Log
**************
*Version     Date          Modified By    Remarks          
*********************************************************************************************
*1.00        06-Apr-2016   Raghuveera          Created 
********************************************************************************************
*1.00        11-Aug-2016   Soumya              Updated  to add insert and update functions 
********************************************************************************************
* 2.60       11-Apr-2019    Girish Kundar      Updated Insert and Update methods by adding  PurchaseTaxId,
*                                              WHO columns . 
*2.70        19-Jun-2019   Akshay Gulaganji    Modified isActive type (from string to bool) and added search Parameter
*2.70.2        25-Jul-2019    Deeksha            Modifications as per three tier standard.
*2.100.0     14-Oct-2020   Mushahid Faizan   Added methods for Pagination and modified search filters method .
*2.110.0        02-Nov-2020   Mushahid Faizan      Web inventory enhancement.
*******************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Microsoft.SqlServer.Server;

namespace Semnox.Parafait.Vendor
{
    /// <summary>
    /// Vendor Data Handler - Handles insert, update and select of vendor data objects
    /// </summary>   
    public class VendorDataHandler
    {
        /// <summary>
        /// Datahandler for Vendor
        /// </summary>
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM Vendor AS v ";
        private List<SqlParameter> parameters = new List<SqlParameter>();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  Dictionary for searching Parameters for the Vendor object.
        /// </summary>
        private static readonly Dictionary<VendorDTO.SearchByVendorParameters, string> DBSearchParameters = new Dictionary<VendorDTO.SearchByVendorParameters, string>
               {
                    {VendorDTO.SearchByVendorParameters.VENDOR_ID, "v.VendorId"},
                    {VendorDTO.SearchByVendorParameters.VENDOR_ID_LIST, "v.VendorId"},
                    {VendorDTO.SearchByVendorParameters.NAME, "v.Name"},
                    {VendorDTO.SearchByVendorParameters.ADDRESS, "v.Address1+Address2"},
                    {VendorDTO.SearchByVendorParameters.CITY, "v.City"},
                    {VendorDTO.SearchByVendorParameters.STATE,"v.State"},
                    {VendorDTO.SearchByVendorParameters.COUNTRY, "v.Country"},
                    {VendorDTO.SearchByVendorParameters.POSTAL_CODE, "v.PostalCode"},
                    {VendorDTO.SearchByVendorParameters.EMAIL, "v.Email"},
                    {VendorDTO.SearchByVendorParameters.PHONE, "v.Phone"},
                    {VendorDTO.SearchByVendorParameters.CONTACT_NAME, "v.ContactName"},
                    {VendorDTO.SearchByVendorParameters.IS_ACTIVE, "v.IsActive"},
                    {VendorDTO.SearchByVendorParameters.SITEID, "v.site_id"},
                    {VendorDTO.SearchByVendorParameters.VENDORCODE, "v.VendorCode"},
                    {VendorDTO.SearchByVendorParameters.VENDORMARKUPPERCENT, "v.VendorMarkupPercent"},
                    {VendorDTO.SearchByVendorParameters.MASTERENTITYID,"v.MasterEntityId" }
               };

        #region MERGE_QUERY
        private const string MERGE_QUERY = @"DECLARE @Output AS VendorType;
                                            MERGE INTO Vendor tbl
                                            USING @VendorList AS src
                                            ON src.VendorId = tbl.VendorId
                                            WHEN MATCHED THEN
                                            UPDATE SET
                                            Name = src.Name,
                                            Remarks = src.Remarks,
                                            DefaultPaymentTermsId = src.DefaultPaymentTermsId,
                                            Address1 = src.Address1,
                                            Address2 = src.Address2,
                                            City = src.City,
                                            State = src.State,
                                            Country = src.Country,
                                            PostalCode = src.PostalCode,
                                            AddressRemarks = src.AddressRemarks,
                                            ContactName = src.ContactName,
                                            Phone = src.Phone,
                                            Fax = src.Fax,
                                            Email = src.Email,
                                            LastModUserId = src.LastModUserId,
                                            LastModDttm = src.LastModDttm,
                                            IsActive = src.IsActive,
                                            Website = src.Website,
                                            TaxRegistrationNumber = src.TaxRegistrationNumber,
                                            --site_id = src.site_id,
                                            Guid = src.Guid,
                                            VendorCode = src.VendorCode,
                                            MasterEntityId = src.MasterEntityId,
                                            VendorMarkupPercent = src.VendorMarkupPercent,
                                            CountryId = src.CountryId,
                                            StateId = src.StateId,
                                            PurchaseTaxId = src.PurchaseTaxId,
                                            PaymentTerms = src.PaymentTerms,
                                            GoodsReturnPolicy = src.GoodsReturnPolicy
                                            WHEN NOT MATCHED THEN INSERT (
                                            Name,
                                            Remarks,
                                            DefaultPaymentTermsId,
                                            Address1,
                                            Address2,
                                            City,
                                            State,
                                            Country,
                                            PostalCode,
                                            AddressRemarks,
                                            ContactName,
                                            Phone,
                                            Fax,
                                            Email,
                                            LastModUserId,
                                            LastModDttm,
                                            IsActive,
                                            Website,
                                            TaxRegistrationNumber,
                                            site_id,
                                            Guid,
                                            VendorCode,
                                            MasterEntityId,
                                            VendorMarkupPercent,
                                            CountryId,
                                            StateId,
                                            CreatedBy,
                                            CreationDate,
                                            PurchaseTaxId,
                                            PaymentTerms,
                                            GoodsReturnPolicy
                                            )VALUES (
                                            src.Name,
                                            src.Remarks,
                                            src.DefaultPaymentTermsId,
                                            src.Address1,
                                            src.Address2,
                                            src.City,
                                            src.State,
                                            src.Country,
                                            src.PostalCode,
                                            src.AddressRemarks,
                                            src.ContactName,
                                            src.Phone,
                                            src.Fax,
                                            src.Email,
                                            src.LastModUserId,
                                            GETDATE(),
                                            src.IsActive,
                                            src.Website,
                                            src.TaxRegistrationNumber,
                                            src.site_id,
                                            src.Guid,
                                            src.VendorCode,
                                            src.MasterEntityId,
                                            src.VendorMarkupPercent,
                                            src.CountryId,
                                            src.StateId,
                                            src.CreatedBy,
                                            GETDATE(),
                                            src.PurchaseTaxId,
                                            src.PaymentTerms,
                                            src.GoodsReturnPolicy
                                            )
                                            OUTPUT
                                            inserted.VendorId,
                                            inserted.LastModUserId,  
                                            inserted.LastModDttm,
                                            inserted.CreatedBy,  
                                            inserted.CreationDate,
                                            inserted.site_id,
                                            inserted.Guid
                                            INTO @Output(VendorId, LastModUserId, LastModDttm, CreatedBy, CreationDate, site_id, Guid);
                                            SELECT * FROM @Output;";
        #endregion
        /// <summary>
        /// Default constructor of VendorDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public VendorDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to VendorDTO class type
        /// </summary>
        /// <param name="vendorDataRow">VendorDTO DataRow</param>
        /// <returns>Returns VendorDTO</returns>
        private VendorDTO GetVendorDTO(DataRow vendorDataRow)
        {
            log.LogMethodEntry(vendorDataRow);
            VendorDTO vendorDataObject = new VendorDTO(Convert.ToInt32(vendorDataRow["VendorId"]),
                                            vendorDataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(vendorDataRow["Name"]),
                                            vendorDataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(vendorDataRow["Remarks"]),
                                            vendorDataRow["DefaultPaymentTermsId"] == DBNull.Value ? -1 : Convert.ToInt32(vendorDataRow["DefaultPaymentTermsId"]),
                                            vendorDataRow["Address1"] == DBNull.Value ? string.Empty : Convert.ToString(vendorDataRow["Address1"]),
                                            vendorDataRow["Address2"] == DBNull.Value ? string.Empty : Convert.ToString(vendorDataRow["Address2"]),
                                            vendorDataRow["City"] == DBNull.Value ? string.Empty : Convert.ToString(vendorDataRow["City"]),
                                            vendorDataRow["State"] == DBNull.Value ? string.Empty : Convert.ToString(vendorDataRow["State"]),
                                            vendorDataRow["Country"] == DBNull.Value ? string.Empty : Convert.ToString(vendorDataRow["Country"]),
                                            vendorDataRow["PostalCode"] == DBNull.Value ? string.Empty : Convert.ToString(vendorDataRow["PostalCode"]),
                                            vendorDataRow["AddressRemarks"] == DBNull.Value ? string.Empty : Convert.ToString(vendorDataRow["AddressRemarks"]),
                                            vendorDataRow["ContactName"] == DBNull.Value ? string.Empty : Convert.ToString(vendorDataRow["ContactName"]),
                                            vendorDataRow["Phone"] == DBNull.Value ? string.Empty : Convert.ToString(vendorDataRow["Phone"]),
                                            vendorDataRow["Fax"] == DBNull.Value ? string.Empty : Convert.ToString(vendorDataRow["Fax"]),
                                            vendorDataRow["Email"] == DBNull.Value ? string.Empty : Convert.ToString(vendorDataRow["Email"]),
                                            vendorDataRow["LastModUserId"] == DBNull.Value ? string.Empty : Convert.ToString(vendorDataRow["LastModUserId"]),
                                            vendorDataRow["LastModDttm"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(vendorDataRow["LastModDttm"]),
                                            vendorDataRow["IsActive"] == DBNull.Value ? true : vendorDataRow["IsActive"].ToString() == "Y",
                                            vendorDataRow["Website"] == DBNull.Value ? string.Empty : Convert.ToString(vendorDataRow["Website"]),
                                            vendorDataRow["TaxRegistrationNumber"] == DBNull.Value ? string.Empty : Convert.ToString(vendorDataRow["TaxRegistrationNumber"]),
                                            vendorDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(vendorDataRow["site_id"]),
                                            vendorDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(vendorDataRow["Guid"]),
                                            vendorDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(vendorDataRow["SynchStatus"]),
                                            vendorDataRow["VendorCode"] == DBNull.Value ? string.Empty : Convert.ToString(vendorDataRow["VendorCode"]),
                                            vendorDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(vendorDataRow["MasterEntityId"]),
                                            //vendorDataRow["TaxId"] == DBNull.Value ? -1 : Convert.ToInt32(vendorDataRow["TaxId"]),
                                            vendorDataRow["VendorMarkupPercent"] == DBNull.Value ? double.NaN : Convert.ToDouble(vendorDataRow["VendorMarkupPercent"]),
                                            vendorDataRow["CountryId"] == DBNull.Value ? -1 : Convert.ToInt32(vendorDataRow["CountryId"]),
                                            vendorDataRow["StateId"] == DBNull.Value ? -1 : Convert.ToInt32(vendorDataRow["StateId"]),
                                            vendorDataRow["PurchaseTaxId"] == DBNull.Value ? -1 : Convert.ToInt32(vendorDataRow["PurchaseTaxId"]),
                                            vendorDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(vendorDataRow["CreatedBy"]),
                                            vendorDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(vendorDataRow["CreationDate"]),
                                            vendorDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(vendorDataRow["LastUpdatedBy"]),
                                            vendorDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(vendorDataRow["LastUpdateDate"]),
                                            vendorDataRow["PaymentTerms"] == DBNull.Value ? string.Empty : Convert.ToString(vendorDataRow["PaymentTerms"]),
                                            vendorDataRow["GoodsReturnPolicy"] == DBNull.Value ? string.Empty : Convert.ToString(vendorDataRow["GoodsReturnPolicy"])
                                            );
            log.LogMethodExit(vendorDataObject);
            return vendorDataObject;
        }

        /// <summary>
        /// Gets the vendor data of passed vendor id
        /// </summary>
        /// <param name="VendorId">integer type parameter</param>
        /// <returns>Returns VendorDTO</returns>
        public VendorDTO GetVendor(int VendorId)
        {
            log.LogMethodEntry(VendorId);
            VendorDTO result = null;
            string query = SELECT_QUERY + @" WHERE v.VendorId= @VendorId";
            SqlParameter parameter = new SqlParameter("@VendorId", VendorId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetVendorDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Returns the vendor table columns
        /// </summary>
        /// <returns>vendorTableColumns</returns>
        public DataTable GetVendorColumns()
        {
            log.LogMethodEntry();
            string selectVendorQuery = "Select columns from(Select '' as columns UNION ALL Select COLUMN_NAME as columns from INFORMATION_SCHEMA.COLUMNS  Where TABLE_NAME='Vendor') a order by columns";
            DataTable vendorTableColumns = dataAccessHandler.executeSelectQuery(selectVendorQuery, null, sqlTransaction);
            log.LogMethodExit(vendorTableColumns);
            return vendorTableColumns;
        }

        internal List<int> GetInactiveVendorToBePublished(int masterSiteId)
        {
            log.LogMethodEntry(masterSiteId);
            List<int> result = new List<int>();
            string query = @"SELECT VendorId
                            FROM Vendor v
                            WHERE site_id = @site_id 
                            AND IsActive = 'N'
                            AND EXISTS (SELECT 1 
                                        FROM 	Vendor vv
                                        WHERE vv.MasterEntityId = v.VendorId
                                        AND vv.IsActive = 'Y')";
            SqlParameter parameter = new SqlParameter("@site_id", masterSiteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    if (row["VendorId"] != DBNull.Value)
                    {
                        result.Add(Convert.ToInt32(row["VendorId"]));
                    }
                }
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Retriving vendor by passing query
        /// </summary>
        /// <param name="sqlQuery">Query passed for retriving the vendor</param>
        /// <returns> List of VendorDTO </returns>
        public List<VendorDTO> GetVendorList(string sqlQuery)
        {
            log.LogMethodEntry(sqlQuery);
            string Query = sqlQuery.ToUpper();
            if (Query.Contains("DROP") || Query.Contains("UPDATE") || Query.Contains("DELETE"))
            {
                log.LogMethodExit();
                return null;
            }
            DataTable vendorData = dataAccessHandler.executeSelectQuery(sqlQuery, null, sqlTransaction);
            if (vendorData.Rows.Count > 0)
            {
                List<VendorDTO> vendorList = new List<VendorDTO>();
                foreach (DataRow vendorDataRow in vendorData.Rows)
                {
                    VendorDTO vendorDataObject = GetVendorDTO(vendorDataRow);
                    vendorList.Add(vendorDataObject);
                }
                log.LogMethodExit(vendorList);
                return vendorList; ;
            }
            else
            {
                log.LogMethodEntry();
                return null;
            }
        }

        /// <summary>
        /// Returns the sql query based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of query</returns>
        public string GetFilterQuery(List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            StringBuilder query = new StringBuilder(" ");

            if (searchParameters != null && (searchParameters.Count > 0))
            {
                query.Append(" where ");
                string joiner;
                foreach (KeyValuePair<VendorDTO.SearchByVendorParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key.Equals(VendorDTO.SearchByVendorParameters.VENDOR_ID)
                           || searchParameter.Key.Equals(VendorDTO.SearchByVendorParameters.MASTERENTITYID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == VendorDTO.SearchByVendorParameters.VENDORCODE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == VendorDTO.SearchByVendorParameters.SITEID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(VendorDTO.SearchByVendorParameters.IS_ACTIVE))
                        {

                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else if (searchParameter.Key == VendorDTO.SearchByVendorParameters.VENDOR_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
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
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
            }
            log.LogMethodExit(query);
            return query.ToString();
        }


        /// <summary>
        /// Gets the VendorDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of VendorDTO matching the search criteria</returns>
        public List<VendorDTO> GetVendorList(List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<VendorDTO> vendorList = new List<VendorDTO>();
            parameters.Clear();
            string selectVendorQuery = SELECT_QUERY;
            selectVendorQuery += GetFilterQuery(searchParameters);
            DataTable vendorData = dataAccessHandler.executeSelectQuery(selectVendorQuery, parameters.ToArray(), sqlTransaction);
            if (vendorData.Rows.Count > 0)
            {
                vendorList = new List<VendorDTO>();
                foreach (DataRow vendorDataRow in vendorData.Rows)
                {
                    VendorDTO vendorDataObject = GetVendorDTO(vendorDataRow);
                    vendorList.Add(vendorDataObject);
                }
            }
            log.LogMethodExit(vendorList);
            return vendorList;
        }

        /// <summary>
        /// Gets the VendorDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of VendorDTO matching the search criteria</returns>
        public List<VendorDTO> GetVendorList(List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> searchParameters, int currentPage, int pageSize)
        {
            log.LogMethodEntry(searchParameters);
            List<VendorDTO> vendorList = new List<VendorDTO>();
            parameters.Clear();
            string selectVendorQuery = SELECT_QUERY;
            selectVendorQuery += GetFilterQuery(searchParameters);
            if (currentPage > 0 && pageSize > 0)
            {
                selectVendorQuery += " ORDER BY v.VendorId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectVendorQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable vendorData = dataAccessHandler.executeSelectQuery(selectVendorQuery, parameters.ToArray(), sqlTransaction);
            if (vendorData.Rows.Count > 0)
            {
                vendorList = new List<VendorDTO>();
                foreach (DataRow vendorDataRow in vendorData.Rows)
                {
                    VendorDTO vendorDataObject = GetVendorDTO(vendorDataRow);
                    vendorList.Add(vendorDataObject);
                }
            }
            log.LogMethodExit(vendorList);
            return vendorList;
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Vendor Record.
        /// </summary>
        /// <param name="vendorDTO">VendorDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(VendorDTO vendorDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(vendorDTO, loginId, siteId);
            double verifyDouble = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@VendorId", vendorDTO.VendorId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", string.IsNullOrEmpty(vendorDTO.Name) ? string.Empty : vendorDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Remarks", string.IsNullOrEmpty(vendorDTO.Remarks) ? string.Empty : vendorDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AddressRemarks", string.IsNullOrEmpty(vendorDTO.AddressRemarks) ? string.Empty : vendorDTO.AddressRemarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Address1", string.IsNullOrEmpty(vendorDTO.Address1) ? string.Empty : vendorDTO.Address1));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Address2", string.IsNullOrEmpty(vendorDTO.Address2) ? string.Empty : vendorDTO.Address2));
            parameters.Add(dataAccessHandler.GetSQLParameter("@City", string.IsNullOrEmpty(vendorDTO.City) ? string.Empty : vendorDTO.City));
            parameters.Add(dataAccessHandler.GetSQLParameter("@State", string.IsNullOrEmpty(vendorDTO.State) ? string.Empty : vendorDTO.State));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Country", string.IsNullOrEmpty(vendorDTO.Country) ? string.Empty : vendorDTO.Country));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PostalCode", string.IsNullOrEmpty(vendorDTO.PostalCode) ? string.Empty : vendorDTO.PostalCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ContactName", string.IsNullOrEmpty(vendorDTO.ContactName) ? string.Empty : vendorDTO.ContactName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Phone", string.IsNullOrEmpty(vendorDTO.Phone) ? string.Empty : vendorDTO.Phone));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Fax", string.IsNullOrEmpty(vendorDTO.Fax) ? string.Empty : vendorDTO.Fax));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Email", string.IsNullOrEmpty(vendorDTO.Email) ? string.Empty : vendorDTO.Email));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Website", string.IsNullOrEmpty(vendorDTO.Website) ? string.Empty : vendorDTO.Website));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TaxRegistrationNumber", string.IsNullOrEmpty(vendorDTO.TaxRegistrationNumber) ? string.Empty : vendorDTO.TaxRegistrationNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", vendorDTO.IsActive ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastModUserId", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DefaultPaymentTermsId", vendorDTO.DefaultPaymentTermsId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityID", vendorDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@countryId", vendorDTO.CountryId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@stateId", vendorDTO.StateId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@purchaseTaxId", vendorDTO.PurchaseTaxId, true));
            //parameters.Add(dataAccessHandler.GetSQLParameter("@TaxId", vendorDTO.TaxId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VendorCode", vendorDTO.VendorCode == null ? DBNull.Value : (object)vendorDTO.VendorCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VendorMarkupPercent", (Double.TryParse(vendorDTO.VendorMarkupPercent.ToString(), out verifyDouble) == false) || Double.IsNaN(vendorDTO.VendorMarkupPercent) || vendorDTO.VendorMarkupPercent.ToString() == "" ? DBNull.Value : (object)vendorDTO.VendorMarkupPercent));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdateBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@paymentTerms", string.IsNullOrEmpty(vendorDTO.PaymentTerms) ? string.Empty : vendorDTO.PaymentTerms));
            parameters.Add(dataAccessHandler.GetSQLParameter("@goodsReturnPolicy", string.IsNullOrEmpty(vendorDTO.GoodsReturnPolicy) ? string.Empty : vendorDTO.GoodsReturnPolicy));
            log.LogMethodExit(parameters);
            return parameters;
        }

        //Added 11-Aug-2016 
        /// <summary>
        /// InsertVendor
        /// </summary>
        /// <param name="vendor">vendor</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <param name="SQLTrx">SQLTrx</param>
        /// <returns>Vendor DTO</returns>
        public VendorDTO InsertVendor(VendorDTO vendor, string loginId, int siteId)
        {
            log.LogMethodEntry(vendor, loginId, siteId);
            setStateCountryNames(vendor);
            string insertVendorQuery = @"insert into vendor(Name,
                                                            Remarks,
                                                            DefaultPaymentTermsId,
                                                            Address1,
                                                            Address2,
                                                            City,
                                                            State,
                                                            Country,
                                                            PostalCode,
                                                            AddressRemarks,
                                                            ContactName,
                                                            Phone, 
                                                            Fax, 
                                                            Email,
                                                            LastModUserId,
                                                            LastModDttm,    
                                                            IsActive,
                                                            Website, 
                                                            TaxRegistrationNumber, 
                                                            site_id, 
                                                            Guid, 
                                                            VendorCode, 
                                                            masterentityid, 
                                                            VendorMarkupPercent,
                                                            CountryId,
                                                            StateId,
                                                            CreatedBy,
                                                            CreationDate,
                                                            LastUpdatedBy,
                                                            LastUpdateDate,
                                                            PurchaseTaxId,
                                                            PaymentTerms,
                                                            GoodsReturnPolicy) 
                                         Values             (@Name,
                                                            @Remarks,
                                                            @DefaultPaymentTermsId,
                                                            @Address1,
                                                            @Address2,
                                                            @City,
                                                            @State,
                                                            @Country,
                                                            @PostalCode,
                                                            @AddressRemarks,
                                                            @ContactName,
                                                            @Phone, 
                                                            @Fax, 
                                                            @Email,
                                                            @LastModUserId,
                                                            Getdate(),    
                                                            @IsActive,
                                                            @Website, 
                                                            @TaxRegistrationNumber, 
                                                            @site_id, 
                                                            NewID(),  
                                                            @VendorCode, 
                                                            @masterentityid, 
                                                            @VendorMarkupPercent,
                                                            @countryId,
                                                            @stateId,
                                                            @createdBy,
                                                            GetDate(),
                                                            @createdBy,
                                                            GetDate(),
                                                            @purchaseTaxId,
                                                            @paymentTerms,
                                                            @goodsReturnPolicy)
                                                SELECT * FROM Vendor WHERE VendorId = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertVendorQuery, GetSQLParameters(vendor, loginId, siteId).ToArray(), sqlTransaction);
                RefreshVendorDTO(vendor, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting vendor", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(vendor);
            return vendor;
        }

        /// <summary>
        /// Updatevendor
        /// </summary>
        /// <param name="vendor">vendor</param>
        /// <param name="siteId">siteId</param>
        /// <param name="loginId">loginId</param>
        /// <param name="SQLTrx">SQLTrx</param>
        /// <returns>Vendor DTO</returns>
        public VendorDTO UpdateVendor(VendorDTO vendor, string loginId, int siteId)
        {
            log.LogMethodEntry(vendor, loginId, siteId);
            setStateCountryNames(vendor);
            string updateVendorQuery = @"update vendor
                                           set              Name = @Name,
                                                            Remarks = @Remarks,
                                                            DefaultPaymentTermsId = @DefaultPaymentTermsId,
                                                            Address1 = @Address1,
                                                            Address2 = @Address2,
                                                            City = @City,
                                                            State = @State,
                                                            Country = @Country,
                                                            PostalCode = @PostalCode,
                                                            AddressRemarks = @AddressRemarks,
                                                            ContactName = @ContactName,
                                                            Phone = @Phone, 
                                                            Fax = @Fax, 
                                                            Email = @Email,
                                                            LastModUserId = @LastModUserId,
                                                            LastModDttm = Getdate(),    
                                                            IsActive = @IsActive,
                                                            Website = @Website, 
                                                            TaxRegistrationNumber = @TaxRegistrationNumber, 
                                                            -- site_id = @site_id, 
                                                            VendorCode = @VendorCode, 
                                                            masterentityid = @masterentityid, 
                                                            VendorMarkupPercent = @VendorMarkupPercent,
                                                            CountryId = @countryId,
                                                            StateId = @stateId,
                                                            LastUpdatedBy = @createdBy,
                                                            LastUpdateDate =GetDate(),
                                                            PurchaseTaxId =@purchaseTaxId,
                                                            PaymentTerms =@paymentTerms,
                                                            GoodsReturnPolicy =@goodsReturnPolicy

                                          where VendorId = @VendorId
                                          SELECT * FROM Vendor WHERE VendorId = @VendorId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateVendorQuery, GetSQLParameters(vendor, loginId, siteId).ToArray(), sqlTransaction);
                RefreshVendorDTO(vendor, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating vendor", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(vendor);
            return vendor;
        }

        /// <summary>
        /// Delete the record from the Vendor database based on VendorId
        /// </summary>
        /// <param name="vendorId">vendorId </param>
        /// <returns>return the int </returns>
        internal int Delete(int vendorId)
        {
            log.LogMethodEntry(vendorId);
            string query = @"DELETE  
                             FROM Vendor
                             WHERE Vendor.VendorId = @VendorId";
            SqlParameter parameter = new SqlParameter("@VendorId", vendorId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Returns the no of Vendor matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of Vendors matching the criteria</returns>
        public int GetVendorCount(List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                count = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(count);
            return count;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="VendorDTO">VendorDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshVendorDTO(VendorDTO vendorDTO, DataTable dt)
        {
            log.LogMethodEntry(vendorDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                vendorDTO.VendorId = Convert.ToInt32(dt.Rows[0]["VendorId"]);
                vendorDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                vendorDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                vendorDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                vendorDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                vendorDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                vendorDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        ///// <summary>
        ///// Search the VendorDTO list matching the search key
        ///// </summary>
        ///// <param name="searchParameters">List of search parameters</param>
        ///// <returns>Returns the list of VendorDTO matching the search criteria</returns>
        //public List<VendorDTO> SearchVendorList(List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> searchParameters)
        //{
        //    log.LogMethodEntry(searchParameters);
        //    List<VendorDTO> vendorList = null;
        //    List<SqlParameter> parameters = new List<SqlParameter>();
        //    string selectVendorQuery = SELECT_QUERY;
        //    if ((searchParameters != null) && (searchParameters.Count > 0))
        //    {
        //        string joiner;
        //        int count = 0;
        //        StringBuilder query = new StringBuilder(" WHERE ");
        //        foreach (KeyValuePair<VendorDTO.SearchByVendorParameters, string> searchParameter in searchParameters)
        //        {
        //            joiner = count == 0 ? string.Empty : " and ";
        //            if (DBSearchParameters.ContainsKey(searchParameter.Key))
        //            {
        //                if (searchParameter.Key.Equals(VendorDTO.SearchByVendorParameters.VENDOR_ID)
        //                    || searchParameter.Key.Equals(VendorDTO.SearchByVendorParameters.MASTERENTITYID))
        //                {
        //                    query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
        //                    parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
        //                }
        //                else if (searchParameter.Key == VendorDTO.SearchByVendorParameters.VENDORCODE)
        //                {
        //                    query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
        //                    parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
        //                }
        //                else if (searchParameter.Key == VendorDTO.SearchByVendorParameters.SITEID)
        //                {
        //                    query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
        //                    parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
        //                }
        //                else if (searchParameter.Key.Equals(VendorDTO.SearchByVendorParameters.IS_ACTIVE))
        //                {
        //                    query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
        //                    parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
        //                }
        //                else
        //                {
        //                    query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
        //                    parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
        //                }
        //            }

        //            else
        //            {
        //                string message = "The query parameter does not exist " + searchParameter.Key;
        //                log.LogVariableState("searchParameter.Key", searchParameter.Key);
        //                log.LogMethodExit(null, "Throwing exception -" + message);
        //                throw new Exception(message);
        //            }
        //            count++;
        //        }
        //        if (searchParameters.Count > 0)
        //            selectVendorQuery = selectVendorQuery + query;
        //    }

        //    DataTable vendorData = dataAccessHandler.executeSelectQuery(selectVendorQuery, null, sqlTransaction);
        //    if (vendorData.Rows.Count > 0)
        //    {
        //        vendorList = new List<VendorDTO>();
        //        foreach (DataRow vendorDataRow in vendorData.Rows)
        //        {
        //            VendorDTO vendorDataObject = GetVendorDTO(vendorDataRow);
        //            vendorList.Add(vendorDataObject);
        //        }
        //    }
        //    log.LogMethodExit(vendorList);
        //    return vendorList;
        //}

        void setStateCountryNames(VendorDTO vendorDTO)
        {
            log.LogMethodEntry(vendorDTO);
            StateDTOList stateDTOListBl = new StateDTOList(null);
            StateDTO stateDTO = new StateDTO();
            stateDTO.StateId = vendorDTO.StateId;
            stateDTO.CountryId = vendorDTO.CountryId;
            List<StateDTO> stateDtoList = stateDTOListBl.GetStateDTOList(stateDTO);
            if (stateDtoList != null && stateDtoList.Count > 0)
                vendorDTO.State = stateDtoList[0].State;
            else
                vendorDTO.State = "";

            CountryDTOList countryDTOListBl = new CountryDTOList(null);
            CountryDTO countryDTO = new CountryDTO();
            countryDTO.CountryId = vendorDTO.CountryId;
            List<CountryDTO> countryDTOList = countryDTOListBl.GetCountryDTOList(countryDTO);
            if (countryDTOList != null && countryDTOList.Count > 0)
                vendorDTO.Country = countryDTOList[0].CountryName;
            else
                vendorDTO.Country = "";

            log.LogMethodExit();
        }
        /// <summary>
        /// Inserts the Vendor record to the database
        /// </summary>
        /// <param name="vendorDTO">VendorDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(VendorDTO vendorDTO, string userId, int siteId)
        {
            log.LogMethodEntry(vendorDTO, userId, siteId);
            Save(new List<VendorDTO>() { vendorDTO }, userId, siteId);
            SaveInventoryActivityLog(vendorDTO, userId, siteId);
            log.LogMethodExit();
        }
        /// <summary>
        /// Inserts the Vendor record to the database
        /// </summary>
        /// <param name="vendorDTOList">List of VendorDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(List<VendorDTO> vendorDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(vendorDTOList, userId, siteId);
            Dictionary<string, VendorDTO> vendorDTOGuidMap = GetVendorDTOGuidMap(vendorDTOList);
            List<SqlDataRecord> sqlDataRecords = GetSqlDataRecords(vendorDTOList, userId, siteId);
            DataTable dataTable = dataAccessHandler.BatchSave(sqlDataRecords,
                                                              sqlTransaction,
                                                              MERGE_QUERY,
                                                              "VendorType",
                                                              "@VendorList");
            UpdateVendorDTOList(vendorDTOGuidMap, dataTable);
            log.LogMethodExit();
        }

        private List<SqlDataRecord> GetSqlDataRecords(List<VendorDTO> vendorDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(vendorDTOList, userId, siteId);
            List<SqlDataRecord> result = new List<SqlDataRecord>();
            SqlMetaData[] columnStructures = new SqlMetaData[33];
            int column = 0;
            columnStructures[column++] = new SqlMetaData("VendorId", SqlDbType.Int);
            columnStructures[column++] = new SqlMetaData("Name", SqlDbType.NVarChar, 200);
            columnStructures[column++] = new SqlMetaData("Remarks", SqlDbType.NVarChar, -1);
            columnStructures[column++] = new SqlMetaData("DefaultPaymentTermsId", SqlDbType.Int);
            columnStructures[column++] = new SqlMetaData("Address1", SqlDbType.NVarChar, 200);
            columnStructures[column++] = new SqlMetaData("Address2", SqlDbType.NVarChar, 200);
            columnStructures[column++] = new SqlMetaData("City", SqlDbType.NVarChar, 100);
            columnStructures[column++] = new SqlMetaData("State", SqlDbType.NVarChar, 50);
            columnStructures[column++] = new SqlMetaData("Country", SqlDbType.NVarChar, 200);
            columnStructures[column++] = new SqlMetaData("PostalCode", SqlDbType.NVarChar, 20);
            columnStructures[column++] = new SqlMetaData("AddressRemarks", SqlDbType.NVarChar, -1);
            columnStructures[column++] = new SqlMetaData("ContactName", SqlDbType.NVarChar, 200);
            columnStructures[column++] = new SqlMetaData("Phone", SqlDbType.NVarChar, 50);
            columnStructures[column++] = new SqlMetaData("Fax", SqlDbType.NVarChar, 50);
            columnStructures[column++] = new SqlMetaData("Email", SqlDbType.NVarChar, 100);
            columnStructures[column++] = new SqlMetaData("LastModUserId", SqlDbType.NVarChar, 50);
            columnStructures[column++] = new SqlMetaData("LastModDttm", SqlDbType.DateTime);
            columnStructures[column++] = new SqlMetaData("IsActive", SqlDbType.Char, 1);
            columnStructures[column++] = new SqlMetaData("Website", SqlDbType.NVarChar, -1);
            columnStructures[column++] = new SqlMetaData("TaxRegistrationNumber", SqlDbType.NVarChar, 50);
            columnStructures[column++] = new SqlMetaData("site_id", SqlDbType.Int);
            columnStructures[column++] = new SqlMetaData("Guid", SqlDbType.UniqueIdentifier);
            columnStructures[column++] = new SqlMetaData("SynchStatus", SqlDbType.Bit);
            columnStructures[column++] = new SqlMetaData("VendorCode", SqlDbType.NVarChar, 50);
            columnStructures[column++] = new SqlMetaData("MasterEntityId", SqlDbType.Int);
            columnStructures[column++] = new SqlMetaData("VendorMarkupPercent", SqlDbType.Float);
            columnStructures[column++] = new SqlMetaData("CountryId", SqlDbType.Int);
            columnStructures[column++] = new SqlMetaData("StateId", SqlDbType.Int);
            columnStructures[column++] = new SqlMetaData("CreatedBy", SqlDbType.NVarChar, 50);
            columnStructures[column++] = new SqlMetaData("CreationDate", SqlDbType.DateTime);
            columnStructures[column++] = new SqlMetaData("PurchaseTaxId", SqlDbType.Int);
            columnStructures[column++] = new SqlMetaData("PaymentTerms", SqlDbType.NVarChar, 200);
            columnStructures[column++] = new SqlMetaData("GoodsReturnPolicy", SqlDbType.NVarChar, 200);
            for (int i = 0; i < vendorDTOList.Count; i++)
            {
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures);
                column = 0;
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].VendorId, true));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].Name));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].Remarks));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].DefaultPaymentTermsId, true));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].Address1));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].Address2));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].City));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].State));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].Country));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].PostalCode));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].AddressRemarks));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].ContactName));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].Phone));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].Fax));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].Email));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].LastModDttm));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].IsActive ? "Y" : "N"));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].Website));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].TaxRegistrationNumber));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(siteId, true));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(Guid.Parse(vendorDTOList[i].Guid)));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].SynchStatus));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].VendorCode));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].MasterEntityId, true));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(Double.IsNaN(vendorDTOList[i].VendorMarkupPercent) ? (double?)null : vendorDTOList[i].VendorMarkupPercent));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].CountryId, true));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].StateId, true));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].CreationDate));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].PurchaseTaxId, true));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].PaymentTerms));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(vendorDTOList[i].GoodsReturnPolicy));
                result.Add(dataRecord);
            }
            log.LogMethodExit(result);
            return result;
        }

        private Dictionary<string, VendorDTO> GetVendorDTOGuidMap(List<VendorDTO> vendorDTOList)
        {
            log.LogMethodEntry(vendorDTOList);
            Dictionary<string, VendorDTO> result = new Dictionary<string, VendorDTO>();
            for (int i = 0; i < vendorDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(vendorDTOList[i].Guid))
                {
                    vendorDTOList[i].Guid = Guid.NewGuid().ToString();
                }
                result.Add(vendorDTOList[i].Guid, vendorDTOList[i]);
            }
            log.LogMethodExit(result);
            return result;
        }

        private void UpdateVendorDTOList(Dictionary<string, VendorDTO> vendorDTOGuidMap, DataTable table)
        {
            log.LogMethodEntry(vendorDTOGuidMap, table);
            foreach (DataRow row in table.Rows)
            {
                VendorDTO vendorDTO = vendorDTOGuidMap[Convert.ToString(row["Guid"])];
                vendorDTO.VendorId = row["VendorId"] == DBNull.Value ? -1 : Convert.ToInt32(row["VendorId"]);
                vendorDTO.LastModUserId = row["LastModUserId"] == DBNull.Value ? null : Convert.ToString(row["LastModUserId"]);
                vendorDTO.LastModDttm = row["LastModDttm"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["LastModDttm"]);
                vendorDTO.CreatedBy = row["CreatedBy"] == DBNull.Value ? null : Convert.ToString(row["CreatedBy"]);
                vendorDTO.CreationDate = row["CreationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["CreationDate"]);
                vendorDTO.SiteId = row["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["site_id"]);
                vendorDTO.AcceptChanges();
                log.LogMethodExit();
            }
        }

        /// <summary>
        ///  Inserts the record to the InventoryActivityLogDTO Table.
        /// </summary>
        /// <param name="vendorInventoryActivityLogDTO">inventoryActivityLogDTO object passed as the Parameter</param>
        /// <param name="loginId">login id of the user </param>
        /// <param name="siteId">site id of the user</param>
        public void SaveInventoryActivityLog(VendorDTO vendorInventoryActivityLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(vendorInventoryActivityLogDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[InventoryActivityLog]
                           (TimeStamp,
                            Message,
                            Guid,
                            site_id,
                            SourceTableName,
                            InvTableKey,
                            SourceSystemId,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate)
                     VALUES
                           (@TimeStamp,
                            @Message,
                            @Guid,
                            @site_id,
                            @SourceTableName,
                            @InvTableKey,
                            @SourceSystemId,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE()
                            )SELECT CAST(scope_identity() AS int)";

            try
            {
                List<SqlParameter> vendorInventoryActivityLogParameters = new List<SqlParameter>();
                vendorInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@InvTableKey", DBNull.Value));
                vendorInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@Message", "Vendor Inserted"));
                vendorInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@SourceSystemId", vendorInventoryActivityLogDTO.VendorId.ToString() + ":" + vendorInventoryActivityLogDTO.Name));
                vendorInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@SourceTableName", "Vendor"));
                vendorInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@TimeStamp", ServerDateTime.Now));
                vendorInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
                vendorInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
                vendorInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", vendorInventoryActivityLogDTO.MasterEntityId, true));
                vendorInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
                vendorInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@Guid", vendorInventoryActivityLogDTO.Guid));
                log.Debug(vendorInventoryActivityLogParameters);

                object rowInserted = dataAccessHandler.executeScalar(query, vendorInventoryActivityLogParameters.ToArray(), sqlTransaction);
                log.LogMethodExit(rowInserted);

            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting InventoryActivityLog ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }
    }
}
