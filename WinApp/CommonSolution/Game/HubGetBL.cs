/********************************************************************************************
 * Project Name - Games
 * Description  - HubGetBL class is used to call the business methods - Only  Get operation
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Game
{
    public class HubGetBL
    {
        private HubDTO hubDTO;
        private HubDataHandler hubDataHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private SqlTransaction sqlTransaction;


        private HubGetBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public HubGetBL(ExecutionContext executionContext, HubDTO hubDTO)
      : this(executionContext)
        {
            log.LogMethodEntry(executionContext, hubDTO);
            this.hubDTO = hubDTO;
            log.LogMethodExit();
        }
        public HubGetBL(ExecutionContext executionContext ,int hubId)
            : this(executionContext)
        {
            log.LogMethodEntry(hubId);
            this.hubDTO = hubDataHandler.GetHub(hubId);
            log.LogMethodExit(hubDTO);
        }
        public HubDTO GetHubDTO
        {
            get { return hubDTO; }
        }
    }
}
