/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - AdjustmentTypes class - This would return adjustment types
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Sep-2020       Girish Kundar             Created : CenterEdge  REST API
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    
    public class AdjustmentTypesConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ToString(string value)
        {
            log.LogMethodEntry("value");
            switch (value)
            {
                case "addValue":
                    {
                        return "addValue";
                    }
                case "addMinute":
                    {
                        return "addMinute";
                    }
                case "addPrivilege":
                    {
                        return "addPrivilege";
                    }
                case "removeValue":
                    {
                        return "removeValue";
                    }
                case "removeMinute":
                    {
                        return "removeMinute";
                    }
                case "removePrivilege":
                    {
                        return "removePrivilege";
                    }
                default:
                    {
                        log.Error("Error :Not a valid AdjustmentTypes  ");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new ArgumentException("Not a valid AdjustmentTypes");
                    }
            }
        }
        public static string ToString(AdjustmentTypes value)
        {
            log.LogMethodEntry("value");
            switch (value)
            {
                case AdjustmentTypes.addMinutes:
                    {
                        return "addMinutes";
                    }
                case AdjustmentTypes.addPrivilege:
                    {
                        return "addPrivilege";
                    }
                case AdjustmentTypes.addValue:
                    {
                        return "addValue";
                    }
                case AdjustmentTypes.removeMinutes:
                    {
                        return "removeMinutes";
                    }
                case AdjustmentTypes.removePrivilege:
                    {
                        return "removePrivilege";
                    }
                case AdjustmentTypes.removeValue:
                    {
                        return "removeValue";
                    }
                default:
                    {
                        log.Error("Error :Not a valid credit plus type ");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new ArgumentException("Not a valid credit plus type");
                    }
            }
        }
    }
}
