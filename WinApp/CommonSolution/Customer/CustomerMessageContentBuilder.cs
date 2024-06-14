/********************************************************************************************
 * Project Name - CustomerMessageContentBuilder 
 * Description  -BL class to build message content for customer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     12-Dec-2020    Guru S A           Created for Subscription changes                                                                               
 ********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer.Accounts;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// CustomerMessageContentBuilder
    /// </summary>
    public class CustomerMessageContentBuilder
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CustomerDTO customerDTO;
        private AccountDTO accountDTO;
        private ExecutionContext executionContext;
        private ParafaitFunctionEvents parafaitFunctionEvents;

        private CustomerMessageContentBuilder(ExecutionContext executionContext, ParafaitFunctionEvents parafaitFunctionEvents)
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
        /// <param name="accountDTO"></param>
        /// <param name="parafaitFunctionEvents"></param>
        public CustomerMessageContentBuilder(ExecutionContext executionContext, CustomerDTO customerDTO, AccountDTO accountDTO, ParafaitFunctionEvents parafaitFunctionEvents)
            : this(executionContext, parafaitFunctionEvents)
        {
            log.LogMethodEntry(executionContext, customerDTO, parafaitFunctionEvents);
            this.customerDTO = customerDTO;
            this.accountDTO = accountDTO;
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
            CustomerTemplateKeywordFormatter customerTemplateKeywordFormatter = new CustomerTemplateKeywordFormatter(executionContext, customerDTO, accountDTO, parafaitFunctionEvents);
            TemplateText templateText = new TemplateText(messageTemplateContent);
            formattedContent = customerTemplateKeywordFormatter.Format(templateText);
            log.LogMethodExit(formattedContent);
            return formattedContent;
        }

        /// <summary>
        /// GenerateMessageSubject
        /// </summary>
        /// <param name="messageTemplateSubject"></param>
        /// <returns></returns>
        public string GenerateMessageSubject(string messageTemplateSubject)
        {
            log.LogMethodEntry(messageTemplateSubject);
            string formattedContent = string.Empty;
            if (string.IsNullOrWhiteSpace(messageTemplateSubject) == false && messageTemplateSubject.Contains("@"))
            {
                CustomerTemplateKeywordFormatter customerTemplateKeywordFormatter = new CustomerTemplateKeywordFormatter(executionContext, customerDTO, accountDTO, parafaitFunctionEvents, true);
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
