/********************************************************************************************
 * Project Name - Object Translations Data Handler
 * Description  - Data handler of the Object Translations
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        02-Dec-2016   Raghuveera        Created
 *2.60        04-Mar-2019   Akshay Gulaganji  Added Created by parameter in InsertObjectTranslations() method
 *            25-Mar-2019   Mushahid Faizan   Modified- Author Version, added log Method Entry & Exit, removed unnecessary namespaces
 *2.60.0      03-May-2019   Divya             SQL Injection
 *2.70.2        25-Jul-2019   Dakshakh          Modified : added GetSQLParameters(), 
 *                                                       SQL injection Issue Fix and
 *                                                       Added CreatedBy,CreationDate to insert/update method
 *2.70.2      06-Dec-2019   Jinto Thomas      Removed siteid from update query        
 *2.80        03-Mar-2020   Mushahid Faizan   Modified : 3 tier Changes for REST API
*2.100.0     31-Aug-2020   Mushahid Faizan   siteId changes in GetSQLParameters().
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Object Translations Data Handler - Handles insert, update and select of object translations objects
    /// </summary>
    public class ObjectTranslationsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction = null;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ObjectTranslations ";

        /// <summary>
        /// Dictionary for searching Parameters for the ObjectTranslations object.
        /// </summary>
        private static readonly Dictionary<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string> DBSearchParameters = new Dictionary<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>
            {
                {ObjectTranslationsDTO.SearchByObjectTranslationsParameters.ID, "Id"},
                {ObjectTranslationsDTO.SearchByObjectTranslationsParameters.LANGUAGE_ID, "LanguageId"},
                {ObjectTranslationsDTO.SearchByObjectTranslationsParameters.ELEMENT, "Element"},
                {ObjectTranslationsDTO.SearchByObjectTranslationsParameters.OBJECT, "Object"},
                {ObjectTranslationsDTO.SearchByObjectTranslationsParameters.ELEMENT_GUID, "ElementGuid"},
                {ObjectTranslationsDTO.SearchByObjectTranslationsParameters.SITE_ID, "site_id"},
                {ObjectTranslationsDTO.SearchByObjectTranslationsParameters.IS_ACTIVE, "IsActive"},
                {ObjectTranslationsDTO.SearchByObjectTranslationsParameters.MASTERENTITYID, "MasterEntityId"}
            };

        /// <summary>
        /// Default constructor of ObjectTranslationsDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ObjectTranslationsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        ///Builds the SQL Parameter list used for inserting and updating objectTranslationsDTO Reecord.
        /// </summary>
        /// <param name="objectTranslationsDTO">objectTranslationsDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns> Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(ObjectTranslationsDTO objectTranslationsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(objectTranslationsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", objectTranslationsDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@languageId", objectTranslationsDTO.LanguageId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@elementGuid", string.IsNullOrEmpty(objectTranslationsDTO.ElementGuid) ? DBNull.Value : (object)objectTranslationsDTO.ElementGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@object", string.IsNullOrEmpty(objectTranslationsDTO.TableObject) ? DBNull.Value : (object)objectTranslationsDTO.TableObject));
            parameters.Add(dataAccessHandler.GetSQLParameter("@element", string.IsNullOrEmpty(objectTranslationsDTO.Element) ? DBNull.Value : (object)objectTranslationsDTO.Element));
            parameters.Add(dataAccessHandler.GetSQLParameter("@translation", string.IsNullOrEmpty(objectTranslationsDTO.Translation) ? DBNull.Value : (object)objectTranslationsDTO.Translation));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", objectTranslationsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", objectTranslationsDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Object Translations record to the database
        /// </summary>
        /// <param name="objectTranslations">ObjectTranslationsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public ObjectTranslationsDTO InsertObjectTranslations(ObjectTranslationsDTO objectTranslations, string loginId, int siteId)
        {

            log.LogMethodEntry(objectTranslations, loginId, siteId);
            string insertObjectTranslationsQuery = @"INSERT INTO [dbo].[ObjectTranslations]  
                                                        ( 
                                                        LanguageId,
                                                        ElementGuid,
                                                        Object,
                                                        Element,
                                                        Translation,
                                                        LastUpdatedBy,
                                                        LastUpdatedDate,
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        IsActive
                                                        ) 
                                                values 
                                                        (
                                                         @languageId,
                                                         @elementGuid,
                                                         @object,
                                                         @element,
                                                         @translation,
                                                         @lastUpdatedBy,
                                                         Getdate(),
                                                         NEWID(),
                                                         @siteid,
                                                         @masterEntityId,
                                                         @createdBy,
                                                         GETDATE(),
                                                         @IsActive
                                                        )SELECT * FROM ObjectTranslations WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertObjectTranslationsQuery, GetSQLParameters(objectTranslations, loginId, siteId).ToArray(), sqlTransaction);
                RefreshObjectTranslationsDTO(objectTranslations, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ObjectTranslations", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(objectTranslations);
            return objectTranslations;
        }

        /// <summary>
        /// Updates the Object Translations record
        /// </summary>
        /// <param name="objectTranslations">ObjectTranslationsDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public ObjectTranslationsDTO UpdateObjectTranslations(ObjectTranslationsDTO objectTranslations, string loginId, int siteId)
        {

            log.LogMethodEntry(objectTranslations, loginId, siteId);
            string updateObjectTranslationsQuery = @"update ObjectTranslations 
                                         set LanguageId = @languageId,
                                             ElementGuid = @elementGuid,
                                             Object = @object,
                                             Element = @element,
                                             Translation = @translation,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             --site_id=@siteid,
                                             MasterEntityId = @masterEntityId ,
                                             IsActive = @IsActive
                                       where Id = @id
                                       SELECT* FROM ObjectTranslations WHERE Id = @id ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateObjectTranslationsQuery, GetSQLParameters(objectTranslations, loginId, siteId).ToArray(), sqlTransaction);
                RefreshObjectTranslationsDTO(objectTranslations, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating ObjectTranslations", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(objectTranslations);
            return objectTranslations;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="objectTranslationsDTO">objectTranslationsDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshObjectTranslationsDTO(ObjectTranslationsDTO objectTranslationsDTO, DataTable dt)
        {
            log.LogMethodEntry(objectTranslationsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                objectTranslationsDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                objectTranslationsDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                objectTranslationsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                objectTranslationsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                objectTranslationsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                objectTranslationsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                objectTranslationsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ObjectTranslationsDTO class type
        /// </summary>
        /// <param name="objectTranslationsDataRow">ObjectTranslations DataRow</param>
        /// <returns>Returns ObjectTranslations</returns>
        private ObjectTranslationsDTO GetObjectTranslationsDTO(DataRow objectTranslationsDataRow)
        {
            log.LogMethodEntry(objectTranslationsDataRow);
            ObjectTranslationsDTO objectTranslationsDataObject = new ObjectTranslationsDTO(Convert.ToInt32(objectTranslationsDataRow["Id"]),
                                            objectTranslationsDataRow["LanguageId"] == DBNull.Value ? -1 : Convert.ToInt32(objectTranslationsDataRow["LanguageId"]),
                                            objectTranslationsDataRow["ElementGuid"].ToString(),
                                            objectTranslationsDataRow["Object"].ToString(),
                                            objectTranslationsDataRow["Element"].ToString(),
                                            objectTranslationsDataRow["Translation"].ToString(),
                                            objectTranslationsDataRow["LastUpdatedBy"].ToString(),
                                            objectTranslationsDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(objectTranslationsDataRow["LastupdatedDate"]),
                                            objectTranslationsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(objectTranslationsDataRow["site_id"]),
                                            objectTranslationsDataRow["Guid"].ToString(),
                                            objectTranslationsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(objectTranslationsDataRow["SynchStatus"]),
                                            objectTranslationsDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(objectTranslationsDataRow["MasterEntityId"]),
                                            objectTranslationsDataRow["CreatedBy"] == DBNull.Value ? string.Empty : objectTranslationsDataRow["CreatedBy"].ToString(),
                                            objectTranslationsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(objectTranslationsDataRow["CreationDate"]),
                                            objectTranslationsDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(objectTranslationsDataRow["IsActive"])
                                            );
            log.LogMethodExit(objectTranslationsDataObject);
            return objectTranslationsDataObject;
        }

        /// <summary>
        /// Gets the Object Translations data of passed id
        /// </summary>
        /// <param name="Id">integer type parameter</param>
        /// <returns>Returns ObjectTranslationsDTO</returns>
        public ObjectTranslationsDTO GetObjectTranslations(int Id)
        {
            log.LogMethodEntry(Id);
            ObjectTranslationsDTO objectTranslationsDTO = new ObjectTranslationsDTO();
            string selectObjectTranslationsQuery = @"SELECT *
                                              FROM ObjectTranslations
                                             WHERE Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", Id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectObjectTranslationsQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                objectTranslationsDTO = GetObjectTranslationsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(objectTranslationsDTO);
            return objectTranslationsDTO;
        }

        internal DateTime? GetObjectTranslationsModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"select max(LastupdatedDate) LastupdatedDate from ObjectTranslations WHERE (site_id = @siteId or @siteId = -1) ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@siteId", siteId));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastupdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastupdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the ObjectTranslationsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ObjectTranslationsDTO matching the search criteria</returns>
        public List<ObjectTranslationsDTO> GetObjectTranslationsList(List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectObjectTranslationsQuery = @"SELECT *
                                              FROM ObjectTranslations";
            List<ObjectTranslationsDTO> objectTranslationsList = new List<ObjectTranslationsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                string joiner = string.Empty;
                foreach (KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (count == 0)
                        {
                            joiner = " ";
                        }
                        else
                        {
                            joiner = " and ";
                        }
                        if (searchParameter.Key == ObjectTranslationsDTO.SearchByObjectTranslationsParameters.ID
                            || searchParameter.Key == ObjectTranslationsDTO.SearchByObjectTranslationsParameters.LANGUAGE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ObjectTranslationsDTO.SearchByObjectTranslationsParameters.OBJECT
                            || searchParameter.Key == ObjectTranslationsDTO.SearchByObjectTranslationsParameters.ELEMENT
                            || searchParameter.Key == ObjectTranslationsDTO.SearchByObjectTranslationsParameters.ELEMENT_GUID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ObjectTranslationsDTO.SearchByObjectTranslationsParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == ObjectTranslationsDTO.SearchByObjectTranslationsParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + " =-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'~') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                if (searchParameters.Count > 0)
                    selectObjectTranslationsQuery = selectObjectTranslationsQuery + query;
            }
            DataTable objectTranslationsData = dataAccessHandler.executeSelectQuery(selectObjectTranslationsQuery, parameters.ToArray(), sqlTransaction);
            if (objectTranslationsData.Rows.Count > 0)
            {
                objectTranslationsList = new List<ObjectTranslationsDTO>();
                foreach (DataRow objectTranslationsDataRow in objectTranslationsData.Rows)
                {
                    ObjectTranslationsDTO objectTranslationsDataObject = GetObjectTranslationsDTO(objectTranslationsDataRow);
                    objectTranslationsList.Add(objectTranslationsDataObject);
                }

            }
            log.LogMethodExit(objectTranslationsList);
            return objectTranslationsList;
        }
    }
}
