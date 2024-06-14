/********************************************************************************************
 * Project Name - Site
 * Description  - Get and Insert or update methods for Company details.
 **************
 **Version Log
 ************** 
 *Version     Date          Modified By           Remarks          
 *********************************************************************************************
 *2.60        08-Mar-2019   Jagan Mohana          Created 
              29-Mar-2019   Mushahid Faizan       Modified- MASTER_SITE_ID search parameter in GetCompany() method,
                                                            Modified DB column name i.e from CompanyName to Company_Name.
                                                            Modified Insert and Update Method.
 ********************************************************************************************/

using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.Site
{
    public class CompanyDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<CompanyDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CompanyDTO.SearchByParameters, string>
        {
                {CompanyDTO.SearchByParameters.COMPANY_ID,"Company_Id"},
                {CompanyDTO.SearchByParameters.COMPANY_NAME, "Company_Name"},
                {CompanyDTO.SearchByParameters.MASTER_SITE_ID, "Master_Site_Id"}
        };
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating company Record.
        /// </summary>
        /// <param name="companyDTO">companyDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CompanyDTO companyDTO, string userId)
        {
            log.LogMethodEntry(companyDTO, userId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@companyId", companyDTO.CompanyId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@companyName", companyDTO.CompanyName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@loginKey", companyDTO.LoginKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterSiteId", companyDTO.MasterSiteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
           //parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Default constructor of CompanyDataHandler class
        /// </summary>
        public CompanyDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Converts the Data row object to CompanyDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private CompanyDTO GetCompanyDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CompanyDTO companyDTO = new CompanyDTO(Convert.ToInt32(dataRow["Company_Id"]),
                                            dataRow["Company_Name"] == DBNull.Value ? "" : dataRow["Company_Name"].ToString(),
                                            dataRow["Login_Key"] == DBNull.Value ? "" : dataRow["Login_Key"].ToString(),
                                            dataRow["Master_Site_Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Master_Site_Id"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                            );
            log.LogMethodExit(companyDTO);
            return companyDTO;
        }

        /// <summary>
        /// Gets the CompanyDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of companyDTO matching the search criteria</returns>
        public List<CompanyDTO> GetCompany(List<KeyValuePair<CompanyDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectQuery = @"select * from Company";
            if (searchParameters != null && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<CompanyDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        {
                            if (searchParameter.Key.Equals(CompanyDTO.SearchByParameters.COMPANY_ID) ||
                                searchParameter.Key.Equals(CompanyDTO.SearchByParameters.MASTER_SITE_ID))
                            {
                                query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "'");
                            }
                        }
                        count++;
                    }
                    else
                    {
                        log.Error("Ends-GetCompany(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception();
                    }
                }
                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
            }

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                List<CompanyDTO> companyDTOList = new List<CompanyDTO>();
                foreach (DataRow companyDataRow in dataTable.Rows)
                {
                    CompanyDTO companyDataObject = GetCompanyDTO(companyDataRow);
                    companyDTOList.Add(companyDataObject);
                }
                log.LogMethodExit(companyDTOList);
                return companyDTOList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Insert company data 
        /// </summary>        
        /// <returns>Returns int companyId</returns>
        public int InsertCompany(CompanyDTO companyDTO, string userId)
        {
            log.LogMethodEntry(companyDTO, userId);
            int idOfRowInserted;
            string insertCompanyQuery = @"INSERT INTO dbo.company
                                                            (Company_Name,
                                                             Login_Key,
                                                             Master_Site_Id,
                                                             CreatedBy,
                                                             CreationDate,
                                                             LastUpdatedBy,
                                                             LastUpdateDate
                                                            )
                                                            VALUES
                                                            (
                                                             @companyName,
                                                             @loginKey,
                                                             @masterSiteId,
                                                             @createdBy,
                                                             GETDATE(),
                                                             @lastUpdatedBy,
                                                             GETDATE()
                                                            )SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(insertCompanyQuery, GetSQLParameters(companyDTO, userId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.LogVariableState("CompanyDTO", companyDTO);
                log.Error("Error occured while inserting the account record", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }

        /// <summary>
        /// Update company data 
        /// </summary>        
        /// <returns>Returns int companyId</returns>
        public int UpdateCompany(CompanyDTO companyDTO, string userId)
        {
            log.LogMethodEntry(companyDTO, userId);
            int rowsUpdated;
            string updateCompanyQuery = @"UPDATE Company
                                            SET Company_Name = @companyName,
                                            Login_Key = @loginKey,
                                            Master_Site_Id = @masterSiteId,
                                            LastUpdatedBy = @lastUpdatedBy,
                                            LastUpdateDate = GETDATE()
                                            WHERE Company_Id=@companyId";
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(updateCompanyQuery, GetSQLParameters(companyDTO, userId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.LogVariableState("CompanyDTO", companyDTO);
                log.Error("Error occured while inserting the account record", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }
    }
}