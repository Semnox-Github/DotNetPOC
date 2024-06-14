/*/********************************************************************************************
 * Project Name - ExternalCardSystem
 * Description  - External Card System
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks 
 *********************************************************************************************
 *2.100       1-Sep-2020    Dakshakh raj            Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Data.SqlClient;

namespace Parafait_POS

{
    public class ExternalCardSystem
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private LegacyCardDTO legacyCardDTO;

        /// <summary>
        /// ExternalCardSystem
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="legacyCardDTO"></param>
        public ExternalCardSystem(ExecutionContext executionContext, LegacyCardDTO legacyCardDTO)
        {
            log.LogMethodEntry(executionContext, legacyCardDTO);
            this.executionContext = executionContext;
            this.legacyCardDTO = legacyCardDTO;
            log.LogMethodExit(null);
        }
        public virtual LegacyCardDTO GetCardInformation()
        {
            log.LogMethodEntry();
            log.LogMethodExit(null);
            return null;
        }
        public virtual void ProcessCardData(SqlTransaction sqltransaction)
        {
            log.LogMethodEntry();
            log.LogMethodExit(null);
        }
        public virtual void Initialize()
        {
            log.LogMethodEntry();
            log.LogMethodExit(null);
        }
    }
}
