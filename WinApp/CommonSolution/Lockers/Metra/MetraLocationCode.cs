/********************************************************************************************
 * Project Name - Parafait Locker Lock
 * Description  - Metra location code
 * 
 **************
 **Version Log
 **************
 *Version     Date               Modified By       Remarks          
 *********************************************************************************************
 *2.130.00    30-Aug-2021        Dakshakh raj      Created 
 ********************************************************************************************/

using System.Collections.Generic;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Device.Lockers
{
    public class MetraLocationCode : ValueObject
    {
        private int value;

        public MetraLocationCode(string zoneCode)
        {
            if (!string.IsNullOrEmpty(zoneCode))
            {
                value = (int)zoneCode[0];
            }
        }
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return value;
        }

        public int Value
        {
            get
            {
                return value;
            }
        }

        public static implicit operator int(MetraLocationCode metraLocationCode)
        {
            return metraLocationCode.Value;
        }
    }
}
