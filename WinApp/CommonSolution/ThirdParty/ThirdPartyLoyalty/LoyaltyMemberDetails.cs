/********************************************************************************************
* Project Name - Loyalty
* Description  - LoyaltyMemberDetails - Class to hold member details
* 
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.120.0     12-Dec-2020      Girish Kundar       Created
*********************************************************************************************/

using System;
using System.Collections.Generic;

namespace Semnox.Parafait.ThirdParty.ThirdPartyLoyalty
{
    public class LoyaltyMemberDetails
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        List<String> LoyaltyCardNumber;
        List<String> FirstName;
        List<String> LastName;
        List<String> PhoneNumber;
        public LoyaltyMemberDetails()
        {
            log.LogMethodEntry();
            LoyaltyCardNumber = new List<String>();
            FirstName = new List<String>();
            LastName = new List<String>();
            PhoneNumber = new List<String>();
            log.LogMethodExit();
        }
        public LoyaltyMemberDetails(String cardNumber, String firstName, String lastName, String phoneNumber)
            : this()
        {
            log.LogMethodEntry(cardNumber, firstName, lastName, phoneNumber);
            LoyaltyCardNumber.Add(cardNumber.ToString());
            FirstName.Add(firstName.ToString());
            LastName.Add(lastName.ToString());
            PhoneNumber.Add(phoneNumber.ToString());
            log.LogMethodExit();
        }
        public List<String> AlohaLoyaltyCardNumber
        {
            get { return LoyaltyCardNumber; }
            set { LoyaltyCardNumber = value; }
        }
        public List<String> LoyaltyFirstName
        {
            get { return FirstName; }
            set { FirstName = value; }
        }
        public List<String> LoyaltyLastName
        {
            get { return LastName; }
            set { LastName = value; }
        }
        public List<String> LoyaltyPhoneNumber
        {
            get { return PhoneNumber; }
            set { PhoneNumber = value; }
        }
    }
}
