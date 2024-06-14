/********************************************************************************************
 * Project Name - TemplateText 
 * Description  -BL class for Template Text
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     12-Dec-2020    Guru S A             Created for Subscription changes                                                                               
 ********************************************************************************************/


using System.Text;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// TemplateText
    /// </summary>
    public class TemplateText
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        StringBuilder text;
        /// <summary>
        /// TemplateText Constructor
        /// </summary>
        /// <param name="templateText"></param>
        public TemplateText(string templateText)
        {
            log.LogMethodEntry();
            text = new StringBuilder(templateText);
            log.LogMethodExit();
        }
        /// <summary>
        /// ReplaceKeyword
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="replaceValue"></param>
        public void ReplaceKeyword(string keyWord, string replaceValue)
        {
            log.LogMethodEntry(keyWord);
            text.Replace(keyWord, replaceValue);
            log.LogMethodExit();
        }
        /// <summary>
        /// Get Text Value
        /// </summary>
        public string GetTextValue
        {
            get
            {
                return text.ToString();
            }
        }
    }
}
