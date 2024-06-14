/********************************************************************************************
 * Project Name - MaintenanceWindow
 * Description  - Business logic for MaintenanceWindow
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        18-Apr-2019      Lakshminarayana     Created 
 *2.155.0     20-Jul-2023      Mathew N       Contains method updated to use parameter value
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Represents the Maintenance Window of the system
    /// </summary>
    public class MaintenanceWindow
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly int maintenanceStartHour;
        private readonly int maintenanceEndHour;
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="configuration"></param>
        public MaintenanceWindow(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            maintenanceStartHour = GetMaintainanceHour("MAINTENANCE_START_HOUR", 23);
            maintenanceEndHour = GetMaintainanceHour("MAINTENANCE_END_HOUR", 10);
            log.LogMethodExit();
        }

        private int GetMaintainanceHour(string defaultValueName, int defaultValue)
        {
            log.LogMethodEntry(defaultValueName, defaultValue);
            string defaultValueString = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, defaultValueName);
            if (string.IsNullOrWhiteSpace(defaultValueString))
            {
                log.LogMethodExit(defaultValue, defaultValueName + " is not defined");
                return defaultValue;
            }
            int result;
            if (int.TryParse(defaultValueString, out result) == false)
            {
                result = defaultValue;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns whether the given time falls under maintenance window
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public bool Contains(DateTime dateTime)
        {
            log.LogMethodEntry(dateTime);
            int hour = dateTime.Hour;
            if (maintenanceEndHour > maintenanceStartHour)
            {
                if (hour >= maintenanceStartHour && hour < maintenanceEndHour)
                {
                    log.LogMethodExit(true);
                    return true;
                }
            }
            else if (maintenanceEndHour < maintenanceStartHour)
            {
                if (hour >= maintenanceStartHour || hour < maintenanceEndHour)
                {
                    log.LogMethodExit(true);
                    return true;
                }
            }
            log.LogMethodExit(false);
            return false;
        }
    }
}
