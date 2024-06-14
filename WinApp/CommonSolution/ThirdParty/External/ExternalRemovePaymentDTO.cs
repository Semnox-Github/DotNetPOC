﻿/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to hold the remove payment details .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    07-Apr-2022   M S Shreyas             Created : External  REST API.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.External
{
    public class ExternalRemovePaymentDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get/Set for Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Get/Set for PaymentId
        /// </summary>
        public int PaymentId { get; set; }
        /// <summary>
        /// Get/Set for PaymentModeId
        /// </summary>
        public int PaymentModeId { get; set; }
        /// <summary>
        /// Get/Set for Remark
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ExternalRemovePaymentDTO()
        {
            log.LogMethodEntry();
            Type = String.Empty;
            PaymentId = -1;
            PaymentModeId = -1;
            Remarks = String.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameter
        /// </summary>
        public ExternalRemovePaymentDTO(string type, int paymentId, int paymentModeId, string remarks)
        {
            log.LogMethodEntry(type, paymentId, paymentModeId, remarks);
            this.Type = type;
            this.PaymentId = paymentId;
            this.PaymentModeId = paymentModeId;
            this.Remarks = remarks;
            log.LogMethodExit();
        }
    }

}
