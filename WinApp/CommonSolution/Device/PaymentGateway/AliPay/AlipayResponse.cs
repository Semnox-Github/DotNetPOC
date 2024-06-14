/********************************************************************************************
 * Project Name - AlipayPaymentGateway
 * Description  - AlipayResponse class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.80       18-Jul-2020      Jinto Thomas    Created                                          
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    class AlipayResponse
    {
        private TradeResponse alipay_trade_pay_response;
        private TradeResponse alipay_trade_query_response;
        private TradeResponse alipay_trade_refund_response;
        TradeResponse tradeResponse;

        public TradeResponse Alipay_trade_pay_response
        {
            get
            {
                return alipay_trade_pay_response;
            }

            set
            {
                alipay_trade_pay_response = value;
            }
        }

        public TradeResponse Alipay_trade_query_response
        {
            get
            {
                return alipay_trade_query_response;
            }

            set
            {
                alipay_trade_query_response = value;
            }
        }

        public TradeResponse Alipay_trade_refund_response
        {
            get
            {
                return alipay_trade_refund_response;
            }

            set
            {
                alipay_trade_refund_response = value;
            }
        }
        public TradeResponse TradeResponse
        {
            get
            {
                return tradeResponse;
            }

            set
            {
                tradeResponse = value;
            }
        }
    }
}
