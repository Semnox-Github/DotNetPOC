/********************************************************************************************
 * Project Name - DiscountCouponsHeader BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 *********************************************************************************************
 *1.00        15-Jul-2017      Lakshminarayana      Created 
 *1.01        30-Oct-2017      Lakshminarayana      Modified   Option to choose generated coupons to sequential or random, Allow multiple coupons in one transaction
 *2.60        05-Mar-2019      Akshay Gulaganji     Added DiscountCouponsHeaderBL(discountCouponsHeaderDTO,executionContext) and modified Save() method in DiscountCouponsHeaderBL class
 *                                                  Added DiscountCouponsHeaderListBL(discountCouponsHeaderDTOList,executionContext), SaveDiscountCouponsHeaderList() methods and 
 *                                                  modified GetDiscountCouponsHeaderDTOList(searchParameters) in DiscountCouponsHeaderListBL class
 *2.70.2        31-Jul-2019      Girish Kundar        Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *2.110.0       15-Dec-2020      Girish Kundar        Modified : Issue fix discount roaming from local to HQ
  * ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// Business logic for DiscountCouponsHeader class.
    /// </summary>
    public class DiscountCouponsHeaderBL
    {
        private DiscountCouponsHeaderDTO discountCouponsHeaderDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<char> numericCharacters;
        private List<char> alphaCharacters;
        private ExecutionContext executionContext;
        /// <summary>
        /// Default constructor of DiscountCouponsHeaderBL class
        /// </summary>
        public DiscountCouponsHeaderBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            discountCouponsHeaderDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor with discountCouponsHeaderDTO and executionContext
        /// </summary>
        /// <param name="discountCouponsHeaderDTO"></param>
        /// <param name="executionContext"></param>
        public DiscountCouponsHeaderBL(ExecutionContext executionContext, DiscountCouponsHeaderDTO discountCouponsHeaderDTO)
        {
            log.LogMethodEntry(executionContext, discountCouponsHeaderDTO);
            this.discountCouponsHeaderDTO = discountCouponsHeaderDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the discountCouponsHeader id as the parameter
        /// Would fetch the discountCouponsHeader object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        public DiscountCouponsHeaderBL(ExecutionContext executionContext, int id)
            : this(executionContext)
        {
            log.LogMethodEntry(id);
            DiscountCouponsHeaderDataHandler discountCouponsHeaderDataHandler = new DiscountCouponsHeaderDataHandler();
            discountCouponsHeaderDTO = discountCouponsHeaderDataHandler.GetDiscountCouponsHeaderDTO(id);
            log.LogMethodExit(discountCouponsHeaderDTO);
        }
        
        /// <summary>
        /// Saves the DiscountCouponsHeader
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();
            DiscountCouponsHeaderDataHandler discountCouponsHeaderDataHandler = new DiscountCouponsHeaderDataHandler();
            if (discountCouponsHeaderDTO.Id < 0)
            {
                discountCouponsHeaderDTO = discountCouponsHeaderDataHandler.InsertDiscountCouponsHeader(discountCouponsHeaderDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                discountCouponsHeaderDTO.AcceptChanges();
            }
            else
            {
                if (discountCouponsHeaderDTO.IsChanged)
                {
                    discountCouponsHeaderDTO = discountCouponsHeaderDataHandler.UpdateDiscountCouponsHeader(discountCouponsHeaderDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    discountCouponsHeaderDTO.AcceptChanges();
                }
            }
            if (discountCouponsHeaderDTO.DiscountCouponsDTOList != null && discountCouponsHeaderDTO.DiscountCouponsDTOList.Count > 0)
            {
                foreach (DiscountCouponsDTO discountCouponsDTO in discountCouponsHeaderDTO.DiscountCouponsDTOList)
                {
                    if (discountCouponsDTO.IsChanged || discountCouponsDTO.CouponSetId < 0)
                    {
                        discountCouponsDTO.DiscountCouponHeaderId = discountCouponsHeaderDTO.Id;
                        if (discountCouponsDTO.ExpiryDate > discountCouponsHeaderDTO.ExpiryDate)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 5175)); //Discount coupon expiry date should be less than Discount coupon header expiry date.
                        }
                        DiscountCouponsBL discountCouponsBL = new DiscountCouponsBL(executionContext, discountCouponsDTO);
                        discountCouponsBL.Save();
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Issues new coupons 
        /// </summary>
        /// <param name="initialCouponNumber">Initial Coupon number in the series</param>
        /// <param name="effectiveDate">coupon effective date</param>
        /// <param name="expiryDate">coupons expiry date</param>
        /// <returns>Returns list of issued coupons</returns>
        public List<DiscountCouponsDTO> IssueCoupons(string initialCouponNumber, 
                                                     DateTime effectiveDate, 
                                                     DateTime expiryDate,
                                                     int productQuantity)//added  productQuantity
        {
            log.LogMethodEntry(initialCouponNumber, effectiveDate, expiryDate, productQuantity);
            List<DiscountCouponsDTO> discountCouponsDTOList = null;
            if (string.IsNullOrWhiteSpace(initialCouponNumber) == false)
            {
                string currentCouponNumber = initialCouponNumber.ToUpper();
                discountCouponsDTOList = new List<DiscountCouponsDTO>();
                int count = discountCouponsHeaderDTO.Count == null ? 1 : Convert.ToInt32(discountCouponsHeaderDTO.Count);
                for (int i = 0; i < count * productQuantity; i++)
                {
                    DiscountCouponsDTO discountCouponsDTO = new DiscountCouponsDTO();
                    discountCouponsDTO.FromNumber = currentCouponNumber;
                    discountCouponsDTO.DiscountCouponHeaderId = discountCouponsHeaderDTO.Id;
                    discountCouponsDTO.DiscountId = discountCouponsHeaderDTO.DiscountId;
                    discountCouponsDTO.StartDate = effectiveDate;
                    discountCouponsDTO.ExpiryDate = expiryDate;
                    discountCouponsDTOList.Add(discountCouponsDTO);
                    if (DiscountCouponsHeaderDTO.Sequential)
                    {
                        currentCouponNumber = GenerateNextCouponNumber(currentCouponNumber);
                        log.Debug("currentCouponNumber :" + currentCouponNumber);
                    }
                    else
                    {
                        currentCouponNumber = GetRandomCouponNumber();
                        log.Debug("currentCouponNumber :" + currentCouponNumber);
                    }
                }
            }
            log.LogMethodExit(discountCouponsDTOList);
            return discountCouponsDTOList;
        }

        /// <summary>
        /// Generates alphanumeric coupon numbers.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string GenerateNextCouponNumber(string text)
        {
            log.LogMethodEntry(text);
            var textArr = text.ToCharArray();

            if (numericCharacters == null)
            {
                numericCharacters = new List<char>();
                for (char c = '0'; c <= '9'; c++)
                {
                    numericCharacters.Add(c);
                }
            }
            if (alphaCharacters == null)
            {
                alphaCharacters = new List<char>();
                for (char c = 'A'; c <= 'Z'; c++)
                {
                    alphaCharacters.Add(c);
                }
            }
            List<char> characters = null;
            // Loop from end to beginning
            for (int i = textArr.Length - 1; i >= 0; i--)
            {
                if (numericCharacters.IndexOf(textArr[i]) != -1)
                {
                    characters = numericCharacters;
                }
                if (alphaCharacters.IndexOf(textArr[i]) != -1)
                {
                    characters = alphaCharacters;
                }
                if (characters != null)
                {
                    if (textArr[i] == characters.Last())
                    {
                        textArr[i] = characters.First();
                    }
                    else
                    {
                        textArr[i] = characters[characters.IndexOf(textArr[i]) + 1];
                        break;
                    }
                }
            }
            log.LogMethodExit(textArr.ToString());
            return new string(textArr);
        }

        /// <summary>
        /// Generates a random 10 digit coupon number of given size.
        /// </summary>
        /// <returns></returns>
        public string GetRandomCouponNumber(int width = 10)
        {
            log.LogMethodEntry(width);
            string validCharacters = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var couponNumber = "";

            byte[] seed = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(seed);
            int seedInt = BitConverter.ToInt32(seed, 0);

            var rnd = new Random(seedInt);
            while (couponNumber.Length < width)
            {
                couponNumber += validCharacters[rnd.Next(validCharacters.Length - 1)].ToString();
            }
            log.LogMethodExit(couponNumber);
            return couponNumber;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public DiscountCouponsHeaderDTO DiscountCouponsHeaderDTO
        {
            get
            {
                return discountCouponsHeaderDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of DiscountCouponsHeader
    /// </summary>
    public class DiscountCouponsHeaderListBL
    {
        private Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<DiscountCouponsHeaderDTO> discountCouponsHeaderDTOList;

        /// <summary>
        /// Parameterized Constructor with executionContext 
        /// </summary>
        /// <param name="executionContext"></param>
        public DiscountCouponsHeaderListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.discountCouponsHeaderDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor with discountCouponsHeaderDTO and executionContext
        /// </summary>
        /// <param name="discountCouponsHeaderDTO"></param>
        /// <param name="executionContext"></param>
        public DiscountCouponsHeaderListBL(ExecutionContext executionContext, List<DiscountCouponsHeaderDTO> discountCouponsHeaderDTOList)
        {
            log.LogMethodEntry(discountCouponsHeaderDTOList, executionContext);
            this.discountCouponsHeaderDTOList = discountCouponsHeaderDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the DiscountCouponsHeader list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<DiscountCouponsHeaderDTO> GetDiscountCouponsHeaderDTOList(List<KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveRecords = false)
        {
            log.LogMethodEntry(searchParameters);

            //List<DiscountCouponsHeaderDTO> discountCouponsHeaderDTOList;
            List<DiscountCouponsDTO> discountCouponsDTOList;
            DiscountCouponsHeaderDTO discountCouponsHeadersDTO;
            DiscountCouponsHeaderDataHandler discountCouponsHeaderDataHandler = new DiscountCouponsHeaderDataHandler();
            List<DiscountCouponsHeaderDTO> discountCouponsHeaderDTOList = discountCouponsHeaderDataHandler.GetDiscountCouponsHeaderDTOList(searchParameters);
            log.LogVariableState("DiscountCouponsHeaderDTOList" , discountCouponsHeaderDTOList);
            if (loadChildRecords)
            {
                if (discountCouponsHeaderDTOList != null && discountCouponsHeaderDTOList.Count > 0)
                {
                    discountCouponsHeadersDTO = discountCouponsHeaderDTOList[0];
                    discountCouponsHeaderDTOList = new List<DiscountCouponsHeaderDTO>();
                    discountCouponsHeaderDTOList.Add(discountCouponsHeadersDTO);

                    DiscountCouponsListBL discountCouponsListBL = new DiscountCouponsListBL(executionContext);
                    List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>> discountCouponsDTOSearchParameters;
                    foreach (DiscountCouponsHeaderDTO discountCouponsHeaderDTO in discountCouponsHeaderDTOList)
                    {
                        discountCouponsDTOSearchParameters = new List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>>();
                       // discountCouponsDTOSearchParameters.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.DISCOUNT_COUPONS_HEADER_ID, discountCouponsHeaderDTO.Id.ToString()));
                        discountCouponsDTOSearchParameters.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        if (loadActiveRecords)
                        {
                            discountCouponsDTOSearchParameters.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.IS_ACTIVE, "1"));
                        }
                        if (discountCouponsHeaderDTO.MasterEntityId != -1 && executionContext.IsCorporate)
                        {
                            discountCouponsDTOSearchParameters.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.MASTER_HEADER_ID, discountCouponsHeaderDTO.MasterEntityId.ToString()));
                        }
                        else
                        {
                            discountCouponsDTOSearchParameters.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.DISCOUNT_COUPONS_HEADER_ID, discountCouponsHeaderDTO.Id.ToString()));
                        }
                        discountCouponsDTOList = discountCouponsListBL.GetDiscountCouponsDTOList(discountCouponsDTOSearchParameters);
                        log.LogVariableState("DiscountCouponsDTOList" , discountCouponsDTOList);
                        if (discountCouponsDTOList != null)
                        {
                            discountCouponsHeaderDTO.DiscountCouponsDTOList = new List<DiscountCouponsDTO>(discountCouponsDTOList);
                            discountCouponsHeaderDTO.AcceptChanges();
                        }
                    }

                }
                else
                {
                    discountCouponsHeadersDTO = new DiscountCouponsHeaderDTO();
                    discountCouponsHeadersDTO.DiscountId = Convert.ToInt32(searchParameters.Where(m => m.Key.Equals(DiscountCouponsHeaderDTO.SearchByParameters.DISCOUNT_ID)).FirstOrDefault().Value);
                    discountCouponsHeadersDTO.EffectiveDate = DateTime.Today;
                    discountCouponsHeadersDTO.ExpiryDate = DateTime.Today.AddDays(1);
                    discountCouponsHeadersDTO.Count = 1;
                    discountCouponsHeadersDTO.Sequential = true;
                    discountCouponsHeaderDTOList = new List<DiscountCouponsHeaderDTO>();
                    discountCouponsHeaderDTOList.Add(discountCouponsHeadersDTO);
                }
            }
            //log.Debug("Ends-GetDiscountCouponsHeaderDTOList(searchParameters) method by returning the result of discountCouponsHeaderDataHandler.GetDiscountCouponsHeaderDTOList(searchParameters) call");
            log.LogMethodExit(discountCouponsHeaderDTOList);
            return discountCouponsHeaderDTOList;
        }

        /// <summary>
        /// Save and Updated the DiscountCouponsHeader List
        /// </summary>
        public void SaveDiscountCouponsHeaderList()
        {
            log.LogMethodEntry();
            try
            {

                if (discountCouponsHeaderDTOList != null)
                {
                    foreach (DiscountCouponsHeaderDTO discountCouponsHeaderDTO in discountCouponsHeaderDTOList)
                    {
                        DiscountCouponsHeaderBL discountCouponsHeaderBL = new DiscountCouponsHeaderBL(executionContext, discountCouponsHeaderDTO);
                        discountCouponsHeaderBL.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit();
        }
    }
}
