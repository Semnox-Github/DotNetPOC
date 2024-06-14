/********************************************************************************************
 * Project Name - Achievements
 * Description  - Bussiness logic of the   AchievementScoreLogExtended List class
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By      Remarks          
 *********************************************************************************************
 *1.00        15-may-2017    Rakshith         Created 
 *2.70        09-jul-2019    Deeksha          Modified :changed log.debug to log.logMethodEntry
 *                                                      and log.logMethodExit
 ********************************************************************************************/

using System.Collections.Generic;


namespace Semnox.Parafait.Achievements
{
     public class AchievementScoreLogExtendedList
    {
        private Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the AchievementScoreLogExtendedDTO data of passed id
        /// </summary>
        /// <param name="cardId">integer type parameter</param>
        /// <returns>Returns list of AchievementScoreLogExtendedDTO</returns>
        public List<AchievementScoreLogExtendedDTO> GetAchievementScoreLogExtendedList(int cardId)
        {
            log.LogMethodEntry(cardId);
            AchievementScoreLogExtendedDataHandler achievementScoreLogExtendedDataHandler = new AchievementScoreLogExtendedDataHandler();
            List<AchievementScoreLogExtendedDTO> achievementScoreLogExtendedDTO = new List<AchievementScoreLogExtendedDTO>();
            achievementScoreLogExtendedDTO= achievementScoreLogExtendedDataHandler.GetAchievementScoreLogExtendedList(cardId);
            log.LogMethodExit(achievementScoreLogExtendedDTO);
            return achievementScoreLogExtendedDTO;
        }
    }
}
