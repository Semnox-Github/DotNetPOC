/********************************************************************************************
 * Project Name - ModTenWeightingThreeTicketStationBL
 * Description  - Business class for ModTenWeightingThreeTicketStation  
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By         Remarks          
 *********************************************************************************************
 *2.70.2        05-Oct-2019       Girish Kundar       Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;


namespace Semnox.Parafait.Redemption
{
    public class ModuleTenWtThreeTicketStationBL : TicketStationBL
    {
        /// <summary>
        /// ModTenWeightingThreeTicketStationBL constructor with one argument
        /// </summary>
        /// <param name="ticketStationDTO">Parameter of type TicketStationDTO</param>
        public ModuleTenWtThreeTicketStationBL(TicketStationDTO ticketStationDTO) : base(machineUserContext, ticketStationDTO)
        {
            log.LogMethodEntry(ticketStationDTO);
            log.LogMethodExit();
        }

        /// <summary>
        /// Matches check bit
        /// </summary>
        /// <param name="receiptNumber">Receipt Number</param>
        /// <param name="checkBit">Check bit of the receipt</param>
        public override void MatchCheckBit(string receiptNumber, int checkBit)
        {
            log.LogMethodEntry(receiptNumber, checkBit);
            if (checkBit != GetCheckBit(receiptNumber))
            {
                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1638));
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets check bit
        /// </summary>
        /// <param name="receiptNumber">Receipt number of the receipt</param>
        /// <returns>Check bit value present in passed receipt</returns>
        public override int GetCheckBit(string receiptNumber)
        {
            log.LogMethodEntry(receiptNumber);
            int checkBit = 0;
            try
            {
                int sumOddDigits = 0;
                int sumEvenDigits = 0;
                string numberValues = "";
                foreach (char c in receiptNumber)
                {
                    if (char.IsNumber(c))
                    {
                        numberValues += c;
                    }
                }
                for (int i = 0; i < numberValues.Length; i++)
                {
                    if ((i + 1) % 2 == 0)
                    {
                        sumEvenDigits += Convert.ToInt32(numberValues[i].ToString());
                    }
                    else
                    {
                        sumOddDigits += Convert.ToInt32(numberValues[i].ToString());
                    }
                }
                int digitSum = (sumOddDigits * 3) + sumEvenDigits;
                if (digitSum % 10 != 0)
                {
                    checkBit = 10 - (digitSum % 10);
                }
                else
                {
                    checkBit = digitSum % 10;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                checkBit = 0;
                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1638));
            }
            log.LogMethodExit(checkBit);
            return checkBit;
        }


        /// <summary>
        /// Validates check bit
        /// </summary>
        /// <param name="barCode">Parameter of type string</param>
        /// <returns>true or false after validating CheckBit</returns>
        public override bool ValidCheckBit(string barCode)
        {
            log.LogMethodEntry(barCode);
            bool valid = false;
            if (ticketStationDTO.CheckDigit)
            {
                string ticketStationId = barCode.Substring(0, ticketStationDTO.TicketStationId.Length);
                log.Info(ticketStationId);
                int ticketStationIdValue = -1;
                string subStringBarCode = string.Empty;
                if (string.IsNullOrWhiteSpace(ticketStationId) == false && int.TryParse(ticketStationId, out ticketStationIdValue) == true)
                {
                    subStringBarCode = barCode;// barCode.Remove(0, (ticketStationDTO.TicketStationId.Length));
                }
                else
                {
                    subStringBarCode = barCode.Remove(0, (ticketStationDTO.TicketStationId.Length));
                }
                try
                {
                    int receiptCheckBit = Convert.ToInt32(subStringBarCode[subStringBarCode.Length - 1].ToString());
                    string numbersWithoutCheckBit = subStringBarCode.Substring(0, subStringBarCode.Length - 1);
                    MatchCheckBit(numbersWithoutCheckBit, receiptCheckBit);
                    valid = true;
                }
                catch (Exception ex)
                {
                    log.Error("Error while executing ValidCheckBit()" + ex.Message);
                    valid = false;
                }
            }
            else
            {
                valid = true;
            }

            log.LogMethodExit(valid);
            return valid;
        }
    }
}
