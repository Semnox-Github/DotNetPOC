/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - CardNumberFormatListBL class - This is business layer class for CardNumberFormatList
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Sep-2020       Girish Kundar             Created : CenterEdge  REST API
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    /// <summary>
    /// This is sealed class which holds the information of cardNumber formats which Parafait supports 
    /// </summary>
    public sealed class CardNumberFormatListBL
    {
        private CardNumberFormats cardNumberFormatDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const int cardNumberLength = 8;
        
        /// <summary>
        /// Parameterized constructor of CardNumberFormatListBL class
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public CardNumberFormatListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public CardNumberFormats GetCardNumberFormats(int skip ,int take)
        {
            log.LogMethodEntry(skip,take);
            try
            {
                cardNumberFormatDTO = new CardNumberFormats();
                string defaultValue = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CARD_NUMBER_LENGTH");
                char[] splitCharacters = new char[] {'|'};
                string[] cardNumberLengths = defaultValue.Split(splitCharacters, StringSplitOptions.RemoveEmptyEntries);
                log.Debug("cardNumberLengths :" + cardNumberLengths);
                if (cardNumberLengths != null && cardNumberLengths.Length > 0)
                {
                    foreach (string length in cardNumberLengths)
                    {
                        CardNumberFormatDTO CardNumberFormatDTO = new CardNumberFormatDTO(Convert.ToInt32(length.Trim()), Convert.ToInt32(length.Trim()), string.Empty);
                        cardNumberFormatDTO.formats.Add(CardNumberFormatDTO);
                        log.LogVariableState("CardNumberFormatDTO", CardNumberFormatDTO);
                    }
                }
                else
                {
                    CardNumberFormatDTO CardNumberFormatDTO = new CardNumberFormatDTO(cardNumberLength, cardNumberLength, string.Empty);
                    this.cardNumberFormatDTO.formats.Add(CardNumberFormatDTO);
                    log.LogVariableState("CardNumberFormatDTO", CardNumberFormatDTO);
                }
                cardNumberFormatDTO.skipped = skip;
                if(take > 0)
                {
                    take = take > cardNumberFormatDTO.formats.Count ? cardNumberFormatDTO.formats.Count : take;
                }
                else
                {
                    take = cardNumberFormatDTO.formats.Count;
                }
                if (take > cardNumberFormatDTO.formats.Count - skip)
                {
                    take = cardNumberFormatDTO.formats.Count - skip;
                }
                List<CardNumberFormatDTO> result = cardNumberFormatDTO.formats.GetRange(skip, take);
                cardNumberFormatDTO.formats.Clear();
                cardNumberFormatDTO.formats.AddRange(result);  //Add filtered list
                cardNumberFormatDTO.totalCount = cardNumberFormatDTO.formats.Count;
                log.LogVariableState("Centeredge cardNumberFormatDTO", cardNumberFormatDTO);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit(cardNumberFormatDTO);
            return cardNumberFormatDTO;
        }

    }
}
