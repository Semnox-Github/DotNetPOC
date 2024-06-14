/********************************************************************************************
 * Project Name - Reports
 * Description  - Data Handler of GamePerformanceBL for Reports module
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.90        24-Jun-2020   Vikas Dwivedi        Created
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Reports
{
    public class GamePerformanceDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private DataTable getProfileDataTable;
        private DataTable overViewDataList;
        private DataTable gameProfileData;

        /// <summary>
        /// Parameterized Constructor 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public GamePerformanceDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        public DataTable GetProfileTabs(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            string query = @"select distinct profile_name from game_profile gp, games g where g.game_profile_id = gp.game_profile_id order by 1";

            SqlParameter[] sqlParameters = new SqlParameter[0];
            gameProfileData = dataAccessHandler.executeSelectQuery(query, sqlParameters, sqlTransaction);
            for (int i = 0; i <= gameProfileData.Rows.Count; i++)
            {
                if (i == gameProfileData.Rows.Count)
                {
                    gameProfileData.Rows.Add("All");
                    break;
                }
            }
            log.LogMethodExit();
            return gameProfileData;
        }

        public DataTable LoadOverView(int pId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(pId, sqlTransaction);

            string query = @"select fromdate, todate from userperiod
                             where periodid = @id";

            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@id", pId);

            DataTable periodDataTable = dataAccessHandler.executeSelectQuery(query, sqlParameters, sqlTransaction);

            DateTime fromDate = Convert.ToDateTime(periodDataTable.Rows[0]["fromdate"].ToString());
            DateTime toDate = Convert.ToDateTime(periodDataTable.Rows[0]["todate"].ToString()).AddDays(1);
            int days = toDate.Subtract(fromDate).Days;

            DataTable overViewDataTable = new DataTable();
            overViewDataTable.Columns.Add("Game Profile", typeof(string));
            overViewDataTable.Columns.Add("Sales Update", typeof(double));
            overViewDataTable.Columns.Add("% Sales", typeof(double));
            overViewDataTable.Columns.Add("Average", typeof(double));
            overViewDataTable.Columns.Add("Sales Forecast for End of the Month", typeof(double));
            overViewDataTable.Columns.Add("Sales Target", typeof(double));
            overViewDataTable.Columns.Add("% Sales Target", typeof(double));
            overViewDataTable.Columns.Add("Difference", typeof(double));
            overViewDataTable.Columns.Add("% Growth", typeof(double));

            string getGameProfileQuery = @"select game_profile_id, game_profile_name, sum(credits) as total from gamemetricview
                                            where play_date between @d1 and @d2
                                            group by game_profile_id, game_profile_name order by 2";

            SqlParameter[] selectGameProfileData = new SqlParameter[2];
            selectGameProfileData[0] = new SqlParameter("@d1", fromDate);
            selectGameProfileData[1] = new SqlParameter("@d2", toDate);

            DataTable gameProfileDataTable = dataAccessHandler.executeSelectQuery(getGameProfileQuery, selectGameProfileData, sqlTransaction);

            int periodId = pId;
            string getProfileWithPTQuery = @"select distinct game_profile_id from games where 
                                                 game_id in (select gameid from usertarget where periodid = @id and target is not null)";

            SqlParameter[] profileWithPeriodParameter = new SqlParameter[1];
            profileWithPeriodParameter[0] = new SqlParameter("@id", periodId);

            DataTable gameProfileWithPTDataTable = dataAccessHandler.executeSelectQuery(getProfileWithPTQuery, profileWithPeriodParameter, sqlTransaction);
            List<int> allList = new List<int>();
            foreach (DataRow targetDataRow in gameProfileWithPTDataTable.Rows)
            {
                bool found = false;
                foreach (DataRow dataRow in gameProfileDataTable.Rows)
                {
                    if (dataRow["game_profile_id"].Equals(targetDataRow["game_profile_id"]))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    allList.Add(Convert.ToInt32(targetDataRow["game_profile_id"]));
                }
            }

            for (int z = 0; z < allList.Count; z++)
            {
                DataRow dataRow = gameProfileDataTable.NewRow();
                gameProfileDataTable.Rows.Add(dataRow);
                gameProfileDataTable.Rows[gameProfileDataTable.Rows.Count - 1]["game_profile_id"] = allList[z];
                gameProfileDataTable.Rows[gameProfileDataTable.Rows.Count - 1]["game_profile_name"] = GetProfileName(allList[z]);
                gameProfileDataTable.Rows[gameProfileDataTable.Rows.Count - 1]["total"] = 0;
            }

            for (int i = 0; i < gameProfileDataTable.Rows.Count; i++)
            {
                DataRow dataRow = overViewDataTable.NewRow();
                overViewDataTable.Rows.Add(dataRow);
                overViewDataTable.Rows[i]["Game Profile"] = gameProfileDataTable.Rows[i]["game_profile_name"].ToString();
                overViewDataTable.Rows[i]["Sales Update"] = Math.Round(Convert.ToDouble(gameProfileDataTable.Rows[i]["total"].ToString()), 2);
                overViewDataTable.Rows[i]["Average"] = Math.Round(Convert.ToDouble(gameProfileDataTable.Rows[i]["total"].ToString()) / days, 2);
                overViewDataTable.Rows[i]["Sales Forecast for End of the Month"] = Math.Round(Convert.ToDouble(gameProfileDataTable.Rows[i]["total"].ToString()), 2);
                overViewDataTable.Rows[i]["Sales Target"] = Math.Round(GetProfileTarget(int.Parse(gameProfileDataTable.Rows[i]["game_profile_id"].ToString()), periodId, days), 2);
                overViewDataTable.Rows[i]["Difference"] = Math.Round(Convert.ToDouble(overViewDataTable.Rows[i]["Sales Forecast for End of the Month"].ToString()) - Convert.ToDouble(overViewDataTable.Rows[i]["Sales Target"].ToString()), 2);
                if (Convert.ToDouble(overViewDataTable.Rows[i]["Sales Target"]) > 0)
                    overViewDataTable.Rows[i]["% Growth"] = Math.Round((Convert.ToDouble(overViewDataTable.Rows[i]["Difference"]) / Convert.ToDouble(overViewDataTable.Rows[i]["Sales Target"])) * 100, 2);
            }
            GetLastRow(overViewDataTable);
            for (int i = 0; i < overViewDataTable.Rows.Count; i++)
            {
                overViewDataTable.Rows[i]["% Sales"] = Math.Round((Convert.ToDouble(overViewDataTable.Rows[i]["Sales Update"]) / Convert.ToDouble(overViewDataTable.Rows[overViewDataTable.Rows.Count - 1]["Sales Update"])) * 100, 2);
                overViewDataTable.Rows[i]["% Sales Target"] = Math.Round((Convert.ToDouble(overViewDataTable.Rows[i]["Sales Target"]) / Convert.ToDouble(overViewDataTable.Rows[overViewDataTable.Rows.Count - 1]["Sales Target"])) * 100, 2);
            }
            overViewDataTable.Rows[overViewDataTable.Rows.Count - 1].Delete();
            GetLastRow(overViewDataTable);
            overViewDataTable.Rows[overViewDataTable.Rows.Count - 1][overViewDataTable.Columns.Count - 1] = 100 * Convert.ToDouble(overViewDataTable.Rows[overViewDataTable.Rows.Count - 1][overViewDataTable.Columns.Count - 2]) / Convert.ToDouble(overViewDataTable.Rows[overViewDataTable.Rows.Count - 1][overViewDataTable.Columns.Count - 4]);

            overViewDataList = overViewDataTable;
            log.LogMethodExit(overViewDataList);
            return overViewDataList;
        }

        private void GetLastRow(DataTable dataTable)
        {
            log.LogMethodEntry(dataTable);
            int j;
            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);
            dataTable.Rows[dataTable.Rows.Count - 1][0] = "Grand Total";
            for (int i = 1; i < dataTable.Columns.Count; i++)
            {
                double sum = 0;
                for (j = 0; j < dataTable.Rows.Count - 1; j++)
                {
                    if (dataTable.Rows[j][i].ToString() == "")
                        continue;
                    sum = Convert.ToDouble(dataTable.Rows[j][i]) + sum;
                }
                dataTable.Rows[j][i] = Math.Round(sum, 2);
            }
            dataTable.Rows[dataTable.Rows.Count - 1][dataTable.Columns.Count - 1] = 100 * Convert.ToDouble(dataTable.Rows[dataTable.Rows.Count - 1][dataTable.Columns.Count - 2]) / Convert.ToDouble(dataTable.Rows[dataTable.Rows.Count - 1][dataTable.Columns.Count - 3]);
            log.LogMethodExit();
        }

        private string GetProfileName(int id)
        {
            log.LogMethodEntry(id);

            string getProfileNameQuery = @"select profile_name from game_profile
                                                               where game_profile_id = @id";

            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@id", id);

            DataTable profileNameDataTable = dataAccessHandler.executeSelectQuery(getProfileNameQuery, sqlParameters, sqlTransaction);
            log.LogMethodExit(profileNameDataTable.Rows[0]["profile_name"].ToString());
            return profileNameDataTable.Rows[0]["profile_name"].ToString();
        }

        private double GetProfileTarget(int gameProfileId, int periodId, int days)
        {
            log.LogMethodEntry(gameProfileId, periodId, days);
            double target = 0;
            string getTargetQuery = @"select isnull(sum(Target), 0) Target 
                                      from userTarget u, games g
                                      where u.gameid = g.game_id
                                      and g.game_profile_id = @gpid
                                      and u.periodid = @pid";

            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@gpid", gameProfileId);
            sqlParameters[1] = new SqlParameter("@pid", periodId);

            DataTable targetDataTable = dataAccessHandler.executeSelectQuery(getTargetQuery, sqlParameters, sqlTransaction);

            target = Convert.ToDouble(targetDataTable.Rows[0]["Target"]);
            log.LogMethodExit(target);
            return target;
        }
        internal int GetDaysCount(int periodId)
        {
            log.LogMethodEntry(periodId);
            DataTable dateDataTable = FromToDateFinder(periodId);
            DateTime fd = Convert.ToDateTime(dateDataTable.Rows[0]["fromdate"].ToString());
            string fromDate = fd.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime td = Convert.ToDateTime(dateDataTable.Rows[0]["todate"]).AddDays(1);
            string toDate = td.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            int days = td.Subtract(fd).Days;
            log.LogMethodExit(days);
            return days;
        }

        public DataTable LoadProfileData(string profileName, int periodId)
        {
            log.LogMethodEntry(profileName, periodId);
            DataTable dateDataTable = FromToDateFinder(periodId);
            DateTime fd = Convert.ToDateTime(dateDataTable.Rows[0]["fromdate"].ToString());
            string fromDate = fd.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime td = Convert.ToDateTime(dateDataTable.Rows[0]["todate"]).AddDays(1);
            string toDate = td.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            int days = td.Subtract(fd).Days;
            if (days <= 270)
            {
                string loadProfileDataQuery = "exec GamePerformanceByTargets @fd, @td, @gp";

                SqlParameter[] sqlParameters = new SqlParameter[3];
                sqlParameters[0] = new SqlParameter("@fd", fromDate);
                sqlParameters[1] = new SqlParameter("@td", toDate);
                sqlParameters[2] = new SqlParameter("@gp", profileName);

                //getProfileDataTable = new DataTable();
                getProfileDataTable = dataAccessHandler.executeSelectQuery(loadProfileDataQuery, sqlParameters, sqlTransaction);

                getProfileDataTable.Columns.Add("Avg/Day", typeof(double));
                getProfileDataTable.Columns.Add("Daily Target", typeof(double));
                getProfileDataTable.Columns.Add("Weekly Target", typeof(double));
                getProfileDataTable.Columns.Add("Monthly Target", typeof(double));
                getProfileDataTable.Columns.Add("Difference", typeof(double));
                getProfileDataTable.Columns.Add("%", typeof(double));

                int[] a = LastPeriodChecker(periodId);
                DataTable dt1x = PeriodFinder(a[0]);
                DataTable dt2x = PeriodFinder(a[1]);
                DataTable dt3x = PeriodFinder(a[2]);

                string profileQuery = @"select u.*, g.game_name from usertarget u, games g
                                                               where u.gameId = g.game_id
                                                                 and periodid = @id 
                                                                 and u.target is not null
                                                                 and g.game_profile_id in (select game_profile_id 
                                                                                            from game_profile
                                                                                            where (@profile = 'All' or profile_name = @profile))";

                SqlParameter[] getProfilParameters = new SqlParameter[2];
                getProfilParameters[0] = new SqlParameter("@id", periodId);
                getProfilParameters[1] = new SqlParameter("@profile", profileName);

                DataTable dtT = dataAccessHandler.executeSelectQuery(profileQuery, getProfilParameters, sqlTransaction);

                List<string> aL = new List<string>();
                foreach (DataRow drT in dtT.Rows)
                {
                    bool found = false;
                    foreach (DataRow dr in getProfileDataTable.Rows)
                    {
                        if (dr["game_name"].Equals(drT["game_name"]))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        aL.Add(drT["game_name"].ToString());
                    }
                }

                for (int z = 0; z < aL.Count; z++)
                {
                    DataRow dataRow = getProfileDataTable.NewRow();
                    getProfileDataTable.Rows.Add(dataRow);
                    getProfileDataTable.Rows[getProfileDataTable.Rows.Count - 1]["game_name"] = aL[z];
                    getProfileDataTable.Rows[getProfileDataTable.Rows.Count - 1]["total"] = 0;
                }

                int gpId = -1;
                if (profileName != "All")
                {
                    string getGameProfileIdQuery = "select game_profile_id from game_profile where profile_name = @name";
                    SqlParameter[] getGPIdParameters = new SqlParameter[1];
                    getGPIdParameters[0] = new SqlParameter("@name", profileName);
                    gpId = Convert.ToInt32(dataAccessHandler.executeScalar(getGameProfileIdQuery, getGPIdParameters, sqlTransaction));
                }
                GetNumberOfGames(gpId);
                GetTarget(days, periodId, gpId);

                for (int i = 0; i < getProfileDataTable.Rows.Count; i++)
                {
                    try
                    {
                        getProfileDataTable.Rows[i]["Avg/Day"] = Math.Round(Convert.ToDouble(getProfileDataTable.Rows[i]["Total"]) / days, 2);
                    }
                    catch
                    {
                        getProfileDataTable.Rows[i]["Avg/Day"] = 0;
                    }

                    getProfileDataTable.Rows[i]["Difference"] = Math.Round(Convert.ToDouble(getProfileDataTable.Rows[i]["Total"]) - Convert.ToDouble(getProfileDataTable.Rows[i]["Monthly Target"]), 2);
                    if (Convert.ToDouble(getProfileDataTable.Rows[i]["Monthly Target"]) > 0)
                        getProfileDataTable.Rows[i]["%"] = Math.Round((Convert.ToDouble(getProfileDataTable.Rows[i]["Difference"]) / Convert.ToDouble(getProfileDataTable.Rows[i]["Monthly Target"])) * 100, 2);
                }

                GetLastThreePeriod(dt1x, profileName, "c");
                GetLastThreePeriod(dt2x, profileName, "b");
                GetLastThreePeriod(dt3x, profileName, "a");

                if (dt1x.Rows.Count == 0)
                    getProfileDataTable.Columns["c"].ColumnName = "No Period1";
                else
                    getProfileDataTable.Columns["c"].ColumnName = dt1x.Rows[0]["Name"].ToString();
                if (dt2x.Rows.Count == 0)
                    getProfileDataTable.Columns["b"].ColumnName = "No Period2";
                else
                    getProfileDataTable.Columns["b"].ColumnName = dt2x.Rows[0]["Name"].ToString();
                if (dt3x.Rows.Count == 0)
                    getProfileDataTable.Columns["a"].ColumnName = "No Period3";
                else
                    getProfileDataTable.Columns["a"].ColumnName = dt3x.Rows[0]["Name"].ToString();

                GetLastRow(getProfileDataTable);
                log.LogMethodExit(getProfileDataTable);
                return getProfileDataTable;
            }
            log.LogMethodExit();
            return null;
        }

        private DataTable PeriodFinder(int id)
        {
            log.LogMethodEntry(id);

            string periodFinderQuery = @"select periodid, Name, FromDate, todate from userperiod
                                                               where periodid = @id
                                                               order by datediff(day,FromDate,ToDate)";
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@id", id);

            DataTable periodDataTable = dataAccessHandler.executeSelectQuery(periodFinderQuery, sqlParameters, sqlTransaction);
            log.LogMethodExit(periodDataTable);
            return periodDataTable;
        }

        private DataTable FromToDateFinder(int id)
        {
            log.LogMethodEntry(id);

            string dateFinderQuery = @"select periodid, Name, FromDate, todate from userperiod
                                        where periodid = @id";

            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@id", id);

            DataTable dateFinderDataTable = dataAccessHandler.executeSelectQuery(dateFinderQuery, sqlParameters, sqlTransaction);
            log.LogMethodExit(dateFinderDataTable);
            return dateFinderDataTable;
        }

        private void GetNumberOfGames(int gameProfileId)
        {
            log.LogMethodEntry(gameProfileId);
            string gamesCountQuery = @"select game_name, count(distinct machine_id) as number from machines m, games g
                                                                where m.game_id = g.game_id 
                                                                and (g.game_profile_id = @gpId or @gpId = -1)
                                                                group by game_name";
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@gpId", gameProfileId);

            DataTable gamesCountDataTable = dataAccessHandler.executeSelectQuery(gamesCountQuery, sqlParameters, sqlTransaction);

            foreach (DataRow dataRow in gamesCountDataTable.Rows)
            {
                foreach (DataRow gameDataRow in getProfileDataTable.Rows)
                {
                    if (gameDataRow["game_name"].Equals(dataRow["game_name"]))
                    {
                        gameDataRow["number"] = dataRow["number"];
                        break;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void GetLastThreePeriod(DataTable dataTable, string profileName, string column)
        {
            log.LogMethodEntry(dataTable, profileName, column);
            if (dataTable.Rows.Count == 0)
            {
                log.LogMethodExit(null);
                return;
            }

            DateTime fromDate = Convert.ToDateTime(dataTable.Rows[0]["fromdate"].ToString());
            DateTime toDate = Convert.ToDateTime(dataTable.Rows[0]["todate"].ToString()).AddDays(1);

            string query = "exec GamePerformanceByTargets @fd, @td, @gp";

            SqlParameter[] sqlParameters = new SqlParameter[3];
            sqlParameters[0] = new SqlParameter("@fd", fromDate);
            sqlParameters[1] = new SqlParameter("@td", toDate);
            sqlParameters[2] = new SqlParameter("@gp", profileName);

            DataTable lastThreePeriodDataTable = dataAccessHandler.executeSelectQuery(query, sqlParameters, sqlTransaction);

            foreach (DataRow dataRow in lastThreePeriodDataTable.Rows)
            {
                foreach (DataRow gameDataRow in getProfileDataTable.Rows)
                {
                    if (gameDataRow["game_name"].Equals(dataRow["game_name"]))
                    {
                        gameDataRow[column] = Math.Round(Convert.ToDouble(dataRow["Total"]), 2);
                        break;
                    }
                }
            }
            log.LogMethodExit();
        }

        private int[] LastPeriodChecker(int periodId)
        {
            log.LogMethodEntry(periodId);
            int[] a = new int[3];
            string periodCheckerQuery = @"select * from userperiod
                                                               where fromdate < (select fromdate from userperiod
                                                                                 where periodid = @id)
                                                               order by fromdate desc";

            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@id", periodId);

            DataTable periodCheckerDataTable = dataAccessHandler.executeSelectQuery(periodCheckerQuery, sqlParameter, sqlTransaction);

            if (periodCheckerDataTable.Rows.Count >= 3)
            {
                a[0] = Convert.ToInt32(periodCheckerDataTable.Rows[0]["periodid"]);
                a[1] = Convert.ToInt32(periodCheckerDataTable.Rows[1]["periodid"]);
                a[2] = Convert.ToInt32(periodCheckerDataTable.Rows[2]["periodid"]);
            }
            else if (periodCheckerDataTable.Rows.Count == 2)
            {
                a[0] = Convert.ToInt32(periodCheckerDataTable.Rows[0]["periodid"]);
                a[1] = Convert.ToInt32(periodCheckerDataTable.Rows[1]["periodid"]);
                a[2] = 0;
            }
            else if (periodCheckerDataTable.Rows.Count == 1)
            {
                a[0] = Convert.ToInt32(periodCheckerDataTable.Rows[0]["periodid"]);
                a[1] = 0;
                a[2] = 0;
            }
            else
            {
                a[0] = 0;
                a[1] = 0;
                a[2] = 0;
            }
            log.LogMethodExit(a);
            return a;
        }

        private void GetTarget(double days, int periodid, int gpId)
        {
            log.LogMethodEntry(days, periodid, gpId);
            double target = 0;
            string getTargetQuery = @"select isnull(Target, 0) Target, game_name 
                                                                from userTarget u, games g
                                                               where u.gameid = g.game_id
                                                                and (g.game_profile_id = @gpId or @gpId = -1) 
                                                                and u.periodid = @periodid";

            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@gpId", gpId);
            sqlParameters[1] = new SqlParameter("@periodid", periodid);

            DataTable dataTable = dataAccessHandler.executeSelectQuery(getTargetQuery, sqlParameters, sqlTransaction);

            foreach (DataRow targetDataRow in getProfileDataTable.Rows)
            {
                targetDataRow["Daily Target"] =
                targetDataRow["Monthly Target"] =
                targetDataRow["Weekly Target"] = 0;
            }

            foreach (DataRow dataRow in dataTable.Rows)
            {
                foreach (DataRow dataRowTarget in getProfileDataTable.Rows)
                {
                    if (dataRowTarget["game_name"].ToString().Equals(dataRow["game_name"].ToString()))
                    {
                        target = Convert.ToDouble(dataRow["Target"].ToString());
                        dataRowTarget["Daily Target"] = Math.Round(target / days, 2);
                        dataRowTarget["Monthly Target"] = Math.Round(target, 2);
                        dataRowTarget["Weekly Target"] = Math.Round((target / days) * 7, 2);

                        break;
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
