/********************************************************************************************
 * Project Name - Utilities
 * Description  - Created to have the derived methods for all the entity level container classes.   
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Timers;

namespace Semnox.Core.Utilities
{
    public class BaseContainer 
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public virtual void OnRefreshTimer(object o, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        public virtual void ClearCache()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public virtual void RebuildCache()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
    }
}
