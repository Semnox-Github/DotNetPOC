/********************************************************************************************
 * Project Name - Redemption Params
 * Description  - Redemption Params
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019   Deeksha                Added logger methods.
 ********************************************************************************************/
using System.ComponentModel;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// RedemtionParams Object
    /// </summary>
    public partial class RedemptionParams
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// RedemptionParams default constructor
        /// </summary>
        public RedemptionParams()
        {
            log.LogMethodEntry();
            ProductId = -1;
            CardNumber = "";
            TicketCount = 0;
            SiteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// ProductId
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// CardNumber
        /// </summary>
        [DefaultValue("")]
        public string CardNumber { get; set; }

        /// <summary>
        /// PosMachine
        /// </summary>
        [DefaultValue("")]
        public string PosMachine { get; set; }

        /// <summary>
        /// SiteId
        /// </summary>
        [Browsable(false)]
        [DefaultValue(-1)]
        public int? SiteId { get; set; }

        /// <summary>
        /// TicketCount
        /// </summary>
        [DefaultValue(-1)]
        public int TicketCount { get; set; }

    }
}
