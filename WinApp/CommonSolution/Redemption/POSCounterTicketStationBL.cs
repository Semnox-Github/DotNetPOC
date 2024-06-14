/********************************************************************************************
 * Project Name - TicketReceipt
 * Description  - Ticket Station class representing the POS counter.
 * After redeeming the gifts, the receipt might need to be printed for the 
 * remaining tickets. To enable the POS counter to print the ticket receipt, it is setup
 * as a pseudo ticket station. 
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 *********************************************************************************************
 *2.3.0       05-Jun-2018      Archana/Guru S A     Created 
 *2.70.2        12-Jul-2019      Deeksha              Added logger methods
 *2.70.2        21-Oct-2019      Girish               Modified : As a part of ticket station enhancement.
 ********************************************************************************************/

using System;
using Semnox.Parafait.Languages;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// POSCounterTicketStationBL class
    /// </summary>
    public class POSCounterTicketStationBL : TicketStationBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities = new Utilities();
        /// <summary>
        /// POSCounterTicketStationBL constructor with one argument
        /// </summary>
        /// <param name="ticketStationDTO">Parameter of type TicketStationDTO</param>
        public POSCounterTicketStationBL(TicketStationDTO ticketStationDTO) : base(ExecutionContext.GetExecutionContext(), ticketStationDTO)
        {
            log.LogMethodEntry(ticketStationDTO);
            log.LogMethodExit();
        }

        /// <summary>
        /// Generates the barcode
        /// </summary>
        /// <param name="tickets">total tickets to be printed in barcode</param>
        /// <returns>barcode</returns>
        public override string GenerateBarCode(int tickets)
        {
            log.LogMethodEntry(tickets);
            string barCodeText = string.Empty;
            barCodeText = ticketStationDTO.TicketStationId;
            TimeSpan diff = (DateTime.Now - DateTime.Today);
            string randomNumber = utilities.GenerateRandomNumber(5, Utilities.RandomNumberType.AlphaNumeric);
            barCodeText += randomNumber + tickets.ToString().PadLeft(6, '0');//(diff.TotalMilliseconds).ToString("#0").PadLeft(8, '0').Substring(3) + tickets.ToString().PadLeft(6, '0'); 
            if (ticketStationDTO.CheckDigit)
            {
                int checkBit = GetCheckBit(randomNumber + tickets.ToString().PadLeft(6, '0'));
                barCodeText += checkBit;
            }
            log.LogMethodExit(barCodeText);
            return barCodeText;
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
                throw new Exception(MessageContainerList.GetMessage(ExecutionContext.GetExecutionContext(), 1638));
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
                throw new Exception(MessageContainerList.GetMessage(ExecutionContext.GetExecutionContext(), 1638));
            }
            log.LogMethodExit(checkBit);
            return checkBit;
        }

        /// <summary>
        /// Gets ticket length
        /// </summary>
        /// <returns>Ticket length is 6 for a receipt generated from POSCounter ticket station </returns>
        internal override int GetTicketLength()
        {
            log.LogMethodEntry();
            log.LogMethodExit(6);
            return 6;
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
                    string subStringBarCode = barCode.Remove(0, (ticketStationDTO.TicketStationId.Length));
                    if (subStringBarCode.EndsWith("X"))
                    {
                        valid = true;
                    }
                    else
                    {
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
