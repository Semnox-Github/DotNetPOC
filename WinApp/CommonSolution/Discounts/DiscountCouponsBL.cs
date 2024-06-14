/********************************************************************************************
 * Project Name - DiscountCoupons BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        15-Jul-2017      Lakshminarayana     Created 
 *1.01        30-Oct-2017      Lakshminarayana     Modified   Option to choose generated coupons to sequential or random, Allow multiple coupons in one transaction 
 *2.60.2      25-Jan-2019      Jagan Mohana        Created Constructor DiscountCouponsListBL and added new method SaveUpdateDiscountCouponsList().
 *            17-Mar-2019      Akshay Gulaganji    Modified isActive (string to bool)
 *            11-Apr-2019      Mushahid Faizan     Modified SaveUpdateDiscountCouponsList(), Added LogMethodEntry/Exit. *            
              19-APR-2019      Raghuveera          Added save and validation method in the list class.
 *            04-Jun-2019      Akshay Gulaganji    Code merge from Development to WebManagementStudio
 *2.70.0      25-Jul-2019      Mushahid Faizan     Added DeleteDiscountCoupons() method.
*2.110.0     14-Apr-2021      Girish Kundar       Modified : Issue fix - Validation for Discount coupons for FromNumber and To Numbers 
*2.120.2     12-May-2021      Mushahid Faizan       Modified : Issue fix - Validation for Discount coupons for expiry Date and Effective Date
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// Coupon number state enum
    /// </summary>
    public enum CouponStatus
    {
        /// <summary>
        /// Invalid Coupon, Coupon doesn't exist in the system
        /// </summary>
        INVALID,
        /// <summary>
        /// Active coupon
        /// </summary>
        ACTIVE,
        /// <summary>
        /// Inactive coupon
        /// </summary>
        IN_ACTIVE,
        /// <summary>
        /// Expired coupon
        /// </summary>
        EXPIRED,
        /// <summary>
        /// Ineffective coupon
        /// </summary>
        INEFFECTIVE,
        /// <summary>
        /// Coupon used in a transaction
        /// </summary>
        USED
    }
    /// <summary>
    /// Business logic for DiscountCoupons class.
    /// </summary>
    public class DiscountCouponsBL
    {
        private DiscountCouponsDTO discountCouponsDTO;
        public string couponNumber;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private int? businessStartHour;
        /// <summary>
        /// Default constructor of DiscountCouponsBL class
        /// </summary>
        public DiscountCouponsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            discountCouponsDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the discountCoupons id as the parameter
        /// Would fetch the discountCoupons object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">SQL Transaction</param>
        public DiscountCouponsBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id, sqlTransaction);
            DiscountCouponsDataHandler discountCouponsDataHandler = new DiscountCouponsDataHandler(sqlTransaction);
            discountCouponsDTO = discountCouponsDataHandler.GetDiscountCouponsDTO(id);
            log.LogMethodExit(discountCouponsDTO);
        }

        /// <summary>
        /// Constructor with the coupon number as the parameter
        /// Would fetch the discountCoupons object from the database based on the coupon number passed. 
        /// </summary>
        /// <param name="couponNumber">coupon number</param>
        /// <param name="sqlTransaction">SQL Transaction</param>
        public DiscountCouponsBL(ExecutionContext executionContext, string couponNumber, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(couponNumber, sqlTransaction);
            this.couponNumber = couponNumber;
            DiscountCouponsDataHandler discountCouponsDataHandler = new DiscountCouponsDataHandler(sqlTransaction);
            if (executionContext.IsCorporate)
            {
                discountCouponsDTO = discountCouponsDataHandler.GetDiscountCouponsDTO(couponNumber, executionContext.SiteId);
            }
            else
            {
                discountCouponsDTO = discountCouponsDataHandler.GetDiscountCouponsDTO(couponNumber);
            }

            log.LogMethodExit(discountCouponsDTO);
        }

        /// <summary>
        /// Creates DiscountCouponsBL object using the DiscountCouponsDTO
        /// </summary>
        /// <param name="discountCouponsDTO">DiscountCouponsDTO object</param>
        public DiscountCouponsBL(ExecutionContext executionContext, DiscountCouponsDTO discountCouponsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(discountCouponsDTO);
            this.discountCouponsDTO = discountCouponsDTO;
            log.LogMethodExit();
        }

        private void ValidateDuplicateCouponNumber(int couponSetId, string couponNumber, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(couponSetId, couponNumber, sqlTransaction);
            DiscountCouponsDataHandler discountCouponsDataHandler = new DiscountCouponsDataHandler(sqlTransaction);
            DiscountCouponsDTO duplicateDiscountCouponsDTO = discountCouponsDataHandler.GetDiscountCouponsDTO(couponNumber);
            if (duplicateDiscountCouponsDTO != null && duplicateDiscountCouponsDTO.CouponSetId != couponSetId)
            {
                log.Debug("Ends-DiscountCouponsBL(couponSetId, couponNumber) Method with an exception.");
                throw new DuplicateCouponException("Coupon with same number already exists.");
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the DiscountCouponsDTO. Throws DuplicateCouponException, ForeignKeyException
        /// </summary>
        public void ValidateCouponDefinition(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (discountCouponsDTO == null)
            {
                throw new ArgumentNullException("discountCouponsDTO is null.");
            }
            if (discountCouponsDTO.CouponSetId != -1)
            {
                DiscountCouponsDataHandler discountCouponsDataHandler = new DiscountCouponsDataHandler(sqlTransaction);
                if (discountCouponsDTO.IsActive == false && discountCouponsDataHandler.GetDiscountCouponsReferenceCount(discountCouponsDTO.CouponSetId) > 0)
                {
                    log.Debug("Ends-ValidateCouponDefinition(couponSetId, couponNumber) Method with an exception.");
                    throw new ForeignKeyException("Cannot Inactivate records for which matching detail data exists.");
                    //throw new ForeignKeyException("hello");
                }
            }
            if (string.IsNullOrWhiteSpace(discountCouponsDTO.FromNumber) == false)
            {
                ValidateDuplicateCouponNumber(discountCouponsDTO.CouponSetId, discountCouponsDTO.FromNumber, sqlTransaction);
            }
            if (string.IsNullOrWhiteSpace(discountCouponsDTO.ToNumber) == false)
            {
                ValidateDuplicateCouponNumber(discountCouponsDTO.CouponSetId, discountCouponsDTO.ToNumber, sqlTransaction);
            }
            if (discountCouponsDTO.Count < discountCouponsDTO.UsedCount)
            {
                throw new ValidationException("Invalid UsedCount.");
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the discount coupon.
        /// </summary>
        /// <returns></returns>
        public void ValidateCouponApplication(DateTime referenceDate, int transactionId = -1, SqlTransaction sqlTransaction = null)
        {

            log.LogMethodEntry(transactionId, sqlTransaction);
            BusinessDate businessDate = new BusinessDate(executionContext);
            if (string.IsNullOrEmpty(couponNumber))
            {
                throw new InvalidCouponException("Coupon number is empty.");
            }
            else if (DiscountCouponsDTO == null)
            {
                throw new InvalidCouponException("Invalid coupon number.");
            }
            else if (discountCouponsDTO.DiscountId == -1)
            {
                throw new ParafaitApplicationException("Invalid coupon");
            }
            else if (DiscountCouponsDTO.IsActive != true)
            {
                throw new InvalidCouponException("Coupon is not active.");
            }
            else if (DiscountCouponsDTO.ExpiryDate != null &&
                     DiscountCouponsDTO.ExpiryDate.Value.Date.AddDays(1).AddHours(businessDate.Start.Hour) < referenceDate)
            {
                throw new InvalidCouponException("Coupon expired.");
            }
            else if (DiscountCouponsDTO.StartDate != null &&
                     DiscountCouponsDTO.StartDate.Value.Date.AddHours(businessDate.Start.Hour) > referenceDate)
            {
                throw new InvalidCouponException("Coupon not yet valid.");
            }
            else if (IsCouponNumberUsed(transactionId, sqlTransaction))
            {
                throw new InvalidCouponException("Coupon use limit reached.");
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the discount coupon.
        /// </summary>
        /// <returns></returns>
        public void ValidateCouponApplication(int transactionId = -1, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(transactionId, sqlTransaction);
            if (string.IsNullOrEmpty(couponNumber))
            {
                throw new InvalidCouponException("Coupon number is empty.");
            }
            else if (DiscountCouponsDTO == null)
            {
                throw new InvalidCouponException("Invalid coupon number.");
            }
            else if (DiscountCouponsDTO.IsActive != true)
            {
                throw new InvalidCouponException("Coupon is not active.");
            }
            else if (IsCouponNumberUsed(transactionId, sqlTransaction))
            {
                throw new InvalidCouponException("Coupon use limit reached.");
            }

            log.LogMethodExit();
        }

        public string CouponNumber
        {
            get
            {
                return couponNumber;
            }
        }

        /// <summary>
        /// Saves the DiscountCoupons
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            DiscountCouponsDataHandler discountCouponsDataHandler = new DiscountCouponsDataHandler(sqlTransaction);
            ValidateCouponDefinition(sqlTransaction);
            ValidateDiscountCouponsDTO();
            if (discountCouponsDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "discountCouponsDTO is not changed.");
                return;
            }
            discountCouponsDataHandler.Save(discountCouponsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
        }

        private void ValidateDiscountCouponsDTO()
        {
            log.LogMethodEntry(discountCouponsDTO);
            string message = string.Empty;

            DiscountCouponsHeaderDataHandler discountCouponsHeaderDataHandler = new DiscountCouponsHeaderDataHandler();
            List<KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>(DiscountCouponsHeaderDTO.SearchByParameters.ID, discountCouponsDTO.DiscountCouponHeaderId.ToString()));
            DiscountCouponsHeaderDTO discountCouponsHeaderDTO = discountCouponsHeaderDataHandler.GetDiscountCouponsHeaderDTO(discountCouponsDTO.DiscountCouponHeaderId);
            if (discountCouponsHeaderDTO != null || discountCouponsHeaderDTO.Count != 0)
            {
                if (discountCouponsDTO.StartDate.Value.Date < discountCouponsHeaderDTO.EffectiveDate.Value.Date)
                {
                    message = message = MessageContainerList.GetMessage(executionContext, 1144, "Effective Date");
                    throw new ValidationException(message);
                }
                if (discountCouponsDTO.ExpiryDate.Value.Date > discountCouponsHeaderDTO.ExpiryDate.Value.Date)
                {
                    message = message = MessageContainerList.GetMessage(executionContext, 1144, "Expiry Date");
                    throw new ValidationException(message);
                }
            }
            if (discountCouponsDTO.Count <= 0)
            {
                message = MessageContainerList.GetMessage(executionContext, 1144, "count");
                throw new ValidationException(message);
            }
            if (discountCouponsDTO.ExpiryDate < discountCouponsDTO.StartDate)
            {
                message = message = MessageContainerList.GetMessage(executionContext, 1144, "Expiry Date");
                throw new ValidationException(message);
            }
            if (string.IsNullOrWhiteSpace(discountCouponsDTO.FromNumber) || discountCouponsDTO.FromNumber.Contains(" "))
            {
                message = message = MessageContainerList.GetMessage(executionContext, 1144, "From Number");
                throw new ValidationException(message);
            }
            if (string.IsNullOrWhiteSpace(discountCouponsDTO.ToNumber) == false && discountCouponsDTO.ToNumber.Contains(" "))
            {
                message = message = MessageContainerList.GetMessage(executionContext, 1144, "To Number");
                throw new ValidationException(message);
            }
            log.LogMethodExit(message);
        }
        private bool IsCouponNumberUsed(int transactionId = -1, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(transactionId, sqlTransaction);
            bool returnValue = false;
            DiscountCouponsDataHandler discountCouponsDataHandler = new DiscountCouponsDataHandler(sqlTransaction);

            if (string.IsNullOrWhiteSpace(discountCouponsDTO.FromNumber) == false &&
                string.IsNullOrWhiteSpace(discountCouponsDTO.ToNumber) == false)
            {
                bool isUsedInAnotherTransaction =
                    discountCouponsDataHandler.IsCouponNumberUsedInAnotherTransaction(transactionId, couponNumber);
                if (isUsedInAnotherTransaction)
                {
                    log.LogMethodExit(true);
                    return true;
                }
            }
            
            bool isUsedInTransaction = discountCouponsDataHandler.IsCouponNumberUsedInTransaction(transactionId, couponNumber);
            int reduceUsageBy = 0;
            if (isUsedInTransaction)
            {
                reduceUsageBy = 1;
            }
            if (DiscountCouponsDTO.UsedCount - reduceUsageBy >= DiscountCouponsDTO.Count)
            {
                returnValue = true;
            }
            
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Returns the coupon status
        /// </summary>
        /// <returns></returns>
        public CouponStatus CouponStatus
        {
            get
            {
                CouponStatus returValue = CouponStatus.INVALID;
                if (discountCouponsDTO != null && discountCouponsDTO.CouponSetId != -1)
                {
                    LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                    DateTime serverDateTime = lookupValuesList.GetServerDateTime();
                    if (IsCouponNumberUsed())
                    {
                        returValue = CouponStatus.USED;
                    }
                    else if (discountCouponsDTO.IsActive == false)
                    {
                        returValue = CouponStatus.IN_ACTIVE;
                    }
                    else if (discountCouponsDTO.ExpiryDate != null && discountCouponsDTO.ExpiryDate.Value.Date.AddDays(1).AddHours(GetBusinessStartHour()) < serverDateTime)
                    {
                        returValue = CouponStatus.EXPIRED;
                    }
                    else if (discountCouponsDTO.StartDate != null && discountCouponsDTO.StartDate.Value.Date.AddHours(GetBusinessStartHour()) > serverDateTime)
                    {
                        returValue = CouponStatus.INEFFECTIVE;
                    }
                    else
                    {
                        returValue = CouponStatus.ACTIVE;
                    }
                }
                return returValue;
            }

        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public DiscountCouponsDTO DiscountCouponsDTO
        {
            get
            {
                return discountCouponsDTO;
            }
        }

        private int GetBusinessStartHour()
        {
            log.LogMethodEntry();
            if (businessStartHour == null)
            {
                businessStartHour = 6;

                ParafaitDefaultsListBL parafaitDefaultsListBL = new ParafaitDefaultsListBL(executionContext);
                List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParafaitDefaultsParam = new List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>>();
                searchParafaitDefaultsParam.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME, "BUSINESS_DAY_START_TIME"));
                List<ParafaitDefaultsDTO> parafaitDefaultsDTOList = parafaitDefaultsListBL.GetParafaitDefaultsDTOList(searchParafaitDefaultsParam);
                if (parafaitDefaultsDTOList != null && parafaitDefaultsDTOList.Count > 0)
                {
                    int i;
                    if (int.TryParse(parafaitDefaultsDTOList[0].DefaultValue, out i))
                    {
                        businessStartHour = i;
                    }
                }
            }
            log.LogMethodExit(businessStartHour);
            return (int)businessStartHour;
        }


        public void Use(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            DiscountCouponsDTO.UsedCount++;
            DiscountCouponsDataHandler discountCouponsDataHandler = new DiscountCouponsDataHandler(sqlTransaction);
            discountCouponsDataHandler.Save(discountCouponsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
        }

        public void Unuse(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            DiscountCouponsDTO.UsedCount--;
            DiscountCouponsDataHandler discountCouponsDataHandler = new DiscountCouponsDataHandler(sqlTransaction);
            discountCouponsDataHandler.Save(discountCouponsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of DiscountCoupons
    /// </summary>
    public class DiscountCouponsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<DiscountCouponsDTO> discountCouponsDTOList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public DiscountCouponsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.discountCouponsDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="discountCouponsDTOList"></param>
        public DiscountCouponsListBL(ExecutionContext executionContext, List<DiscountCouponsDTO> discountCouponsDTOList)
        {
            log.LogMethodEntry(executionContext, discountCouponsDTOList);
            this.executionContext = executionContext;
            this.discountCouponsDTOList = discountCouponsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the DiscountCoupons list
        /// </summary>
        public List<DiscountCouponsDTO> GetDiscountCouponsDTOList(List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            DiscountCouponsDataHandler discountCouponsDataHandler = new DiscountCouponsDataHandler(sqlTransaction);
            log.LogMethodExit();
            return discountCouponsDataHandler.GetDiscountCouponsDTOList(searchParameters);
        }

        /// <summary>
        /// Returns the DiscountCoupons list
        /// </summary>
        public List<DiscountCouponsDTO> GetDiscountCouponsAndUsedCouponsList(List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            DiscountCouponsDataHandler discountCouponsDataHandler = new DiscountCouponsDataHandler(sqlTransaction);
            List<DiscountCouponsDTO> discountCouponsDTOList = discountCouponsDataHandler.GetDiscountCouponsDTOList(searchParameters);

            if (discountCouponsDTOList != null)
            {
                foreach (DiscountCouponsDTO discountCouponsDTO in discountCouponsDTOList)
                {
                    List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>> searchDiscountCouponsUsedParameters = new List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>>();
                    searchDiscountCouponsUsedParameters.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchDiscountCouponsUsedParameters.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.COUPON_SET_ID, discountCouponsDTO.CouponSetId.ToString()));
                    DiscountCouponsUsedListBL discountCouponsUsedListBL = new DiscountCouponsUsedListBL(executionContext);
                    discountCouponsDTO.DiscountCouponsUsedDTOList = discountCouponsUsedListBL.GetDiscountCouponsUsedDTOList(searchDiscountCouponsUsedParameters); ;
                }
            }
            log.LogMethodExit(discountCouponsDTOList);
            return discountCouponsDTOList;
        }
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            List<ValidationError> validationErrors = new List<ValidationError>();
            if (discountCouponsDTOList.Any(x => x.CouponSetId >= 0))
            {
                ValidationError validationError = new ValidationError("DiscountCoupon", "CouponSetId", "List save is implemented for new coupons only");
                validationErrors.Add(validationError);
            }

            List<DiscountCouponsDTO> discountCouponsDTOFilterList;
            for (int i = 0; i < discountCouponsDTOList.Count; i++)
            {
                if ((string.IsNullOrEmpty(discountCouponsDTOList[i].FromNumber)
                    && (string.IsNullOrEmpty(discountCouponsDTOList[i].ToNumber) || !string.IsNullOrEmpty(discountCouponsDTOList[i].ToNumber))))
                {
                    validationErrors.Add(new ValidationError("DiscountCoupon", "FromNumber", "Coupon number is empty", -1, i));
                }
                discountCouponsDTOFilterList = discountCouponsDTOList.Where(x => (bool)(discountCouponsDTOList[i].FromNumber == x.FromNumber)).ToList<DiscountCouponsDTO>();
                if (discountCouponsDTOFilterList != null && discountCouponsDTOFilterList.Count > 1)
                {
                    validationErrors.Add(new ValidationError("DiscountCoupon", "FromNumber", "Duplicate coupon number", -1, i));
                }
                if (discountCouponsDTOList[i].DiscountId == -1 && discountCouponsDTOList[i].DiscountCouponHeaderId == -1 && discountCouponsDTOList[i].PaymentModeId == -1)
                {
                    validationErrors.Add(new ValidationError("DiscountCoupon", "DiscountId", "Coupon number is not a valid type(Discount/Payment).", -1, i));
                }
            }

            DiscountCouponsDataHandler discountCouponsDataHandler = new DiscountCouponsDataHandler(sqlTransaction);
            discountCouponsDTOFilterList = discountCouponsDataHandler.ValidateDuplicate(discountCouponsDTOList, executionContext.GetUserId(), executionContext.GetSiteId());
            if (discountCouponsDTOFilterList != null && discountCouponsDTOFilterList.Count > 0)
            {
                for (int i = discountCouponsDTOList.Count - 1; i >= 0; i--)
                {
                    for (int j = 0; j < discountCouponsDTOFilterList.Count; j++)
                    {
                        if (AreEqual(discountCouponsDTOList[i].FromNumber, discountCouponsDTOFilterList[j].FromNumber) &&
                            AreEqual(discountCouponsDTOList[i].ToNumber, discountCouponsDTOFilterList[j].ToNumber))
                        {
                            validationErrors.Add(new ValidationError("DiscountCoupon", "FromNumber", "Coupon number already exists", -1, i));
                        }
                    }
                }
            }
            log.LogMethodExit(validationErrors);
            return validationErrors;
        }

        private bool AreEqual(string left, string right)
        {
            log.LogMethodEntry(left, right);
            bool result = (string.IsNullOrWhiteSpace(left) && string.IsNullOrWhiteSpace(right)) ||
                string.Equals(left, right);
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Validates and saves the discountCouponsDTOList to the db
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (discountCouponsDTOList == null ||
                discountCouponsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                throw new Exception("Can't save empty list.");
            }
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                throw new ValidationException("Duplicate Coupon numbers", validationErrors);
            }
            DiscountCouponsDataHandler discountCouponsDataHandler = new DiscountCouponsDataHandler(sqlTransaction);
            discountCouponsDataHandler.Save(discountCouponsDTOList, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the ProductBarcodeDTO List for Product Id List
        /// </summary>
        /// <param name="couponSetIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductDTO</returns>
        public List<DiscountCouponsDTO> GetDiscountCouponsDTOList(List<int> couponSetIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(couponSetIdList);
            DiscountCouponsDataHandler discountCouponsDataHandler = new DiscountCouponsDataHandler(sqlTransaction);
            List<DiscountCouponsDTO> discountCouponsDTOList = discountCouponsDataHandler.GetDiscountCouponsDTOList(couponSetIdList, activeRecords);
            log.LogMethodExit(discountCouponsDTOList);
            return discountCouponsDTOList;
        }
    }

    /// <summary>
    /// Represents duplicate coupon error that occur during application execution. 
    /// </summary>
    public class DuplicateCouponException : Exception
    {
        /// <summary>
        /// Default constructor of DuplicateCouponException.
        /// </summary>
        public DuplicateCouponException()
        {
        }

        /// <summary>
        /// Initializes a new instance of DuplicateCouponException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public DuplicateCouponException(string message)
        : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of DuplicateCouponException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">inner exception</param>
        public DuplicateCouponException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }

    /// <summary>
    /// Represents invalid coupon error that occur during application execution. 
    /// </summary>
    public class InvalidCouponException : Exception
    {
        /// <summary>
        /// Default constructor of InvalidCouponException.
        /// </summary>
        public InvalidCouponException()
        {
        }

        /// <summary>
        /// Initializes a new instance of InvalidCouponException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public InvalidCouponException(string message)
        : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of InvalidCouponException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">inner exception</param>
        public InvalidCouponException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }

    /// <summary>
    /// Represents coupon mandatory error that occur during application execution. 
    /// </summary>
    public class CouponMandatoryException : Exception
    {
        /// <summary>
        /// Default constructor of CouponMandatoryException.
        /// </summary>
        public CouponMandatoryException()
        {
        }

        /// <summary>
        /// Initializes a new instance of CouponMandatoryException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public CouponMandatoryException(string message)
        : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of CouponMandatoryException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">inner exception</param>
        public CouponMandatoryException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }

    /// <summary>
    /// Represents coupon used error that occur during application execution. 
    /// </summary>
    public class CouponUsedException : Exception
    {
        /// <summary>
        /// Default constructor of CouponUsedException.
        /// </summary>
        public CouponUsedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of CouponUsedException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public CouponUsedException(string message)
        : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of CouponUsedException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">inner exception</param>
        public CouponUsedException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
