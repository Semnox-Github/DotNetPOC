/********************************************************************************************
 * Project Name - AccountService
 * Description  - Consolidate Cards for the Consolidate Cards
 * 
 **************
 **Version Log
 **************
 *Version    Date          Modified By            Remarks          
 *********************************************************************************************
 *2.80.0     12-Mar-2020   Girish Kundar          Created
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.POS;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This class has method to consolidate the cards
    /// </summary>
    public class ConsolidateCardBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private TransactionServiceDTO transactionServiceDTO;
        private ConsolidateCardBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="transactionServiceDTO"></param>
        public ConsolidateCardBL(ExecutionContext executionContext, TransactionServiceDTO transactionServiceDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.transactionServiceDTO = transactionServiceDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Consolidates the list of cards
        /// </summary>
        public void CardConsolidate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            SqlConnection sqlConnection = null;
            Utilities utilities = GetUtility();
            SqlTransaction parafaitDBTrx;
            if (sqlTransaction == null)
            {
                sqlConnection = utilities.createConnection();
                parafaitDBTrx = sqlConnection.BeginTransaction();
            }
            else
            {
                parafaitDBTrx = sqlTransaction;
            }
            try
            {
                string message = string.Empty;
                TaskProcs taskProcs = new TaskProcs(utilities);
                List<Card> consolidateCardsList = new List<Card>();
                if (transactionServiceDTO.AccountDTOList != null && transactionServiceDTO.AccountDTOList.Any())
                {
                    foreach (AccountDTO accountDTO in transactionServiceDTO.AccountDTOList)
                    {
                        Card card = new Card(accountDTO.AccountId, executionContext.GetUserId(), utilities, parafaitDBTrx);
                        consolidateCardsList.Add(card);
                    }
                    if (!taskProcs.Consolidate(consolidateCardsList, consolidateCardsList.Count, transactionServiceDTO.Remarks, ref message, parafaitDBTrx, transactionServiceDTO.InvalidateSourceCard, transactionServiceDTO.MergeHistoryDuringSourceInactivation))
                    {
                        log.Error("CONSOLIDATE- cards  has error " + message);
                        throw new Exception(message);
                    }
                    TransactionBL transactionBL = new TransactionBL(executionContext, taskProcs.TransactionId, parafaitDBTrx);
                    if (transactionBL.TransactionDTO != null)
                    {
                        transactionBL.TransactionDTO.ExternalSystemReference = taskProcs.TransactionId.ToString();
                        transactionBL.Save(parafaitDBTrx);
                    }
                    log.LogMethodExit();
                }
                else
                {
                    log.Error("No cards selected for consolidation");
                    throw new ValidationException("Invalid inputs : Mininum two cards needed for consildation");
                }
                if (sqlTransaction == null)
                {
                    parafaitDBTrx.Commit();
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                if (sqlTransaction == null)  //SQLTransaction handled locally
                {
                    parafaitDBTrx.Rollback();
                }
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }
        internal Utilities GetUtility()
        {
            log.LogMethodEntry();
            Utilities utilities = new Utilities();
            utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
            utilities.ParafaitEnv.POSMachineId = executionContext.GetMachineId();
            POSMachineContainerDTO pOSMachineContainerDTO = POSMachineContainerList.GetPOSMachineContainerDTOOrDefault(executionContext.GetSiteId(), "CenterEdge", "", -1);
            if (pOSMachineContainerDTO != null)
            {
                utilities.ParafaitEnv.SetPOSMachine("", pOSMachineContainerDTO.POSName);
            }
            else
            {
                utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
            }
            //List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
            //searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_NAME, "CenterEdge"));
            //List<POSMachineDTO> POSMachineDTOList = new POSMachineList().GetAllPOSMachines(searchParameters);
            //if (POSMachineDTOList != null || POSMachineDTOList.Any())
            //{
            //    utilities.ParafaitEnv.SetPOSMachine("", (POSMachineDTOList[0].POSName));
            //}
            //else
            //{
            //    utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
            //}
            utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
            utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
            utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
            utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
            UserContainerDTO user = UserContainerList.GetUserContainerDTOOrDefault(executionContext.GetUserId(), "", executionContext.GetSiteId());
            utilities.ParafaitEnv.User_Id = user.UserId;
            utilities.ParafaitEnv.RoleId = user.RoleId;
            utilities.ExecutionContext.SetUserId(user.LoginId);
            utilities.ParafaitEnv.Initialize();
            log.LogMethodExit();
            return utilities;
        }
    }
}
