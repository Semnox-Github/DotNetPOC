/********************************************************************************************
 * Project Name - Utilities
 * Description  - Data structure of MessageViewContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.110.0     27-NOV-2020   Lakshminarayana     Created: POS Redesign
 ********************************************************************************************/

using System.Collections.Generic;

namespace Semnox.Parafait.Languages
{
    /// <summary>
    /// Data structure of MessageViewContainer
    /// </summary>
    public class MessageContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int messageId;
        private int messageNumber;
        private string message;
        private string translatedMessage;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MessageContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public MessageContainerDTO(int messageId, int messageNumber, string message, string translatedMessage)
            : this()
        {
            log.LogMethodEntry(messageId, messageNumber, message, translatedMessage);
            this.messageId = messageId;
            this.messageNumber = messageNumber;
            this.message = message;
            this.translatedMessage = translatedMessage;
            log.LogMethodExit();
        }



        /// <summary>
        /// Get/Set method of the messageId field
        /// </summary>
        public int MessageId
        {
            get
            {
                return messageId;
            }

            set
            {
                messageId = value;
            }
        }



        /// <summary>
        /// Get/Set method of the messageNo field
        /// </summary>
        public int MessageNumber
        {
            get
            {
                return messageNumber;
            }

            set
            {
                messageNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of the message field
        /// </summary>
        public string Message
        {
            get
            {
                return message;
            }

            set
            {
                message = value;
            }
        }

        /// <summary>
        /// Get/Set method of the translatedMessage field
        /// </summary>
        public string TranslatedMessage
        {
            get
            {
                return translatedMessage;
            }

            set
            {
                translatedMessage = value;
            }
        }

    }
}
