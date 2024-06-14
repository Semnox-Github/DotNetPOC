/********************************************************************************************
 * Project Name - Game
 * Description  - Hub status data transfer object
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By                          Remarks          
 ********************************************************************************************* 
 *2.150.2     29-Nov-2022       Abhishek                    Created - Game Server Cloud Movement.
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Game
{
    public class HubStatusDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        private bool restartAP;
   
        public HubStatusDTO()
        {
            log.LogMethodEntry();
            restartAP = false;
            log.LogMethodExit();
        }

        public HubStatusDTO(bool restartAP)
            :this()
        {
            log.LogMethodEntry(restartAP);
            this.RestartAP = restartAP;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the RestartAP field
        /// </summary>
        public bool RestartAP { get { return restartAP; } set { restartAP = value; } }

    }
}
