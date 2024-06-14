using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Semnox.Parafait.Achievements
{
    public class AchievementScoreLogExtendedDTO : AchievementScoreLogDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string className;
        string machineName;
        string gameName;

        public AchievementScoreLogExtendedDTO() : base()
        {
            this.className = string.Empty;
            this.machineName = string.Empty;
            this.gameName = string.Empty;
        }

        public AchievementScoreLogExtendedDTO(string className, string machineName, string gameName, AchievementScoreLogDTO achievementScoreLogDTO)
             : base(achievementScoreLogDTO.Id, achievementScoreLogDTO.CardId, achievementScoreLogDTO.AchievementClassId, achievementScoreLogDTO.MachineId, achievementScoreLogDTO.Score, achievementScoreLogDTO.Timestamp, achievementScoreLogDTO.ConvertedToEntitlement, achievementScoreLogDTO.CardCreditPlusId, achievementScoreLogDTO.IsActive, achievementScoreLogDTO.LastUpdatedDate, achievementScoreLogDTO.LastUpdatedUser, achievementScoreLogDTO.Guid, achievementScoreLogDTO.SynchStatus, achievementScoreLogDTO.MasterEntityId, achievementScoreLogDTO.SiteId, achievementScoreLogDTO.CreatedBy, achievementScoreLogDTO.CreatedDate)
        {
            this.className = className;
            this.machineName = machineName;
            this.gameName = gameName;
        }

        /// <summary>
        /// Get/Set method of the ClassName field
        /// </summary>
        [DisplayName("ClassName")]
        public string ClassName { get { return className; } set { className = value;  } }

        /// <summary>
        /// Get/Set method of the MachineName field
        /// </summary>
        [DisplayName("MachineName")]
        public string MachineName { get { return machineName; } set { machineName = value; } }


        /// <summary>
        /// Get/Set method of the GameName field
        /// </summary>
        [DisplayName("GameName")]
        public string GameName { get { return gameName; } set { gameName = value; } }
    }
}
