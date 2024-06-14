/********************************************************************************************
* Project Name - POS
* Description  - DataHandler - POSType
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.00        04-Mar-2019    Indhu                 Modified for Remote Shift Open/Close changes
*2.60        17-Feb-2019    Nagesh Badiger        Added IsActive.
*2.70        09-Jul-2019    Deeksha               Added createdBy,lastUpdatedBy,lastUpdateUser and creationDate fields  
*2.70.2      17-Oct-2019    Dakshakh raj          Modified : Issue fix for IN-claus in Sql Injection
*2.70.2      10-Dec-2019    Jinto Thomas          Removed siteid from update query
*2.90        26-Jun-2020    Girish Kundar         Modified as per the Standard CheckList 
*2.130.0     21-May-2021   Girish Kundar          Modified: Added Attribue1  to 5 columns to the table as part of Xero integration
*2.140       14-Sep-2021      Fiona               Modified: Issue fix in RefreshPOSTypeDTO 
 *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.POS
{
    /// <summary>
    ///  POSType Data Handler - Handles insert, update and select of  POSType objects
    /// </summary>
    public class POSTypeDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM POSTypes AS pt ";
        /// <summary>
        /// Dictionary for searching Parameters for the AchievementClass object.
        /// </summary>
        private static readonly Dictionary<POSTypeDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<POSTypeDTO.SearchByParameters, string>
            {
                {POSTypeDTO.SearchByParameters.POS_TYPE_ID, "pt.POSTypeId"},
                {POSTypeDTO.SearchByParameters.POS_TYPE_NAME, "pt.POSTypeName"},
                {POSTypeDTO.SearchByParameters.SITE_ID, "pt.site_id"},
                {POSTypeDTO.SearchByParameters.POS_TYPE_NAME_LIST, "pt.POSTypeName"},
                {POSTypeDTO.SearchByParameters.MASTER_ENTITY_ID, "pt.MasterEntityId"},
                {POSTypeDTO.SearchByParameters.IS_ACTIVE, "pt.IsActive"}
            };

        /// <summary>
        /// Default constructor of POSTypeDataHandler class
        /// </summary>
        public POSTypeDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating POSType Record.
        /// </summary>
        /// <param name="pOSTypeDTO">POSTypeDTO type object</param> 
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(POSTypeDTO pOSTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(pOSTypeDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSTypeId", pOSTypeDTO.POSTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSTypeName", pOSTypeDTO.POSTypeName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Description", pOSTypeDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", pOSTypeDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", pOSTypeDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute1", pOSTypeDTO.Attribute1));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute2", pOSTypeDTO.Attribute2));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute3", pOSTypeDTO.Attribute3));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute4", pOSTypeDTO.Attribute4));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute5", pOSTypeDTO.Attribute5));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the POSType record to the database
        /// </summary>
        /// <param name="pOSTypeDTO">POSTypeDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted POSType record</returns>
        public POSTypeDTO InsertPOSType(POSTypeDTO pOSTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(pOSTypeDTO, loginId, siteId);
            string query = @"INSERT INTO POSTypes 
                                        ( 
                                            POSTypeName,
                                            Description,
                                            site_id,
                                            MasterEntityId,
                                            IsActive,     
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate,                                        
                                            Attribute1,                                        
                                            Attribute2,                                        
                                            Attribute3,                                        
                                            Attribute4,                                        
                                            Attribute5                                        
                                        ) 
                                VALUES 
                                        (
                                            @POSTypeName,
                                            @Description,
                                            @site_id,
                                            @MasterEntityId,
                                            @isActive,     
                                            @createdBy,
                                            getDate(),
                                            @lastUpdatedBy,
                                            getDate(),
                                            @Attribute1,
                                            @Attribute2,
                                            @Attribute3,
                                            @Attribute4,
                                            @Attribute5
                                            )
                                        SELECT * FROM POSTypes WHERE POSTypeId = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(pOSTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPOSTypeDTO(pOSTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting POSTypesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);

                throw;
            }
            log.LogMethodExit(pOSTypeDTO);
            return pOSTypeDTO;
        }

        /// <summary>
        /// Updates the POSType record
        /// </summary>
        /// <param name="pOSTypeDTO">POSTypeDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns updated POSType record</returns>
        public POSTypeDTO UpdatePOSType(POSTypeDTO pOSTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(pOSTypeDTO, loginId, siteId);
            string query = @"UPDATE POSTypes 
                             SET POSTypeName = @POSTypeName,
                                 Description = @Description,
                                 MasterEntityId = @MasterEntityId,
                                 IsActive=@isActive,
                                 LastUpdateDate = Getdate(),
                                 LastUpdatedBy = @lastUpdatedBy ,
                                 Attribute1 = @Attribute1 ,
                                 Attribute2 = @Attribute2 ,
                                 Attribute3 = @Attribute3 ,
                                 Attribute4 = @Attribute4 ,
                                 Attribute5 = @Attribute5 
                                 -- site_id = @site_id
                                 WHERE POSTypeId =@POSTypeId
                                    SELECT * FROM POSTypes WHERE POSTypeId = @POSTypeId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(pOSTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPOSTypeDTO(pOSTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating pOSTypeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(pOSTypeDTO);
            return pOSTypeDTO;
        }

        /// <summary>
        /// Converts the Data row object to POSTypeDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns POSTypeDTO</returns>
        private POSTypeDTO GetPOSTypeDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            POSTypeDTO pOSTypeDTO = new POSTypeDTO(Convert.ToInt32(dataRow["POSTypeId"]),
                                            dataRow["POSTypeName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["POSTypeName"]),
                                            dataRow["Description"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Description"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                             dataRow["Attribute1"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Attribute1"]),
                                             dataRow["Attribute2"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Attribute2"]),
                                             dataRow["Attribute3"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Attribute3"]),
                                             dataRow["Attribute4"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Attribute4"]),
                                             dataRow["Attribute5"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Attribute5"])
                                            );
            log.LogMethodExit(pOSTypeDTO);
            return pOSTypeDTO;
        }

        /// <summary>
        /// Gets the POSType data of passed POSType Id
        /// </summary>
        /// <param name="pOSTypeId">integer type parameter</param>
        /// <returns>Returns POSTypeDTO</returns>
        public POSTypeDTO GetPOSTypeDTO(int pOSTypeId)
        {
            log.LogMethodEntry(pOSTypeId);
            POSTypeDTO returnValue = null;
            string query = SELECT_QUERY + @" WHERE pt.POSTypeId= @POSTypeId";
            SqlParameter parameter = new SqlParameter("@POSTypeId", pOSTypeId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetPOSTypeDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Deletes the POSType record of passed POSType Id
        /// </summary>
        /// <param name="pOSTypeId">integer type parameter</param>
        internal void DeletePOSType(int pOSTypeId)
        {
            log.LogMethodEntry(pOSTypeId);
            string query = @"DELETE  
                             FROM POSTypes
                             WHERE POSTypes.POSTypeId = @POSTypeId";
            SqlParameter parameter = new SqlParameter("@POSTypeId", pOSTypeId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="achievementClassDTO">AchievementClassDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshPOSTypeDTO(POSTypeDTO pOSTypeDTO, DataTable dt)
        {
            log.LogMethodEntry(pOSTypeDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                pOSTypeDTO.POSTypeId = Convert.ToInt32(dt.Rows[0]["POSTypeId"]);
                pOSTypeDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                pOSTypeDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                pOSTypeDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                pOSTypeDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                pOSTypeDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                pOSTypeDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the POSTypeDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of POSTypeDTO matching the search criteria</returns>
        public List<POSTypeDTO> GetPOSTypeDTOList(List<KeyValuePair<POSTypeDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<POSTypeDTO> pOSTypeDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<POSTypeDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == POSTypeDTO.SearchByParameters.POS_TYPE_ID ||
                            searchParameter.Key == POSTypeDTO.SearchByParameters.MASTER_ENTITY_ID )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == POSTypeDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == POSTypeDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == POSTypeDTO.SearchByParameters.POS_TYPE_NAME ||
                                 searchParameter.Key == POSTypeDTO.SearchByParameters.POS_TYPE_NAME_LIST)   //string
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }

                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                pOSTypeDTOList = new List<POSTypeDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    POSTypeDTO pOSTypeDTO = GetPOSTypeDTO(dataRow);
                    pOSTypeDTOList.Add(pOSTypeDTO);
                }
            }
            log.LogMethodExit(pOSTypeDTOList);
            return pOSTypeDTOList;
        }

        /// <summary>
        /// Gets the productKey and licenseKey
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="SiteKey"></param>
        /// <param name="LicenseKey"></param>
        public void ReadKeysFromDB(int siteId, ref string SiteKey, ref string LicenseKey)
        {
            log.LogMethodEntry(siteId);
            string query = @"select * from ProductKey where site_id = @site_id or @site_id = -1 order by site_id";
            SqlParameter parameter = new SqlParameter("@site_id", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                byte[] buffer;
                string siteKey = "", licenseKey = "";
                buffer = (byte[])dataTable.Rows[0][0];
                for (int i = 0; i < buffer.Length; i++)
                {
                    if (buffer[i] != 0)
                        siteKey += (char)buffer[i];
                }
                SiteKey = siteKey;
                buffer = (byte[])dataTable.Rows[0][1];
                for (int i = 0; i < buffer.Length; i++)
                {
                    if (buffer[i] != 0)
                        licenseKey += (char)buffer[i];
                }
                LicenseKey = licenseKey;
            }
            else
            {
                SiteKey = "";
                LicenseKey = "";
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the productKey and licenseKey
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="SiteKey"></param>
        /// <param name="LicenseKey"></param>
        public object NoOfPOSMachinesLicensed(int siteId)
        {
            log.LogMethodEntry(siteId);
            object noOfPOSMachinesLicensed = null;
            string query = @"select NoOfPOSMachinesLicensed from productKey where (site_id = @site_id or @site_id = -1)";
            SqlParameter parameter = new SqlParameter("@site_id", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                if (dataTable.Rows[0][0] != null)
                {
                    noOfPOSMachinesLicensed = dataTable.Rows[0][0];
                }
            }
            log.LogMethodExit(noOfPOSMachinesLicensed);
            return noOfPOSMachinesLicensed;
        }
        /// <summary>
        /// Inserts the managementFormAccess record to the database
        /// </summary>
        /// <param name="formName">string type object</param>
        /// <param name="functionalGuid">string type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void AddManagementFormAccess(string formName, string functionGuid, int siteId, bool isActive)
        {
            log.LogMethodEntry(formName, functionGuid, siteId);
            string query = @"exec InsertOrUpdateManagementFormAccess 'POS Counter',@formName,'Data Access',@siteId,@functionGuid,@isActive";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", formName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGuid", functionGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", isActive));
            dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Rename the managementFormAccess record to the database
        /// </summary>
        /// <param name="newFormName">string type object</param>
        /// <param name="formName">string type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void RenameManagementFormAccess(string newFormName, string formName, int siteId, string functionGuid)
        {
            log.LogMethodEntry(newFormName, formName, siteId);
            string query = @"exec RenameManagementFormAccess @newFormName,'POS Counter',@formName,'Data Access',@siteId, @functionGuid";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@newFormName", newFormName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", formName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGuid", functionGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Update the managementFormAccess record to the database
        /// </summary>
        /// <param name="formName">string type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="updatedIsActive">Site to which the record belongs</param>
        /// <param name="functionGuid">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void UpdateManagementFormAccess(string formName, int siteId, bool updatedIsActive, string functionGuid)
        {
            log.LogMethodEntry(formName, siteId);
            string query = @"exec InsertOrUpdateManagementFormAccess 'POS Counter',@formName,'Data Access',@siteId,@functionGuid,@updatedIsActive";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", formName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@updatedIsActive", updatedIsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGuid", functionGuid));
            dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }
    }
}
