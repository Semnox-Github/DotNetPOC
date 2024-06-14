/********************************************************************************************
 * Project Name - TemplateKeywordFormatter 
 * Description  -BL class for Template Keyword Formatter
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     12-Dec-2020    Guru S A             Created for Subscription changes                                                                               
 ********************************************************************************************/


using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// TemplateKeywordFormatter
    /// </summary>
    public class TemplateKeywordFormatter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<string, string> keywordValueMap;
        /// <summary>
        /// TemplateKeywordFormatter
        /// </summary>
        public TemplateKeywordFormatter()
        {
            log.LogMethodEntry();
            keywordValueMap = new Dictionary<string, string>();
            log.LogMethodExit();
        }
        /// <summary>
        /// Add
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="value"></param>
        public void Add(string keyword, string value)
        {
            if (keywordValueMap.ContainsKey(keyword) == false)
            {
                keywordValueMap.Add(keyword, value);
            }
            else
            {
                keywordValueMap[keyword] = value;
            }
        }
        /// <summary>
        /// Format
        /// </summary>
        /// <param name="templateText"></param>
        /// <returns></returns>
        public virtual string Format(TemplateText templateText)
        {
            log.LogMethodEntry(templateText);
            foreach (string textKey in keywordValueMap.Keys)
            {
                templateText.ReplaceKeyword(textKey, keywordValueMap[textKey]);
            }
            string formattedContent = templateText.GetTextValue;
            log.LogMethodExit(formattedContent);
            return formattedContent;
        }
        /// <summary>
        /// FormatedTemplateText
        /// </summary>
        /// <param name="templateText"></param>
        /// <returns></returns>
        public TemplateText FormatedTemplateText (TemplateText templateText)
        {
            log.LogMethodEntry(templateText);
            foreach (string textKey in keywordValueMap.Keys)
            {
                templateText.ReplaceKeyword(textKey, keywordValueMap[textKey]);
            } 
            log.LogMethodExit(templateText);
            return templateText;
        }
    }
}
