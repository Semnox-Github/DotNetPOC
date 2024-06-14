/********************************************************************************************
 * Project Name - CampaignMessageContentBuilder 
 * Description  -BL class to build message content for Campaign Messages
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
using Semnox.Parafait.Customer; 

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// CampaignMessageContentBuilder
    /// </summary>
    public class CampaignMessageContentBuilder
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CampaignCustomerDTO campaignCustomerDTO;
        private ExecutionContext executionContext;  

        private string DATE_FORMAT = string.Empty;
        private string AMOUNT_FORMAT = string.Empty;
        private string NUMBER_FORMAT = string.Empty;
        private string DATETIME_FORMAT = string.Empty; 
        private bool isSubjectContent = false;

        private CampaignMessageContentBuilder(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext); 
            this.executionContext = executionContext; 
            log.LogMethodExit();
        }
        /// <summary>
        /// CampaignMessageContentBuilder
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="executionContext"></param>
        /// <param name="campaignCustomerDTO"></param>
        /// <param name="isSubjectContent"></param>
        public CampaignMessageContentBuilder(ExecutionContext executionContext, CampaignCustomerDTO campaignCustomerDTO,  bool isSubjectContent = false)
            : this(executionContext)
        {
            log.LogMethodEntry(campaignCustomerDTO, isSubjectContent);
            this.campaignCustomerDTO = campaignCustomerDTO; 
            this.isSubjectContent = isSubjectContent; 
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
            formattedContent = CampaignCustomerDTOTextFormatter(messageTemplateContent);  
            log.LogMethodExit(formattedContent);
            return formattedContent;
        }
          

        private string CampaignCustomerDTOTextFormatter(string messageTemplateContent)
        {
            log.LogMethodEntry(messageTemplateContent);
            string formattedContent = string.Empty;
            if (this.campaignCustomerDTO != null)
            {
                TemplateKeywordFormatter templateKeywordFormatter = new TemplateKeywordFormatter();
                templateKeywordFormatter.Add("@CustomerName", campaignCustomerDTO.Name);
                templateKeywordFormatter.Add("@customerName", campaignCustomerDTO.Name);
                templateKeywordFormatter.Add("@CardNumber", campaignCustomerDTO.CardNumber);
                templateKeywordFormatter.Add("@CardCredits", (campaignCustomerDTO.Credit != null? Convert.ToDouble(campaignCustomerDTO.Credit).ToString(AMOUNT_FORMAT) : string.Empty));
                templateKeywordFormatter.Add("@EmailId", campaignCustomerDTO.Email);

                TemplateText templateText = new TemplateText(messageTemplateContent);
                formattedContent = templateKeywordFormatter.Format(templateText);
            }
            log.LogMethodExit(formattedContent);
            return formattedContent;
        }

    }
}
