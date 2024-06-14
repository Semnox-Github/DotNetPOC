/********************************************************************************************
 * Class Name - Credit Card                                                                          
 * Description  - Handler to manage the credit card. Primarily check if the card is valid
 * and determines the type of the card. 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Dec-2015            Kiran          Created
 *2.70.2        08-Aug-2019            Deeksha        Modified to add logger methods.
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// Credit card handler, which checks the credit card number and deterimes the type.
    /// This class is no longer used as we do not handle un-encrypted credit card numbers.
    /// </summary>
    [Obsolete("Not used anymore, unencrypted credit card number cannot be passed for processing. Class is there only for backward compatibility", false)]
    public class CreditCard
    {
        private string cardNumber;
        private CreditCardType creditCardType;
        private bool isCardValid;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Enum managing the various credit card types
        /// </summary>
        public enum CreditCardType
        {
            /// <summary>
            /// Identifies AMEX card
            /// </summary>
            AMEX = 1,
            /// <summary>
            /// Identifies VISA card
            /// </summary>
            VISA = 2,
            /// <summary>
            /// Identifies MASTERCARD
            /// </summary>
            MASTERCARD = 3,
            /// <summary>
            /// Identifies DISCOVERER card
            /// </summary>
            DISCOVERER = 4,
            /// <summary>
            /// Identifies DINERS CLUB card
            /// </summary>
            DINERSCLUB = 5,
            /// <summary>
            /// Identifies CARTE BLANCH card
            /// </summary>
            CARTEBLANCH = 6,
            /// <summary>
            /// Identifies ENROUTE card
            /// </summary>
            ENROUTE = 7,
            /// <summary>
            /// Identifies JCB card
            /// </summary>
            JCB = 8,
            /// <summary>
            /// Signifies that the card could not be found, marked as UNKNOWN
            /// </summary>
            UNKNOWN = 0
        }

        /// <summary>
        /// Constructor taking in the credit card number
        /// Will check if the credit card is valid or not and determines the type of card
        /// </summary>
        /// <param name="creditCardNumber">Credit card number</param>
        public CreditCard(string creditCardNumber)
        {
            log.LogMethodEntry("creditCardNumber");
            this.cardNumber = creditCardNumber;
            CheckCC();
            log.LogMethodExit();
        }


        private void CheckCC()
        {
            log.LogMethodEntry();
            isCardValid = false;


            byte[] number = new byte[16]; // number to validate

            // Remove non-digits
            int len = 0;
            for (int i = 0; i < cardNumber.Length; i++)
            {
                if (char.IsDigit(cardNumber, i))
                {
                    if (len == 16) return;
                    number[len++] = byte.Parse(cardNumber[i].ToString());
                }
            }
            log.LogVariableState("number", number);
            // Use Luhn Algorithm to validate
            int sum = 0;
            for (int i = len - 1; i >= 0; i--)
            {
                if (i % 2 == len % 2)
                {
                    int n = number[i] * 2;
                    sum += (n / 10) + (n % 10);
                }
                else
                    sum += number[i];
            }
            log.LogVariableState("sum", sum);

            isCardValid = (bool)(sum % 10 == 0);
            if ((isCardValid == true))
            {
                switch (cardNumber.Substring(0, 2))
                {
                    case "34":
                    case "37":
                        creditCardType = CreditCardType.AMEX;
                        break;
                    case "36":
                        creditCardType = CreditCardType.DINERSCLUB;
                        break;
                    case "38":
                        creditCardType = CreditCardType.CARTEBLANCH;
                        break;
                    case "51":
                    case "52":
                    case "53":
                    case "54":
                    case "55":
                        creditCardType = CreditCardType.MASTERCARD;
                        break;
                    default:
                        switch (cardNumber.Substring(0, 4))
                        {
                            case "2014":
                            case "2149":
                                creditCardType = CreditCardType.ENROUTE;
                                break;
                            case "2131":
                            case "1800":
                                creditCardType = CreditCardType.JCB;
                                break;
                            case "6011":
                                creditCardType = CreditCardType.DISCOVERER;
                                break;
                            default:
                                switch (cardNumber.Substring(0, 3))
                                {
                                    case "300":
                                    case "301":
                                    case "302":
                                    case "303":
                                    case "304":
                                    case "305":
                                        creditCardType = CreditCardType.DINERSCLUB;
                                        break;
                                    default:
                                        switch (cardNumber.Substring(0, 1))
                                        {
                                            case "3":
                                                creditCardType = CreditCardType.JCB;
                                                break;
                                            case "4":
                                                creditCardType = CreditCardType.VISA;
                                                break;
                                        }
                                        break;
                                }
                                break;
                        }
                        break;
                }

            }
            else
            {
                creditCardType = CreditCardType.UNKNOWN;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the card type - whether it's VISA, AMEX etc
        /// </summary>
        public CreditCardType CardType { get { return creditCardType; } }
        /// <summary>
        /// Gets whether card is valid or not
        /// </summary>
        public bool CardValid { get { return isCardValid; } }
    }
}
