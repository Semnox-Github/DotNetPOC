/********************************************************************************************
* Project Name - Transaction
* Description  - LocalTransactionUseCase
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
2.110        13-Nov-2020       Girish Kundar      Created :Waiver Link changes
2.130        21-Nov-2021       Girish Kundar      Modifies :Check In check out changes
2.130.9      16-Jun-2022       Guru S A           Execute online transaction changes in Kiosk 
*2.140.0     01-Jun-2021       Fiona Lishal       Modified for Delivery Order enhancements for F&B
*2.150.0     12-Dec-2022       Abhishek           Modified : Added usecases for waiver
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Customer.Waivers;
using Semnox.Parafait.Game;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Transaction.KDS;
using Semnox.Parafait.User;
using Semnox.Parafait.GenericUtilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Waiver;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Product;
using Semnox.Parafait.POS;
using Semnox.Parafait.Printer;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Implementation of user use-cases
    /// </summary>
    public class LocalTransactionUseCase : LocalUseCases, ITransactionUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// LocalTransactionUseCase
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalTransactionUseCase(ExecutionContext executionContext) : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// GetWaiverLinks
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public async Task<string> GetWaiverLinks(int transactionId)
        {
            log.LogMethodEntry(transactionId);
            return await Task<string>.Factory.StartNew(() =>
            {
                try
                {
                    Utilities utilities = new Utilities();
                    utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
                    utilities.ParafaitEnv.SetPOSMachine("", executionContext.POSMachineName);
                    utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                    utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
                    utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
                    utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
                    utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
                    TransactionUtils transactionUtils = new TransactionUtils(utilities);
                    Transaction transaction = transactionUtils.CreateTransactionFromDB(transactionId, utilities);
                    SignWaiverEmail signWaiverEmail = new SignWaiverEmail(executionContext, transaction, utilities);
                    List<ValidationError> validationErrorList = signWaiverEmail.CanSendSignWaiverEmail(null);
                    if (validationErrorList != null && validationErrorList.Any())
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Validation Error"), validationErrorList);
                    }
                    string content = signWaiverEmail.GenerateWaiverSigningLink(null);
                    log.LogMethodExit(content);
                    return content;
                }
                catch (ValidationException vex)
                {
                    log.Error(vex);
                    throw vex;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw ex;
                }
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputList"></param>
        /// <returns></returns>
        public async Task<List<KeyValuePair<TransactionDTO, string>>> UpdateTransactionStatus(List<TransactionDTO> inputList)
        {
            log.LogMethodEntry(inputList);
            return await Task<List<KeyValuePair<TransactionDTO, string>>>.Factory.StartNew(() =>
            {
                List<KeyValuePair<TransactionDTO, string>> resultList = new List<KeyValuePair<TransactionDTO, string>>();
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    if (inputList != null)
                    {
                        try
                        {
                            foreach (TransactionDTO transactionDTO in inputList)
                            {
                                parafaitDBTrx.BeginTransaction();
                                KeyValuePair<TransactionDTO, string> result;
                                try
                                {
                                    TransactionBL transactionBL = new TransactionBL(executionContext, transactionDTO.TransactionId, parafaitDBTrx.SQLTrx);
                                    transactionBL.UpdateTransactionStatus(transactionDTO.Status, parafaitDBTrx.SQLTrx);
                                    parafaitDBTrx.EndTransaction();
                                    result = new KeyValuePair<TransactionDTO, string>(transactionBL.TransactionDTO, null);
                                }
                                catch (Exception ex)
                                {
                                    parafaitDBTrx.RollBack();
                                    log.Error(ex);
                                    result = new KeyValuePair<TransactionDTO, string>(transactionDTO, ex.Message);
                                }
                                resultList.Add(result);
                            }

                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            resultList.Add(new KeyValuePair<TransactionDTO, string>(null, ex.Message));
                            if (parafaitDBTrx != null)
                            {
                                parafaitDBTrx.RollBack();
                                parafaitDBTrx.Dispose();
                            }
                        }
                    }
                    log.LogMethodExit(resultList);
                    return resultList;
                }

            });

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputList"></param>
        /// <returns></returns>
        public async Task<List<KeyValuePair<TransactionDTO, string>>> SetAsCustomerReconfirmedOrder(List<TransactionDTO> inputList)
        {
            log.LogMethodEntry();
            return await Task<List<KeyValuePair<TransactionDTO, string>>>.Factory.StartNew(() =>
            {
                List<KeyValuePair<TransactionDTO, string>> resultList = new List<KeyValuePair<TransactionDTO, string>>();
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    if (inputList != null)
                    {
                        try
                        {
                            foreach (TransactionDTO transactionDTO in inputList)
                            {
                                parafaitDBTrx.BeginTransaction();
                                KeyValuePair<TransactionDTO, string> result;
                                try
                                {
                                    TransactionBL transactionBL = new TransactionBL(executionContext, transactionDTO.TransactionId, parafaitDBTrx.SQLTrx);
                                    transactionBL.SetAsCustomerReconfirmedOrder(parafaitDBTrx.SQLTrx);
                                    parafaitDBTrx.EndTransaction();
                                    result = new KeyValuePair<TransactionDTO, string>(transactionBL.TransactionDTO, null);
                                }
                                catch (Exception ex)
                                {
                                    parafaitDBTrx.RollBack();
                                    log.Error(ex);
                                    result = new KeyValuePair<TransactionDTO, string>(transactionDTO, ex.Message);
                                }
                                resultList.Add(result);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            resultList.Add(new KeyValuePair<TransactionDTO, string>(null, ex.Message));
                            if (parafaitDBTrx != null)
                            {
                                parafaitDBTrx.RollBack();
                                parafaitDBTrx.Dispose();
                            }
                        }
                    }
                    log.LogMethodExit(resultList);
                    return resultList;
                }

            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputList"></param>
        /// <returns></returns>
        public async Task<List<KeyValuePair<TransactionDTO, string>>> SetAsPreparationReconfirmedOrder(List<TransactionDTO> inputList)
        {
            log.LogMethodEntry(inputList);
            return await Task<List<KeyValuePair<TransactionDTO, string>>>.Factory.StartNew(() =>
            {
                List<KeyValuePair<TransactionDTO, string>> resultList = new List<KeyValuePair<TransactionDTO, string>>();
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    if (inputList != null)
                    {
                        try
                        {
                            foreach (TransactionDTO transactionDTO in inputList)
                            {
                                parafaitDBTrx.BeginTransaction();
                                KeyValuePair<TransactionDTO, string> result;
                                try
                                {
                                    TransactionBL transactionBL = new TransactionBL(executionContext, transactionDTO.TransactionId, parafaitDBTrx.SQLTrx);
                                    transactionBL.SetAsPreparationReconfirmedOrder(parafaitDBTrx.SQLTrx);
                                    parafaitDBTrx.EndTransaction();
                                    result = new KeyValuePair<TransactionDTO, string>(transactionBL.TransactionDTO, null);

                                }
                                catch (Exception ex)
                                {
                                    parafaitDBTrx.RollBack();
                                    log.Error(ex);
                                    result = new KeyValuePair<TransactionDTO, string>(transactionDTO, ex.Message);
                                }
                                resultList.Add(result);
                            }

                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            resultList.Add(new KeyValuePair<TransactionDTO, string>(null, ex.Message));
                            if (parafaitDBTrx != null)
                            {
                                parafaitDBTrx.RollBack();
                                parafaitDBTrx.Dispose();
                            }
                        }
                    }
                    log.LogMethodExit(resultList);
                    return resultList;
                }

            });
        }
        /// <summary>
        /// AssignRider
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="transactionDeliveryDetailsDTO"></param>
        /// <returns></returns>
        public async Task<TransactionOrderDispensingDTO> AssignRider(int transactionId, TransactionDeliveryDetailsDTO transactionDeliveryDetailsDTO)
        {
            log.LogMethodEntry(transactionId, transactionDeliveryDetailsDTO);
            return await Task<TransactionOrderDispensingDTO>.Factory.StartNew(() =>
            {
                TransactionOrderDispensingDTO resultDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    if (transactionDeliveryDetailsDTO != null && transactionId > -1)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            TransactionBL transactionBL = new TransactionBL(executionContext, transactionId, parafaitDBTrx.SQLTrx);
                            resultDTO = transactionBL.AssignRider(transactionDeliveryDetailsDTO, parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            if (parafaitDBTrx != null)
                            {
                                parafaitDBTrx.RollBack();
                                parafaitDBTrx.Dispose();
                            }
                            throw;
                        }
                    }
                    log.LogMethodExit(resultDTO);
                    return resultDTO;
                }

            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="transactionDeliveryDetailsDTO"></param>
        /// <returns></returns>
        public async Task<TransactionOrderDispensingDTO> UnAssignRider(int transactionId, TransactionDeliveryDetailsDTO transactionDeliveryDetailsDTO)
        {
            log.LogMethodEntry(transactionId, transactionDeliveryDetailsDTO);
            return await Task<TransactionOrderDispensingDTO>.Factory.StartNew(() =>
            {
                TransactionOrderDispensingDTO resultDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    if (transactionDeliveryDetailsDTO != null && transactionId > -1)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            TransactionBL transactionBL = new TransactionBL(executionContext, transactionId, parafaitDBTrx.SQLTrx);
                            resultDTO = transactionBL.UnAssignRider(transactionDeliveryDetailsDTO, parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            if (parafaitDBTrx != null)
                            {
                                parafaitDBTrx.RollBack();
                                parafaitDBTrx.Dispose();
                            }
                            throw ex;
                        }
                    }
                    log.LogMethodExit(resultDTO);
                    return resultDTO;
                }

            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="transactionDeliveryDetailsDTOList"></param>
        /// <returns></returns>
        public async Task<TransactionOrderDispensingDTO> SaveRiderDeliveryStatus(int transactionId, List<TransactionDeliveryDetailsDTO> transactionDeliveryDetailsDTOList)
        {
            log.LogMethodEntry();
            return await Task<TransactionOrderDispensingDTO>.Factory.StartNew(() =>
            {
                TransactionOrderDispensingDTO resultDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    if (transactionDeliveryDetailsDTOList != null && transactionId > -1)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            TransactionBL transactionBL = new TransactionBL(executionContext, transactionId, parafaitDBTrx.SQLTrx);
                            resultDTO = transactionBL.SaveRiderDeliveryStatus(transactionDeliveryDetailsDTOList, parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            if (parafaitDBTrx != null)
                            {
                                parafaitDBTrx.RollBack();
                                parafaitDBTrx.Dispose();
                            }
                            throw ex;
                        }
                    }
                    log.LogMethodExit(resultDTO);
                    return resultDTO;
                }

            });
        }
        /// <summary>
        /// SaveRiderAssignmentRemarks
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="transactionDeliveryDetailsDTOList"></param>
        /// <returns></returns>
        public async Task<TransactionOrderDispensingDTO> SaveRiderAssignmentRemarks(int transactionId, List<TransactionDeliveryDetailsDTO> transactionDeliveryDetailsDTOList)
        {
            log.LogMethodEntry(transactionId, transactionDeliveryDetailsDTOList);
            return await Task<TransactionOrderDispensingDTO>.Factory.StartNew(() =>
            {
                TransactionOrderDispensingDTO resultDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    if (transactionDeliveryDetailsDTOList != null && transactionId > -1)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            TransactionBL transactionBL = new TransactionBL(executionContext, transactionId, parafaitDBTrx.SQLTrx);
                            resultDTO = transactionBL.SaveRiderAssignmentRemarks(transactionDeliveryDetailsDTOList, parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            if (parafaitDBTrx != null)
                            {
                                parafaitDBTrx.RollBack();
                                parafaitDBTrx.Dispose();
                            }
                            throw;
                        }
                    }
                    log.LogMethodExit(resultDTO);
                    return resultDTO;
                }

            });

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="timetoAmend"></param>
        /// <returns></returns>
        public async Task<List<TransactionLineDTO>> AmendKOTScheduleTime(int transactionId, double timetoAmend)
        {
            log.LogMethodEntry(transactionId, timetoAmend);
            return await Task<List<TransactionLineDTO>>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                List<TransactionLineDTO> resultDTOList = new List<TransactionLineDTO>();
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    if (transactionId > -1)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            TransactionBL transactionBL = new TransactionBL(executionContext, transactionId, parafaitDBTrx.SQLTrx);
                            transactionBL.AmendKOTScheduleTime(timetoAmend, parafaitDBTrx.SQLTrx);
                            resultDTOList = new TransactionBL(executionContext, transactionId, parafaitDBTrx.SQLTrx).TransactionDTO.TransactionLinesDTOList;
                            parafaitDBTrx.EndTransaction();
                        }

                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                    log.LogMethodExit(resultDTOList);
                    return resultDTOList;
                }

            });
        }
        /// <summary>
        /// GetTransactionDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="utilities"></param>
        /// <param name="sqlTransaction"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="buildChildRecords"></param>
        /// <param name="buildTickets"></param>
        /// <param name="buildReceipt"></param>
        /// <returns></returns>
        public async Task<List<TransactionDTO>> GetTransactionDTOList(List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters, Utilities utilities, SqlTransaction sqlTransaction = null, int pageNumber = 0, int pageSize = 10, bool buildChildRecords = false, bool buildTickets = false, bool buildReceipt = false)
        {
            log.LogMethodEntry(searchParameters, utilities, sqlTransaction, pageNumber, pageSize, buildChildRecords, buildTickets, buildReceipt);
            return await Task<List<TransactionDTO>>.Factory.StartNew(() =>
            {
                TransactionListBL transactionListBL = new TransactionListBL(executionContext);
                List<TransactionDTO> transactionDTOList = transactionListBL.GetTransactionDTOList(searchParameters, utilities, null, pageNumber, pageSize, buildChildRecords, buildTickets, buildReceipt);
                log.LogMethodExit(transactionDTOList);
                return transactionDTOList;
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public async Task<int> GetTransactionCount(List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            return await Task<int>.Factory.StartNew(() =>
            {
                TransactionListBL transactionListBL = new TransactionListBL(executionContext);
                int transactionCount = transactionListBL.GetTransactionCount(searchParameters);
                log.LogMethodExit(transactionCount);
                return transactionCount;
            });
        }
        /// <summary>
        /// CreateTransactionOrderDispensingDTO
        /// </summary>
        /// <param name="transactionOrderDispensingDTOList"></param>
        /// <returns></returns>
        public async Task<List<TransactionOrderDispensingDTO>> CreateTransactionOrderDispensingDTO(List<TransactionOrderDispensingDTO> transactionOrderDispensingDTOList)
        {
            log.LogMethodEntry(transactionOrderDispensingDTOList);
            return await Task<List<TransactionOrderDispensingDTO>>.Factory.StartNew(() =>
            {
                TransactionOrderDispensingListBL transactionOrderDispensingListBL = new TransactionOrderDispensingListBL(executionContext, transactionOrderDispensingDTOList);
                List<TransactionOrderDispensingDTO> result = transactionOrderDispensingListBL.Save();
                log.LogMethodExit(result);
                return result;
            });
        }
        /// <summary>
        /// Settle Transaction Payments
        /// </summary>
        /// <param name="transactionPaymentsDTOList"></param>
        /// <returns></returns>
        public async Task<List<KeyValuePair<TransactionPaymentsDTO, string>>> SettleTransactionPayments(List<TransactionPaymentsDTO> transactionPaymentsDTOList)
        {
            log.LogMethodEntry();
            return await Task<List<KeyValuePair<TransactionPaymentsDTO, string>>>.Factory.StartNew(() =>
            {
                List<KeyValuePair<TransactionPaymentsDTO, string>> resultList = new List<KeyValuePair<TransactionPaymentsDTO, string>>();
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    if (transactionPaymentsDTOList != null)
                    {
                        try
                        {
                            foreach (TransactionPaymentsDTO transactionPaymentsDTO in transactionPaymentsDTOList)
                            {
                                parafaitDBTrx.BeginTransaction();
                                KeyValuePair<TransactionPaymentsDTO, string> result;
                                try
                                {
                                    TransactionBL transactionBL = new TransactionBL(executionContext, transactionPaymentsDTO.TransactionId, parafaitDBTrx.SQLTrx);
                                    string message = transactionBL.SettleTransactionPayment(transactionPaymentsDTO);
                                    parafaitDBTrx.EndTransaction();
                                    result = new KeyValuePair<TransactionPaymentsDTO, string>(transactionPaymentsDTO, message);
                                }
                                catch (Exception ex)
                                {
                                    parafaitDBTrx.RollBack();
                                    log.Error(ex);
                                    result = new KeyValuePair<TransactionPaymentsDTO, string>(transactionPaymentsDTO, ex.Message);
                                }
                                resultList.Add(result);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            resultList.Add(new KeyValuePair<TransactionPaymentsDTO, string>(null, ex.Message));
                            if (parafaitDBTrx != null)
                            {
                                parafaitDBTrx.RollBack();
                                parafaitDBTrx.Dispose();
                            }
                        }
                    }
                    log.LogMethodExit(resultList);
                    return resultList;
                }

            });
        }

        ///// <summary>
        ///// CheckIn
        ///// </summary>
        ///// <param name="transactionId"></param>
        ///// <param name="checkInId"></param>
        ///// <param name="checkInDetailDTOList"></param>
        ///// <returns></returns>
        //public async Task<CheckInDTO> CheckIn(int transactionId, int checkInId, List<CheckInDetailDTO> checkInDetailDTOList)
        //{
        //    log.LogMethodEntry(checkInDetailDTOList);

        //    return await Task<CheckInDTO>.Factory.StartNew(() =>
        //      {
        //          CheckInDTO result = null;
        //          using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
        //          {
        //              try
        //              {
        //                  parafaitDBTrx.BeginTransaction();
        //                  CheckInBL checkInBL = new CheckInBL(executionContext, checkInId);
        //                  result = checkInBL.CheckIn(checkInDetailDTOList, parafaitDBTrx.SQLTrx);
        //                  parafaitDBTrx.EndTransaction();
        //              }

        //              catch (Exception ex)
        //              {
        //                  parafaitDBTrx.RollBack();
        //                  log.Error(ex);
        //                  log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
        //                  throw;
        //              }

        //              log.LogMethodExit(result);
        //              return result;
        //          }
        //      });
        //}

        ///// <summary>
        ///// Pause
        ///// </summary>
        ///// <param name="transactionId"></param>
        ///// <param name="checkInId"></param>
        ///// <param name="checkInDetailDTOList"></param>
        ///// <returns></returns>
        //public async Task<CheckInDTO> Pause(int transactionId, int checkInId, List<CheckInDetailDTO> checkInDetailDTOList)
        //{
        //    log.LogMethodEntry(checkInDetailDTOList);

        //    return await Task<CheckInDTO>.Factory.StartNew(() =>
        //    {
        //        CheckInDTO result = null;
        //        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
        //        {
        //            try
        //            {
        //                parafaitDBTrx.BeginTransaction();
        //                CheckInBL checkInBL = new CheckInBL(executionContext, checkInId);
        //                result = checkInBL.Pause(checkInDetailDTOList, parafaitDBTrx.SQLTrx);
        //                parafaitDBTrx.EndTransaction();
        //            }

        //            catch (Exception ex)
        //            {
        //                parafaitDBTrx.RollBack();
        //                log.Error(ex);
        //                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
        //                throw;
        //            }

        //            log.LogMethodExit(result);
        //            return result;
        //        }
        //    });
        //}

        ///// <summary>
        ///// Checkout
        ///// </summary>
        ///// <param name="transactionId"></param>
        ///// <param name="checkInId"></param>
        ///// <param name="checkInDetailDTOList"></param>
        ///// <returns></returns>
        //public async Task<CheckInDTO> CheckOut(int transactionId, int checkInId, List<CheckInDetailDTO> checkInDetailDTOList)
        //{
        //    log.LogMethodEntry(checkInDetailDTOList);

        //    return await Task<CheckInDTO>.Factory.StartNew(() =>
        //    {
        //        CheckInDTO result = null;
        //        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
        //        {
        //            try
        //            {
        //                parafaitDBTrx.BeginTransaction();
        //                CheckInBL checkInBL = new CheckInBL(executionContext, checkInId);
        //                result = checkInBL.CheckOut(checkInDetailDTOList, parafaitDBTrx.SQLTrx);
        //                parafaitDBTrx.EndTransaction();
        //            }

        //            catch (Exception ex)
        //            {
        //                parafaitDBTrx.RollBack();
        //                log.Error(ex);
        //                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
        //                throw;
        //            }

        //            log.LogMethodExit(result);
        //            return result;
        //        }
        //    });
        //}

        /// <summary>
        /// SaveCheckIn
        /// </summary>
        /// <param name="checkInId"></param>
        /// <param name="checkInDetailDTOList"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public async Task<CheckInDTO> UpdateCheckInStatus(int checkInId, List<CheckInDetailDTO> checkInDetailDTOList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(checkInDetailDTOList, sqlTransaction);
            return await Task<CheckInDTO>.Factory.StartNew(() =>
            {
                CheckInBL checkInBL = new CheckInBL(executionContext, checkInId, true, true, sqlTransaction);
                if (checkInDetailDTOList == null || checkInDetailDTOList.Any() == false)
                {
                    log.Debug("updatedCheckInDetailDTOList is empty");
                    log.LogMethodExit(false);
                    throw new Exception("updatedCheckInDetailDTOList is empty");
                }
                foreach (CheckInDetailDTO checkInDetailDTO in checkInDetailDTOList)
                {
                    CheckInStatus currentStatus = CheckInStatus.PENDING;
                    if (checkInDetailDTO.CheckInDetailId > -1)
                    {
                        currentStatus = checkInBL.CheckInDTO.CheckInDetailDTOList.Where(x => x.CheckInDetailId == checkInDetailDTO.CheckInDetailId).FirstOrDefault().Status;
                        log.Debug("currentStatus: " + currentStatus.ToString());
                    }
                    CheckInStatus newStatus = checkInDetailDTO.Status;
                    log.Debug("newStatus: " + newStatus.ToString());
                    if (checkInBL.IsValidCheckInStatus(currentStatus, newStatus))
                    {
                        if (newStatus == CheckInStatus.CHECKEDIN &&
                             (currentStatus == CheckInStatus.PENDING || currentStatus == CheckInStatus.ORDERED))
                        {
                            checkInDetailDTO.CheckInTime = ServerDateTime.Now;
                        }

                        else if (newStatus == CheckInStatus.CHECKEDIN && currentStatus == CheckInStatus.PAUSED)
                        {
                            CheckInPauseLogListBL checkInPauseLogList = new CheckInPauseLogListBL(executionContext);
                            List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>> searchCheckInPauseLogParams = new List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>>();
                            searchCheckInPauseLogParams.Add(new KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.CHECK_IN_DETAIL_ID, checkInDetailDTO.CheckInDetailId.ToString()));
                            searchCheckInPauseLogParams.Add(new KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.PAUSE_END_TIME_IS_NULL, "null"));
                            List<CheckInPauseLogDTO> listCheckInPauseLogDTO = checkInPauseLogList.GetCheckInPauseLogDTOList(searchCheckInPauseLogParams, sqlTransaction);
                            if (listCheckInPauseLogDTO != null && listCheckInPauseLogDTO.Any())
                            {
                                listCheckInPauseLogDTO = listCheckInPauseLogDTO.OrderByDescending(s => s.CheckInPauseLogId).ToList();
                                CheckInPauseLogDTO checkInPauseLogDTO = new CheckInPauseLogDTO();
                                DateTime serverDateTime = ServerDateTime.Now;
                                listCheckInPauseLogDTO[0].PauseEndTime = serverDateTime;
                                listCheckInPauseLogDTO[0].TotalPauseTime = Convert.ToInt32((serverDateTime - listCheckInPauseLogDTO[0].PauseStartTime).TotalMinutes);
                                listCheckInPauseLogDTO[0].UnPausedBy = executionContext.GetUserId();
                                CheckInPauseLogBL checkInPauseLogBL = new CheckInPauseLogBL(executionContext, listCheckInPauseLogDTO[0]);
                                checkInPauseLogBL.Save(sqlTransaction);
                            }
                        }
                        else if (newStatus == CheckInStatus.PAUSED && currentStatus == CheckInStatus.CHECKEDIN)
                        {
                            CheckInPauseLogListBL checkInPauseLogList = new CheckInPauseLogListBL(executionContext);
                            List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>> searchCheckInPauseLogParams = new List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>>();
                            searchCheckInPauseLogParams.Add(new KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.CHECK_IN_DETAIL_ID, checkInDetailDTO.CheckInDetailId.ToString()));
                            searchCheckInPauseLogParams.Add(new KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.PAUSE_END_TIME_IS_NULL, "null"));
                            List<CheckInPauseLogDTO> listCheckInPauseLogDTO = checkInPauseLogList.GetCheckInPauseLogDTOList(searchCheckInPauseLogParams, sqlTransaction);
                            if (listCheckInPauseLogDTO == null || (listCheckInPauseLogDTO != null && listCheckInPauseLogDTO.Any() == false))
                            {
                                CheckInPauseLogDTO checkInPauseLogDTO = new CheckInPauseLogDTO();
                                checkInPauseLogDTO.CheckInDetailId = checkInDetailDTO.CheckInDetailId;
                                checkInPauseLogDTO.PauseStartTime = ServerDateTime.Now;
                                checkInPauseLogDTO.PausedBy = executionContext.GetUserId();
                                checkInPauseLogDTO.POSMachine = executionContext.POSMachineName;
                                CheckInPauseLogBL checkInPauseLogBL = new CheckInPauseLogBL(executionContext, checkInPauseLogDTO);
                                checkInPauseLogBL.Save(sqlTransaction);
                            }
                        }
                        CheckInDetailBL checkInDetailBL = new CheckInDetailBL(executionContext, checkInDetailDTO);
                        checkInDetailBL.Save(sqlTransaction);
                    }
                    else
                    {
                        log.Error("Invalid CheckInStatus");
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4082)); // "Invalid CheckInStatus"));
                    }
                }
                log.LogMethodExit();
                return checkInBL.CheckInDTO;
            });
        }

        /// <summary>
        /// create staff card
        /// </summary>
        /// <param name="staffCardDTOList"></param>
        /// <returns></returns>
        public async Task<List<StaffCardDTO>> CreateStaffCard(List<StaffCardDTO> staffCardDTOList)
        {
            return await Task<List<StaffCardDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(staffCardDTOList);
                List<StaffCardDTO> result = null;
                if (staffCardDTOList == null)
                {
                    throw new ValidationException("staffCardDTOList is empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    Utilities parafaitUtility = GetUtility();
                    foreach (StaffCardDTO staffCardDTO in staffCardDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            int cardId = -1;
                            if (staffCardDTO.ProductId > -1)
                            {
                                ProductsContainerDTO product = ProductsContainerList.GetProductsContainerDTO(executionContext, staffCardDTO.ProductId);
                                AccountBL accountBL = new AccountBL(executionContext, staffCardDTO.CardNumber);
                                if (accountBL.AccountDTO == null)
                                {
                                    accountBL.SetStaffCard(staffCardDTO.CardNumber, parafaitDBTrx.SQLTrx);
                                }
                                CheckStaffCreditsLimit(product, accountBL.AccountDTO);
                                Card card = new Card(staffCardDTO.CardNumber, executionContext.GetUserId(), parafaitUtility, parafaitDBTrx.SQLTrx);
                                card.technician_card = 'Y';
                                Transaction transaction = new Transaction(parafaitUtility);
                                string message = staffCardDTO.Remarks;
                                int retcode = transaction.createTransactionLine(card, staffCardDTO.ProductId, 1, ref message);
                                if (retcode != 0)
                                {
                                    throw new Exception(message);
                                }
                                for (int i = 0; i < transaction.TrxLines.Count; i++)
                                {
                                    transaction.TrxLines[i].Price = 0;
                                    transaction.TrxLines[i].LineAmount = 0;
                                }
                                transaction.CashAmount = 0;
                                transaction.Transaction_Amount = 0;
                                transaction.Net_Transaction_Amount = 0;

                                retcode = transaction.SaveTransacation(parafaitDBTrx.SQLTrx, ref message);
                                if (retcode != 0)
                                {
                                    throw new Exception(message);
                                }
                                cardId = card.card_id;
                            }
                            else
                            {
                                AccountBL accountBL = new AccountBL(executionContext, staffCardDTO.CardNumber);
                                if (accountBL.AccountDTO == null)
                                {
                                    accountBL.SetStaffCard(staffCardDTO.CardNumber, parafaitDBTrx.SQLTrx);
                                }
                                cardId = accountBL.AccountDTO.AccountId;
                            }
                            staffCardDTO.CardId = cardId;
                            Users users = new Users(executionContext, staffCardDTO.UserId, loadChildRecords: true, activeChildRecords: true);
                            if (users.UserDTO != null)
                            {
                                if (users.UserDTO.UserIdentificationTagsDTOList.Any() == false)
                                {
                                    users.AddStaffCard(staffCardDTO, parafaitDBTrx.SQLTrx);
                                }
                            }
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                    result = staffCardDTOList;
                }
                log.LogMethodExit(result);
                return result;
            });
        }

        private bool CheckStaffCreditsLimit(ProductsContainerDTO product, AccountDTO accountDTO)
        {
            log.LogMethodEntry(product, accountDTO);
            bool canAddProduct = true;
            if (product != null)
            {
                double staffCreditLmt = 0;
                int staffGameLimit = 200;
                int timeLimit = 30;
                string staffCardCreditsLimitConfig = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "STAFF_CARD_CREDITS_LIMIT");
                if (!string.IsNullOrWhiteSpace(staffCardCreditsLimitConfig))
                {
                    try
                    {
                        staffCreditLmt = Convert.ToDouble(staffCardCreditsLimitConfig);
                    }
                    catch
                    {
                        staffCreditLmt = 0;
                    }
                }
                string staffCardGameLimitConfig = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "STAFF_CARD_GAME_LIMIT");
                if (!string.IsNullOrWhiteSpace(staffCardGameLimitConfig))
                {
                    try
                    {
                        staffGameLimit = Convert.ToInt32(staffCardGameLimitConfig);
                    }
                    catch
                    {
                        staffGameLimit = 200;
                    }
                }
                string staffCardTimeLimitConfig = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "STAFF_CARD_TIME_LIMIT");
                if (!string.IsNullOrWhiteSpace(staffCardTimeLimitConfig))
                {
                    try
                    {
                        timeLimit = Convert.ToInt32(staffCardTimeLimitConfig);
                    }
                    catch
                    {
                        timeLimit = 30;
                    }
                }


                if (staffCreditLmt > 0)
                {
                    double productCredits = 0;
                    double totalCredits = 0;
                    if (accountDTO != null && accountDTO.AccountSummaryDTO != null)
                    {
                        totalCredits = Convert.ToDouble(accountDTO.AccountSummaryDTO.TotalGamePlayCreditsBalance);
                    }
                    productCredits = Convert.ToDouble(product.Credits);
                    if (product.ProductCreditPlusContainerDTOList != null)
                    {
                        List<ProductCreditPlusContainerDTO> filteredList = product.ProductCreditPlusContainerDTOList.Where(cp => cp.CreditPlusType.Equals("A") || cp.CreditPlusType.Equals("G")).ToList();
                        if (filteredList != null && filteredList.Count > 0)
                        {
                            productCredits += Convert.ToDouble(filteredList.Sum(x => x.CreditPlus));
                        }
                    }
                    if (totalCredits != 0 || productCredits != 0)
                    {
                        if ((totalCredits + productCredits) > staffCreditLmt)
                        {
                            string message = MessageContainerList.GetMessage(executionContext, 1164);
                            log.Error(message);
                            throw new ValidationException(message);                           
                        }
                    }
                }
                if (timeLimit > 0)
                {
                    double productTime = 0;
                    double cardTime = 0;
                    if (accountDTO != null && accountDTO.AccountSummaryDTO != null)
                    {
                        cardTime = Convert.ToDouble(accountDTO.AccountSummaryDTO.TotalTimeBalance);
                    }
                    if (product.ProductCreditPlusContainerDTOList != null)
                    {
                        List<ProductCreditPlusContainerDTO> filteredList = product.ProductCreditPlusContainerDTOList.Where(cp => cp.CreditPlusType.Equals("M")).ToList();
                        if (filteredList != null && filteredList.Count > 0)
                        {
                            productTime = Convert.ToDouble(filteredList.Sum(x => x.CreditPlus));
                        }
                    }
                    if (productTime != 0)
                    {
                        if ((cardTime + productTime) > Convert.ToDouble(timeLimit))
                        {
                            string message = MessageContainerList.GetMessage(executionContext, 1385);
                            log.Error(message);
                            throw new ValidationException(message);
                        }
                    }
                }
                if (staffGameLimit > 0)
                {
                    int productGame = 0;
                    int cardGame = 0;
                    if (accountDTO != null && accountDTO.AccountSummaryDTO != null)
                    {
                        cardGame = Convert.ToInt32(accountDTO.AccountSummaryDTO.TotalGamesBalance);
                    }
                    if (product.ProductGamesContainerDTOList != null && product.ProductGamesContainerDTOList.Count > 0)
                    {
                        //List<ProductGamesContainerDTO> filteredList = product.ProductGamesContainerDTOList.Where(g => g.Game_profile_id == -1 && g.Game_id == -1).ToList();
                        //if (filteredList != null && filteredList.Count > 0)
                        //{
                        //    productGame = Convert.ToInt32(filteredList.Sum(x => x.Quantity));
                        //}
                        productGame = Convert.ToInt32(product.ProductGamesContainerDTOList.Sum(x => x.Quantity));
                    }
                    if (productGame != 0)
                    {
                        if ((cardGame + productGame) > Convert.ToDouble(staffGameLimit))
                        {
                            string message = MessageContainerList.GetMessage(executionContext, 1444);
                            log.Error(message);
                            throw new ValidationException(message);
                        }
                    }
                }
            }
            log.LogMethodExit(canAddProduct);
            return canAddProduct;
        }

        /// <summary>
        /// deativate staff card
        /// </summary>
        /// <param name="staffCardDTOs"></param>
        /// <returns></returns>
        public async Task<List<StaffCardDTO>> DeactivateStaffCard(List<StaffCardDTO> staffCardDTOs)
        {
            return await Task<List<StaffCardDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(staffCardDTOs);
                List<StaffCardDTO> result = null;
                if (staffCardDTOs == null)
                {
                    throw new ValidationException("staffCardDTOs is empty");
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    Utilities parafaitUtility = GetUtility();
                    foreach (StaffCardDTO staffCardDTO in staffCardDTOs)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            Users user = new Users(executionContext, staffCardDTO.UserId, true, true);
                            user.RemoveStaffCard(staffCardDTO, parafaitDBTrx.SQLTrx);
                            Card card = new Card(staffCardDTO.CardNumber, executionContext.GetUserId(), parafaitUtility, parafaitDBTrx.SQLTrx);
                            TaskProcs tp = new TaskProcs(parafaitUtility);
                            string message = string.Empty;
                            if (!tp.RefundCard(card, 0, 0, 0, "Deactivate", ref message, inSQLTrx: parafaitDBTrx.SQLTrx))
                            {
                                throw new Exception(message);
                            }
                            else
                            {
                                AccountBL accountBL = new AccountBL(executionContext, staffCardDTO.CardNumber);
                                accountBL.RemoveStaffCard(parafaitDBTrx.SQLTrx);
                            }
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                    result = staffCardDTOs;
                }
                log.LogMethodExit(staffCardDTOs);
                return result;
            });
        }
        internal Utilities GetUtility()
        {
            log.LogMethodEntry();
            Utilities utilities = new Utilities();
            utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
            utilities.ParafaitEnv.POSMachineId = executionContext.GetMachineId();
            utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
            log.Debug("executionContext - siteId" + executionContext.GetSiteId());
            utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
            utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
            utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
            UserContainerDTO user = UserContainerList.GetUserContainerDTOOrDefault(executionContext.GetUserId(), "", executionContext.GetSiteId());
            utilities.ParafaitEnv.User_Id = user.UserId;
            utilities.ParafaitEnv.RoleId = user.RoleId;
            utilities.ExecutionContext.SetUserId(user.LoginId);
            utilities.ParafaitEnv.Initialize();
            log.LogMethodExit(utilities);
            return utilities;
        }
        public UserIdentificationTagsDTO GetUserIdentificationTagsDTO(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            UserIdentificationTagsDTO result = null;
            try
            {
                UserIdentificationTagListBL userIdentificationTagListBL = new UserIdentificationTagListBL();
                List<KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>> userIdTagSearchParams = new List<KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>>();
                userIdTagSearchParams.Add(new KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.ACTIVE_FLAG, "1"));
                userIdTagSearchParams.Add(new KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.CARD_NUMBER, cardNumber));
                userIdTagSearchParams.Add(new KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                List<UserIdentificationTagsDTO> userIdentificationTagsDTOList = userIdentificationTagListBL.GetUserIdentificationTagsDTOList(userIdTagSearchParams);
                if (userIdentificationTagsDTOList != null && userIdentificationTagsDTOList.Count > 0)
                {
                    result = new UserIdentificationTagsDTO();
                    result = userIdentificationTagsDTOList[0];
                }
            }
            catch (Exception ex)
            {
                log.Error("Ends - CheckCardExist Error: " + ex.Message);
                result = null;
            }
            log.LogMethodExit("userTagDTO");
            return result;
        }

        /// <summary>
        /// UpdateTransactionPaymentModeDetails
        /// </summary>
        /// <param name="transactionPaymentsDTOList"></param>
        /// <returns></returns>
        public async Task<List<KeyValuePair<TransactionPaymentsDTO, string>>> UpdateTransactionPaymentModeDetails(List<TransactionPaymentsDTO> transactionPaymentsDTOList)
        {
            log.LogMethodEntry(transactionPaymentsDTOList);
            return await Task<List<KeyValuePair<TransactionPaymentsDTO, string>>>.Factory.StartNew(() =>
            {
                List<KeyValuePair<TransactionPaymentsDTO, string>> resultList = new List<KeyValuePair<TransactionPaymentsDTO, string>>();
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        foreach (TransactionPaymentsDTO transactionPaymentsDTO in transactionPaymentsDTOList)
                        {
                            parafaitDBTrx.BeginTransaction();
                            KeyValuePair<TransactionPaymentsDTO, string> result;
                            try
                            {
                                TransactionBL transactionBL = new TransactionBL(executionContext, transactionPaymentsDTO.TransactionId);
                                transactionBL.UpdateTransactionPayments(new List<TransactionPaymentsDTO>() { transactionPaymentsDTO });
                                parafaitDBTrx.EndTransaction();
                                result = new KeyValuePair<TransactionPaymentsDTO, string>(transactionPaymentsDTO, null);
                            }
                            catch (Exception ex)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(ex);
                                result = new KeyValuePair<TransactionPaymentsDTO, string>(transactionPaymentsDTO, ex.Message);
                            }
                            resultList.Add(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        resultList.Add(new KeyValuePair<TransactionPaymentsDTO, string>(null, ex.Message));
                        if (parafaitDBTrx != null)
                        {
                            parafaitDBTrx.RollBack();
                            parafaitDBTrx.Dispose();
                        }
                    }
                    log.LogMethodExit(resultList);
                    return resultList;
                }

            });
        }
        /// <summary>
        /// GetTransactionDTOList
        /// </summary>
        /// <param name="searchCriteria"></param> 
        /// <param name="buildChildRecords"></param>
        /// <param name="buildTickets"></param>
        /// <param name="buildReceipt"></param>
        /// <returns></returns>
        public async Task<List<TransactionDTO>> GetTransactionDTOList(TransactionSearchCriteria searchCriteria, bool buildChildRecords = false, bool buildTickets = false, bool buildReceipt = false)
        {
            log.LogMethodEntry(searchCriteria, buildChildRecords, buildTickets, buildReceipt);
            return await Task<List<TransactionDTO>>.Factory.StartNew(() =>
            {
                List<TransactionDTO> result = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    parafaitDBTrx.BeginTransaction();
                    try
                    {
                        TransactionListBL transactionListBL = new TransactionListBL(executionContext);
                        result = transactionListBL.GetTransactionDTOList(searchCriteria, parafaitDBTrx.SQLTrx, buildChildRecords, buildTickets, buildReceipt);
                        parafaitDBTrx.EndTransaction();
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                    }
                    log.LogMethodExit(result);
                    return result;
                }

            });

        }
        /// <summary>
        /// GetUnsettledTransactionPayments
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="paymentModeId"></param>
        /// <param name="deliveryChannelId"></param>
        /// <param name="trxFromDate"></param>
        /// <param name="trxToDate"></param>
        /// <returns></returns>
        public async Task<List<TransactionPaymentsDTO>> GetUnsettledTransactionPayments(int transactionId = -1, int paymentModeId = -1, int deliveryChannelId = -1, DateTime? trxFromDate = null, DateTime? trxToDate = null)
        {
            log.LogMethodEntry(transactionId, paymentModeId, deliveryChannelId, trxFromDate, trxToDate);
            return await Task<List<TransactionPaymentsDTO>>.Factory.StartNew(() =>
            {
                List<TransactionPaymentsDTO> result = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    parafaitDBTrx.BeginTransaction();
                    try
                    {
                        TransactionListBL transactionListBL = new TransactionListBL(executionContext);
                        result = transactionListBL.GetUnsettledTransactionPayyments(transactionId, paymentModeId, deliveryChannelId, trxFromDate, trxToDate, parafaitDBTrx.SQLTrx);
                        parafaitDBTrx.EndTransaction();
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                    }
                    log.LogMethodExit(result);
                    return result;
                }

            });

        }
        /// <summary>
        /// SubmitUrbanPiperOrderCancellationRequest
        /// </summary>
        /// <param name="inputList"></param>
        /// <returns></returns>
        public async Task<List<KeyValuePair<TransactionDTO, string>>> SubmitUrbanPiperOrderCancellationRequest(Dictionary<TransactionDTO, string> inputList)
        {
            log.LogMethodEntry(inputList);
            return await Task<List<KeyValuePair<TransactionDTO, string>>>.Factory.StartNew(() =>
            {
                List<KeyValuePair<TransactionDTO, string>> resultList = new List<KeyValuePair<TransactionDTO, string>>();
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    if (inputList != null)
                    {
                        try
                        {
                            foreach (KeyValuePair<TransactionDTO, string> trxKeyValue in inputList)
                            {
                                parafaitDBTrx.BeginTransaction();
                                TransactionUtils transactionUtils;
                                KeyValuePair<TransactionDTO, string> result;
                                try
                                {
                                    ParafaitMessageQueueListBL parafaitMessageQueueListBL = new ParafaitMessageQueueListBL(executionContext);
                                    List<KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>>();
                                    searchParameters.Add(new KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>(ParafaitMessageQueueDTO.SearchByParameters.IS_ACTIVE, "1"));
                                    searchParameters.Add(new KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>(ParafaitMessageQueueDTO.SearchByParameters.ENTITY_GUID, trxKeyValue.Key.Guid));
                                    searchParameters.Add(new KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>(ParafaitMessageQueueDTO.SearchByParameters.ATTEMPTS_LESS_THAN, "3"));
                                    List<ParafaitMessageQueueDTO> parafaitMessageQueueDTOList = parafaitMessageQueueListBL.GetParafaitMessageQueues(searchParameters);
                                    if (parafaitMessageQueueDTOList != null && parafaitMessageQueueDTOList.Any())
                                    {
                                        if (parafaitMessageQueueDTOList.Exists(x => x.Status == MessageQueueStatus.UnRead))
                                        {
                                            string errorMessage = MessageContainerList.GetMessage(executionContext, 4229);
                                            // 'Cannot Proceed. The transaction is still processing'
                                            log.Error(errorMessage);
                                            throw new Exception(errorMessage);
                                        }
                                    }


                                    transactionUtils = new TransactionUtils(GetUtility());
                                    Transaction transaction = transactionUtils.CreateTransactionFromDB(trxKeyValue.Key.TransactionId, GetUtility());
                                    if (transaction.Status == Transaction.TrxStatus.CANCELLED)
                                    {
                                        string errorMessage = MessageContainerList.GetMessage(executionContext, 2409);
                                        log.Error(errorMessage);
                                        throw new Exception(errorMessage);
                                    }
                                    if (transaction.Status == Transaction.TrxStatus.ORDERED || transaction.Status == Transaction.TrxStatus.PREPARED)
                                    {
                                        string errorMessage = MessageContainerList.GetMessage(executionContext, 4230);
                                        //Order has been accepted. The transaction cannot be cancelled.
                                        log.Error(errorMessage);
                                        throw new Exception(errorMessage);
                                    }
                                    if (transaction.Status == Transaction.TrxStatus.CLOSED)
                                    {
                                        string errorMessage = MessageContainerList.GetMessage(executionContext, 4231);
                                        //Order has been closed. The transaction cannot be cancelled.
                                        log.Error(errorMessage);
                                        throw new Exception(errorMessage);
                                    }
                                    if (transaction.IsReversedTransaction())
                                    {
                                        string errorMessage = MessageContainerList.GetMessage(executionContext, 335);
                                        log.Error(errorMessage);
                                        throw new Exception(errorMessage);
                                    }
                                    //Add Reason code to ApplicationRemarksDTO
                                    ApplicationRemarksDTO applicationRemarksDTO = new ApplicationRemarksDTO(-1, "Transaction", "RejectionReasonCode", trxKeyValue.Key.Guid, trxKeyValue.Value, true);
                                    ApplicationRemarks applicationRemarks = new ApplicationRemarks(executionContext, applicationRemarksDTO);
                                    applicationRemarks.Save(parafaitDBTrx.SQLTrx);

                                    //Add to ParafaitMessageQueue
                                    string remarks = MessageContainerList.GetMessage(executionContext, 4232);
                                    string actionType = "UrbanPiperOrderCancellation";
                                    //Request has been raised with Urban Piper for Order Cancellation.
                                    ParafaitMessageQueueDTO parafaitMessageQueueDTO = new ParafaitMessageQueueDTO(-1, trxKeyValue.Key.Guid, ParafaitMessageQueueDTO.EntityNames.Transaction.ToString(), "REVERSE_TRANSACTION", MessageQueueStatus.UnRead, true, actionType, remarks, 0);
                                    ParafaitMessageQueueBL parafaitMessageQueueBL = new ParafaitMessageQueueBL(executionContext, parafaitMessageQueueDTO, parafaitDBTrx.SQLTrx);
                                    parafaitMessageQueueBL.Save(parafaitDBTrx.SQLTrx);

                                    parafaitDBTrx.EndTransaction();
                                    result = new KeyValuePair<TransactionDTO, string>(trxKeyValue.Key, null);
                                }
                                catch (Exception ex)
                                {
                                    parafaitDBTrx.RollBack();
                                    log.Error(ex);
                                    result = new KeyValuePair<TransactionDTO, string>(trxKeyValue.Key, ex.Message);
                                }
                                resultList.Add(result);
                            }

                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            resultList.Add(new KeyValuePair<TransactionDTO, string>(null, ex.Message));
                            if (parafaitDBTrx != null)
                            {
                                parafaitDBTrx.RollBack();
                                parafaitDBTrx.Dispose();
                            }
                        }
                    }
                    log.LogMethodExit(resultList);
                    return resultList;
                }

            });
        }
        /// <summary>
        /// GetPrintableTransactionLines
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="printerTypeList"></param>
        /// <param name="forVirtualStore"></param>
        /// <returns></returns>
        public async Task<List<KeyValuePair<string, List<TransactionLineDTO>>>> GetPrintableTransactionLines(int transactionId, string printerTypeList, bool forVirtualStore)
        {
            log.LogMethodEntry(transactionId, printerTypeList, forVirtualStore);
            return await Task<List<KeyValuePair<string, List<TransactionLineDTO>>>>.Factory.StartNew(() =>
            {
                try
                {
                    if (transactionId == -1)
                    {
                        log.Debug("transactionId is -1");
                        log.LogMethodExit(false);
                        throw new Exception("Transaction id cannot be -1");
                    }
                    TransactionBL transactionBL = new TransactionBL(executionContext, transactionId);
                    List<KeyValuePair<string, List<TransactionLineDTO>>> valuePairList = transactionBL.GetPrintableTransactionLines(printerTypeList, forVirtualStore);
                    log.LogMethodExit(valuePairList);
                    return valuePairList;
                }
                catch (Exception vex)
                {
                    log.Error(vex);
                    throw;
                }
            });
        }
        /// <summary>
        /// PrintVirtualStoreTransaction
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="transactionListDTOList"></param>
        /// <returns></returns>
        public async Task<TransactionDTO> PrintVirtualStoreTransaction(int transactionId, List<TransactionLineDTO> transactionLineDTOList)
        {

            log.LogMethodEntry(transactionId, transactionLineDTOList);
            return await Task<TransactionDTO>.Factory.StartNew(() =>
            {
                try
                {
                    if (transactionId == -1)
                    {
                        log.Debug("transactionId is -1");
                        log.LogMethodExit(false);
                        throw new Exception("Transaction id cannot be -1");
                    }
                    Utilities utilities = new Utilities();
                    utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
                    utilities.ParafaitEnv.SetPOSMachine("", executionContext.POSMachineName);
                    utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                    utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
                    utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
                    utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
                    utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
                    TransactionBL transactionBL = new TransactionBL(executionContext, transactionId);
                    transactionBL.BuildTransactionWithPrintDetails(utilities, transactionLineDTOList);
                    log.LogMethodExit(transactionBL.TransactionDTO);
                    return transactionBL.TransactionDTO;
                }
                catch (Exception vex)
                {
                    log.Error(vex);
                    throw;
                }
            });
        }

        /// <summary>
        /// PrintExecuteOnlinetaskReceipt
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="taskInfoList"></param>
        /// <returns></returns>
        public async Task<TransactionDTO> PrintExecuteOnlinetaskReceipt(int transactionId, List<int> taskInfoList)
        {

            log.LogMethodEntry(transactionId, taskInfoList);
            return await Task<TransactionDTO>.Factory.StartNew(() =>
            {
                try
                {
                    TransactionDTO transactionDTO = null;
                    if (transactionId == -1)
                    {
                        log.Debug("transactionId is -1");
                        log.LogMethodExit(false);
                        throw new Exception("Transaction id cannot be -1");
                    }
                    Utilities utilities = new Utilities();
                    utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
                    utilities.ParafaitEnv.SetPOSMachine("", executionContext.POSMachineName);
                    utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                    utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
                    utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
                    utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
                    utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
                    TransactionBL transactionBL = new TransactionBL(executionContext, transactionId);
                    if (taskInfoList == null || taskInfoList.Count != 2)
                    {
                        throw new Exception("Please provide proper value for taskInfoList parameter");
                    }
                    if (taskInfoList[0] != 1 && taskInfoList[0] != 0)
                    {
                        throw new Exception("Please provide proper value for taskInfoList[0] parameter");
                    }
                    if (taskInfoList[1] < 0)
                    {
                        throw new Exception("Please provide proper value for taskInfoList[1] parameter");
                    }

                    PrinterDTO printerDTO = new PrinterDTO(-1, "Default", "Default", 0, true, DateTime.Now, "", DateTime.Now, "", "", "", -1, PrinterDTO.PrinterTypes.ReceiptPrinter, -1, "", false, -1, -1, 0);
                    if (taskInfoList[0] == 1)
                    {
                        //get  Receipt Template ID based on configuration EXECUTE_ONLINE_TRX_RECEIPT
                        int executeOnlineReceiptId = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "EXECUTE_ONLINE_TRX_RECEIPT", -1);
                        ReceiptPrintTemplateHeaderBL tempBL = new ReceiptPrintTemplateHeaderBL(executionContext, executeOnlineReceiptId, true);
                        ReceiptPrintTemplateHeaderDTO receiptPrintTemplateDTO = tempBL.ReceiptPrintTemplateHeaderDTO;

                        POSPrinterDTO posPrinterDTO = new POSPrinterDTO(-1, executionContext.GetMachineId(), -1, -1, -1, -1, executeOnlineReceiptId, printerDTO, null, receiptPrintTemplateDTO, true, DateTime.Now, "", DateTime.Now, "", -1, "", false, -1, -1);
                        transactionDTO = transactionBL.PrintExecuteOnlineReceipt(transactionBL.Transaction, posPrinterDTO, taskInfoList[1]);
                    }
                    else
                    {
                        //get  Receipt Template ID based on configuration EXECUTE_ONLINE_TRX_ERROR_RECEIPT
                        int executeOnlineReceiptId = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "EXECUTE_ONLINE_TRX_ERROR_RECEIPT", -1);
                        ReceiptPrintTemplateHeaderBL tempBL = new ReceiptPrintTemplateHeaderBL(executionContext, executeOnlineReceiptId, true);
                        ReceiptPrintTemplateHeaderDTO receiptPrintTemplateDTO = tempBL.ReceiptPrintTemplateHeaderDTO;

                        POSPrinterDTO posPrinterDTO = new POSPrinterDTO(-1, executionContext.GetMachineId(), -1, -1, -1, -1, executeOnlineReceiptId, printerDTO, null, receiptPrintTemplateDTO, true, DateTime.Now, "", DateTime.Now, "", -1, "", false, -1, -1);

                        transactionDTO = transactionBL.PrintExecuteOnlineErrorReceipt(transactionBL.Transaction, posPrinterDTO, taskInfoList[1]);
                    } 
                    
                    log.LogMethodExit(transactionBL.TransactionDTO);
                    return transactionBL.TransactionDTO;
                }
                catch (Exception vex)
                {
                    log.Error(vex);
                    throw;
                }
            });
        }

        public async Task<List<WaiverSignatureDTO>> SaveWaiverSignatures(int transactionId, List<WaiverSignatureDTO> waiverSignatureDTOList)
        {
            return await Task<List<WaiverSignatureDTO>>.Factory.StartNew(() =>
            {
                List<WaiverSignatureDTO> result = new List<WaiverSignatureDTO>();
                log.LogMethodEntry(waiverSignatureDTOList);
                Utilities utilities = GetUtility();
                TransactionUtils transactionUtils = new TransactionUtils(utilities);
                Transaction transaction = transactionUtils.CreateTransactionFromDB(transactionId, utilities);
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (WaiverSignatureDTO waiverSignatureDTO in waiverSignatureDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            CustomerSignedWaiverBL customerSignedWaiverBL = new CustomerSignedWaiverBL(executionContext, waiverSignatureDTO.CustomerSignedWaiverId);
                            CustomerSignedWaiverDTO customerSignedWaiverDTO = customerSignedWaiverBL.GetCustomerSignedWaiverDTO;
                            if (transaction.Trx_id > -1 && customerSignedWaiverDTO != null)
                            {
                                if (customerSignedWaiverDTO.ExpiryDate == null || transaction.TransactionDate < customerSignedWaiverDTO.ExpiryDate)
                                {
                                    WaiverSignatureListBL waiverSignatureListBL = new WaiverSignatureListBL(executionContext);
                                    List<KeyValuePair<WaiverSignatureDTO.SearchByWaiverSignatureParameters, string>> searchWaiverSignedParams = new List<KeyValuePair<WaiverSignatureDTO.SearchByWaiverSignatureParameters, string>>();
                                    searchWaiverSignedParams.Add(new KeyValuePair<WaiverSignatureDTO.SearchByWaiverSignatureParameters, string>(WaiverSignatureDTO.SearchByWaiverSignatureParameters.TRX_ID, waiverSignatureDTO.TrxId.ToString()));
                                    searchWaiverSignedParams.Add(new KeyValuePair<WaiverSignatureDTO.SearchByWaiverSignatureParameters, string>(WaiverSignatureDTO.SearchByWaiverSignatureParameters.LINE_ID, waiverSignatureDTO.LineId.ToString()));
                                    searchWaiverSignedParams.Add(new KeyValuePair<WaiverSignatureDTO.SearchByWaiverSignatureParameters, string>(WaiverSignatureDTO.SearchByWaiverSignatureParameters.WAIVERSETDETAIL_ID, waiverSignatureDTO.WaiverSetDetailId.ToString()));
                                    //searchWaiverSignedParams.Add(new KeyValuePair<WaiverSignatureDTO.SearchByWaiverSignatureParameters, string>(WaiverSignatureDTO.SearchByWaiverSignatureParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                                    List<WaiverSignatureDTO> waiverSignatureList = waiverSignatureListBL.GetWaiverSignatureDTOList(searchWaiverSignedParams);
                                    if (waiverSignatureList != null && waiverSignatureList.Any())
                                    {
                                        waiverSignatureList[0].CustomerSignedWaiverId = waiverSignatureDTO.CustomerSignedWaiverId;
                                        WaiverSignatureBL waiverSignatureBL = new WaiverSignatureBL(executionContext, waiverSignatureList[0]);
                                        waiverSignatureBL.Save(parafaitDBTrx.SQLTrx);
                                        result.Add(waiverSignatureBL.GetWaiverSignatureDTO);
                                    }
                                    else
                                    {
                                        log.Error("Entry not found");
                                        string errorMessage = MessageContainerList.GetMessage(executionContext, 3036);
                                        throw new ValidationException(errorMessage);
                                    }
                                }
                                else
                                {
                                    log.Error("Cannot map expired waivers of customer with customer id " + customerSignedWaiverDTO.SignedFor + " to transaction");
                                    string errorMessage = MessageContainerList.GetMessage(executionContext, "Cannot map expired waivers of customer with customer id " + customerSignedWaiverDTO.SignedFor + " to transaction");
                                    throw new ValidationException(errorMessage);
                                }
                            }
                            else
                            {
                                log.Error("Customer did not sign Waiver.");
                                string errorMessage = MessageContainerList.GetMessage(executionContext, 1001);//Customer did not sign Waiver.
                                throw new ValidationException(errorMessage);
                            }
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ex;
                        }
                    }
                }

                log.LogMethodExit(result);
                return result;
            });
        }

        /// <summary>
        /// GetTransactionDTOList
        /// </summary>
        /// <param name="transactionId"></param> 
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public async Task<List<CustomerDTO>> GetLatestSignedCustomers(int transactionId = -1, int totalCount = 10)
        {
            log.LogMethodEntry(transactionId, totalCount);
            return await Task<List<CustomerDTO>>.Factory.StartNew(() =>
            {
                List<CustomerDTO> customerDTOList = new List<CustomerDTO>();
                Utilities utilities = GetUtility();
                try
                {
                    TransactionUtils transactionUtils = new TransactionUtils(utilities);
                    Transaction transaction = transactionUtils.CreateTransactionFromDB(transactionId, utilities);
                    List<KeyValuePair<CustomerSearchByParameters, string>> searchParams = new List<KeyValuePair<CustomerSearchByParameters, string>>();
                    string waiverIdList = string.Empty;
                    if (transaction.GetWaiversDTOList() != null && transaction.GetWaiversDTOList().Any())
                    {
                        for (int i = 0; i < transaction.GetWaiversDTOList().Count; i++)
                        {
                            waiverIdList = waiverIdList + transaction.GetWaiversDTOList()[i].WaiverSetDetailId.ToString() + ",";
                        }
                        waiverIdList = waiverIdList.Substring(0, waiverIdList.Length - 1);
                    }
                    searchParams.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.LATEST_TO_SIGN_WAIVER, totalCount.ToString() + "|" + waiverIdList));
                    searchParams.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.CUSTOMER_SITE_ID, executionContext.GetSiteId().ToString()));
                    CustomerListBL customerListBL = new CustomerListBL(executionContext);
                    customerDTOList = customerListBL.GetCustomerDTOList(searchParams, true, true, true);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    throw ex;
                }
                log.LogMethodExit(customerDTOList);
                return customerDTOList;
            });

        }
    }
}
