/********************************************************************************************
* Project Name - Loyalty
* Description  - LoyaltyMember - Class to hold member details
* 
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.120.0     12-Dec-2020      Girish Kundar       Created
*********************************************************************************************/

using System;

namespace Semnox.Parafait.ThirdParty.ThirdPartyLoyalty
{

    public class LoyaltyMember
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
}
