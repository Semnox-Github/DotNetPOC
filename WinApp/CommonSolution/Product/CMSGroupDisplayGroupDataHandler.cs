
/********************************************************************************************
 * Project Name -  CMSGroupDisplayGroupDataHandler Data Handler
 * Description  - Data handler of the CMSGroupDisplayGroupDataHandler class
 * 
 **************
 **Version Log
 **************
 *Version       Date            Modified By     Remarks          
 *********************************************************************************************
 *1.00          23-sep-2016     Rakshith        Created 
 *2.3.0         25-Jun-2018     Guru S A        Modifications handle products exclusion at user role
 *                                              level 
 *2.70.2        10-Dec-2019   Jinto Thomas      Removed siteid from update query      
 *2.70.3        31-Mar-2020   Jeevan            Removed syncstatus from update query                                          
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Product
{
    /// <summary>
    /// CMS DisplayGroup DataHandler - Handles insert, update and select of  cmsDisplayGroup objects
    /// </summary>

   public class CMSGroupDisplayGroupDataHandler
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<CMSGroupDisplayGroupDTO.SearchByDisplayGroupsParameters, string> DBSearchParameters = new Dictionary<CMSGroupDisplayGroupDTO.SearchByDisplayGroupsParameters, string>
        {
              {CMSGroupDisplayGroupDTO.SearchByDisplayGroupsParameters.GROUP_DISPLAY_GROUP_ID , "GroupDisplayGroupId"},    
              {CMSGroupDisplayGroupDTO.SearchByDisplayGroupsParameters.GROUP_ID , "GroupId"}   ,
              {CMSGroupDisplayGroupDTO.SearchByDisplayGroupsParameters.PRODUCT_DISPLAY_GROUP_ID , "ProductDisplayGroupId"}   
        };

        DataAccessHandler dataAccessHandler;
        /// <summary>
        /// Default constructor of CMSDisplayGroupDataHandler class
        /// </summary>
        public CMSGroupDisplayGroupDataHandler()
        {
            log.Debug("Starts-cmsDisplayGroupDataHandler() default constructor.");
            dataAccessHandler = new DataAccessHandler();
            log.Debug("Ends-cmsDisplayGroupDataHandler() default constructor.");
        }

        /// <summary>
        /// Inserts the CmsDisplayGroup record to the database
        /// </summary>
        /// <param name="cmsDisplayGroup">CmsDisplayGroup type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertDisplayGroups(CMSGroupDisplayGroupDTO cmsDisplayGroup, string userId, int siteId)
        {
            log.Debug("Starts-InsertDisplayGroups(cmsDisplayGroup, userId, siteId) Method.");
            string insertDisplayQuery = @"insert into CMSGroupDisplayGroups 
                                                        (                                                 
                                                         GroupId,
                                                         ProductDisplayGroupId,
                                                         CreatedBy,
                                                         CreationDate,
                                                         LastUpdatedUser,
                                                         LastUpdatedDate,
                                                         site_id,
                                                         Guid 
                                                        ) 
                                                values 
                                                        (
                                                          @groupId,
                                                          @productDisplayGroupId,
                                                          @user,
                                                          GetDate(),
                                                          @user,
                                                          GetDate(),
                                                          @siteId,
                                                          Newid()                                                    
                                                         )SELECT CAST(scope_identity() AS int)";

             List<SqlParameter> insertDisplayGroupsParameters = new List<SqlParameter>();
             insertDisplayGroupsParameters.Add(new SqlParameter("@groupId", cmsDisplayGroup.GroupId));
             insertDisplayGroupsParameters.Add(new SqlParameter("@productDisplayGroupId", cmsDisplayGroup.ProductDisplayGroupId));
             insertDisplayGroupsParameters.Add(new SqlParameter("@user", userId));
             if (cmsDisplayGroup.SiteId == -1)
             {
                 insertDisplayGroupsParameters.Add(new SqlParameter("@siteId", DBNull.Value));
             }
             else
             {
                 insertDisplayGroupsParameters.Add(new SqlParameter("@siteId", siteId));
             }
              



             int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertDisplayQuery, insertDisplayGroupsParameters.ToArray());
             log.Debug("Ends-InsertDisplayGroups(cmsDisplayGroup, userId, siteId) Method.");
             return idOfRowInserted;
         }

         /// <summary>
         /// Updates the cmsDisplayGroup  record
         /// </summary>
         /// <param name="cmsDisplayGroup">CmsDisplayGroup type parameter</param>
         /// <param name="userId">User inserting the record</param>
         /// <param name="siteId">Site to which the record belongs</param>
         /// <returns>Returns the count of updated rows</returns>
         public int UpdateDisplayGroups(CMSGroupDisplayGroupDTO cmsDisplayGroup, string userId, int siteId)
         {
             log.Debug("Starts-UpdateDisplayGroups(cmsDisplayGroup, userId, siteId) Method.");
             string updateCmsDisplayQuery = @"update CMSGroupDisplayGroups
                                                   set   GroupId =@groupId,
                                                         ProductDisplayGroupId =@productDisplayGroupId,
                                                         LastUpdatedDate = GetDate(),
                                                         LastUpdatedUser =@lastUpdatedUser
                                                         -- site_id =@siteId,                                                                            
                                                         where  GroupDisplayGroupId =@groupDisplayGroupId";

             List<SqlParameter> updateproductDisplayGroupFormatParameters = new List<SqlParameter>();

             List<SqlParameter> updateDisplayGroupsParameters = new List<SqlParameter>();
             updateDisplayGroupsParameters.Add(new SqlParameter("@groupDisplayGroupId", cmsDisplayGroup.GroupDisplayGroupId));
             updateDisplayGroupsParameters.Add(new SqlParameter("@groupId", cmsDisplayGroup.GroupId));
             updateDisplayGroupsParameters.Add(new SqlParameter("@productDisplayGroupId", cmsDisplayGroup.ProductDisplayGroupId));
             updateDisplayGroupsParameters.Add(new SqlParameter("@lastUpdatedUser", userId));
             if (cmsDisplayGroup.SiteId == -1)
             {
                 updateDisplayGroupsParameters.Add(new SqlParameter("@siteId", DBNull.Value));
             }
             else
             {
                 updateDisplayGroupsParameters.Add(new SqlParameter("@siteId", siteId));
             } 
       
             if (cmsDisplayGroup.SynchStatus)
             {
                 updateDisplayGroupsParameters.Add(new SqlParameter("@synchStatus", cmsDisplayGroup.SynchStatus));
             }
             else
             {
                 updateDisplayGroupsParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
             }


             int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateCmsDisplayQuery, updateDisplayGroupsParameters.ToArray());
             log.Debug("Ends-UpdateDisplayGroups(cmsDisplayGroup, userId, siteId) Method.");
             return rowsUpdated;
         }

         public int Delete(int groupDisplayGroupId)
         {
             log.Debug(" Starts-Delete(int cmsGroupDisplayGroupId) Method.");
             try
             {
                 string displayGroupQuery = @"delete  
                                          from CMSGroupDisplayGroups
                                          where GroupDisplayGroupId = @groupDisplayGroupId";

                 SqlParameter[] displayGroupParameters = new SqlParameter[1];
                 displayGroupParameters[0] = new SqlParameter("@groupDisplayGroupId", groupDisplayGroupId);

                 int deleteStatus = dataAccessHandler.executeUpdateQuery(displayGroupQuery, displayGroupParameters);
                 log.Debug(" Ends-Delete(int cmsGroupDisplayGroupId) Method.");
                 return deleteStatus;
             }
             catch (Exception expn)
             {
                 throw new System.Exception(expn.Message.ToString());
             }
         }


         public int DeleteByGroupId(int groupId)
         {
             log.Debug(" Starts-DeleteByGroupId(int groupId) Method.");
             try
             {
                 string displayGroupQuery = @"delete  
                                          from CMSGroupDisplayGroups
                                          where GroupId = @groupId";

                 SqlParameter[] displayGroupParameters = new SqlParameter[1];
                 displayGroupParameters[0] = new SqlParameter("@groupId", groupId);

                 int deleteStatus = dataAccessHandler.executeUpdateQuery(displayGroupQuery, displayGroupParameters);
                 log.Debug(" Ends-DeleteByGroupId(int groupId) Method.");
                 return deleteStatus;
             }
             catch (Exception expn)
             {
                 throw new System.Exception(expn.Message.ToString());
             }
         }
         /// <summary>
         /// Converts the Data row object to CmsDisplayGroup class type
         /// </summary>
         /// <param name="displayGroupsDataRow">ProductDisplayGroup DataRow</param>
         /// <returns>Returns ProductDisplayGroupFormat</returns>
         private CMSGroupDisplayGroupDTO GetDisplayGroupsDTO(DataRow displayGroupsDataRow)
         {
             log.Debug("Starts-GetDisplayGroupsDTO(cMSGroupDisplayGroupDataRow) Method.");
             CMSGroupDisplayGroupDTO cMSGroupDisplayGroupDataObject = new CMSGroupDisplayGroupDTO(Convert.ToInt32(displayGroupsDataRow["GroupDisplayGroupId"]),
                                                     displayGroupsDataRow["GroupId"] == DBNull.Value ? -1 : Convert.ToInt32(displayGroupsDataRow["GroupId"]),
                                                     displayGroupsDataRow["ProductDisplayGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(displayGroupsDataRow["ProductDisplayGroupId"]),
                                                     displayGroupsDataRow["CreatedBy"].ToString(),
                                                     displayGroupsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(displayGroupsDataRow["CreationDate"]),
                                                     displayGroupsDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(displayGroupsDataRow["LastUpdatedDate"]),
                                                     displayGroupsDataRow["LastUpdatedUser"].ToString(),
                                                     displayGroupsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(displayGroupsDataRow["site_id"]),
                                                     displayGroupsDataRow["Guid"].ToString(),
                                                     displayGroupsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(displayGroupsDataRow["SynchStatus"])                               
                                                     );
             log.Debug("Ends-GetDisplayGroupsDTO(displayGroupsDataRow) Method.");
             return cMSGroupDisplayGroupDataObject;
         }

         /// <summary>
         /// Gets the GetDisplayGroups data of passed cmsGroupDisplayGroup Id
         /// </summary>
         /// <param name="cmsGroupDisplayGroupId">integer type parameter</param>
         /// <returns>Returns CmsDisplayGroup</returns>
         public CMSGroupDisplayGroupDTO GetDisplayGroups(int cmsGroupDisplayGroupId)
         {
             log.Debug("Starts-GetDisplayGroups(cmsGroupDisplayGroupId) Method.");
             string selectDisplayGroupsQuery = @"select *
                                         from CMSGroupDisplayGroups
                                        where GroupDisplayGroupId = @GroupDisplayGroupId";

             SqlParameter[] selectDisplayGroupsParameters = new SqlParameter[1];
             selectDisplayGroupsParameters[0] = new SqlParameter("@GroupDisplayGroupId", cmsGroupDisplayGroupId);
             DataTable dtCmsDisplayGroup = dataAccessHandler.executeSelectQuery(selectDisplayGroupsQuery, selectDisplayGroupsParameters);
             CMSGroupDisplayGroupDTO cmsDisplayGroup = new CMSGroupDisplayGroupDTO();
             if (dtCmsDisplayGroup.Rows.Count > 0)
             {
                 DataRow displayGroupsRow = dtCmsDisplayGroup.Rows[0];
                 cmsDisplayGroup = GetDisplayGroupsDTO(displayGroupsRow);
                 log.Debug("Ends-GetDisplayGroups(cmsGroupDisplayGroupId) Method by returnting cmsDisplayGroup.");
                
             }
             return cmsDisplayGroup;
         }

         /// <summary>
         /// Gets the CmsDisplayGroup list matching the search key
         /// </summary>
         /// <param name="searchParameters">List of search parameters</param>
         /// <returns>Returns the list of CmsDisplayGroup matching the search criteria</returns>
         public List<CMSGroupDisplayGroupDTO> GetDisplayGroupsList(List<KeyValuePair<CMSGroupDisplayGroupDTO.SearchByDisplayGroupsParameters, string>> searchParameters)
         {
             log.Debug("Starts-GetDisplayGroupsList(searchParameters) Method.");
             int count = 0;
             string selectCmsDisplayQuery = @"select *
                                         from CMSGroupDisplayGroups";
             if (searchParameters != null)
             {
                 StringBuilder query = new StringBuilder(" where ");
                 foreach (KeyValuePair<CMSGroupDisplayGroupDTO.SearchByDisplayGroupsParameters, string> searchParameter in searchParameters)
                 {
                     if (DBSearchParameters.ContainsKey(searchParameter.Key))
                     {
                         string joinOperator = (count == 0 ? " " : " and ");
                         
                             if (searchParameter.Key.Equals(CMSGroupDisplayGroupDTO.SearchByDisplayGroupsParameters.GROUP_DISPLAY_GROUP_ID) ||
                                 searchParameter.Key.Equals(CMSGroupDisplayGroupDTO.SearchByDisplayGroupsParameters.GROUP_ID)||
                                 searchParameter.Key.Equals(CMSGroupDisplayGroupDTO.SearchByDisplayGroupsParameters.PRODUCT_DISPLAY_GROUP_ID))
                             {
                                 query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                             }
                             else
                             {
                                 query.Append( joinOperator + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                             }                      
                         count++;
                     }
                     else
                     {
                         log.Debug("Ends-GetDisplayGroupsList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                         throw new Exception("The query parameter does not exist " + searchParameter.Key);
                     }
                 }
                 if (searchParameters.Count > 0)
                     selectCmsDisplayQuery = selectCmsDisplayQuery + query;
             }

             DataTable dtDisplayGroup = dataAccessHandler.executeSelectQuery(selectCmsDisplayQuery, null);
             List<CMSGroupDisplayGroupDTO> cmsDisplayGroupList = new List<CMSGroupDisplayGroupDTO>();

             if (dtDisplayGroup.Rows.Count > 0)
             {
                 foreach (DataRow displayGroupRow in dtDisplayGroup.Rows)
                 {
                     CMSGroupDisplayGroupDTO cmsDisplayGroup = GetDisplayGroupsDTO(displayGroupRow);
                     cmsDisplayGroupList.Add(cmsDisplayGroup);
                 }
                 log.Debug("Ends-GetDisplayGroupsList(searchParameters) Method by returning cmsDisplayGroupList.");
            
             }
             return cmsDisplayGroupList;
         }


    }

}
