/********************************************************************************************
 * Project Name - WebCMS
 * Description  - Data Handler class for CMSThemes
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.80        22-Oct-2019   Mushahid Faizan    Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.WebCMS
{
    public class CMSThemesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CMSThemes ";

        /// <summary>
        /// Dictionary for searching Parameters for the CMSThemes object.
        /// </summary>
        private static readonly Dictionary<CMSThemesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CMSThemesDTO.SearchByParameters, string>
        {
            { CMSThemesDTO.SearchByParameters.THEME_ID,"ThemeId"},
            { CMSThemesDTO.SearchByParameters.MODULE_ID,"ModuleId"},
            { CMSThemesDTO.SearchByParameters.PAGE_ID,"PageId"},
            { CMSThemesDTO.SearchByParameters.SITE_ID,"SiteId"},
            { CMSThemesDTO.SearchByParameters.ACTIVE,"Active"}
        };

        /// <summary>
        /// Parameterized Constructor for CMSThemesDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction objects</param>
        public CMSThemesDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }


        ///    *******NOT TESTED ******** Uncomment this code when Insert/Update is required.
          
        ///// <summary>
        ///// Builds the SQL Parameter list used for inserting and updating CMSThemes Record.
        ///// </summary>
        ///// <param name="cMSThemesDTO">cMSThemesDTO object is passed as parameter.</param>
        ///// <param name="loginId">login id of user </param>
        ///// <param name="siteId">site id of user</param>
        ///// <returns>SQL parameters</returns>
        //private List<SqlParameter> GetSQLParameters(CMSThemesDTO cMSThemesDTO, string loginId, int siteId)
        //{
        //    log.LogMethodEntry(cMSThemesDTO, loginId, siteId);
        //    List<SqlParameter> parameters = new List<SqlParameter>();
        //    parameters.Add(dataAccessHandler.GetSQLParameter("@ThemeId", cMSThemesDTO.ThemeId, true));
        //    parameters.Add(dataAccessHandler.GetSQLParameter("@ThemeName", cMSThemesDTO.ThemeName));
        //    parameters.Add(dataAccessHandler.GetSQLParameter("@Key", cMSThemesDTO.Key));
        //    parameters.Add(dataAccessHandler.GetSQLParameter("@Value", cMSThemesDTO.Value));
        //    parameters.Add(dataAccessHandler.GetSQLParameter("@PageId", cMSThemesDTO.PageId, true));
        //    parameters.Add(dataAccessHandler.GetSQLParameter("@ModuleId", cMSThemesDTO.ModuleId));
        //    parameters.Add(dataAccessHandler.GetSQLParameter("@Active", cMSThemesDTO.Active));
        //    parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", cMSThemesDTO.MasterEntityId, true));
        //    parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
        //    parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
        //    parameters.Add(dataAccessHandler.GetSQLParameter("@Guid", cMSThemesDTO.Guid));
        //    log.LogMethodExit(parameters);
        //    return parameters;
        //}


        ///// <summary>
        ///// Used to update the current DTO with auto generated Database column values.
        ///// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        ///// </summary>
        ///// <param name="cmsThemesDTO">cmsThemesDTO</param>
        ///// <param name="dt">dt</param>
        //private void RefreshCMSThemesDTO(CMSThemesDTO cmsThemesDTO, DataTable dt)
        //{
        //    log.LogMethodEntry(cmsThemesDTO, dt);
        //    if (dt.Rows.Count > 0)
        //    {
        //        DataRow dataRow = dt.Rows[0];
        //        cmsThemesDTO.ThemeId = Convert.ToInt32(dt.Rows[0]["Datatype_Id"]);
        //        cmsThemesDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
        //        cmsThemesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
        //        cmsThemesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
        //        cmsThemesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
        //        cmsThemesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
        //        cmsThemesDTO.SiteId = dataRow["SiteId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SiteId"]);
        //    }
        //    log.LogMethodExit();
        //}
        ///// <summary>
        ///// Inserts the cmsThemesDTO record to the database
        ///// </summary>
        ///// <param name="cmsThemesDTO">cmsThemesDTO type object</param>
        ///// <param name="loginId">User inserting the record</param>
        ///// <param name="siteId">Site to which the record belongs</param>
        ///// <returns>Returns inserted record id</returns>
        //public CMSThemesDTO InsertCMSThemes(CMSThemesDTO cmsThemesDTO, string loginId, int siteId)
        //{
        //    log.LogMethodEntry(cmsThemesDTO, loginId, siteId);
        //    string query = @"INSERT INTO CMSThemes 
        //                                ( 
        //                                    ThemeName,
        //                                    Key,
        //                                    Value,
        //                                    PageId,
        //                                    ModuleId,
        //                                    Active,
        //                                    MasterEntityId
        //                                ) 
        //                        VALUES 
        //                                (
        //                                    @ThemeName,
        //                                    @Key,
        //                                    @Value,
        //                                    @PageId,
        //                                    @ModuleId,
        //                                    @Active,
        //                                    @MasterEntityId
        //                                )SELECT * FROM CMSThemes WHERE ThemeId = scope_identity()";
        //    try
        //    {
        //        DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(cmsThemesDTO, loginId, siteId).ToArray(), sqlTransaction);
        //        RefreshCMSThemesDTO(cmsThemesDTO, dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Error occurred while inserting cmsThemesDTO", ex);
        //        log.LogMethodExit(null, "Throwing exception - " + ex.Message);
        //        throw;
        //    }
        //    log.LogMethodExit(cmsThemesDTO);
        //    return cmsThemesDTO;
        //}


        /// <summary>
        /// Converts the Data row object to CMSThemesDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of CMSThemesDTO</returns>
        private CMSThemesDTO GetCMSThemesDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CMSThemesDTO cMSThemesDTO = new CMSThemesDTO(dataRow["ThemeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ThemeId"]),
                                                dataRow["ThemeName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ThemeName"]),
                                                dataRow["Key"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Key"]),
                                                dataRow["Value"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Value"]),
                                                dataRow["PageId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PageId"]),
                                                dataRow["ModuleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ModuleId"]),
                                                dataRow["Active"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["Active"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                dataRow["SiteId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SiteId"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]));
                                               
            return cMSThemesDTO;
        }

        /// <summary>
        /// Returns the List of CMSThemesDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of CMSThemesDTO</returns>
        public List<CMSThemesDTO> GetCMSThemeDTOList(List<KeyValuePair<CMSThemesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<CMSThemesDTO> cMSThemesDTOList = new List<CMSThemesDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CMSThemesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CMSThemesDTO.SearchByParameters.THEME_ID ||
                            searchParameter.Key == CMSThemesDTO.SearchByParameters.PAGE_ID ||
                            searchParameter.Key == CMSThemesDTO.SearchByParameters.MODULE_ID ||
                            searchParameter.Key == CMSThemesDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CMSThemesDTO.SearchByParameters.ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == CMSThemesDTO.SearchByParameters.SITE_ID)
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
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CMSThemesDTO cMSThemesDTO = GetCMSThemesDTO(dataRow);
                    cMSThemesDTOList.Add(cMSThemesDTO);
                }
            }
            log.LogMethodExit(cMSThemesDTOList);
            return cMSThemesDTOList;
        }
    }
}
