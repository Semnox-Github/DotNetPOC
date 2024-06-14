/********************************************************************************************
 * Project Name - PaymentMode Programs
 * Description  - Data object of PaymentMode  
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        26-Nov-2016   Rakshith       Created 
 *2.50.0      07-Dec-2018   Mathew Ninan   Transaction re-design 
 *2.70        17-Mar-2019   Jagan Mohana Rao    added SaveUpdatePaymentModesList and constructors in PaymentModeList class
              25-Mar-2019   Akshay Gulaganji    updated Author information 
              11-Apr-2019   Mushahid Faizan     Added GetAllPaymentModeList(), 
 *                                              Added Sql Transaction in Save() & SaveUpdatePaymentModesList () method
 *2.70.2        09-Jul-2019   Girish Kundar  Modified : Added LogMethodEntry() and LogMethodExit(). 
 *            21-Oct-2019   Jagan Mohana    Delete functionality implemented in Save()
 *2.90.0      09-Jul-2020   Girish Kundar   Modified : Passing execution context to PaymentChannelListBL constructor
 *2.90.0      09-Aug-2020   Girish Kundar   Modified : Issue Fix in Save() - DTOList null checks Or(||) condition replaced with and(&&)
 *2.140.0     03-Sep-2021   Fiona Lishal    Modified : Added Build method for Constructor
                                                      Changes to include CompatiablePaymentModes
 *2.150.1     22-Feb-2023   Guru S A        Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Discounts;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    ///  PaymentMode Class
    /// </summary>
    public class PaymentMode
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private string gateway = null;
        private PaymentModeDTO paymentModeDTO;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        private PaymentMode(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates PaymentModesBL object using the PaymentModesDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext object</param>
        /// <param name="paymentModeDTO">PaymentModesDTO object</param>
        public PaymentMode(ExecutionContext executionContext, PaymentModeDTO paymentModeDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, paymentModeDTO);
            this.executionContext = executionContext;
            this.paymentModeDTO = paymentModeDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the paymentModes id as the parameter
        /// Would fetch the paymentModes object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        public PaymentMode(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null, bool loadChildRecords = false, bool activeChildRecords = false)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction, loadChildRecords, activeChildRecords);
            PaymentModeDatahandler paymentModeDatahandler = new PaymentModeDatahandler(sqlTransaction);
            paymentModeDTO = paymentModeDatahandler.GetPaymentModeDTO(id);
            if (paymentModeDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "paymentModeDTO", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }

            if (paymentModeDTO.Gateway > -1)
            {
                if (!string.IsNullOrEmpty(Gateway) && Enum.IsDefined(typeof(PaymentGateways), Gateway))
                    paymentModeDTO.GatewayLookUp = (PaymentGateways)Enum.Parse(typeof(PaymentGateways), Gateway);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(paymentModeDTO);
        }
        private void Build(bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            PaymentChannelList paymentChannelList = new PaymentChannelList(executionContext);
            List<PaymentModeChannelsDTO> paymentModeChannelsDTOList = paymentChannelList.GetPaymentModeChannelsDTOList(new List<int>() { paymentModeDTO.PaymentModeId }, activeChildRecords, sqlTransaction);
            paymentModeDTO.PaymentModeChannelsDTOList = paymentModeChannelsDTOList;
            //DiscountCouponsListBL discountCouponsListBL = new DiscountCouponsListBL(executionContext);
            //List<DiscountCouponsDTO> discountCouponsDTOList = discountCouponsListBL.GetDiscountCouponsDTOListofPaymentModes(new List<int>() { paymentModeDTO.PaymentModeId }, activeChildRecords, sqlTransaction);
            //paymentModeDTO.DiscountCouponsDTOList = discountCouponsDTOList;
            CompatiablePaymentModesListBL compatiablePaymentModesListBL = new CompatiablePaymentModesListBL(executionContext);
            List<CompatiablePaymentModesDTO> compatiablePaymentModesDTOList = compatiablePaymentModesListBL.GetCompatiablePaymentModes(new List<int>() { paymentModeDTO.PaymentModeId }, activeChildRecords, sqlTransaction);
            paymentModeDTO.CompatiablePaymentModesDTOList = compatiablePaymentModesDTOList;

            PaymentModeDisplayGroupsListBL paymentModeDisplayGroupsListBL = new PaymentModeDisplayGroupsListBL(executionContext);
            List<PaymentModeDisplayGroupsDTO> paymentModeDisplayGroupsDTOList = paymentModeDisplayGroupsListBL.GetPaymentModeDisplayGroupsByPaymentModeIdList(new List<int>() { paymentModeDTO.PaymentModeId }, activeChildRecords, sqlTransaction);
            paymentModeDTO.PaymentModeDisplayGroupsDTOList = paymentModeDisplayGroupsDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the Payment Mode
        /// Checks if the PaymentModeId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            PaymentModeDatahandler paymentModeDatahandler = new PaymentModeDatahandler(sqlTransaction);
            //if (paymentModeDTO.IsActive)
            //{
            if (paymentModeDTO.PaymentModeId < 0)
            {
                paymentModeDTO = paymentModeDatahandler.InsertPaymentModes(paymentModeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                paymentModeDTO.AcceptChanges();
            }
            else
            {
                if (paymentModeDTO.IsChanged)
                {
                    paymentModeDTO = paymentModeDatahandler.UpdatePaymentModes(paymentModeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    paymentModeDTO.AcceptChanges();
                }
            }
            if (paymentModeDTO.PaymentModeChannelsDTOList != null && paymentModeDTO.PaymentModeChannelsDTOList.Any())
            {
                foreach (PaymentModeChannelsDTO paymentModeChannelsDTO in paymentModeDTO.PaymentModeChannelsDTOList)
                {
                    if (paymentModeChannelsDTO.PaymentModeId != paymentModeDTO.PaymentModeId)
                    {
                        paymentModeChannelsDTO.PaymentModeId = paymentModeDTO.PaymentModeId;
                    }
                    if (paymentModeChannelsDTO.IsChanged)
                    {
                        PaymentModeChannel paymentModeChannel = new PaymentModeChannel(executionContext, paymentModeChannelsDTO);
                        paymentModeChannel.Save(sqlTransaction);
                    }
                }
            }

            if (paymentModeDTO.DiscountCouponsDTOList != null && paymentModeDTO.DiscountCouponsDTOList.Any())
            {
                foreach (DiscountCouponsDTO discountCouponDTO in paymentModeDTO.DiscountCouponsDTOList)
                {
                    if (discountCouponDTO.PaymentModeId != paymentModeDTO.PaymentModeId)
                    {
                        discountCouponDTO.PaymentModeId = paymentModeDTO.PaymentModeId;
                    }
                    if (discountCouponDTO.IsChangedRecursive)
                    {
                        DiscountCouponsBL discountCouponsBL = new DiscountCouponsBL(executionContext, discountCouponDTO);
                        discountCouponsBL.Save(sqlTransaction);
                    }
                }
            }
            if (paymentModeDTO.CompatiablePaymentModesDTOList != null && paymentModeDTO.CompatiablePaymentModesDTOList.Any())
            {
                foreach (CompatiablePaymentModesDTO compatiablePaymentModesDTO in paymentModeDTO.CompatiablePaymentModesDTOList)
                {
                    if (compatiablePaymentModesDTO.PaymentModeId != paymentModeDTO.PaymentModeId)
                    {
                        compatiablePaymentModesDTO.PaymentModeId = paymentModeDTO.PaymentModeId;
                    }
                    if (compatiablePaymentModesDTO.IsChanged)
                    {
                        CompatiablePaymentModesBL compatiablePaymentModesBL = new CompatiablePaymentModesBL(executionContext, compatiablePaymentModesDTO);
                        compatiablePaymentModesBL.Save(sqlTransaction);
                    }
                }
            }

            if (paymentModeDTO.PaymentModeDisplayGroupsDTOList != null && paymentModeDTO.PaymentModeDisplayGroupsDTOList.Any())
            {
                foreach (PaymentModeDisplayGroupsDTO paymentModeDisplayGroupsDTO in paymentModeDTO.PaymentModeDisplayGroupsDTOList)
                {
                    if (paymentModeDisplayGroupsDTO.PaymentModeId != paymentModeDTO.PaymentModeId)
                    {
                        paymentModeDisplayGroupsDTO.PaymentModeId = paymentModeDTO.PaymentModeId;
                    }
                    if (paymentModeDisplayGroupsDTO.IsChanged)
                    {
                        PaymentModeDisplayGroupsBL paymentModeDisplayGroupsBL = new PaymentModeDisplayGroupsBL(executionContext, paymentModeDisplayGroupsDTO);
                        paymentModeDisplayGroupsBL.Save(sqlTransaction);
                    }
                }
            }
            //}
            //else
            //{
            //    if (paymentModeDTO.PaymentModeId > 0)
            //    {
            //        paymentModeDatahandler.Delete(paymentModeDTO.PaymentModeId);
            //    }
            //}
            log.LogMethodExit();
        }

        public PaymentModeDTO GetPaymentModeDTO { get { return paymentModeDTO; } }

        /// <summary>
        /// Returns the gateway.
        /// </summary>
        public string Gateway
        {
            get
            {
                //gateway value will be cached. Subsequent queries won't access database. 
                if (gateway == null)
                {
                    gateway = string.Empty;
                    if (paymentModeDTO != null && paymentModeDTO.Gateway != -1)
                    {
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE_ID, paymentModeDTO.Gateway.ToString()));
                        List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParameters);
                        if (lookupValuesDTOList != null &&
                           lookupValuesDTOList.Count > 0 &&
                           string.IsNullOrWhiteSpace(lookupValuesDTOList[0].LookupValue) == false)
                        {
                            gateway = lookupValuesDTOList[0].LookupValue;
                        }
                    }
                }
                return gateway;
            }
        }
        /// <summary>
        /// GetEligibleDisplayGroups for the pos machine and user role
        /// </summary> 
        /// <returns></returns>
        public List<PaymentModeDisplayGroupsDTO> GetEligibleDisplayGroups(int posMachineId, int userRoleId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(posMachineId, userRoleId, sqlTransaction);
            if (this.paymentModeDTO.PaymentModeId < 0)
            {
                //Please save changes before this operation
                string message = MessageContainerList.GetMessage(executionContext, 665);
                ValidationException validationException = new ValidationException(message);
                log.Error(validationException);
                throw validationException; 
            }
            if (posMachineId < 0 && userRoleId < 0)
            {
                //Please enter valid value for &1
                string appendText = MessageContainerList.GetMessage(executionContext, "POS Machine") + " and " + MessageContainerList.GetMessage(executionContext, "User Role");
                string message = MessageContainerList.GetMessage(executionContext, 1144, appendText);
                ValidationException validationException = new ValidationException(message);
                log.Error(validationException);
                throw validationException;
            }
            int resultVal = 0;
            if (posMachineId < -1 || Int32.TryParse(posMachineId.ToString(), out resultVal) == false)
            {
                //Please enter valid value for &1
                string appendText = MessageContainerList.GetMessage(executionContext, "POS Machine");
                string message = MessageContainerList.GetMessage(executionContext, 1144, appendText);
                ValidationException validationException = new ValidationException(message);
                log.Error(validationException);
                throw validationException;
            }
            if (userRoleId < -1 || Int32.TryParse(userRoleId.ToString(), out resultVal) == false)
            {
                //Please enter valid value for &1
                string appendText = MessageContainerList.GetMessage(executionContext, "User Role");
                string message = MessageContainerList.GetMessage(executionContext, 1144, appendText);
                ValidationException validationException = new ValidationException(message);
                log.Error(validationException);
                throw validationException;
            }
            List<PaymentModeDisplayGroupsDTO> paymentModeDisplayGroupsDTOList = new List<PaymentModeDisplayGroupsDTO>();
            List<KeyValuePair<PaymentModeDisplayGroupsDTO.SearchByParameters, string>> searchParams = 
                                                                                    new List<KeyValuePair<PaymentModeDisplayGroupsDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<PaymentModeDisplayGroupsDTO.SearchByParameters, string>(PaymentModeDisplayGroupsDTO.SearchByParameters.PAYMENT_MODE_ID, 
                                                                                                                       this.paymentModeDTO.PaymentModeId.ToString()));
            if (posMachineId > -1)
            {
                searchParams.Add(new KeyValuePair<PaymentModeDisplayGroupsDTO.SearchByParameters, string>(PaymentModeDisplayGroupsDTO.SearchByParameters.NOT_EXCLUDED_FOR_POS_MACHINE_ID, posMachineId.ToString()));
            }
            if (userRoleId > -1)
            {
                searchParams.Add(new KeyValuePair<PaymentModeDisplayGroupsDTO.SearchByParameters, string>(PaymentModeDisplayGroupsDTO.SearchByParameters.NOT_EXCLUDED_FOR_USER_ROLE_ID, userRoleId.ToString()));
            }
            PaymentModeDisplayGroupsListBL paymentModeDisplayGroupsBL = new PaymentModeDisplayGroupsListBL(executionContext);
            paymentModeDisplayGroupsDTOList = paymentModeDisplayGroupsBL.GetPaymentModeDisplayGroups(searchParams, sqlTransaction);
            log.LogMethodExit();
            return paymentModeDisplayGroupsDTOList;
        }
    }


    /// <summary>
    ///  PaymentModeList Class
    /// </summary>
    public class PaymentModeList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<PaymentModeDTO> paymentModeDTOList;

        public PaymentModeList()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// parameterized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public PaymentModeList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.paymentModeDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="paymentModeDTOsList">paymentModeDTOsList</param>
        /// <param name="executionContext">executionContext</param>
        public PaymentModeList(ExecutionContext executionContext, List<PaymentModeDTO> paymentModeDTOsList)
        {
            log.LogMethodEntry(executionContext, paymentModeDTOsList);
            this.paymentModeDTOList = paymentModeDTOsList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        ///Takes PaymentModeParams as parameter
        /// </summary>
        /// <param name="paymentModeParams">PaymentModeParams</param>
        /// <returns>Returns search parameters<KeyValuePair<<PaymentModeDTO.SearchByParameters, string>> by converting PaymentModeParams</returns>
        public List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> BuildPaymentModeSearchParametersList(PaymentModeParams paymentParams)
        {
            log.LogMethodEntry(paymentParams);
            List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> lSearchParams = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();

            if (paymentParams.PaymentModeId != -1)
                lSearchParams.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.PAYMENT_MODE_ID, Convert.ToString(paymentParams.PaymentModeId)));
            if (!string.IsNullOrEmpty(paymentParams.PaymentMode))
                lSearchParams.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.PAYMENT_MODE, Convert.ToString(paymentParams.PaymentMode)));
            if (paymentParams.SiteId != -1)
                lSearchParams.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, paymentParams.SiteId.ToString()));
            if (!string.IsNullOrEmpty(paymentParams.PaymentChannel))
                lSearchParams.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.PAYMENT_CHANNEL_NAME, Convert.ToString(paymentParams.PaymentChannel)));

            log.LogMethodExit(lSearchParams);
            return lSearchParams;
        }

        /// <summary>
        ///  Returns payment modes having payment gateways.
        /// </summary>
        /// <param name="posAvailable">bool posAvailable</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns> List of PaymentModeDTO</returns>
        public List<PaymentModeDTO> GetPaymentModesWithPaymentGateway(bool? posAvailable = null, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(posAvailable, sqlTransaction);
            PaymentModeDatahandler paymentModeDatahandler = new PaymentModeDatahandler(sqlTransaction);
            List<PaymentModeDTO> paymentModeDTOList = paymentModeDatahandler.GetPaymentModesWithPaymentGateway(posAvailable);
            log.LogMethodExit(paymentModeDTOList);
            return paymentModeDTOList;
        }

        /// <summary>
        /// Gets the PaymentModeDTO  matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of  PaymentModeDTO matching the search criteria</returns>
        public List<PaymentModeDTO> GetPaymentModeList(List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            PaymentModeDatahandler paymentModeDatahandler = new PaymentModeDatahandler(sqlTransaction);
            List<PaymentModeDTO> paymentModeDTOList = paymentModeDatahandler.GetPaymentModeList(searchParameters);
            log.LogMethodExit(paymentModeDTOList);
            return paymentModeDTOList;

        }
        /// <summary>
        ///  Returns the PaymentModeList,PaymentChannelList and Coupons list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="loadChild">loadChild</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of PaymentModeDTO</returns>
        public List<PaymentModeDTO> GetAllPaymentModeList(List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters, bool loadChild = false, bool loadActiveChild= false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChild, sqlTransaction);
            PaymentModeDatahandler paymentModeDatahandler = new PaymentModeDatahandler(sqlTransaction);
            List<PaymentModeDTO> paymentModeDTOList = paymentModeDatahandler.GetPaymentModeList(searchParameters);
            if (paymentModeDTOList != null && paymentModeDTOList.Count != 0 && loadChild)
            {
                foreach (PaymentModeDTO paymentModeDTO in paymentModeDTOList)
                {
                    List<KeyValuePair<PaymentModeChannelsDTO.SearchByParameters, string>> searchPaymentModeChannelParameters = new List<KeyValuePair<PaymentModeChannelsDTO.SearchByParameters, string>>();
                    searchPaymentModeChannelParameters.Add(new KeyValuePair<PaymentModeChannelsDTO.SearchByParameters, string>(PaymentModeChannelsDTO.SearchByParameters.PAYMENT_MODE_ID, paymentModeDTO.PaymentModeId.ToString()));
                    if (loadActiveChild)
                    {
                        searchPaymentModeChannelParameters.Add(new KeyValuePair<PaymentModeChannelsDTO.SearchByParameters, string>(PaymentModeChannelsDTO.SearchByParameters.ISACTIVE, "1"));
                    }
                    PaymentChannelList paymentChannelList = new PaymentChannelList(executionContext);
                    paymentModeDTO.PaymentModeChannelsDTOList = paymentChannelList.GetAllPaymentChannels(searchPaymentModeChannelParameters, sqlTransaction);
                }
                foreach (PaymentModeDTO paymentModeDTO in paymentModeDTOList)
                {
                    List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>> searchTaxRulesParameters = new List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>>();
                    searchTaxRulesParameters.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.PAYMENT_MODE_ID, paymentModeDTO.PaymentModeId.ToString()));
                    DiscountCouponsListBL discountCouponsListBL = new DiscountCouponsListBL(executionContext);
                    paymentModeDTO.DiscountCouponsDTOList = discountCouponsListBL.GetDiscountCouponsAndUsedCouponsList(searchTaxRulesParameters);
                }
                foreach (PaymentModeDTO paymentModeDTO in paymentModeDTOList)
                {
                    List<KeyValuePair<CompatiablePaymentModesDTO.SearchByParameters, string>> searchCompatiablePaymentModesParameters = new List<KeyValuePair<CompatiablePaymentModesDTO.SearchByParameters, string>>();
                    searchCompatiablePaymentModesParameters.Add(new KeyValuePair<CompatiablePaymentModesDTO.SearchByParameters, string>(CompatiablePaymentModesDTO.SearchByParameters.PAYMENT_MODE_ID, paymentModeDTO.PaymentModeId.ToString()));
                    if (loadActiveChild)
                    {
                        searchCompatiablePaymentModesParameters.Add(new KeyValuePair<CompatiablePaymentModesDTO.SearchByParameters, string>(CompatiablePaymentModesDTO.SearchByParameters.IS_ACTIVE, "1"));
                    }
                    CompatiablePaymentModesListBL compatiablePaymentModesListBL = new CompatiablePaymentModesListBL(executionContext);
                    paymentModeDTO.CompatiablePaymentModesDTOList = compatiablePaymentModesListBL.GetCompatiablePaymentModes(searchCompatiablePaymentModesParameters);
                }

                foreach (PaymentModeDTO paymentModeDTO in paymentModeDTOList)
                {
                    List<KeyValuePair<PaymentModeDisplayGroupsDTO.SearchByParameters, string>> searchPaymentModeDisplayGroupsParameters = new List<KeyValuePair<PaymentModeDisplayGroupsDTO.SearchByParameters, string>>();
                    searchPaymentModeDisplayGroupsParameters.Add(new KeyValuePair<PaymentModeDisplayGroupsDTO.SearchByParameters, string>(PaymentModeDisplayGroupsDTO.SearchByParameters.PAYMENT_MODE_ID, paymentModeDTO.PaymentModeId.ToString()));
                    if (loadActiveChild)
                    {
                        searchPaymentModeDisplayGroupsParameters.Add(new KeyValuePair<PaymentModeDisplayGroupsDTO.SearchByParameters, string>(PaymentModeDisplayGroupsDTO.SearchByParameters.IS_ACTIVE, "1"));
                    }
                    PaymentModeDisplayGroupsListBL paymentModeDisplayGroupsListBL = new PaymentModeDisplayGroupsListBL(executionContext);
                    paymentModeDTO.PaymentModeDisplayGroupsDTOList = paymentModeDisplayGroupsListBL.GetPaymentModeDisplayGroups(searchPaymentModeDisplayGroupsParameters);
                }
            }

            log.LogMethodExit(paymentModeDTOList);
            return paymentModeDTOList;
        }

        public List<PaymentModeDTO> GetPaymentModeDTOList(List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters, bool loadChild = false, bool loadActiveChild = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChild, loadActiveChild, sqlTransaction);
            PaymentModeDatahandler paymentModeDatahandler = new PaymentModeDatahandler(sqlTransaction);
            List<PaymentModeDTO> paymentModeDTOList = paymentModeDatahandler.GetPaymentModeList(searchParameters);
            if (paymentModeDTOList != null && paymentModeDTOList.Count != 0 && loadChild)
            {
                foreach (PaymentModeDTO paymentModeDTO in paymentModeDTOList)
                {
                    List<KeyValuePair<PaymentModeChannelsDTO.SearchByParameters, string>> searchPaymentModeChannelParameters = new List<KeyValuePair<PaymentModeChannelsDTO.SearchByParameters, string>>();
                    searchPaymentModeChannelParameters.Add(new KeyValuePair<PaymentModeChannelsDTO.SearchByParameters, string>(PaymentModeChannelsDTO.SearchByParameters.PAYMENT_MODE_ID, paymentModeDTO.PaymentModeId.ToString()));
                    if (loadActiveChild)
                    {
                        searchPaymentModeChannelParameters.Add(new KeyValuePair<PaymentModeChannelsDTO.SearchByParameters, string>(PaymentModeChannelsDTO.SearchByParameters.ISACTIVE, "1"));
                    }
                    PaymentChannelList paymentChannelList = new PaymentChannelList(executionContext);
                    paymentModeDTO.PaymentModeChannelsDTOList = paymentChannelList.GetAllPaymentChannels(searchPaymentModeChannelParameters, sqlTransaction);

                    List<KeyValuePair<CompatiablePaymentModesDTO.SearchByParameters, string>> searchCompatiablePaymentModesParameters = new List<KeyValuePair<CompatiablePaymentModesDTO.SearchByParameters, string>>();
                    searchCompatiablePaymentModesParameters.Add(new KeyValuePair<CompatiablePaymentModesDTO.SearchByParameters, string>(CompatiablePaymentModesDTO.SearchByParameters.PAYMENT_MODE_ID, paymentModeDTO.PaymentModeId.ToString()));
                    if (loadActiveChild)
                    {
                        searchCompatiablePaymentModesParameters.Add(new KeyValuePair<CompatiablePaymentModesDTO.SearchByParameters, string>(CompatiablePaymentModesDTO.SearchByParameters.IS_ACTIVE, "1"));
                    }
                    CompatiablePaymentModesListBL compatiablePaymentModesListBL = new CompatiablePaymentModesListBL(executionContext);
                    paymentModeDTO.CompatiablePaymentModesDTOList = compatiablePaymentModesListBL.GetCompatiablePaymentModes(searchCompatiablePaymentModesParameters, sqlTransaction);

                    List<KeyValuePair<PaymentModeDisplayGroupsDTO.SearchByParameters, string>> searchPaymentModeDisplayGroupsParameters = new List<KeyValuePair<PaymentModeDisplayGroupsDTO.SearchByParameters, string>>();
                    searchPaymentModeDisplayGroupsParameters.Add(new KeyValuePair<PaymentModeDisplayGroupsDTO.SearchByParameters, string>(PaymentModeDisplayGroupsDTO.SearchByParameters.PAYMENT_MODE_ID, paymentModeDTO.PaymentModeId.ToString()));
                    if (loadActiveChild)
                    {
                        searchPaymentModeDisplayGroupsParameters.Add(new KeyValuePair<PaymentModeDisplayGroupsDTO.SearchByParameters, string>(PaymentModeDisplayGroupsDTO.SearchByParameters.IS_ACTIVE, "1"));
                    }
                    PaymentModeDisplayGroupsListBL paymentModeDisplayGroupsListBL = new PaymentModeDisplayGroupsListBL(executionContext);
                    paymentModeDTO.PaymentModeDisplayGroupsDTOList = paymentModeDisplayGroupsListBL.GetPaymentModeDisplayGroups(searchPaymentModeDisplayGroupsParameters, sqlTransaction);

                }
            }

            log.LogMethodExit(paymentModeDTOList);
            return paymentModeDTOList;
        }


        /// <summary>
        /// Gets the PaymentModeDTO  matching the search key
        /// </summary>
        /// <param name="paymentParams">List of search parameters</param>
        /// <returns>Returns the list of PaymentModeDTO matching the search criteria</returns>
        public List<PaymentModeDTO> GetPaymentModeList(PaymentModeParams paymentParams, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(paymentParams, sqlTransaction);

            List<PaymentModeDTO> paymentModeDTOList = new List<PaymentModeDTO>();
            PaymentModeDatahandler paymentModeDatahandler = new PaymentModeDatahandler(sqlTransaction);

            List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = BuildPaymentModeSearchParametersList(paymentParams);
            paymentModeDTOList = paymentModeDatahandler.GetPaymentModeList(searchParameters);

            log.LogMethodExit(paymentModeDTOList);
            return paymentModeDTOList;

        }
        /// <summary>
        /// This method should be used to Save and Update the Payment Mode details for Web Management Studio.
        /// </summary>
        public void SaveUpdatePaymentModesList()
        {
            log.LogMethodEntry();
            if (paymentModeDTOList != null && paymentModeDTOList.Count > 0)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (PaymentModeDTO paymentModeDto in paymentModeDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            PaymentMode paymentMode = new PaymentMode(executionContext, paymentModeDto);
                            paymentMode.Save(parafaitDBTrx.SQLTrx);
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
                }
                log.LogMethodExit();
            }
        }

        internal DateTime? GetPaymentModeModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry();
            PaymentModeDatahandler PaymentModeDataHandler = new PaymentModeDatahandler();
            DateTime? result = PaymentModeDataHandler.GetPaymentModeLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
