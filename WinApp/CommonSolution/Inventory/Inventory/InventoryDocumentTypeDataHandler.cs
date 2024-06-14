/********************************************************************************************
 * Project Name - Inventory Document Type Data Handler
 * Description  - Data handler of the inventory document type class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By     Remarks          
 *********************************************************************************************
 *1.00        08-Aug-2016   Raghuveera      Created 
 *2.70.2        13-Jul-2019   Deeksha         Modifications as per three tier standard
 *2.70.2        09-Dec-2019   Jinto Thomas        Removed siteid from update query 
 *2.150.0     18-Aug-2022    Abhishek       Modified : Added GetInventoryDocumentTypeModuleLastUpdateTime method to get LastUpdate DateTime
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Inventory Document Type Data Handler - Handles insert, update and select of inventory document type objects
    /// </summary>
    public class InventoryDocumentTypeDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM InventoryDocumentType AS idt ";
        
        /// <summary>
        /// Dictionary for searching Parameters for the Inventory Document Type  object.
        /// </summary>
        private static readonly Dictionary<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string> DBSearchParameters = new Dictionary<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>
            {
                {InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.NAME, "idt.Name"},
                {InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.DOCUMENT_TYPE_ID, "idt.DocumentTypeId"},
                {InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.ACTIVE_FLAG, "idt.IsActive"},
                {InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.APPLICABILITY, "idt.Applicability"},
                {InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.CODE, "idt.Code"},
                {InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.MASTER_ENTITY_ID,"idt.MasterEntityId"},
                {InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.SITE_ID, "idt.site_id"}
            };

        /// <summary>
        /// Parameterized Constructor for InventoryDocumentTypeDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public InventoryDocumentTypeDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating InventoryDocumentTypeDataHandler Record.
        /// </summary>
        /// <param name="inventoryDocumentTypeDTO">InventoryDocumentTypeDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(InventoryDocumentTypeDTO inventoryDocumentTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryDocumentTypeDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@DocumentTypeId", inventoryDocumentTypeDTO.DocumentTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", inventoryDocumentTypeDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Description", string.IsNullOrEmpty(inventoryDocumentTypeDTO.Description) ? DBNull.Value : (object)inventoryDocumentTypeDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Applicability", string.IsNullOrEmpty(inventoryDocumentTypeDTO.Applicability) ? DBNull.Value : (object)inventoryDocumentTypeDTO.Applicability));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Code", inventoryDocumentTypeDTO.Code));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", inventoryDocumentTypeDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", inventoryDocumentTypeDTO.MasterEntityId, true));            
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Converts the Data row object to InventoryDocumentTypeDTO class type
        /// </summary>
        /// <param name="inventoryDocumentTypeDataRow">InventoryDocumentType DataRow</param>
        /// <returns>Returns InventoryDocumentType</returns>
        private InventoryDocumentTypeDTO GetInventoryDocumentTypeDTO(DataRow inventoryDocumentTypeDataRow)
        {
            log.LogMethodEntry(inventoryDocumentTypeDataRow);
            InventoryDocumentTypeDTO inventoryDocumentTypeDataObject = new InventoryDocumentTypeDTO(Convert.ToInt32(inventoryDocumentTypeDataRow["DocumentTypeId"]),
                                            inventoryDocumentTypeDataRow["Name"].ToString(),
                                            inventoryDocumentTypeDataRow["Description"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryDocumentTypeDataRow["Description"]),
                                            inventoryDocumentTypeDataRow["Applicability"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryDocumentTypeDataRow["Applicability"]),
                                            inventoryDocumentTypeDataRow["Code"].ToString(),
                                            inventoryDocumentTypeDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(inventoryDocumentTypeDataRow["IsActive"]),
                                            inventoryDocumentTypeDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryDocumentTypeDataRow["CreatedBy"]),
                                            inventoryDocumentTypeDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryDocumentTypeDataRow["CreationDate"]),
                                            inventoryDocumentTypeDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryDocumentTypeDataRow["LastUpdatedBy"]),
                                            inventoryDocumentTypeDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryDocumentTypeDataRow["LastupdatedDate"]),
                                            inventoryDocumentTypeDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryDocumentTypeDataRow["Guid"]),
                                            inventoryDocumentTypeDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryDocumentTypeDataRow["site_id"]),
                                            inventoryDocumentTypeDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(inventoryDocumentTypeDataRow["SynchStatus"]),
                                            inventoryDocumentTypeDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryDocumentTypeDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(inventoryDocumentTypeDataObject);
            return inventoryDocumentTypeDataObject;
        }

        /// <summary>
        /// Inserts the InventoryDocumentType record to the database
        /// </summary>
        /// <param name="inventoryDocumentTypeDTO">InventoryDocumentTypeDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public InventoryDocumentTypeDTO InsertInventoryDocumentType(InventoryDocumentTypeDTO inventoryDocumentTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryDocumentTypeDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[InventoryDocumentType]
                                                        (    
                                                          Name,
                                                          Description,
                                                          Applicability,
                                                          Code,
                                                          IsActive,
                                                          CreatedBy,
                                                          CreationDate,
                                                          LastUpdatedBy,
                                                          LastupdatedDate,
                                                          Guid,                                                    
                                                          Site_id,
                                                          MasterEntityId
                                                        ) 
                                                values 
                                                        (    
                                                          @Name,
                                                          @Description,
                                                          @Applicability,
                                                          @Code,
                                                          @IsActive,
                                                          @CreatedBy,
                                                          GETDATE(),
                                                          @LastUpdatedBy,
                                                          GETDATE(),
                                                          NewId(),
                                                          @SiteId,                                                       
                                                          @MasterEntityId)
                                               SELECT * FROM InventoryDocumentType WHERE DocumentTypeId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(inventoryDocumentTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInventoryDocumentTypeDTO(inventoryDocumentTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting inventoryDocumentTypeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(inventoryDocumentTypeDTO);
            return inventoryDocumentTypeDTO;
        }

        /// <summary>
        /// Updates the InventoryDocumentType record
        /// </summary>
        /// <param name="inventoryDocumentTypeDTO">InventoryDocumentTypeDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public InventoryDocumentTypeDTO UpdateInventoryDocumentType(InventoryDocumentTypeDTO inventoryDocumentTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryDocumentTypeDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[InventoryDocumentType]
                                    SET 
                                             Name = @Name,
                                             Description = @Description,
                                             Applicability = @Applicability,
                                             Code = @Code,
                                             IsActive = @IsActive,
                                             --Site_id = @SiteId,
                                             MasterEntityId = @MasterEntityId,
                                             LastupdatedDate = GETDATE(),
                                             LastUpdatedBy = @LastUpdatedBy
                                             
                                       WHERE DocumentTypeId =@DocumentTypeId 
                                    SELECT * FROM InventoryDocumentType WHERE DocumentTypeId = @DocumentTypeId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(inventoryDocumentTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInventoryDocumentTypeDTO(inventoryDocumentTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating inventoryDocumentTypeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(inventoryDocumentTypeDTO);
            return inventoryDocumentTypeDTO;
        }

        /// <summary>
        /// Delete the record from the InventoryDocumentType database based on DocumentTypeId
        /// </summary>
        /// <param name="documentTypeId">documentTypeId</param>
        /// <returns>return the int </returns>
        internal int Delete(int documentTypeId)
        {
            log.LogMethodEntry(documentTypeId);
            string query = @"DELETE  
                             FROM InventoryDocumentType
                             WHERE InventoryDocumentType.DocumentTypeId = @DocumentTypeId";
            SqlParameter parameter = new SqlParameter("@DocumentTypeId", documentTypeId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="inventoryDocumentTypeDTO">InventoryDocumentTypeDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshInventoryDocumentTypeDTO(InventoryDocumentTypeDTO inventoryDocumentTypeDTO, DataTable dt)
        {
            log.LogMethodEntry(inventoryDocumentTypeDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                inventoryDocumentTypeDTO.DocumentTypeId = Convert.ToInt32(dt.Rows[0]["DocumentTypeId"]);
                inventoryDocumentTypeDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                inventoryDocumentTypeDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                inventoryDocumentTypeDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                inventoryDocumentTypeDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                inventoryDocumentTypeDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                inventoryDocumentTypeDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the InventoryDocumentType data of passed id 
        /// </summary>
        /// <param name="id">id of InventoryDocumentType is passed as parameter</param>
        /// <returns>Returns InventoryDocumentType</returns>
        public InventoryDocumentTypeDTO GetInventoryDocumentType(int id)
        {
            log.LogMethodEntry(id);
            InventoryDocumentTypeDTO result = null;
            string query = SELECT_QUERY + @" WHERE idt.DocumentTypeId= @DocumentTypeId";
            SqlParameter parameter = new SqlParameter("@DocumentTypeId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetInventoryDocumentTypeDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the Document type data of passed document type Id
        /// </summary>
        /// <param name="name">string type parameter</param>
        /// <returns>Returns InventoryDocumentTypeDTO</returns>
        public InventoryDocumentTypeDTO GetInventoryDocumentType(string name)
        {
            log.LogMethodEntry(name);
            string selectInventoryDocumentTypeQuery = @"SELECT *
                                              FROM InventoryDocumentType
                                             WHERE name = @Name";
            SqlParameter[] selectInventoryDocumentTypeParameters = new SqlParameter[1];
            selectInventoryDocumentTypeParameters[0] = new SqlParameter("@Name", name);
            DataTable inventoryDocumentType = dataAccessHandler.executeSelectQuery(selectInventoryDocumentTypeQuery, selectInventoryDocumentTypeParameters, sqlTransaction);
            if (inventoryDocumentType.Rows.Count > 0)
            {
                DataRow InventoryDocumentTypeRow = inventoryDocumentType.Rows[0];
                InventoryDocumentTypeDTO inventoryDocumentTypeDataObject = GetInventoryDocumentTypeDTO(InventoryDocumentTypeRow);
                log.LogMethodExit(inventoryDocumentTypeDataObject);
                return inventoryDocumentTypeDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the Document type data of passed document type Id
        /// </summary>
        /// <param name="name">string type parameter</param>
        /// <param name="siteId">Site id is -1 then function will fetch all site record or else the function will return only that site record</param>
        /// <returns>Returns InventoryDocumentTypeDTO</returns>
        public List<InventoryDocumentTypeDTO> GetInventoryDocumentType(string name, int siteId)
        {
            log.LogMethodEntry(name, siteId);
            string selectInventoryDocumentTypeQuery = @"SELECT *
                                              FROM InventoryDocumentType
                                             WHERE name = @Name and (Site_id = @siteId or @siteId = -1)";
            SqlParameter[] selectInventoryDocumentTypeParameters = new SqlParameter[2];
            selectInventoryDocumentTypeParameters[0] = new SqlParameter("@Name", name);
            selectInventoryDocumentTypeParameters[1] = new SqlParameter("@siteId", siteId);
            DataTable inventoryDocumentType = dataAccessHandler.executeSelectQuery(selectInventoryDocumentTypeQuery, selectInventoryDocumentTypeParameters, sqlTransaction);
            if (inventoryDocumentType.Rows.Count > 0)
            {
                List<InventoryDocumentTypeDTO> inventoryDocumentTypeList = new List<InventoryDocumentTypeDTO>();
                foreach (DataRow inventoryDocumentTypeDataRow in inventoryDocumentType.Rows)
                {
                    InventoryDocumentTypeDTO inventoryDocumentTypeDataObject = GetInventoryDocumentTypeDTO(inventoryDocumentTypeDataRow);
                    inventoryDocumentTypeList.Add(inventoryDocumentTypeDataObject);
                }
                log.LogMethodExit(inventoryDocumentTypeList);
                return inventoryDocumentTypeList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the Document type data of passed document type Id
        /// </summary>
        /// <param name="applicability">String array holds the values of applicability </param>
        /// <param name="siteId">siteId parameter </param>
        /// <param name="sqlTrxn">SqlTransaction object </param>
        /// <returns>Returns List InventoryDocumentTypeDTO</returns>
        public List<InventoryDocumentTypeDTO> GetInventoryDocumentTypeList(string[] applicability, int siteId)
        {
            log.LogMethodEntry(applicability, siteId);
            string applicabilities="in (''";
            if(applicability!=null)
            {
                foreach(string s in applicability)
                {
                    applicabilities += ",'" + s + "'";
                }
            }
            applicabilities += ")";
            string selectInventoryDocumentTypeQuery = @"SELECT *
                                              FROM InventoryDocumentType
                                             WHERE Applicability " + applicabilities + " and (site_id = "+siteId+" or -1 = " + siteId + ") Order by Applicability, Name ";
            
            DataTable inventoryDocumentType = dataAccessHandler.executeSelectQuery(selectInventoryDocumentTypeQuery, null, sqlTransaction);
            if (inventoryDocumentType.Rows.Count > 0)
            {
                List<InventoryDocumentTypeDTO> inventoryDocumentTypeList = new List<InventoryDocumentTypeDTO>();
                foreach (DataRow inventoryDocumentTypeDataRow in inventoryDocumentType.Rows)
                {
                    InventoryDocumentTypeDTO inventoryDocumentTypeDataObject = GetInventoryDocumentTypeDTO(inventoryDocumentTypeDataRow);
                    inventoryDocumentTypeList.Add(inventoryDocumentTypeDataObject);
                }
                log.LogMethodExit(inventoryDocumentTypeList);
                return inventoryDocumentTypeList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }


        /// <summary>
        /// Gets the InventoryDocumentTypeDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTrxn">SqlTransaction object </param>
        /// <returns>Returns the list of InventoryDocumentTypeDTO matching the search criteria</returns>
        public List<InventoryDocumentTypeDTO> GetInventoryDocumentTypeList(List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<InventoryDocumentTypeDTO> inventoryDocumentTypeDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectInventoryDocumentTypeQuery = SELECT_QUERY;
            
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                       

                        if (searchParameter.Key == InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.DOCUMENT_TYPE_ID 
                                || searchParameter.Key == InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.MASTER_ENTITY_ID)
                                
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.ACTIVE_FLAG)
                        {
                            
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                            
                        }
                        else if (searchParameter.Key == InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.APPLICABILITY 
                                || searchParameter.Key == InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.CODE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }

                        else if (searchParameter.Key == InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.SITE_ID)
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
                if (searchParameters.Count > 0)
                    selectInventoryDocumentTypeQuery = selectInventoryDocumentTypeQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectInventoryDocumentTypeQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                inventoryDocumentTypeDTOList = new List<InventoryDocumentTypeDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    InventoryDocumentTypeDTO inventoryDocumentTypeDTO = GetInventoryDocumentTypeDTO(dataRow);
                    inventoryDocumentTypeDTOList.Add(inventoryDocumentTypeDTO);
                }
            }
            log.LogMethodExit(inventoryDocumentTypeDTOList);
            return inventoryDocumentTypeDTOList;
        }

        /// <summary>
        /// Gets the Document type last Update DateTime
        /// </summary>
        /// <param name="siteId">siteId parameter </param>
        /// <returns>Returns InventoryDocumentTypeModuleLastUpdateTime</returns>
        internal DateTime? GetInventoryDocumentTypeModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"select max(LastupdatedDate) LastupdatedDate from InventoryDocumentType WHERE (site_id = @siteId or @siteId = -1)";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastupdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastupdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
