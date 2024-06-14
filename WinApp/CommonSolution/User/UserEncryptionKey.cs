/********************************************************************************************
 * Project Name - User
 * Description  - Represents a encryption key used for encrypting user details
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.110.0        1-Jul-2019      Lakshminarayana     Created : POS Redesign 
 ********************************************************************************************/
using System.Text;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.User
{
    internal class UserEncryptionKey : SystemUserEncryptionKey
    {
        private static readonly Semnox.Parafait.logging.Logger log =
            new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public UserEncryptionKey(Semnox.Core.Utilities.ExecutionContext executionContext, string insert) 
            : base(GenerateEncryptionKey(insert, Core.Utilities.SystemOptionContainerList.GetSystemOption(executionContext.SiteId, "Parafait Keys", "MifareCard", "46A97988SEMNOX!1CCCC9D1C581D86EE")))
        {
            log.LogMethodEntry(executionContext, "insert");
            log.LogMethodExit();
        }
    }
}
