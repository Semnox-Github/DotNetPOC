using Semnox.Core.Utilities;
using System;

namespace Semnox.Parafait.Discounts
{
    public enum DiscountApplicability
    {
        TRANSACTION,
        LINE
    }

    public class TransactionDiscountType
    {
        public static TransactionDiscountType GENERIC = new TransactionDiscountType("G");
        public static TransactionDiscountType SPECIFIC = new TransactionDiscountType("S");

        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string value;
        private TransactionDiscountType(string value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value;
        }

        public static implicit operator string(TransactionDiscountType transactionDiscountType)
        {
            return transactionDiscountType.value;
        }

        public static explicit operator TransactionDiscountType(string value)
        {
            log.LogMethodEntry(value);
            TransactionDiscountType result;
            if (value == GENERIC.value)
            {
                result = GENERIC;
            }
            else if(value == SPECIFIC.value)
            {
                result = SPECIFIC;
            }
            else
            {
                string errorMessage = "Invalid transaction discount type";
                log.LogMethodExit("Throwing Exception -" + errorMessage);
                throw new ParafaitApplicationException(errorMessage);
            }
            log.LogMethodExit(result);
            return result;
        }
    }

    public class DiscountApplicabilityConverter
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static DiscountApplicability FromString(string value)
        {
            log.LogMethodEntry();
            switch (value)
            {
                case "T":
                {
                    log.LogMethodExit(DiscountApplicability.TRANSACTION);
                    return DiscountApplicability.TRANSACTION;
                }
                case "L":
                {
                    log.LogMethodExit(DiscountApplicability.LINE);
                    return DiscountApplicability.LINE;
                }
                default:
                {
                    string errorMessage = "Invalid discount applicability value :" + value;
                    log.LogMethodExit(null, "Throwing Exception" + errorMessage);
                    throw new Exception(errorMessage);
                }
            }
        }

        public static string ToString(DiscountApplicability value)
        {
            log.LogMethodEntry(value);
            switch (value)
            {
                case DiscountApplicability.TRANSACTION:
                {
                    log.LogMethodExit("T");
                    return "T";
                }
                case DiscountApplicability.LINE:
                {
                    log.LogMethodExit("L");
                    return "L";
                }
                default:
                {
                    string errorMessage = "Invalid discount applicability value :" + value;
                    log.LogMethodExit(null, "Throwing Exception" + errorMessage);
                    throw new Exception(errorMessage);
                }
            }
        }
    }


}
