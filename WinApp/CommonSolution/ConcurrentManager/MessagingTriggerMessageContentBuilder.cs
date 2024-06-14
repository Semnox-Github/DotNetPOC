/********************************************************************************************
 * Project Name - MessagingTriggerMessageContentBuilder 
 * Description  -BL class to build message content for customer data
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

namespace Semnox.Parafait.ConcurrentManager
{
    /// <summary>
    /// MessagingTriggerMessageContentBuilder
    /// </summary>
    public class MessagingTriggerMessageContentBuilder
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private MessagingTriggerDTO messagingTriggerDTO;
        private CustomerDTO customerDTO;
        private AccountDTO accountDTO;
        private ExecutionContext executionContext;
        private ParafaitFunctionEvents parafaitFunctionEvents;  

        private MessagingTriggerMessageContentBuilder(ExecutionContext executionContext, ParafaitFunctionEvents parafaitFunctionEvents)
        {
            log.LogMethodEntry(executionContext, parafaitFunctionEvents);
            this.executionContext = executionContext;
            this.parafaitFunctionEvents = parafaitFunctionEvents;
            log.LogMethodExit();
        }
        /// <summary>
        /// CustomerMessageContentBuilder
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="customerDTO"></param>
        public MessagingTriggerMessageContentBuilder(ExecutionContext executionContext, ParafaitFunctionEvents parafaitFunctionEvents, MessagingTriggerDTO messagingTriggerDTO, CustomerDTO customerDTO, AccountDTO accountDTO) 
            : this(executionContext, parafaitFunctionEvents)
        {
            log.LogMethodEntry("customerDTO", accountDTO, messagingTriggerDTO);
            this.messagingTriggerDTO = messagingTriggerDTO;
            this.customerDTO = customerDTO;
            this.accountDTO = accountDTO; 
            log.LogMethodExit();
        } 
        /// <summary>
        /// GenerateMessageContent
        /// </summary>
        /// <param name="messageTemplateContent"></param>
        /// <returns></returns>
        public string GenerateMessageContentForCustomer(string messageTemplateContent)
        {
            log.LogMethodEntry(messageTemplateContent);
            string formattedContent = string.Empty;
            CustomerTemplateKeywordFormatter customerTemplateKeywordFormatter = new CustomerTemplateKeywordFormatter(executionContext, customerDTO, accountDTO, this.parafaitFunctionEvents);
            TemplateText templateText = new TemplateText(messageTemplateContent);
            formattedContent = customerTemplateKeywordFormatter.Format(templateText); 
            log.LogMethodExit(formattedContent);
            return formattedContent;
        }
        
        /// <summary>
        /// GenerateMessageSubjectForCustomer
        /// </summary>
        /// <param name="messageTemplateSubject"></param>
        /// <returns></returns>
        public string GenerateMessageSubjectForCustomer(string messageTemplateSubject)
        {
            log.LogMethodEntry(messageTemplateSubject);
            string formattedContent = string.Empty;
            if (string.IsNullOrWhiteSpace(messageTemplateSubject) == false && messageTemplateSubject.Contains("@"))
            {
                CustomerTemplateKeywordFormatter customerTemplateKeywordFormatter = new CustomerTemplateKeywordFormatter(executionContext, customerDTO, accountDTO, this.parafaitFunctionEvents, true);
                TemplateText templateText = new TemplateText(messageTemplateSubject);
                formattedContent = customerTemplateKeywordFormatter.Format(templateText);
            }
            else
            {
                formattedContent = messageTemplateSubject;
            }
            log.LogMethodExit(formattedContent);
            return formattedContent;
        }   

    }
}
