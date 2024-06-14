/********************************************************************************************
 * Project Name - GenericUtilities                                                                          
 * Description  - Base class for value objects.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************* 
 *2.50.0      14-June-2019   Lakshminarayana      Created
 *2.70.2        10-Aug-2019    Deeksha              Added logger methods.
 ********************************************************************************************/

using System.Collections.Generic;
using System.Linq;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// Base class for value objects.
    /// </summary>
    public abstract class ValueObject
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static bool EqualOperator(ValueObject left, ValueObject right)
        {
            log.LogMethodEntry(left, right);
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                log.LogMethodExit(false);
                return false;
            }
            bool returnvalue = ReferenceEquals(left, null) || left.Equals(right);
            log.LogMethodExit(returnvalue);
            return returnvalue;
        }

        private static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            log.LogMethodEntry(left, right);
            bool returnvalue = !(EqualOperator(left, right));
            log.LogMethodEntry(returnvalue);
            return returnvalue;
        }

        protected abstract IEnumerable<object> GetAtomicValues();

        public override bool Equals(object obj)
        {
            log.LogMethodEntry(obj);
            if (obj == null || obj.GetType() != GetType())
            {
                log.LogMethodEntry(false);
                return false;
            }

            ValueObject other = (ValueObject)obj;
            using (IEnumerator<object> thisValues = GetAtomicValues().GetEnumerator())
            {
                using (IEnumerator<object> otherValues = other.GetAtomicValues().GetEnumerator())
                {
                    while (thisValues.MoveNext() && otherValues.MoveNext())
                    {
                        if (ReferenceEquals(thisValues.Current, null) ^
                            ReferenceEquals(otherValues.Current, null))
                        {
                            log.LogMethodEntry(false);
                            return false;
                        }

                        if (thisValues.Current != null &&
                            !thisValues.Current.Equals(otherValues.Current))
                        {
                            log.LogMethodEntry(false);
                            return false;
                        }
                    }
                    bool returnvalue = !thisValues.MoveNext() && !otherValues.MoveNext();
                    log.LogMethodEntry(returnvalue);
                    return returnvalue;
                }
            }
        }

        public override int GetHashCode()
        {
            log.LogMethodEntry();
            int returnValue = GetAtomicValues()
             .Select(x => x != null ? x.GetHashCode() : 0)
             .Aggregate((x, y) => x ^ y);
            log.LogMethodEntry(returnValue);
            return returnValue;
        }

        public static bool operator ==(ValueObject valueObject1, ValueObject valueObject2)
        {
            log.LogMethodEntry(valueObject1, valueObject2);
            bool returnValue = EqualOperator(valueObject1, valueObject2);
            log.LogMethodEntry(returnValue);
            return returnValue;
        }

        public static bool operator !=(ValueObject valueObject1, ValueObject valueObject2)
        {
            log.LogMethodEntry(valueObject1, valueObject2);
            bool returnValue = NotEqualOperator(valueObject1, valueObject2);
            log.LogMethodEntry(returnValue);
            return returnValue;
        }
    }
}
