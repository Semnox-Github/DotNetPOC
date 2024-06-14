/********************************************************************************************
 * Project Name - Screen Setup Data Handler
 * Description  - Data handler of the Screen Setup Data Handler
 * 
 **************
 **Version Log
 ************** 
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        07-Mar-2017   Raghuveera          Created 
 *2.70.2        31-Jul-2019   Dakshakh raj        Modified : added GetSQLParameters(),
 *                                                         SQL injection Issue Fix
 *2.70.2       06-Dec-2019   Jinto Thomas         Removed siteid from update query                                                          
 *2.90         13-Aug-2020   Mushahid Faizan     Modified : default isActive value to true.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// Screen Setup Data Handler  - Handles insert, update and select of screen setup data handler
    /// </summary>
    public class ScreenSetupDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM ScreenSetup AS ss";

        /// <summary>
        /// Dictionary for searching Parameters for the ScreenSetup object.
        /// </summary>
        private static readonly Dictionary<ScreenSetupDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ScreenSetupDTO.SearchByParameters, string>
            {
                {ScreenSetupDTO.SearchByParameters.SCREEN_ID, "ss.ScreenId"},
                {ScreenSetupDTO.SearchByParameters.NAME, "ss.Name"},
                {ScreenSetupDTO.SearchByParameters.IS_ACTIVE, "ss.Active_Flag"},
                {ScreenSetupDTO.SearchByParameters.SITE_ID, "ss.site_id"},
                {ScreenSetupDTO.SearchByParameters.MASTER_ENTITY_ID, "ss.MasterEntityId"},
            };

        /// <summary>
        /// Default constructor of ScreenSetupDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ScreenSetupDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating screenSetupDTO parameters Record.
        /// </summary>
        /// <param name="screenSetupDTO">screenSetupDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>  Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(ScreenSetupDTO screenSetupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(screenSetupDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@screenId", screenSetupDTO.ScreenId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@name", string.IsNullOrEmpty(screenSetupDTO.Name) ? DBNull.Value : (object)screenSetupDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@alignment", screenSetupDTO.Alignment, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@active_Flag", (screenSetupDTO.IsActive == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@scrDivHorizontal", screenSetupDTO.ScrDivHorizontal == -1 ? DBNull.Value : (object)screenSetupDTO.ScrDivHorizontal));
            parameters.Add(dataAccessHandler.GetSQLParameter("@scrDivVertical", screenSetupDTO.ScrDivVertical == -1 ? DBNull.Value : (object)screenSetupDTO.ScrDivVertical));
            parameters.Add(dataAccessHandler.GetSQLParameter("@description", string.IsNullOrEmpty(screenSetupDTO.Description) ? DBNull.Value : (object)screenSetupDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", screenSetupDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@updatedby", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the screen setup record to the database
        /// </summary>
        /// <param name="screenSetupDTO">ScreenSetupDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">SQL Transactions </param>
        /// <returns>ScreenSetup DTO</returns>
        public ScreenSetupDTO InsertScreenSetup(ScreenSetupDTO screenSetupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(screenSetupDTO, loginId, siteId, sqlTransaction);
            string insertScreenSetupQuery = @"insert into ScreenSetup 
                                                        ( 
                                                        Name,
                                                        Alignment,
                                                        ScrDivHorizontal,
                                                        ScrDivVertical,
                                                        Description,
                                                        Active_Flag,
                                                        CreatedUser,
                                                        Creationdate,
                                                        last_updated_user,
                                                        last_updated_date,
                                                        Guid,
                                                        site_id
                                                        ) 
                                                values 
                                                        (                                                         
                                                         @name,
                                                         @alignment,
                                                         @scrDivHorizontal,
                                                         @scrDivVertical,
                                                         @description,
                                                         @active_Flag,
                                                         @createdBy,
                                                         Getdate(),
                                                         @updatedby,
                                                         Getdate(),  
                                                         NEWID(),
                                                         @siteid
                                                        )SELECT * FROM ScreenSetup WHERE ScreenId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertScreenSetupQuery, GetSQLParameters(screenSetupDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScreenSetupDTO(screenSetupDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ScreenSetupDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(screenSetupDTO);
            return screenSetupDTO;
        }

        /// <summary>
        /// Updates the Screen Setup record
        /// </summary>
        /// <param name="screenSetupDTO">ScreenSetupDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">SQL Transactions </param>
        /// <returns>ScreenSetupDTO</returns>
        public ScreenSetupDTO UpdateScreenSetup(ScreenSetupDTO screenSetupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(screenSetupDTO, loginId, siteId, sqlTransaction);
            string updateScreenSetupQuery = @"update ScreenSetup 
                                         set Name = @name,
                                             Alignment = @alignment,
                                             ScrDivHorizontal = @scrDivHorizontal,
                                             ScrDivVertical = @scrDivVertical,
                                             Description = @description,
                                             Active_Flag = @active_Flag,
                                             last_updated_user = @updatedby, 
                                             last_updated_date = Getdate(),
                                             --site_id = @siteid,
                                             MasterEntityId = @masterEntityId                                             
                                       where ScreenId = @screenId
                                       SELECT* FROM ScreenSetup WHERE ScreenId = @screenId";
            try
            {
                if (string.Equals(screenSetupDTO.IsActive, "N") && GetScreenReferenceCount(screenSetupDTO.ScreenId) > 0)
                {
                    throw new ForeignKeyException("Cannot Inactivate records for which matching detail data exists.");
                }
                DataTable dt = dataAccessHandler.executeSelectQuery(updateScreenSetupQuery, GetSQLParameters(screenSetupDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScreenSetupDTO(screenSetupDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating screenSetupDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(screenSetupDTO);
            return screenSetupDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="screenSetupDTO">screenSetupDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshScreenSetupDTO(ScreenSetupDTO screenSetupDTO, DataTable dt)
        {
            log.LogMethodEntry(screenSetupDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                screenSetupDTO.ScreenId = Convert.ToInt32(dt.Rows[0]["ScreenId"]);
                screenSetupDTO.LastUpdateDate = dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]);
                screenSetupDTO.CreationDate = dataRow["Creationdate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["Creationdate"]);
                screenSetupDTO.Guid = dataRow["guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["guid"]);
                screenSetupDTO.LastUpdatedBy = dataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_updated_user"]);
                screenSetupDTO.CreatedBy = dataRow["CreatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedUser"]);
                screenSetupDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Checks whether screen is in use.
        /// <param name="id">Screen Id</param>
        /// </summary>
        /// <returns>Returns refrenceCount</returns>
        private int GetScreenReferenceCount(int id)
        {
            log.LogMethodEntry(id);
            int refrenceCount = 0;
            string query = @"SELECT 
                            (SELECT COUNT(*) FROM Theme WHERE InitialScreenId = @ScreenId AND IsActive = 'Y') + 
                            (SELECT COUNT(*) FROM ScreenTransitions WHERE (FromScreenId = @ScreenId OR ToScreenId = @ScreenId) AND IsActive = 'Y') AS ReferenceCount";
            SqlParameter parameter = new SqlParameter("@ScreenId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter });
            if (dataTable.Rows.Count > 0)
            {
                refrenceCount = Convert.ToInt32(dataTable.Rows[0]["ReferenceCount"]);
            }
            log.LogMethodExit(refrenceCount);
            return refrenceCount;
        }

        /// <summary>
        /// Converts the Data row object to ScreenSetupDTO class type
        /// </summary>
        /// <param name="screenSetupDataRow">ScreenSetup DataRow</param>
        /// <returns>Returns ScreenSetup</returns>
        private ScreenSetupDTO GetScreenSetupDTO(DataRow screenSetupDataRow)
        {
            log.LogMethodEntry(screenSetupDataRow);
            ScreenSetupDTO screenSetupDataObject = new ScreenSetupDTO(Convert.ToInt32(screenSetupDataRow["ScreenId"]),
                                            screenSetupDataRow["Name"].ToString(),
                                            screenSetupDataRow["Alignment"] == DBNull.Value ? -1 : Convert.ToInt32(screenSetupDataRow["Alignment"]),
                                            screenSetupDataRow["ScrDivHorizontal"] == DBNull.Value ? -1 : Convert.ToInt32(screenSetupDataRow["ScrDivHorizontal"]),
                                            screenSetupDataRow["ScrDivVertical"] == DBNull.Value ? -1 : Convert.ToInt32(screenSetupDataRow["ScrDivVertical"]),
                                            screenSetupDataRow["Description"].ToString(),
                                             screenSetupDataRow["Active_Flag"] == DBNull.Value ? true : (screenSetupDataRow["Active_Flag"].ToString() == "Y"? true: false), 
                                            screenSetupDataRow["CreatedUser"].ToString(),
                                            screenSetupDataRow["Creationdate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(screenSetupDataRow["CreationDate"]),
                                            screenSetupDataRow["last_updated_user"].ToString(),
                                            screenSetupDataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(screenSetupDataRow["last_updated_date"]),
                                            screenSetupDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(screenSetupDataRow["site_id"]),
                                            screenSetupDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(screenSetupDataRow["MasterEntityId"]),
                                            screenSetupDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(screenSetupDataRow["SynchStatus"]),
                                            screenSetupDataRow["Guid"].ToString()
                                            );
            log.LogMethodExit(screenSetupDataObject);
            return screenSetupDataObject;
        }

        /// <summary>
        /// Gets the Screen Setup data of passed asset asset Group Id
        /// </summary>
        /// <param name="screenSetupId">integer type parameter</param>
        /// <returns>Returns ScreenSetupDTO</returns>
        public ScreenSetupDTO GetScreenSetup(int screenSetupId)
        {
            log.LogMethodEntry(screenSetupId);
            string selectScreenSetupQuery = SELECT_QUERY + @" WHERE ss.ScreenId = @screenId";
            SqlParameter[] selectScreenSetupParameters = new SqlParameter[1];
            selectScreenSetupParameters[0] = new SqlParameter("@screenId", screenSetupId);
            DataTable screenSetup = dataAccessHandler.executeSelectQuery(selectScreenSetupQuery, selectScreenSetupParameters, sqlTransaction);
            if (screenSetup.Rows.Count > 0)
            {
                DataRow ScreenSetupRow = screenSetup.Rows[0];
                ScreenSetupDTO screenSetupDataObject = GetScreenSetupDTO(ScreenSetupRow);
                log.LogMethodExit(screenSetupDataObject);
                return screenSetupDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the ScreenSetupDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ScreenSetupDTO matching the search criteria</returns>
        public List<ScreenSetupDTO> GetScreenSetupList(List<KeyValuePair<ScreenSetupDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<ScreenSetupDTO> screenSetupList = null;
            string selectScreenSetupQuery = SELECT_QUERY;
            if (searchParameters != null)
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ScreenSetupDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key == ScreenSetupDTO.SearchByParameters.SCREEN_ID
                            || searchParameter.Key == ScreenSetupDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScreenSetupDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScreenSetupDTO.SearchByParameters.IS_ACTIVE)
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
                if (searchParameters.Count > 0)
                    selectScreenSetupQuery = selectScreenSetupQuery + query;
            }
            DataTable screenSetupData = dataAccessHandler.executeSelectQuery(selectScreenSetupQuery, parameters.ToArray(), sqlTransaction);
            if (screenSetupData.Rows.Count > 0)
            {
                screenSetupList = new List<ScreenSetupDTO>();
                foreach (DataRow screenSetupDataRow in screenSetupData.Rows)
                {
                    ScreenSetupDTO screenSetupDataObject = GetScreenSetupDTO(screenSetupDataRow);
                    screenSetupList.Add(screenSetupDataObject);
                }
               
            }
            log.LogMethodExit(screenSetupList);
            return screenSetupList;
        }
    }
     
}
