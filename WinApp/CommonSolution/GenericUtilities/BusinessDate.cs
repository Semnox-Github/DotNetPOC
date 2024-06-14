/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - Represents Business Date of a site
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
 *2.150.0     8-Mar-2022      Lakshminarayana     Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Site;
using System;
using System.Collections.Generic;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Represents Business Date of a site
    /// </summary>
    public class BusinessDate : ValueObject
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DateTime value;

        public BusinessDate(ExecutionContext executionContext)
            :this(executionContext.SiteId, ServerDateTime.Now)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public BusinessDate(int siteId, DateTime date)
        {
            log.LogMethodEntry(siteId, date);
            DateTime businessDate = date;
            int businessStartHour = ParafaitDefaultContainerList.GetParafaitDefault(siteId, "BUSINESS_DAY_START_TIME", 6);
            if (businessDate.Hour < businessStartHour)
            {
                businessDate = businessDate.AddDays(-1);
            }
            businessDate = businessDate.Date;
            businessDate = businessDate.AddHours(businessStartHour);
            value = businessDate;
            log.LogMethodExit();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return value;
        }

        public static implicit operator DateTime(BusinessDate d)
        {
            return d.value;
        }

        public DateTime Start
        {
            get
            {
                return value;
            }
        }

        public DateTime End
        {
            get
            {
                return value.AddDays(1);
            }
        }
    }
}
