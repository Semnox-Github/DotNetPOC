/********************************************************************************************
*Project Name - RedemptionAuthentication
*Description -  Class to do manager/logib user authentication check
*************
**Version Log
*************
*Version      Date                   Modified By                 Remarks          
*********************************************************************************************
*2.80         20-Aug-2019            Archana                     Created                                      
**********************************************************************************************/
using Semnox.Core.Utilities;

namespace Parafait_POS
{
    static class RedemptionAuthentication
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal static bool RedemptionAuthenticateManger(DeviceClass cardReader, ref int managerId)
        {
            log.LogMethodEntry(cardReader, managerId);
            bool returnValue = false;
            if (cardReader != null)
            {
                Common.Devices.CardReaders.Add(cardReader);
            }
            returnValue = Authenticate.Manager(ref managerId);
            if (cardReader != null)
            {
                Common.Devices.CardReaders.Remove(cardReader);
                //cardReader.UnRegister();
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        internal static bool RedemptionLoginUser(Utilities utilities, DeviceClass cardReader, ref Security.User user, bool isBasicCheck = false)
        {
            log.LogMethodEntry(utilities, cardReader, isBasicCheck);
            bool returnValue = false;
            if (cardReader != null)
            {
                Common.Devices.CardReaders.Add(cardReader);
            }
            if (isBasicCheck)
            {
                returnValue = Authenticate.BasicCheck(ref user, false);
            }
            else
            {
                Authenticate.loginUser(user, utilities.ParafaitEnv);
            }
            if (cardReader != null)
            {
                Common.Devices.CardReaders.Remove(cardReader);
                //cardReader.UnRegister();
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
