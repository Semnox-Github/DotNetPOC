/********************************************************************************************
 * Project Name - AppNotificationContentBuilder 
 * Description  -BL class to build message content for app notifications
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     12-Dec-2020    Guru S A             Created for Subscription changes  
 *2.130.5     11-Mar-2022    Nitin Pai            Added new tags in app template for images
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Semnox.Parafait.Communication
{
    /// <summary>
    /// AppNotificationContentBuilder
    /// </summary>
    public class AppNotificationContentBuilder
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private ParafaitFunctionEvents parafaitFunctionEvents;
        private EmailTemplateDTO appNotificationTemplateDTO;
        private const string MESSAGING_TRIGGER_TYPE_CODE_CARD_VALIDITY = "CARD";
        private const string MESSAGING_TRIGGER_TYPE_CODE_PURCHASE = "TRX";
        private const string MESSAGING_TRIGGER_TYPE_CODE_REDEMPTION = "REDEMPTION";
        private const string NOTIFICATION_TYPE_CODE_PROMOTION = "PROMOTION";
        /// <summary>
        /// AppNotificationType
        /// </summary>
        public enum AppNotificationType
        {
            /// <summary>
            /// None
            /// </summary>
            NONE,
            /// <summary>
            /// CARD_VALIDITY
            /// </summary>
            CARD_VALIDITY,
            /// <summary>
            /// PURCHAGSE_TRIGGER
            /// </summary>
            PURCHAGSE_TRIGGER,
            /// <summary>
            /// REDEMPTION_TRIGGER
            /// </summary>
            REDEMPTION_TRIGGER,
            /// <summary>
            /// PROMOTION
            /// </summary>
            PROMOTION
        }

        /// <summary>
        /// SourceEnumFromString for NotificationType
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static AppNotificationType SourceEnumFromString(String status)
        {
            AppNotificationType returnValue = AppNotificationType.NONE;
            switch (status)
            {
                case MESSAGING_TRIGGER_TYPE_CODE_CARD_VALIDITY:
                    returnValue = AppNotificationType.CARD_VALIDITY;
                    break;
                case MESSAGING_TRIGGER_TYPE_CODE_PURCHASE:
                    returnValue = AppNotificationType.PURCHAGSE_TRIGGER;
                    break;
                case MESSAGING_TRIGGER_TYPE_CODE_REDEMPTION:
                    returnValue = AppNotificationType.REDEMPTION_TRIGGER;
                    break;
                case NOTIFICATION_TYPE_CODE_PROMOTION:
                    returnValue = AppNotificationType.PROMOTION;
                    break;
                default:
                    returnValue = AppNotificationType.NONE;
                    break;
            }
            return returnValue;
        }

        /// <summary>
        /// SourceEnumToString for NotificationType
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string SourceEnumToString(AppNotificationType status)
        {
            String returnString = "";
            switch (status)
            {
                case AppNotificationType.CARD_VALIDITY:
                    returnString = MESSAGING_TRIGGER_TYPE_CODE_CARD_VALIDITY;
                    break;
                case AppNotificationType.PURCHAGSE_TRIGGER:
                    returnString = MESSAGING_TRIGGER_TYPE_CODE_PURCHASE;
                    break;
                case AppNotificationType.REDEMPTION_TRIGGER:
                    returnString = MESSAGING_TRIGGER_TYPE_CODE_REDEMPTION;
                    break;
                case AppNotificationType.PROMOTION:
                    returnString = NOTIFICATION_TYPE_CODE_PROMOTION;
                    break;
                default:
                    returnString = "";
                    break;
            }
            return returnString;
        }
        /// <summary>
        /// AppNotificationContentBuilder
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="parafaitFunctionEvents"></param>
        public AppNotificationContentBuilder(ExecutionContext executionContext, ParafaitFunctionEvents parafaitFunctionEvents)
        {
            log.LogMethodEntry(executionContext, parafaitFunctionEvents);
            this.executionContext = executionContext;
            this.parafaitFunctionEvents = parafaitFunctionEvents;
            LoadNotificationTemplate();
            log.LogMethodExit();
        }
        private void LoadNotificationTemplate()
        {
            log.LogMethodEntry();
            appNotificationTemplateDTO = null;
            // Get the app notification template 
            EmailTemplateListBL emailTemplateListBL = new EmailTemplateListBL(executionContext);
            List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.NAME, "APP_NOTIFICATION_TEMPLATE"));
            List<EmailTemplateDTO> emailTemplateDTOList = emailTemplateListBL.GetEmailTemplateDTOList(searchParameters);
            if (emailTemplateDTOList != null && emailTemplateDTOList.Any())
            {
                appNotificationTemplateDTO = emailTemplateDTOList[0];
            }
            else
            {
                log.Info("APP_NOTIFICATION_TEMPLATE is not defined");
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// FormatAppNotificationContent
        /// </summary>
        /// <param name="formattedSubject"></param>
        /// <param name="formattedContent"></param>
        /// <param name="notificationType"></param>
        /// <param name="cardNumber"></param>
        /// <param name="transactionId"></param>
        /// <param name="redemptionId"></param>
        /// <param name="promotionId"></param>
        /// <returns></returns>
        public string FormatAppNotificationContent(string formattedSubject, string formattedContent, AppNotificationType notificationType,
                                          string cardNumber, string transactionId, string promotionId)
        {
            log.LogMethodEntry(formattedSubject, formattedContent, notificationType, cardNumber, transactionId, promotionId);
            String tempString = string.Empty;

            String imageURL = "";
            String redirectURL = "";
            String redirectText = "";
            String imageWidth = "";
            String imageHeiget = "";
            String subjectImageURL = "";
            if (appNotificationTemplateDTO != null)
            {
                tempString = appNotificationTemplateDTO.EmailTemplate;
                String separator = "|";

                List<String> searchStringList = new List<string>() { "IMAGEURL" + separator, "REDIRECTURL" + separator,
                                                                      "REDIRECTTEXT" + separator, "IMAGEWIDTH" + separator, "IMAGEHEIGHT" + separator };

                foreach (string searchString in searchStringList)
                {
                    if (formattedContent.ToUpper().Contains(searchString))
                    {
                        int startPosition = formattedContent.ToUpper().IndexOf(searchString);
                        int endPosition = formattedContent.IndexOf(separator, startPosition + searchString.Length);
                        String tempSubString = "";

                        if (startPosition > 0 && endPosition > 0)
                        {
                            tempSubString = formattedContent.Substring(startPosition, endPosition - startPosition + 1);
                            formattedContent = formattedContent.Replace(tempSubString, "");
                            tempSubString = Regex.Replace(tempSubString, searchString, "", RegexOptions.IgnoreCase);
                            tempSubString = tempSubString.Replace("|", "");
                        }

                        if (searchString.Contains("IMAGEURL"))
                        {
                            imageURL = tempSubString;
                        }
                        else if (searchString.Contains("REDIRECTURL"))
                        {
                            redirectURL = tempSubString;
                        }
                        else if (searchString.Contains("REDIRECTTEXT"))
                        {
                            redirectText = tempSubString;
                        }
                        else if (searchString.Contains("IMAGEWIDTH"))
                        {
                            imageWidth = tempSubString;
                        }
                        else if (searchString.Contains("IMAGEHEIGHT"))
                        {
                            imageHeiget = tempSubString;
                        }
                    }
                }

                String searchSubjectString = "SUBJECTIMAGEURL" + separator;
                if (!String.IsNullOrEmpty(formattedSubject) && formattedSubject.ToUpper().Contains(searchSubjectString))
                {
                    int startPosition = formattedSubject.ToUpper().IndexOf(searchSubjectString);
                    int endPosition = formattedSubject.IndexOf(separator, startPosition + searchSubjectString.Length);

                    if (startPosition > 0 && endPosition > 0)
                    {
                        subjectImageURL = formattedSubject.Substring(startPosition, endPosition - startPosition + 1);
                        formattedSubject = formattedSubject.Replace(searchSubjectString, "");
                        subjectImageURL = Regex.Replace(subjectImageURL, searchSubjectString, "", RegexOptions.IgnoreCase);
                        subjectImageURL = subjectImageURL.Replace("|", "");
                    }
                }

                tempString = tempString.Replace("@notificationSubject", formattedSubject);
                tempString = tempString.Replace("@notificationBody", formattedContent);
                if (string.IsNullOrEmpty(cardNumber) == false)
                {
                    tempString = tempString.Replace("@CardNumber", cardNumber);
                }
                if (string.IsNullOrEmpty(transactionId) == false)
                {
                    tempString = tempString.Replace("@transactionId", transactionId);
                }
                tempString = tempString.Replace("@notificationType", SourceEnumToString(notificationType));
                tempString = tempString.Replace("@imageURL", imageURL);
                tempString = tempString.Replace("@redirectURL", redirectURL);
                tempString = tempString.Replace("@redirectText", redirectText);
                tempString = tempString.Replace("@imageWidth", imageWidth);
                tempString = tempString.Replace("@imageHeiget", imageHeiget);
                tempString = tempString.Replace("@subjectImageURL", subjectImageURL);
            }
            log.LogMethodExit(tempString);
            return tempString;
        }
    }
}
