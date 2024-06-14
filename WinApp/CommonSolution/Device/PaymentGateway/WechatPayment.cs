using System;
//using Semnox.Parafait.PaymentGateway;
 using Semnox.Core.Utilities;
//using Semnox.Parafait.TransactionPayments;
 
 
namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// WechatPayment Class 
    /// </summary>
    public class WechatPaymentGateway : PaymentGateway
    {
        private WeChatPayMethods weChatPayDataHandler;
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        /// <summary>
        /// parameterized constructor
        /// </summary>
        public WechatPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
           : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            weChatPayDataHandler = new WeChatPayMethods(utilities);
            log.LogMethodExit(null);
        }


        /// <summary>
        /// MakePayment method
        /// </summary>
        /// <param name="transactionPaymentsDTO">transactionPaymentsDTO</param>
        /// <returns> returns TransactionPaymentsDTO</returns>
        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            try
            {

                transactionPaymentsDTO= weChatPayDataHandler.DoPayment(transactionPaymentsDTO);
                // BuildWeChatResponse(wechatPayResponseDTO, ref transactionPaymentsDTO);
                
                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;

            }
            catch (Exception ex)
            {
                log.Error("Error occured while making payment", ex);
                log.LogMethodExit(null, "Throwing Payment Gateway Exception-" + ex.Message);
                throw new PaymentGatewayException(ex.Message);
            }
         }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionPaymentsDTO">transactionPaymentsDTO</param>
        /// <returns> returns TransactionPaymentsDTO</returns>
        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            try
            {
                transactionPaymentsDTO = weChatPayDataHandler.DoRefund(transactionPaymentsDTO);
                //   BuildWeChatResponse(wechatPayResponseDTO,ref transactionPaymentsDTO);
                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;
            }
            catch (Exception ex)
            {
                log.LogMethodExit(null, "PaymentGatewayException"+ ex.Message);
                throw new PaymentGatewayException(ex.Message);
            }

        }




       

        //private WeChatPayDTO BuildWeChatPayRefundDTO(TransactionPaymentsDTO transactionPaymentsDTO)
        //{
        //    WeChatPayDTO weChatPayDTO = new WeChatPayDTO();
        //    weChatPayDTO.Amount = transactionPaymentsDTO.Amount;
        //    weChatPayDTO.CurrencyType = transactionPaymentsDTO.CurrencyCode;
        //    weChatPayDTO.VendorOrderNumber = transactionPaymentsDTO.TransactionId.ToString();
        //    weChatPayDTO.RefundAmount= transactionPaymentsDTO.Amount
        //    return weChatPayDTO;
        //}

    } 
 }