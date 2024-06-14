/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - TimePlayGroups class - This is business layer class for PrivilegeGroup
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Sep-2020       Girish Kundar             Created : CenterEdge  REST API
 ********************************************************************************************/
using System.Collections.Generic;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    public class TimePlayGroups
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int skippedRecords;
        private int totalRecordCount;
        private List<TimePlayGroupDTO> timePlayGroupDTOList;

        public TimePlayGroups()
        {
            log.LogMethodEntry();
            timePlayGroups = new List<TimePlayGroupDTO>();
            skippedRecords = 0;
            totalRecordCount = 1;
            log.LogMethodExit();
        }

        public TimePlayGroups(int skipped, int totalCount)
        {
            log.LogMethodEntry(totalCount, skipped);
            this.skippedRecords = skipped;
            this.totalRecordCount = totalCount;
            log.LogMethodExit();
        }
        public int skipped { get { return skippedRecords; } set { skippedRecords = value; } }
        public int totalCount { get { return totalRecordCount; } set { totalRecordCount = value; } }
        public List<TimePlayGroupDTO> timePlayGroups { get { return timePlayGroupDTOList; } set { timePlayGroupDTOList = value; } }

    }
}
