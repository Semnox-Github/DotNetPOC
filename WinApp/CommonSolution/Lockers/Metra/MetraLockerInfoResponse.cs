﻿/********************************************************************************************
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
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using Semnox.Core.Utilities;
using Semnox.Parafait.logger;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// Business logic for the base class of parafait locker lock
    /// </summary>
    public class MetraLockerInfoResponse : MetraLockResponse
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// MetraLockerIssueResponse
        /// </summary>
        /// <param name="response"></param>
        public MetraLockerInfoResponse(string response)
            : base(response)
        {
            log.LogMethodEntry();
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
                case "5": returnMessage = "ERROR_ACCESS_DENIED"; break;
                default: returnMessage = "Unable to issue locker"; break;
            }
            log.LogMethodExit(returnMessage);
            return returnMessage;
        }
    }
}