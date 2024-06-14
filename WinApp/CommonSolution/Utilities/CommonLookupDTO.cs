/********************************************************************************************
 * Project Name - LookupMasterDataTableDTO                                                                         
 * Description  - DTO for the Mater Data tables.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.40.1        16-Nov-2018    Jagan Mohana          Created new DTO clas to hold the properties for the Master table.
 ********************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Core.Utilities
{
    public class CommonLookupDTO
    {
        string id;
        string name;
        Dictionary<string, string> values;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public CommonLookupDTO(string id, string name, Dictionary<string, string> values = null)
        {
            log.LogMethodEntry(id,name);
            this.id = id;
            this.name = name;
            this.values = values;
            log.LogMethodExit();
        }
        public string Id { get { return id; } set { id = value; } }
        public string Name { get { return name; } set { name = value; } }
        public Dictionary<string, string> Values { get { return values; } set { values = value; } }
    }

    public class CommonLookupsDTO
    {
        public string DropdownName { get; set; }
        public List<CommonLookupDTO> Items { get; set; }
    }
}
