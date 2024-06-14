/********************************************************************************************
 * Project Name - Site
 * Description  - Get and Insert or update methods for OrganizationStructure details.
 **************
 **Version Log
 ************** 
 *Version     Date          Modified By           Remarks          
 *********************************************************************************************
 *2.60        11-Mar-2019   Jagan Mohan           Created
 *2.60        04-Apr-2019   Mushahid Faizan       Added LogMethodEntry & LogMethodExit, Added COMPANY_ID DBSearchParameters.
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
    public class OrganizationStructureDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;

        private static readonly Dictionary<OrganizationStructureDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<OrganizationStructureDTO.SearchByParameters, string>
               {
                    { OrganizationStructureDTO.SearchByParameters.STRUCTURE_ID, "StructureId"},
                    {OrganizationStructureDTO.SearchByParameters.STRUCTURE_NAME, "StructureName"},
                    {OrganizationStructureDTO.SearchByParameters.PARENT_STRUCTURE_ID, "ParentStructureId"},
                    {OrganizationStructureDTO.SearchByParameters.COMPANY_ID, "Company_Id"}
               };
        
        /// <summary>
        /// Default constructor of OrganizationStructureDataHandler class
        /// </summary>
        public OrganizationStructureDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to OrganizationDTO class type
        /// </summary>
        /// <param name="organizationDataRow">OrganizationDTO DataRow</param>
        /// <returns>Returns OrganizationDTO</returns>
        private OrganizationStructureDTO GetOrganizationStructureDTO(DataRow organizationDataRow)
        {
            log.LogMethodEntry(organizationDataRow);
            OrganizationStructureDTO organizationDataObject = new OrganizationStructureDTO(Convert.ToInt32(organizationDataRow["StructureId"]),
                                            organizationDataRow["StructureName"].ToString(),
                                            organizationDataRow["ParentStructureId"] == DBNull.Value ? -1 : Convert.ToInt32(organizationDataRow["ParentStructureId"]),
                                            organizationDataRow["Company_Id"] == DBNull.Value ? -1 : Convert.ToInt32(organizationDataRow["Company_Id"]),
                                            organizationDataRow["AutoRoam"] == DBNull.Value ? "" : Convert.ToString(organizationDataRow["AutoRoam"]),
                                            organizationDataRow["CreatedBy"] == DBNull.Value ? "" : Convert.ToString(organizationDataRow["CreatedBy"]),
                                            organizationDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(organizationDataRow["CreationDate"]),
                                            organizationDataRow["LastUpdatedBy"] == DBNull.Value ? "" : Convert.ToString(organizationDataRow["LastUpdatedBy"]),
                                            organizationDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(organizationDataRow["LastUpdateDate"])
                                            );

            log.LogMethodExit(organizationDataObject);
            return organizationDataObject;
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating OrganizationStructure Record.
        /// </summary>
        /// <param name="companyDTO">companyDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(OrganizationStructureDTO organizationDTO, string userId)
        {
            log.LogMethodEntry(organizationDTO, userId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@structureId", organizationDTO.StructureId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@structureName", organizationDTO.StructureName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@parentStructureId", organizationDTO.ParentStructureId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@companyId", organizationDTO.CompanyId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@autoRoam", organizationDTO.AutoRoam));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Gets the OrganizationStructureDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of OrganizationStructureDTO matching the search criteria</returns>
        public List<OrganizationStructureDTO> GetOrganizationStructureList(List<KeyValuePair<OrganizationStructureDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectOrganizationQuery = @"select *
                                         from OrgStructure";
            if (searchParameters != null)
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<OrganizationStructureDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";

                        if (searchParameter.Key.Equals(OrganizationStructureDTO.SearchByParameters.STRUCTURE_ID) || searchParameter.Key.Equals(OrganizationStructureDTO.SearchByParameters.PARENT_STRUCTURE_ID)
                            || searchParameter.Key.Equals(OrganizationStructureDTO.SearchByParameters.COMPANY_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                        }
                        else
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "'");
                        }
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetOrganizationStructureList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectOrganizationQuery = selectOrganizationQuery + query;
                selectOrganizationQuery = selectOrganizationQuery + " order by StructureName ";
            }

            DataTable organizationDataTable = dataAccessHandler.executeSelectQuery(selectOrganizationQuery, null, sqlTransaction);
            if (organizationDataTable.Rows.Count > 0)
            {
                List<OrganizationStructureDTO> organizationList = new List<OrganizationStructureDTO>();
                foreach (DataRow organizationDataRow in organizationDataTable.Rows)
                {
                    OrganizationStructureDTO organizationDataObject = GetOrganizationStructureDTO(organizationDataRow);
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
        /// Insert organization structure data 
        /// </summary>        
        /// <returns>Returns int structureId</returns>
        public int InsertOrganizationStructure(OrganizationStructureDTO organizationDTO, string userId)
        {
            log.LogMethodEntry(organizationDTO, userId);
            int idOfRowInserted;
            string insertOrganizationStructureQuery = @"INSERT INTO dbo.OrgStructure
                                                            (StructureName,
                                                             ParentStructureId,
                                                             Company_Id,
                                                             AutoRoam,
                                                             CreatedBy,
                                                             CreationDate,
                                                             LastUpdatedBy,
                                                             LastUpdateDate
                                                            )
                                                            VALUES
                                                            (
                                                             @structureName,
                                                             @parentStructureId,
                                                             @companyId,
                                                             @autoRoam,
                                                             @createdBy,
                                                             GETDATE(),
                                                             @lastUpdatedBy,
                                                             GETDATE()
                                                            )SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(insertOrganizationStructureQuery, GetSQLParameters(organizationDTO, userId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.LogVariableState("OrganizationStructureDTO", organizationDTO);
                log.Error("Error occured while inserting the account record", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }

        /// <summary>
        /// Update organization structure data 
        /// </summary>        
        /// <returns>Returns int structureId</returns>
        public int UpdateOrganizationStructure(OrganizationStructureDTO organizationDTO, string userId)
        {
            log.LogMethodEntry(organizationDTO, userId);
            int rowsUpdated;
            string updateOrganizationStructureQuery = @"UPDATE OrgStructure
                                            SET StructureName = @structureName,
                                            ParentStructureId = @parentStructureId,
                                            Company_Id = @companyId,
                                            AutoRoam = @autoRoam,
                                            LastUpdatedBy = @lastUpdatedBy,
                                            LastUpdateDate = GETDATE()
                                            WHERE StructureId=@structureId";
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(updateOrganizationStructureQuery, GetSQLParameters(organizationDTO, userId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.LogVariableState("OrganizationStructureDTO", organizationDTO);
                log.Error("Error occured while inserting the account record", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }
    }
}
