/********************************************************************************************
 * Project Name - MessagesFunctions
 * Description  -MessagesFunctions class of the reports
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By        Remarks          
 *********************************************************************************************
 * 2.130        02-Aug-2021   Laster Menezes     Modified logger entries
 ********************************************************************************************/
using System.Collections.Generic;
using Telerik.Reporting.Expressions;
using System.Data;
using Semnox.Parafait.logging;

namespace Semnox.Parafait.Reports
{
    public enum AlertMessageTypes
    {
        /// <summary>
        /// info
        /// </summary>
        info,
        /// <summary>
        /// error
        /// </summary>
        error,
        /// <summary>
        /// warning
        /// </summary>
        warning,
        /// <summary>
        /// success
        /// </summary>
        success
    }

    /// <summary>
    /// MessagesFunctions class
    /// </summary>
    public static class MessagesFunctions
    {
        /// <summary>
        /// Semnox.Parafait.logging.Logger
        /// </summary>
        public static Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get  method of the messageList field
        /// </summary>
        public  static List<messageClass> messageList;

        /// <summary>
        /// CreateMessageList method
        /// </summary>
        /// <param name="LanguageID">LanguageID</param>
        public static void CreateMessageList(long LanguageID)
        {
            log.LogMethodEntry(LanguageID);
            messageList = new List<messageClass>();
            ReportsList eportsList = new ReportsList();
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler();
            DataTable dtMessageList = ReportsDataHandler.GetMessageList(LanguageID);
            if (dtMessageList != null)
            {
                if (dtMessageList.Rows.Count > 0)
                {
                    for (int i = 0; i < dtMessageList.Rows.Count; i++)
                    {
                        messageClass msg = new messageClass();
                        msg.Message = dtMessageList.Rows[i]["Message"].ToString();
                        msg.TranslatedMessage = dtMessageList.Rows[i]["TranslatedMessage"].ToString();
                        messageList.Add(msg);
                    }
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// CreateMessageList method
        /// </summary>
        /// <param name="language">language</param>
        /// <param name="siteId">siteId</param>
        public static void CreateMessageList(string language, int siteId)
        {
            log.LogMethodEntry(language,siteId);
            messageList = new List<messageClass>();
            ReportsList reportsList = new ReportsList();
            DataTable dtMessageList = reportsList.GetMessageList(language, siteId);
            if (dtMessageList != null)
            {
                if (dtMessageList.Rows.Count > 0)
                {
                    for (int i = 0; i < dtMessageList.Rows.Count; i++)
                    {
                        messageClass msg = new messageClass();
                        msg.Message = dtMessageList.Rows[i]["Message"].ToString();
                        msg.TranslatedMessage = dtMessageList.Rows[i]["TranslatedMessage"].ToString();
                        messageList.Add(msg);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// getMessage method
        /// </summary>
        /// <param name="Message">Message</param>
        /// <returns>returns string</returns>
        [Function(Category = "Messages", Namespace = "MessagesFunctions",
          Description = "Gets message corresponding Language ID passed")]
        public static string getMessage(string Message)
        {
            log.LogMethodEntry(Message);
            string message = findInList(Message);
            if (message == null)
            {
                message = Message;
            }
            log.LogMethodExit();
            return message;
        }


        /// <summary>
        /// findInList method
        /// </summary>
        /// <param name="msg">msg</param>
        /// <returns>returns string</returns>
        private static string findInList(string msg)
        {
            log.LogMethodEntry(msg);
            if(messageList!=null)
            foreach (messageClass msgObject in messageList)
            {
                if (string.Compare(msgObject.Message, msg, true) == 0)
                {                    
                    return msgObject.TranslatedMessage;
                }
            }
            log.LogMethodExit();
            return null;
        }
    }


    /// <summary>
    /// Default constructor
    /// </summary>
    public class messageClass
    {
        /// <summary>
        /// Message property
        /// </summary>
        public string Message;

        /// <summary>
        /// TranslatedMessage property
        /// </summary>
        public string TranslatedMessage;
    }
}
