/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common -model for navigation user control
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.130.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.CommonUI
{
    public class NavigationTag : ViewModelBase
    {
        #region Members
        private string key;
        private string text;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public string Key
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(key);
                return key;
            }
            set
            {
                log.LogMethodEntry(key, value);
                SetProperty(ref key, value);
                log.LogMethodExit(key);
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
                log.LogMethodEntry(text, value);
                SetProperty(ref text, value);
                log.LogMethodExit(text);
            }
        }
        #endregion
                
        #region Constructor
        public NavigationTag()
        {
            log.LogMethodEntry();
            key = string.Empty;
            text = string.Empty;
            log.LogMethodExit();
        }
        public NavigationTag(string key, string text)
        {
            log.LogMethodEntry(key,text);
            this.key = key;
            this.text = text;
            log.LogMethodExit();
        }
        #endregion
    }
}
