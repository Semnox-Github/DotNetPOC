using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{

    /// <summary>
    /// WeChatConfiguration Class
    /// </summary>
    public class WeChatConfiguration
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get/Set method of the WECHAT_APP_ID field
        /// </summary>
        public string WECHAT_APP_ID { get; set; }

        /// <summary>
        /// Get/Set method of the WECHAT_VENDOR_ID field
        /// </summary>
        public string WECHAT_VENDOR_ID { get; set; }

        /// <summary>
        /// Get/Set method of the WECHAT_SUB_VENDOR_ID field
        /// </summary>
        public string WECHAT_SUB_VENDOR_ID { get; set; }

        /// <summary>
        /// Get/Set method of the WECHAT_APP_KEY field
        /// </summary>
        public string WECHAT_APP_KEY { get; set; }

        /// <summary>
        /// Get/Set method of the AppId field
        /// </summary>
        public string WECHAT_APP_SECRET_KEY { get; set; }

        /// <summary>
        /// Get/Set method of the MICRO_PAY_URL field
        /// </summary>

        public string MICRO_PAY_URL { get; set; }

        /// <summary>
        /// Get/Set method of the QUERY_ORDER_URL field
        /// </summary>
        public string QUERY_ORDER_URL { get; set; }

        /// <summary>
        /// Get/Set method of the REVOKE_ORDER_URL field
        /// </summary>
        public string REVOKE_ORDER_URL { get; set; }

        /// <summary>
        /// Get/Set method of the REFUND_URL field
        /// </summary>
        public string REFUND_URL { get; set; }

        /// <summary>
        /// Get/Set method of the NOTIFY_URL field
        /// </summary>

        public string NOTIFY_URL { get; set; }

        /// <summary>
        /// Get/Set method of the IP field
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// Get/Set method of the SSLCERT_PATH field
        /// </summary>
        public string SSLCERT_PATH { get; set; }

        /// <summary>
        /// Get/Set method of the SSLCERT_PASSWORD field
        /// </summary>
        public string SSLCERT_PASSWORD { get; set; }

        public WeChatConfiguration()
        {
            log.LogMethodEntry();
            WECHAT_APP_ID = "";
            WECHAT_VENDOR_ID = "";
            WECHAT_SUB_VENDOR_ID = "";
            WECHAT_APP_KEY = "";
            WECHAT_APP_SECRET_KEY = "";
            MICRO_PAY_URL = "";
            QUERY_ORDER_URL = "";
            REVOKE_ORDER_URL = "";
            REFUND_URL = "";
            NOTIFY_URL = "";
            IP = "";
            SSLCERT_PATH = "";
            SSLCERT_PASSWORD = "";
            log.LogMethodExit(null);
        }
    }
}
