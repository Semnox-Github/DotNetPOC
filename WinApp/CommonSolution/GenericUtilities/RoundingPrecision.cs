/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - RoundingPrecision 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Core.GenericUtilities
{
    public class RoundingPrecision : ValueObject
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly int value;

        public RoundingPrecision(int value)
        {
            log.LogMethodEntry(value);
            this.value = value;
            log.LogMethodExit();
        }

        public RoundingPrecision(string amountFormat)
        {            
            log.LogMethodEntry(amountFormat);
            value = 0;
            if (string.IsNullOrWhiteSpace(amountFormat))
            {
                log.LogMethodExit();
                return;
            }
            
            try
            {
                if (amountFormat.Contains("#"))
                {
                    int pos = amountFormat.IndexOf(".", StringComparison.Ordinal);
                    if (pos >= 0)
                    {
                        value = amountFormat.Length - pos - 1;
                    }
                    else
                    {
                        value = 0;
                    }
                }
                else
                {
                    if (amountFormat.Length > 1)
                        value = Convert.ToInt32(amountFormat.Substring(1));
                }
            }
            catch(Exception ex)
            {
                log.Error("Error occured while executing RoundingPrecision() " + ex.Message);
                value = 0;
            }
            log.LogMethodExit();
        }

        public static implicit operator RoundingPrecision(string value)
        {
            log.LogMethodEntry(value);
            RoundingPrecision roundingPrecision = new RoundingPrecision(value);
            log.LogMethodExit(roundingPrecision);
            return roundingPrecision;
        }

        public static implicit operator int(RoundingPrecision value)
        {
            log.LogMethodEntry(value);
            log.LogMethodExit(value);
            return value.value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            log.LogMethodEntry();
            log.LogMethodExit(value);
            yield return value;
        }
    }
}
