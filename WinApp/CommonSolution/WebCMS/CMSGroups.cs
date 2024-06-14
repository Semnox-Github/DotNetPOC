
/********************************************************************************************
 * Project Name - CMSGroups  BL Class  
 * Description  - Bussiness logic of the CMSGroups class
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By       Remarks          
 *********************************************************************************************
 *1.00        23-Sept-2016    Rakshith          Created 
 *2.70       09-Jul-2019    Girish Kundar       Modified : Save() method : Insert/update methods returns DTO instead of Id.
 *                                                        LogMethodEntry() and LogMethodExit(). 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.WebCMS
{
    public class CMSGroups
    {
        private CMSGroupsDTO cmsGroupsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Default constructor
        /// </summary>
        private CMSGroups(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        //Constructor Call Corresponding Data Hander based id
        //And return Correspond Object
        public CMSGroups(ExecutionContext executionContext, int groupId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext,groupId, sqlTransaction);
            CMSGroupsDataHandler cmsGroupsDataHandler = new CMSGroupsDataHandler(sqlTransaction);
            cmsGroupsDTO = cmsGroupsDataHandler.GetGroup(groupId);
            if (cmsGroupsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CMSGroups", groupId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        ///  //Constructor Initializes with Corresponding Object
        /// </summary>
        /// <param name="cmsGroupsDTO">cmsGroupsDTO</param>
        public CMSGroups(ExecutionContext executionContext, CMSGroupsDTO cmsGroupsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext,cmsGroupsDTO);
            this.cmsGroupsDTO = cmsGroupsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Used For Save 
        /// It may by Insert Or Update
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (cmsGroupsDTO.IsChanged == false
                  && cmsGroupsDTO.GroupId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return ;
            }
            CMSGroupsDataHandler cmsGroupsDataHandler = new CMSGroupsDataHandler(sqlTransaction);
            try
            {
                if (cmsGroupsDTO.GroupId < 0)
                {
                    cmsGroupsDTO = cmsGroupsDataHandler.InsertGroup(cmsGroupsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    cmsGroupsDTO.AcceptChanges();
                }
                else
                {
                    if (cmsGroupsDTO.IsChanged)
                    {
                        cmsGroupsDTO = cmsGroupsDataHandler.UpdateGroup(cmsGroupsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        cmsGroupsDTO.AcceptChanges();
                    }
                    log.LogMethodExit();
                }
            }
            catch (Exception expn)
            {
                log.Error("Error  at Save() method ", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the CMSGroupsDTO
        /// </summary>
        public CMSGroupsDTO GetCMSGroupsDTO
        {
            get { return cmsGroupsDTO; }
        }

        /// <summary>
        /// Delete the record from the database based on  groupId
        /// </summary>
        /// <returns>return the int </returns>
        public int Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                CMSGroupsDataHandler cmsGroupsDataHandler = new CMSGroupsDataHandler(sqlTransaction);
                int id = cmsGroupsDataHandler.GroupDelete(cmsGroupsDTO.GroupId);
                log.LogMethodExit(id);
                return id;
            }
            catch (Exception expn)
            {
                log.Error("Error  at Delete() method ", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }

    }
    public class CMSGroupList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///Takes LookupParams as parameter
        /// </summary>
        /// <returns>Returns List<KeyValuePair<CMSGroupsDTO.SearchByParameters, string>> by converting CMSGroupsParams</returns>
        public List<KeyValuePair<CMSGroupsDTO.SearchByParameters, string>> BuildCMSGroupSearchParametersList(CMSGroupsParams cMSGroupsParams)
        {
            log.LogMethodEntry(cMSGroupsParams);
            List<KeyValuePair<CMSGroupsDTO.SearchByParameters, string>> cMSGroupSearchParams = new List<KeyValuePair<CMSGroupsDTO.SearchByParameters, string>>();
            if (cMSGroupsParams != null)
            {
                if (cMSGroupsParams.GroupId > 0)
                    cMSGroupSearchParams.Add(new KeyValuePair<CMSGroupsDTO.SearchByParameters, string>(CMSGroupsDTO.SearchByParameters.GROUP_ID, cMSGroupsParams.GroupId.ToString()));
                if (cMSGroupsParams.ParentGroupId > 0)
                    cMSGroupSearchParams.Add(new KeyValuePair<CMSGroupsDTO.SearchByParameters, string>(CMSGroupsDTO.SearchByParameters.PARENT_GROUP_ID, cMSGroupsParams.ParentGroupId.ToString()));
                if (!string.IsNullOrEmpty(cMSGroupsParams.Name))
                    cMSGroupSearchParams.Add(new KeyValuePair<CMSGroupsDTO.SearchByParameters, string>(CMSGroupsDTO.SearchByParameters.NAME, cMSGroupsParams.Name));
                if (cMSGroupsParams.Active)
                    cMSGroupSearchParams.Add(new KeyValuePair<CMSGroupsDTO.SearchByParameters, string>(CMSGroupsDTO.SearchByParameters.ACTIVE, "1"));
                if (cMSGroupsParams.SiteId > 0)
                    cMSGroupSearchParams.Add(new KeyValuePair<CMSGroupsDTO.SearchByParameters, string>(CMSGroupsDTO.SearchByParameters.SITE_ID, cMSGroupsParams.SiteId.ToString()));

            }
            log.LogMethodExit(cMSGroupSearchParams);
            return cMSGroupSearchParams;
        }


        /// <summary>
        /// Returns the GetAllCMGroups based on the search paramaters
        /// </summary>
        public List<CMSGroupsDTO> GetAllCMSGroups(List<KeyValuePair<CMSGroupsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CMSGroupsDataHandler cmsGroupsDataHandler = new CMSGroupsDataHandler(sqlTransaction);
            List<CMSGroupsDTO> cMSGroupsDTOList = cmsGroupsDataHandler.GetGroupsList(searchParameters);
            log.LogMethodExit(cMSGroupsDTOList);
            return cMSGroupsDTOList;

        }

        /// <summary>
        /// Returns the GetAllCMGroups based on the CMSGroupsParams search parameters
        /// </summary>
        /// <param name="cMSGroupsParams">CMSGroupsParams cMSGroupsParams</param>
        /// <returns>List of CMSGroupsDTO> </returns>
        public List<CMSGroupsDTO> GetAllCMSGroups(CMSGroupsParams cMSGroupsParams, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(cMSGroupsParams);
            List<KeyValuePair<CMSGroupsDTO.SearchByParameters, string>> searchParameters = BuildCMSGroupSearchParametersList(cMSGroupsParams);
            CMSGroupsDataHandler cmsGroupsDataHandler = new CMSGroupsDataHandler(sqlTransaction);
            List<CMSGroupsDTO> cMSGroupsDTOList = cmsGroupsDataHandler.GetGroupsList(searchParameters);
            log.LogMethodExit(cMSGroupsDTOList);
            return cMSGroupsDTOList;
        }

        /// <summary>
        /// Returns the GetCMGroupsListTree based on the search parameters
        /// </summary>
        public List<CMSGroupsDTOTree> GetCMSGroupsListTree(int groupId, bool showActive, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(groupId, showActive);
            CMSGroupsDataHandler cmsGroupsDataHandler = new CMSGroupsDataHandler(sqlTransaction);
            List<CMSGroupsDTOTree> cMSGroupsDTOList = cmsGroupsDataHandler.GetGroupsListTree(groupId, showActive);
            log.LogMethodExit(cMSGroupsDTOList);
            return cMSGroupsDTOList;

        }
    }

}
