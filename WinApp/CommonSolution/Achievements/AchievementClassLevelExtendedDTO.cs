using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Semnox.Parafait.Achievements
{
    public class AchievementClassLevelExtendedDTO :AchievementClassLevelDTO
    {
        private string className;

        public AchievementClassLevelExtendedDTO():base()
        {
            this.className = "";
        }
        public AchievementClassLevelExtendedDTO( string className, AchievementClassLevelDTO achievementClassLevelDTO) : base(achievementClassLevelDTO.AchievementClassLevelId, achievementClassLevelDTO.LevelName, achievementClassLevelDTO.AchievementClassId, achievementClassLevelDTO.ParentLevelId, achievementClassLevelDTO.QualifyingScore, achievementClassLevelDTO.QualifyingLevelId, achievementClassLevelDTO.RegistrationRequired, achievementClassLevelDTO.BonusEntitlement, achievementClassLevelDTO.BonusAmount, achievementClassLevelDTO.IsActive, achievementClassLevelDTO.Picture, achievementClassLevelDTO.LastUpdatedDate, achievementClassLevelDTO.LastUpdatedUser, achievementClassLevelDTO.Guid, achievementClassLevelDTO.SynchStatus, achievementClassLevelDTO.MasterEntityId, achievementClassLevelDTO.SiteId, achievementClassLevelDTO.ExternalSystemReference, achievementClassLevelDTO.CreatedBy, achievementClassLevelDTO.CreationDate)
        {
            this.className = className;
        }

        /// <summary>
        /// Get/Set method of the ClassName field
        /// </summary>
        [DisplayName("ClassName")]
        public string ClassName { get { return className; } set { className = value; } }

    }
}
