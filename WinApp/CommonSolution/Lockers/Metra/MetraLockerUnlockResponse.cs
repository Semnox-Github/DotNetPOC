/********************************************************************************************
 * Project Name - Parafait Locker Lock
 * Description  - The bussiness logic for parafait locker lock
 * 
 **************
 **Version Log
 **************
 *Version     Date               Modified By       Remarks          
 *********************************************************************************************
 *2.130.00    26-May-2021        Dakshakh raj      Created 
 ********************************************************************************************/

using System;
using System.Linq;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// Business logic for the base class of parafait locker lock
    /// </summary>
    public class MetraLockerUnlockResponse :MetraLockResponse
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// MetraLockerIssueResponse
        /// </summary>
        /// <param name="response"></param>
        public MetraLockerUnlockResponse(string response)
            : base(response)
        {
            log.LogMethodEntry(response);
            if (dataDictionary != null && dataDictionary.Any())
            {
                string resultCode = string.Empty;
                dataDictionary.TryGetValue("result", out resultCode);
                if (resultCode != null)
                {
                    if (resultCode.Equals("0"))
                    {
                        success = true;
                    }
                    else
                    {
                        throw new Exception(resultCode + ", " + GetErrorMessage(resultCode));
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get Error Message
        /// </summary>
        /// <param name="resultCode"></param>
        /// <returns></returns>
        public string GetErrorMessage(string resultCode)
        {
            log.LogMethodEntry(resultCode);
            string returnMessage = "";
            switch (resultCode)
            {
                case "87": returnMessage = "ERROR_INVALID_PARAMETER"; break;
                case "85": returnMessage = "ERROR_ALREADY_ASSIGNED"; break;
                case "21": returnMessage = "ERROR_NOT_READY"; break;
                case "5": returnMessage = "ERROR_ACCESS_DENIED"; break;
                case "1": returnMessage = "ERROR_INVALID_FUNCTION"; break;
                default: returnMessage = "Unable to issue locker"; break;
            }
            log.LogMethodExit(returnMessage);
            return returnMessage;
        }
    }
}
