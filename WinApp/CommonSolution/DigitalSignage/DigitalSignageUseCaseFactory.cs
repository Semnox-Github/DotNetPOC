/********************************************************************************************
* Project Name - DigitalSignage
* Description  - DigitalSignageUseCaseFactory use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.140.00   14-Sep-2021   Prajwal S            Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DigitalSignage
{
    public  class DigitalSignageUseCaseFactory
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static IThemeUseCases GetThemeUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IThemeUseCases themeUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                themeUseCases = new RemoteThemeUseCases(executionContext);
            }
            else
            {
                themeUseCases = new LocalThemeUseCases(executionContext);
            }
            log.LogMethodExit(themeUseCases);
            return themeUseCases;
        }

        public static IDisplayPanelThemeMapUseCases GetDisplayPanelThemeMapUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IDisplayPanelThemeMapUseCases DisplayPanelThemeMapUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                DisplayPanelThemeMapUseCases = new RemoteDisplayPanelThemeMapUseCases(executionContext);
            }
            else
            {
                DisplayPanelThemeMapUseCases = new LocalDisplayPanelThemeMapUseCases(executionContext);
            }
            log.LogMethodExit(DisplayPanelThemeMapUseCases);
            return DisplayPanelThemeMapUseCases;
        }

        public static IDisplayPanelUseCases GetDisplayPanelUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IDisplayPanelUseCases DisplayPanelUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                DisplayPanelUseCases = new RemoteDisplayPanelUseCases(executionContext);
            }
            else
            {
                DisplayPanelUseCases = new LocalDisplayPanelUseCases(executionContext);
            }
            log.LogMethodExit(DisplayPanelUseCases);
            return DisplayPanelUseCases;
        }

        public static ISignagePatternUseCases GetSignagePatternUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ISignagePatternUseCases SignagePatternUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                SignagePatternUseCases = new RemoteSignagePatternUseCases(executionContext);
            }
            else
            {
                SignagePatternUseCases = new LocalSignagePatternUseCases(executionContext);
            }
            log.LogMethodExit(SignagePatternUseCases);
            return SignagePatternUseCases;
        }

        public static ITickerUseCases GetTickerUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ITickerUseCases TickerUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                TickerUseCases = new RemoteTickerUseCases(executionContext);
            }
            else
            {
                TickerUseCases = new LocalTickerUseCases(executionContext);
            }
            log.LogMethodExit(TickerUseCases);
            return TickerUseCases;
        }

        public static IScreenSetupUseCases GetScreenSetupUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IScreenSetupUseCases ScreenSetupUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                ScreenSetupUseCases = new RemoteScreenSetupUseCases(executionContext);
            }
            else
            {
                ScreenSetupUseCases = new LocalScreenSetupUseCases(executionContext);
            }
            log.LogMethodExit(ScreenSetupUseCases);
            return ScreenSetupUseCases;
        }

        public static IEventUseCases GetEventUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IEventUseCases EventUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                EventUseCases = new RemoteEventUseCases(executionContext);
            }
            else
            {
                EventUseCases = new LocalEventUseCases(executionContext);
            }
            log.LogMethodExit(EventUseCases);
            return EventUseCases;
        }

        public static IDSLookupUseCases GetDSLookupUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IDSLookupUseCases DSLookupUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                DSLookupUseCases = new RemoteDSLookupUseCases(executionContext);
            }
            else
            {
                DSLookupUseCases = new LocalDSLookupUseCases(executionContext);
            }
            log.LogMethodExit(DSLookupUseCases);
            return DSLookupUseCases;
        }
    }
}
