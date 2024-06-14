/********************************************************************************************
 * Project Name - Reports
 * Description  - Data Handler of WirelessDashBoardBL for Reports module
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.80        15-Jun-2020   Vikas Dwivedi        Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Reports
{
    public class WirelessDashBoardDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;

        /// <summary>
        /// Parameterized Constructor 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public WirelessDashBoardDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        public List<WirelessDashBoardDTO> GetHubDetailsForWirelessDashBoard(DateTime fromDate, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(fromDate, sqlTransaction);
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            List<WirelessDashBoardDTO> wirelessDashBoardDTOList = new List<WirelessDashBoardDTO>();
            string query = @"select h.master_id, master_name hub_name, port_number, h.notes address, count(*) Machine_Count  " +
                                "from masters h, machines m " +
                                "where h.master_id = m.master_id " +
                                "and m.active_flag = 'Y' " +
                                "group by h.master_id, master_name, port_number, h.notes";
            try
            {
                sqlParameters.Add(new SqlParameter("@fromdate", fromDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                DataTable wireLessDashBoardMachineData = dataAccessHandler.executeSelectQuery(query, sqlParameters.ToArray(), sqlTransaction);
                if (wireLessDashBoardMachineData.Rows.Count > 0)
                {
                    wirelessDashBoardDTOList = new List<WirelessDashBoardDTO>();
                    foreach (DataRow wirelessDashBoardDataRow in wireLessDashBoardMachineData.Rows)
                    {
                        // call the method inside for loop
                        WirelessDashBoardDTO wirelessDashBoardDataObject = new WirelessDashBoardDTO();
                        wirelessDashBoardDataObject.HubId = wirelessDashBoardDataRow["master_id"] == DBNull.Value ? -1 : Convert.ToInt32(wirelessDashBoardDataRow["master_id"].ToString());
                        wirelessDashBoardDataObject.HubName = wirelessDashBoardDataRow["hub_name"] == DBNull.Value ? string.Empty : wirelessDashBoardDataRow["hub_name"].ToString();
                        wirelessDashBoardDataObject.PortNumber = wirelessDashBoardDataRow["port_number"] == DBNull.Value ? (int?)null : Convert.ToInt32(wirelessDashBoardDataRow["port_number"]);
                        wirelessDashBoardDataObject.Address = wirelessDashBoardDataRow["address"] == DBNull.Value ? string.Empty : wirelessDashBoardDataRow["address"].ToString();
                        wirelessDashBoardDataObject.MachineCount = wirelessDashBoardDataRow["Machine_Count"] == DBNull.Value ? -1 : Convert.ToInt32(wirelessDashBoardDataRow["Machine_Count"].ToString());
                        wirelessDashBoardDataObject.TotalPolls = GetTotalPolls(wirelessDashBoardDataObject, fromDate);
                        wirelessDashBoardDataObject.Failures = GetAdditionalFields(wirelessDashBoardDataObject, fromDate);
                        wirelessDashBoardDataObject.FailurePercent = GetAdditionalFields(wirelessDashBoardDataObject, fromDate);
                        wirelessDashBoardDataObject.TrxFailures = GetAdditionalFields(wirelessDashBoardDataObject, fromDate);
                        wirelessDashBoardDataObject.TrxFailuresPercent = GetAdditionalFields(wirelessDashBoardDataObject, fromDate);
                        wirelessDashBoardDataObject.RcvFailures = GetAdditionalFields(wirelessDashBoardDataObject, fromDate);
                        wirelessDashBoardDataObject.RcvFailuresPercent = GetAdditionalFields(wirelessDashBoardDataObject, fromDate);
                        wirelessDashBoardDTOList.Add(wirelessDashBoardDataObject);
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            return wirelessDashBoardDTOList;
        }

        /// <summary>
        /// It will return the value of TotalPolls
        /// </summary>
        private int GetTotalPolls(WirelessDashBoardDTO wirelessDashBoardDTO, DateTime fromDate)
        {
            log.LogMethodEntry(wirelessDashBoardDTO, fromDate);
            TimeSpan ts = DateTime.Now - fromDate;
            int durationInSecs = (int)ts.TotalSeconds; // (ts.Days * 24 * 60 + ts.Hours * 60 + ts.Minutes) * 60 + ts.Seconds;
            int PollFrequency = 30;
            int MinTimeBetweenPolls = 0;
            try
            {
                PollFrequency = Convert.ToInt32(Common.Utilities.getParafaitDefaults("LOG_FREQUENCY_IN_POLLS"));
            }
            catch { }

            try
            {
                MinTimeBetweenPolls = Convert.ToInt32(Common.Utilities.getParafaitDefaults("MIN_TIME_BETWEEN_POLLS"));
            }
            catch { }

            const int avgPollTimeInms = 130;
            string totalPollsQuery = @"select count(*) " +
                    "from machines m " +
                    "where m.master_id = @master_id " +
                    "and m.active_flag = 'Y' ";

            SqlParameter[] selectTotalPollsParameters = new SqlParameter[1];
            selectTotalPollsParameters[0] = new SqlParameter("@master_id", wirelessDashBoardDTO.HubId);
            int totalMachines = Convert.ToInt32(dataAccessHandler.executeScalar(totalPollsQuery, selectTotalPollsParameters, sqlTransaction));

            double PollsPerSecond = 1000.0 / (PollFrequency * Math.Max(MinTimeBetweenPolls, avgPollTimeInms * totalMachines));

            int totalPolls = Convert.ToInt32(durationInSecs * PollsPerSecond);

            wirelessDashBoardDTO.TotalPolls = totalPolls;

            log.LogMethodExit();
            return totalPolls;
        }

        /// <summary>
        /// It will return the value of Failure, Failure%, Trx_Failure, Trx_Failure%, Rcv_Failure and Rcv_Failure%
        /// </summary>
        private int GetAdditionalFields(WirelessDashBoardDTO wirelessDashBoardDTO, DateTime fromDate)
        {
            log.LogMethodEntry(wirelessDashBoardDTO, fromDate);
            TimeSpan ts = DateTime.Now - fromDate;
            int durationInSecs = (int)ts.TotalSeconds; // (ts.Days * 24 * 60 + ts.Hours * 60 + ts.Minutes) * 60 + ts.Seconds;
            int PollFrequency = 30;
            int MinTimeBetweenPolls = 0;
            try
            {
                PollFrequency = Convert.ToInt32(Common.Utilities.getParafaitDefaults("LOG_FREQUENCY_IN_POLLS"));
            }
            catch { }

            try
            {
                MinTimeBetweenPolls = Convert.ToInt32(Common.Utilities.getParafaitDefaults("MIN_TIME_BETWEEN_POLLS"));
            }
            catch { }

            const int avgPollTimeInms = 130;

            string totalPollsQuery = @"select count(*) " +
                    "from machines m " +
                    "where m.master_id = @master_id " +
                    "and m.active_flag = 'Y' ";

            SqlParameter[] selectTotalPollsParameters = new SqlParameter[1];
            selectTotalPollsParameters[0] = new SqlParameter("@master_id", wirelessDashBoardDTO.HubId);
            int totalMachines = Convert.ToInt32(dataAccessHandler.executeScalar(totalPollsQuery, selectTotalPollsParameters, sqlTransaction));

            double PollsPerSecond = 1000.0 / (PollFrequency * Math.Max(MinTimeBetweenPolls, avgPollTimeInms * totalMachines));

            int TotalPolls = Convert.ToInt32(durationInSecs * PollsPerSecond);

            wirelessDashBoardDTO.TotalPolls = TotalPolls;
            //dgvHubs.Columns["TotalPolls"].DefaultCellStyle.Format = "N0";

            string failureQuery = "select count(1) " +
                            "from machines m, communicationLog l " +
                            "where m.machine_id = l.MachineId " +
                            "and m.master_id = @master_id " +
                            "and l.Timestamp >= @fromDate " +
                            "and m.active_flag = 'Y' ";

            SqlParameter[] selectFailureParameters = new SqlParameter[2];
            selectFailureParameters[0] = new SqlParameter("@master_id", wirelessDashBoardDTO.HubId);
            selectFailureParameters[1] = new SqlParameter("@fromDate", fromDate);

            int failures = Convert.ToInt32(dataAccessHandler.executeScalar(failureQuery, selectFailureParameters, sqlTransaction));
            wirelessDashBoardDTO.Failures = failures;
            if ((TotalPolls * totalMachines) != 0)
                wirelessDashBoardDTO.FailurePercent = Math.Round(100.0 * failures / (TotalPolls * totalMachines), 2);

            string trxFailureQuery = "select count(1) " +
                            "from machines m, communicationLog l " +
                            "where m.machine_id = l.MachineId " +
                            "and m.master_id = @master_id " +
                            "and ReceivedData like 'TRANSMISSION%' " +
                            "and l.Timestamp >= @fromDate " +
                            "and m.active_flag = 'Y' ";

            SqlParameter[] selectTrxFailureParameters = new SqlParameter[2];
            selectTrxFailureParameters[0] = new SqlParameter("@master_id", wirelessDashBoardDTO.HubId);
            selectTrxFailureParameters[1] = new SqlParameter("@fromDate", fromDate);

            int Trxfailures = Convert.ToInt32(dataAccessHandler.executeScalar(trxFailureQuery, selectTrxFailureParameters, sqlTransaction));
            wirelessDashBoardDTO.TrxFailures = Trxfailures;
            if (failures != 0)
                wirelessDashBoardDTO.TrxFailuresPercent = Math.Round(100.0 * Trxfailures / failures, 2);

            string rcvFailureQuery = "select count(1) " +
                            "from machines m, communicationLog l " +
                            "where m.machine_id = l.MachineId " +
                            "and m.master_id = @master_id " +
                            "and ReceivedData like 'HUB RECEIVE%' " +
                            "and l.Timestamp >= @fromDate " +
                            "and m.active_flag = 'Y' ";

            SqlParameter[] selectRcvFailureParameters = new SqlParameter[2];
            selectRcvFailureParameters[0] = new SqlParameter("@master_id", wirelessDashBoardDTO.HubId);
            selectRcvFailureParameters[1] = new SqlParameter("@fromDate", fromDate);

            int Rcvfailures = Convert.ToInt32(dataAccessHandler.executeScalar(rcvFailureQuery, selectRcvFailureParameters, sqlTransaction));
            wirelessDashBoardDTO.RcvFailures = Rcvfailures;
            if (failures != 0)
                wirelessDashBoardDTO.RcvFailuresPercent = Math.Round(100.0 * Rcvfailures / failures, 2);

            log.LogMethodExit();
            return failures;
        }

        internal List<WirelessDashBoardDTO> GetMachineDetailsForWirelessDashBoard(int masterId, DateTime fromDate, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(masterId, fromDate, sqlTransaction);
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            List<WirelessDashBoardDTO> wirelessDashBoardDTOList = new List<WirelessDashBoardDTO>();
            string query = @"select Machine_id, Machine_name, m.Machine_address, count(l.machineAddress) total_failures, sum(case when receivedData like 'TRANSMISSION%' then 1 else 0 end) Trx_Failures, " +
                                "sum(case when receivedData like 'HUB RECEIVE%' then 1 else 0 end) Rcv_Failures " +
                                "from machines m left outer join communicationLog l " +
                                "on m.machine_id = l.MachineId " +
                                "and l.Timestamp >= @fromDate " +
                                "where (m.master_id = @master_id or @master_id = -1) " +
                                "and m.active_flag = 'Y' " +
                                "group by machine_id, m.machine_address, Machine_name order by 3 desc";
            try
            {
                sqlParameters.Add(new SqlParameter("@master_id", masterId));
                sqlParameters.Add(new SqlParameter("@fromdate", fromDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                DataTable wireLessDashBoardMachineData = dataAccessHandler.executeSelectQuery(query, sqlParameters.ToArray(), sqlTransaction);
                if (wireLessDashBoardMachineData.Rows.Count > 0)
                {
                    wirelessDashBoardDTOList = new List<WirelessDashBoardDTO>();
                    foreach (DataRow wirelessDashBoardDataRow in wireLessDashBoardMachineData.Rows)
                    {
                        WirelessDashBoardDTO wirelessDashBoardDataObject = new WirelessDashBoardDTO();
                        wirelessDashBoardDataObject.MachineId = wirelessDashBoardDataRow["Machine_Id"] == DBNull.Value ? -1 : Convert.ToInt32(wirelessDashBoardDataRow["Machine_Id"].ToString());
                        wirelessDashBoardDataObject.MachineName = wirelessDashBoardDataRow["Machine_name"] == DBNull.Value ? string.Empty : wirelessDashBoardDataRow["Machine_name"].ToString();
                        wirelessDashBoardDataObject.MachineAddress = wirelessDashBoardDataRow["Machine_address"] == DBNull.Value ? string.Empty : wirelessDashBoardDataRow["Machine_address"].ToString();
                        wirelessDashBoardDataObject.TotalFailures = wirelessDashBoardDataRow["total_failures"] == DBNull.Value ? -1 : Convert.ToInt32(wirelessDashBoardDataRow["total_failures"].ToString());
                        wirelessDashBoardDataObject.TrxFailures = wirelessDashBoardDataRow["Trx_Failures"] == DBNull.Value ? -1 : Convert.ToInt32(wirelessDashBoardDataRow["Trx_Failures"].ToString());
                        wirelessDashBoardDataObject.RcvFailures = wirelessDashBoardDataRow["Rcv_Failures"] == DBNull.Value ? -1 : Convert.ToInt32(wirelessDashBoardDataRow["Rcv_Failures"].ToString());
                        wirelessDashBoardDTOList.Add(wirelessDashBoardDataObject);
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            return wirelessDashBoardDTOList;
        }
    }
}
