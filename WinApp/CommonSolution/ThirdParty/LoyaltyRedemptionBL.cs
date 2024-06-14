
/********************************************************************************************
 * Project Name - RedemptionBL
 * Description  - Bussiness logic of the LoyalityRedemption class
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00        8-jun-2016   Amaresh          Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using POSCore;
//using Semnox.Core.Customer;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;

namespace Semnox.Parafait.ThirdParty
{
    /// <summary>
    /// Bussiness logic of the LoyalityRedemptionBL class
    /// </summary>
   public class LoyaltyRedemptionBL
    {
       LoyaltyRedemptionDataHandler loyaltyRedemptionHandler = new LoyaltyRedemptionDataHandler();
       HttpUtilsDataHandler httpUtilsDataHandler = new HttpUtilsDataHandler();
       LoyaltyRedemptionDTO loyaltyRedemption = new LoyaltyRedemptionDTO();

       private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

       /// <summary>
       /// Returns the LoyaltyRedemptionDTO based on the request Phone Number
       /// </summary>
       /// <param name="phoneNo">Phone Number</param>
       /// <param name="msg">Exception Message</param>
       public LoyaltyRedemptionDTO IsCustomerExist(string phoneNo, ref string msg)
       {
           log.LogMethodEntry(phoneNo, msg);

           try
           {
               string json = httpUtilsDataHandler.ValidateCustomer(phoneNo);
               LoyaltyRedemptionDTO loyaltyRedemptionDTO = loyaltyRedemptionHandler.ValidateResponseMessage(json);

               log.LogMethodExit(loyaltyRedemptionDTO);
               return loyaltyRedemptionDTO; 
           }
           catch(Exception ex)
           {
                log.Error("Error occured while Loyalty redemption");
                msg = ex.Message;
                loyaltyRedemption = null;

                log.LogMethodExit(loyaltyRedemption);
                return loyaltyRedemption;
           }
       }

       /// <summary>
       /// Add customer Details To Capillary and return LoyaltyRedemptionDTO
       /// </summary>
       /// <param name="customerDTO">Customer Object</param>
       public LoyaltyRedemptionDTO AddCustomerToCapillary(CustomerDTO customerDTO)
       {
           log.LogMethodEntry("customerDTO");
                       
           root rootXml = new root();
           rootXml.coupon = null;

           customer cutsomerDetails = new customer();
           cutsomerDetails.firstname = customerDTO.FirstName;
           cutsomerDetails.mobile = customerDTO.PhoneNumber;
           cutsomerDetails.email = customerDTO.Email;
           cutsomerDetails.gender = customerDTO.Gender.ToString();
           cutsomerDetails.lastname = customerDTO.LastName;
           rootXml.customer = cutsomerDetails;

           try
           {
               string xml = loyaltyRedemptionHandler.Serialize(rootXml);
               string jsonResult = httpUtilsDataHandler.AddCustomerDetails(xml);
               LoyaltyRedemptionDTO loyaltyRedemptionDTO = loyaltyRedemptionHandler.ValidateResponseMessage(jsonResult);

               log.LogMethodExit(loyaltyRedemptionDTO);
               return loyaltyRedemptionDTO;       
           }
           catch (Exception ex)
           {
               log.Error("Error occured while calculating loyalty redemption", ex);

               loyaltyRedemption.success = false;
               loyaltyRedemption.item_status = false;
               loyaltyRedemption.message = ex.Message;
                
               log.LogMethodExit(loyaltyRedemption);
               return loyaltyRedemption;
           }
       }

       /// <summary>
       /// UpdateCustomerDetails To Capillary and return LoyaltyRedemptionDTO
       /// </summary>
       /// <param name="customerDTO">customerDTO type object</param>
       public LoyaltyRedemptionDTO UpdateCustomerToCapillary(CustomerDTO customerDTO)
       {
           log.LogMethodEntry("customerDTO");

           root rootXml = new root();
           rootXml.coupon = null;
           customer cutsomerDetails = new customer();
           cutsomerDetails.firstname = customerDTO.FirstName;
           cutsomerDetails.mobile = customerDTO.PhoneNumber;
           cutsomerDetails.email = customerDTO.Email;
           cutsomerDetails.gender = customerDTO.Gender.ToString();
           cutsomerDetails.lastname = customerDTO.LastName;
           rootXml.customer = cutsomerDetails;

           try
           {
               string xml = loyaltyRedemptionHandler.Serialize(rootXml);
               string jsonResult = httpUtilsDataHandler.AddCustomerDetails(xml);
               LoyaltyRedemptionDTO loyaltyRedemptionDTO = loyaltyRedemptionHandler.ValidateResponseMessage(jsonResult);

               log.LogMethodExit(loyaltyRedemption);
               return loyaltyRedemptionDTO;
           }
           catch (Exception ex)
           {
                log.Error("Error occured while Updating Customer To Capillary", ex);

               loyaltyRedemption.success = false;
               loyaltyRedemption.item_status = false;
               loyaltyRedemption.message = ex.Message;

                log.LogMethodExit(loyaltyRedemption);
               return loyaltyRedemption;
           }
       }

       /// <summary>
       /// Checks wheather passed coupon number is redeemable or not and Returns the LoyaltyRedemptionDTO based on passed couponNumber and phoneNo
       /// </summary>
       /// <param name="couponNumber">Coupon Number</param>
       /// <param name="phoneNo">Phone Number</param>
       public LoyaltyRedemptionDTO IsCouponRedeemable(string couponNumber, string phoneNo)
       {
            log.LogMethodEntry(couponNumber, phoneNo);

           try
           {
               string Rjson = httpUtilsDataHandler.ValidateCouponRedeemable(couponNumber, phoneNo);
               LoyaltyRedemptionDTO loyaltyRedemptionDTO = loyaltyRedemptionHandler.ValidateResponseMessage(Rjson);

               log.LogMethodExit(loyaltyRedemptionDTO);            
               return loyaltyRedemptionDTO;
           }
           catch(Exception ex)
           {
               log.Error("Error occured while checking for redeemable coupon", ex);
               loyaltyRedemption.success = false;
               loyaltyRedemption.is_redeemable = false;
               loyaltyRedemption.message = ex.Message;

               log.LogMethodExit(loyaltyRedemption);
               return loyaltyRedemption;
           }
       }

       /// <summary>
       /// Redeem the coupon returns the bool wheather success or not
       /// </summary>
       /// <param name="couponLoyaltyRedemption">LoyaltyRedemptionDTO Type object</param>
       /// <param name="msg">message</param>
       public bool RedeemCoupon(LoyaltyRedemptionDTO couponLoyaltyRedemption, ref string msg)
       {
            log.LogMethodEntry(couponLoyaltyRedemption, msg);

           coupon coupon = new coupon();
           coupon.code = couponLoyaltyRedemption.code;

           customer customerDetails = new customer();
           customerDetails.mobile = couponLoyaltyRedemption.mobile;
           customerDetails.email = couponLoyaltyRedemption.email;
           coupon.customer = customerDetails;

           transaction trans = new transaction();
           trans.number = couponLoyaltyRedemption.billNo;
           trans.amount = couponLoyaltyRedemption.amount.ToString();

           coupon.transaction = trans;

           root rootxml = new root();
           rootxml.coupon = coupon;
           rootxml.customer = null;

           try
           {
               //convert object to xml
               string xml = loyaltyRedemptionHandler.Serialize(rootxml);
               string json = httpUtilsDataHandler.RedeemCouponInCapillary(xml);
               LoyaltyRedemptionDTO loyaltyRedemptionDTO = loyaltyRedemptionHandler.ValidateResponseMessage(json);
               msg = loyaltyRedemptionDTO.message;

               if (loyaltyRedemptionDTO.success && loyaltyRedemptionDTO.item_status)
               {
                   log.LogMethodExit(true);
                   return true;
               }
               else
               {
                   log.LogMethodExit(false);
                   return false;
               }
           }
           catch(Exception ex)
           {
               log.Error("Error occured while converting to XML", ex);
               msg = ex.Message;

               log.LogMethodExit(false);
               return false;
           }
       }
        
      /// <summary>
      ///Customer Receive the validationcode 
      /// </summary>
       /// <param name="redeemPoints">redeem Points</param>
       /// <param name="phoneNo">Phone Number</param>
       public LoyaltyRedemptionDTO GetValidationCode(string redeemPoints, string phoneNo)
       {
            log.LogMethodEntry(redeemPoints, phoneNo); 

           try
           {
               string Rjson = httpUtilsDataHandler.ReceiveValidationCode(redeemPoints, phoneNo);
               LoyaltyRedemptionDTO loyaltyRedemptionDTO = loyaltyRedemptionHandler.ValidateResponseMessage(Rjson);

               log.LogMethodExit(loyaltyRedemptionDTO);
               return loyaltyRedemptionDTO;
           }
           catch(Exception ex)
           {
               log.Error("Error occured while getting the validation code", ex);

               loyaltyRedemption.success = false;
               loyaltyRedemption.message = ex.Message;

               log.LogMethodExit(loyaltyRedemption);
               return loyaltyRedemption;
           }
       }

      /// <summary>
      /// Check the Passed Ponits and validationcode are valid for existing customer phone number
      /// </summary>
      /// <param name="points">Points to redeem </param>
      /// <param name="validationCode">Validation Code</param>
      /// <param name="phoneNo">Customer Phone Number </param>
      public LoyaltyRedemptionDTO IsPointsRedeemable(string points, string validationCode, string phoneNo)
       {
          log.LogMethodEntry(points, validationCode, phoneNo);

           try
           {
               string Rjson = httpUtilsDataHandler.ValidatePointsRedeemable(points, validationCode, phoneNo);
               LoyaltyRedemptionDTO loyaltyRedemptionDTO = loyaltyRedemptionHandler.ValidateResponseMessage(Rjson);

               log.LogMethodExit(loyaltyRedemptionDTO);
               return loyaltyRedemptionDTO;
           }
          catch(Exception ex)
           {
               log.Error("Error occured while checking if points are redeemable", ex);

               loyaltyRedemption.success = false;
               loyaltyRedemption.is_redeemable = false;
               loyaltyRedemption.message = ex.Message;

               log.LogMethodExit(loyaltyRedemption);
               return loyaltyRedemption;
           }
       }

       /// <summary>
       /// returns the bool wheather redeemtion success or not
       /// </summary>
      /// <param name="pointsLoyaltyRedemption">LoyaltyRedemptionDTO Type object</param>
      /// <param name="msg">Message</param>
      public bool RedeemPoints(LoyaltyRedemptionDTO pointsLoyaltyRedemption, ref string msg)
       {
            log.LogMethodEntry(pointsLoyaltyRedemption, msg);

           try
           {
               string xml = loyaltyRedemptionHandler.ObjectToXml(ref pointsLoyaltyRedemption);
               string json = httpUtilsDataHandler.RedeemPointsInCapillary(xml);
               LoyaltyRedemptionDTO loyaltyRedemptionDTO = loyaltyRedemptionHandler.ValidateResponseMessage(json);
               msg = loyaltyRedemptionDTO.message;

               if (loyaltyRedemptionDTO.success && loyaltyRedemptionDTO.item_status)
               {
                   log.LogMethodExit(true);
                   return true;
               }
               else
               {
                   log.LogMethodExit(false);
                   return false;
               }
           }
          catch(Exception ex)
           {
               log.Error("Error occured while Redeeming Points", ex);
               msg = ex.Message;

               log.LogMethodExit(false);
               return false;
           }
       }
    }
}
 