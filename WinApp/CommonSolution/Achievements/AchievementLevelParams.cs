/********************************************************************************************
 * Project Name - Achievement
 * Description  - Data Handler -AchievementLevelParams
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        09-July-2019    Deeksha               Modified:Added get/set method for site id
 *                                                  and masterentityId
 ********************************************************************************************/

using System.ComponentModel;

namespace Semnox.Parafait.Achievements
{
    public class AchievementLevelParams
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int id;
        private int cardId;
        private int achievementClassLevelId;
        private bool isValid;
        private int siteId;
        private int masterEntityId;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public AchievementLevelParams()
        {
            log.LogMethodEntry();
            id = -1;
            cardId = -1;
            achievementClassLevelId = -1;
            isValid = true ;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        public int Id { get { return id; } set { id = value; } }


        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        [DisplayName("CardId")]
        public int CardId { get { return cardId; } set { cardId = value; } }

     
        /// <summary>
        /// Get/Set method of the AchievementClassLevelId field
        /// </summary>
        [DisplayName("AchievementClassLevelId")]
        public int AchievementClassLevelId { get { return achievementClassLevelId; } set { achievementClassLevelId = value; } }


        /// <summary>
        /// Get/Set method of the IsValid field
        /// </summary>
        [DisplayName("IsValid")]
        public bool IsValid { get { return isValid; } set { isValid = value; } }

        /// <summary>
        /// Get/Set method of the IsValid field
        /// </summary>
        [DisplayName("SiteId")]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the IsValid field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }
    }
}
