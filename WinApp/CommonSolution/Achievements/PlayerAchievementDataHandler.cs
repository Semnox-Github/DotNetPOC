/********************************************************************************************
* Project Name - PlayerAchievementDataHandler
* Description  - PlayerAchievementDataHandler
* 
**************
**Version Log
**************
* Version     Date             Modified By        Remarks          
*********************************************************************************************
* 2.90.0      29-May-2020      Dakashakh raj       Ability to handle multiple projects. 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Semnox.Parafait.Achievements
{

    public class PlayerAchievementDataHandler
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Semnox.Core.Utilities.DataAccessHandler dataAccessHandler;


        /// <summary>
        /// Default constructor of PlayerAchievementDataHandler class
        /// </summary>
        public PlayerAchievementDataHandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new Semnox.Core.Utilities.DataAccessHandler();
            log.LogMethodExit();
        }



        /// <summary>
        /// GetAchievement Class Machine
        /// </summary>
        /// <param name="machineName"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public int GetAchievementClassMachine(string machineName, ref int gameId)
        {
            log.LogMethodEntry(machineName, gameId);
            int machineId = -1;

            SqlParameter[] getAchievementLevelParameters = new SqlParameter[1];
            getAchievementLevelParameters[0] = new SqlParameter("@machineName", machineName);

            DataTable dtGM = dataAccessHandler.executeSelectQuery("select g.game_id,m.machine_id, m.machine_address, timer_machine " +
                                                      @"from machines m left outer join CustomDataView cdv
                                                                    on cdv.CustomDataSetId = m.CustomDataSetId 
                                                                     and cdv.Name = 'External System Identifier' 
                                                                     and cdv.CustomDataText = @machineName, " +
                                                      "games g, game_profile gp " +
                                                      "where (cdv.CustomDataSetId is not null or (cdv.CustomDataSetId is null and m.machine_name = @machineName)) " +
                                                      "and m.game_id = g.game_id " +
                                                      "and gp.game_profile_id = g.game_profile_id", getAchievementLevelParameters);
            if (dtGM.Rows.Count > 0)
            {
                machineId = Convert.ToInt32(dtGM.Rows[0]["machine_id"]);
                gameId = Convert.ToInt32(dtGM.Rows[0]["game_id"]);
            }
            log.LogMethodExit(machineId);
            return machineId;
        }

        /// <summary>
        /// GetPlayerAchievementScore
        /// </summary>
        /// <param name="achievementParams"></param>
        /// <returns></returns>
        public int GetPlayerAchievementScore(AchievementParams achievementParams)
        {
            log.LogMethodEntry(achievementParams);
            try
            {
                string getAchievementlevelQuery = @"select isnull(sum(isnull(score,0)),0) score
                                                            from AchievementClasslevelView
                                                        where CardId = @CardId 
                                                            and (AchievementClassId = @AchievementClassId or @AchievementClassId = -1 
                                                                 or (@AchievementClassId != -1 and @GameId = -1))";
                string appendQueryWithProjName = @"and(ProjectName in (" + GetInClauseParameterName("Project_Name", achievementParams.ProjectName) + @"))";
                List<SqlParameter> getAchievementLevelParameters = new List<SqlParameter>();
                getAchievementLevelParameters.Add(new SqlParameter("@CardId", achievementParams.CardId));
                getAchievementLevelParameters.Add(new SqlParameter("@GameId", achievementParams.GameId));
                getAchievementLevelParameters.Add(new SqlParameter("@AchievementClassId", achievementParams.AchievementClassId));
                getAchievementLevelParameters.AddRange(GetSqlParametersForInClause("Project_Name", achievementParams.ProjectName));
                if (!string.IsNullOrEmpty(achievementParams.ProjectName))
                {
                    getAchievementlevelQuery = getAchievementlevelQuery + appendQueryWithProjName;
                }
                DataTable dtScore = dataAccessHandler.executeSelectQuery(getAchievementlevelQuery, getAchievementLevelParameters.ToArray());
                int playerScore = 0;
                if (dtScore.Rows.Count > 0)
                {
                    playerScore = Convert.ToInt32(dtScore.Rows[0]["score"]);
                }

                log.LogMethodExit(playerScore);
                return playerScore;
            }
            catch (Exception expn)
            {
                log.Error(expn.Message);
                throw new System.Exception(expn.Message.ToString());
            }
        }





        /// <summary>
        /// GetTopNPlayers
        /// </summary>
        /// <param name="achievementParams"></param>
        /// <returns></returns>
        public DataTable GetTopNPlayers(AchievementParams achievementParams, DateTime? fromDate = null, DateTime? todate = null)
        {
            log.LogMethodEntry(achievementParams, fromDate, todate);

            DataTable dtPlayers = new DataTable();
            try
            {
                string appendQueryWithProjName = @"and(ProjectName in (" + GetInClauseParameterName("Project_Name", achievementParams.ProjectName) + @"))
                                                    group by c.card_id, customer_id
                                                    having sum(isnull(CreditPlusBalance, 0)) > 0
                                                    order by points desc ";
                string appendQuery = @"group by c.card_id, customer_id
                                                    having sum(isnull(CreditPlusBalance, 0)) > 0
                                                    order by points desc ";
                string getAchievementlevelQuery = @" select top(@Nplayers)  c.card_id, customer_id,
                                                    sum(isnull(CreditPlusBalance, 0)) points,
                                                    sum(isnull(CreditPlus, 0)) historicalpoints
                                                    from CardCreditPlus cpl
                                                    inner join cards c on c.card_id = cpl.Card_id
                                                    inner join AchievementScoreLog l on l.CardId = c.card_id
                                                    inner join AchievementClass ac on ac.AchievementClassId = l.AchievementClassId
                                                    inner join AchievementProject ap on ap.AchievementProjectId = ac.AchievementProjectId
                                                    where(c.card_id = @CardId  or @CardId = -1)
                                                             and valid_flag = 'Y'
                                                             and(PeriodFrom is null or PeriodFrom between @fromDate and @toDate)
                                                             and(PeriodTo is null or PeriodFrom between @fromDate and @toDate)";
                if (string.IsNullOrEmpty(achievementParams.ProjectName))
                {
                    getAchievementlevelQuery = getAchievementlevelQuery + appendQuery;
                }
                else
                {
                    getAchievementlevelQuery = getAchievementlevelQuery + appendQueryWithProjName;
                }

                List<SqlParameter> getAchievementLevelParameters = new List<SqlParameter>();
                getAchievementLevelParameters.Add(new SqlParameter("@CardId", achievementParams.CardId));
                getAchievementLevelParameters.Add(new SqlParameter("@Nplayers", achievementParams.Players <= 0 ? 10 : achievementParams.Players));
                getAchievementLevelParameters.Add(new SqlParameter("@fromDate", string.IsNullOrEmpty(fromDate.ToString()) ? Convert.ToDateTime("1/1/1900") : fromDate));
                getAchievementLevelParameters.Add(new SqlParameter("@toDate", string.IsNullOrEmpty(todate.ToString()) ? DateTime.Now.AddDays(1) : todate));
                getAchievementLevelParameters.AddRange(GetSqlParametersForInClause("Project_Name", achievementParams.ProjectName));
                dtPlayers = dataAccessHandler.executeSelectQuery(getAchievementlevelQuery, getAchievementLevelParameters.ToArray());
                log.LogMethodExit();
            }
            catch(Exception ex)
            {
                log.LogMethodExit(dtPlayers);
                log.Error("Error occurred while executing GetTopNPlayers()" + ex.Message);
            }

            log.LogMethodExit(dtPlayers);
            return dtPlayers;
        }



        public string GetInClauseParameterName(string key, string commaSeperatedValues)
        {
            log.LogMethodEntry(key, commaSeperatedValues);
            List<string> parameterNameList = GetInClauseParameterNameList(key, commaSeperatedValues);
            string result = string.Join(",", parameterNameList);
            log.LogMethodExit(result);
            return result;
        }

        private List<string> GetInClauseParameterNameList(string key, string commaSeperatedValues)
        {
            log.LogMethodEntry(key, commaSeperatedValues);
            List<string> result = new List<string>();
            string[] values = commaSeperatedValues.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < values.Length; i++)
            {
                result.Add("@" + key.ToString() + i);
            }
            log.LogMethodExit(result);
            return result;
        }

        private List<SqlParameter> GetSqlParametersForInClause(string key, string commaSeperatedValues)
        {
            log.LogMethodEntry(key, commaSeperatedValues);
            string[] values = commaSeperatedValues.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> parameterNameList = GetInClauseParameterNameList(key, commaSeperatedValues);
            List<SqlParameter> result = new List<SqlParameter>();
            for (int i = 0; i < parameterNameList.Count; i++)
            {
                result.Add(new SqlParameter(parameterNameList[i], values[i]));
            }
            log.LogMethodExit(result);
            return result;
        }

    }


}
