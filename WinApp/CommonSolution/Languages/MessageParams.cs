/********************************************************************************************
 * Project Name - MessageParams
 * Description  - Parameters to passed for get messages
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        05-Jan-2017   Vinayaka V          Created 
 *2.70.2        12-Aug-2019   Deeksha             Added logger methods.
 ********************************************************************************************/

using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Languages
{
    /// <summary>
    /// This is the Messages params class. This acts as data holder for the Messages parameters
    /// </summary>
    public class MessageParams
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int messageId;
        string messageKey;
        int languageId;
        int site_Id;
        string[] messageKeyList;
        List<int> messageKeysList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MessageParams()
        {
            log.LogMethodEntry();
            this.messageId = -1;
            this.messageKey = "";
            this.languageId = -1;
            this.site_Id = -1;
            this.messageKeyList = new string[]{};
            this.messageKeysList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with parameters
        /// </summary>
        public MessageParams(int messageId, string messageKey, int languageId, int site_Id)
        {
            log.LogMethodEntry(messageId, messageKey, languageId, site_Id);
            this.messageId = messageId;
            this.messageKey = messageKey;
            this.languageId = languageId;
            this.site_Id = site_Id;
            log.LogMethodExit();
        }

        
        /// <summary>
        /// constructor with parameters
        /// </summary>
        public MessageParams(int messageId, int languageId, int site_Id)
        {
            log.LogMethodEntry(messageId, languageId, site_Id);
            this.messageId = messageId;
            this.messageKey = "";
            this.languageId = languageId;
            this.site_Id = site_Id;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with parameters
        /// </summary>
        public MessageParams(string messageKey, int languageId, int site_Id)
        {
            log.LogMethodEntry(messageId, languageId, site_Id);
            this.messageId = -1;
            this.messageKey = messageKey;
            this.languageId = languageId;
            this.site_Id = site_Id;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with parameters
        /// </summary>
        public MessageParams(string[] messageKeyList, int languageId, int site_Id)
        {
            log.LogMethodEntry(messageKeyList, languageId, site_Id);
            this.messageId = -1;
            this.messageKey = "";
            this.messageKeyList = messageKeyList;
            this.languageId = languageId;
            this.site_Id = site_Id;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with parameters
        /// </summary>
        public MessageParams(List<int> messageKeysList, int languageId, int site_Id)
        {
            log.LogMethodEntry(messageKeysList, languageId, site_Id);
            this.messageId = -1;
            this.messageKey = "";
            this.messageKeysList = messageKeysList;
            this.languageId = languageId;
            this.site_Id = site_Id;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the messageId field
        /// </summary>
        [DisplayName("MessageId")]
        public int MessageId { get { return messageId; } set { messageId = value; } }

        /// <summary>
        /// Get/Set method of the messageKey field
        /// </summary>
        [DisplayName("MessageKey")]
        public string MessageKey { get { return messageKey; } set { messageKey = value; } }

        /// <summary>
        /// Get/Set method of the languageId field
        /// </summary>
        [DisplayName("LanguageId")]
        public int LanguageId { get { return languageId; } set { languageId = value; } }

        /// <summary>
        /// Get/Set method of the site_Id field
        /// </summary>
        [DisplayName("Site_Id")]
        public int Site_Id { get { return site_Id; } set { site_Id = value; } }

        /// <summary>
        /// Get/Set method of the messageKeyList field
        /// </summary>
        [DisplayName("MessageKeyList")]
        public string[] MessageKeyList { get { return messageKeyList; } set { messageKeyList = value; } }

        /// <summary>
        /// Get/Set method of the messageKeyList field
        /// </summary>
        [DisplayName("MessageKeysList")]
        public List<int> MessageKeysList { get { return messageKeysList; } set { messageKeysList = value; } }


    }
}
