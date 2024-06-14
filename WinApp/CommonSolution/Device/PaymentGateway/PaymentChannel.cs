///********************************************************************************************
// * Project Name - PaymentMode Programs
// * Description  - Data object of PaymentChannel  
// *  
// **************
// **Version Log
// **************
// *Version     Date          Modified By    Remarks          
// *********************************************************************************************
// *1.00        08-Feb-2016   Rakshith       Created 
// ********************************************************************************************/
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Semnox.Core.Utilities;


//namespace Semnox.Parafait.Device.PaymentGateway
//{
//    /// <summary>
//    ///  PaymentChannel Class
//    /// </summary>
//    public class PaymentChannel
//    {
//        PaymentModeChannelsDTO paymentChannelsDTO;
//        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

//        /// <summary>
//        /// Default constructor
//        /// </summary>
//        public PaymentChannel()
//        {
//            log.Debug("Starts-PaymentChannel() default constructor.");
//            paymentChannelsDTO = new PaymentModeChannelsDTO();
//            log.Debug("Ends-PaymentChannel() default constructor.");
//        }

//        /// <summary>
//        /// PaymentChannel constructor
//        /// </summary>
//        public PaymentChannel(PaymentModeChannelsDTO paymentChannelsDTO)
//        {
//            log.Debug("Starts-PaymentChannel(PaymentChannelsDTO paymentChannelsDTO)  constructor.");
//            this.paymentChannelsDTO = paymentChannelsDTO;
//            log.Debug("Ends-PaymentChannel(PaymentChannelsDTO paymentChannelsDTO)  constructor.");
//        }
//        //Constructor Call Corresponding Data Hander besed id
//        //And return Correspond Object
//        //EX: "'PaymentChannelsDTO"'  DTO  ====>  ""PaymentChannelsDTO" DataHandler
//        public PaymentChannel(int paymentChannelId)
//            : this()
//        {
//            log.Debug("Starts- PaymentChannel (int customerId) parameterized constructor.");
//            PaymentModeChannelsDatahandler paymentChannelsDatahandler = new PaymentModeChannelsDatahandler();
//            paymentChannelsDTO = paymentChannelsDatahandler.GetPaymentChannelsDTO(paymentChannelId);
//            log.Debug("Ends- PaymentChannel (int customerId)parameterized constructor.");
//        }

//        /// <summary>
//        /// Used For Save 
//        /// It may by Insert Or Update
//        /// </summary>
//        /// <returns>returs int status</returns>
//        public int Save()
//        {
//            log.Debug("Starts-Save() Method.");
//            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
//            int PaymentChannelId = -1;
//            PaymentModeChannelsDatahandler paymentChannelsDatahandler = new PaymentModeChannelsDatahandler();
//            try
//            {
//                if (paymentChannelsDTO.PaymentChannelId <= 0)
//                {
//                    PaymentChannelId = paymentChannelsDatahandler.InsertPaymentChannel(paymentChannelsDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
//                    paymentChannelsDTO.PaymentChannelId = PaymentChannelId;
//                    log.Debug("ends-Save() Method.");
//                }
//                else
//                {
//                    if (paymentChannelsDTO.IsChanged == true)
//                    {
//                        paymentChannelsDatahandler.UpdatePaymentChannel(paymentChannelsDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
//                        paymentChannelsDTO.AcceptChanges();
//                        PaymentChannelId= paymentChannelsDTO.PaymentChannelId;
//                    }
//                    log.Debug("Ends-Save() Method.");
                     
//                }

//            }
//            catch (Exception expn)
//            {
//                throw new System.Exception(expn.Message.ToString());
//            }
//            return PaymentChannelId;
//        }

//        /// <summary>
//        /// Delete the PaymentChannelsDTO based on Id
//        /// </summary>
//        /// <param name="customerId">paymentChannelId</param>
//        /// <returns>returs int status</returns>
//        public int Delete(int paymentChannelId)
//        {
//            log.Debug("Starts- Delete(int paymentChannelId) Method.");
//            try
//            {
//                PaymentModeChannelsDatahandler paymentChannelsDatahandler = new PaymentModeChannelsDatahandler();
//                log.Debug("Ends-Delete(int customerId) Method.");
//                return paymentChannelsDatahandler.DeletePaymentChannel(paymentChannelId);
//            }
//            catch (Exception expn)
//            {
//                throw new System.Exception(expn.Message.ToString());
//            }
//        }

       

//    }



//    /// <summary>
//    ///  PaymentChannelList Class
//    /// </summary>
//    public class PaymentChannelList
//    {

//         Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

//        /// <summary>
//        /// Gets the PaymentChannelsDTO matching the search key
//        /// </summary>
//        /// <param name="searchParameters">List of search parameters</param>
//        /// <returns>Returns the list of Generic PaymentChannelsDTO matching the search criteria</returns>
//        public List<PaymentModeChannelsDTO> GetAllPaymentChannels(List<KeyValuePair<PaymentModeChannelsDTO.SearchByParameters, string>> searchParameters)
//        {
//            log.Debug("Starts- GetAllPaymentChannels(searchParameters) Method.");
//            PaymentModeChannelsDatahandler paymentChannelsDatahandler = new PaymentModeChannelsDatahandler();
//            log.Debug("Ends-GetAllPaymentChannels(searchParameters) Method.");
//            return paymentChannelsDatahandler.GetAllPaymentChannels(searchParameters);
//        }

        
//    }
//}
