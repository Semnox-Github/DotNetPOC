/********************************************************************************************
 * Project Name - Product Price
 * Description  - Base class of price selection mode 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      18-Aug-2021      Lakshminarayana           Created : Price container enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.ProductPrice
{
    /// <summary>
    /// Base class of price selection mode 
    /// </summary>
    public abstract class PriceSelectionMode
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Selects the least price among the different price types
        /// </summary>
        public static PriceSelectionMode LEAST_PRICE_SELECTION_MODE = new LeastPriceSelectionMode();

        /// <summary>
        /// selects the price hierarchically;  
        /// </summary>
        public static PriceSelectionMode HIERARCHICAL_PRICE_SELECTION_MODE = new HierarchicalPriceSelectionMode(new List<string>()
        {
            PriceType.PROMOTION,
            PriceType.MEMBERSHIP,
            PriceType.TRANSACTION_PROFILE,
            PriceType.USER_ROLE,
            PriceType.BASE
        });

        /// <summary>
        /// Selects the final price
        /// </summary>
        /// <param name="priceContainerDetailDTO"></param>
        public abstract void SelectFinalPrice(PriceContainerDetailDTO priceContainerDetailDTO);

        /// <summary>
        /// Selects the final price
        /// </summary>
        /// <param name="priceContainerDTOList"></param>
        public virtual List<PriceContainerDTO> SelectFinalPrice(List<PriceContainerDTO> priceContainerDTOList)
        {
            log.LogMethodEntry(priceContainerDTOList);
            foreach (var priceContainerDTO in priceContainerDTOList)
            {
                foreach (var priceContainerDetailDTO in priceContainerDTO.PriceContainerDetailDTOList)
                {
                    SelectFinalPrice(priceContainerDetailDTO);
                }
            }
            log.LogMethodExit(priceContainerDTOList);
            return priceContainerDTOList;
        }
    }

    /// <summary>
    /// Selects the least price among the price types
    /// </summary>

    public class LeastPriceSelectionMode : PriceSelectionMode
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public LeastPriceSelectionMode()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// select the final price
        /// </summary>
        /// <param name="priceContainerDetailDTO"></param>
        public override void SelectFinalPrice(PriceContainerDetailDTO priceContainerDetailDTO)
        {
            log.LogMethodEntry(priceContainerDetailDTO);
            decimal finalPrice = priceContainerDetailDTO.BasePrice.HasValue ? priceContainerDetailDTO.BasePrice.Value : 0;
            string priceType = PriceType.BASE;
            if (priceContainerDetailDTO.MembershipPrice.HasValue && finalPrice > priceContainerDetailDTO.MembershipPrice.Value)
            {
                finalPrice = priceContainerDetailDTO.MembershipPrice.Value;
                priceType = PriceType.MEMBERSHIP;
            }
            if (priceContainerDetailDTO.UserRolePrice.HasValue && finalPrice > priceContainerDetailDTO.UserRolePrice.Value)
            {
                finalPrice = priceContainerDetailDTO.UserRolePrice.Value;
                priceType = PriceType.USER_ROLE;
            }
            if (priceContainerDetailDTO.TransactionProfilePrice.HasValue && finalPrice > priceContainerDetailDTO.TransactionProfilePrice.Value)
            {
                finalPrice = priceContainerDetailDTO.TransactionProfilePrice.Value;
                priceType = PriceType.TRANSACTION_PROFILE;
            }
            if (priceContainerDetailDTO.PromotionPrice.HasValue && finalPrice > priceContainerDetailDTO.PromotionPrice.Value)
            {
                finalPrice = priceContainerDetailDTO.PromotionPrice.Value;
                priceType = PriceType.PROMOTION;
            }
            priceContainerDetailDTO.FinalPrice = finalPrice;
            priceContainerDetailDTO.PriceType = priceType;
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// selects the price based on the hierarchy of the price types
    /// </summary>
    public class HierarchicalPriceSelectionMode : PriceSelectionMode
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly List<string> orderedPriceTypes;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public HierarchicalPriceSelectionMode(List<string> orderedPriceTypes)
        {
            log.LogMethodEntry();
            this.orderedPriceTypes = orderedPriceTypes;
            if(orderedPriceTypes == null ||
                orderedPriceTypes.Contains(PriceType.BASE) == false ||
                orderedPriceTypes.Contains(PriceType.USER_ROLE ) == false ||
                orderedPriceTypes.Contains(PriceType.MEMBERSHIP) == false ||
                orderedPriceTypes.Contains(PriceType.TRANSACTION_PROFILE) == false ||
                orderedPriceTypes.Contains(PriceType.PROMOTION) == false)
            {
                throw new ArgumentException("orderedPriceTypes");
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// select the hierarchical price
        /// </summary>
        /// <param name="priceContainerDetailDTO"></param>
        public override void SelectFinalPrice(PriceContainerDetailDTO priceContainerDetailDTO)
        {
            log.LogMethodEntry(priceContainerDetailDTO);
            foreach (var priceType in orderedPriceTypes)
            {
                if(HasPriceOfType(priceType, priceContainerDetailDTO))
                {
                    priceContainerDetailDTO.FinalPrice = GetPriceOfType(priceType, priceContainerDetailDTO);
                    priceContainerDetailDTO.PriceType = priceType;
                    break;
                }
            }
            log.LogMethodExit();
        }

        private decimal? GetPriceOfType(string priceType, PriceContainerDetailDTO priceContainerDetailDTO)
        {
            log.LogMethodEntry(priceType, priceContainerDetailDTO);
            decimal? result;
            switch (priceType)
            {
                case PriceType.MEMBERSHIP:
                    {
                        result = priceContainerDetailDTO.MembershipPrice.Value;
                        break;
                    }
                case PriceType.USER_ROLE:
                    {
                        result = priceContainerDetailDTO.UserRolePrice.Value;
                        break;
                    }
                case PriceType.TRANSACTION_PROFILE:
                    {
                        result = priceContainerDetailDTO.TransactionProfilePrice.Value;
                        break;
                    }
                case PriceType.PROMOTION:
                    {
                        result = priceContainerDetailDTO.PromotionPrice.Value;
                        break;
                    }
                case PriceType.BASE:
                    {
                        result = priceContainerDetailDTO.BasePrice.Value;
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("Invalid price Type", "priceType");
                    }
            }
            log.LogMethodExit(result);
            return result;
        }

        private bool HasPriceOfType(string priceType, PriceContainerDetailDTO priceContainerDetailDTO)
        {
            log.LogMethodEntry(priceType, priceContainerDetailDTO);
            bool result = false;
            switch (priceType)
            {
                case PriceType.MEMBERSHIP:
                    {
                        result = priceContainerDetailDTO.MembershipPrice.HasValue;
                        break;
                    }
                case PriceType.USER_ROLE:
                    {
                        result = priceContainerDetailDTO.UserRolePrice.HasValue;
                        break;
                    }
                case PriceType.TRANSACTION_PROFILE:
                    {
                        result = priceContainerDetailDTO.TransactionProfilePrice.HasValue;
                        break;
                    }
                case PriceType.PROMOTION:
                    {
                        result = priceContainerDetailDTO.PromotionPrice.HasValue;
                        break;
                    }
                case PriceType.BASE:
                    {
                        result = priceContainerDetailDTO.BasePrice.HasValue;
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("Invalid price Type", "priceType");
                    }
            }
            log.LogMethodExit(result);
            return result;
        }
    }

}
