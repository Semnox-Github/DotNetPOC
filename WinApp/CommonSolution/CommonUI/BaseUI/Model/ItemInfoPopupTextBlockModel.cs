/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Redemption - model for item info popup text block
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     25-Nov-2020   Siba Maharana            Created for POS UI Redesign 
 ********************************************************************************************/

using System.Windows;

namespace Semnox.Parafait.CommonUI
{
    public class ItemInfoPopupTextBlockModel
    {
        #region Members
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string text;
        private FontWeight fontWeight;
        #endregion

        #region Properties
        public string Text
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(text);
                return text;
            }
            set
            {
                log.LogMethodEntry(text, value);
                text = value;
                log.LogMethodExit(text);
            }
        }

        public FontWeight FontWeight
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(fontWeight);
                return fontWeight;
            }
            set
            {
                log.LogMethodEntry(fontWeight, value);
                fontWeight = value;
                log.LogMethodExit(fontWeight);
            }
        }
        #endregion

        #region Constructor
        public ItemInfoPopupTextBlockModel()
        {
            log.LogMethodEntry();
            this.text = string.Empty;
            this.fontWeight = FontWeights.Normal;
            log.LogMethodExit();
        }

        public ItemInfoPopupTextBlockModel(string text, FontWeight fontWeight)
        {
            log.LogMethodEntry(text, fontWeight);
            this.text = text;
            this.fontWeight = fontWeight;
            log.LogMethodExit();
        }
        #endregion
    }
}
