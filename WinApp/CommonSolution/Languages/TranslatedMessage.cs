/********************************************************************************************
 * Project Name - TranslatedMessage
 * Description  - Data object of the TranslatedMessage
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        05-Jan-2017   Vinayaka V          Created 
 *2.70.2        19-Jul -2019  Girish Kundar       Modified : Added Logger methods .
 ********************************************************************************************/
using System;
using System.Xml.Serialization;

namespace Semnox.Parafait.Languages
{
    /// <summary>
    /// Summary description for TranslatedMessage
    /// </summary>
    [Serializable]
    [XmlType(Namespace = "http://semnox.com/Messages")]
    public class TranslatedMessage
    {
        private int messageIdentifierField;
        private string messageTextField;
        private string translatedTextField;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// Default Constructor for TranslatedMessage
        /// </summary>
        public TranslatedMessage()
        {
            log.LogMethodEntry();
            messageIdentifierField = -1;
            messageTextField = "";
            translatedTextField = "";
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with full parameters for TranslatedMessage
        /// </summary>
        public TranslatedMessage(int messageId, string messageTxt, string translatedTxt)
        {
            log.LogMethodEntry(messageId,  messageTxt,  translatedTxt);
            messageIdentifierField = messageId;
            messageTextField = messageTxt;
            translatedTextField = translatedTxt;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set for MessageIdentifier field
        /// </summary>
        public int MessageIdentifier { get { return messageIdentifierField; } set { messageIdentifierField = value; } }

        /// <summary>
        /// Get/Set for MessageText field
        /// </summary>
        public string MessageText { get { return messageTextField; } set { messageTextField = value; } }
        /// <summary>
        /// Get/Set for TranslatedText field
        /// </summary>
        public string TranslatedText { get { return translatedTextField; } set { translatedTextField = value; } }
    }
}
