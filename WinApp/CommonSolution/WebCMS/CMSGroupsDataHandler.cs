
/********************************************************************************************
 * Project Name - CMSGroupsData Data Handler 
 * Description  - Data handler of the CMSGroupsData DataHandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        23-Sept-2016    Rakshith          Created 
 *2.70.2        11-Dec-2019     Jinto Thomas      Removed siteid from update query
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
    /// <summary>
    /// This is the  CMSGroupsDataHandler data object class. This acts as data holder for the CMSGroups DataHandler business object
    /// </summary>
    public class CMSGroupsDataHandler
    {
        private  DataAccessHandler dataAccessHandler;
        private static readonly  Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SELECT_QUERY = "SELECT *  FROM CMSGroups  AS cg";
        private SqlTransaction sqlTransaction = null;
        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<CMSGroupsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CMSGroupsDTO.SearchByParameters, string>
        {
                {CMSGroupsDTO.SearchByParameters.GROUP_ID, "cg.GroupId"},
                {CMSGroupsDTO.SearchByParameters.MASTER_ENTITY_ID, "cg.MasterEntityId"},
                {CMSGroupsDTO.SearchByParameters.NAME, "cg.Name"},
                {CMSGroupsDTO.SearchByParameters.PARENT_GROUP_ID, "cg.ParentGroupId"}, 
                {CMSGroupsDTO.SearchByParameters.ACTIVE, "cg.Active"} ,
                 {CMSGroupsDTO.SearchByParameters.SITE_ID, "cg.site_id"} 
        };

        /// <summary>
        /// Default constructor of  CMS Groups DataHandlerr class
        /// </summary>
        public CMSGroupsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CMSGroups Record.
        /// </summary>
        /// <param name="CMSGroupsDTO">CMSGroupsDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CMSGroupsDTO cmsGroupsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsGroupsDTO, loginId, siteId);

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@groupId", cmsGroupsDTO.GroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@parentGroupId", cmsGroupsDTO.ParentGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@name", string.IsNullOrEmpty(cmsGroupsDTO.Name) ? DBNull.Value : (object)cmsGroupsDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@imageFileName", string.IsNullOrEmpty(cmsGroupsDTO.ImageFileName) ? DBNull.Value : (object)cmsGroupsDTO.ImageFileName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@groupUrl", string.IsNullOrEmpty(cmsGroupsDTO.GroupUrl) ? DBNull.Value : (object)cmsGroupsDTO.GroupUrl));
            parameters.Add(dataAccessHandler.GetSQLParameter("@active", cmsGroupsDTO.Active));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", cmsGroupsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            return parameters;
        }

        /// <summary>
        /// Inserts the CMS Groups  DataHandler  Items   record to the database
        /// </summary>
        /// <param name="cmsGroupsDTO"> CMSGroupsDTO  type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CMSGroupsDTO</returns>
        public CMSGroupsDTO InsertGroup(CMSGroupsDTO cmsGroupsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsGroupsDTO, loginId, siteId);
              string insertGroupQuery = @"insert into CMSGroups 
                                                    (   
                                                        ParentGroupId, 
                                                        Name, 
                                                        ImageFileName, 
                                                        GroupUrl,
                                                        Active, 
                                                        Guid,
                                                        site_id, 
                                                        CreatedBy, 
                                                        CreationDate, 
                                                        LastUpdatedBy, 
                                                        LastupdatedDate,
                                                        MasterEntityId
                                                    ) 
                                                    values 
                                                    (
                                                        @parentGroupId, 
                                                        @name, 
                                                        @imageFileName,
                                                        @groupUrl, 
                                                        @active, 
                                                        NEWID(),
                                                        @site_id, 
                                                        @createdBy, 
                                                        GetDate() , 
                                                        @lastUpdatedBy, 
                                                        GetDate()  ,
                                                        @masterEntityId
                                                    ) SELECT* FROM CMSGroups WHERE GroupId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertGroupQuery, GetSQLParameters(cmsGroupsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCMSGroupsDTO(cmsGroupsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting cmsGroupsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cmsGroupsDTO);
            return cmsGroupsDTO;
        }


        /// <summary>
        /// Update the CMS Groups  DataHandler  Items   record to the database
        /// </summary>
        /// <param name="cmsGroupsDTO"> CMSGroupsDTO  type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CMSGroupsDTO</returns>
        public CMSGroupsDTO UpdateGroup(CMSGroupsDTO cmsGroupsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsGroupsDTO, loginId, siteId);
            string updateGroupQuery = @"update  CMSGroups 
                                                    set   
                                                        ParentGroupId=@parentGroupId,  
                                                        Name=@name,  
                                                        ImageFileName=@imageFileName,  
                                                        GroupUrl=@groupUrl,  
                                                        Active= @active,   
                                                        -- site_id=@site_id, 
                                                        LastUpdatedBy=@lastUpdatedBy, 
                                                        LastupdatedDate= GetDate()  ,
                                                        MasterEntityId= @masterEntityId
                                                        where GroupId = @groupId SELECT* FROM CMSGroups WHERE GroupId = scope_identity()";
                try
                {
                    DataTable dt = dataAccessHandler.executeSelectQuery(updateGroupQuery, GetSQLParameters(cmsGroupsDTO, loginId, siteId).ToArray(), sqlTransaction);
                    RefreshCMSGroupsDTO(cmsGroupsDTO, dt);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while Updating cmsGroupsDTO", ex);
                    log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                    throw;
                }
                log.LogMethodExit(cmsGroupsDTO);
                return cmsGroupsDTO;
            }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="CMSGroupsDTO">CMSGroupsDTO </param>
        private void RefreshCMSGroupsDTO(CMSGroupsDTO cmsGroupsDTO, DataTable dt)
        {
            log.LogMethodEntry(cmsGroupsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                cmsGroupsDTO.GroupId = Convert.ToInt32(dt.Rows[0]["GroupId"]);
                cmsGroupsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                cmsGroupsDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Delete the record from the database based on  groupId
        /// </summary>
        /// <returns>return the int </returns>
        public int GroupDelete(int groupId)
        {
                log.LogMethodEntry(groupId);      
                string groupQuery = @"delete from CMSGroups
                                         where GroupId = @groupId";

                SqlParameter[] groupParameters = new SqlParameter[1];
                groupParameters[0] = new SqlParameter("@groupId", groupId);
                int deleteStatus = dataAccessHandler.executeUpdateQuery(groupQuery, groupParameters,sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
        }

        /// <summary>
        ///  GetGroupsDTO method
        /// </summary>
        /// <param name="groupRow">data row groupRow</param>
        /// <returns>returns CMSGroupsDTO object</returns>
        private CMSGroupsDTO GetGroupsDTO(DataRow groupRow)
        {
            CMSGroupsDTO cmsGroupsDTO = new CMSGroupsDTO(
                                                   groupRow["GroupId"] == DBNull.Value ? -1 : Convert.ToInt32(groupRow["GroupId"]),
                                                   groupRow["ParentGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(groupRow["ParentGroupId"]),
                                                   groupRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(groupRow["Name"]),
                                                   groupRow["ImageFileName"] == DBNull.Value ? string.Empty : Convert.ToString(groupRow["ImageFileName"]),
                                                   groupRow["GroupUrl"] == DBNull.Value ? string.Empty : Convert.ToString(groupRow["GroupUrl"]),
                                                   groupRow["Active"] == DBNull.Value ? false : Convert.ToBoolean(groupRow["Active"]),
                                                   groupRow["Guid"]== DBNull.Value ? string.Empty : Convert.ToString(groupRow["Guid"]),
                                                   groupRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(groupRow["SynchStatus"]),
                                                   groupRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(groupRow["site_id"]),
                                                   groupRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(groupRow["CreatedBy"]),
                                                   groupRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(groupRow["CreationDate"]),
                                                   groupRow["LastUpdatedBy"]== DBNull.Value ? string.Empty : Convert.ToString(groupRow["LastUpdatedBy"]),
                                                   groupRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(groupRow["LastupdatedDate"]),
                                                   groupRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(groupRow["MasterEntityId"])
                                                   );
            return cmsGroupsDTO;
        }

        /// <summary>
        /// return the record from the database based on  groupId
        /// </summary>
        /// <returns>return the CMSGroupsDTO object</returns>

        public CMSGroupsDTO GetGroup(int groupId)
        {
            log.LogMethodEntry(groupId);
                string groupQuery = SELECT_QUERY+ " where  cg.GroupId = @groupId";
                SqlParameter[] groupParameters = new SqlParameter[1];
                groupParameters[0] = new SqlParameter("@groupId", groupId);
                DataTable dtGroups = dataAccessHandler.executeSelectQuery(groupQuery, groupParameters,sqlTransaction);
                CMSGroupsDTO cmsGroupsDTO = new CMSGroupsDTO();
            if (dtGroups.Rows.Count > 0)
            {
                DataRow groupRow = dtGroups.Rows[0];
                cmsGroupsDTO = GetGroupsDTO(groupRow);
            }
            log.LogMethodExit(cmsGroupsDTO);
            return cmsGroupsDTO;
        }

        /// <summary>
        /// Gets the GetGroupsList matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic CMSGroupsDTO matching the search criteria</returns>
        public List<CMSGroupsDTO> GetGroupsList(List<KeyValuePair<CMSGroupsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CMSGroupsDTO> cmsGroupsList = new List<CMSGroupsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
          
                string selectGroupsQuery = SELECT_QUERY;

                if ((searchParameters != null) && (searchParameters.Count > 0))
                {
                    StringBuilder query = new StringBuilder(" where ");
                    foreach (KeyValuePair<CMSGroupsDTO.SearchByParameters, string> searchParameter in searchParameters)
                    {
                        if (DBSearchParameters.ContainsKey(searchParameter.Key))
                        {
                            string joiner = (count == 0) ? string.Empty : " and ";

                            if (searchParameter.Key.Equals(CMSGroupsDTO.SearchByParameters.NAME))
                               
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
                            else if (searchParameter.Key.Equals(CMSGroupsDTO.SearchByParameters.GROUP_ID) ||
                                    searchParameter.Key.Equals(CMSGroupsDTO.SearchByParameters.PARENT_GROUP_ID)||
                                    searchParameter.Key.Equals(CMSGroupsDTO.SearchByParameters.MASTER_ENTITY_ID))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            if (searchParameter.Key.Equals(CMSGroupsDTO.SearchByParameters.ACTIVE))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] +" = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                            }
                            else if (searchParameter.Key.Equals(CMSGroupsDTO.SearchByParameters.SITE_ID))
                            {
                                query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            count++;
                        }
                        else
                        {
                            log.LogMethodExit(null, "throwing exception");
                            log.LogVariableState("searchParameter.Key", searchParameter.Key);
                            throw new Exception("The query parameter does not exist " + searchParameter.Key);
                        }
                    }

                    selectGroupsQuery = selectGroupsQuery + query ;
                }
                DataTable dtGroups = dataAccessHandler.executeSelectQuery(selectGroupsQuery, parameters.ToArray(),sqlTransaction);
              
                if (dtGroups.Rows.Count > 0)
                {

                    foreach (DataRow groupRow in dtGroups.Rows)
                    {
                        CMSGroupsDTO cmsGroupsDTO = GetGroupsDTO(groupRow);
                        cmsGroupsList.Add(cmsGroupsDTO);
                    }
                }
            log.LogMethodExit(cmsGroupsList);
            return cmsGroupsList;

        }


      

        /// <summary>
        /// Gets the GroupsListTree matching the search key
        /// </summary>
        /// <param name="groupId">groupId as parameters</param>
        /// <param name="showActive">showActive as parameters </param>
        /// <returns>Returns the list of Generic CMSGroupsDTOTree matching the search criteria</returns>
        public List<CMSGroupsDTOTree> GetGroupsListTree(int groupId, bool showActive)
        {
            log.LogMethodEntry(groupId, showActive);
            List<CMSGroupsDTOTree> cmsGroupsDTOListTree = new List<CMSGroupsDTOTree>();
            try
            {
                int count = 0;

                List<KeyValuePair<CMSGroupsDTO.SearchByParameters, string>> parentSearchParameters = new List<KeyValuePair<CMSGroupsDTO.SearchByParameters, string>>();

                if (groupId!=-1)
                {
                    parentSearchParameters.Add(new KeyValuePair<CMSGroupsDTO.SearchByParameters, string>(CMSGroupsDTO.SearchByParameters.GROUP_ID, groupId.ToString()));
                }
             
                if (showActive)
                {
                    parentSearchParameters.Add(new KeyValuePair<CMSGroupsDTO.SearchByParameters, string>(CMSGroupsDTO.SearchByParameters.ACTIVE, "1"));
                }
                List<CMSGroupsDTO> cmsGroupsDTOList = GetGroupsList(parentSearchParameters);

                if (cmsGroupsDTOList.Count > 0)
                {
                    foreach (CMSGroupsDTO cmsGroupsDTO in cmsGroupsDTOList)
                    {
                        List<KeyValuePair<CMSGroupsDTO.SearchByParameters, string>> childSearchParameters = new List<KeyValuePair<CMSGroupsDTO.SearchByParameters, string>>();
                        childSearchParameters.Add(new KeyValuePair<CMSGroupsDTO.SearchByParameters, string>(CMSGroupsDTO.SearchByParameters.PARENT_GROUP_ID, cmsGroupsDTO.GroupId.ToString()));
                        if (showActive)
                        {
                            childSearchParameters.Add(new KeyValuePair<CMSGroupsDTO.SearchByParameters, string>(CMSGroupsDTO.SearchByParameters.ACTIVE, "1"));
                        }
                        List<CMSGroupsDTO> childCmsGroupsDTOList = GetGroupsList(childSearchParameters);
                        if (childCmsGroupsDTOList != null)
                        {
                            count = childCmsGroupsDTOList.Count;
                        }
                        else
                        {
                            count = 0;
                        }
                        cmsGroupsDTOListTree.Add(new CMSGroupsDTOTree(cmsGroupsDTO, childCmsGroupsDTOList, count));
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogMethodExit(null, "throwing exception At  GetGroupsListTree(int groupId, bool showActive)" + ex.Message);
                log.LogVariableState("CMSGroupsDTOListTree", cmsGroupsDTOListTree);
                throw ;
            }
            log.LogMethodExit(cmsGroupsDTOListTree);
            return cmsGroupsDTOListTree;
        }
    }
}
