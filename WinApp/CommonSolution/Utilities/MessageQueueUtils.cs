using System;
using System.Collections.Generic;
using System.Messaging;
using System.Linq;
using System.Text;

namespace Semnox.Core.Utilities
{
    public static class MessageQueueUtils
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static MessageQueue GetMessageQueue(object QueueId)
        {
            log.LogMethodEntry(QueueId);
            MessageQueue messageQueue = null;
            if (MessageQueue.Exists(@".\Private$\ParafaitQueue" + QueueId.ToString()))
            {
                messageQueue = new MessageQueue(@".\Private$\ParafaitQueue" + QueueId.ToString());
                messageQueue.Label = "Existing Queue";
            }
            else
            {
                // Create the Queue
                MessageQueue.Create(@".\Private$\ParafaitQueue" + QueueId.ToString());
                messageQueue = new MessageQueue(@".\Private$\ParafaitQueue" + QueueId.ToString());
                messageQueue.Label = "Newly Created Queue";
            }
            log.LogMethodExit(messageQueue);
            return messageQueue;
        }

        public static void SendMessage(object QueueId, string message)
        {
            log.LogMethodEntry(QueueId, message);
            MessageQueue messageQueue = GetMessageQueue(QueueId);
            messageQueue.Send(message);
            log.LogMethodExit(null);
        }
    }
}

