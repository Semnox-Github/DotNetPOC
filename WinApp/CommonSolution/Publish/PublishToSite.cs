/********************************************************************************************
 * Project Name - Game                                                                          
 * Description  - Manages the game object. The game machines are the physical objects and they
 * are of a particular type which is referred to as game
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By            Remarks          
 *********************************************************************************************
 *2.50        12-Dec-2018   Jagan Mohana Rao       Tree view structure for the publish to sites for HQ.
 *2.90        12-Jun-2020    Girish Kundar         Modified :  Enhanced to support bulk publishing feature /Batch Publish
 ********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Publish
{
    public class PublishToSite
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Utilities utilities = new Utilities();
        private List<PublishToSiteDTO> publishToSiteDTOList;
        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
        private HashSet<int> selectedSiteIdList;
        public PublishToSite(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.publishToSiteDTOList = null;
            log.LogMethodExit();
        }
        public PublishToSite(ExecutionContext executionContext, List<PublishToSiteDTO> publishToSiteDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, publishToSiteDTOList);
            this.publishToSiteDTOList = publishToSiteDTOList;
            utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
            utilities.ExecutionContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
            selectedSiteIdList = new HashSet<int>();
            log.LogMethodExit();
        }
        /// <summary>
        /// Get method for Organizations and Sites recurvie function for leaf node.
        /// </summary>
        /// <param name="publishToSiteDTO"></param>
        public PublishToSiteDTO Recursive(PublishToSiteDTO publishToSiteDTO)
        {
            log.LogMethodEntry(publishToSiteDTO);
            List<PublishToSiteDTO> treeItems = new List<PublishToSiteDTO>();
            Semnox.Parafait.Site.OrganizationList organizationList = new Semnox.Parafait.Site.OrganizationList();
            List<KeyValuePair<Semnox.Parafait.Site.OrganizationDTO.SearchByOrganizationParameters, string>> searchParameters = new List<KeyValuePair<Semnox.Parafait.Site.OrganizationDTO.SearchByOrganizationParameters, string>>();
            searchParameters.Add(new KeyValuePair<OrganizationDTO.SearchByOrganizationParameters, string>(OrganizationDTO.SearchByOrganizationParameters.PARENT_ORG_ID, Convert.ToString(publishToSiteDTO.Id)));
            List<Semnox.Parafait.Site.OrganizationDTO> organizationDTOList = organizationList.GetAllOrganizations(searchParameters);
            publishToSiteDTO.Children = new List<PublishToSiteDTO>();
            if (organizationDTOList != null)
            {
                List<PublishToSiteDTO> childItems = new List<PublishToSiteDTO>();
                publishToSiteDTO.Children = new List<PublishToSiteDTO>();
                foreach (OrganizationDTO organizationDTO in organizationDTOList)
                {
                    if (organizationDTO != null)
                    {
                        PublishToSiteDTO newItem = new PublishToSiteDTO();
                        newItem.Children = new List<PublishToSiteDTO>();
                        newItem.Id = Convert.ToString(organizationDTO.OrgId);
                        newItem.Name = organizationDTO.OrgName;
                        newItem.isSelected = false;
                        newItem.isSite = false;
                        childItems.Add(newItem);
                        Recursive(newItem);
                    }
                }
                publishToSiteDTO.Children = childItems;
            }
            else
            {
                // Find the sites for the leaf node of that particular node                
                List<PublishToSiteDTO> newSiteDetails = new List<PublishToSiteDTO>();
                List<PublishToSiteDTO> siteItems = new List<PublishToSiteDTO>();
                siteItems = GetSites(publishToSiteDTO.Id);
                if (siteItems.Count != 0)
                {
                    foreach (var siteListDTO in siteItems)
                    {
                        if (siteListDTO != null)
                        {
                            PublishToSiteDTO site = new PublishToSiteDTO();
                            site.Children = new List<PublishToSiteDTO>();
                            site.Id = siteListDTO.Id;
                            site.Name = siteListDTO.Name;
                            site.isSelected = false;
                            site.isSite = true;
                            newSiteDetails.Add(site);
                        }
                    }
                    publishToSiteDTO.Children = newSiteDetails;
                }
            }
            log.LogMethodExit(publishToSiteDTO);
            return publishToSiteDTO;
        }
        /// <summary>
        /// Get the sites based on the parent organization id
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        private List<PublishToSiteDTO> GetSites(string orgId)
        {
            log.LogMethodEntry(orgId);
            SiteList siteList = new SiteList(executionContext);
            List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchSiteParameters = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
            searchSiteParameters.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.ORG_ID, Convert.ToString(orgId)));
            List<SiteDTO> siteDTOList = siteList.GetAllSites(searchSiteParameters);
            List<PublishToSiteDTO> childrenList = new List<PublishToSiteDTO>();
            if (siteDTOList != null)
            {
                foreach (SiteDTO siteDTO in siteDTOList)
                {
                    PublishToSiteDTO publishToSiteDTO = new PublishToSiteDTO();
                    publishToSiteDTO.Id = Convert.ToString(siteDTO.SiteId);
                    publishToSiteDTO.Name = siteDTO.SiteName;
                    publishToSiteDTO.isSelected = false;
                    childrenList.Add(publishToSiteDTO);
                }
            }
            return childrenList;
        }

        /// <summary>
        /// Post method for Sites level recurvie function 
        /// Check the coditon for particular site selected site or not and publish the site.
        /// </summary>
        /// <param name="publishToSiteDTO"></param>
        private void PublishingRecursive(PublishToSiteDTO dtoRecusive = null)
        {
            if (dtoRecusive.Children.Count > 0)
            {
                foreach (PublishToSiteDTO publishDto in dtoRecusive.Children)
                {
                    PublishingRecursive(publishDto);
                }
            }
            else
            {
                if (dtoRecusive.isSite == true && dtoRecusive.isSelected == true)
                {
                    try
                    {
                        Publish publish = new Publish();
                        publish = new Publish(dtoRecusive.EntityName, utilities);
                        publish.PublishEntity(Convert.ToInt32(dtoRecusive.EntityId), executionContext.GetSiteId(), Convert.ToInt32(dtoRecusive.Id));   // MasterEntityId, CurrentSiteId, ToSiteId
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        throw ex;
                    }
                }
            }
        }

        public void Publishing()
        {
            try
            {
                log.LogMethodEntry();
                if (publishToSiteDTOList != null && publishToSiteDTOList.Any())
                {
                    try
                    {
                        if (publishToSiteDTOList.Exists(entity => entity.EntityName == null || entity.EntityName == string.Empty))
                        {
                            log.Error("Entity name or entity id is not valid");
                            throw new Exception(MessageContainerList.GetMessage(executionContext, "Entity name or entity id is not valid"));
                        }
                        if (publishToSiteDTOList.Exists(entity => entity.EntityId == null || Convert.ToInt32(entity.EntityId) == -1))
                        {
                            log.Error("Entity name or entity id is not valid");
                            throw new Exception(MessageContainerList.GetMessage(executionContext, "Entity id is not valid"));
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Publish data is not valid. Please check the entity Id and entity name");
                        log.Error("Entity name or entity id is not valid" + ex.Message);
                        throw new Exception(MessageContainerList.GetMessage(executionContext, "Entity name or entity id is not valid"));
                    }
                    string entityName = string.IsNullOrWhiteSpace(publishToSiteDTOList[0].EntityName) ? string.Empty : publishToSiteDTOList[0].EntityName;
                    log.Debug("EntityName  for Publish : " + entityName);
                    if (TableFactory.EntitySupportedByTableFactory(entityName, utilities.ExecutionContext))
                    {
                        HashSet<int> selectedSiteIdList = new HashSet<int>();
                        HashSet<int> primaryKeyIdList = new HashSet<int>();
                        var PKeyIdList = publishToSiteDTOList.Select(entity => int.Parse(entity.EntityId)).Distinct().ToList();
                        selectedSiteIdList = PopulateSelectedSiteIdList(publishToSiteDTOList);
                        primaryKeyIdList = new HashSet<int>(PKeyIdList);
                        log.LogVariableState("selectedSiteIdList", selectedSiteIdList);
                        log.LogVariableState("primaryKeyIdList", primaryKeyIdList);
                        Publish publish = new Publish(publishToSiteDTOList[0].EntityName, utilities);
                        publish.BulkEntityPublish(selectedSiteIdList, primaryKeyIdList, true);
                    }
                    else
                    {
                        foreach (PublishToSiteDTO publishToSites in publishToSiteDTOList)
                        {
                            PublishingRecursive(publishToSites);
                        }
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
        }

        private HashSet<int> PopulateSelectedSiteIdList(List<PublishToSiteDTO> publishToSiteDTOList)
        {
            log.LogMethodEntry(publishToSiteDTOList);
            foreach (PublishToSiteDTO dto in publishToSiteDTOList)
            {
                GetSitesRecursive(dto);
            }
            log.LogMethodExit(selectedSiteIdList);
            return selectedSiteIdList;
        }

        private void GetSitesRecursive(PublishToSiteDTO dtoRecusive = null)
        {
            if (dtoRecusive.Children.Count > 0)
            {
                foreach (PublishToSiteDTO publishDto in dtoRecusive.Children)
                {
                    GetSitesRecursive(publishDto);
                }
            }
            else
            {
                if (dtoRecusive.isSite == true && dtoRecusive.isSelected == true && !string.IsNullOrEmpty(dtoRecusive.Id))
                {
                    try
                    {
                        selectedSiteIdList.Add(Convert.ToInt32(dtoRecusive.Id)); //ToSiteId
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        throw ex;
                    }
                }
            }
        }
    }
}