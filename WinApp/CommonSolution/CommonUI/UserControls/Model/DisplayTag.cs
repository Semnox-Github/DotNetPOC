/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Header items model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda           Created for POS UI Redesign 
 *2.110.0     25-Nov-2020   Raja Uthanda           to add button style
 ********************************************************************************************/
using System.Windows;

namespace Semnox.Parafait.CommonUI
{
    public enum DisplayTagType
    {
        None = 0,
        Button = 1
    }

    public class DisplayTag : ViewModelBase
    {
        #region Members
        private DisplayTagType type;

        private string text;
        private TextSize textSize = TextSize.XSmall;
        private FontWeight fontWeight = FontWeights.Normal;
        private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public HorizontalAlignment HorizontalAlignment
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(horizontalAlignment);
                return horizontalAlignment;
            }
            set
            {
                log.LogMethodEntry(horizontalAlignment, value);
                SetProperty(ref horizontalAlignment, value);
                log.LogMethodExit(horizontalAlignment);
            }
        }
        public DisplayTagType Type
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(type);
                return type;
            }
            set
            {
                log.LogMethodEntry(type, value);
                SetProperty(ref type, value);
                log.LogMethodExit(type);
            }
        }

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
                log.LogMethodEntry();
                SetProperty(ref text, value);
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
                log.LogMethodEntry();
                SetProperty(ref fontWeight, value);
                log.LogMethodExit(fontWeight);
            }
        }

        public TextSize TextSize
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(textSize);
                return textSize;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref textSize, value);
                log.LogMethodExit(textSize);
            }
        }
        #endregion
    }
}
