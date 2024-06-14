/********************************************************************************************
* Project Name - Loyalty
* Description  - LoyaltyPrograms - Base class for all the Loyalty programs
* 
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.120.0     12-Dec-2020      Girish Kundar       Created
*********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.ThirdPartyLoyalty
{
    /// <summary>
    /// LoyaltyPrograms
    /// </summary>
    public class LoyaltyPrograms
    {
        private static readonly Semnox.Parafait.logging.Logger log =
            new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parafait utilities.
        /// </summary>
        protected Utilities utilities;

        internal void ExecuteAction(Action method)
        {
            log.LogMethodEntry();
            try
            {
                method();
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                throw;
            }
            catch (AggregateException e)
            {
                if (e.InnerException is OperationCanceledException)
                {
                    log.Error(e);
                    throw;
                }
            }
            catch (OperationCanceledException opx)
            {
                log.Error(opx);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }

            log.LogMethodExit();
        }
        /// <summary>
        /// LoyaltyPrograms
        /// </summary>
        /// <param name="_utilities"></param>
        public LoyaltyPrograms(Utilities _utilities)
        {
            utilities = _utilities;
        }


        /// <summary>
        /// LoadLoyaltyConfigs
        /// </summary>

        public virtual void LoadLoyaltyConfigs()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// PostCustomers
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual string PostCustomers(string json)
        {
            log.LogMethodEntry();
            log.LogMethodExit(false);
            return null;
        }

        /// <summary>
        /// GetCustomers
        /// </summary>
        /// <param name="phoneNumberOrEmail"></param>
        /// <returns></returns>
        public virtual Task<LoyaltyMemberDetails> GetCustomers(string phoneNumberOrEmail)
        {
            log.LogMethodEntry(phoneNumberOrEmail);
            log.LogMethodExit(false);
            return null;
        }

        /// <summary>
        /// ValidateCouponRedeemable
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual string ValidateCouponRedeemable(string json)
        {
            log.LogMethodEntry();
            log.LogMethodExit(false);
            return null;
        }

        /// <summary>
        /// PostTransaction by default returns success
        /// </summary>
        /// <returns></returns>
        public virtual string PostTransaction()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return "Success";
        }

        /// <summary>
        /// RedeemPoints
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual string RedeemPoints(string json)
        {
            log.LogMethodEntry();
            log.LogMethodExit(false);
            return null;
        }

        /// <summary>
        /// ValidatePointsRedeemable
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual string ValidatePointsRedeemable(string json)
        {
            log.LogMethodEntry();
            log.LogMethodExit(false);
            return null;
        }

        /// <summary>
        /// RedeemCoupon
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual string RedeemCoupon(string json)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return null;
        }

        /// <summary>
        /// SendOTP
        /// </summary>
        /// <param name="phoneNumberOrEmail"></param>
        /// <returns></returns>
        public virtual string SendOTP(string phoneNumberOrEmail)
        {
            log.LogMethodEntry(phoneNumberOrEmail);
            log.LogMethodExit(false);
            return null;
        }


        /// <summary>
        /// UpdateLoyaltyNumber
        /// </summary>
        /// <param name="trxId"></param>
        /// <param name="reference"></param>
        /// <param name="sqlTransaction"></param>
        //public void UpdateLoyaltyNumber(int trxId, string reference, SqlTransaction sqlTransaction = null)
        //{
        //    log.LogMethodEntry(reference, trxId, sqlTransaction);
        //    try
        //    {
        //        TransactionBL transactionBL = new TransactionBL(utilities.ExecutionContext, trxId);
        //        if (transactionBL.TransactionDTO != null)
        //        {
        //            transactionBL.TransactionDTO.ExternalSystemReference = reference;
        //        }
        //        transactionBL = new TransactionBL(utilities.ExecutionContext, transactionBL.TransactionDTO, sqlTransaction);
        //        transactionBL.Save(sqlTransaction);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Error in updating Loyalty no", ex);
        //    }
        //    log.LogMethodExit(null);
        //}


        ///// <summary>
        ///// UpdateConcurrentRequests
        ///// </summary>
        ///// <param name="requestId"></param>
        ///// <param name="endTime"></param>
        //internal void UpdateConcurrentRequests(int requestId, string status ,DateTime endTime)
        //{
        //    log.LogMethodEntry(requestId, status, endTime);
        //    try
        //    {
        //        if (requestId > 0)
        //        {
        //            //utilities.executeNonQuery(@"Update  ConcurrentRequests
        //            //                                Set EndTime = @EndTime,
        //            //                                LastUpdatedBy =@LastUpdatedBy
        //            //                          where RequestId= @RequestId",
        //            //                           new SqlParameter("@EndTime", endTime.ToString("yyyy-MM-dd HH:mm:ss.fff")),
        //            //                           new SqlParameter("@RequestId", requestId),
        //            //                           new SqlParameter("@LastUpdatedBy", utilities.ExecutionContext.GetUserId()));
        //            //log.LogMethodExit();
        //            ConcurrentRequests concurrentRequests = new ConcurrentRequests(utilities.ExecutionContext, requestId);
        //            ConcurrentRequestsDTO concurrentRequestsDTO = concurrentRequests.GetconcurrentRequests;
        //            concurrentRequestsDTO.RequestId = requestId;
        //            concurrentRequestsDTO.Phase = "Complete";
        //            concurrentRequestsDTO.Status = status;
        //            concurrentRequestsDTO.EndTime = DateTime.Now.ToString();
        //            concurrentRequests = new ConcurrentRequests(utilities.ExecutionContext, concurrentRequestsDTO);
        //            concurrentRequests.Save();
        //            log.Debug("concurrentRequests is updated");
        //            if (status == "Normal")
        //            {
        //                ConcurrentPrograms concurrentPrograms = new ConcurrentPrograms(concurrentRequestsDTO.ProgramId);
        //                concurrentPrograms.GetconcurrentPrograms.LastExecutedOn = concurrentRequestsDTO.StartTime;
        //                concurrentPrograms.GetconcurrentPrograms.LastUpdatedDate = endTime;
        //                concurrentPrograms = new ConcurrentPrograms(concurrentPrograms.GetconcurrentPrograms);
        //                concurrentPrograms.Save();
        //                log.Debug("ConcurrentPrograms is updated");
        //            }
        //        }
        //        else
        //        {
        //            log.Debug("ConcurrentRequests is not created . Please check the error");
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        log.Error(ex);
        //        log.Debug("Failed to save ConcurrentRequests EnDTime.");
        //    }
        //}

        //internal DateTime GetLastUpdatedTime()
        //{
        //    log.LogMethodEntry();
        //    DateTime lastUpdateTime = ServerDateTime.Now;
        //    try
        //    {
        //        DataTable logDT = utilities.executeDataTable(@"SELECT top 1 ActualStartTime, RequestId
        //                                                     FROM ConcurrentRequests 
        //                                                    WHERE programId = (SELECT top 1 ProgramId 
        //                                                                          FROM ConcurrentPrograms 
					   //                                                          WHERE ProgramName = 'ThirdPartyLoyaltyProgram' 
					   //                                                            AND Active=1)
        //                                                      AND Phase = 'Complete' 
        //                                                      AND status = 'Normal'
        //                                                   ORDER BY EndTime desc");
        //        if (logDT != null && logDT.Rows.Count > 0 && logDT.Rows[0][0] != DBNull.Value)
        //        {
        //            lastUpdateTime = Convert.ToDateTime((logDT.Rows[0][0]));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        log.Debug("Failed to get transaction details at GetTransactionDetails() method");
        //    }
        //    log.LogMethodExit(lastUpdateTime);
        //    return lastUpdateTime;
        //}

        //internal int CreateConcurrentRequest()
        //{
        //    log.LogMethodEntry();
        //    int programId = -1;
        //    int concurrentRequestId = -1;
        //    List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>> searchByProgramsParameters = new List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>>();
        //    searchByProgramsParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
        //    searchByProgramsParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.PROGRAM_NAME, "ThirdPartyLoyaltyProgram"));
        //    searchByProgramsParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.ACTIVE_FLAG, "1"));
        //    ConcurrentProgramList concurrentProgramList = new ConcurrentProgramList(utilities.ExecutionContext);
        //    List<ConcurrentProgramsDTO> concurrentProgramsDTOList = concurrentProgramList.GetAllConcurrentPrograms(searchByProgramsParameters);
        //    if (concurrentProgramsDTOList != null && concurrentProgramsDTOList.Any())
        //    {
        //        log.Debug("Concurrent program ID :" + concurrentProgramsDTOList.First().ProgramId);
        //        programId = concurrentProgramsDTOList.First().ProgramId;
        //    }
        //    if (programId > 0)
        //    {
        //        ConcurrentRequestsDTO concurrentRequestsDTO = new ConcurrentRequestsDTO(-1, programId, -1, null,
        //                              utilities.ExecutionContext.GetUserId(), null,
        //                              DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), null,
        //                              "Running", "Normal", false, string.Empty, string.Empty,
        //                              string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
        //                              string.Empty, string.Empty, -1, -1);
        //        ConcurrentRequests concurrentRequests = new ConcurrentRequests(utilities.ExecutionContext, concurrentRequestsDTO);
        //        concurrentRequests.Save();
        //        concurrentRequestsDTO = concurrentRequests.GetconcurrentRequests;
        //        log.Debug("Concurrent Request ID :" + concurrentRequestsDTO.RequestId);
        //        concurrentRequestId = concurrentRequestsDTO.RequestId;
        //    }
        //    log.LogMethodExit(concurrentRequestId);
        //    return concurrentRequestId;
        //}
    }
}
