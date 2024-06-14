/********************************************************************************************
 * Project Name - PaymentMode Programs
 * Description  - Data object of PaymentChannel  
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        08-Feb-2016   Rakshith       Created 
 *2.60.2      13-Jun-2019   Mushahid Faizan   Added LogMethodEntry/Exit
 *                                            Removed Default Constructor and passed ExecutionContext in Constructor
 *2.70.2        09-Jul-2019   Girish Kundar   Modified : Save() method : Insert/update methods returns DTO instead of Id.
 *                                                         LogMethodEntry() and LogMethodExit(). 
 *2.90.0       17-Jul -2020  Girish Kundar    Modified : Added constructor for the PaymentModeList BL class
 *2.140.0       07-Sep-2021   Fiona            Modified Save()
 *                                             Added GetPaymentModeChannelsDTOList(List<int> paymentModesIdList, bool activeChildRecords, SqlTransaction sqlTransaction) 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    ///  PaymentChannel Class
    /// </summary>
    public class PaymentModeChannel
    {
        private PaymentModeChannelsDTO paymentChannelsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor having executionContext as parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public PaymentModeChannel(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            paymentChannelsDTO = new PaymentModeChannelsDTO();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with DTO as Parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="paymentChannelsDTO">paymentChannelsDTO</param>
        public PaymentModeChannel(ExecutionContext executionContext, PaymentModeChannelsDTO paymentChannelsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(paymentChannelsDTO);
            this.paymentChannelsDTO = paymentChannelsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor to Get DTO by passing Id as parameter.
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="paymentChannelId">paymentChannelId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public PaymentModeChannel(ExecutionContext executionContext, int paymentChannelId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, paymentChannelId);
            PaymentModeChannelsDatahandler paymentChannelsDatahandler = new PaymentModeChannelsDatahandler(sqlTransaction);
            paymentChannelsDTO = paymentChannelsDatahandler.GetPaymentModeChannelsDTO(paymentChannelId);
            log.LogMethodExit(paymentChannelsDTO);
        }

        /// <summary>
        /// Used For Save 
        /// It may by Insert Or Update
        /// </summary>
        /// <returns>returns int status</returns>
        public int Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            PaymentModeChannelsDatahandler paymentChannelsDatahandler = new PaymentModeChannelsDatahandler(sqlTransaction);
            int PaymentChannelId = -1;
            try
            {
                // if (paymentChannelsDTO.IsActive)
                // {
                if (paymentChannelsDTO.PaymentModeChannelId < 0)
                {
                    paymentChannelsDTO = paymentChannelsDatahandler.InsertPaymentModeChannel(paymentChannelsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    paymentChannelsDTO.AcceptChanges();
                    PaymentChannelId = paymentChannelsDTO.PaymentModeChannelId;
                }
                else
                {
                    if (paymentChannelsDTO.IsChanged)
                    {
                        paymentChannelsDTO = paymentChannelsDatahandler.UpdatePaymentModeChannel(paymentChannelsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        paymentChannelsDTO.AcceptChanges();
                        PaymentChannelId = paymentChannelsDTO.PaymentModeChannelId;
                    }
                }
                //}
                //else  // Hard Delete
                //{
                //    Delete(PaymentChannelId);
                //}
                log.LogMethodExit();
                return PaymentChannelId;

            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Saving paymentChannelsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }

        }

        ///// <summary>
        ///// Deletes the PaymentChannelsDTO based on paymentModeChannelId
        ///// </summary>
        ///// <param name="paymentModeChannelId">paymentChannelId</param>
        ///// <returns>returns int status</returns>
        //public int Delete(int paymentModeChannelId, SqlTransaction sqlTransaction = null)
        //{
        //    log.LogMethodEntry(paymentModeChannelId, sqlTransaction);
        //    try
        //    {
        //        PaymentModeChannelsDatahandler paymentChannelsDatahandler = new PaymentModeChannelsDatahandler(sqlTransaction);
        //        int id = paymentChannelsDatahandler.DeletePaymentModeChannel(paymentModeChannelId);
        //        log.LogMethodExit(id);
        //        return id;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Error occurred while Deleting paymentChannelsDTO", ex);
        //        log.LogMethodExit(null, "Throwing exception - " + ex.Message);
        //        throw;
        //    }
        //}

    }
    /// <summary>
    ///  PaymentChannelList Class
    /// </summary>
    public class PaymentChannelList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        public PaymentChannelList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the PaymentChannelsDTO matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic PaymentChannelsDTO matching the search criteria</returns>
        public List<PaymentModeChannelsDTO> GetAllPaymentChannels(List<KeyValuePair<PaymentModeChannelsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<PaymentModeChannelsDTO> list = new List<PaymentModeChannelsDTO>();
            PaymentModeChannelsDatahandler paymentModeChannelsDatahandler = new PaymentModeChannelsDatahandler(sqlTransaction);
            list = paymentModeChannelsDatahandler.GetAllPaymentModeChannels(searchParameters);
            log.LogMethodExit(list);
            return list;
        }
        /// <summary>
        /// GetPaymentModeChannelsDTOList
        /// </summary>
        /// <param name="paymentModesIdList"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<PaymentModeChannelsDTO> GetPaymentModeChannelsDTOList(List<int> paymentModesIdList, bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(paymentModesIdList, activeChildRecords, sqlTransaction);
            PaymentModeChannelsDatahandler paymentModeChannelDataHandler = new PaymentModeChannelsDatahandler(sqlTransaction);
            List<PaymentModeChannelsDTO> paymentModeChannelsDTOList = paymentModeChannelDataHandler.GetPaymentModeChannelDTOListOfPaymentModes(paymentModesIdList, activeChildRecords);
            log.LogMethodExit(paymentModeChannelsDTOList);
            return paymentModeChannelsDTOList;
        }
    }
}
