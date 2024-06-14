/********************************************************************************************
 * Project Name - Transaction
 * Description  - Factory class to create KDS Terminal type
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        9-Sep-2019   Lakshminarayana         Created 
 ********************************************************************************************/

using System;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction.KDS
{
    /// <summary>
    /// Factory class to create the KDS terminals based on the KDS Terminal type.
    /// </summary>
    public static class KDSTerminalTypeFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// Creates the KDS Terminal based on the terminal type parameters
        /// </summary>
        /// <param name="terminalType">KDS Terminal type</param>
        /// <param name="executionContext">Execution context</param>
        /// <param name="terminalId">terminal id</param>
        /// <returns></returns>
        public static KDSTerminal GetTerminalType(TerminalTypes terminalType,
                                                  ExecutionContext executionContext, int terminalId)
        {
            log.LogMethodEntry(terminalType, executionContext, terminalId);
            KDSTerminal result;
            switch (terminalType)
            {
                case TerminalTypes.Both:
                    {
                        result = new CombinedTerminal(executionContext, terminalId);
                        break;
                    }
                case TerminalTypes.Kitchen:
                    {
                        result = new KitchenTerminal(executionContext, terminalId);
                        break;
                    }
                case TerminalTypes.Delivery:
                    {
                        result = new DeliveryTerminal(executionContext, terminalId);
                        break;
                    }
                default:
                {
                    string errorMessage = "Invalid terminal type (" + terminalType.ToString() +
                                          "). Terminal type is not supported.";
                    log.LogMethodExit("Throwing Exception - " + errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
