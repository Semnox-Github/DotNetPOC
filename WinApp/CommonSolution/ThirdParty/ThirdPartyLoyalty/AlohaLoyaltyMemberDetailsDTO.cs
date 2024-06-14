using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.ThirdPartyLoyalty
{
    class AlohaLoyaltyMemberDetailsDTO
    {
        String[][] memberValues;
        string[] memberColumns;

        public String[][] values
        {
            get { return memberValues; }
            set { memberValues = value; }
        }

        public String[] columnNames
        {
            get { return memberColumns; }
            set { memberColumns = value; }
        }
    }

    public class AlohaMemberDetails
    {
        List<String> CardNumber;
        List<String> MaskedCardNumber;
        List<String> FirstName;
        List<String> LastName;
        List<String> PhoneNumber;
        public AlohaMemberDetails()
        {
            CardNumber = new List<String>();
            MaskedCardNumber = new List<String>();
            FirstName = new List<String>();
            LastName = new List<String>();
            PhoneNumber = new List<String>();
        }
        public AlohaMemberDetails(String cardNumber, String maskedCardNumber, String firstName, String lastName, String phoneNumber)
            : this()
        {
            CardNumber.Add(cardNumber);
            MaskedCardNumber.Add(cardNumber);
            FirstName.Add(firstName);
            LastName.Add(lastName);
            PhoneNumber.Add(phoneNumber);
        }
        public List<String> LoyaltyCardNumber { get { return CardNumber; } set { value = CardNumber; } }
        public List<String> LoyaltyMaskedCardNumber { get { return MaskedCardNumber; } set { value = MaskedCardNumber; } }
        public List<String> LoyaltyFirstName { get { return FirstName; } set { value = FirstName; } }
        public List<String> LoyaltyLastName { get { return LastName; } set { value = LastName; } }
        public List<String> LoyaltyPhoneNumber { get { return PhoneNumber; } set { value = PhoneNumber; } }
    }
}
