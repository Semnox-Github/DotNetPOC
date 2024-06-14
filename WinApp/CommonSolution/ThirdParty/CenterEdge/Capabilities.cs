/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - CapabilityDTO class - This would return the capabilities of the system
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Sep-2020       Girish Kundar             Created : CenterEdge  REST API
 ********************************************************************************************/

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    public class CapabilityDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string capSystemName;
        private double capInterfaceVersion;
        private bool capCardCombineToExistingCard;
        private bool capWipeCard;
        private bool capVirtualPlay;
        private PointTypes capPointTypes;
        private CapabilityAdjustments capAdjustments;
        private TimePlay capTimePlays;
        private Privileges capPrivileges;
        private BulkIssues capBulkIssues;

        public CapabilityDTO()
        {
            log.LogMethodEntry();
            capSystemName = string.Empty;
            capInterfaceVersion = 0;
            capPrivileges = new Privileges();
            capBulkIssues = new BulkIssues();
            capPointTypes = new PointTypes();
            capAdjustments = new CapabilityAdjustments();
            capTimePlays = new TimePlay();
            capCardCombineToExistingCard = true;
            capVirtualPlay = true;
            capWipeCard = true;
            log.LogMethodExit();
        }

        public CapabilityDTO( string systemName,double interfaceVersion, PointTypes pointTypeDTO, Privileges privilegesDTO, BulkIssues bulkIssueDTO, 
                              CapabilityAdjustments adjustmentDTO, TimePlay timePlayDTO, 
                              bool cardCombineToExistingCard, bool virtualPlay, bool wipeCard)
        {
            log.LogMethodEntry(systemName, interfaceVersion, cardCombineToExistingCard, virtualPlay, wipeCard, privilegesDTO,
                                 bulkIssueDTO, pointTypeDTO, adjustmentDTO, timePlayDTO);
            this.capSystemName = systemName;
            this.capInterfaceVersion = interfaceVersion;
            this.capCardCombineToExistingCard = cardCombineToExistingCard;
            this.capVirtualPlay = virtualPlay;
            this.capWipeCard = wipeCard;
            this.capPrivileges = privilegesDTO;
            this.capBulkIssues = bulkIssueDTO;
            this.capPointTypes = pointTypeDTO;
            this.capAdjustments = adjustmentDTO;
            this.capTimePlays = timePlayDTO;
            log.LogMethodExit();
        }

        public string systemName { get { return capSystemName; } set { capSystemName = value; } }
        public double interfaceVersion { get { return capInterfaceVersion; } set { capInterfaceVersion = value; } }
        public bool virtualPlay { get { return capVirtualPlay; } set { capVirtualPlay = value; } }
        public bool wipeCard { get { return capWipeCard; } set { capWipeCard = value; } }
        public bool cardCombineToExistingCard { get { return capCardCombineToExistingCard; } set { capCardCombineToExistingCard = value; } }
        public PointTypes pointTypes { get { return capPointTypes; } set { capPointTypes = value; } }
        public CapabilityAdjustments adjustments { get { return capAdjustments; } set { capAdjustments = value; } }
        public Privileges privileges { get { return capPrivileges; } set { capPrivileges = value; } }
        public BulkIssues bulkIssue { get { return capBulkIssues; } set { capBulkIssues = value; } }
        public TimePlay timePlay { get { return capTimePlays; } set { capTimePlays = value; } }
    }


}
