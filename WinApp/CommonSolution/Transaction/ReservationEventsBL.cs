/********************************************************************************************
 * Project Name - ReservationEventsBL 
 * Description  -BL class of the Reservation function Events
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.140.5     10-Jan-2023   Muaaz Musthafa      Created for Reservation changes                                                           
 ********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// TransactionEventsBL
    /// </summary>
    public class ReservationEventsBL : TransactionEventsBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// reservationDTO
        /// </summary>
        protected ReservationDTO reservationDTO;
        private int? executedCardCount;

        private const string PURCHASE_MSG_REF = "Purchase";
        private const string PAYMENT_LINK_MSG_REF = "Send Payment Link";
        private const string EXECUTE_ONLINE_TRANSACTION_MSG_REF = "New Customer Registration";
        private const string PURCHASE_MESSAGE_TRIGGERD_MSG_REF = "Purchase Trigger";
        private const string PAYMENT_MODE_OTP_EVENT_MSG_REF = "Payment Mode OTP Validation";

        private const string TRANSACTIONPAYMENTLINKTEMPLATE = "TRANSACTION_PAYMENT_LINK_TEMPLATE";
        private const string ONLINE_RECEIPT_EMAIL_TEMPLATE = "ONLINE_RECEIPT_EMAIL_TEMPLATE";
        private const string ONLINE_TICKETS_SMS_TEXT_TEMPLATE = "ONLINE_TICKETS_SMS_TEXT_TEMPLATE";
        private string NUMBER_FORMAT = string.Empty;
        private const string IMAGE_DIRECTORY = "IMAGE_DIRECTORY";
        private const string ONLINE_RESERVATION_EMAIL_TEMPLATE = "ONLINE_RESERVATION_EMAIL_TEMPLATE";

        /// <summary>
        /// TransactionEventsBL
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="utilities"></param>
        /// <param name="parafaitFunctionEvents"></param>
        /// <param name="transaction"></param>
        /// <param name="transactionEventContactsDTO"></param>
        /// <param name="executedCardCount"></param>
        /// <param name="sqlTransaction"></param>
        public ReservationEventsBL(ExecutionContext executionContext, Utilities utilities, ParafaitFunctionEvents parafaitFunctionEvents, Transaction transaction, ReservationDTO reservationDTO,
            TransactionEventContactsDTO transactionEventContactsDTO = null, int? executedCardCount = null, SqlTransaction sqlTransaction = null)
            : base(executionContext, utilities, parafaitFunctionEvents, transaction, transactionEventContactsDTO, executedCardCount, sqlTransaction)
        {
            log.LogMethodEntry(reservationDTO);
            if (transaction.IsReservationTransaction(null) && reservationDTO == null)
            {
                this.reservationDTO = GetReservationDTO(null);
            }
            else
            {
                this.reservationDTO = reservationDTO;
            }
            this.NUMBER_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NUMBER_FORMAT");
            log.LogMethodExit();
        }

        /// <summary>
        /// MessageSubjectFormatter
        /// </summary>
        /// <param name="messageTemplateSubject"></param>
        /// <returns></returns>
        public override string MessageSubjectFormatter(string messageTemplateSubject)
        {
            log.LogMethodEntry(messageTemplateSubject);
            string messageSubjectContent = string.Empty;
            if (string.IsNullOrWhiteSpace(messageTemplateSubject) == false && messageTemplateSubject.Contains("@"))
            {
                TransactionEmailTemplatePrint transactionEmailTemplatePrint = new TransactionEmailTemplatePrint(executionContext, utilities, -1, transaction, reservationDTO, true);
                messageSubjectContent = transactionEmailTemplatePrint.BuildContent(messageTemplateSubject);
            }
            else
            {
                messageSubjectContent = messageTemplateSubject;
            }
            log.LogMethodExit(messageSubjectContent);
            return messageSubjectContent;
        }
        /// <summary>
        /// MessageBodyFormatter
        /// </summary>
        /// <param name="messageTemplateContent"></param>
        /// <returns></returns>
        public override string MessageBodyFormatter(string messageTemplateContent)
        {
            log.LogMethodEntry(messageTemplateContent);
            string messageBodyContent = string.Empty;
            TransactionEmailTemplatePrint transactionEmailTemplatePrint = new TransactionEmailTemplatePrint(executionContext, utilities, -1, transaction, reservationDTO);
            messageBodyContent = transactionEmailTemplatePrint.BuildContent(messageTemplateContent, true);
            if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.EXECUTE_ONLINE_TRANSACTION_EVENT)
            {
                messageBodyContent = messageBodyContent.Replace("@TodaysIssuedCards", (executedCardCount == null
                                                                                         ? String.Empty : ((int)executedCardCount).ToString(NUMBER_FORMAT)));
            }
            if (this.parafaitFunctionEventDTO.ParafaitFunctionEventName == ParafaitFunctionEvents.PAYMENT_MODE_OTP_EVENT)
            {
                messageBodyContent = messageBodyContent.Replace("@PaymentModeOTP", (transactionEventContactsDTO == null ? String.Empty : transactionEventContactsDTO.OTPValue));
                messageBodyContent = messageBodyContent.Replace("@OTPGameCard", (transactionEventContactsDTO == null ? String.Empty : transactionEventContactsDTO.OTPGameCard));
            }
            log.LogMethodExit(messageBodyContent);
            return messageBodyContent;
        }

        private ReservationDTO GetReservationDTO(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            ReservationDTO reservationDTO = null;
            ReservationListBL reservationListBL = new ReservationListBL(this.executionContext);
            List<KeyValuePair<ReservationDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<ReservationDTO.SearchByParameters, string>>();
            searchParam.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.TRX_ID, this.transaction.Trx_id.ToString()));
            searchParam.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.SITE_ID, this.executionContext.GetSiteId().ToString()));
            List<ReservationDTO> reservationDTOList = reservationListBL.GetReservationDTOList(searchParam, sqlTrx);
            if (reservationDTOList != null && reservationDTOList.Count > 0)
            {
                reservationDTO = reservationDTOList[0];
                //ReservationBL reservationBL = new ReservationBL(executionContext, utilities, reservationDTOList[0].BookingId, sqlTrx);
            }
            log.LogMethodExit(reservationDTO);
            return reservationDTO;
        }

    }
}
