/********************************************************************************************
 * Project Name - MaintenanceContentBuilder 
 * Description  -BL class to build message content for Maintenance
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     14-Dec-2020    Guru S A             Created for Subscription changes                                                                               
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// MaintenanceContentBuilder
    /// </summary>
    public class MaintenanceContentBuilder
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private UserJobItemsDTO userJobItemsDTO;
        private ExecutionContext executionContext;
        private ParafaitFunctionEvents parafaitFunctionEvents;
        private LookupValues jobStatusLookup;

        private string DATE_FORMAT = string.Empty;
        private string AMOUNT_FORMAT = string.Empty;
        private string NUMBER_FORMAT = string.Empty;
        private string DATETIME_FORMAT = string.Empty; 
        private bool isSubjectContent = false;

        private MaintenanceContentBuilder(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext); 
            this.executionContext = executionContext; 
            log.LogMethodExit();
        }
        /// <summary>
        /// MaintenanceContentBuilder
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="parafaitFunctionEvents"></param>
        /// <param name="userJobItemsDTO"></param>
        /// <param name="isSubjectContent"></param>
        public MaintenanceContentBuilder(ExecutionContext executionContext, ParafaitFunctionEvents parafaitFunctionEvents, UserJobItemsDTO userJobItemsDTO,  bool isSubjectContent = false)
            : this(executionContext)
        {
            log.LogMethodEntry(parafaitFunctionEvents, userJobItemsDTO, isSubjectContent);
            this.parafaitFunctionEvents = parafaitFunctionEvents;
            this.userJobItemsDTO = userJobItemsDTO; 
            this.isSubjectContent = isSubjectContent;
            jobStatusLookup = new LookupValues(executionContext, userJobItemsDTO.Status);
            SetFormats();
            log.LogMethodExit();
        } 
        private void SetFormats()
        {
            log.LogMethodEntry();
            DATE_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT");
            AMOUNT_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT");
            NUMBER_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NUMBER_FORMAT");
            DATETIME_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT");
            log.LogMethodExit();
        }
        /// <summary>
        /// GenerateMessageContent
        /// </summary>
        /// <param name="messageTemplateContent"></param>
        /// <returns></returns>
        public string GenerateMessageContent(string messageTemplateContent)
        {
            log.LogMethodEntry(messageTemplateContent);
            string formattedContent = string.Empty; 
            formattedContent = UserJobItemsDTOTextFormatter(messageTemplateContent);  
            log.LogMethodExit(formattedContent);
            return formattedContent;
        }          

        private string UserJobItemsDTOTextFormatter(string messageTemplateContent)
        {
            log.LogMethodEntry(messageTemplateContent);
            string formattedContent = string.Empty;
            if (this.userJobItemsDTO != null)
            {
                TemplateKeywordFormatter templateKeywordFormatter = new TemplateKeywordFormatter();
                templateKeywordFormatter.Add("@ServiceRequest", userJobItemsDTO.MaintJobNumber + ", " + userJobItemsDTO.MaintJobName);
                templateKeywordFormatter.Add("@Status", (jobStatusLookup.LookupValuesDTO != null ? jobStatusLookup.LookupValuesDTO.Description : string.Empty));
                string siteName = GetSiteName();
                templateKeywordFormatter.Add("@siteName", siteName);
                templateKeywordFormatter.Add("@SiteName", siteName); 

                TemplateText templateText = new TemplateText(messageTemplateContent);
                formattedContent = templateKeywordFormatter.Format(templateText);
            }
            log.LogMethodExit(formattedContent);
            return formattedContent;
        }

        private string GetSiteName()
        {
            log.LogMethodEntry(executionContext);
            SiteList siteList = new SiteList(executionContext);
            List<SiteDTO> siteDTOList = siteList.GetAllSites(new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>());
            string siteName = string.Empty;
            if (siteDTOList != null)
            {
                SiteDTO siteDTO = null;
                if (executionContext.GetSiteId() == -1)
                {
                    siteDTO = siteDTOList.FirstOrDefault();
                }
                else
                {
                    siteDTO = siteDTOList.Where(x => x.SiteId == executionContext.GetSiteId()).FirstOrDefault();
                }
                if (siteDTO != null)
                {
                    siteName = siteDTO.SiteName;
                }
            }
            log.LogMethodExit(siteName);
            return siteName;
        }
    }
}
