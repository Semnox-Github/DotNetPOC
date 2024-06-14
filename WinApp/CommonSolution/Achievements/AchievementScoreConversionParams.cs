using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Semnox.Parafait.Achievements
{
    public class AchievementScoreConversionParams
    {
        int id;
        int achievementClassLevelId;
        bool isActive;
        int masterEtityId;

        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Default constructor
        /// </summary>
        public AchievementScoreConversionParams()
        {
            log.LogMethodEntry();
            id = -1;
            isActive = false;
            achievementClassLevelId = -1;
            masterEtityId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        public int Id { get { return id; } set { id = value;   } }



        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value;   } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("MasterEtityId")]
        public int MasterEtityId { get { return masterEtityId; } set { masterEtityId = value; } }

        /// <summary>
        /// Get/Set method of the AchievementClassLevelId field
        /// </summary>
        [DisplayName("AchievementClassLevelId")]
        public int AchievementClassLevelId { get { return achievementClassLevelId; } set { achievementClassLevelId = value;   } }

    }
}
