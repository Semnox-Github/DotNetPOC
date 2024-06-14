using System;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Operator Factory
    /// </summary>

    public class OperatorFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the operator based on the operator type
        /// </summary>
        /// <param name="operatorType"></param>
        /// <returns></returns>
        public static IOperator GetOperator(Operator operatorType)
        {
            log.LogMethodEntry(operatorType);
            IOperator @operator = null;
            switch (operatorType)
            {
                case Operator.EQUAL_TO:
                    @operator = new SingleValueOperator("=", "Equal To");
                    break;
                case Operator.NOT_EQUAL_TO:
                    @operator = new SingleValueOperator("!=", "Not Equal to");
                    break;
                case Operator.LIKE:
                    @operator = new LikeOperator("LIKE", "Contains");
                    break;
                case Operator.NOT_LIKE:
                    @operator = new LikeOperator("NOT LIKE", "Doesn't Contain");
                    break;
                case Operator.GREATER_THAN:
                    @operator = new SingleValueOperator(">", "Greater Than");
                    break;
                case Operator.LESSER_THAN:
                    @operator = new SingleValueOperator("<", "Less Than");
                    break;
                case Operator.GREATER_THAN_OR_EQUAL_TO:
                    @operator = new SingleValueOperator(">=", "Greater Than Equal To");
                    break;
                case Operator.LESSER_THAN_OR_EQUAL_TO:
                    @operator = new SingleValueOperator("<=", "Less Than Equal To");
                    break;
                case Operator.BETWEEN:
                    @operator = new BetweenOperator();
                    break;
                case Operator.IN:
                    @operator = new InOperator();
                    break;
                case Operator.IS_NULL:
                    @operator = new NoParameterOperator("IS NULL", "Is Null");
                    break;
                case Operator.IS_NOT_NULL:
                    @operator = new NoParameterOperator("IS NOT NULL", "Is Not Null");
                    break;
                default:
                    throw new Exception("Invalid Operator");
            }
            log.LogMethodExit(@operator);
            return @operator;
        }
    }
}
