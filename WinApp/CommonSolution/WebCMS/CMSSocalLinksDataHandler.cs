/********************************************************************************************
 * Project Name - CMSSocalLinks Data Handler Programs  
 * linkName  -    Data handler of the CMSSocalLinksDataHandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Apr-2016   Rakshith          Created
 *2.70.2        11-Dec-2019   Jinto Thomas      Removed siteid from update query
 *2.70.3      31-Mar-2020    Jeevan            Removed syncstatus from update query  
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.WebCMS
{
    public class CMSSocalLinksDataHandler
    {
        private DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SELECT_QUERY = "SELECT *  FROM CMSSocialLinks  AS csl";
        private SqlTransaction sqlTransaction = null;

        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<CMSSocialLinksDTO.SearchByRequestParameters, string> DBSearchParameters = new Dictionary<CMSSocialLinksDTO.SearchByRequestParameters, string>
        {
                {CMSSocialLinksDTO.SearchByRequestParameters.SOCIAL_LINK_ID, "csl.SocialLinkId"},
                {CMSSocialLinksDTO.SearchByRequestParameters.MASTER_ENTITY_ID, "csl.MasterEntityId"},
                {CMSSocialLinksDTO.SearchByRequestParameters.ACTIVE, "csl.Active"},
        };

        /// <summary>
        /// Default constructor of CMSSocalLinks Data Handler class
        /// </summary>
        public CMSSocalLinksDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }



        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CMSSocialLinks Record.
        /// </summary>
        /// <param name="CMSPagesDTO">CMSSocialLinksDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(CMSSocialLinksDTO cmsSocialLinksDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsSocialLinksDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@socialLinkId", cmsSocialLinksDTO.SocialLinkId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@linkName", string.IsNullOrEmpty(cmsSocialLinksDTO.LinkName) ? DBNull.Value : (object)cmsSocialLinksDTO.LinkName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@imagePath", string.IsNullOrEmpty(cmsSocialLinksDTO.ImagePath) ? DBNull.Value : (object)cmsSocialLinksDTO.ImagePath));
            parameters.Add(dataAccessHandler.GetSQLParameter("@url", string.IsNullOrEmpty(cmsSocialLinksDTO.Url) ? DBNull.Value : (object)cmsSocialLinksDTO.Url));
            parameters.Add(dataAccessHandler.GetSQLParameter("@displayOrder", cmsSocialLinksDTO.DisplayOrder == -1 ? DBNull.Value : (object)cmsSocialLinksDTO.DisplayOrder));
            parameters.Add(dataAccessHandler.GetSQLParameter("@active", cmsSocialLinksDTO.Active));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", cmsSocialLinksDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }



        /// <summary>
        /// Inserts the CMSSocialLinksDTO Items   record to the database
        /// </summary>
        /// <param name="cmsBannerDTO">CMSSocialLinksDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CMSSocialLinksDTO</returns>
        public CMSSocialLinksDTO InsertSocialLinks(CMSSocialLinksDTO cmsSocialLinksDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsSocialLinksDTO, loginId, siteId);
            string insertSocialLinksQuery = @"insert into CMSSocialLinks
                                                        (  
                                                          LinkName,
                                                          Url,
                                                          Active,
                                                          DisplayOrder,
                                                          Guid,
                                                          site_id,
                                                          CreatedBy,
                                                          CreationDate,
                                                          LastUpdatedBy,
                                                          LastupdatedDate,
                                                          ImagePath,
                                                          MasterEntityId
                                                        ) 
                                                values 
                                                        (
                                                          @linkName,
                                                          @url,
                                                          @active,
                                                          @displayOrder,
                                                          NEWID(),
                                                          @site_id ,
                                                          @createdBy,
                                                          Getdate(), 
                                                          @lastUpdatedBy,
                                                          GetDate(),
                                                          @imagePath,
                                                          @masterEntityId
                                                         ) SELECT * FROM CMSSocialLinks WHERE SocialLinkId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertSocialLinksQuery, GetSQLParameters(cmsSocialLinksDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCMSSocialLinksDTO(cmsSocialLinksDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting cmsSocialLinksDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cmsSocialLinksDTO);
            return cmsSocialLinksDTO;


        }

        /// <summary>
        /// Updates the CMSSocialLinksDTO Items   record to the database
        /// </summary>
        /// <param name="cmsBannerDTO">CMSSocialLinksDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns updated record id</returns>
        public CMSSocialLinksDTO UpdateSocialLinks(CMSSocialLinksDTO cmsSocialLinksDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsSocialLinksDTO, loginId, siteId);
            string updateSocialLinksQuery = @"update CMSSocialLinks 
                                                          set 
                                                          LinkName= @linkName,
                                                          Url=@Url,
                                                          Active=@active,
                                                          DisplayOrder=@displayOrder, 
                                                          -- Site_id= @site_id ,
                                                          LastUpdatedBy=@lastUpdatedBy,
                                                          LastupdatedDate=GetDate(),
                                                          ImagePath=@imagePath,
                                                          MasterEntityId = @masterEntityId 
                                                          where SocialLinkId=@socialLinkId
                                   SELECT * FROM CMSSocialLinks WHERE SocialLinkId = @socialLinkId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateSocialLinksQuery, GetSQLParameters(cmsSocialLinksDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCMSSocialLinksDTO(cmsSocialLinksDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating cmsSocialLinksDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cmsSocialLinksDTO);
            return cmsSocialLinksDTO;

        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="cmsSocialLinksDTO">CMSSocialLinksDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCMSSocialLinksDTO(CMSSocialLinksDTO cmsSocialLinksDTO, DataTable dt)
        {
            log.LogMethodEntry(cmsSocialLinksDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                cmsSocialLinksDTO.SocialLinkId = Convert.ToInt32(dt.Rows[0]["SocialLinkId"]);
                cmsSocialLinksDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                cmsSocialLinksDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                cmsSocialLinksDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                cmsSocialLinksDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                cmsSocialLinksDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                cmsSocialLinksDTO.LastupdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// return the record from the database
        /// Convert the datarow to CMSBannerItemsDTO object
        /// </summary>
        /// <returns>return the CMSBannerItemsDTO object</returns>
        private CMSSocialLinksDTO GetSocailLinksDTO(DataRow cmsSocailLinksRow)
        {
            log.LogMethodEntry(cmsSocailLinksRow);
            CMSSocialLinksDTO cmsSocialLinksDTO = new CMSSocialLinksDTO(
                                                     cmsSocailLinksRow["SocialLinkId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsSocailLinksRow["socialLinkId"]),
                                                     cmsSocailLinksRow["LinkName"] == DBNull.Value ? string.Empty : Convert.ToString(cmsSocailLinksRow["LinkName"]),
                                                     cmsSocailLinksRow["Url"] == DBNull.Value ? string.Empty : Convert.ToString(cmsSocailLinksRow["Url"]),
                                                     cmsSocailLinksRow["Active"] == DBNull.Value ? false : Convert.ToBoolean(cmsSocailLinksRow["active"]),
                                                     cmsSocailLinksRow["DisplayOrder"] == DBNull.Value ? -1 : Convert.ToInt32(cmsSocailLinksRow["displayOrder"]),
                                                     cmsSocailLinksRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(cmsSocailLinksRow["Guid"]),
                                                     cmsSocailLinksRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(cmsSocailLinksRow["synchStatus"]),
                                                     cmsSocailLinksRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(cmsSocailLinksRow["site_id"]),
                                                     cmsSocailLinksRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(cmsSocailLinksRow["CreatedBy"]),
                                                     cmsSocailLinksRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cmsSocailLinksRow["CreationDate"]),
                                                     cmsSocailLinksRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(cmsSocailLinksRow["LastUpdatedBy"]),
                                                     cmsSocailLinksRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cmsSocailLinksRow["LastupdatedDate"]),
                                                     cmsSocailLinksRow["ImagePath"] == DBNull.Value ? string.Empty : Convert.ToString(cmsSocailLinksRow["ImagePath"]),
                                                     cmsSocailLinksRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsSocailLinksRow["MasterEntityId"])
                                                    );
            log.LogMethodExit(cmsSocialLinksDTO);
            return cmsSocialLinksDTO;

        }


        /// <summary>
        /// return the record from the database based on  socialLinkId
        /// </summary>
        /// <returns>return the CMSSocialLinksDTO object</returns>
        /// or null
        public CMSSocialLinksDTO GetcmsSocialLinks(int socialLinkId)
        {

            string CmsSocialLinksRequestQuery = SELECT_QUERY + "   where csl.SocialLinkId = @socialLinkId";
            SqlParameter[] CmsSocialLinksParameters = new SqlParameter[1];
            CMSSocialLinksDTO cmsSocialLinksDTO = new CMSSocialLinksDTO();
            CmsSocialLinksParameters[0] = new SqlParameter("@socialLinkId", socialLinkId);
            DataTable CmsSocialLinksRequests = dataAccessHandler.executeSelectQuery(CmsSocialLinksRequestQuery, CmsSocialLinksParameters, sqlTransaction);
            if (CmsSocialLinksRequests.Rows.Count > 0)
            {
                DataRow CmsSocialLinksRequestRow = CmsSocialLinksRequests.Rows[0];
                cmsSocialLinksDTO = GetSocailLinksDTO(CmsSocialLinksRequestRow);

            }
            log.LogMethodEntry(cmsSocialLinksDTO);
            return cmsSocialLinksDTO;

        }

        /// <summary>
        /// Gets the GetSocialLinksList list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic CMSSocialLinksDTO   matching the search criteria</returns>
        public List<CMSSocialLinksDTO> GetSocialLinksList(List<KeyValuePair<CMSSocialLinksDTO.SearchByRequestParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CMSSocialLinksDTO> SocialLinksDataList = new List<CMSSocialLinksDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;

            string selectSocialLinksQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<CMSSocialLinksDTO.SearchByRequestParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (count == 0)
                        {
                            query.Append("Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            
                        }

                        else
                        {
                            query.Append(" and Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                selectSocialLinksQuery = selectSocialLinksQuery + query;
            }
            DataTable SocialLinksData = dataAccessHandler.executeSelectQuery(selectSocialLinksQuery, parameters.ToArray(),sqlTransaction);

            if (SocialLinksData.Rows.Count > 0)
            {
                foreach (DataRow SocialLinksDataRow in SocialLinksData.Rows)
                {
                    CMSSocialLinksDTO SocialLinksObject = GetSocailLinksDTO(SocialLinksDataRow);
                    SocialLinksDataList.Add(SocialLinksObject);
                }
            }
            log.LogMethodExit(SocialLinksDataList);
            return SocialLinksDataList;


        }

        /// <summary>
        /// Delete the CMSSocialLinksDTO based on Id
        /// </summary>
        public int cmsSocialLinkDelete(int socialLinkId)
        {
            log.LogMethodEntry(socialLinkId);
            try
            {
                string CmsSocialLinksQuery = @"delete from CMSSocialLinks
                                               where SocialLinkId = @socialLinkId";

                SqlParameter[] CmsSocialLinksParameters = new SqlParameter[1];
                CmsSocialLinksParameters[0] = new SqlParameter("@socialLinkId", socialLinkId);
                int deleteStatus = dataAccessHandler.executeUpdateQuery(CmsSocialLinksQuery, CmsSocialLinksParameters,sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred at Detete(socialLinkId) method ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }

    }
}
