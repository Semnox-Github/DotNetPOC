/********************************************************************************************
 * Project Name - Achievement
 * Description  - Data Handler -AchievementParams
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        13-Aug-2019     Deeksha                 Added logger methods.
 ********************************************************************************************/
using System.ComponentModel;

namespace Semnox.Parafait.Achievements
{
    public class AchievementParams
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int cardId;
        private int achievementClassId;
        private string className;
        private string achievementClassExternalSystemReference;
        private string projectName;
        private int score;
        private int players;
        private int gameId;
        private int customerId;
        private int scoringEventId;
        private bool showValidLevelsOnly;

        
        /// <summary>
        /// Default constructor
        /// </summary>
        public AchievementParams()
        {
            log.LogMethodEntry();
            cardId = -1;
            achievementClassId = -1;
            className = string.Empty;
            projectName = string.Empty;
            achievementClassExternalSystemReference = string.Empty;
            score = 0;
            players = 10;
            gameId = -1;
            customerId = -1;
            showValidLevelsOnly = true;
            scoringEventId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        [DisplayName("CardId")]
        public int CardId { get { return cardId; } set { cardId = value; } }


        /// <summary>
        /// Get/Set method of the AchievementClassId field
        /// </summary>
        [DisplayName("AchievementClassId")]
        public int AchievementClassId { get { return achievementClassId; } set { achievementClassId = value; } }

        /// <summary>
        /// Get/Set method of the ProjectName field
        /// </summary>
        [DisplayName("ProjectName")]
        public string ProjectName { get { return projectName; } set { projectName = value; } }


        /// <summary>
        /// Get/Set method of the ClassName field
        /// </summary>
        [DisplayName("ClassName")]
        public string ClassName { get { return className; } set { className = value;  } }

        /// <summary>
        /// Get/Set method of the AchievementClassExternalSystemReference field
        /// </summary>
        [DisplayName("AchievementClassExternalSystemReference")]
        public string AchievementClassExternalSystemReference { get { return achievementClassExternalSystemReference; } set { achievementClassExternalSystemReference = value; } }

        /// <summary>
        /// Get/Set method of the Score field
        /// </summary>
        [DisplayName("Score")]
        public int Score { get { return score; } set { score = value; } }

        /// <summary>
        /// Get/Set method of the Players field
        /// </summary>
        [DisplayName("Players")]
        public int Players { get { return players; } set { players = value; } }


        /// <summary>
        /// Get/Set method of the GameId field
        /// </summary>
        [DisplayName("GameId")]
        public int GameId { get { return gameId; } set { gameId = value; } }


        /// <summary>
        /// Get/Set method of the CustomerId field
        /// </summary>
        [DisplayName("CustomerId")]
        public int CustomerId { get { return customerId; } set { customerId = value; } }

        /// <summary>
        /// Get/Set method of the ScoringEventId field
        /// </summary>
        [DisplayName("ScoringEventId")]
        public int ScoringEventId { get { return scoringEventId; } set { scoringEventId = value; } }


        /// <summary>
        /// Get/Set method of the ShowValidLevelsOnly field
        /// </summary>
        [DisplayName("ShowValidLevelsOnly")]
        public bool ShowValidLevelsOnly { get { return showValidLevelsOnly; } set { showValidLevelsOnly = value; } }


    }
}
