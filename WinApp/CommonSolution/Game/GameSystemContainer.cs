/********************************************************************************************
 * Project Name - GameSystemContainer  Class
 * Description  - GameSystemContainer class to get the data    
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System.Timers;

namespace Semnox.Parafait.Game
{
    class GameSystemContainer :BaseContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        private GameSystemContainer(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Regresh timer call to refresh the DTO lists 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public override void OnRefreshTimer(object o, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Clear the cached data in lists and dictionaries
        /// </summary>
        public override void ClearCache()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
    }
}
