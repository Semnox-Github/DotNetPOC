
/********************************************************************************************
 * Project Name - Loyalty Redemption Data Handler
 * Description  - Data handler of the  Loyalty RedemptionBL  class
 * 
 **************
 **Version Logss
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        1-July-2016   Amaresh          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Semnox.Parafait.ThirdParty
{
    /// <summary>
    /// LoyaltyRedemptionDataHandler class
    /// </summary>
   public class LoyaltyRedemptionDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        /// <summary>
        ///  Read the response
        /// </summary>
        /// <param name="xmlstring">xml file</param>
        public LoyaltyRedemptionDTO ValidateResponseMessage(string xmlstring)
        {
            log.LogMethodEntry(xmlstring);

            LoyaltyRedemptionDTO loyaltyRedemption = new LoyaltyRedemptionDTO();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlstring);
            XmlNode success = doc.SelectSingleNode("response").SelectSingleNode("status").SelectSingleNode("success");
            loyaltyRedemption.success = Convert.ToBoolean(success.InnerText);

            XmlNode code = doc.SelectSingleNode("response").SelectSingleNode("status").SelectSingleNode("code");
            loyaltyRedemption.code = code.InnerText;

            //When Failed get the message
            if (!loyaltyRedemption.success)
            {
                loyaltyRedemption = GetMessage(doc, ref loyaltyRedemption);

                log.LogMethodExit(loyaltyRedemption);
                return loyaltyRedemption;
            }

            #region Read Customer details
            if (doc.SelectSingleNode("response").SelectSingleNode("customers") != null)
            {
                XmlNode firstName = doc.SelectSingleNode("response").SelectSingleNode("customers").SelectSingleNode("customer").SelectSingleNode("firstname");
                loyaltyRedemption.firstname = firstName.InnerText;

                XmlNode lastName = doc.SelectSingleNode("response").SelectSingleNode("customers").SelectSingleNode("customer").SelectSingleNode("lastname");
                loyaltyRedemption.lastname = lastName.InnerText;

                XmlNode mobile = doc.SelectSingleNode("response").SelectSingleNode("customers").SelectSingleNode("customer").SelectSingleNode("mobile");
                loyaltyRedemption.mobile = mobile.InnerText;

                XmlNode email = doc.SelectSingleNode("response").SelectSingleNode("customers").SelectSingleNode("customer").SelectSingleNode("email");
                loyaltyRedemption.email = email.InnerText;

                //Read the points
                XmlNode Points = doc.SelectSingleNode("response").SelectSingleNode("customers").SelectSingleNode("customer").SelectSingleNode("loyalty_points");
                loyaltyRedemption.points = Points.InnerText == "" ? 0 : Convert.ToInt32(Points.InnerText);
                
                //Read the Coupon count
                if(doc.SelectSingleNode("response").SelectSingleNode("customers").SelectSingleNode("customer").SelectSingleNode("coupons")!=null)
                {
                    int count = 0;
                    XmlNodeList xnList = doc.SelectNodes("/response/customers/customer/coupons/coupon");

                    foreach (XmlNode xn in xnList) //Loop check the coupons count
                    {
                        if (xn["redeemed"] != null)
                        {
                            string isRedeemedCoupon = xn["redeemed"].InnerText;
                            if (Convert.ToBoolean(isRedeemedCoupon) == false)
                                count++;
                        }
                    }
                    loyaltyRedemption.coupons = count;
                }

                if (doc.SelectSingleNode("response").SelectSingleNode("customers").SelectSingleNode("customer").SelectSingleNode("fraud_details") != null)
                {
                    XmlNode fraudCustomer = doc.SelectSingleNode("response").SelectSingleNode("customers").SelectSingleNode("customer").SelectSingleNode("fraud_details").SelectSingleNode("status");
                    if (fraudCustomer.InnerText != null)
                    {
                        if (fraudCustomer.InnerText == "Marked" || fraudCustomer.InnerText == "Confirmed" || fraudCustomer.InnerText == "Reconfirmed")
                           loyaltyRedemption.isFraudCustomer = true;
                    }          
                }

                if (doc.SelectSingleNode("response").SelectSingleNode("customers").SelectSingleNode("customer").SelectSingleNode("gender") != null)
                {
                    XmlNode gender = doc.SelectSingleNode("response").SelectSingleNode("customers").SelectSingleNode("customer").SelectSingleNode("gender");
                    loyaltyRedemption.gender = gender.InnerText;
                }
            }

            #endregion

            #region coupon IsReedemable //For Cheking Coupon IsReedemable
            if (doc.SelectSingleNode("response").SelectSingleNode("coupons") != null)
            {
                if (doc.SelectSingleNode("response").SelectSingleNode("coupons").SelectSingleNode("redeemable") != null)
                {
                    XmlNode IsRedeemable = doc.SelectSingleNode("response").SelectSingleNode("coupons").SelectSingleNode("redeemable").SelectSingleNode("is_redeemable");
                    loyaltyRedemption.is_redeemable = Convert.ToBoolean(IsRedeemable.InnerText);

                    if (doc.SelectSingleNode("response").SelectSingleNode("coupons").SelectSingleNode("redeemable").SelectSingleNode("series_info") != null)
                    {
                        XmlNode discount_type = doc.SelectSingleNode("response").SelectSingleNode("coupons").SelectSingleNode("redeemable").SelectSingleNode("series_info").SelectSingleNode("discount_type");
                        loyaltyRedemption.discount_type = discount_type.InnerText;
                        XmlNode discount_value = doc.SelectSingleNode("response").SelectSingleNode("coupons").SelectSingleNode("redeemable").SelectSingleNode("series_info").SelectSingleNode("discount_value");
                        if (discount_value.InnerText != "")
                            loyaltyRedemption.discount_value = Convert.ToDouble(discount_value.InnerText);
                    }
                }
            }
            #endregion

            #region Points IsReedemable //For Reading points and Cheking Points IsReedemable
            if (doc.SelectSingleNode("response").SelectSingleNode("points") != null)
            {
                XmlNode points = doc.SelectSingleNode("response").SelectSingleNode("points").SelectSingleNode("redeemable").SelectSingleNode("points");
                if (points != null)
                    loyaltyRedemption.points = Convert.ToInt32(points.InnerText);

                XmlNode IsRedeemable = doc.SelectSingleNode("response").SelectSingleNode("points").SelectSingleNode("redeemable").SelectSingleNode("is_redeemable");
                loyaltyRedemption.is_redeemable = Convert.ToBoolean(IsRedeemable.InnerText);
            }
            #endregion

            //Read item status message
            loyaltyRedemption = GetMessage(doc, ref loyaltyRedemption);

            log.LogMethodExit(loyaltyRedemption);
            return loyaltyRedemption;
        }

        /// <summary>
        ///  Read the Messages
        /// </summary>
        /// <param name="doc">XmlDocument</param>
        /// <param name="LoyaltyRedemptionMessage">RedemptionDTO object Type</param>
        LoyaltyRedemptionDTO GetMessage(XmlDocument doc, ref LoyaltyRedemptionDTO LoyaltyRedemptionMessage)
        {
            log.LogMethodEntry(doc, LoyaltyRedemptionMessage);

            // Read Customer failure message
            if (doc.SelectSingleNode("response").SelectSingleNode("customers") != null)
            {
                if (doc.SelectSingleNode("response").SelectSingleNode("customers").SelectSingleNode("customer").SelectSingleNode("item_status") != null)
                {
                    XmlNode message = doc.SelectSingleNode("response").SelectSingleNode("customers").SelectSingleNode("customer").SelectSingleNode("item_status").SelectSingleNode("message");
                    LoyaltyRedemptionMessage.message = message.InnerText;

                    XmlNode itemStatus = doc.SelectSingleNode("response").SelectSingleNode("customers").SelectSingleNode("customer").SelectSingleNode("item_status").SelectSingleNode("success");
                    LoyaltyRedemptionMessage.item_status = Convert.ToBoolean(itemStatus.InnerText);   
                }
            }

            //Read points redeemable message
            if (doc.SelectSingleNode("response").SelectSingleNode("points") != null)
            {        
                XmlNode message = doc.SelectSingleNode("response").SelectSingleNode("points").SelectSingleNode("redeemable").SelectSingleNode("item_status").SelectSingleNode("message");
                LoyaltyRedemptionMessage.message = message.InnerText;            
            }
           
            //Read points redeemed message
            if (doc.SelectSingleNode("response").SelectSingleNode("responses") != null)
            {
                XmlNode message = doc.SelectSingleNode("response").SelectSingleNode("responses").SelectSingleNode("points").SelectSingleNode("item_status").SelectSingleNode("message");
                LoyaltyRedemptionMessage.message = message.InnerText;

                XmlNode itemStatus = doc.SelectSingleNode("response").SelectSingleNode("responses").SelectSingleNode("points").SelectSingleNode("item_status").SelectSingleNode("success");
                LoyaltyRedemptionMessage.item_status = Convert.ToBoolean(itemStatus.InnerText);   
            }

            //For Validation Code Message
            if (doc.SelectSingleNode("response").SelectSingleNode("validation_code") != null)
            {
                XmlNode message = doc.SelectSingleNode("response").SelectSingleNode("validation_code").SelectSingleNode("code").SelectSingleNode("item_status").SelectSingleNode("message");
                LoyaltyRedemptionMessage.message = message.InnerText;
            }

            //For Read Coupon IsReedemable Message 
            if (doc.SelectSingleNode("response").SelectSingleNode("coupons") != null)
            {
                if (doc.SelectSingleNode("response").SelectSingleNode("coupons").SelectSingleNode("redeemable") != null)
                {
                    XmlNode message = doc.SelectSingleNode("response").SelectSingleNode("coupons").SelectSingleNode("redeemable").SelectSingleNode("item_status").SelectSingleNode("message");
                    LoyaltyRedemptionMessage.message = message.InnerText;
                }

                if (doc.SelectSingleNode("response").SelectSingleNode("coupons").SelectSingleNode("coupon") != null)
                {
                    XmlNode message = doc.SelectSingleNode("response").SelectSingleNode("coupons").SelectSingleNode("coupon").SelectSingleNode("item_status").SelectSingleNode("message");
                    LoyaltyRedemptionMessage.message = message.InnerText;

                    XmlNode itemStatus = doc.SelectSingleNode("response").SelectSingleNode("coupons").SelectSingleNode("coupon").SelectSingleNode("item_status").SelectSingleNode("success");
                    LoyaltyRedemptionMessage.item_status = Convert.ToBoolean(itemStatus.InnerText);
                }
            }

            log.LogMethodExit(LoyaltyRedemptionMessage);
            return LoyaltyRedemptionMessage;
        }

        /// <summary>
        /// convert object to xml
        /// </summary>
        /// <param name="loyaltyRedemption">RedemptionDTO type object</param>
        public string ObjectToXml(ref LoyaltyRedemptionDTO loyaltyRedemption)
        {
            log.LogMethodEntry(loyaltyRedemption);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("root");
            xmlDoc.AppendChild(rootNode);
            XmlNode redeemNode = xmlDoc.CreateElement("redeem");
            rootNode.AppendChild(redeemNode);

            XmlNode pointsRedeemedXml = xmlDoc.CreateElement("points_redeemed");
            pointsRedeemedXml.InnerText = loyaltyRedemption.points_redeemed;
            redeemNode.AppendChild(pointsRedeemedXml);

            XmlNode transactionNumberXml = xmlDoc.CreateElement("transaction_number");
            transactionNumberXml.InnerText = loyaltyRedemption.billNo;
            redeemNode.AppendChild(transactionNumberXml);

            XmlNode customerXml = xmlDoc.CreateElement("customer");
            customerXml.InnerText = "";
            redeemNode.AppendChild(customerXml);

            XmlNode mobileXml = xmlDoc.CreateElement("mobile");
            mobileXml.InnerText = loyaltyRedemption.mobile;
            customerXml.AppendChild(mobileXml);

            XmlNode emailXml = xmlDoc.CreateElement("email");
            emailXml.InnerText = loyaltyRedemption.email;
            customerXml.AppendChild(emailXml);

            //XmlNode externalIdXml = xmlDoc.CreateElement("external_id");
            //externalIdXml.InnerText = "";
            //customerXml.AppendChild(externalIdXml);

            XmlNode notesXml = xmlDoc.CreateElement("notes");
            notesXml.InnerText = "";
            redeemNode.AppendChild(notesXml);

            XmlNode validationCodeXml = xmlDoc.CreateElement("validation_code");
            validationCodeXml.InnerText = loyaltyRedemption.validation_code;
            redeemNode.AppendChild(validationCodeXml);

            XmlNode redemptiontimeXml = xmlDoc.CreateElement("redemption_time");
            redemptiontimeXml.InnerText = loyaltyRedemption.redemption_time;
            redeemNode.AppendChild(redemptiontimeXml);

            string xml = xmlDoc.InnerXml;

            log.LogMethodExit(xml);
            return xml;
        }

        /// <summary>
        /// converts object to xml
        /// </summary>
        /// <param name="dataToSerialize">Object type </param>
        public string Serialize(object dataToSerialize)
        {
            log.LogMethodEntry(dataToSerialize);

            StringWriter stringwriter = new System.IO.StringWriter();

            XmlSerializer s = new XmlSerializer(dataToSerialize.GetType());
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            s.Serialize(stringwriter, dataToSerialize, ns);

            String xml = stringwriter.ToString();
            xml = xml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");

            log.LogMethodExit(xml);
            return xml;
        }

    }
}
