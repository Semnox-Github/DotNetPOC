/********************************************************************************************
 * Project Name - PartnerRevenueDTODefination
 * Description  - PartnerRevenueDTODefination
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.90        06-Jul-2020   Mushahid Faizan      Created 
 ********************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    public class PartnerRevenueDTODefination : ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string revenueSharePercentage;
        private string minimumGuarantee;
        private string month;
        private string totalAmount;
        private string finalAmount;
        private string partner;
        private string machineGroup;
        private string agentGroupName;
        private string shareAmount;

        public PartnerRevenueDTODefination(string month, string partner, string machineGroup, string agentGroupName, string totalAmount, string revenueSharePercentage, string shareAmount, string minimumGuarantee, string finalAmount)
        {
            log.LogMethodEntry();
            this.month = month;
            this.machineGroup = machineGroup;
            this.partner = partner;
            this.revenueSharePercentage = revenueSharePercentage;
            this.minimumGuarantee = minimumGuarantee;
            this.agentGroupName = agentGroupName;
            this.totalAmount = totalAmount;
            this.shareAmount = shareAmount;
            this.finalAmount = finalAmount;
            log.LogMethodExit();
        }

        public string RevenueSharePercentage { get { return revenueSharePercentage; } set { revenueSharePercentage = value; } }
        public string Month { get { return month; } set { month = value; } }
        public string Partner { get { return partner; } set { partner = value; } }
        public string MachineGroup { get { return machineGroup; } set { machineGroup = value; } }
        public string AgentGroupName { get { return agentGroupName; } set { agentGroupName = value; } }
        public string ShareAmount { get { return shareAmount; } set { shareAmount = value; } }
        public string TotalAmount { get { return totalAmount; } set { totalAmount = value; } }
        public string FinalAmount { get { return finalAmount; } set { finalAmount = value; } }
        public string MinimumGuarantee { get { return minimumGuarantee; } set { minimumGuarantee = value; } }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public PartnerRevenueDTODefination(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(PartnerRevenueDTODefination))
        {

            attributeDefinitionList.Add(new SimpleAttributeDefinition("Month", "Month", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("Partner", "Partner", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("MachineGroup", "Machine Group / POS Counter", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("AgentGroupName", "Agent Group Name", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("TotalAmount", "Total Amount", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("RevenueSharePercentage", "Revenue Share %", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("ShareAmount", "Share Amount", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("MinimumGuarantee", "Min Guarantee", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("FinalAmount", "Final Amount", new StringValueConverter()));

        }
    }
}
