/********************************************************************************************
 * Project Name - MessagingTriggerRedemptionContentBuilder 
 * Description  -BL class of the Redemption Message Trigger Event  
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     13-Dec-2020    Guru S A             Created for Subscription changes                                                                               
 ********************************************************************************************/

using System;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.Transaction;

namespace Semnox.Parafait.ConcurrentManager
{
    /// <summary>
    /// MessagingTriggerRedemptionContentBuilder
    /// </summary>
    public class MessagingTriggerRedemptionContentBuilder
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 
        private ExecutionContext executionContext;
        private ParafaitFunctionEvents parafaitFunctionEventName;
        private MessagingTriggerDTO messagingTriggerDTO;
        private RedemptionDTO redemptionDTO; 
        private AccountDTO accountDTO;
        private CustomerDTO customerDTO;
        /// <summary>
        /// MessagingTriggerRedemptionContentBuilder
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="parafaitFunctionEventName"></param>
        /// <param name="messagingTriggerDTO"></param>
        /// <param name="redemptionDTO"></param>
        /// <param name="customerDTO"></param>
        /// <param name="accountDTO"></param>
        public MessagingTriggerRedemptionContentBuilder(ExecutionContext executionContext, ParafaitFunctionEvents parafaitFunctionEventName, MessagingTriggerDTO messagingTriggerDTO, RedemptionDTO redemptionDTO, CustomerDTO customerDTO,
            AccountDTO accountDTO)
        {
            log.LogMethodEntry(executionContext, parafaitFunctionEventName, messagingTriggerDTO, redemptionDTO, customerDTO, accountDTO); 
            this.executionContext = executionContext;
            this.parafaitFunctionEventName = parafaitFunctionEventName;
            this.messagingTriggerDTO = messagingTriggerDTO;
            this.redemptionDTO = redemptionDTO;
            this.customerDTO = customerDTO;
            this.accountDTO = accountDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// GenerateMessageSubjectForRedemption
        /// </summary>
        /// <param name="messageTemplateContent"></param>
        /// <returns></returns>
        public string GenerateMessageSubjectForRedemption(string messageTemplateSubject)
        {
            log.LogMethodEntry(messageTemplateSubject);
            string messageSubjectContent = string.Empty;
            if (string.IsNullOrWhiteSpace(messageTemplateSubject) == false && messageTemplateSubject.Contains("@"))
            {
                RedemptionTemplateKeywordFormatter redemptionTemplateKeywordFormatter = new RedemptionTemplateKeywordFormatter(executionContext, parafaitFunctionEventName, redemptionDTO, customerDTO, accountDTO, true);
                TemplateText templateText = new TemplateText(messageTemplateSubject);
                messageSubjectContent = redemptionTemplateKeywordFormatter.Format(templateText);
            }
            else
            {
                messageSubjectContent = messageTemplateSubject;
            }
            log.LogMethodExit(messageSubjectContent);
            return messageSubjectContent; 
        }
        /// <summary>
        /// GenerateMessageContentForRedemption
        /// </summary>
        /// <param name="messageTemplateContent"></param>
        /// <returns></returns>
        public string GenerateMessageContentForRedemption(string messageTemplateContent)
        { 
            log.LogMethodEntry(messageTemplateContent);
            string messageBodyContent = string.Empty;
            RedemptionTemplateKeywordFormatter redemptionTemplateKeywordFormatter = new RedemptionTemplateKeywordFormatter(executionContext, parafaitFunctionEventName, redemptionDTO, customerDTO, accountDTO,false);
            TemplateText templateText = new TemplateText(messageTemplateContent);
            messageBodyContent = redemptionTemplateKeywordFormatter.Format(templateText);
            log.LogMethodExit(messageBodyContent);
            return messageBodyContent;  
        }
    }
}