/********************************************************************************************
 * Project Name - ProductCreditPlus DTO  
 * Description  - Data object of ProductCreditPlusContainerDTO 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By                 Remarks          
 *********************************************************************************************
 *2.150.0     07-Mar-2022   Prajwal S                  Created
 ********************************************************************************************/

using Semnox.Core.GenericUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    public class ProductCreditPlusContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int productCreditPlusId;
        private decimal creditPlus;
        private bool refundable;
        private string remarks;
        private int product_id;
        private string creditPlusType;
        private string guid;
        private DateTime periodFrom;
        private DateTime periodTo;
        private int? validForDays;
        private bool extendOnReload;
        private decimal? timeFrom;
        private decimal? timeTo;
        private int? minutes;
        private bool monday;
        private bool tuesday;
        private bool wednesday;
        private bool thursday;
        private bool friday;
        private bool saturday;
        private bool sunday;
        private bool ticketAllowed;
        private string frequency;
        private bool pauseAllowed;
        private List<CreditPlusConsumptionRulesContainerDTO> creditPlusConsumptionRulesContainerDTOList;
        private List<EntityOverrideDateContainerDTO> entityOverrideDateContainerDTOList;
        private int effectiveAfterMinutes;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductCreditPlusContainerDTO()
        {
            log.LogMethodEntry();
            creditPlusConsumptionRulesContainerDTOList = new List<CreditPlusConsumptionRulesContainerDTO>();
            entityOverrideDateContainerDTOList = new List<EntityOverrideDateContainerDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor required data fields
        /// </summary>
        public ProductCreditPlusContainerDTO(int productCreditPlusId, decimal creditPlus, bool refundable, string remarks, int product_id,
                                    string creditPlusType, DateTime periodFrom,
                                    DateTime periodTo, int? validForDays, bool extendOnReload, decimal? timeFrom, decimal? timeTo, int? minutes,
                                    bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday, bool sunday,
                                    bool ticketAllowed, string frequency, bool pauseAllowed, int effectiveAfterMinutes = 0)
            : this()
        {
            log.LogMethodEntry(productCreditPlusId, creditPlus, refundable, remarks, product_id, creditPlusType, periodFrom,
                               periodTo, validForDays, extendOnReload, timeFrom, timeTo, minutes, monday, tuesday, wednesday, thursday, friday, saturday, sunday,
                               ticketAllowed, frequency, pauseAllowed, effectiveAfterMinutes);

            this.productCreditPlusId = productCreditPlusId;
            this.creditPlus = creditPlus;
            this.refundable = refundable;
            this.remarks = remarks;
            this.product_id = product_id;
            this.creditPlusType = creditPlusType;
            this.periodFrom = periodFrom;
            this.periodTo = periodTo;
            this.validForDays = validForDays;
            this.extendOnReload = extendOnReload;
            this.timeFrom = timeFrom;
            this.timeTo = timeTo;
            this.minutes = minutes;
            this.monday = monday;
            this.tuesday = tuesday;
            this.wednesday = wednesday;
            this.thursday = thursday;
            this.friday = friday;
            this.saturday = saturday;
            this.sunday = sunday;
            this.ticketAllowed = ticketAllowed;
            this.frequency = frequency;
            this.pauseAllowed = pauseAllowed;
            creditPlusConsumptionRulesContainerDTOList = new List<CreditPlusConsumptionRulesContainerDTO>();
            this.effectiveAfterMinutes = effectiveAfterMinutes;
            log.LogMethodExit();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public ProductCreditPlusContainerDTO(ProductCreditPlusContainerDTO productCreditPlusContainerDTO)
        : this(productCreditPlusContainerDTO.productCreditPlusId, productCreditPlusContainerDTO.creditPlus, productCreditPlusContainerDTO.refundable,
             productCreditPlusContainerDTO.remarks, productCreditPlusContainerDTO.product_id, productCreditPlusContainerDTO.creditPlusType,
             productCreditPlusContainerDTO.periodFrom, productCreditPlusContainerDTO.periodTo, productCreditPlusContainerDTO.validForDays,
             productCreditPlusContainerDTO.extendOnReload, productCreditPlusContainerDTO.timeFrom,
             productCreditPlusContainerDTO.timeTo, productCreditPlusContainerDTO.minutes, productCreditPlusContainerDTO.monday,
             productCreditPlusContainerDTO.tuesday, productCreditPlusContainerDTO.wednesday,
             productCreditPlusContainerDTO.thursday, productCreditPlusContainerDTO.friday,
             productCreditPlusContainerDTO.saturday, productCreditPlusContainerDTO.sunday,
             productCreditPlusContainerDTO.ticketAllowed, productCreditPlusContainerDTO.frequency,
             productCreditPlusContainerDTO.pauseAllowed, productCreditPlusContainerDTO.effectiveAfterMinutes)
        {
            log.LogMethodEntry(productCreditPlusContainerDTO);
            if (productCreditPlusContainerDTO.creditPlusConsumptionRulesContainerDTOList != null)
            {
                creditPlusConsumptionRulesContainerDTOList = new List<CreditPlusConsumptionRulesContainerDTO>();
                foreach (var creditPlusConsumptionRulesContainerDTO in productCreditPlusContainerDTO.creditPlusConsumptionRulesContainerDTOList)
                {
                    CreditPlusConsumptionRulesContainerDTO copy = new CreditPlusConsumptionRulesContainerDTO(creditPlusConsumptionRulesContainerDTO);
                    creditPlusConsumptionRulesContainerDTOList.Add(copy);
                }
            }
            if (productCreditPlusContainerDTO.entityOverrideDateContainerDTOList != null)
            {
                entityOverrideDateContainerDTOList = new List<EntityOverrideDateContainerDTO>();
                foreach (var entityOverrideDateContainerDTO in productCreditPlusContainerDTO.entityOverrideDateContainerDTOList)
                {
                    EntityOverrideDateContainerDTO copy = new EntityOverrideDateContainerDTO(entityOverrideDateContainerDTO);
                    entityOverrideDateContainerDTOList.Add(copy);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set for ProductCreditPlusId
        /// </summary>
        public int ProductCreditPlusId { get { return productCreditPlusId; } set { productCreditPlusId = value; } }

        /// <summary>
        /// Get/Set for CreditPlus
        /// </summary>
        public decimal CreditPlus { get { return creditPlus; } set { creditPlus = value; } }

        /// <summary>
        /// Get/Set for Refundable
        /// </summary>
        public bool Refundable { get { return refundable; } set { refundable = value; } }

        /// <summary>
        /// Get/Set for Remarks
        /// </summary>
        public string Remarks { get { return remarks; } set { remarks = value; } }


        /// <summary>
        /// Get/Set for ProductId
        /// </summary>
        public int Product_id { get { return product_id; } set { product_id = value; } }

        /// <summary>
        /// Get/Set for CreditPlusType
        /// </summary>
        public string CreditPlusType { get { return creditPlusType; } set { creditPlusType = value; } }

        /// <summary>
        /// Get/Set for Guid
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set for PeriodFrom
        /// </summary>
        public DateTime PeriodFrom { get { return periodFrom; } set { periodFrom = value; } }

        /// <summary>
        /// Get/Set for PeriodTo
        /// </summary>
        public DateTime PeriodTo { get { return periodTo; } set { periodTo = value; } }

        /// <summary>
        /// Get/Set for ValidForDays
        /// </summary>
        public int? ValidForDays { get { return validForDays; } set { validForDays = value; } }

        /// <summary>
        /// Get/Set for ExtendOnReload
        /// </summary>
        public bool ExtendOnReload { get { return extendOnReload; } set { extendOnReload = value; } }

        /// <summary>
        /// Get/Set for TimeFrom
        /// </summary>
        public decimal? TimeFrom { get { return timeFrom; } set { timeFrom = value; } }

        /// <summary>
        /// Get/Set for TimeToss
        /// </summary>
        public decimal? TimeTo { get { return timeTo; } set { timeTo = value; } }

        /// <summary>
        /// Get/Set for Minutes
        /// </summary>
        public int? Minutes { get { return minutes; } set { minutes = value; } }

        /// <summary>
        /// Get/Set for Monday
        /// </summary>
        public bool Monday { get { return monday; } set { monday = value; } }

        /// <summary>
        /// Get/Set for Tuesday
        /// </summary>
        public bool Tuesday { get { return tuesday; } set { tuesday = value; } }

        /// <summary>
        /// Get/Set for Wednesday
        /// </summary>
        public bool Wednesday { get { return wednesday; } set { wednesday = value; } }

        /// <summary>
        /// Get/Set for Thursday
        /// </summary>
        public bool Thursday { get { return thursday; } set { thursday = value; } }

        /// <summary>
        /// Get/Set for Friday
        /// </summary>
        public bool Friday { get { return friday; } set { friday = value; } }

        /// <summary>
        /// Get/Set for Saturday
        /// </summary>
        public bool Saturday { get { return saturday; } set { saturday = value; } }

        /// <summary>
        /// Get/Set for Sunday
        /// </summary>
        public bool Sunday { get { return sunday; } set { sunday = value; } }

        /// <summary>
        /// Get/Set for TicketAllowed
        /// </summary>
        public bool TicketAllowed { get { return ticketAllowed; } set { ticketAllowed = value; } }

        /// <summary>
        /// Get/Set for Frequency
        /// </summary>
        public string Frequency { get { return frequency; } set { frequency = value; } }

        /// <summary>
        /// Get/Set for PauseAllowed
        /// </summary>
        public bool PauseAllowed { get { return pauseAllowed; } set { pauseAllowed = value; } }

        /// <summary>
        /// Get/Set for CreditPlusConsumptionRulesList Field
        /// </summary>
        public List<CreditPlusConsumptionRulesContainerDTO> CreditPlusConsumptionRulesContainerDTOList { get { return creditPlusConsumptionRulesContainerDTOList; } set { creditPlusConsumptionRulesContainerDTOList = value; } }

        /// <summary>
        /// Get/Set for EntityOverrideDateContainerDTOList Field
        /// </summary>
        public List<EntityOverrideDateContainerDTO> EntityOverrideDateContainerDTOList { get { return entityOverrideDateContainerDTOList; } set { entityOverrideDateContainerDTOList = value; } }

        /// <summary> 
        /// Get/Set for ProductCreditPlusId
        /// </summary>
        public int EffectiveAfterMinutes { get { return effectiveAfterMinutes; } set { effectiveAfterMinutes = value; } }

    }
}
