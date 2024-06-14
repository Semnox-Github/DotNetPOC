/********************************************************************************************
 * Project Name - MessagingTriggerPurchaseContentBuilder 
 * Description  -BL class of the Purchase Message Trigger Event  
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     13-Dec-2020    Guru S A             Created for Subscription changes                                                                               
 ********************************************************************************************/

using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Transaction;

namespace Semnox.Parafait.ConcurrentManager
{
    /// <summary>
    /// MessagingTriggerPurchaseContentBuilder
    /// </summary>
    public class MessagingTriggerPurchaseContentBuilder
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 
        private ExecutionContext executionContext;
        private ParafaitFunctionEvents parafaitFunctionEventName;
        private MessagingTriggerDTO messagingTriggerDTO;
        private Transaction.Transaction transaction;
        private Utilities utilities;

        public MessagingTriggerPurchaseContentBuilder(Utilities utilities, ExecutionContext executionContext, ParafaitFunctionEvents parafaitFunctionEventName, MessagingTriggerDTO messagingTriggerDTO, Transaction.Transaction transaction)
        {
            log.LogMethodEntry(executionContext, parafaitFunctionEventName, messagingTriggerDTO, transaction);
            this.utilities = utilities;
            this.executionContext = executionContext;
            this.parafaitFunctionEventName = parafaitFunctionEventName;
            this.messagingTriggerDTO = messagingTriggerDTO;
            this.transaction = transaction;
            log.LogMethodExit();
        }
        /// <summary>
        /// GenerateMessageSubjectForTransaction
        /// </summary>
        /// <param name="messageTemplateContent"></param>
        /// <returns></returns>
        public string GenerateMessageSubjectForTransaction(string messageTemplateSubject)
        {
            log.LogMethodEntry(messageTemplateSubject);
            string messageSubjectContent = string.Empty;
            if (string.IsNullOrWhiteSpace(messageTemplateSubject) == false && messageTemplateSubject.Contains("@"))
            {
                TransactionEmailTemplatePrint transactionEmailTemplatePrint = new TransactionEmailTemplatePrint(executionContext, utilities, -1, transaction,null, true);
                messageSubjectContent = transactionEmailTemplatePrint.BuildContent(messageTemplateSubject);
            }
            else
            {
                messageSubjectContent = messageTemplateSubject;
            }
            log.LogMethodExit(messageSubjectContent);
            return messageSubjectContent; 
        }
        /// <summary>
        /// GenerateMessageContentForTransaction
        /// </summary>
        /// <param name="messageTemplateContent"></param>
        /// <returns></returns>
        public string GenerateMessageContentForTransaction(string messageTemplateContent)
        { 
            log.LogMethodEntry(messageTemplateContent);
            string messageBodyContent = string.Empty;
            TransactionEmailTemplatePrint transactionEmailTemplatePrint = new TransactionEmailTemplatePrint(executionContext, utilities, -1, transaction, null);
            messageBodyContent = transactionEmailTemplatePrint.BuildContent(messageTemplateContent);
            log.LogMethodExit(messageBodyContent);
            return messageBodyContent;  
        }
    }
}