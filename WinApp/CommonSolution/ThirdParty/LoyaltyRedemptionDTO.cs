/********************************************************************************************
 * Project Name - Loyalty Redemption DTO
 * Description  - Data object of Loyalty Redemption
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        1-July-2015    Amaresh          Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.ComponentModel;

namespace Semnox.Parafait.ThirdParty
{
    /// <summary>
    /// This is the LoyaltyRedemptionDTO data object class, This acts as data holder for the LoyaltyRedemption business object
    /// </summary>
    public class LoyaltyRedemptionDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// success
        /// </summary>
        public bool success;
        /// <summary>
        /// item_status
        /// </summary>
        public bool item_status;
        /// <summary>
        /// code
        /// </summary>
        public string code;
        /// <summary>
        /// firstname
        /// </summary>
        public string firstname;
        /// <summary>
        /// lastname
        /// </summary>
        public string lastname;
        /// <summary>
        /// mobile
        /// </summary>
        public string mobile;
        /// <summary>
        /// email
        /// </summary>
        public string email;
        /// <summary>
        /// gender
        /// </summary>
        public string gender;
        /// <summary>
        /// validation_code
        /// </summary>
        public string validation_code;
        /// <summary>
        /// is_redeemable
        /// </summary>
        public bool is_redeemable;
        /// <summary>
        /// discount_type
        /// </summary>
        public string discount_type;
        /// <summary>
        /// discount_value
        /// </summary>
        public double discount_value;
        /// <summary>
        /// points_redeemed
        /// </summary>
        public string points_redeemed;
        /// <summary>
        /// redemption_time
        /// </summary>
        public string redemption_time;
        /// <summary>
        /// billNo
        /// </summary>
        public string billNo;
        /// <summary>
        /// amount
        /// </summary>
        public double amount;
        /// <summary>
        /// external_id
        /// </summary>
        public string external_id;
        /// <summary>
        /// message
        /// </summary>
        public string message;
        /// <summary>
        /// registered_on
        /// </summary>
        public string registered_on;
        /// <summary>
        /// coupons
        /// </summary>
        public int coupons; //for Coupons
        /// <summary>
        /// points
        /// </summary>
        public int points; //for Loyolity points
        /// <summary>
        /// isFraudCustomer
        /// </summary>
        public bool isFraudCustomer = false;
    }

    /// <summary>
    /// class root 
    /// </summary>
    public class root
    {
        /// <summary>
        /// coupon object
        /// </summary>
        public coupon coupon = new coupon();
        /// <summary>
        /// customer object
        /// </summary>
        public customer customer = new customer();
    }

    /// <summary>
    /// class coupon 
    /// </summary>
    public class coupon
    {
        /// <summary>
        /// code
        /// </summary>
        public string code;
        /// <summary>
        /// validation_code
        /// </summary>
        public string validation_code;
        /// <summary>
        /// customer object
        /// </summary>
        public customer customer = new customer();
        /// <summary>
        /// custom_fields object
        /// </summary>
        public CustomField custom_fields = new CustomField();
        /// <summary>
        /// transaction object
        /// </summary>
        public transaction transaction = new transaction();
    }

    /// <summary>
    /// class response 
    /// </summary>
    public class Response
    {
        /// <summary>
        /// status
        /// </summary>
        public Status status;
        /// <summary>
        /// customers
        /// </summary>
        public customers customers;
    }

    /// <summary>
    /// class Status 
    /// </summary>
    public class Status
    {
        /// <summary>
        /// success
        /// </summary>
        public bool success;
        /// <summary>
        /// code
        /// </summary>
        public string code;
        /// <summary>
        /// message
        /// </summary>
        public string message;
        /// <summary>
        /// total
        /// </summary>
        public string total;
        /// <summary>
        /// success_count
        /// </summary>
        public string success_count;
    }

    /// <summary>
    /// class customers 
    /// </summary>
    public class customers
    {
        /// <summary>
        /// customer
        /// </summary>
        public customer customer = new customer();
    }

    /// <summary>
    /// class customer 
    /// </summary>
     [Serializable]
    public class customer
    {
        /// <summary>
        /// user_id
        /// </summary>
        public string user_id;
        /// <summary>
        /// firstname
        /// </summary>
        public string firstname;
        /// <summary>
        /// lastname
        /// </summary>
        public string lastname;
        /// <summary>
        /// mobile
        /// </summary>
        public string mobile;
        /// <summary>
        /// email
        /// </summary>
        public string email;
        /// <summary>
        /// external_id
        /// </summary>
        public string external_id;
        /// <summary>
        /// gender
        /// </summary>
        public string gender;
        /// <summary>
        /// lifetime_points
        /// </summary>
        public string lifetime_points;
        /// <summary>
        /// lifetime_purchase
        /// </summary>
        public string lifetime_purchase;
        /// <summary>
        /// loyalty_points
        /// </summary>
        public string loyalty_points;
        /// <summary>
        /// current_slab
        /// </summary>
        public string current_slab;
        /// <summary>
        /// next_slab
        /// </summary>
        public string next_slab;
        /// <summary>
        /// next_slab_serial_number
        /// </summary>
        public string next_slab_serial_number;
        /// <summary>
        /// next_slab_description
        /// </summary>
        public string next_slab_description;
        /// <summary>
        /// registered_on
        /// </summary>
        public string registered_on;
        /// <summary>
        /// registered_by
        /// </summary>
        public string registered_by;
        /// <summary>
        /// updated_on
        /// </summary>
        public string updated_on;
        /// <summary>
        /// customfield
        /// </summary>
        public CustomField customfield;
        /// <summary>
        /// segments
        /// </summary>
        public Segments segments;
        /// <summary>
        /// transactions
        /// </summary>
        public transactions transactions;
        /// <summary>
        /// coupons
        /// </summary>
        public Coupons coupons = new Coupons();
        /// <summary>
        /// notes
        /// </summary>
        public Notes notes = new Notes();
        /// <summary>
        /// itemStatus
        /// </summary>
        public item_status itemStatus;
        /// <summary>
        /// sideEffects
        /// </summary>
        public SideEffects sideEffects;
        /// <summary>
        /// IsReedeemable
        /// </summary>
        public redeemable IsReedeemable = new redeemable();
    }

    /// <summary>
    /// class redeemable 
    /// </summary>
    public class redeemable
    {
        /// <summary>
        /// mobile
        /// </summary>
        public string mobile;
        /// <summary>
        /// code
        /// </summary>
        public string code;
        /// <summary>
        /// is_redeemable
        /// </summary>
        public bool is_redeemable;
        /// <summary>
        /// seriesInfo
        /// </summary>
        public series_info seriesInfo;
    }

    /// <summary>
    /// class series_info 
    /// </summary>
    public class series_info
    {
        /// <summary>
        /// discount_type
        /// </summary>
        public string discount_type;
        /// <summary>
        /// discount_value
        /// </summary>
        public string discount_value;
    }

    /// <summary>
    /// class CustomField 
    /// </summary>   
    [XmlType(TypeName = "custom_fields")]
    public class CustomField
    {
        /// <summary>
        /// field
        /// </summary>
        public List<field> field = new List<field>();
    }

    /// <summary>
    /// class Field 
    /// </summary>
    public class field
    {
        /// <summary>
        /// name
        /// </summary>
        public string name;
        /// <summary>
        /// value
        /// </summary>
        public string value;
    }

    /// <summary>
    /// class Segments 
    /// </summary>
    public class Segments
    {
        /// <summary>
        /// segment
        /// </summary>
        public Segment segment = new Segment();
    }

    /// <summary>
    /// class Segments 
    /// </summary>
    public class Segment
    {
        /// <summary>
        /// name
        /// </summary>
        public string name;
        /// <summary>
        /// type
        /// </summary>
        public string type;
        /// <summary>
        /// values
        /// </summary>
        public Values values;
    }

    /// <summary>
    /// class Values 
    /// </summary>
    public class Values
    {
        /// <summary>
        /// valueList
        /// </summary>
        public List<Value> valueList = new List<Value>();
    }

    /// <summary>
    /// class Value 
    /// </summary>
    public class Value
    {
        /// <summary>
        /// name
        /// </summary>
        public string name;
        /// <summary>
        /// description
        /// </summary>
        public string description;
    }

    /// <summary>
    /// class transactions 
    /// </summary>
    public class transactions
    {
        /// <summary>
        /// Transaction
        /// </summary>
        public List<transaction> Transaction  = new List<transaction>();
    }

    /// <summary>
    /// class transaction 
    /// </summary>
    public class transaction
    {
        /// <summary>
        /// id
        /// </summary>
        public string id;
        /// <summary>
        /// number
        /// </summary>
        public string number;
        /// <summary>
        /// type
        /// </summary>
        public string type;
        /// <summary>
        /// amount
        /// </summary>
        public string amount;
        /// <summary>
        /// created_date
        /// </summary>
        public string created_date;
        /// <summary>
        /// store
        /// </summary>
        public string store;
    }

    /// <summary>
    /// class Coupons 
    /// </summary>
    public class Coupons
    {
        /// <summary>
        /// coupon
        /// </summary>
        public Coupon coupon = new Coupon();
    }

    /// <summary>
    /// class Coupon 
    /// </summary>
    public class Coupon
    {
        /// <summary>
        /// code
        /// </summary>
        public string code;
        /// <summary>
        /// validation_code
        /// </summary>
        public string validation_code;
        /// <summary>
        /// customer
        /// </summary>
        public customer customer;
        /// <summary>
        /// custom_fields
        /// </summary>
        public CustomField custom_fields;
        /// <summary>
        /// transaction
        /// </summary>
        public transaction transaction;
        /// <summary>
        /// id
        /// </summary>
        public string id;
        /// <summary>
        /// series_id
        /// </summary>
        public string series_id;
        /// <summary>
        /// description
        /// </summary>
        public string description;
        /// <summary>
        /// valid_till
        /// </summary>
        public string valid_till;
        /// <summary>
        /// redeemed
        /// </summary>
        public bool redeemed;
    }

    /// <summary>
    /// class Notes 
    /// </summary>
    public class Notes
    {
        /// <summary>
        /// noteList
        /// </summary>
        public List<Note> noteList = new List<Note>();
    }

    /// <summary>
    /// class Note 
    /// </summary>
    public class Note
    {
        /// <summary>
        /// id
        /// </summary>
        public string id;
        /// <summary>
        /// date
        /// </summary>
        public string date;
        /// <summary>
        /// description
        /// </summary>
        public string description;
    }

    /// <summary>
    /// class ItemStatus 
    /// </summary>
    public class item_status
    {
        /// <summary>
        /// success
        /// </summary>
        public bool success;
        /// <summary>
        /// code
        /// </summary>
        public string code;
        /// <summary>
        /// message
        /// </summary>
        public string message;
    }

    /// <summary>
    /// class SideEffects 
    /// </summary>
    public class SideEffects
    {
        /// <summary>
        /// effectList
        /// </summary>
        public List<Effect> effectList = new List<Effect>();
    }

    /// <summary>
    /// class Effect 
    /// </summary>
    public class Effect
    {
        /// <summary>
        /// type
        /// </summary>
        public string type;
        /// <summary>
        /// points
        /// </summary>
        public string points;
        /// <summary>
        /// awarded_points
        /// </summary>
        public string awarded_points;
        /// <summary>
        /// total_points
        /// </summary>
        public string total_points;
        /// <summary>
        /// coupon_code
        /// </summary>
        public string coupon_code;
        /// <summary>
        /// description
        /// </summary>
        public string description;
        /// <summary>
        /// coupon_type
        /// </summary>
        public string coupon_type;
        /// <summary>
        /// discount_code
        /// </summary>
        public string discount_code;
        /// <summary>
        /// discount_value
        /// </summary>
        public string discount_value;
        /// <summary>
        /// valid_till
        /// </summary>
        public string valid_till;
        /// <summary>
        /// id
        /// </summary>
        public string id;
    }
}
