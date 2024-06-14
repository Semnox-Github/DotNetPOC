/********************************************************************************************
 * Project Name - LinkCustomerAccountEventBL 
 * Description  -BL class of the Link Customer Account Event  
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     13-Dec-2020    Guru S A             Created for Subscription changes                                                                               
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using System.Data.SqlClient;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// LinkCustomerAccountEventBL
    /// </summary>
    public class LinkCustomerAccountEventBL: CustomerEventsBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 
        private AccountDTO accountDTO;

        /// <summary>
        /// LinkCustomerAccountEventBL
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="customerDTO"></param>
        /// <param name="accountDTO"></param>
        /// <param name="sqlTransaction"></param>
        public LinkCustomerAccountEventBL(ExecutionContext executionContext, CustomerDTO customerDTO, AccountDTO accountDTO, SqlTransaction sqlTransaction = null)
            : base(executionContext, ParafaitFunctionEvents.LINK_CUSTOMER_ACCOUNT_EVENT, customerDTO, sqlTransaction)
        {
            log.LogMethodEntry(executionContext, customerDTO, accountDTO, sqlTransaction);
            this.accountDTO = accountDTO; 
            LoadParafaitFunctionEventDTO(ParafaitFunctionEvents.LINK_CUSTOMER_ACCOUNT_EVENT, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// MessageSubjectFormatter
        /// </summary>
        /// <param name="messageTemplateSubject"></param>
        /// <returns></returns>
        public override string MessageSubjectFormatter(string messageTemplateSubject)
        {
            log.LogMethodEntry(messageTemplateSubject);
            string messageSubjectContent = string.Empty;
            if (string.IsNullOrWhiteSpace(messageTemplateSubject) == false && messageTemplateSubject.Contains("@"))
            {
                CustomerMessageContentBuilder customerMessageContentBuilder = new CustomerMessageContentBuilder(executionContext, this.customerDTO, this.accountDTO, this.parafaitFunctionEventDTO.ParafaitFunctionEventName);
                messageSubjectContent = customerMessageContentBuilder.GenerateMessageSubject(messageTemplateSubject);
            }
            else
            {
                messageSubjectContent = messageTemplateSubject;
            }
            log.LogMethodExit(messageSubjectContent);
            return messageSubjectContent;
        }
        /// <summary>
        /// MessageBodyFormatter
        /// </summary>
        /// <param name="messageTemplateContent"></param>
        /// <returns></returns>
        public override string MessageBodyFormatter(string messageTemplateContent)
        {
            log.LogMethodEntry(messageTemplateContent);
            string messageBodyContent = string.Empty;
            CustomerMessageContentBuilder customerMessageContentBuilder = new CustomerMessageContentBuilder(executionContext, this.customerDTO, this.accountDTO, this.parafaitFunctionEventDTO.ParafaitFunctionEventName);
            messageBodyContent = customerMessageContentBuilder.GenerateMessageContent(messageTemplateContent);
            log.LogMethodExit(messageBodyContent);
            return messageBodyContent;
        } 

    }
}

