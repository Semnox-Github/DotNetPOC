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
    public class MetraLockerCardInfoResponse : MetraLockResponse
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// MetraLockerIssueResponse
        /// </summary>
        /// <param name="response"></param>
        public MetraLockerCardInfoResponse(string response)
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
                        //success = false;
                        log.LogMethodExit(resultCode + ", " + GetErrorMessage(resultCode));
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Success or error status
        /// </summary>
        public int LockerNumber
        {
            get
            {
                int result = -1;
                if (dataDictionary != null && dataDictionary.Any() && dataDictionary.Keys.Contains("number"))
                {
                    result = Convert.ToInt32(dataDictionary.FirstOrDefault(x => x.Key == "number").Value);
                }
                return result;
            }
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
                case "5": returnMessage = "ERROR_ACCESS_DENIED"; break;
                default: returnMessage = "Unable to issue locker"; break;
            }
            log.LogMethodExit(returnMessage);
            return returnMessage;
        }
    }
}
