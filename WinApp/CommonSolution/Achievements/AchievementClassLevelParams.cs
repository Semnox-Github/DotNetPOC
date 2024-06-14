/********************************************************************************************
 * Project Name - Achievements
 * Description  - Data Handler-AchievementClassLevelParams class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00        15-may-2017   Rakshith         Created 
 *2.70        09-Jul-2019   Deeksha          Modified :Added get/set method for MasterEntityId
 *2.70.2        13-Aug-2019   Deeksha          Added logger methods.
 ********************************************************************************************/

using System.ComponentModel;

namespace Semnox.Parafait.Achievements
{
    public class AchievementClassLevelParams
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int achievementClassLevelId;
        private string levelName;
        private int achievementClassId;
        private bool isActive;
        private int parentLevelId;
        private int qualifyingLevelId;
        private bool registrationRequired;
        private int siteId;
        private int masterEntityId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AchievementClassLevelParams()
        {
            log.LogMethodEntry();
            achievementClassLevelId = -1;
            levelName = string.Empty;
            achievementClassId = -1;
            siteId = -1;
            parentLevelId = -1;
            qualifyingLevelId = -1;
            registrationRequired = false;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the AchievementClassLevelId field
        /// </summary>
        [DisplayName("AchievementClassLevelId")]
        [ReadOnly(true)]
        public int AchievementClassLevelId { get { return achievementClassLevelId; } set { achievementClassLevelId = value; } }

        /// <summary>
        /// Get/Set method of the LevelName field
        /// </summary>
        [DisplayName("LevelName")]
        public string LevelName { get { return levelName; } set { levelName = value; } }

        /// <summary>
        /// Get/Set method of the AchievementClassId field
        /// </summary>
        [DisplayName("AchievementClassId")]
        public int AchievementClassId { get { return achievementClassId; } set { achievementClassId = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the ParentLevelId field
        /// </summary>
        [DisplayName("ParentLevelId")]
        public int ParentLevelId { get { return parentLevelId; } set { parentLevelId = value; } }

        /// <summary>
        /// Get/Set method of the QualifyingLevelId field
        /// </summary>
        [DisplayName("QualifyingLevelId")]
        public int QualifyingLevelId { get { return qualifyingLevelId; } set { qualifyingLevelId = value; } }

        /// <summary>
        /// Get/Set method of the RegistrationRequired field
        /// </summary>
        [DisplayName("RegistrationRequired")]
        public bool RegistrationRequired { get { return registrationRequired; } set { registrationRequired = value; } }


        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }

    }
}
