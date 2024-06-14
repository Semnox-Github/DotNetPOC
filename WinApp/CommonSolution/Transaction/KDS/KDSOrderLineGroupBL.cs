/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data object of KDSOrderEntry
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        22-Sep-2019   Lakshminarayana           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction.KDS
{
    /// <summary>
    /// Represents a KDS order line group.
    /// </summary>
    public class KDSOrderLineGroupBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<KDSOrderLineGroupBL> kdsOrderLineGroupBlList;
        private readonly KDSOrderLineGroupDTO kdsOrderLineGroupDTO;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="kdsOrderLineDTO">kds Order Line DTO</param>
        public KDSOrderLineGroupBL(ExecutionContext executionContext, KDSOrderLineDTO kdsOrderLineDTO)
        {
            log.LogMethodEntry(executionContext, kdsOrderLineDTO);
            kdsOrderLineGroupBlList = new List<KDSOrderLineGroupBL>();
            KDSOrderLineBL kdsOrderLineBL = new KDSOrderLineBL(executionContext, kdsOrderLineDTO);
            string status = kdsOrderLineBL.GetStatus();
            kdsOrderLineGroupDTO = new KDSOrderLineGroupDTO(kdsOrderLineDTO.ProductId, kdsOrderLineDTO.ProductName, kdsOrderLineDTO.ProductType, kdsOrderLineDTO.ProductDescription, kdsOrderLineDTO.Quantity,status,kdsOrderLineDTO.LineId, kdsOrderLineDTO.LineCancelledTime.HasValue, kdsOrderLineDTO.ParentLineId == -1, kdsOrderLineDTO.TransactionLineRemarks);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="transactionLineDTO">transaction line DTO</param>
        public KDSOrderLineGroupBL(ExecutionContext executionContext, TransactionLineDTO transactionLineDTO)
        {
            log.LogMethodEntry(executionContext, transactionLineDTO);
            kdsOrderLineGroupBlList = new List<KDSOrderLineGroupBL>();
            string status = "P";
            kdsOrderLineGroupDTO = new KDSOrderLineGroupDTO(transactionLineDTO.ProductId, transactionLineDTO.ProductName, transactionLineDTO.ProductTypeCode, transactionLineDTO.ProductDescription, transactionLineDTO.Quantity.Value,status,transactionLineDTO.LineId, false, transactionLineDTO.ParentLineId == -1, transactionLineDTO.Remarks);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get method of kdsOrderLineGroupDTO field
        /// </summary>
        private KDSOrderLineGroupDTO KDSOrderLineGroupDTO
        {
            get { return kdsOrderLineGroupDTO; }
        }

        /// <summary>
        /// Adds the child order line group
        /// </summary>
        /// <param name="kdsOrderLineGroupBL"></param>
        public void AddChild(KDSOrderLineGroupBL kdsOrderLineGroupBL)
        {
            log.LogMethodEntry(kdsOrderLineGroupBL);
            kdsOrderLineGroupDTO.ParentLine = true;
            kdsOrderLineGroupBlList.Add(kdsOrderLineGroupBL);
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the comma separated product id string
        /// </summary>
        /// <returns></returns>
        public string GetLineHierarchyString()
        {
            log.LogMethodEntry();
            string result = (kdsOrderLineGroupDTO.Cancelled ? "C":string.Empty) +  kdsOrderLineGroupDTO.ProductId + "," + kdsOrderLineGroupDTO.TransactionLineRemarks;
            if (kdsOrderLineGroupBlList.Any() == false)
            {
                return result;
            }

            foreach (KDSOrderLineGroupBL kdsOrderLineGroupBL in kdsOrderLineGroupBlList.OrderBy(x => x.KDSOrderLineGroupDTO.ProductId))
            {
                result += kdsOrderLineGroupBL.GetLineHierarchyString();
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Consolidates the order line group.
        /// </summary>
        public void Consolidate()
        {
            log.LogMethodEntry();
            foreach (KDSOrderLineGroupBL childKDSOrderLineGroupBL in kdsOrderLineGroupBlList)
            {
                childKDSOrderLineGroupBL.Consolidate();
            }

            List<KDSOrderLineGroupBL> mergedKDSOrderLineGroupBLList = new List<KDSOrderLineGroupBL>();
            Dictionary<string, KDSOrderLineGroupBL> lineHierarchyStringKDSOrderLineGroupBLDictionary = new Dictionary<string, KDSOrderLineGroupBL>();
            
            foreach (KDSOrderLineGroupBL childKDSOrderLineGroupBL in kdsOrderLineGroupBlList)
            {
                string lineHierarchyString = childKDSOrderLineGroupBL.GetLineHierarchyString();
                if (lineHierarchyStringKDSOrderLineGroupBLDictionary.ContainsKey(lineHierarchyString))
                {
                    lineHierarchyStringKDSOrderLineGroupBLDictionary[lineHierarchyString].Merge(childKDSOrderLineGroupBL);
                    continue;
                }
                lineHierarchyStringKDSOrderLineGroupBLDictionary.Add(lineHierarchyString, childKDSOrderLineGroupBL);
                mergedKDSOrderLineGroupBLList.Add(childKDSOrderLineGroupBL);
            }
            kdsOrderLineGroupBlList = mergedKDSOrderLineGroupBLList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Mergers one order line group with another
        /// </summary>
        /// <param name="kdsOrderLineGroupBL"></param>
        public void Merge(KDSOrderLineGroupBL kdsOrderLineGroupBL)
        {
            log.LogMethodEntry();
            kdsOrderLineGroupDTO.Quantity += kdsOrderLineGroupBL.KDSOrderLineGroupDTO.Quantity;
            kdsOrderLineGroupDTO.TransactionLineIdList.AddRange(kdsOrderLineGroupBL.KDSOrderLineGroupDTO.TransactionLineIdList);

            Dictionary<string, KDSOrderLineGroupBL> lineHierarchyStringKDSOrderLineGroupBLDictionary = new Dictionary<string, KDSOrderLineGroupBL>();
            foreach (KDSOrderLineGroupBL orderLineGroupBL in kdsOrderLineGroupBlList)
            {
                string lineHierarchyString = orderLineGroupBL.GetLineHierarchyString();
                if (lineHierarchyStringKDSOrderLineGroupBLDictionary.ContainsKey(lineHierarchyString))
                {
                    continue;
                }
                lineHierarchyStringKDSOrderLineGroupBLDictionary.Add(lineHierarchyString, orderLineGroupBL);
            }
            foreach (KDSOrderLineGroupBL childKDSOrderLineGroupBL in kdsOrderLineGroupBL.kdsOrderLineGroupBlList)
            {
                string lineHierarchyString = childKDSOrderLineGroupBL.GetLineHierarchyString();
                if (lineHierarchyStringKDSOrderLineGroupBLDictionary.ContainsKey(lineHierarchyString) == false)
                {
                    continue;
                }
                lineHierarchyStringKDSOrderLineGroupBLDictionary[lineHierarchyString].Merge(childKDSOrderLineGroupBL);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the transactionlineid represented by the heirarchy
        /// </summary>
        /// <returns></returns>
        public List<int> GetTransactionLineIdList()
        {
            log.LogMethodEntry();
            List<int> result = new List<int>();
            result.AddRange(kdsOrderLineGroupDTO.TransactionLineIdList);
            foreach (KDSOrderLineGroupBL kdsOrderLineGroupBL in kdsOrderLineGroupBlList)
            {
                result.AddRange(kdsOrderLineGroupBL.GetTransactionLineIdList());
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the order line group dto list.
        /// </summary>
        /// <returns></returns>
        public List<KDSOrderLineGroupDTO> GetKDSOrderLineGroupList()
        {
            log.LogMethodEntry();
            List<KDSOrderLineGroupDTO> result = new List<KDSOrderLineGroupDTO> {kdsOrderLineGroupDTO};
            foreach (KDSOrderLineGroupBL kdsOrderLineGroupBL in kdsOrderLineGroupBlList)
            {
                result.AddRange(kdsOrderLineGroupBL.GetKDSOrderLineGroupList());
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Sets the product name offset based on the depth specified. 
        /// </summary>
        /// <param name="depth"></param>
        public void SetProductNameOffset(int depth)
        {
            log.LogMethodEntry(depth);
            if (depth > 0)
            {
                byte[] b = { 20, 37 };
                string sOffset = Encoding.Unicode.GetString(b);
                sOffset = sOffset.PadLeft(depth * 3 + 1, ' ') + " ";
                kdsOrderLineGroupDTO.ProductNameOffset = sOffset;
            }
            
            foreach (KDSOrderLineGroupBL kdsOrderLineGroupBL in kdsOrderLineGroupBlList)
            {
                kdsOrderLineGroupBL.SetProductNameOffset(depth + 1);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the total KDSOrderLineGroupDTO count under the heirarchy
        /// </summary>
        /// <returns></returns>
        public int GetKDSOrderLineGroupDTOCount()
        {
            log.LogMethodEntry();
            int result = 1;
            foreach (KDSOrderLineGroupBL kdsOrderLineGroupBL in kdsOrderLineGroupBlList)
            {
                result += kdsOrderLineGroupBL.GetKDSOrderLineGroupDTOCount();
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
