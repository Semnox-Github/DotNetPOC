using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Achievements
{
    public class AchievementLevelExtended : AchievementLevelDTO
    {

        string className;
        string levelName;
        string medalPicture;
        bool isPrimaryLevel;
         /// <summary>
        /// Default constructor
        /// </summary>
        public AchievementLevelExtended()
            : base()
        {
            this.className = string.Empty;
            this.levelName = string.Empty;
            this.medalPicture = string.Empty;
            this.isPrimaryLevel = false;
        }


        public AchievementLevelExtended(string levelName, AchievementLevelDTO achLevelDTO)
            : base(achLevelDTO.Id, achLevelDTO.CardId, achLevelDTO.AchievementClassLevelId, achLevelDTO.IsValid, achLevelDTO.IsActive, achLevelDTO.EffectiveDate, achLevelDTO.LastUpdatedDate, achLevelDTO.LastUpdatedUser, achLevelDTO.Guid, achLevelDTO.SynchStatus, achLevelDTO.MasterEntityId, achLevelDTO.SiteId, achLevelDTO.CreatedBy, achLevelDTO.CreatedDate)
        {
            this.levelName = levelName;
        }



        /// <summary>
        /// Get/Set method of the ClassName field
        /// </summary>
        [DisplayName("ClassName")]
        public string ClassName { get { return className; } set { className = value; } }


        /// <summary>
        /// Get/Set method of the LevelName field
        /// </summary>
        [DisplayName("LevelName")]
        public string LevelName { get { return levelName; } set { levelName = value; } }


        /// <summary>
        /// Get/Set method of the IsPrimaryLevel field
        /// </summary>
        [DisplayName("IsPrimaryLevel")]
        public bool IsPrimaryLevel { get { return isPrimaryLevel; } set { isPrimaryLevel = value; } }


        /// <summary>
        /// Get/Set method of the MedalPicture field
        /// </summary>
        [DisplayName("MedalPicture")]
        public string MedalPicture { get { return medalPicture; } set { medalPicture = value; } }

    }
}
