/********************************************************************************************
 * Project Name - Achievements
 * Description  - BL -AchievementClassLevelExtended
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        13-Aug-2019   Deeksha                Added logger methods
 ********************************************************************************************/
using System.Collections.Generic;

namespace Semnox.Parafait.Achievements
{
    public class AchievementClassLevelExtendedList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Returns the AchievementClassLevelExtendedDTO list
        /// </summary>
        public List<AchievementClassLevelExtendedDTO> GetAchievementClassLevelExtended(int cadId)
        {
            log.LogMethodEntry(cadId);
            AchievementClassLevelExtendedDataHandler achievementClassLevelExtendedDataHandler = new AchievementClassLevelExtendedDataHandler();
            List<AchievementClassLevelExtendedDTO> achievementClassLevelExtendedDTOs = new List<AchievementClassLevelExtendedDTO>();
            achievementClassLevelExtendedDTOs = achievementClassLevelExtendedDataHandler.GetAchievementClassLevelExtended(cadId);
            log.LogMethodExit(achievementClassLevelExtendedDTOs);
            return achievementClassLevelExtendedDTOs;
        }
    }
}
