/********************************************************************************************
 * Project Name - Customer
 * Description  - MembershipContainerDTOCollection Data object of RedemptionCurrencyContainerDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *0.0         05-Dec-2020   Vikas Dwivedi           Created
 ********************************************************************************************/
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer.Membership
{
    public class MembershipContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<MembershipContainerDTO> membershipContainerDTOList;
        private string hash;

        public MembershipContainerDTOCollection()
        {
            log.LogMethodEntry();
            membershipContainerDTOList = new List<MembershipContainerDTO>();
            log.LogMethodExit();
        }

        public MembershipContainerDTOCollection(List<MembershipContainerDTO> membershipContainerDTOList)
        {
            log.LogMethodEntry(membershipContainerDTOList);
            this.membershipContainerDTOList = membershipContainerDTOList;
            if (membershipContainerDTOList == null)
            {
                membershipContainerDTOList = new List<MembershipContainerDTO>();
            }
            hash = new DtoListHash(membershipContainerDTOList);
            log.LogMethodExit();
        }

        public List<MembershipContainerDTO> MembershipContainerDTOList
        {
            get
            {
                return membershipContainerDTOList;
            }

            set
            {
                membershipContainerDTOList = value;
            }
        }

        public string Hash
        {
            get
            {
                return hash;
            }
            set
            {
                hash = value;
            }
        }
    }
}
