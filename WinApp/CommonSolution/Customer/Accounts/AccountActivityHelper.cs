using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Game;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Account activity and game play helper class. Retrieves the information from HQ
    /// </summary>
    public class AccountActivityHelper
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataSet dataSet;
        private int accountId;
        private string tagNumber;
        private int siteId;
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="tagNumber"></param>
        /// <param name="siteId"></param>
        public AccountActivityHelper(int accountId, string tagNumber, int siteId)
        {
            log.LogMethodEntry(accountId, tagNumber);
            this.accountId = accountId;
            this.tagNumber = tagNumber;
            this.siteId = siteId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Load account activity data from the server
        /// </summary>
        public void Initialize()
        {
            log.LogMethodEntry();
            RemotingClient CardRoamingRemotingClient = null;
            CardRoamingRemotingClient = new RemotingClient();
            if (CardRoamingRemotingClient != null)
            {
                dataSet = CardRoamingRemotingClient.GetServerCardActivity(tagNumber, siteId);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the account activity across all the sites
        /// </summary>
        /// <returns></returns>
        public List<AccountActivityDTO> GetServerAccountActivityDTOList()
        {
            log.LogMethodEntry();
            List<AccountActivityDTO> serverAccountActivityDTOList = null;
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable centralAccountActivityTable = dataSet.Tables[0];
                if (centralAccountActivityTable.Rows.Count > 0)
                {
                    serverAccountActivityDTOList = new List<AccountActivityDTO>();
                    AccountActivityDataHandler accountActivityViewDataHandler = new AccountActivityDataHandler(null);
                    foreach (DataRow dataRow in centralAccountActivityTable.Rows)
                    {
                        AccountActivityDTO accountActivityDTO = accountActivityViewDataHandler.GetAccountActivityDTO(dataRow);
                        serverAccountActivityDTOList.Add(accountActivityDTO);
                    }
                }
            }
            log.LogMethodExit(serverAccountActivityDTOList);
            return serverAccountActivityDTOList;
        }

        /// <summary>
        /// Returns the account game metric view across sites
        /// </summary>
        /// <param name="detailed"></param>
        /// <returns></returns>
        public List<GamePlayDTO> GetServerGamePlayDTOList(bool detailed)
        {
            log.LogMethodEntry();
            List<GamePlayDTO> serverGamePlayDTOList = null;
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable centralGameMetricTable = null;
                if (detailed)
                {
                    centralGameMetricTable = dataSet.Tables[2];
                }
                else
                {
                    centralGameMetricTable = dataSet.Tables[1];
                }
                if (centralGameMetricTable.Columns.Contains("task_id") == false)
                {
                    centralGameMetricTable.Columns.Add("task_id");
                }
                if (centralGameMetricTable.Rows.Count > 0)
                {
                    serverGamePlayDTOList = new List<GamePlayDTO>();
                    GamePlaySummaryDataHandler accountGameMetricViewDataHandler = new GamePlaySummaryDataHandler(null);
                    foreach (DataRow dataRow in centralGameMetricTable.Rows)
                    {
                        GamePlayDTO gamePlayDTO = accountGameMetricViewDataHandler.GetGamePlayDTO(dataRow, detailed);
                        serverGamePlayDTOList.Add(gamePlayDTO);
                    }
                }
            }
            log.LogMethodExit(serverGamePlayDTOList);
            return serverGamePlayDTOList;
        }

        /// <summary>
        /// Returns the account activity across all the sites for the web services call
        /// </summary>
        /// <param name="companyKey"></param>
        /// <param name="cardNumber"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public List<AccountActivityDTO> GetServerAccountActivityDTOList(string companyKey, string cardNumber, int siteId)
        {
            log.LogMethodEntry(companyKey, cardNumber, siteId);

            RemotingClient CardRoamingRemotingClient = null;
            CardRoamingRemotingClient = new RemotingClient(true);
            if (CardRoamingRemotingClient != null)
            {
                dataSet = CardRoamingRemotingClient.GetServerCardActivityFromWS(companyKey, tagNumber, siteId);
            }

            List<AccountActivityDTO> serverAccountActivityDTOList = null;
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable centralAccountActivityTable = dataSet.Tables[0];
                if (centralAccountActivityTable.Rows.Count > 0)
                {
                    serverAccountActivityDTOList = new List<AccountActivityDTO>();
                    AccountActivityDataHandler accountActivityViewDataHandler = new AccountActivityDataHandler(null);
                    foreach (DataRow dataRow in centralAccountActivityTable.Rows)
                    {
                        AccountActivityDTO accountActivityDTO = accountActivityViewDataHandler.GetAccountActivityDTO(dataRow);
                        serverAccountActivityDTOList.Add(accountActivityDTO);
                    }
                }
            }
            log.LogMethodExit(serverAccountActivityDTOList);
            return serverAccountActivityDTOList;
        }
    }
}
