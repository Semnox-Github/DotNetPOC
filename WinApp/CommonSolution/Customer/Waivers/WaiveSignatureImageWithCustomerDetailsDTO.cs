/********************************************************************************************
 * Project Name - WaiveSignatureImageWithCustomerDetailsDTO 
 * Description  - DTO to hold signature image and basic customer info
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 ********************************************************************************************* 
 *2.100        12-Mov-2020      Guru S A       Created for Enabling minor signature option for waiver
 ********************************************************************************************/
using System.Drawing; 

namespace Semnox.Parafait.Customer.Waivers
{
    public class WaiveSignatureImageWithCustomerDetailsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int customerId;
        private string customerName;
        private Image signatureImage;

        private WaiveSignatureImageWithCustomerDetailsDTO()
        {
            log.LogMethodEntry();

            log.LogMethodExit();
        }
        public WaiveSignatureImageWithCustomerDetailsDTO(int customerId, string customerName, Image signatureImage): base()
        {
            log.LogMethodEntry(customerId, customerName, signatureImage);
            this.customerId = customerId;
            this.customerName = customerName;
            this.signatureImage = signatureImage;
            log.LogMethodExit();
        }
        public int CustomerId { get { return customerId; } set { customerId = value; } }
        public string CustomerName { get { return customerName; } set { customerName = value; } }
        public Image SignatureImage { get { return signatureImage; } set { signatureImage = value; } }
    }
}
