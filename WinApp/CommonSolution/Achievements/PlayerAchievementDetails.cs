/********************************************************************************************
 * Project Name - Achievement
 * Description  - Player Achievement Details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        13-Aug-2019     Deeksha                 Added logger methods.
 ********************************************************************************************/
using Semnox.Parafait.Customer;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Achievements
{
    public class PlayerAchievementDetails
    {
        private CustomerDTO customerDTO;
        private List<AchievementLevelExtended> achievementLevelExtendedList;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public PlayerAchievementDetails()
        {
            log.LogMethodEntry();
            customerDTO = new CustomerDTO();
            achievementLevelExtendedList = new List<AchievementLevelExtended>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the AchievementLevelExtendedList field
        /// </summary>
        [DisplayName("AchievementLevelExtendedList")]
        public List<AchievementLevelExtended> AchievementLevelList { get { return achievementLevelExtendedList; } set { achievementLevelExtendedList = value; } }


        /// <summary>
        /// Get/Set method of the CustomerDTO field
        /// </summary>
        [DisplayName("CustomerDTO")]
        public CustomerDTO CustomerDTO { get { return customerDTO; } set { customerDTO = value; } }
    }
}
