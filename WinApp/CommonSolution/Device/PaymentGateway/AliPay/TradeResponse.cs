/********************************************************************************************
 * Project Name - AlipayPaymentGateway
 * Description  - TradeResponse class
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
    class TradeResponse
    {
        private string code;
        private string msg;
        private string out_trade_no;
        private string buyer_pay_amount;
        private string trade_status;
        private string send_back_fee;
        private string sub_code;
        private string trade_no;

        public string Code
        {
            get
            {
                return code;
            }

            set
            {
                code = value;
            }
        }

        public string Msg
        {
            get
            {
                return msg;
            }

            set
            {
                msg = value;
            }
        }

        public string Out_trade_no
        {
            get
            {
                return out_trade_no;
            }

            set
            {
                out_trade_no = value;
            }
        }

        public string Buyer_pay_amount
        {
            get
            {
                return buyer_pay_amount;
            }

            set
            {
                buyer_pay_amount = value;
            }
        }
        public string Send_back_fee
        {
            get
            {
                return send_back_fee;
            }

            set
            {
                send_back_fee = value;
            }
        }
        public string Trade_status
        {
            get
            {
                return trade_status;
            }

            set
            {
                trade_status = value;
            }
        }
        public string Sub_code
        {
            get
            {
                return sub_code;
            }

            set
            {
                sub_code = value;
            }
        }
        public string Trade_no
        {
            get
            {
                return trade_no;
            }

            set
            {
                trade_no = value;
            }
        }
    }
}
