/********************************************************************************************
 * Project Name - RedemptionTemplateKeywordFormatter 
 * Description  -Template Keyword Formatter for Redemption
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     12-Dec-2020    Guru S A             Created for Subscription changes                                                                               
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts; 
using Semnox.Parafait.Site;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// RedemptionTemplateKeywordFormatter
    /// </summary>
    public class RedemptionTemplateKeywordFormatter : TemplateKeywordFormatter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private RedemptionDTO redemptionDTO;
        private CustomerDTO customerDTO;
        private AccountDTO accountDTO;
        private LookupValuesList serverDateTime;
        private string DATE_FORMAT = string.Empty;
        private string AMOUNT_FORMAT = string.Empty;
        private string NUMBER_FORMAT = string.Empty;
        private ParafaitFunctionEvents parafaitFunctionEvent;
        private bool isSubject = false;
        private CustomerTemplateKeywordFormatter customerTemplateKeywordFormatter;
        private RedemptionTemplateKeywordFormatter(ExecutionContext executionContext, ParafaitFunctionEvents parafaitFunctionEvents) :base()
        {
            log.LogMethodEntry(executionContext, parafaitFunctionEvents);
            this.executionContext = executionContext;
            this.parafaitFunctionEvent = parafaitFunctionEvents;
            this.customerTemplateKeywordFormatter = null;
            serverDateTime = new LookupValuesList(executionContext);
            DATE_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT");
            AMOUNT_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT");
            NUMBER_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NUMBER_FORMAT"); 
            log.LogMethodExit();
        }
        /// <summary>
        /// RedemptionTemplateKeywordFormatter
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="parafaitFunctionEvents"></param>
        /// <param name="redemptionDTO"></param>
        /// <param name="customerDTO"></param>
        /// <param name="accountDTO"></param>
        /// <param name="isSubject"></param>
        public RedemptionTemplateKeywordFormatter(ExecutionContext executionContext, ParafaitFunctionEvents parafaitFunctionEvents, RedemptionDTO redemptionDTO,
                                                  CustomerDTO customerDTO, AccountDTO accountDTO, bool isSubject = false)
            : this(executionContext, parafaitFunctionEvents)
        {
            log.LogMethodEntry(redemptionDTO, parafaitFunctionEvent, isSubject);
            this.redemptionDTO = redemptionDTO;
            this.customerDTO = customerDTO;
            this.accountDTO = accountDTO;
            this.isSubject = isSubject;
            if (customerDTO != null || accountDTO != null)
            {
                customerTemplateKeywordFormatter = new CustomerTemplateKeywordFormatter(executionContext, customerDTO, accountDTO, parafaitFunctionEvents, isSubject);
            }
            AddAllTags();
            log.LogMethodExit();
        } 
        private void AddAllTags()
        {
            log.LogMethodEntry();
            AddRedemptionHeaderTags();
            AddRedemptionLineTags();
            if (customerTemplateKeywordFormatter == null)
            {
                AddSiteNameTag();
            }
            log.LogMethodExit();
        }
        private void AddSiteNameTag()
        {
            log.LogMethodEntry();
            string siteName = GetSiteName();
            Add("@siteName", siteName);
            Add("@SiteName", siteName);
            log.LogMethodExit();
        }
        /// <summary>
        /// GetSiteName
        /// </summary>
        /// <returns></returns>
        public string GetSiteName()
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
        private void AddRedemptionHeaderTags()
        {
            log.LogMethodEntry(); 
            Add("@RedemptionOrderNo", redemptionDTO.RedemptionOrderNo);
            Add("@RedeemedDate", redemptionDTO.RedeemedDate == null? string.Empty: ((DateTime)redemptionDTO.RedeemedDate).ToString(DATE_FORMAT));
            Add("@RedemptionPreparedDate", redemptionDTO.OrderCompletedDate == null ? string.Empty : ((DateTime)redemptionDTO.OrderCompletedDate).ToString(DATE_FORMAT));
            Add("@RedemptionDeliveryDate", redemptionDTO.OrderDeliveredDate == null ? string.Empty : ((DateTime)redemptionDTO.OrderDeliveredDate).ToString(DATE_FORMAT));
            Add("@RedemptionStatus", redemptionDTO.RedemptionStatus);
            Add("@RedemptionSource", redemptionDTO.Source);
            Add("@RedemptionId", redemptionDTO.RedemptionId.ToString(NUMBER_FORMAT));
            log.LogMethodExit();
        } 
        private void AddRedemptionLineTags()
        {
            log.LogMethodEntry(); 
            //place holder
            log.LogMethodExit();
        }
        /// <summary>
        /// Format
        /// </summary>
        /// <param name="templateText"></param>
        /// <returns></returns>
        public override string Format(TemplateText templateText)
        {
            log.LogMethodEntry(templateText);
            string formattedContent = string.Empty;
            TemplateText formattedCustTemplateText = null;
            if (customerTemplateKeywordFormatter != null)
            {
                formattedCustTemplateText = customerTemplateKeywordFormatter.FormatedTemplateText(templateText);
            }
            formattedContent = base.Format(formattedCustTemplateText == null ? templateText : formattedCustTemplateText);
            log.LogMethodExit(formattedContent);
            return formattedContent;
        }
    }
}
