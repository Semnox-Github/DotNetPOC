/********************************************************************************************
 * Project Name - Generic Utilities
 * Description  - Class to hold the ulc key data.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70.0      04-Sep-2019   Mushahid Faizan     Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// 
    /// </summary>
    public class UlcKeyDTO
    {
        public UlcKeyDTO()
        {
            Key = string.Empty;
            CurrentKey = false;
        }
        public string Key { get; set; }
        public bool CurrentKey { get; set; }

    }
}
