/********************************************************************************************
 * Class Name - POS                                                                         
 * Description - EOD BL
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019            Deeksha        Added logger methods.
 *2.130.4     22-Feb-2022   Mathew Ninan    Modified DateTime to ServerDateTime 
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.POS
{
    public class EODBL
    {
        Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public EODBL(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            utilities = _Utilities;
            log.LogMethodExit();
        }

        public bool IsEndOfDayPerformedForCurrentBusinessDay()
        {
            log.LogMethodEntry();
            POSMachineList posMachineList = new POSMachineList(utilities.ExecutionContext);
            POSMachineDTO posMachineDTO = new POSMachineDTO();
            posMachineDTO = posMachineList.GetPOSMachine(utilities.ParafaitEnv.POSMachineId);
            if (posMachineDTO != null)
            {
                DateTime businessStartDate = posMachineDTO.DayBeginTime = ServerDateTime.Now.Date.AddHours(Convert.ToInt32(utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME")));
                DateTime businessEndDate;
                if (businessStartDate.CompareTo(ServerDateTime.Now) == 1)
                {
                    businessStartDate = businessStartDate.AddDays(-1);
                }
                businessEndDate = businessStartDate.AddDays(1);
                if (posMachineDTO.DayEndTime.CompareTo(businessStartDate) >= 0 && posMachineDTO.DayEndTime.CompareTo(businessEndDate) <= 0)
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
