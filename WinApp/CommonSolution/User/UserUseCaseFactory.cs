/********************************************************************************************
 * Project Name - User
 * Description  - Factory class to instantiate the user use cases 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         13-Nov-2020       Mushahid Faizan           Created : POS UI Redesign with REST API
**2.120.0    01-Apr-2021       Prajwal S                 Modified : Added new Use cases.
 *2.140.0    16-Aug-2021       Deeksha                   Modified : Provisional Shift changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Factory class to instantiate the user use cases
    /// </summary>
    public class UserUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public static IUserUseCases GetUserUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IUserUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteUserUseCases(executionContext);
            }
            else
            {
                result = new LocalUserUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        public static IUserRoleUseCases GetUserRoleUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IUserRoleUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteUserRoleUseCases(executionContext);
            }
            else
            {
                result = new LocalUserRoleUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        public static IAgentsUseCases GetAgentsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IAgentsUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteAgentsUseCases(executionContext);
            }
            else
            {
                result = new LocalAgentsUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        public static IAgentGroupsUseCases GetAgentGroupsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IAgentGroupsUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteAgentGroupsUseCases(executionContext);
            }
            else
            {
                result = new LocalAgentGroupsUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        public static IAttendanceUseCases GetAttendanceUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IAttendanceUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteAttendanceUseCases(executionContext);
            }
            else
            {
                result = new LocalAttendanceUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        public static IShiftConfigurationsUseCases GetShiftConfigurationUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IShiftConfigurationsUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteShiftConfigurationsUseCases(executionContext);
            }
            else
            {
                result = new LocalShiftConfigurationsUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        public static IAttendanceReaderUseCases GetAttendanceReaderUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IAttendanceReaderUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteAttendanceReaderUseCases(executionContext);
            }
            else
            {
                result = new LocalAttendanceReaderUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        public static IAttendanceRoleUseCases GetAttendanceRoleUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IAttendanceRoleUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteAttendanceRoleUseCases(executionContext);
            }
            else
            {
                result = new LocalAttendanceRoleUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        public static IPayConfigurationsUseCases GetPayConfigurationsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IPayConfigurationsUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemotePayConfigurationsUseCases(executionContext);
            }
            else
            {
                result = new LocalPayConfigurationsUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        public static IPayConfigurationMapUseCases GetPayConfigurationMapUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IPayConfigurationMapUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemotePayConfigurationMapUseCases(executionContext);
            }
            else
            {
                result = new LocalPayConfigurationMapUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        public static ILeaveTemplateUseCases GetLeaveTemplateUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ILeaveTemplateUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteLeaveTemplateUseCases(executionContext);
            }
            else
            {
                result = new LocalLeaveTemplateUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }


        public static IManagementFormAccessUseCases GetManagementFormAccessUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IManagementFormAccessUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteManagementFormAccessUseCases(executionContext);
            }
            else
            {
                result = new LocalManagementFormAccessUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        public static IWorkShiftUseCases GetWorkShiftUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IWorkShiftUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteWorkShiftUseCases(executionContext);
            }
            else
            {
                result = new LocalWorkShiftUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }


        public static IDepartmentUseCases GetDepartmentUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IDepartmentUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteDepartmentUseCases(executionContext);
            }
            else
            {
                result = new LocalDepartmentUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        public static ILeaveCycleUseCases GetLeaveCycleUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ILeaveCycleUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteLeaveCycleUseCases(executionContext);
            }
            else
            {
                result = new LocalLeaveCycleUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        public static ILeaveUseCases GetLeaveUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ILeaveUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteLeaveUseCases(executionContext);
            }
            else
            {
                result = new LocalLeaveUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        public static IUserToAttendanceRolesMapUseCases GetUserToAttendanceRolesMapUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IUserToAttendanceRolesMapUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteUserToAttendanceRolesMapUseCases(executionContext);
            }
            else
            {
                result = new LocalUserToAttendanceRolesMapUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }


        public static ILeaveActivityUseCases GetLeaveActivityUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ILeaveActivityUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteLeaveActivityUseCases(executionContext);
            }
            else
            {
                result = new LocalLeaveActivityUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        public static IDataAccessRuleUseCases GetDataAccessRuleUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IDataAccessRuleUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteDataAccessRuleUseCases(executionContext);
            }
            else
            {
                result = new LocalDataAccessRuleUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        public static IHolidayUseCases GetHolidayUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IHolidayUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteHolidayUseCases(executionContext);
            }
            else
            {
                result = new LocalHolidayUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// GetOverrideItemsUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IShiftUseCases GetShiftUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IShiftUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteShiftsUseCases(executionContext);
            }
            else
            {
                result = new LocalShiftUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}
