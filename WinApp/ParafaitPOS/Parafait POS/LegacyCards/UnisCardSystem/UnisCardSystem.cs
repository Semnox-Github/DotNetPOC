/*/********************************************************************************************
 * Project Name - UnisCardSystem
 * Description  - Unis Card System
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks 
 *********************************************************************************************
 *2.100       1-Sep-2020    Dakshakh raj            Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Data.SqlClient;

namespace Parafait_POS
{
    class UnisCardSystem : ExternalCardSystem
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private LegacyCardDTO legacyCardDTO;

        public UnisCardSystem(ExecutionContext executionContext, LegacyCardDTO legacyCardDTO)
                : base(executionContext, legacyCardDTO)
        {
            log.LogMethodEntry(executionContext, legacyCardDTO);
            this.executionContext = executionContext;
            this.legacyCardDTO = legacyCardDTO;
            log.LogMethodExit(null);
        }
        /// <summary>
        /// GetCardInformation
        /// </summary>
        /// <returns></returns>
        public override LegacyCardDTO GetCardInformation()
        {
            log.LogMethodEntry();
            if (legacyCardDTO.CardId <= -1)
            {
                string message = "Legacy card does not exist.Please enter valid details and continue.";
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(legacyCardDTO);
            return legacyCardDTO;
        }

        /// <summary>
        /// ProcessCardData
        /// </summary>
        /// <param name="sqltransaction"></param>
        public override void ProcessCardData(SqlTransaction sqltransaction)
        {
            log.LogMethodEntry(sqltransaction);
            log.LogMethodExit();
        }
    }
}
