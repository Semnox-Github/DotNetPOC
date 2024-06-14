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

using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// Business logic for the base class of parafait locker lock
    /// </summary>
    public class MetraLockResponse
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected bool success = false;
        protected Dictionary<string, string> dataDictionary = new Dictionary<string, string>();

        /// <summary>
        /// MetraLockerIssueResponse
        /// </summary>
        /// <param name="response"></param>
        public MetraLockResponse(string response)
        {
            log.LogMethodEntry(response);
            if (!string.IsNullOrEmpty(response))
            {
                XDocument doc = XDocument.Parse(response);

                foreach (XElement element in doc.Descendants().Where(p => p.HasElements == false))
                {
                    int keyInt = 0;
                    string keyName = element.Name.LocalName;

                    while (dataDictionary.ContainsKey(keyName))
                    {
                        keyName = element.Name.LocalName + "_" + keyInt++;
                    }

                    dataDictionary.Add(keyName, element.Value);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Success or error status
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return success;
            }
        }
    }
}
