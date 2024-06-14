/********************************************************************************************
 * Project Name - CMSModules Data handler Program
 * Description  - Data object of the CMSModules Data handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        11-Oct-2016   Rakshith           Created 
 *2.70.2      17-Oct-2019   Mushahid Faizan    Added "Description"  DBSearchParameters.
 *2.70.2      11-Dec-2019   Jinto Thomas       Removed siteid from update query
 *2.70.3      31-Mar-2020   Jeevan             Removed syncstatus from update query 
 *2.80        12-May-2020   Indrajeet Kumar    Modified - Removed MenuId
 *2.130.2     17-Feb-2022   Nitin Pai          CMS Changes for SmartFun
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;


namespace Semnox.Parafait.WebCMS
{
    public class CMSModulesDatahandler
    {
        private DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SELECT_QUERY = "SELECT *  FROM CMSModules  AS cmo";
        private SqlTransaction sqlTransaction = null;
        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<CMSModulesDTO.SearchByRequestParameters, string> DBSearchParameters = new Dictionary<CMSModulesDTO.SearchByRequestParameters, string>
            {
                {CMSModulesDTO.SearchByRequestParameters.MODULE_ID,"cmo.ModuleId"},
                {CMSModulesDTO.SearchByRequestParameters.ACTIVE, "cmo.Active"},
                {CMSModulesDTO.SearchByRequestParameters.MASTER_ENTITY_ID, "cmo.MasterEntityId"},
                {CMSModulesDTO.SearchByRequestParameters.DESCRIPTION, "cmo.Description"}
            };

        /// <summary>
        ///  constructor of  CMS CMSModulesDatahandler  class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CMSModulesDatahandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CMSModules Record.
        /// </summary>
        /// <param name="CMSModulesDTO">CMSModulesDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(CMSModulesDTO cmsModulesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsModulesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ModuleId", cmsModulesDTO.ModuleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MenuId", cmsModulesDTO.MenuId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Description", string.IsNullOrEmpty(cmsModulesDTO.Description) ? DBNull.Value : (object)cmsModulesDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Title", string.IsNullOrEmpty(cmsModulesDTO.Title) ? DBNull.Value : (object)cmsModulesDTO.Title));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ImageFileName", string.IsNullOrEmpty(cmsModulesDTO.ImageFileName) ? DBNull.Value : (object)cmsModulesDTO.ImageFileName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterPage", string.IsNullOrEmpty(cmsModulesDTO.MasterPage) ? DBNull.Value : (object)cmsModulesDTO.MasterPage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ModuleRoute", string.IsNullOrEmpty(cmsModulesDTO.ModuleRoute) ? DBNull.Value : (object)cmsModulesDTO.ModuleRoute));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Active", cmsModulesDTO.Active));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", cmsModulesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the CMSModules  Data Handler Items record to the database
        /// </summary>
        /// <param name="cmsModulesDTO">CMSModules Data Handler  type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CMSModulesDTO</returns>
        public CMSModulesDTO InsertModule(CMSModulesDTO cmsModulesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsModulesDTO, loginId, siteId);
            string insertModuleQuery = @"insert into CMSModules
                                                        (                                                 
                                                        Description,
                                                        Title,
                                                        ImageFileName, 
                                                        MasterPage,
                                                        ModuleRoute, 
                                                        MenuId,
                                                        Active,
                                                        Guid,
                                                        site_id,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastupdatedDate ,
                                                        MasterEntityId
                                                        ) 
                                                values 
                                                        (
                                                        @Description,
                                                        @Title,
                                                        @ImageFileName, 
                                                        @MasterPage,
                                                        @ModuleRoute,  
                                                        @MenuId,
                                                        @Active,
                                                        NEWID(),
                                                        @site_id,
                                                        @CreatedBy,
                                                        Getdate(), 
                                                        @LastUpdatedBy,
                                                        Getdate() ,
                                                        @masterEntityId
                                                )SELECT * FROM CMSModules WHERE ModuleId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertModuleQuery, GetSQLParameters(cmsModulesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCMSModulesDTO(cmsModulesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting cmsModulesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cmsModulesDTO);
            return cmsModulesDTO;
        }



        /// <summary>
        /// Updates the CMSModules  Data Handler Items record to the database
        /// </summary>
        /// <param name="cmsModulesDTO">CMSModules Data Handler  type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CMSModulesDTO</returns>
        public CMSModulesDTO UpdateModule(CMSModulesDTO cmsModulesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsModulesDTO, loginId, siteId);
            string updateModuleQuery = @"UPDATE CMSModules
                                            SET
                                                Description=@Description,
                                                Title=@Title,
                                                ImageFileName=@ImageFileName, 
                                                MasterPage=@MasterPage,
                                                ModuleRoute=@ModuleRoute,    
                                                MenuId=@MenuId,
                                                Active=@Active, 
                                                -- site_id=@site_id,
                                                LastUpdatedBy=@LastUpdatedBy,
                                                LastupdatedDate=Getdate()  
                                                where ModuleId = @ModuleId
                                                SELECT * FROM CMSModules WHERE ModuleId = @ModuleId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateModuleQuery, GetSQLParameters(cmsModulesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCMSModulesDTO(cmsModulesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating cmsModulesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cmsModulesDTO);
            return cmsModulesDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="cmsModulesDTO">CMSModulesDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCMSModulesDTO(CMSModulesDTO cmsModulesDTO, DataTable dt)
        {
            log.LogMethodEntry(cmsModulesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                cmsModulesDTO.ModuleId = Convert.ToInt32(dt.Rows[0]["ModuleId"]);
                cmsModulesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                cmsModulesDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                cmsModulesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                cmsModulesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                cmsModulesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                cmsModulesDTO.LastupdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// return the record from the database
        /// Convert the datarow to CMSModulesDTO object
        /// </summary>
        /// <returns>return the CMSModulesDTO object</returns>
        private CMSModulesDTO GetCMSModulesDTO(DataRow cmsModulesRow)
        {
            log.LogMethodEntry(cmsModulesRow);
            CMSModulesDTO cmsModulesDTO = new CMSModulesDTO(
                                                  cmsModulesRow["ModuleId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsModulesRow["ModuleId"]),
                                                  cmsModulesRow["Description"] == DBNull.Value ? string.Empty : Convert.ToString(cmsModulesRow["Description"]),
                                                  cmsModulesRow["Title"] == DBNull.Value ? string.Empty : Convert.ToString(cmsModulesRow["Title"]),
                                                  cmsModulesRow["ImageFileName"] == DBNull.Value ? string.Empty : Convert.ToString(cmsModulesRow["ImageFileName"]),
                                                  cmsModulesRow["MasterPage"] == DBNull.Value ? string.Empty : Convert.ToString(cmsModulesRow["MasterPage"]),
                                                  cmsModulesRow["ModuleRoute"] == DBNull.Value ? string.Empty : Convert.ToString(cmsModulesRow["ModuleRoute"]),
                                                  cmsModulesRow.Table.Columns.Contains("MenuId") ? cmsModulesRow["MenuId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsModulesRow["MenuId"]) : -1,
                                                  cmsModulesRow["Active"] == DBNull.Value ? false : Convert.ToBoolean(cmsModulesRow["Active"]),
                                                  cmsModulesRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(cmsModulesRow["Guid"]),
                                                  cmsModulesRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(cmsModulesRow["SynchStatus"]),
                                                  cmsModulesRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(cmsModulesRow["site_id"]),
                                                  cmsModulesRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(cmsModulesRow["CreatedBy"]),
                                                  cmsModulesRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cmsModulesRow["CreationDate"]),
                                                  cmsModulesRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(cmsModulesRow["LastUpdatedBy"]),
                                                  cmsModulesRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cmsModulesRow["LastupdatedDate"]),
                                                  cmsModulesRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsModulesRow["MasterEntityId"])

                                                  );
            log.LogMethodExit(cmsModulesDTO);
            return cmsModulesDTO;

        }

        /// <summary>
        /// return the record from the database based on  moduleId
        /// </summary>
        /// <returns>return the CMSModulesDTO object</returns>
        public CMSModulesDTO GetModule(int moduleId)
        {
            log.LogMethodEntry(moduleId);
            string CmsModuleQuery = SELECT_QUERY + "  where ModuleId = @ModuleId";
            SqlParameter[] CmsModuleParameters = new SqlParameter[1];
            CmsModuleParameters[0] = new SqlParameter("@ModuleId", moduleId);
            DataTable dtCmsModule = dataAccessHandler.executeSelectQuery(CmsModuleQuery, CmsModuleParameters, sqlTransaction);
            CMSModulesDTO cmsModulesDTO = new CMSModulesDTO();
            if (dtCmsModule.Rows.Count > 0)
            {
                DataRow CmsModuleRow = dtCmsModule.Rows[0];
                cmsModulesDTO = GetCMSModulesDTO(CmsModuleRow);
            }
            log.LogMethodExit(cmsModulesDTO);
            return cmsModulesDTO;

        }

        /// <summary>
        /// Delete the record from the database based on  moduleId
        /// </summary>
        /// <returns>return the int </returns>
        public int cmsModuleDelete(int moduleId)
        {
            log.LogMethodEntry(moduleId);
            try
            {
                string CmsModuleQuery = @"delete from CMSModules where ModuleId = @ModuleId";
                SqlParameter[] CmsModuleParameters = new SqlParameter[1];
                CmsModuleParameters[0] = new SqlParameter("@ModuleId", moduleId);
                int id = dataAccessHandler.executeUpdateQuery(CmsModuleQuery, CmsModuleParameters, sqlTransaction);
                log.LogMethodExit(id);
                return id;
            }
            catch (Exception expn)
            {
                log.Error("Error occurred while Deleting at method cmsModuleDelete(moduleId)", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the CMSModulesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CMSModulesDTO matching the search criteria</returns>
        public List<CMSModulesDTO> GetCmsModulesList(List<KeyValuePair<CMSModulesDTO.SearchByRequestParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CMSModulesDTO> modulesList = new List<CMSModulesDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectCmsModuleQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<CMSModulesDTO.SearchByRequestParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key.Equals(CMSModulesDTO.SearchByRequestParameters.ACTIVE))
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0) =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key.Equals(CMSModulesDTO.SearchByRequestParameters.MODULE_ID)
                                || searchParameter.Key.Equals(CMSModulesDTO.SearchByRequestParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
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
                selectCmsModuleQuery = selectCmsModuleQuery + query;
            }
            DataTable dtmoduleData = dataAccessHandler.executeSelectQuery(selectCmsModuleQuery, parameters.ToArray(), sqlTransaction);

            if (dtmoduleData.Rows.Count > 0)
            {

                foreach (DataRow PagesDataRow in dtmoduleData.Rows)
                {
                    CMSModulesDTO cMSModulesDTO = GetCMSModulesDTO(PagesDataRow);
                    modulesList.Add(cMSModulesDTO);
                }

            }
            log.LogMethodExit(modulesList);
            return modulesList;
        }
    }
}
