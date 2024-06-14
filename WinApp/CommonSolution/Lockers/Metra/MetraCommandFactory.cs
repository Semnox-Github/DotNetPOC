/********************************************************************************************
 * Project Name - Parafait Locker Lock
 * Description  - The factory class for MetraCommands
 * 
 **************
 **Version Log
 **************
 *Version     Date               Modified By       Remarks          
 *********************************************************************************************
 *2.130.00    26-Aug-2021        Dakshakh raj      Created 
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Device.Lockers
{
    public class MetraCommandFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get Metra Locker Issue Command
        /// </summary>
        /// <param name="lockerMake"></param>
        /// <param name="cardNumber"></param>
        /// <param name="zoneCode"></param>
        /// <param name="isFixedMode"></param>
        /// <param name="lockerNo"></param>
        /// <param name="dateTimeFrom"></param>
        /// <param name="dateTimeTo"></param>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static MetraLockerIssueCommand GetMetraLockerIssueCommand(string lockerMake, string cardNumber, string zoneCode, bool isFixedMode, string lockerNo, DateTime dateTimeFrom, DateTime dateTimeTo, ExecutionContext executionContext)
        {
            log.LogMethodEntry(lockerMake, cardNumber, zoneCode, isFixedMode, lockerNo, dateTimeFrom, dateTimeTo, executionContext);
            if (ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS.ToString().Equals(lockerMake))
            {
                log.LogMethodExit(true);
                return new MetraELSLockerIssueCommand(cardNumber, zoneCode, isFixedMode, lockerNo, dateTimeFrom, dateTimeTo, executionContext);
            }
            else if (ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS_NET.ToString().Equals(lockerMake) && isFixedMode)
            {
                log.LogMethodExit(true);
                return new MetraELSNETFixedLockerIssueCommand(cardNumber, zoneCode, isFixedMode, lockerNo, dateTimeFrom, dateTimeTo, executionContext);
            }
            else if (ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS_NET.ToString().Equals(lockerMake) && !isFixedMode)
            {
                log.LogMethodExit(true);
                return new MetraELSNETFreeLockerIssueCommand(cardNumber, executionContext);
            }
            else
            {
                string message = MessageContainerList.GetMessage(executionContext, 4046);
                log.LogMethodExit(message);
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Get Metra Locker Unlock Command
        /// </summary>
        /// <param name="lockerMake"></param>
        /// <param name="zoneCode"></param>
        /// <param name="lockerNo"></param>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static MetraLockerUnlockCommand GetMetraLockerUnlockCommand(string lockerMake, string zoneCode, string lockerNo, ExecutionContext executionContext)
        {
            log.LogMethodEntry(lockerMake, zoneCode, lockerNo, executionContext);
            if (ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS.ToString().Equals(lockerMake))
            {
                log.LogMethodExit(true);
                return new MetraELSLockerUnlockCommand(zoneCode, lockerNo, executionContext);
            }
            if (ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS_NET.ToString().Equals(lockerMake))
            {
                log.LogMethodExit(true);
                return new MetraELSNETLockerUnlockCommand(zoneCode, lockerNo, executionContext);
            }
            else
            {
                string message = MessageContainerList.GetMessage(executionContext, 4046);
                log.LogMethodExit(message);
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Get Metra Locker Card Info Command
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static MetraLockerCardInfoCommand GetMetraLockerCardInfoCommand(string cardNumber, ExecutionContext executionContext)
        {
            log.LogMethodEntry(cardNumber, executionContext);
            try
            {
                log.LogMethodExit(true);
                return new MetraLockerCardInfoCommand(cardNumber, executionContext);
            }
            catch
            {
                string message = MessageContainerList.GetMessage(executionContext, 4046);
                log.LogMethodExit(message);
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Metra Locker BlackList Command
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="action"></param>
        /// <param name="executionContext"></param>
        /// <param name="lockerMake"></param>
        /// <returns></returns>
        public static MetraLockerBlackListCommand GetMetraLockerBlackListCommand(string cardNumber, string action, ExecutionContext executionContext, string lockerMake)
        {
            log.LogMethodEntry(cardNumber, action, executionContext, lockerMake);
            if (ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS_NET.ToString().Equals(lockerMake))
            {
                log.LogMethodExit(true);
                return new MetraELSNETLockerBlackListCommand(cardNumber, action, executionContext, lockerMake);
            }
            else
            {
                string message = MessageContainerList.GetMessage(executionContext, 4046);
                log.LogMethodExit(message);
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Get Metra Locker Erase Command
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="executionContext"></param>
        /// <param name="lockerMake"></param>
        /// <param name="lockerMode"></param>
        /// <param name="zoneCode"></param>
        /// <param name="lockerNumber"></param>
        /// <returns></returns>
        public static MetraLockerEraseCommand GetMetraLockerEraseCommand(string cardNumber, ExecutionContext executionContext, string lockerMake, string lockerMode, string zoneCode, string lockerNumber)
        {
            log.LogMethodEntry(cardNumber, executionContext, lockerMake, lockerMode, zoneCode, lockerNumber);
            if (ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS.ToString().Equals(lockerMake))
            {
                log.LogMethodExit(true);
                return new MetraELSLockerEraseCommand(cardNumber, executionContext);
            }
            else if (ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS_NET.ToString().Equals(lockerMake) && lockerMode.ToString().Equals("FIXED"))
            {
                log.LogMethodExit(true);
                return new MetraELSNETFixedLockerEraseCommand(cardNumber, executionContext);
            }
            else if (ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS_NET.ToString().Equals(lockerMake) && lockerMode.ToString().Equals("FREE"))
            {
                log.LogMethodExit(true);
                return new MetraELSNETFreeLockerEraseCommand(cardNumber, executionContext);
            }
            else
            {
                string message = MessageContainerList.GetMessage(executionContext, 4046);
                log.LogMethodExit(message);
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Get Metra Locker Free Command
        /// </summary>
        /// <param name="lockerMake"></param>
        /// <param name="lockerMode"></param>
        /// <param name="zoneCode"></param>
        /// <param name="lockerNumber"></param>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static MetraLockerFreeCommand GetMetraLockerFreeCommand(string lockerMake, string lockerMode, string zoneCode, string lockerNumber, ExecutionContext executionContext)
        {
            log.LogMethodEntry(lockerMake, lockerMode, zoneCode, lockerNumber, executionContext);
            if (ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS_NET.ToString().Equals(lockerMake))//&& lockerMode.ToString().Equals("FREE"))
            {
                log.LogMethodExit(true);
                return new MetraELSNETFreeLockerFreeCommand(zoneCode, lockerNumber, executionContext);
            }
            else
            {
                string message = MessageContainerList.GetMessage(executionContext, 4046);
                log.LogMethodExit(message);
                throw new Exception(message);
            }
        }
    }
}
