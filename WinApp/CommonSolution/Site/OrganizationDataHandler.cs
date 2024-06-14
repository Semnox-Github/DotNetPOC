/********************************************************************************************
 * Project Name - Organization Data Handler
 * Description  - Data handler of the organization data handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        01-Mar-2016   Raghuveera          Created
 *2.60        29-Mar-2019   Mushahid Faizan     Added COMPANY_ID, DBSearchParameters.
 *2.70.2        23-Jul-2019   Deeksha             Modifications as per three tier standard.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Site
{
    /// <summary>
    /// Organization Data Handler - Handles insert, update and select of organization data objects
    /// </summary>
    public class OrganizationDataHandler
    {
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM Organization AS o ";
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;

        /// <summary>
        ///  Dictionary for searching Parameters for the Organization object.
        /// </summary>
        private static readonly Dictionary<OrganizationDTO.SearchByOrganizationParameters, string> DBSearchParameters = new Dictionary<OrganizationDTO.SearchByOrganizationParameters, string>
               {
                    {OrganizationDTO.SearchByOrganizationParameters.ORG_ID, "o.OrgId"},
                    {OrganizationDTO.SearchByOrganizationParameters.ORG_NAME, "o.OrgName"},
                    {OrganizationDTO.SearchByOrganizationParameters.PARENT_ORG_ID, "o.ParentOrgId"},
                    {OrganizationDTO.SearchByOrganizationParameters.COMPANY_ID, "o.Company_Id"}
               };

        /// <summary>
        /// Default constructor of OrganizationDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public OrganizationDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Default constructor of OrganizationDataHandler class
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        public OrganizationDataHandler(string connectionString)
        {
            log.LogMethodEntry(connectionString);
            if (string.IsNullOrEmpty(connectionString))
                dataAccessHandler = new DataAccessHandler();
            else
            {
                dataAccessHandler = new DataAccessHandler(connectionString);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to OrganizationDTO class type
        /// </summary>
        /// <param name="organizationDataRow">OrganizationDTO DataRow</param>
        /// <returns>Returns OrganizationDTO</returns>
        private OrganizationDTO GetOrganizationDTO(DataRow organizationDataRow)
        {
            log.LogMethodEntry(organizationDataRow);
            log.Debug("GetOrganizationDTO(organizationDataRow) Method. OrgId=" + organizationDataRow["OrgId"] + ", Org Name=" + organizationDataRow["OrgName"].ToString() + ", structureId=" + organizationDataRow["StructureId"]
                + ", ParentOrgId=" + organizationDataRow["ParentOrgId"] + ", company Id=" + organizationDataRow["Company_Id"] + ", Guid=" + organizationDataRow["Guid"]);
            OrganizationDTO organizationDataObject = new OrganizationDTO(Convert.ToInt32(organizationDataRow["OrgId"]),
                                            organizationDataRow["OrgName"] == DBNull.Value ? string.Empty : Convert.ToString(organizationDataRow["OrgName"]),
                                            organizationDataRow["StructureId"] == DBNull.Value ? -1 : Convert.ToInt32(organizationDataRow["StructureId"]),
                                            organizationDataRow["ParentOrgId"] == DBNull.Value ? -1 : Convert.ToInt32(organizationDataRow["ParentOrgId"]),
                                            organizationDataRow["Company_Id"] == DBNull.Value ? -1 : Convert.ToInt32(organizationDataRow["Company_Id"]),
                                            organizationDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(organizationDataRow["Guid"]),
                                            organizationDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(organizationDataRow["CreatedBy"]),
                                            organizationDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(organizationDataRow["CreationDate"]),
                                            organizationDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(organizationDataRow["LastUpdatedBy"]),
                                            organizationDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(organizationDataRow["LastUpdateDate"])
                                            );

            log.LogMethodExit(organizationDataObject);
            return organizationDataObject;
        }

        /// <summary>
        /// Gets the organization data of passed organization id
        /// </summary>
        /// <param name="organizationId">integer type parameter</param>
        /// <returns>Returns OrganizationDTO</returns>
        public OrganizationDTO GetOrganization(int organizationId)
        {
            log.LogMethodEntry(organizationId);
            string selectOrganizationQuery = SELECT_QUERY + "WHERE o.OrgId = @organizationId";
            OrganizationDTO result = null;
            SqlParameter[] selectOrganizationParameters = new SqlParameter[1];
            selectOrganizationParameters[0] = new SqlParameter("@organizationId", organizationId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectOrganizationQuery, selectOrganizationParameters, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetOrganizationDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the root organization data 
        /// </summary>        
        /// <returns>Returns OrganizationDTO</returns>
        public List<OrganizationDTO> GetRootOrganizationList()
        {
            log.LogMethodEntry();
            string selectOrganizationQuery = @"select *
                                         from Organization
                                        where ParentOrgId is null ";
            DataTable organizationData = dataAccessHandler.executeSelectQuery(selectOrganizationQuery, null, sqlTransaction);
            if (organizationData.Rows.Count > 0)
            {
                List<OrganizationDTO> organizationList = new List<OrganizationDTO>();
                foreach (DataRow organizationDataRow in organizationData.Rows)
                {
                    OrganizationDTO organizationDataObject = GetOrganizationDTO(organizationDataRow);
                    organizationList.Add(organizationDataObject);
                }
                log.LogMethodExit(organizationList);
                return organizationList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
        /// <summary>
        /// Gets the OrganizationDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of OrganizationDTO matching the search criteria</returns>
        public List<OrganizationDTO> GetOrganizationList(List<KeyValuePair<OrganizationDTO.SearchByOrganizationParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<OrganizationDTO> organizationList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectOrganizationQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<OrganizationDTO.SearchByOrganizationParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key.Equals(OrganizationDTO.SearchByOrganizationParameters.ORG_ID)
                            || searchParameter.Key.Equals(OrganizationDTO.SearchByOrganizationParameters.PARENT_ORG_ID)
                            || searchParameter.Key.Equals(OrganizationDTO.SearchByOrganizationParameters.COMPANY_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
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
                if (searchParameters.Count > 0)
                    selectOrganizationQuery = selectOrganizationQuery + query;
                selectOrganizationQuery = selectOrganizationQuery + " order by OrgName ";
            }

            DataTable organizationData = dataAccessHandler.executeSelectQuery(selectOrganizationQuery, parameters.ToArray(), sqlTransaction);
            if (organizationData.Rows.Count > 0)
            {
                organizationList = new List<OrganizationDTO>();
                foreach (DataRow organizationDataRow in organizationData.Rows)
                {
                    OrganizationDTO organizationDataObject = GetOrganizationDTO(organizationDataRow);
                    organizationList.Add(organizationDataObject);
                }
            }
            log.LogMethodExit(organizationList);
            return organizationList;
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating OrganizationDataHandler Record.
        /// </summary>
        /// <param name="organizationDTO">OrganizationDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(OrganizationDTO organizationDTO, string loginId)
        {
            log.LogMethodEntry(organizationDTO, loginId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@OrgId", organizationDTO.OrgId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@StructureId", organizationDTO.StructureId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParentOrgId", organizationDTO.ParentOrgId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CompanyId", organizationDTO.CompanyId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OrgName", string.IsNullOrEmpty(organizationDTO.OrgName) ? DBNull.Value : (object)organizationDTO.OrgName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Insert organization data
        /// </summary>
        /// <param name="organizationDTO">organizationDTO</param>
        /// <param name="loginId">loginId</param>
        /// <returns>Returns int orgId</returns>
        public OrganizationDTO InsertOrganization(OrganizationDTO organizationDTO, string loginId)
        {
            log.LogMethodEntry(organizationDTO, loginId);
            string InsertOrganizationQuery = @"INSERT INTO dbo.Organization
                                                            (OrgName
                                                            ,StructureId
                                                            ,ParentOrgId
                                                            ,Company_Id
                                                            ,Guid
                                                            ,CreatedBy
                                                            ,CreationDate
                                                            ,LastUpdatedBy
                                                            ,LastUpdateDate
                                                            )
                                                            VALUES
                                                            (
                                                             @OrgName
                                                            ,@StructureId
                                                            ,@ParentOrgId
                                                            ,@CompanyId
                                                            , NewId()
                                                            ,@CreatedBy
                                                            ,GETDATE()
                                                            ,@LastUpdatedBy
                                                            ,GETDATE()
                                                            )SELECT * FROM Organization WHERE OrgId = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(InsertOrganizationQuery, GetSQLParameters(organizationDTO, loginId).ToArray(), sqlTransaction);
                RefreshOrganizationDTO(organizationDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting organizationDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(organizationDTO);
            return organizationDTO;
        }

        /// <summary>
        /// Update organization data 
        /// </summary>
        /// <param name="organizationDTO">organizationDTO</param>
        /// <param name="loginId">loginId</param>
        /// <returns>Returns int orgId</returns>
        public OrganizationDTO UpdateOrganization(OrganizationDTO organizationDTO, string loginId)
        {
            log.LogMethodEntry(organizationDTO, loginId);
            string UpdateOrganizationQuery = @"UPDATE Organization
                                            SET OrgName = @OrgName
                                            ,StructureId = @StructureId
                                            ,ParentOrgId = @ParentOrgId
                                            ,Company_Id = @CompanyId
                                            WHERE OrgId=@OrgId
                            SELECT * FROM Organization WHERE OrgId = @OrgId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(UpdateOrganizationQuery, GetSQLParameters(organizationDTO, loginId).ToArray(), sqlTransaction);
                RefreshOrganizationDTO(organizationDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating organizationDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(organizationDTO);
            return organizationDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="organizationDTO">OrganizationDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshOrganizationDTO(OrganizationDTO organizationDTO, DataTable dt)
        {
            log.LogMethodEntry(organizationDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                organizationDTO.OrgId = Convert.ToInt32(dt.Rows[0]["OrgId"]);
                organizationDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                organizationDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                organizationDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                organizationDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                organizationDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the root DeleteOrganisation data
        ///<param name="orgId">orgId</param>
        /// </summary>        
        /// <returns>Returns int </returns>
        public int DeleteOrganization(int orgId)
        {
            log.LogMethodEntry(orgId);
            string deleteOrganisationQuery = @"delete from organization where orgid = @orgid ";
            SqlParameter[] deleteOrganisationParameters = new SqlParameter[1];
            deleteOrganisationParameters[0] = new SqlParameter("@orgid", orgId);
            int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteOrganisationQuery, deleteOrganisationParameters, sqlTransaction);
            log.LogMethodExit(deleteStatus);
            return deleteStatus;

        }
    }
}
