
/********************************************************************************************
 * Project Name - DigitalSignage
 * Description  - ThemeContainerFactory class to get route the request to remote or local based on the config
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
using Semnox.Parafait.Communication;
using System;
using System.Configuration;

namespace Semnox.Parafait.DigitalSignage
{
    public class ThemeContainerFactory
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static IThemeContainerDataService GetThemeContainerDataService(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IThemeContainerDataService themeContainerDataService = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                    themeContainerDataService = new RemoteThemeContainerDataService(executionContext);
            }
            else
            {
                themeContainerDataService = new LocalThemeContainerDataService(executionContext);
            }
            log.LogMethodExit(themeContainerDataService);
            return themeContainerDataService;
        }
    }
}
