/********************************************************************************************
 * Project Name - Achievements
 * Description  - Data Handler-   AchievementClassLevelParams class
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
    public class AchievementClassParams
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private int achievementClassId;
        private string className;
        private int achievementProjectId;
        private int gameId;
        private bool isActive;
        private int siteId;
        private int masterEntityId;
        private string externalSystemReference;
    

        /// <summary>
        /// Default constructor
        /// </summary>
        public AchievementClassParams()
        {
            log.LogMethodEntry();
            achievementClassId = -1;
            className = string.Empty;
            achievementProjectId = -1;
            gameId = -1;
            siteId = -1;
            masterEntityId = -1;
            externalSystemReference = string.Empty;
            log.LogMethodExit();
        }
     
        /// <summary>
        /// Get/Set method of the AchievementClassId field
        /// </summary>
        [DisplayName("AchievementClassId")]
        public int AchievementClassId { get { return achievementClassId; } set { achievementClassId = value;  } }

        /// <summary>
        /// Get/Set method of the ClassName field
        /// </summary>
        [DisplayName("ClassName")]
        public string ClassName { get { return className; } set { className = value;  } }

        /// <summary>
        /// Get/Set method of the AchievementProjectId field
        /// </summary>
        [DisplayName("AchievementProjectId")]
        public int AchievementProjectId { get { return achievementProjectId; } set { achievementProjectId = value; } }

        /// <summary>
        /// Get/Set method of the GameId field
        /// </summary>
        [DisplayName("GameId")]
        public int GameId { get { return gameId; } set { gameId = value;  } }


        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value;  } }


        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }


        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }

        /// <summary>
        /// Get/Set method of the ExternalSystemReference field
        /// </summary>
        [DisplayName("ExternalSystemReference")]
        public string ExternalSystemReference { get { return externalSystemReference; } set { externalSystemReference = value; } }

 
    }
}
