/********************************************************************************************
 * Project Name - DiscountedLineCollection
 * Description  - DiscountedLineCollection class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************* 
 *2.150.0    29-Apr-2021    Abhishek        Modified : POS UI Redesign
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Discounts;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Holds a collection of discounted transactionLineBLs
    /// </summary>
    public class DiscountedLineCollection
    {
        private Semnox.Parafait.logging.Logger log;
        private readonly Dictionary<int, List<Transaction.TransactionLine>> criteriaIdTransactionLinesListDictionary;
        private readonly Dictionary<int, List<Transaction.TransactionLine>> discountedProductIDTransactionLinesListDictionary;
        private readonly List<Transaction.TransactionLine> discountedLines;
        private readonly DiscountContainerDTO discountContainerDTO;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="discountContainerDTO"></param>
        public DiscountedLineCollection(ExecutionContext executionContext, DiscountContainerDTO discountContainerDTO)
        {
            log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.LogMethodEntry(executionContext, discountContainerDTO);
            this.executionContext = executionContext;
            this.discountContainerDTO = discountContainerDTO;
            criteriaIdTransactionLinesListDictionary = new Dictionary<int, List<Transaction.TransactionLine>>();
            foreach (var discountPurchaseCriteriaContainerDTO in discountContainerDTO.DiscountPurchaseCriteriaContainerDTOList)
            {
                criteriaIdTransactionLinesListDictionary.Add(discountPurchaseCriteriaContainerDTO.CriteriaId, new List<Transaction.TransactionLine>());
            }
            discountedProductIDTransactionLinesListDictionary = new Dictionary<int, List<Transaction.TransactionLine>>();
            foreach (var discountedProductsContainerDTO in discountContainerDTO.DiscountedProductsContainerDTOList)
            {
                discountedProductIDTransactionLinesListDictionary.Add(discountedProductsContainerDTO.Id, new List<Transaction.TransactionLine>());
            }
            
            discountedLines = new List<Transaction.TransactionLine>();
            log.LogMethodExit();
        }

        private Transaction.TransactionLine AddToCriteriaLines(Transaction.TransactionLine transactionLineBL)
        {
            log.LogMethodEntry(transactionLineBL);
            Transaction.TransactionLine replacementLine = null;

            foreach (var discountPurchaseCriteriaContainerDTO in discountContainerDTO.DiscountPurchaseCriteriaContainerDTOList)
            {
                if(DiscountContainerList.IsSimpleCriteria(executionContext, discountPurchaseCriteriaContainerDTO.DiscountId, discountPurchaseCriteriaContainerDTO.CriteriaId) == false ||
                   DiscountContainerList.IsCriteriaProduct(executionContext, discountPurchaseCriteriaContainerDTO.DiscountId, discountPurchaseCriteriaContainerDTO.CriteriaId, transactionLineBL.ProductID) == false)
                {
                    continue;
                }
                int minQuantity = discountPurchaseCriteriaContainerDTO.MinQuantity.HasValue == false || discountPurchaseCriteriaContainerDTO.MinQuantity.Value == 0? 1 : discountPurchaseCriteriaContainerDTO.MinQuantity.Value;
                if (criteriaIdTransactionLinesListDictionary[discountPurchaseCriteriaContainerDTO.CriteriaId].Count < minQuantity)
                {
                    criteriaIdTransactionLinesListDictionary[discountPurchaseCriteriaContainerDTO.CriteriaId].Add(transactionLineBL);
                    //log.Info("Added to criteria transactionLineBLs productId " + productId);
                    log.LogMethodExit(null, "product criteria transactionLineBL");
                    return null;
                }
            }

            foreach (var discountPurchaseCriteriaContainerDTO in discountContainerDTO.DiscountPurchaseCriteriaContainerDTOList)
            {
                if (DiscountContainerList.IsSimpleCriteria(executionContext, discountPurchaseCriteriaContainerDTO.DiscountId, discountPurchaseCriteriaContainerDTO.CriteriaId) ||
                   DiscountContainerList.IsCriteriaProduct(executionContext, discountPurchaseCriteriaContainerDTO.DiscountId, discountPurchaseCriteriaContainerDTO.CriteriaId, transactionLineBL.ProductID) == false)
                {
                    continue;
                }
                int minQuantity = discountPurchaseCriteriaContainerDTO.MinQuantity.HasValue == false || discountPurchaseCriteriaContainerDTO.MinQuantity.Value == 0 ? 1 : discountPurchaseCriteriaContainerDTO.MinQuantity.Value;
                
                if (IsCriteriaLinesDiscounted() == false)
                {
                    foreach (var categoryLine in criteriaIdTransactionLinesListDictionary[discountPurchaseCriteriaContainerDTO.CriteriaId]
                        .OrderBy(x => x.LineAmount))
                    {
                        if (transactionLineBL.LineAmount > categoryLine.LineAmount)
                        {
                            if (DiscountContainerList.IsDiscounted(executionContext, discountContainerDTO.DiscountId, categoryLine.ProductID))
                            {
                                replacementLine = categoryLine;
                                break;
                            }
                        }
                        else if ((decimal)transactionLineBL.LineAmount == (decimal)categoryLine.LineAmount)
                        {
                            if (transactionLineBL.ProductID != categoryLine.ProductID &&
                                DiscountContainerList.IsDiscounted(executionContext, discountContainerDTO.DiscountId, categoryLine.ProductID) &&
                                DiscountContainerList.IsDiscounted(executionContext, discountContainerDTO.DiscountId, transactionLineBL.ProductID) == false)
                            {
                                replacementLine = categoryLine;
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (replacementLine != null)
                    {
                        criteriaIdTransactionLinesListDictionary[discountPurchaseCriteriaContainerDTO.CriteriaId].Remove(replacementLine);
                        criteriaIdTransactionLinesListDictionary[discountPurchaseCriteriaContainerDTO.CriteriaId].Add(transactionLineBL);
                        //log.Info("Replacing criteria transactionLineBL of productId : " + replacementLine.ProductID + " amount " + replacementLine.Amount + " with transactionLineBL of productId " + transactionLineBL.ProductID + " amount" + transactionLineBL.Amount);
                        log.LogMethodExit(replacementLine, "Found a replacement transactionLineBL for category criteria");
                        return replacementLine;
                    }
                }

                if (criteriaIdTransactionLinesListDictionary[discountPurchaseCriteriaContainerDTO.CriteriaId].Count < minQuantity)
                {
                    criteriaIdTransactionLinesListDictionary[discountPurchaseCriteriaContainerDTO.CriteriaId].Add(transactionLineBL);
                    //log.Info("Added to criteria transactionLineBLs productId " + productId);
                    log.LogMethodExit(null, "product criteria transactionLineBL");
                    return null;
                }
            }

            log.LogMethodExit(transactionLineBL, "Not added to the criteria transactionLineBLs");
            return transactionLineBL;
        }

        private Transaction.TransactionLine AddToDiscountedLines(Transaction.TransactionLine transactionLineBL)
        {
            log.LogMethodEntry(transactionLineBL);
            Transaction.TransactionLine replacementLine = null;
            if(discountContainerDTO.AllProductsAreDiscounted)
            {
                discountedLines.Add(transactionLineBL);
                //log.Info("Added to discounted transactionLineBLs categoryId " + categoryId);
                log.LogMethodExit(null, "discounted category transactionLineBL");
                return null;
            }
            else
            {
                foreach (var discountedProductsContainerDTO in discountContainerDTO.DiscountedProductsContainerDTOList)
                {
                    if (DiscountContainerList.IsSimpleDiscountedProduct(executionContext, discountedProductsContainerDTO.DiscountId, discountedProductsContainerDTO.Id) == false ||
                        DiscountContainerList.IsDiscountedProduct(executionContext, discountedProductsContainerDTO.DiscountId, discountedProductsContainerDTO.Id, transactionLineBL.ProductID) == false)
                    {
                        continue;
                    }
                    int quantity = discountedProductsContainerDTO.Quantity.HasValue == false ? 0 : discountedProductsContainerDTO.Quantity.Value;
                    if (quantity == 0 || discountedProductIDTransactionLinesListDictionary[discountedProductsContainerDTO.Id].Count < quantity)
                    {
                        discountedProductIDTransactionLinesListDictionary[discountedProductsContainerDTO.Id].Add(transactionLineBL);
                        log.LogMethodExit(null, "product discounted transactionLineBL");
                        return null;
                    }
                }

                foreach (var discountedProductsContainerDTO in discountContainerDTO.DiscountedProductsContainerDTOList)
                {
                    if (DiscountContainerList.IsSimpleDiscountedProduct(executionContext, discountedProductsContainerDTO.DiscountId, discountedProductsContainerDTO.Id)||
                        DiscountContainerList.IsDiscountedProduct(executionContext, discountedProductsContainerDTO.DiscountId, discountedProductsContainerDTO.Id, transactionLineBL.ProductID) == false)
                    {
                        continue;
                    }
                    int quantity = discountedProductsContainerDTO.Quantity.HasValue == false ? 0 : discountedProductsContainerDTO.Quantity.Value;

                    if (IsCriteriaLinesDiscounted() == false)
                    {
                        foreach (var discountedLine in discountedProductIDTransactionLinesListDictionary[discountedProductsContainerDTO.Id].OrderByDescending(x => x.LineAmount))
                        {
                            if (transactionLineBL.LineAmount < discountedLine.LineAmount)
                            {
                                if (DiscountContainerList.IsCriteriaProduct(executionContext, discountContainerDTO.DiscountId, discountedLine.ProductID))
                                {
                                    replacementLine = discountedLine;
                                    break;
                                }
                            }
                            else
                            {
                                if (transactionLineBL.ProductID != discountedLine.ProductID &&
                                    DiscountContainerList.IsCriteriaProduct(executionContext, discountContainerDTO.DiscountId, discountedLine.ProductID) &&
                                    DiscountContainerList.IsCriteriaProduct(executionContext, discountContainerDTO.DiscountId, transactionLineBL.ProductID) == false)
                                {
                                    replacementLine = discountedLine;
                                    break;
                                }
                            }
                        }

                        if (replacementLine != null)
                        {
                            discountedProductIDTransactionLinesListDictionary[discountedProductsContainerDTO.Id].Remove(replacementLine);
                            discountedProductIDTransactionLinesListDictionary[discountedProductsContainerDTO.Id].Add(transactionLineBL);
                            //log.Info("Replacing discounted transactionLineBL of productId : " + replacementLine.ProductID + " amount " + replacementLine.Amount + " with transactionLineBL of productId " + transactionLineBL.ProductID + " amount" + transactionLineBL.Amount);
                            log.LogMethodExit(replacementLine, "Found a replacement transactionLineBL for discounted category");
                            return replacementLine;
                        }
                    }

                    if (quantity == 0 || discountedProductIDTransactionLinesListDictionary[discountedProductsContainerDTO.Id].Count < quantity)
                    {
                        discountedProductIDTransactionLinesListDictionary[discountedProductsContainerDTO.Id].Add(transactionLineBL);
                        //log.Info("Added to discounted transactionLineBLs categoryId " + categoryId);
                        log.LogMethodExit(null, "discounted category transactionLineBL");
                        return null;
                    }
                }
            }
               
            log.LogMethodExit(transactionLineBL, "Not added to the discounted transactionLineBLs");
            return transactionLineBL;
        }

        /// <summary>
        /// Tries to add the transactionLineBL to the collection. may return a replacement transactionLineBL back
        /// </summary>
        /// <param name="transactionLineBL"></param>
        /// <returns></returns>
        public Transaction.TransactionLine Add(Transaction.TransactionLine transactionLineBL)
        {
            log.LogMethodEntry(transactionLineBL);
            Transaction.TransactionLine replacementLine = AddToCriteriaLines(transactionLineBL);

            if (IsCriteriaLinesDiscounted() && replacementLine == null)//change
            {
                AddToDiscountedLines(transactionLineBL);
            }

            if (replacementLine == transactionLineBL)
            {
                replacementLine = AddToDiscountedLines(transactionLineBL);
            }

            if (replacementLine == transactionLineBL)
            {
                //log.Info("Unable to add the transactionLineBL to the collection");
            }
            
            log.LogMethodExit(replacementLine);
            return replacementLine;
        }

        private bool IsCriteriaLinesDiscounted()
        {
            return discountContainerDTO.DiscountCriteriaLines && 
                   discountContainerDTO.DiscountPurchaseCriteriaValidityQuantityCount > 0;
        }

        /// <summary>
        /// Returns whether this Collection is complete and can be discounted
        /// </summary>
        /// <returns></returns>
        public bool IsComplete()
        {
            log.LogMethodEntry();
            bool result = false;
            foreach (var discountPurchaseCriteriaContainerDTO in discountContainerDTO.DiscountPurchaseCriteriaContainerDTOList)
            {
                int quantity = discountPurchaseCriteriaContainerDTO.MinQuantity.HasValue == false || discountPurchaseCriteriaContainerDTO.MinQuantity.Value == 0? 1 : discountPurchaseCriteriaContainerDTO.MinQuantity.Value;
                if (criteriaIdTransactionLinesListDictionary[discountPurchaseCriteriaContainerDTO.CriteriaId].Count < quantity)
                {
                    log.LogMethodExit(result, "Didn't meet the criteriaId: " + discountPurchaseCriteriaContainerDTO.CriteriaId);
                    return false;
                }
            }
            if(discountContainerDTO.AllProductsAreDiscounted)
            {
                if(discountedLines.Count >= 1)
                {
                    result = true;
                }
            }
            else
            {
                foreach (var discountedProductsContainerDTO in discountContainerDTO.DiscountedProductsContainerDTOList)
                {
                    if(discountedProductIDTransactionLinesListDictionary[discountedProductsContainerDTO.Id].Count > 0)
                    {
                        result = true;
                        break;
                    }
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Get method of discountedLines
        /// </summary>
        /// <returns></returns>
        public List<Transaction.TransactionLine> DiscountedLines
        {
            get
            {
                return discountedLines;
            }
        }


        /// <summary>
        /// Get method of productIdCriteriaLinesDictionary
        /// </summary>
        public Dictionary<int, List<Transaction.TransactionLine>> CriteriaIdTransactionLinesListDictionary
        {
            get
            {
                return criteriaIdTransactionLinesListDictionary;
            }
            
        }

        /// <summary>
        /// Get method of categoryIdCriteriaLinesDictionary
        /// </summary>
        public Dictionary<int, List<Transaction.TransactionLine>> DiscountedProductIDTransactionLinesListDictionary
        {
            get
            {
                return discountedProductIDTransactionLinesListDictionary;
            }

        }

        /// <summary>
        /// Applies the discount to the lines
        /// </summary>
        /// <param name="discountAmount"></param>
        /// <param name="remarks"></param>
        /// <param name="approvedBy"></param>
        /// <param name="discountApplicability"></param>
        public void Apply(decimal? discountAmount, string remarks, int approvedBy, DiscountApplicability discountApplicability)
        {
            log.LogMethodEntry(discountAmount, remarks, approvedBy);
            foreach (var line in criteriaIdTransactionLinesListDictionary.Values.SelectMany(x => x))
            {
                line.DiscountQualifierList.Add(discountContainerDTO.DiscountId);
            }
            if (discountAmount > 0)
            {
                if (discountContainerDTO.OverridingDiscountAmountExists)
                {
                    foreach (var discountedProductId in discountedProductIDTransactionLinesListDictionary.Keys)
                    {
                        DiscountedProductsContainerDTO discountedProductsContainerDTO = DiscountContainerList.GetDiscountedProductsContainerDTO(executionContext, discountContainerDTO.DiscountId, discountedProductId);
                        decimal productDiscountAmount = (decimal)(discountedProductsContainerDTO.DiscountAmount.HasValue == false ? 0 : discountedProductsContainerDTO.DiscountAmount.Value);
                        if (productDiscountAmount > 0)
                        {
                            ApplyDiscountAmount(discountedProductIDTransactionLinesListDictionary[discountedProductId],
                                productDiscountAmount, remarks, approvedBy, discountApplicability);
                        }
                    }
                }
                else
                {
                    if (discountContainerDTO.AllProductsAreDiscounted)
                    {
                        ApplyDiscountAmount(discountedLines, discountAmount.Value, remarks, approvedBy, discountApplicability);
                    }
                    else
                    {
                        ApplyDiscountAmount(discountedProductIDTransactionLinesListDictionary.Values.SelectMany(x => x).ToList(), discountAmount.Value, remarks, approvedBy, discountApplicability);
                    }
                        
                }
            }
            else if (discountContainerDTO.DiscountPercentage.HasValue &&
                     discountContainerDTO.DiscountPercentage.Value > 0)
            {
                if (discountContainerDTO.OverridingDiscountPercentageExists)
                {
                    foreach (var discountedProductId in discountedProductIDTransactionLinesListDictionary.Keys)
                    {
                        DiscountedProductsContainerDTO discountedProductsContainerDTO = DiscountContainerList.GetDiscountedProductsContainerDTO(executionContext, discountContainerDTO.DiscountId, discountedProductId);
                        double productDiscountPercentageValue = (discountedProductsContainerDTO.DiscountPercentage.HasValue == false ? discountContainerDTO.DiscountPercentage.Value : discountedProductsContainerDTO.DiscountPercentage.Value);
                        foreach (var line in discountedProductIDTransactionLinesListDictionary[discountedProductId])
                        {
                            ApplyDiscountPercentage(line, productDiscountPercentageValue, remarks, approvedBy, discountApplicability);
                        }
                    }
                }
                else
                {
                    if (discountContainerDTO.AllProductsAreDiscounted)
                    {
                        foreach (Transaction.TransactionLine line in discountedLines)
                        {
                            ApplyDiscountPercentage(line, discountContainerDTO.DiscountPercentage.Value, remarks,
                                approvedBy, discountApplicability);
                        }
                    }
                    else
                    {
                        foreach (Transaction.TransactionLine line in discountedProductIDTransactionLinesListDictionary.Values.SelectMany(x => x))
                        {
                            ApplyDiscountPercentage(line, discountContainerDTO.DiscountPercentage.Value, remarks,
                                approvedBy, discountApplicability);
                        }
                    }
                    
                }
            }
            else if (discountAmount == 0 &&
                     (discountContainerDTO.DiscountPercentage.HasValue == false ||
                      discountContainerDTO.DiscountPercentage.Value == 0) &&
                     (discountContainerDTO.OverridingDiscountAmountExists ||
                      discountContainerDTO.OverridingDiscountedPriceExists))
            {
                foreach (var discountedProductId in discountedProductIDTransactionLinesListDictionary.Keys)
                {
                    DiscountedProductsContainerDTO discountedProductsContainerDTO = DiscountContainerList.GetDiscountedProductsContainerDTO(executionContext, discountContainerDTO.DiscountId, discountedProductId);
                    decimal productDiscountAmountValue = (decimal)(discountedProductsContainerDTO.DiscountAmount.HasValue == false ? 0 : discountedProductsContainerDTO.DiscountAmount.Value);
                    if (productDiscountAmountValue == 0)
                    {
                        decimal discountedPrice = discountedProductsContainerDTO.DiscountedPrice.HasValue == false ? 0 : discountedProductsContainerDTO.DiscountedPrice.Value;
                        if (discountedPrice > 0)
                        {
                            foreach (var line in discountedProductIDTransactionLinesListDictionary[discountedProductId])
                            {
                                decimal discountableAmount = line.GetDiscountableAmount();
                                if (discountableAmount > discountedPrice)
                                {
                                    line.ApplyDiscountAmount(discountContainerDTO.DiscountId, discountableAmount - discountedPrice, remarks, approvedBy, discountApplicability);
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var line in discountedProductIDTransactionLinesListDictionary[discountedProductId])
                        {
                            line.ApplyDiscountAmount(discountContainerDTO.DiscountId, productDiscountAmountValue, remarks, approvedBy, discountApplicability);
                        }
                    }

                }
            }
            log.LogMethodExit();
        }

        private void ApplyDiscountPercentage(Transaction.TransactionLine line, double discountPercentage, string remarks, int approvedBy, DiscountApplicability discountApplicability)
        {
            log.LogMethodEntry(line, discountPercentage, remarks, approvedBy);
            decimal finalDiscountAmount = (decimal)line.LineAmount * (decimal)discountPercentage / (decimal)100.0;
            line.ApplyDiscountAmount(discountContainerDTO.DiscountId, finalDiscountAmount, remarks, approvedBy, discountApplicability);
            log.LogMethodExit();
        }



        private void ApplyDiscountAmount(List<Transaction.TransactionLine> lineList, decimal discountAmount, string remarks, int approvedBy, DiscountApplicability discountApplicability)
        {
            log.LogMethodEntry(lineList, discountAmount, remarks, approvedBy, discountApplicability);
            decimal remainingAmount = discountAmount;
            decimal totalDiscountedLineAmount = 0;
            bool fractionQuantityExists = false;
            foreach (var line in lineList)
            {
                totalDiscountedLineAmount += (decimal)line.LineAmount / line.quantity;
                if (Math.Abs(line.quantity % 1) != 0)
                {
                    fractionQuantityExists = true;
                }

            }

            if (totalDiscountedLineAmount <= 0)
            {
                log.LogMethodExit(null, "Unable to apply discount as totalDiscountedLineAmount is 0");
                return;
            }
            decimal discountPercentage = remainingAmount / totalDiscountedLineAmount;
            foreach (var line in lineList)
            {
                decimal lineAmount = (decimal)line.LineAmount;
                decimal roundedDiscountAmount = lineAmount * discountPercentage;
                decimal discountableAmount = line.GetDiscountableAmount();
                if (roundedDiscountAmount > discountableAmount)
                {
                    roundedDiscountAmount = discountableAmount;
                }

                if (remainingAmount > roundedDiscountAmount)
                {
                    remainingAmount -= roundedDiscountAmount;
                }
                else
                {
                    roundedDiscountAmount = remainingAmount;
                    remainingAmount = 0;
                }

                line.ApplyDiscountAmount(discountContainerDTO.DiscountId, roundedDiscountAmount, remarks, approvedBy, discountApplicability);
            }

            if ((fractionQuantityExists == false || string.Equals("Y", discountContainerDTO.CouponMandatory, StringComparison.InvariantCultureIgnoreCase) ) && remainingAmount > 0)
            {
                foreach (var line in lineList)
                {
                    if (line.CanAdjustFractionDiscountAmount(discountContainerDTO.DiscountId) == false)
                    {
                        continue;
                    }
                    decimal discountableAmount = line.GetDiscountableAmount();
                    decimal adjustableDiscountAmount = remainingAmount;
                    if (remainingAmount > discountableAmount)
                    {
                        adjustableDiscountAmount = discountableAmount;
                    }
                    line.AdjustFractionDiscountAmount(discountContainerDTO.DiscountId, adjustableDiscountAmount);
                    remainingAmount = remainingAmount - adjustableDiscountAmount;
                    if (remainingAmount == 0)
                    {
                        break;
                    }
                }
            }

            log.LogMethodExit();
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            string result = "CriteriaLines[" + string.Join(",", criteriaIdTransactionLinesListDictionary.Values.SelectMany(x => x).Select(x => x.ProductID)) + "]";
            result += Environment.NewLine;
            if(discountContainerDTO.AllProductsAreDiscounted)
            {
                result += "DiscountedLines[" + string.Join(",", discountedLines.Select(x => x.ProductID)) + "]";
            }
            else
            {
                result += "DiscountedLines[" + string.Join(",", discountedProductIDTransactionLinesListDictionary.Values.SelectMany(x => x).Select(x => x.ProductID)) + "]";
                
            }
            return result;
        }
    }
}
