/********************************************************************************************
 * Project Name - DSLookup Data Handler
 * Description  - Data handler of the DSLookup class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        06-Mar-2017   Lakshminarayana     Created 
 *2.70.2        30-Jul-2019   Dakshakh raj        Modified : added GetSQLParameters(),
 *                                                         SQL injection Issue Fix
 *2.70.2       06-Dec-2019   Jinto Thomas            Removed siteid from update query                                                          
 *2.90         07-Aug-2020   Mushahid Faizan     Modified : default isActive value to true.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    ///  DSLookup Data Handler - Handles insert, update and select of  DSLookup objects
    /// </summary>
    public class DSLookupDataHandler
    {
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM DSLookup AS dl";

        /// <summary>
        /// Dictionary for searching Parameters for the DSLookup object.
        /// </summary>
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<DSLookupDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<DSLookupDTO.SearchByParameters, string>
            {
                {DSLookupDTO.SearchByParameters.DSLOOKUP_ID, "dl.DSLookupID"},
                {DSLookupDTO.SearchByParameters.DSLOOKUP_NAME, "dl.DSLookupName"},
                {DSLookupDTO.SearchByParameters.IS_ACTIVE, "dl.Active_Flag"},
                {DSLookupDTO.SearchByParameters.MASTER_ENTITY_ID,"dl.MasterEntityId"},
                {DSLookupDTO.SearchByParameters.SITE_ID, "dl.site_id"}
            };

        /// <summary>
        /// Default constructor of DSLookupDataHandler class
        /// </summary>
        public DSLookupDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating dSLookupDTO parameters Record.
        /// </summary>
        /// <param name="dSLookupDTO">dSLookupDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>  Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(DSLookupDTO dSLookupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(dSLookupDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@DSLookupID", dSLookupDTO.DSLookupID,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DSLookupName", string.IsNullOrEmpty(dSLookupDTO.DSLookupName) ? DBNull.Value : (object)dSLookupDTO.DSLookupName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Description", string.IsNullOrEmpty(dSLookupDTO.Description) ? DBNull.Value : (object)dSLookupDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OffsetX", dSLookupDTO.OffsetX < 0 ? DBNull.Value : (object)dSLookupDTO.OffsetX));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OffsetY", dSLookupDTO.OffsetY < 0 ? DBNull.Value : (object)dSLookupDTO.OffsetY));
            parameters.Add(dataAccessHandler.GetSQLParameter("@HDR12Spacing", dSLookupDTO.HDR12Spacing < 0 ? DBNull.Value : (object)dSLookupDTO.HDR12Spacing));
            parameters.Add(dataAccessHandler.GetSQLParameter("@HDR23Spacing", dSLookupDTO.HDR23Spacing < 0 ? DBNull.Value : (object)dSLookupDTO.HDR23Spacing));
            parameters.Add(dataAccessHandler.GetSQLParameter("@HDR34Spacing", dSLookupDTO.HDR34Spacing < 0 ? DBNull.Value : (object)dSLookupDTO.HDR34Spacing));
            parameters.Add(dataAccessHandler.GetSQLParameter("@HDR45Spacing", dSLookupDTO.HDR45Spacing < 0 ? DBNull.Value : (object)dSLookupDTO.HDR45Spacing));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TextWidth", dSLookupDTO.TextWidth < 0 ? DBNull.Value : (object)dSLookupDTO.TextWidth));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TextHeight", dSLookupDTO.TextHeight < 0 ? DBNull.Value : (object)dSLookupDTO.TextHeight));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Dynamic_Flag", dSLookupDTO.DynamicFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Query", dSLookupDTO.Query));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RefreshDataSecs", dSLookupDTO.RefreshDataSecs < 0 ? DBNull.Value : (object)dSLookupDTO.RefreshDataSecs));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MoveDataSecs", dSLookupDTO.MoveDataSecs < 0 ? DBNull.Value : (object)dSLookupDTO.MoveDataSecs));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Active_Flag", (dSLookupDTO.IsActive == true? "Y":"N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@last_updated_user", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", dSLookupDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the DSLookup record to the database
        /// </summary>
        /// <param name="dSLookupDTO">DSLookupDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>DSLookup DTO</returns>
        public DSLookupDTO InsertDSLookup(DSLookupDTO dSLookupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(dSLookupDTO, loginId, siteId);
            string query = @"INSERT INTO DSLookup 
                                        ( 
                                            DSLookupName,
                                            Description,
                                            OffsetX,
                                            OffsetY,
                                            HDR12Spacing,
                                            HDR23Spacing,
                                            HDR34Spacing,
                                            HDR45Spacing,
                                            TextWidth,
                                            TextHeight,
                                            Dynamic_Flag,
                                            Query,
                                            RefreshDataSecs,
                                            MoveDataSecs,
                                            Active_Flag,
                                            CreatedUser,
                                            CreationDate,
                                            last_updated_user,
                                            last_updated_date,
                                            site_id,
                                            guid,
                                            MasterEntityId
                                        ) 
                                VALUES 
                                        (
                                            @DSLookupName,
                                            @Description,
                                            @OffsetX,
                                            @OffsetY,
                                            @HDR12Spacing,
                                            @HDR23Spacing,
                                            @HDR34Spacing,
                                            @HDR45Spacing,
                                            @TextWidth,
                                            @TextHeight,
                                            @Dynamic_Flag,
                                            @Query,
                                            @RefreshDataSecs,
                                            @MoveDataSecs,
                                            @Active_Flag,
                                            @CreatedUser,
                                            GETDATE(),
                                            @last_updated_user,
                                            GETDATE(),
                                            @site_id,
                                            NEWID(),
                                            @MasterEntityId
                                        )SELECT * FROM DSLookup WHERE DSLookupID = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(dSLookupDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDSLookupDTO(dSLookupDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting dSLookupDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(dSLookupDTO);
            return dSLookupDTO;
        }

        /// <summary>
        /// Updates the DSLookup record
        /// </summary>
        /// <param name="dSLookupDTO">DSLookupDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>DSLookup DTO</returns>
        public DSLookupDTO UpdateDSLookup(DSLookupDTO dSLookupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(dSLookupDTO, loginId, siteId);
            string query = @"UPDATE DSLookup 
                             SET DSLookupName=@DSLookupName,
                                 Description=@Description,
                                 OffsetX=@OffsetX,
                                 OffsetY=@OffsetY,
                                 HDR12Spacing=@HDR12Spacing,
                                 HDR23Spacing=@HDR23Spacing,
                                 HDR34Spacing=@HDR34Spacing,
                                 HDR45Spacing=@HDR45Spacing,
                                 TextWidth=@TextWidth,
                                 TextHeight=@TextHeight,
                                 Dynamic_Flag=@Dynamic_Flag,
                                 Query=@Query,
                                 RefreshDataSecs=@RefreshDataSecs,
                                 MoveDataSecs=@MoveDataSecs,
                                 Active_Flag=@Active_Flag,
                                 last_updated_user=@last_updated_user,
                                 last_updated_date = GETDATE()
                                 --site_id=@site_id
                             WHERE DSLookupID = @DSLookupID
                             SELECT* FROM DSLookup WHERE  DSLookupID = @DSLookupID";
            try
            {
                if (string.Equals(dSLookupDTO.IsActive, "N") && GetDSLookupReferenceCount(dSLookupDTO.DSLookupID) > 0)
                {
                    throw new ForeignKeyException("Cannot Inactivate records for which matching detail data exists.");
                }
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(dSLookupDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDSLookupDTO(dSLookupDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating dSLookupDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(dSLookupDTO);
            return dSLookupDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="dSLookupDTO">dSLookupDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshDSLookupDTO(DSLookupDTO dSLookupDTO, DataTable dt)
        {
            log.LogMethodEntry(dSLookupDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                dSLookupDTO.DSLookupID = Convert.ToInt32(dt.Rows[0]["DSLookupID"]);
                dSLookupDTO.LastUpdateDate = dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]);
                dSLookupDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                dSLookupDTO.Guid = dataRow["guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["guid"]);
                dSLookupDTO.LastUpdatedBy = dataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_updated_user"]);
                dSLookupDTO.CreatedBy = dataRow["CreatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedUser"]);
                dSLookupDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Checks whether dSLookup is in use.
        /// <param name="id">DSLookup Id</param>
        /// </summary>
        /// <returns>Returns refrenceCount</returns>
        private int GetDSLookupReferenceCount(int id)
        {
            log.LogMethodEntry(id);
            int refrenceCount = 0;
            string query = @"SELECT COUNT(*) AS ReferenceCount
                             FROM ScreenZoneContentMap
                             WHERE ContentType='LOOKUP' AND ContentID = @DSLookupId AND Active_Flag = 'Y'";
            SqlParameter parameter = new SqlParameter("@DSLookupId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if(dataTable.Rows.Count > 0)
            {
                refrenceCount = Convert.ToInt32(dataTable.Rows[0]["ReferenceCount"]);
            }
            log.LogMethodExit(refrenceCount);
            return refrenceCount;
        }

        /// <summary>
        /// Converts the Data row object to DSLookupDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns DSLookupDTO</returns>
        private DSLookupDTO GetDSLookupDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            DSLookupDTO dSLookupDTO = new DSLookupDTO(Convert.ToInt32(dataRow["DSLookupID"]),
                                            dataRow["DSLookupName"] == DBNull.Value ? "" : dataRow["DSLookupName"].ToString(),
                                            dataRow["Description"] == DBNull.Value ? "" : dataRow["Description"].ToString(),
                                            dataRow["offsetX"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["offsetX"]),
                                            dataRow["OffsetY"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["OffsetY"]),
                                            dataRow["HDR12Spacing"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["HDR12Spacing"]),
                                            dataRow["HDR23Spacing"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["HDR23Spacing"]),
                                            dataRow["HDR34Spacing"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["HDR34Spacing"]),
                                            dataRow["HDR45Spacing"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["HDR45Spacing"]),
                                            dataRow["TextWidth"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["TextWidth"]),
                                            dataRow["TextHeight"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["TextHeight"]),
                                            dataRow["Dynamic_Flag"] == DBNull.Value ? "N" : dataRow["Dynamic_Flag"].ToString(),
                                            dataRow["Query"] == DBNull.Value ? "" : dataRow["Query"].ToString(),
                                            dataRow["RefreshDataSecs"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["RefreshDataSecs"]),
                                            dataRow["MoveDataSecs"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["MoveDataSecs"]),
                                            dataRow["Active_Flag"] == DBNull.Value ? true : (dataRow["Active_Flag"].ToString() == "Y"? true: false),
                                            dataRow["CreatedUser"] == DBNull.Value ? "" : dataRow["CreatedUser"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["last_updated_user"] == DBNull.Value ? "" : dataRow["last_updated_user"].ToString(),
                                            dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["guid"] == DBNull.Value ? "" : dataRow["guid"].ToString()
                                            );
            log.LogMethodExit(dSLookupDTO);
            return dSLookupDTO;
        }

        /// <summary>
        /// Gets the DSLookup data of passed DSLookupId
        /// </summary>
        /// <param name="dSLookupID">integer type parameter</param>
        /// <returns>Returns DSLookupDTO</returns>
        public DSLookupDTO GetDSLookupDTO(int dSLookupID)
        {
            log.LogMethodEntry(dSLookupID);
            DSLookupDTO returnValue = null;
            string query = SELECT_QUERY + @" WHERE dl.DSLookupID = @DSLookupID";
            SqlParameter parameter = new SqlParameter("@DSLookupID", dSLookupID);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if(dataTable.Rows.Count > 0)
            {
                returnValue = GetDSLookupDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the DSLookup data of passed Guid
        /// </summary>
        /// <param name="contentGuid">string type parameter</param>
        /// <returns>Returns DSLookupDTO</returns>
        public DSLookupDTO GetDSLookupDTOByGuid(string contentGuid)
        {
            log.LogMethodEntry(contentGuid);
            DSLookupDTO returnValue = null;
            string query = SELECT_QUERY + @" WHERE dl.guid = @ContentGuid";
            SqlParameter parameter = new SqlParameter("@ContentGuid", contentGuid);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetDSLookupDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Checks whether the query is valid. 
        /// </summary>
        /// <param name="query">query</param>
        public bool CheckQuery(string query)
        {
            log.LogMethodEntry(query);
            bool valid = false;
            if(ValidateQueryString(query))
            {
                try
                {
                    dataAccessHandler.executeSelectQuery(query, new System.Data.SqlClient.SqlParameter[] { }, sqlTransaction);
                    valid = true;
                }
                catch(Exception)
                {
                    valid = false;
                }
            }
            log.LogMethodExit(valid);
            return valid;
        }

        /// <summary>
        /// Gets Dynamic Content Data
        /// </summary>
        /// <param name="query">query</param>
        public List<List<string>> GetDynamicContentData(string query)
        {
            log.LogMethodEntry(query);
            List<List<string>> stringTable = null;
            List<string> stringRow;
            if(ValidateQueryString(query))
            {
                try
                {
                    DataTable dataTable = dataAccessHandler.executeSelectQuery(query, null, sqlTransaction);
                    if(dataTable.Rows.Count > 0)
                    {
                        stringTable = new List<List<string>>();
                        foreach(DataRow dataRow in dataTable.Rows)
                        {
                            stringRow = new List<string>();
                            for(int i = 0; i < dataTable.Columns.Count; i++)
                            {
                                if(dataRow[i] != DBNull.Value)
                                {
                                    stringRow.Add(Convert.ToString(dataRow[i]));
                                }
                                else
                                {
                                    stringRow.Add("");
                                }

                            }
                            stringTable.Add(stringRow);
                        }
                    }
                }
                catch(Exception)
                {
                    stringTable = null;
                }
            }
            log.LogMethodExit(stringTable);
            return stringTable;
        }

        /// <summary>
        /// Validate Query String
        /// </summary>
        /// <param name="query">query</param>
        /// <returns></returns>
        private bool ValidateQueryString(string query)
        {
            log.LogMethodEntry(query);
            bool valid = true;
            if (!string.IsNullOrEmpty(query))
            {
                query = Regex.Replace(query, @"\s+", " ");
                CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
                if (compareInfo.IndexOf(query, "DELETE ", CompareOptions.IgnoreCase) >= 0)
                {
                    valid = false;
                }
                //if (compareInfo.IndexOf(query, "DROP ", CompareOptions.IgnoreCase) >= 0)
                //{
                //    valid = false;
                //}
                //if (compareInfo.IndexOf(query, "ALTER ", CompareOptions.IgnoreCase) >= 0)
                //{
                //    valid = false;
                //}
                //if (compareInfo.IndexOf(query, "TRUNCATE TABLE ", CompareOptions.IgnoreCase) >= 0)
                //{
                //    valid = false;
                //}
                //if (compareInfo.IndexOf(query, "CREATE ", CompareOptions.IgnoreCase) >= 0)
                //{
                //    valid = false;
                //}
                //if (compareInfo.IndexOf(query, "EXEC ", CompareOptions.IgnoreCase) >= 0)
                //{
                //    valid = false;
                //}
                //if (compareInfo.IndexOf(query, "INSERT ", CompareOptions.IgnoreCase) >= 0)
                //{
                //    valid = false;
                //}
                //if (compareInfo.IndexOf(query, "UPDATE ", CompareOptions.IgnoreCase) >= 0)
                //{
                //    valid = false;
                //}
                //if (compareInfo.IndexOf(query, "SELECT ", CompareOptions.IgnoreCase) < 0)
                //{
                //    valid = false;
                //}
            }
            else
            {
                valid = false;
            }
            log.LogMethodExit(valid);
            return valid;
        }

        /// <summary>
        /// Gets the MediaDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of DSLookupDTO matching the search criteria</returns>
        public List<DSLookupDTO> GetDSLookupDTOList(List<KeyValuePair<DSLookupDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<DSLookupDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach(KeyValuePair<DSLookupDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if(DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if(searchParameter.Key == DSLookupDTO.SearchByParameters.DSLOOKUP_ID ||
                            searchParameter.Key == DSLookupDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if(searchParameter.Key == DSLookupDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if(searchParameter.Key == DSLookupDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
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
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if(dataTable.Rows.Count > 0)
            {
                list = new List<DSLookupDTO>();
                foreach(DataRow dataRow in dataTable.Rows)
                {
                    DSLookupDTO dSLookupDTO = GetDSLookupDTO(dataRow);
                    list.Add(dSLookupDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }    
}
