/********************************************************************************************
* Project Name - ThirdParty
* Description  - AlohaBSPCallbackResponseDTO
* 
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
 *2.130.7    26-July-2021      Girish Kundar      Created
 *2.80.8       08-July-2022      Deeksha            Modified
*********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.AlohaBSP
{

    public class AlohaBSPCallbackResponseDTO
    {
        public CurrentOrder currentOrder { get; set; }
        public string id { get; set; }
        public UpdatedOrder updatedOrder { get; set; }
    }


    public class ExternalIds
    {
        public string lineId { get; set; }
        public string type { get; set; }
        public string typeLabel { get; set; }
        public string value { get; set; }
        public string description { get; set; }
        public string additionalProperties { get; set; }
        public string example { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ExternalIds()
        {
            this.lineId = string.Empty;
            this.type = string.Empty;
            this.typeLabel = string.Empty;
            this.value = string.Empty;
            this.description = string.Empty;
            this.additionalProperties = string.Empty;
            this.example = string.Empty;
        }
    }
    public class Customer
    {
        public string id { get; set; }
        public List<ExternalIds> externalIds { get; set; }
        public string name { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phone { get; set; }
        public string phoneExtension { get; set; }
        public string email { get; set; }
        public string fiscalId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Customer()
        {
            this.id = string.Empty;
            this.externalIds = new List<ExternalIds>();
            this.name = string.Empty;
            this.firstName = string.Empty;
            this.lastName = string.Empty;
            this.phone = string.Empty;
            this.phoneExtension = string.Empty;
            this.email = string.Empty;
            this.fiscalId = string.Empty;
        }
    }


    public class Total
    {
        public string lineId { get; set; }
        public string groupMemberId { get; set; }
        public string type { get; set; }
        public decimal? value { get; set; }

        public Total()
        {
            this.lineId = null;
            this.groupMemberId = null;
            this.type = null;
            this.value = null;
        }
    }

    public class BusinessInfo
    {
        public string name { get; set; }
        public string department { get; set; }
        public BusinessInfo()
        {
            this.name = null;
            this.department = null;
        }
    }
    public class Coordinates
    {
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }

        public Coordinates()
        {
            this.latitude = -90;
            this.longitude = -180;
        }
    }

    public class CrossStreet
    {
        public string lineId { get; set; }
        public string name { get; set; }
        public CrossStreet()
        {
            this.lineId = null;
            this.name = null;
        }
    }
    public class Address
    {
        public string type { get; set; }
        public string typeLabel { get; set; }
        public string line1 { get; set; }
        public string line2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string postalCode { get; set; }
        public Coordinates coordinates { get; set; }
        public List<CrossStreet> crossStreets { get; set; }
        public string notes { get; set; }
        public BusinessInfo businessInfo { get; set; }
        public Address()
        {
            this.type = "";
            this.typeLabel = "";
            this.line1 = null;
            this.line2 = null;
            this.city = null;
            this.state = null;
            this.country = null;
            this.postalCode = null;
            this.coordinates = new Coordinates();
            this.crossStreets = new List<CrossStreet>();
            this.notes = null;
            this.businessInfo = new BusinessInfo();
        }
    }

    public class Fulfillment
    {
        public Address address { get; set; }
        public List<LeadTime> leadTimes { get; set; }
        public string notes { get; set; }
        public string pickupDate { get; set; }
        public string pickupLocation { get; set; }
        public string fulfillmentTime { get; set; }
        public string type { get; set; }
        public string typeLabel { get; set; }
        public bool? autoRelease { get; set; }
        public bool? catering { get; set; }
        public Fulfillment()
        {
            this.address = new Address();
            this.leadTimes = new List<LeadTime>();
            this.notes = null;
            this.pickupDate = null;
            this.pickupLocation = null;
            this.fulfillmentTime = null;
            this.type = null;
            this.typeLabel = null;
            this.autoRelease = false;
            this.catering = false;
        }
    }

    public class ProductId
    {
        public string type { get; set; }
        public string value { get; set; }
        public ProductId()
        {
            this.type = null;
            this.value = null;
        }
    }

    public class Quantity
    {
        public string unitOfMeasure { get; set; }
        public string unitOfMeasureLabel { get; set; }
        public decimal? value { get; set; }
        public Quantity()
        {
            this.unitOfMeasure = null;
            this.unitOfMeasureLabel = null;
            this.value = null;
        }
    }

    public class PriceModifier
    {
        public string lineId { get; set; }
        public decimal? amount { get; set; }
        public string description { get; set; }
        public string referenceId { get; set; }
        public PriceModifier()
        {
            this.lineId = null;
            this.amount = null;
            this.description = null;
            this.referenceId = null;
        }
    }
    public class Tax
    {
        public string lineId { get; set; }
        public decimal? amount { get; set; }
        public string code { get; set; }
        public bool? isIncluded { get; set; }
        public decimal? percentage { get; set; }
        public string groupMemberId { get; set; }
        public string description { get; set; }
        public string source { get; set; }
        public bool? active { get; set; }
        public Tax()
        {
            this.lineId = null;
            this.amount = null;
            this.code = null;
            this.isIncluded = false;
            this.percentage = null;
            this.groupMemberId = null;
            this.description = null;
            this.source = null;
            this.active = false;
        }
    }
    public class Note
    {
        public string lineId { get; set; }
        public string type { get; set; }
        // public string typeLabel { get; set; }
        public string value { get; set; }
        public Note()
        {
            this.lineId = null;
            this.type = null;
            //this.typeLabel = null;
            this.value = null;
        }
    }
    public class OrderLine
    {
        public string lineId { get; set; }
        public string groupMemberId { get; set; }
        public string comments { get; set; }
        public string description { get; set; }
        public decimal? extendedAmount { get; set; }
        public string itemType { get; set; }
        public List<Note> notes { get; set; }
        public string parentLineId { get; set; }
        public List<PriceModifier> priceModifiers { get; set; }
        public ProductId productId { get; set; }
        public Quantity quantity { get; set; }
        public bool? substitutionAllowed { get; set; }
        public List<Tax> taxes { get; set; }
        public decimal? unitPrice { get; set; }
        public string scanData { get; set; }
        public string supplementalData { get; set; }
        public string modifierCode { get; set; }
        public string linkGroupCode { get; set; }
        public string lineReplaced { get; set; }
        public string fulfillmentResult { get; set; }
        public bool? overridePrice { get; set; }
    }

    public class Expiration
    {
        public int? month { get; set; }
        public int? year { get; set; }
        public Expiration()
        {
            this.month = 0;
            this.year = 0;
        }
    }

    public class Payment
    {
        public string lineId { get; set; }
        public string groupMemberId { get; set; }
        public decimal? amount { get; set; }
        public string description { get; set; }
        public decimal? gratuity { get; set; }
        public string referenceId { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string subType { get; set; }
        public string maskedPAN { get; set; }
        public string token { get; set; }
        public bool? payBalance { get; set; }
        public string accountNumber { get; set; }
        public Expiration expiration { get; set; }
        public string paymentTime { get; set; }
        public Payment()
        {
            this.lineId = null;
            this.groupMemberId = null;
            this.amount = null;
            this.description = null;
            this.gratuity = null;
            this.referenceId = null;
            this.status = null;
            this.type = null;
            this.subType = null;
            this.maskedPAN = null;
            this.token = null;
            this.payBalance = false;
            this.accountNumber = null;
            this.expiration = null;
            this.paymentTime = null;
        }
    }

    public class AdditionalReferenceIds
    {
        public string type { get; set; }
        public string atoOrderId { get; set; }
        public string alohaCheckId { get; set; }
        public string atoReferenceId { get; set; }
        public AdditionalReferenceIds()
        {
            this.type = null;
            this.atoOrderId = null;
            this.alohaCheckId = null;
            this.atoReferenceId = null;
        }
    }

    public class Lock
    {
        public string lockedBy { get; set; }
        public string lockedDate { get; set; }
    }

    public class Fee
    {
        public string lineId { get; set; }
        public string groupMemberId { get; set; }
        public string type { get; set; }
        public string typeLabel { get; set; }
        public string provider { get; set; }
        public decimal? amount { get; set; }
        public bool? @override { get; set; }
        public Fee()
        {
            this.lineId = null;
            this.groupMemberId = null;
            this.type = null;
            this.typeLabel = null;
            this.provider = null;
            this.amount = null;
            this.@override = null;
        }
    }
    public class CurrentOrder
    {
        public string comments { get; set; }
        public string channel { get; set; }
        public string currency { get; set; }
        public Customer customer { get; set; }
        public string referenceId { get; set; }
        public string owner { get; set; }
        public string source { get; set; }
        public string status { get; set; }
        public List<Total> totals { get; set; }
        public Fulfillment fulfillment { get; set; }
        public string expireAt { get; set; }
        public List<Fee> fees { get; set; }
        public List<OrderLine> orderLines { get; set; }
        public List<Payment> payments { get; set; }
        public List<Tax> taxes { get; set; }
        public List<Promotion> promotions { get; set; }
        public AdditionalReferenceIds additionalReferenceIds { get; set; }
        public bool? taxExempt { get; set; }
        public List<TotalModifier> totalModifiers { get; set; }
        public int? partySize { get; set; }
        public string dateCreated { get; set; }
        public string dateUpdated { get; set; }
        public string id { get; set; }
        public Lock @lock { get; set; }
        public string sourceOrganization { get; set; }
        public string dateAcknowledged { get; set; }
        public string enterpriseUnitId { get; set; }
        public string etag { get; set; }
    }

    public class TotalModifier
    {
        public string lineId { get; set; }
        public string groupMemberId { get; set; }
        public decimal? amount { get; set; }
        public string description { get; set; }
        public string referenceId { get; set; }
        public TotalModifier()
        {
            this.lineId = null;
            this.groupMemberId = null;
            this.amount = null;
            this.description = null;
            this.referenceId = null;
        }
    }

    public class OrderLineGroup
    {
        public string lineId { get; set; }
        public string name { get; set; }
        public List<string> orderLineIds { get; set; }
        public OrderLineGroup()
        {
            this.lineId = null;
            this.name = null;
            this.orderLineIds = new List<string>();
        }
    }
    public class Adjustment
    {
        public string level { get; set; }
        public string type { get; set; }
        public bool? applied { get; set; }
        public Adjustment()
        {
            this.level = null;
            this.type = null;
            this.applied = false;
        }
    }
    public class Promotion
    {
        public string lineId { get; set; }
        public string referenceId { get; set; }
        public string supportingData { get; set; }
        public decimal? amount { get; set; }
        public int? numGuests { get; set; }
        public List<OrderLineGroup> orderLineGroups { get; set; }
        public Adjustment adjustment { get; set; }
        public Promotion()
        {
            this.lineId = null;
            this.referenceId = null;
            this.supportingData = null;
            this.amount = null;
            this.numGuests = 0;
            this.orderLineGroups = new List<OrderLineGroup>();
            this.adjustment = new Adjustment();
        }
    }
    public class LeadTime
    {
        public string lineId { get; set; }
        public string type { get; set; }
        public int? interval { get; set; }
        public string intervalUnits { get; set; }
    }

    public class GroupMember
    {
        /// <summary>
        /// 
        /// </summary>
        public string lineId { get; set; }
        public string name { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string fiscalId { get; set; }
        public ExternalIds externalIds { get; set; }

        public GroupMember()
        {
            this.lineId = null;
            this.name = null;
            this.firstName = null;
            this.lastName = null;
            this.fiscalId = null;
            this.externalIds = new ExternalIds();
        }
    }

    public class UpdatedOrder
    {
        public string comments { get; set; }
        public string channel { get; set; }
        public string currency { get; set; }
        public Customer customer { get; set; }
        public string owner { get; set; }
        public List<GroupMember> groupMembers { get; set; }
        public string referenceId { get; set; }
        public string status { get; set; }
        public List<Total> totals { get; set; }
        public Fulfillment fulfillment { get; set; }
        public string expireAt { get; set; }
        public List<Fee> fees { get; set; }
        public List<OrderLine> orderLines { get; set; }
        public List<Payment> payments { get; set; }
        public List<Tax> taxes { get; set; }
        public List<Promotion> promotions { get; set; }
        public AdditionalReferenceIds additionalReferenceIds { get; set; }
        public bool? taxExempt { get; set; }
        public List<TotalModifier> totalModifiers { get; set; }
        public int? partySize { get; set; }
        public string dateCreated { get; set; }
        public string dateUpdated { get; set; }
        public string id { get; set; }
        public Lock @lock { get; set; }
        public string sourceOrganization { get; set; }
        public string enterpriseUnitId { get; set; }
        public string etag { get; set; }
    }

}
