/********************************************************************************************
 * Project Name - Achievements
 * Description  - Bussiness logic of the   AchievementScoreLogExtendedDataHandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00        15-may-2017    Rakshith         Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Achievements
{
    public class AchievementScoreLogExtendedDataHandler
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Semnox.Core.Utilities.DataAccessHandler dataAccessHandler;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of AchievementScoreLogExtendedDataHandler class
        /// </summary>
        public AchievementScoreLogExtendedDataHandler()
        {
            log.Debug("Starts-AchievementScoreLogExtendedDataHandler() default constructor.");
            dataAccessHandler = new Semnox.Core.Utilities.DataAccessHandler();
            log.Debug("Ends-AchievementScoreLogExtendedDataHandler() default constructor.");
        }


        /// <summary>
        /// Converts the Data row object to AchievementScoreLogExtendedDTO class type
        /// </summary>
        /// <param name="achievementScoreLogExtendedDataRow">AchievementScoreLogExtendedDTO DataRow</param>
        /// <param name="achievementScoreLogDTO">AchievementScoreLogDTO achievementScoreLogDTO</param>
        /// <returns>Returns AchievementScoreLogExtendedDTO</returns>
        private AchievementScoreLogExtendedDTO GetAchievementScoreLogExtendedDTO(DataRow achievementScoreLogExtendedDataRow, AchievementScoreLogDTO achievementScoreLogDTO)
        {
            log.Debug("Starts-GetAchievementScoreLogExtendedDTO(achievementScoreLogExtendedDataRow) Method.");
            AchievementScoreLogExtendedDTO achievementScoreLogExtendedDTO = new AchievementScoreLogExtendedDTO
                (
                achievementScoreLogExtendedDataRow["ClassName"].ToString(),
                achievementScoreLogExtendedDataRow["machine_name"].ToString(),
                achievementScoreLogExtendedDataRow["game_name"].ToString(),
                achievementScoreLogDTO
                );
            log.Debug("Ends-GetAchievementScoreLogExtendedDTO(achievementScoreLogExtendedDataRow) Method.");
            return achievementScoreLogExtendedDTO;
        }

        /// <summary>
        /// Gets the AchievementScoreLogExtendedDTO data of passed id
        /// </summary>
        /// <param name="cardId">integer type parameter</param>
        /// <returns>Returns list of AchievementScoreLogExtendedDTO</returns>
        public List<AchievementScoreLogExtendedDTO> GetAchievementScoreLogExtendedList(int cardId)
        {
            log.Debug("Starts-GetAchievementScoreLogExtendedList(int cardId) Method.");

            string selectAchievementScoreLogExtendedQuery = @"select ac.ClassName,m.machine_name,g.game_name, asl.* 
                        from  AchievementScoreLog asl
                        inner join  AchievementClass ac on ac.AchievementClassId=asl.AchievementClassId
                        inner join  machines m on m.machine_id=asl.MachineId
                        inner join  games g on g.game_id=ac.GameId
                        where asl.CardId=@cardId";

            SqlParameter[] achievementScoreLogExtendedParameters = new SqlParameter[1];
            achievementScoreLogExtendedParameters[0] = new SqlParameter("@cardId", cardId);

            List<AchievementScoreLogExtendedDTO> achievementScoreLogExtendedList = new List<AchievementScoreLogExtendedDTO>();
            DataTable dtAchievementScoreLogExtended = dataAccessHandler.executeSelectQuery(selectAchievementScoreLogExtendedQuery, achievementScoreLogExtendedParameters);
            if (dtAchievementScoreLogExtended.Rows.Count > 0)
            {
                AchievementScoreLogDataHandler achievementScoreLogDataHandler = new AchievementScoreLogDataHandler();
                foreach (DataRow dataRow in dtAchievementScoreLogExtended.Rows)
                {
                    int id = string.IsNullOrEmpty(dataRow["Id"].ToString()) ? -1 : Convert.ToInt32(dataRow["Id"]);
                    AchievementScoreLogDTO achievementScoreLogDTO = achievementScoreLogDataHandler.GetAchievementScoreLogDTO(id);
                    AchievementScoreLogExtendedDTO achievementScoreLogExtendedDTO = GetAchievementScoreLogExtendedDTO(dataRow, achievementScoreLogDTO);
                    achievementScoreLogExtendedList.Add(achievementScoreLogExtendedDTO);
                }
            }
            log.Debug("Ends-GetAchievementScoreLogExtendedList(int cardId) Method.");
            return achievementScoreLogExtendedList;

        }
    }
}
