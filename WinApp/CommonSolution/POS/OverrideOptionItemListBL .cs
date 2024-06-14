/********************************************************************************************
 * Project Name - OverrideOptionItemList BL
 * Description  - BL class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *2.110.0     09-Dec-2020      Dakshakh Raj     Created for Peru Invoice Enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.POS
{
    public class OverrideOptionItemListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<OverrideOptionItemDTO> overrideOptionItemDTOList;
        private const string RECEIPT_TABLE = "ReceiptPrintTemplateHeader";
        private const string RECEIPT_COLUMN = "Guid";
        private const string SEQUENCE_TABLE = "Sequences";
        private const string SEQUENCE_COLUMN = "Guid";

        /// <summary>
        /// Parameterized constructor of OverrideOptionItemListBL
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        public OverrideOptionItemListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            BuildDTOList();
            log.LogMethodExit();
        }

        /// <summary>
        /// To build receipt template and invoice sequence
        /// </summary> 
        private void BuildDTOList()
        {
            log.LogMethodEntry();
            overrideOptionItemDTOList = new List<OverrideOptionItemDTO>();  
            OverrideOptionItemDTO receiptDTO = new OverrideOptionItemDTO(POSPrinterOverrideOptionItemNames.RECEIPT_TEMPLATE, POSPrinterOverrideOptionItemCode.RECEIPT, RECEIPT_TABLE, RECEIPT_COLUMN);
            OverrideOptionItemDTO sequenceDTO = new OverrideOptionItemDTO(POSPrinterOverrideOptionItemNames.INVOICE_SEQUENCE, POSPrinterOverrideOptionItemCode.SEQUENCE, SEQUENCE_TABLE, SEQUENCE_COLUMN);
            OverrideOptionItemDTO reversalSequenceDTO = new OverrideOptionItemDTO(POSPrinterOverrideOptionItemNames.REVERSAL_SEQUENCE, POSPrinterOverrideOptionItemCode.REVERSALSEQUENCE, SEQUENCE_TABLE, SEQUENCE_COLUMN);
            overrideOptionItemDTOList.Add(receiptDTO);
            overrideOptionItemDTOList.Add(sequenceDTO);
            overrideOptionItemDTOList.Add(reversalSequenceDTO);
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the OverrideOptionItemList DTO list based on the search parameter.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>The List of OverrideOptionItemLisDTO </returns>
        public List<OverrideOptionItemDTO> GetOverrideOptionItemDTOList(List<KeyValuePair<OverrideOptionItemDTO.SearchByParameters, string>> searchParameters,
                                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<OverrideOptionItemDTO> pOSPrinterOverrideOptionsDTOList = new List<OverrideOptionItemDTO>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            { 
                foreach (KeyValuePair<OverrideOptionItemDTO.SearchByParameters, string> searchParameter in searchParameters)
                {

                    if (searchParameter.Key == OverrideOptionItemDTO.SearchByParameters.OPTION_ITEM_CODE)
                    {
                        if (searchParameter.Value == POSPrinterOverrideOptionItemCode.RECEIPT.ToString())
                        {
                            pOSPrinterOverrideOptionsDTOList = AddDTO(pOSPrinterOverrideOptionsDTOList, POSPrinterOverrideOptionItemCode.RECEIPT);
                        }
                        else if (searchParameter.Value == POSPrinterOverrideOptionItemCode.SEQUENCE.ToString())
                        {
                            pOSPrinterOverrideOptionsDTOList = AddDTO(pOSPrinterOverrideOptionsDTOList, POSPrinterOverrideOptionItemCode.SEQUENCE);
                        }
                        else if (searchParameter.Value == POSPrinterOverrideOptionItemCode.REVERSALSEQUENCE.ToString())
                        {
                            pOSPrinterOverrideOptionsDTOList = AddDTO(pOSPrinterOverrideOptionsDTOList, POSPrinterOverrideOptionItemCode.REVERSALSEQUENCE);
                        }
                    }
                    else if (searchParameter.Key == OverrideOptionItemDTO.SearchByParameters.OPTION_ITEM_NAME)
                    {
                        if (searchParameter.Value == POSPrinterOverrideOptionItemNames.RECEIPT_TEMPLATE.ToString())
                        {
                            pOSPrinterOverrideOptionsDTOList = AddDTO(pOSPrinterOverrideOptionsDTOList, POSPrinterOverrideOptionItemCode.RECEIPT);
                        }
                        else if (searchParameter.Value == POSPrinterOverrideOptionItemNames.INVOICE_SEQUENCE.ToString())
                        {
                            pOSPrinterOverrideOptionsDTOList = AddDTO(pOSPrinterOverrideOptionsDTOList, POSPrinterOverrideOptionItemCode.SEQUENCE);
                        }
                        else if (searchParameter.Value == POSPrinterOverrideOptionItemNames.REVERSAL_SEQUENCE.ToString())
                        {
                            pOSPrinterOverrideOptionsDTOList = AddDTO(pOSPrinterOverrideOptionsDTOList, POSPrinterOverrideOptionItemCode.REVERSALSEQUENCE);
                        }
                    }
                }
            }
             log.LogMethodExit(pOSPrinterOverrideOptionsDTOList);
            return pOSPrinterOverrideOptionsDTOList;
        }

        private List<OverrideOptionItemDTO> AddDTO(List<OverrideOptionItemDTO> overrideOptionItemsDTOList, POSPrinterOverrideOptionItemCode itemCode)
        {
            log.LogMethodEntry(overrideOptionItemsDTOList, itemCode);
            if (overrideOptionItemsDTOList == null)
            {
                overrideOptionItemsDTOList = new List<OverrideOptionItemDTO>();
            }
            if (overrideOptionItemsDTOList.Exists(poo => poo.OptionItemCode == itemCode) == false)
            {
                overrideOptionItemsDTOList.AddRange(overrideOptionItemDTOList.Where(oot => oot.OptionItemCode == itemCode).ToList());
            }
            log.LogMethodExit(overrideOptionItemsDTOList);
            return overrideOptionItemsDTOList;
        }
    }
}
