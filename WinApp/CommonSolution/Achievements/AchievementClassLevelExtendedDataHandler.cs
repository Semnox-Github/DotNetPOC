/********************************************************************************************
 * Project Name - Achievements
 * Description  - Bussiness logic of the   AchievementClassLevelExtendedDataHandler class
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


using System.Data;
using System.Data.SqlClient;

namespace Semnox.Parafait.Achievements
{
   public class AchievementClassLevelExtendedDataHandler
    {
          Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
         Semnox.Core.Utilities.DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of AchievementClassLevelExtendedDataHandler class
        /// </summary>
        public AchievementClassLevelExtendedDataHandler()
        {
            log.Debug("Starts-AchievementClassLevelExtendedDataHandler() default constructor.");
            dataAccessHandler = new Semnox.Core.Utilities.DataAccessHandler();
            log.Debug("Ends-AchievementClassLevelExtendedDataHandler() default constructor.");
        }

        /// <summary>
        /// Gets the AchievementClassLevelExtendedDTO data of passed achievementClassId
        /// </summary>
        /// <param name="cadId">integer type parameter</param>
        /// <returns>Returns AchievementClassLevelExtendedDTO</returns>
        public List<AchievementClassLevelExtendedDTO> GetAchievementClassLevelExtended(int cadId)
        {
            log.Debug("Starts-GetAchievementClassLevelExtended(cadId) Method.");

            string achievementClassLevelExtendedQuery = @"
                            select ac.ClassName, acl.*  from  AchievementClass ac
                            inner join  AchievementClasslevel acl on ac.AchievementClassId=acl.AchievementClassId
                            inner join  AchievementLevel al on al.AchievementClassLevelId=acl.AchievementClassLevelId
                             where al.CardId=@cadId ";
            SqlParameter[] achievementClassLevelExtendedParameters = new SqlParameter[1];
            achievementClassLevelExtendedParameters[0] = new SqlParameter("@cadId", cadId);
            DataTable dtAchievementClassLevelExtended = dataAccessHandler.executeSelectQuery(achievementClassLevelExtendedQuery, achievementClassLevelExtendedParameters);

            AchievementClassLevelExtendedDTO achievementClassLevelExtended = new AchievementClassLevelExtendedDTO();

            List<AchievementClassLevelExtendedDTO> achievementClassLevelExtendedList = new List<AchievementClassLevelExtendedDTO>();
            if (dtAchievementClassLevelExtended.Rows.Count > 0)
            {
                AchievementClassLevelDataHandler achievementClassLevelDataHandler = new AchievementClassLevelDataHandler();
                foreach (DataRow dataRow in dtAchievementClassLevelExtended.Rows)
                {
                    int achievementClassLevelId = dataRow["AchievementClassLevelId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AchievementClassLevelId"]);
                    AchievementClassLevelDTO achievementClassLevelDTO = achievementClassLevelDataHandler.GetAchievementClassLevelDTO(achievementClassLevelId);
                    achievementClassLevelExtended = GetAchievementClassLevelExtended(dataRow, achievementClassLevelDTO);
                    achievementClassLevelExtendedList.Add(achievementClassLevelExtended);
                }
              
               

            }
            log.Debug("Ends-GetAchievementClassLevelExtended(cadId) Method.");
            return achievementClassLevelExtendedList;
        }



        /// <summary>
        /// Converts the Data row object to AchievementClassLevelExtended class type
        /// </summary>
        /// <param name="achievementClassLevelExtendedDataRow">AchievementClassLevelExtended DataRow</param>
        /// <returns>Returns list of AchievementClassLevelExtended</returns>
        private AchievementClassLevelExtendedDTO GetAchievementClassLevelExtended(DataRow achievementClassLevelExtendedDataRow, AchievementClassLevelDTO achievementClassLevelDTO)
        {
            log.Debug("Starts-GetAchievementClassLevelExtended(achievementClassLevelExtendedDataRow) Method.");
            AchievementClassLevelExtendedDTO achievementClassLevelExtended = new AchievementClassLevelExtendedDTO
                (
                    achievementClassLevelExtendedDataRow["ClassName"].ToString(),
                    achievementClassLevelDTO
                );
            log.Debug("Ends-GetAchievementClassLevelExtended(achievementClassLevelExtendedDataRow) Method.");
            return achievementClassLevelExtended;
        }

    }
}
