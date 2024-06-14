/********************************************************************************************
 * Project Name - Achievement  DTO Programs 
 * Description  - Data object of the AchievementParams
 * 
 **
 * ************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        16-feb-2017   Rakshith           Created 
 *2.70        09-jul-2019   Deeksha            Modified :Added get/set method for MasterEntityId
 ********************************************************************************************/

using System.ComponentModel;

namespace Semnox.Parafait.Achievements
{
    public class AchievementProjectParams
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string projectName;
        private int achievementProjectId;
        private int siteId;
        private bool isActive;
        private int masterEntityId;

        /// <summary>
        /// Default Conrtructor
        /// </summary>
        public AchievementProjectParams()
        {
            log.LogMethodEntry();
            this.achievementProjectId = -1;
            this.isActive = true;
            this.siteId = -1;
            this.projectName = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ProjectName field
        /// </summary>
        [DisplayName("ProjectName")]
        public string ProjectName { get { return projectName; } set { projectName = value; } }

        /// <summary>
        /// Get/Set method of the AchievementProjectId field
        /// </summary>
        [ReadOnly(true)]
        public int AchievementProjectId { get { return achievementProjectId; } set { achievementProjectId = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DefaultValue(-1)]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>o
        /// Get/Set method of the Active field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }

    }
}
